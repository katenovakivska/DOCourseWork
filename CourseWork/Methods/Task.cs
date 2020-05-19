using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Methods
{
    class Task
    {
        public readonly int[] Q;
        public readonly int[] X_start;
        public readonly int S;
        public readonly int[][] Constraints;

        //q_and_s_vectors is 2-dimensional array, where first row is q_vector, second row is s_vector
        public Task(int[] q_vector, int[] s_vector,  int s)
        {
            int n = s_vector.Length;
            int[] x = new int[n];
            int[][] constraints = new int[n][];
            for (int i = 0; i < n; i++)
            {
                constraints[i] = new int[2];
                double varbl = Convert.ToDouble(s_vector[i]) / q_vector[i];
                x[i] = Convert.ToInt32(varbl);
                double value1 = x[i] * 0.99;
                constraints[i][0] = Convert.ToInt32(value1);
                double value2 = x[i] * 1.01;
                constraints[i][1] = Convert.ToInt32(value2);
            }
            Q = q_vector;
            X_start = x;
            S = s;
            Constraints = constraints;
        }


        public long count_fx(int[] x_point)
        {
            long fx = 0l;
            for(int i = 0; i < x_point.Length; i++)
            {
                fx += Q[i] * x_point[i];
            }
            fx -= S;
            return Math.Abs(fx);
        }

    }

    class TaskPair : IComparable
    {
        public int[] X;
        public long Fx;

        public TaskPair(int[] x, long fx)
        {
            X = x;
            Fx = fx;
        }

        public TaskPair()
        {

        }

        public int CompareTo(Object obj)
        {
            return Convert.ToInt32(Fx - ((TaskPair)obj).Fx);
        }

        public static void Shuffle(IList<TaskPair> ts)
        {
            Random rand = new Random();
            var count = ts.Count;
            for (var i = 0; i < count - 1; ++i)
            {
                var r = rand.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}
