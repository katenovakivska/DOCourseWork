using CourseWork.Methods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CourseWork.Methods
{
    class SimulatedAnnealing
    {
        private Task IndividualTask;
        private int n;
        private int H;
        private Random rand = new Random();
        private int startTemperature;

        public SimulatedAnnealing(Task individualTask, int temperature, int h)
        {
            IndividualTask = individualTask;
            n = IndividualTask.X_start.Length;
            H = h;
            startTemperature = temperature;
        }

        public TaskPair executeAlgorithm()
        {
            int temperature = startTemperature;
            int h = 0;
            float epsilon = 0.01F;
            int[] x_current = new int[n];
            Array.Copy(IndividualTask.X_start, x_current, n);
            TaskPair bestSolution = new TaskPair(x_current, IndividualTask.count_fx(x_current));
            TaskPair currentSolution = bestSolution;
            while (h <= H && bestSolution.Fx != 0 && temperature != 0)
            {
                IList<TaskPair> z = new List<TaskPair>();
                while (bestSolution.Fx != 0 && !balanceIsReached(z, currentSolution, epsilon))
                {
                    z.Add(currentSolution);
                    TaskPair Y = generateNewPoint(x_current);
                    if (Y.Fx == 0)
                    {
                        bestSolution = Y;
                        break;
                    }
                    long delta = Y.Fx - currentSolution.Fx;
                    double p = Math.Min(1, Math.Pow(Math.E, (-delta / temperature)));
                    int minKsi = 75 + (75 * (temperature - startTemperature) / startTemperature);
                    int maxKsi = 100 * temperature / startTemperature;
                    double ksi = Convert.ToDouble(rand.Next(minKsi, maxKsi + 1))/100;
                    if (p > ksi || delta < 0)
                    {
                        currentSolution = Y;
                        if (Y.Fx < bestSolution.Fx)
                            bestSolution = Y;
                    }
                }
                h++;
                double alpha = Convert.ToDouble(rand.Next(50, 100)) / 100;
                temperature = Convert.ToInt32(temperature * alpha);
            }
            return bestSolution;
        }

        private TaskPair generateNewPoint(int[] x_current)
        {
            const int boundary = 3;
            int[] x_new = new int[n];
            for (int i = 0; i < n; i++)
              {
                    x_new[i] = rand
                        .Next(Math.Max(x_current[i] - boundary, IndividualTask.Constraints[i][0]),
                        Math.Min(x_current[i] + boundary, IndividualTask.Constraints[i][1]) + 1);
              }
            return new TaskPair(x_new, IndividualTask.count_fx(x_new));
        }

        private bool balanceIsReached(IList<TaskPair> points, TaskPair currentPoint, float epsilon)
        {
            IList<double> Ratio = new List<double>();
            foreach(TaskPair point in points)
            {
                if(point != currentPoint)
                    Ratio.Add(Convert.ToDouble(Math.Abs(point.Fx - currentPoint.Fx)) / point.Fx);
            }
            return (Ratio.Count != 0)?(Ratio.Min() <= epsilon):false;
        }
    }
}