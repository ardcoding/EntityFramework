<<<<<<< HEAD
﻿using System;
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

CountAsync
Sorgunun execute edilmesiyle gelen satırların sayısını integer olarak döndürü.
Sorguyu execute ettikten sonra çalıştırmak daha fazla maaliyete sebep olacaktır. Bunun yerine sorgu anında kullanmak daha iyidir.
var customers = await GlobalContext.Customers.CountAsync();

LongCountAsync 
Aslında CountAsync fonksiyonuyla aynı işi yapar. Ancak geri dönüş değeri olarak integer yerine long değeri döndürür.
Büyük veri kullanan projeler için kullanılır.

Bu 2 fonksiyonda şart eklenebilir 
var customers = await GlobalContext.Customers.CountLongAsync( u => u.id == 1);

AnyAsync
Sorgu sonucunda verinin gelip gelmediğini kontrol eder. Bool değer döndürür.
var customers = await GlobalContext.Customers.AnyAsync( u=> u.id == 3);

MaxAsync - MinAsync
Oluşturulan sorguldan belirtilen kolonda değeri en yüksek (maxasync) yada en düşük (minasync) olan değeri döndürür.
var customers = await GlobalContext.Customers.MaxAsync(u=>u.id);

Distinct
Tekrar eden kolonları tekrar tekrar göstermek yerine sadece bir kez gösterir.
var customers = await GlobalContext.Custpmers.Distinct().ToListAsync(); 2 kez ahmet olan tabloda bir kez ahmet getirir.

AllAsync
Gelen verilerin verilen şarta uyup uymadığını kontrol eder. Bool değer döndürür.
var customers = GlobalContext.AllAsync(u=>u.id > 0 );

Sum
Verilen kolondaki verilerin toplamını döndürür.

Average
Verilen kolondaki verilerin aritmetik ortalamasını döndürür.

Contains - StartsWith - EndsWith
Like sorgusu gibi çalışır.
Verilen stringi içeren (contains) yada verilen string eğer o verinin başında (startswith) veya sonunda (endswith) olan verileri döndürür.

ToDictionary
Gelen verilerin Dictionary formatta kullanmak için bu fonksiyon kullanılır.
await GlobalContext.Customers.where(u => u.id>1).ToDictionaryAsync();

ToArray
Gelen verilerin Array formatta kullanmak için bu fonksiyon kullanılır.
await GlobalContext.Customers.where(u => u.id>1).ToArrayAsync();

Select
Görmek istediğimiz kolonları seçebilmek için kullanılır.
2. olarak Gelen verileri farklı türde karşılamayı sağlar.
await GlobalContext.Customers.Select(u =>  new Customer{
Name = u.Name
}).ToListAsync();

SelectMany (INNER JOIN)
Select ile benzerdir. İlişkisel tablolardan gelen koleksiyonel verileri tekilleştirip kullanmamızı sağlar.
await GlobalContext.Customers.Select(u => ı.Parcalar (u,p) => new{
u.Name,
p.Id
})

GroupBy
Gruplandırma yapmak için kullanacağımız fonksiyondur.
var datas = context.Urunler.GroupBy(u => u.Fiyat).Select(group => new{
Count = group.Count(),
Fiyat = group.Key //groupbyda çektiğimiz kolonu temsil ediyor
}).ToListAsync();
Bu kodda ürünün fiyatlarını gruplandırır ve hangi fiyattan kaç adet ürün olduğunu gösterir.

ForEach Fonksiyonu
Foreach döngüsünün fonksiyon halidir.
Sorgulama sonucu gelen verilerin üzerinde iterasyonel olarak dönmemizi ve işlemler yapmamızı sağlar.
datas.ForEach(items => {
    items.Name
})



                                        Change Tracker

Context nesnesinden gelen verileri takip eden mekanizmadır. Nesne üzerindeki değişiklikleri takip eder.

    Change Tracker Property

Takip edilen nesnelere erişebilmeyi sağlayan ve gerekirse işlem yapmayı sağlayan propertydir.
var customers = await GlobalContext.Customers.ToListAsync();
customers[1].Name = "AHMET"
var datas = GlobalContext.ChangeTracker.Entries();
await GlobalContext.SaveChangesAsync();

    Detect Changes

Yapılan değişikliklerin veri tabanına göndermeden kontrol etmek gerekir. SaveChanges bunu otomatik olarak yapar.
Değişikliklerin algılanmasını opsiyonel olarak gerçekleştirmek için DetectChanges kullanılır.
var customer = await GlobalContext.Customers.FirstOrDefaultAsync(u=>u.id == 2);
customer.Name = "A"
GlobalContext.ChangeTracker.DetectChanges();
await GlobalContext.SaveChangesAsync();

    AutoDetectChanges
