using System;
using EntityFrameworkProject.Context;
using EntityFrameworkProject.Entities;

namespace EntityFrameworkProject
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var GlobalContext = new GlobalContext())
            {
                var result = GlobalContext.Customers.ToList();
                foreach (var customer in result)
                {
                    Console.WriteLine("{0}: {1}",customer.CustomerId,customer.CustomerName);
                }
            }
        }

        public static void DeleteOrder()
        {
            using (var GlobalContext = new GlobalContext())
            {
                Customer customer = GlobalContext.Customers.Find("C1");
                GlobalContext.Customers.Remove(customer);
            }
        }

        public static void UpdateOrder()
        {
            using (var GlobalContext = new GlobalContext())
            {
                Customer customer = GlobalContext.Customers.Find("C1");
                customer.CustomerName = "Rüchan"
                GlobalContext.SaveChanges();//Delete Insert ve Update sorgularını oluşturup bir tx (transaction) ile veritabanında execute eder.
            }                              //Eğer bir hata olursa tüm işlemleri geri alır.
        }

        public static void addOrder()
        {
            using (var GlobalContext = new GlobalContext())
            {
                Customer customer = GlobalContext.Customers.Find("C1");
                customer.Orders.Add(new Order
                {
                    OrderId = "O1",
                    OrderDate = DateTime.Now,
                });
                GlobalContext.SaveChanges();//Delete Insert ve Update sorgularını oluşturup bir tx (transaction) ile veritabanında execute eder.
            }                              //Eğer bir hata olursa tüm işlemleri geri alır.
        }

            public static void addCustomer()
        {
            using (var GlobalContext = new GlobalContext())
            {
                Customer customer = new Customer()
                { CustomerId = "C1", CustomerName = "Ahmet", PhoneNumber = "123123", ShoppingScore = 0 };
                GlobalContext.Customers.Add(customer); //Customers kullanımı tip güvenliğini sağlar.
                GlobalContext.SaveChanges(); //Delete Insert ve Update sorgularını oluşturup bir tx (transaction) ile veritabanında execute eder.
            }                               //Eğer bir hata olursa tüm işlemleri geri alır.
        }
    }
}



