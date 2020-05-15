//using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Models;

namespace CourseWork.Methods
{
    public class FileReading
    {
        public int[,] num;
        public int count = 0;
        public Order FileRead(string file)
        {
            Order order = new Order();
            order.OrdersProducts = new List<OrderProduct>();
            string path = @".\Files\";
            string[] lines = File.ReadAllLines($"{path}{file}");
            num = new int[lines[0].Split(';').Length-1, lines.Length-2];
            for (int i = 1; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(';');
                if(i != lines.Length - 1)
                {
                    for (int j = 1; j < temp.Length; j++)
                        num[j-1,i-1] = Convert.ToInt32(temp[j]);
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.Product = new Product();
                    orderProduct.Product.Name = temp[0];
                    orderProduct.Amount = Convert.ToInt32(temp[1]);
                    orderProduct.Sum = Convert.ToInt32(temp[2]);
                    order.OrdersProducts.Add(orderProduct);
                }
                else
                {
                    order.AllSum = Convert.ToInt32(temp[temp.Length-1]);
                }
            }
            count = num.GetLength(1);
            return order;
        }

        public List<string> AllFiles()
        {
            List<string> csvFiles = Directory.GetFiles(@".\Files\", "*.csv")
                                     .Select(Path.GetFileName)
                                     .ToList();
            return csvFiles;
        }


    }
}
