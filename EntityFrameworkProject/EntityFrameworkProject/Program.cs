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

 */