SaveChanges fonksiyonunun yaptığı otomatik kontrolü kapatıp kendimiz kontrol edebiliriz. Optimizasyon amacıyla yapılır

    Entries Metodu
Contextneki entrynin koleksiyonek versiyonudur. Her entity bilgisini EntityEntry türünden elde edebiliriz.
DetectChanges methodunu tetikler.
Buradaki maliyetten kaçınmak için AutoDetectChangesEnabled özelliğine false değeri verilebilir.

    AcceptAllChanges
SaveChanges tetiklendiğinde EF Core her şeyin yolunda olduğunu varsayar ve takibi keser.
Beklenmeyen bir hata olursa nesnelerin takipi bırakıldığı için düzeltme yapılamaz. 
Bu durumda SaveChanges(false) AcceptAllChanges kullanılır.
AcceptAllChanges ile takibi manuel olarak keseriz.

    HasChanges
Nesneler arasında değişikliği kontrol eder.
DetectChangesi tetikler.



                                    Entity States

Entity nesnelerinin durumunu ifade eder:
Detached => Nesnenin takip edilmediğini ifade eder.
Added => Veri tabanına verinin ekleneceğini ifade eder.
Modified => Veri tabanındaki verinin güncelleneceğini ifade eder.
Deleted => Veri tabanındaki verinin silineceğini ifade eder.
Unchanged => Takip edilen nesnenin herhangi bir işleme uğramadığını ifade eder.


                                    Context Nesnesi Üzerinden Change Tracker

GlobalContext.Entry(customer).OriginalValues.GetValue<int>(nameof(Customer.Id)); Güncellenecek nesnenin orjinal halini getirir.
GlobalContext.Entry(customer).CurrentlValues.GetValue<int>(nameof(Customer.Id)); Nesnenin anlık değerini gösterir.
GlobalContext.Entry(customer).OriginalValues.GetDatabaseValues<int>(nameof(Customer.Id)); Nesnenin veri tabanındaki değerini gösterir.



                                   Change Trackerin Interceptor  Olarak Kullanılması

public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    var entries = ChangeTracker.Entries);
    foreach (var entry in entries){
        1f (entry.State == EntityState.Added){
        
        }
    }
    return base. SaveChangesAsync(cancellationToken);
}

Veri ekleme, güncelleme gibi işlemleri veri tabanına gönderilmeden önce yapmak istediklerimizi yapabilmeyi sağlar.

-> 27.07.2023 Perşembe



-> 28.07.2023 Cuma
                                                     AsNoTracking

ChangeTracker mekanizmasının her nesneyi takip etmesi bir maliyettir. Çok fazla veriyle çalışırken changetracker her bir nesne için ayrı ayrı takip eder.
Bu da nesne arttıkça maliyeti arttırır. İşlem yapılmayacak verilerin takip edilmesi bize maliyet olarak geri dönecektir.
AsNoTracking metodu sorgu neticesinde gelecek olan verilerin takip edilmesini engeller. Maliyetten kazanç elde etmiş oluruz

await GlobalContext.Users.AsNoTracking().ToListAsync();


    AsNoTrackingWithIdentityResolution

ChangeTracker sayesinde yinelenen veriler aynı  instanceları kullanır ancak AsNoTracking metodu ile yapıları sorgularda yinelenen veriler farklı instancelarda karşılanırlar.
Tekil instance kullanmayı istiyorsak bu fonksiyonu kullanmalıyız.
Doğru kullanılmazsa maliyeti düşürmek için kullandığımız bu fonksiyon daha fazla maliyete sebep olabilir.
İlişkisel verilerle çalışırken dikkat etmemiz gerekir aksi takdirde aynı nesne için ayrı ayrı instance oluşturur. Bu da maliyete neden olur.

await GlobalContext.Customers.Include(c => c.Name).AsNoTrackingWithIdentityResolution().ToListAsync();,
Maliyet bakımından:
ChangeTracker > AsNoTrackingOdentityResolution > AsNoTracking

    AsTracking

Tracking mekanizmasını takip etmemizi sağlar. 
Zaten default olarak takip ederken bu fonksiyonu neden kullanıyoruz? 
+ ChangeTracker mekanizmasını pasife çektiğimiz zaman tekrar devreye sokmak için kullanılır.

await GlobalContext.Customers.AsTracking().ToListAsync();

    UseQueryTrackingBehavior

Configurasyon fonksiyonudur.
EF Core seviyesinde ilgili contextten gelen verilerin üzerinde ChangeTracker mekanizmasının davranışını temel seviyede belirlememizi sağlar.

optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)

                                İlişkisel Yapılar
    Temel Kavramlar
    
    Principal Entity
Kendi başına var olabilen entity'dir.
Bir veri tabanında depertman kendi başına var olabilir bunu örnek verebiliriz.

    Dependent Entity
