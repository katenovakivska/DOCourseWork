using CourseWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork
{
    public class InitializeData
    {
        public static void Initialize(OrderContext context)
        {
            if (!context.Products.Any())
            {
                context.Orders.AddRange(
                    new Order
                    {
                        Date = DateTime.Now,
                        Customer = "Rozbiunik Roma",
                        PhoneNumber = "+38096515487",
                        OldAllSum = 0,
                        NewAllSum = 0,
                        AllWriteOffSum = 0,
                        OldDisbalance = 0,
                        NewDisbalance = 0
                    }
                    );
                context.SaveChanges();
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Blender",
                        Description = "a kitchen and laboratory appliance used to mix, purée, or emulsify food and other substances"
                    },
                    new Product
                    {
                        Name = "Coffee maker",
                        Description = "cooking appliances used to brew coffee"
                    }
                );
                context.SaveChanges();
                context.OrderProducts.AddRange(
                    new OrderProduct
                    {
                        OrderId = 1,
                        ProductId = 1,
                        Amount=5,
                        OldSum = 0,
                        WriteOffSum = 0,
                        OldPrice = 0,
                        NewPrice = 0,
                        NewSum = 0
                    },

                     new OrderProduct
                     {
                         OrderId = 1,
                         ProductId = 2,
                         Amount = 3,
                         OldSum = 0,
                         WriteOffSum = 0,
                         OldPrice = 0,
                         NewPrice = 0,
                         NewSum = 0
                     }
                    );
            }
        }
    }
}
