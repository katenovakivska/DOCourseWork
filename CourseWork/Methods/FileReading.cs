using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Methods
{
    public class FileReading
    {
        public int[,] num;
        public int count = 0;
        public void FileRead(string file)
        {
            string path = @"C:\Users\Katya\source\repos\CourseWork\CourseWork\Files\";
            string[] lines = File.ReadAllLines($"{path}{file}");
            num = new int[lines.Length, lines[0].Split(' ').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num[i, j] = Convert.ToInt32(temp[j]);
                
            }
            count = num.GetLength(1);
        }

        public List<string> AllFiles()
        {
            List<string> txtFiles = Directory.GetFiles(@"C:\Users\Katya\source\repos\CourseWork\CourseWork\Files", "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToList();
            return txtFiles;
        }
    }
}
