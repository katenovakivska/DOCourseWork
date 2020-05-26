using CourseWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Methods
{
    public class AlgorithmsRuntimeCounter
    {
        private Stopwatch timeCounter; //timeCounter;
        Random random = new Random();
        List<long> geneticTime = new List<long>();
        List<long> annealingTime = new List<long>();
        List<long> wolfTime = new List<long>();

        public CounterResult CountTime(int i, int productsQuantity, int leftSum, int rightSum, int leftAmount, int rightAmount)
        {
            List<TaskPair[]> Fxs = new List<TaskPair[]>();
            CounterResult result = new CounterResult();
            //int i = 1000;               //user-defined number of Individual Tasks to generate
            int geneticBetter = 0;
            int annealBetter = 0;
            int wolfBetter = 0;
            //int productsQuantity = 3; //may be randomized or user-defined
            while (i != 0)
            {
                Order generatedOrder = RandomHandler.GetRandomizedOrder(productsQuantity, leftSum, rightSum, leftAmount, rightAmount);
                Task task = ConvertOrderToTask(generatedOrder);
                TaskPair pair1 = ExecuteGeneticAndSaveResults(task);
                TaskPair pair2 = ExecuteAnnealingAndSaveResults(task);
                TaskPair pair3 = ExecuteFrankWolfAndSaveResults(generatedOrder, task);
                TaskPair[] pairs = new TaskPair[3] { pair1, pair2, pair3 };
                //counting situations when one algorithm gives results better than other 
                //(you can remove it or use to compare results)
                if (pair1.Fx != 0 || pair2.Fx != 0 || pair3.Fx != 0)
                {
                    if (pair1.Fx <= pair2.Fx && pair1.Fx <= pair3.Fx)
                        geneticBetter++;
                    if (pair2.Fx <= pair1.Fx && pair2.Fx <= pair3.Fx)
                        annealBetter++;
                    if (pair3.Fx <= pair1.Fx && pair3.Fx <= pair2.Fx)
                        wolfBetter++;
                }
                Fxs.Add(pairs);
                i--;
            }
            result.AnnealAvg = annealingTime.Average();
            result.GeneticAvg = geneticTime.Average();
            result.WolfAvg = wolfTime.Average();
            result.GeneticBetter = geneticBetter;
            result.AnnealBetter = annealBetter;
            result.FrankBetter = wolfBetter;
            return result;
        }

        private TaskPair ExecuteAnnealingAndSaveResults(Task task)
        {
            SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(task, 200, 10);
            timeCounter = Stopwatch.StartNew();
            TaskPair pair2 = simulatedAnnealing.executeAlgorithm();
            timeCounter.Stop();
            annealingTime.Add(timeCounter.ElapsedTicks);
            return pair2;
        }

        private TaskPair ExecuteGeneticAndSaveResults(Task task)
        {
            Genetic genetic = new Genetic(task, 100);
            timeCounter = Stopwatch.StartNew();
            TaskPair pair1 = genetic.ExecuteAlgorithm();
            timeCounter.Stop();
            geneticTime.Add(timeCounter.ElapsedTicks);
            return pair1;
        }

        //Check if I done everything correct
        private TaskPair ExecuteFrankWolfAndSaveResults(Order order, Task task)
        {
            FrankWolf frankWolf = new FrankWolf();
            timeCounter = Stopwatch.StartNew();
            int[] x_new = frankWolf.FrankWolfMethod(order, order.OrdersProducts);
            timeCounter.Stop();
            wolfTime.Add(timeCounter.ElapsedTicks);
            return new TaskPair(x_new, task.count_fx(x_new));
        }

        private static Task ConvertOrderToTask(Order order)
        {
            int orderCount = order.OrdersProducts.Count;
            int[] s = new int[orderCount];
            int[] q = new int[orderCount];
            for (int i = 0; i < orderCount; i++)
            {
                s[i] = order.OrdersProducts.ElementAt(i).Sum;
                q[i] = order.OrdersProducts.ElementAt(i).Amount;
            }
            Task task = new Task(q, s, order.AllWriteOffSum);
            return task;
        }


    }
    public class CounterResult
    {
        public double AnnealAvg { get; set; }
        public double GeneticAvg { get; set; }
        public double WolfAvg { get; set; }
        public double GeneticBetter { get; set; }
        public double AnnealBetter { get; set; }
        public double FrankBetter { get; set; }
    }
    
}
