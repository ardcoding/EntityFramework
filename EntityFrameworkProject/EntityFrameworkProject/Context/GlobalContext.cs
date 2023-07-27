<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkProject.Entities;

namespace EntityFrameworkProject.Context
{
    public class GlobalContext:DbContext //DbContext, veri tabanını temsil eder. Sorumlulukları: Konfigürasyon, Sorgulama, Change Trading* , Veri Kalıcılığı (CUD)** , Caching
    {
        public DbSet<Customer> Customers { get; set; } //Her bir tablo nesnesine Entity denir. Entity tabloları temsil eden sınfılardır.
        public DbSet<Order> Orders { get; set; }  //Tablo isimleri çoğul isimlendirilirken entityler tekil isimlendirilir.
    }
}

/*
 
* Change Trading: Sorgulama sonucu elde edilen veriler üzerindeki değişiklikleri takip eder.
** Veri Kalıcılığı: (Create, Update, Delete) işlemleriyle ilgilidir.

    OnConfiguring fonksiyonu kongigürasyon ayarlarını gerçekleştirir, 
    EF Core Toolu yapılandırmak için kullanılır.
    Context nesnesinde override edilerek kullanılır.
    Bu fonksiyon kullanılan yerde bir yapılandırma amacı vardır.
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkProject.Entities;

namespace EntityFrameworkProject.Context
{
    public class GlobalContext:DbContext //DbContext, veri tabanını temsil eder. Sorumlulukları: Konfigürasyon, Sorgulama, Change Trading* , Veri Kalıcılığı (CUD)** , Caching
    {
        public DbSet<Customer> Customers { get; set; } //Her bir tablo nesnesine Entity denir. Entity tabloları temsil eden sınfılardır.
        public DbSet<Order> Orders { get; set; }  //Tablo isimleri çoğul isimlendirilirken entityler tekil isimlendirilir.
    }
}

/*
 
* Change Trading: Sorgulama sonucu elde edilen veriler üzerindeki değişiklikleri takip eder.
** Veri Kalıcılığı: (Create, Update, Delete) işlemleriyle ilgilidir.

    OnConfiguring fonksiyonu kongigürasyon ayarlarını gerçekleştirir, 
    EF Core Toolu yapılandırmak için kullanılır.
    Context nesnesinde override edilerek kullanılır.
    Bu fonksiyon kullanılan yerde bir yapılandırma amacı vardır.
>>>>>>> 1cfac191f8af0a258d2baf5ee92395b9c10c73c6
 */