/*
                            Database First Yaklaşımı

Bu yaklaşım kullanılıyorsa reverse engineering yapılıyordur.
Package Manager Console veya DotNet CLI ile bu yaklaşım yürütülebilir.
Oluşturulmuş bir veri tabanından Entityler ve Context otomatik olarak oluşturulur.
Eğer zaten oluşturulmuş bir tabloyu güncellemek istiyorsak --force ile üzerine yazmak için zorlarız.
Ancak bu durum sıkıntıya yol açacağı için partial class kullanılır. Böylelikle yapılan özelleştirmeler kaybolmaz.


--------------------------------------------------------------------------------------------------------------------


                            Code First Yaklaşımı

Bu projede code first yaklaşımını kullandık oluşturmuş olduğumuz entityler ve contexte göre veri tabanı otomatik olarak oluşturulmuştur.
Bu süreç Database firstte olduğu gibi yine Package Manager Console ve DotNetCLI ile yürütülebilir.
Migration: Veri tabanının anlayacağı hale getiren C# classıdır.
Migrate ise oluşturulan Migration'ı veri tabanına iletme kısmıdır.

Migration Kodu:
AppDBContext context = new();
await context.Database.MigrateAsync();


                   Veri Kalıcılığı Veri Ekleme, Silme ve Güncelleme Notlar

context.Entry(customer).State -> verinin durumunu gösterir eğer oluşturulup bir işlem yapılmadıysa Detached, eklenmişse added, savechange'den sonra çağrılırsa unchanged çıktısı verir.

    Birden Fazla Veri Eklemek:
Farklı tx'ler oluşturmak maliyeti arttırır. Her tx = maliyettir. Bu yüzden birden fazla veri eklemek tek txte yapılmaya çalışılır.
Bunun yerine AddRangeAsync(product1, product2) şeklinde kullanılabilir.





                        Veri Sorgulama

   Method Syntax

var customers = await GlobalContext.Customers.TolistAsync();

    Query Syntax

var customers = await (from customer in GlobalContext.Urunler select urun).ToListAsync();


                    Sorguları Execute Etmek

    IQueryable

Sorguya karşılık gelir. EFCore üzerinden yapılmış olan sorgunun execute edilmemiş halidir.

    IEnumerable

Çalıştırılıp/execute edilip verilerin memorye yüklenmiş halidir.

Kısacası IQueryable sorgulanan verilerin execute edilmeden önceki hali IEnumerable ise execute edildikten sonraki halidir.

Execute etmek için ToListyAsync(); fonksiyonu kullanılır. 

                    Deferred Execution

IQueryable çalışmalarında kod yazıldığı noktada değil execute edildiği noktada tetiklenir ve kullanılacak değişkenlerin son değeri baz alınır.
Yani önemli olan sorgunun yapıldığı nokta değil execute edildiği noktadır.

Örnek
string name = "Ahmet";
var customers = from customer in GlobalContext.Urunler 
                    where customer.CustomerName === name
                    select urun;
string name = "Rüchan";

foreach(Customer customer in customers){
 Console.WriteLine(customer.CustomerName);
}

Bu kodda CustomerName i Rüchan olanlar gelecektir çünkü sorgu oluşturulduğu anda execute edilmemiş execute edildiği an foreach döngüsünün olduğu yerdir.
Dolayısıyla foreach döngüsünün olduğu kısımda name değişkenini son duruumu Rüchan olduğu için sorguda Rüchan olanlar aranacaktır.


                    Where Fonksiyonu

Şart eklemek için kullanılır.

await GlobalContext.Customers.where(u => u.id>1).ToListAsync();

                    OrderBy Fonksiyonu

Sıralama için kullanılır default olarak ascending sıralama yapar descending sıralama yapmak için OrderByDescending() kullanılır.

await GlobalContext.Customers.where(u => u.id>1).OrderBy(u=>u.CustomerName).ToListAsync();
await GlobalContext.Customers.where(u => u.id>1).OrderByDescending(u=>u.CustomerName).ToListAsync();

                    ThenBy Fonksiyonu 

OrderBy ile yapılan sıralama işlemini farklı kolonlarda uygulamayı sağlayan fonksiyondur. Default olarak ascending sıralama yapar.
Descending yapmak için ThenByDescending() kullanılmalıdır.

await GlobalContext.Customers.where(u => u.id>1).OrderBy(u=>u.CustomerName).ThenBy(u=>u.id).ToListAsync();
await GlobalContext.Customers.where(u => u.id>1).OrderBy(u=>u.CustomerName).ThenByDescending(u=>u.id).ToListAsync();



        Tekil Veri Getiren Sorgu Fonksiyonları

SingleAsync

Sorgu neticisinde birden fazla veri geliyorsa yada hiç veri gelmiyorsa exception fırlatır.
var customer = await GlobalContext.Customers.SinglAsync(u => u.CustomerName == "Ahnet");

SingleOrDefaultAsync
Sorgu neticesinde birden fazla veri geliyorsa exception fırlatır. Eğer hiç veri gelmiyorsa null döndürür.


Yapılan sorguda tek bir verinin gelmesi isteniyorsa şu fonksiyonlar kullanılır: FirstAsync, FirstOrDefautAsync

FirstAsync
Sorgu sonucunda elde edilen verilerin ilkini getirir. Eğer veri gelmiyorsa hata verir.

FirstOrDefaultAsync
Sorgu sonucunda elde edilen verilerin ilkini getirir. Eğer veri gelmiyorsa null döndürür.

FindAsync
Primary key kolonuna hızlı bir sorgulama yapma fırsatı verir. Yalnızca primary key içindir. Kayıt yoksa null döndürür.
GlobalContext.Customers.Find("C1"); primary key için direkt bu şekilde değer verebiliriz.

LastAsync
Bu fonksiyonu kullanabilmek için mutlaka OrderBy fonksiyonu kullanılmalıdır.
Sorgu neticisinde gelen verilerden en sonuncusunu getirir. Veri gelmiyorsa hata verir.

LastOrDefaultAsync
Bu fonksiyonu kullanabilmek için mutlaka OrderBy fonksiyonu kullanılmalıdır.
Sorgu neticisinde gelen verilerden en sonuncusunu getirir. Veri gelmiyorsa null döndürür.
 */


