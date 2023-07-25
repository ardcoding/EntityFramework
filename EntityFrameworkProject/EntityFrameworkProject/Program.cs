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
                GlobalContext.SaveChanges();
            }
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
                GlobalContext.SaveChanges();
            }
        }

            public static void addCustomer()
        {
            using (var GlobalContext = new GlobalContext())
            {
                Customer customer = new Customer()
                { CustomerId = "C1", CustomerName = "Ahmet", PhoneNumber = "123123", ShoppingScore = 0 };
                GlobalContext.Customers.Add(customer);
                GlobalContext.SaveChanges();
            }
        }
    }
}