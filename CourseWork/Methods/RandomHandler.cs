using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Models;

namespace CourseWork.Methods
{
    public class RandomHandler
    {
        private static Random random = new Random();

        public static Order GetRandomizedOrder(int productsQuantity)
        {
            Order order = GenerateOrder();
            order.OrdersProducts = new List<OrderProduct>();
            for (int i = 0; i < productsQuantity; i++)
            {
                Product product = GenerateProduct();
                OrderProduct orderProduct = GenerateOrderProducts(order, product);
                order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
                order.AllSum = order.AllSum + (orderProduct.Amount * orderProduct.Price);
                order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);
                order.OrdersProducts.Add(orderProduct);
                //orderProducts.Add(orderProduct);
            }

            return order;
        }

        private static Order GenerateOrder()
        {
            //Random random = new Random();
            Order order = new Order();
            const string chars = "0123456789";
            order.Date = DateTime.Now;
            order.Customer = "Customer";
            order.PhoneNumber = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());

            // _context.Orders.Add(order);
            //_context.SaveChanges();

            return order;
        }

        private static Product GenerateProduct()
        {
            //Random random = new Random();
            Product product = new Product();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            product.Name = new string(Enumerable.Repeat(chars, 9).Select(s => s[random.Next(s.Length)]).ToArray());
            product.Description = "Description";
            //_context.Products.Add(product);
            //_context.SaveChanges();

            return product;
        }
        private static OrderProduct GenerateOrderProducts(Order order, Product product)
        {
            //Random random = new Random();

            OrderProduct orderProduct = new OrderProduct();
            orderProduct.Order = order;
            orderProduct.Product = product;
            orderProduct.WriteOffSum = random.Next(10000, 25000);
            orderProduct.Amount = random.Next(2, 8);
            orderProduct.Price = (int)Math.Round((double)orderProduct.WriteOffSum / orderProduct.Amount);
            orderProduct.Sum = (int)Math.Round((double)orderProduct.Amount * orderProduct.Price);
            //_context.OrderProducts.Add(orderProduct);
            //_context.SaveChanges();

            // var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            //order.AllWriteOffSum = order.AllWriteOffSum + orderProduct.WriteOffSum;
            //order.AllSum = order.AllSum + (orderProduct.Amount * orderProduct.Price);
            //order.Disbalance = Math.Abs(order.AllWriteOffSum - order.AllSum);
            //_context.Orders.Update(order);
            //_context.SaveChanges();

            return orderProduct;
        }
    }
}
