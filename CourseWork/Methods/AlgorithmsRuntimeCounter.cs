﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CourseWork.Models;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Methods
{
    class AlgorithmsRuntimeCounter
    {
        private Stopwatch timeCounter; //timeCounter;
        Random random = new Random();
        List<long> geneticTime = new List<long>();
        List<long> annealingTime = new List<long>();
        List<long> wolfTime = new List<long>();

        public List<TaskPair[]> countTime()
        {
            List<TaskPair[]> Fxs = new List<TaskPair[]>();
            int i = 1000;               //user-defined number of Individual Tasks to generate
            int geneticBetter = 0;
            int annealBetter = 0;
            int wolfBetter = 0;
            int productsQuantity = 3; //may be randomized or user-defined
            while (i != 0)
            {
                Order generatedOrder = RandomHandler.GetRandomizedOrder(productsQuantity);
                Task task = convertOrderToTask(generatedOrder);
                TaskPair pair1 = executeGeneticAndSaveResults(task);
                TaskPair pair2 = executeAnnealingAndSaveResults(task);
                TaskPair pair3 = executeFrankWolfAndSaveResults(generatedOrder, task);
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
            double annealAvg = annealingTime.Average();
            double geneticAvg = geneticTime.Average();
            double wolfAvg = wolfTime.Average();
            return Fxs;
        }

        private TaskPair executeAnnealingAndSaveResults(Task task)
        {
            SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(task, 200, 10);
            timeCounter = Stopwatch.StartNew();
            TaskPair pair2 = simulatedAnnealing.executeAlgorithm();
            timeCounter.Stop();
            annealingTime.Add(timeCounter.ElapsedTicks);
            return pair2;
        }

        private TaskPair executeGeneticAndSaveResults(Task task)
        {
            Genetic genetic = new Genetic(task, 100);
            timeCounter = Stopwatch.StartNew();
            TaskPair pair1 = genetic.ExecuteAlgorithm();
            timeCounter.Stop();
            geneticTime.Add(timeCounter.ElapsedTicks);
            return pair1;
        }

        //Check if I done everything correct
        private TaskPair executeFrankWolfAndSaveResults(Order order, Task task)
        {
            FrankWolf frankWolf = new FrankWolf();
            timeCounter = Stopwatch.StartNew();
            int[] x_new = frankWolf.FrankWolfMethod(order, order.OrdersProducts);
            timeCounter.Stop();
            wolfTime.Add(timeCounter.ElapsedTicks);
            return new TaskPair(x_new, task.count_fx(x_new));
        }

        private static Task convertOrderToTask(Order order)
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

}