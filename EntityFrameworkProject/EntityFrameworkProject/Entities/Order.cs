using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Entities
{
    public class Order //Kolonlar Entity içinde propertylerle oluşturulur. Customer Entitysinin propertyleri aşağıda gördüğümüz gibi CustomerId, CustomerName, ShoppingScore ve PhoneNumber'dır.
                       //Customers Tablomuzun Entitysi tekil yani Customer olarak isimlendirilmiştir.
    {
        public string OrderId   { get; set; }//EF Core ile geliştirme yapıyorsak her tabloda mutlaka bir primary Key olmalıdır. Eğer bu primary keyi tanımlamazsak hata alırız.
                                             //Ancak primary key tanımlamadan da tablo oluşturmanın yolları vardır.
                                             //EF Core Id, ID, <Entityİsmi>Id yani bu örnekte CustomerId yada CustomerID ile tanımlanmış propertyleri default olarak primary key kabul eder.
        public string CustomerId { get; set; }
        public DateTime OrderDate   { get; set; }
        public Customer Customer { get; set; }
    }
}