Kendi başına var olamayan entity'dir.
Çalışan entitysi departman entitysine bağlı olarak var olabilir bu yüzden çalışan entitysi dependent entitydir.

    Foreign Key
Principal Entity ve Dependent Entity'i bağlayan keydir. Dependent Entityde tanımlanır. Principal keye karşılık gelir.

    Principal Key
Principal Entitydeki id'dir.

    Navigation Property
İlişkisel tablolar arasındaki fiziksel erişimi entity classları üzerinden sağlayan properylerdir.
Bir propertynin navigation property olabilmesi için kesinlikle entity türünden olmalıdır.
İlişki türlerini ifade ede. (many to many, one to many)


class Customer { Principal entitydir. Var olabilmesi için başka entitye ihtiyacı yoktur
    public int Id { get; set; }, principal key
    public string Name { get; set; },
    public string Address { get; set; },
    public ICollection<Order> Orders { get; set; } navigation property
}

class Order{ Order dependent entitydir orderın var olabilmesi customera bağlıdır.
    public int Id { get; set; },
    public string Address { get; set; },
    public float Amount { get; set; },
    public int CustomerId { get; set; } foreign key
    public Customer Customer { get; set; } navigation property
}
    
    Default Conventions 
Varsayılan entity kurallarını kullanarak yapılan ilişki yapılandırma yöntemidir.
Navigation propertyleri kullanarak ilişki şablonlarını çıkartmaktadır.

    Data Annotations Attributes
Entitynin özelliklerine göre ayarlama yapmamızı sağlayan attributelardır. (key, foreign key)

    Fluent API
Entity modellerindeki ilişkileri yapılandırırken daha detaylı çalışmamızı sağlar.

HasOne
İlgili entitynin ilişkisel entitye birebir yada bire çok olacak şekilde ilişkisini yapılandırır.

HasMany
İlgili entitynin ilişkisel entitye çoka bir yada çoka çok olacak şekilde ilişkisini yapılandırır.

WithOne
HasOne yada HasMany den sonra birebir yada çoka bir olacak şekilde ilişki yapılandırmasını sağlar

WithMany
HasOne yada HasMany den sonra bire çok yada çoka çok olacak şekilde ilişki yapılandırmasını sağlar

-> 28.07.2023 Cuma

-> 31.07.2023 Pazartesi

                        Backing Fields

Tablo içerisindeki kolonları entity classları içerisinde propertyler ile değil fieldlarla temsil etmemizi sağlar.

class Person
{
    public int Id { get; set; }
    public string Name { get => name.Substring(0, 3); set => name = value.Substring(0, 3); }
}
verileri eklerken yada getirirken kapsülleme de yapabiliriz bu şekilde


    Backing Filed Attributes

class Person
{
    public int Id { get; set; }
    [BackingField(nameof(name))]
    public string Name { get; set; }
}


    FluentAPI
HasField metodu BackingFielda karşılık gelir.

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Person>()
                .Property(p=p.Name)
                .HasField(nameof(Person.name));
}

Field-Only-Properties
Entitiylerde değerleri almak için propertyler yerine metotların kullanıldığı yada belirli alanların hiç gösterilmemesi gerektiği durumlarda
kullanılabilir.

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Person>()
                .Property(nameof(Person.name));
}


                    Shadow Properties

Gösterilmesini istemediğimiz kolonları modellerken propertylerle göstermek istemeyiz.(örnek created date)
Bu tip properylere shadow property denir. Yani Entity sınıflarında tanımlanmayan ancak EFCore tarafından var olarak kabul edilen propertylerdir.
ChangeTracker Shadow propertyleri de takip eder.

Foreign Key 

Aslında foreign keyler de shadow propertydir. 2 Entity arasında ilişkiyi kurduktan sonra EFCoreun otomatik olarak foreign key oluşturuyor.
Bunu entityde göstermesek de bu property oluşturulmuş olur ve yeri geldiği zaman kullanılabilir.

Shadow Property Oluşturma

FluentAPI ile oluşturulur.

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Person>()
                .Property<DateTime>("CreatedTime");

    base.OnModelCreating(modelBuilder);
}

Shadow Propertye Erişim Sağlama

var person = await context.Persons.FirstAsync();
var date = context.Entry(person).Property("Creeateddate");
Console.WriteLine(data.CurrentValue);
data.CurrentValue = DateTime.Now;
await context.SaveChangesAsync();

EF.Property İle Erişim Sağlama

Özellikle LINQ sorgularında Shadow Propertylerine erişim için EF.Property static yapılanmasın kullanılır.

var person = await context.Person.OrderBy(p => EF.Property<DateTime>(p, "CreatedDate")).ToListAsync();

-> 31.07.2023 Pazartesi
 */


