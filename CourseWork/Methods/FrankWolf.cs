using CourseWork.Models;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Methods
{
    public class FrankWolf
    {
            public int[] FrankWolfMethod(Order order, IEnumerable<OrderProduct> orderProducts)
            {
                int count = orderProducts.Count();
                int[] x0 = new int[count];
                int[] fx = new int[count + 1];
                int[] gradient = new int[count];
                int[,] simplexTable = new int[2 * count + 1, 3 * count + 1];
                fx[count] = order.AllWriteOffSum;
                int[] y = new int[count];
                int[] bp = new int[2 * count];
                int[,] xAlpha = new int[count, 2];
                int[] fAlpha = new int[2];
                int[] fAlphaMarch = new int[2];
                int[] xNew = new int[count];
                double alpha;
                List<string> grad = new List<string>();
                int i = 0, fx0 = 0;
                int fxNew = 0;
                foreach (var op in orderProducts)
                {
                    x0[i] = op.Price;
                    fx[i] = op.Amount;
                    i++;
                }
                for (int j = 0; j < count + 1; j++)
                {
                    if (j != count)
                    {
                        fx0 = fx0 + x0[j] * fx[j];
                    }
                    else
                    {
                        fx0 = fx0 - fx[j];
                        fx0 = (int)Math.Pow(fx0, 2);
                    }
                }
                gradient = Gradient(gradient, count, x0, fx);
          
                simplexTable = SimplexTable(simplexTable, count, gradient, x0);
                for (int k = 0; k < 2 * count; k++)
                {
                    bp[k] = count + k;
                }
            if (order.AllWriteOffSum - order.AllSum > 0)
            {
                simplexTable = MinSimplexMethod(simplexTable, count, bp);
            }
            else if (order.AllWriteOffSum - order.AllSum < 0)
            {
                simplexTable = MaxSimplexMethod(simplexTable, count, bp);
            }

                for (int j = 1; j < 2 * count + 1; j++)
                {
                    for (int k = 0; k < count; k++)
                    {
                        if (bp[j - 1] == k)
                        {
                            y[k] = simplexTable[j, 3 * count];
                        }
                    }
                }

                for (int j = 0; j < count; j++)
                {
                    xAlpha[j, 0] = x0[j];
                    xAlpha[j, 1] = y[j] - x0[j];
                }

                for (int j = 0; j < count; j++)
                {
                    fAlpha[1] += xAlpha[j, 1] * fx[j];
                    fAlpha[0] += xAlpha[j, 0] * fx[j];
                }
                fAlpha[0] -= fx[count];
                fAlphaMarch[0] = 2 * (int)Math.Pow(fAlpha[1], 2);
                fAlphaMarch[1] = 2 * fAlpha[1] * fAlpha[0];
          
                alpha = Math.Round((double)-fAlphaMarch[1] / fAlphaMarch[0], 4);
            if (Double.IsInfinity(alpha))
            {
                alpha = 0;
            }
                
                for (int j = 0; j < count; j++)
                {
                   if (y[j] == 0)
                   {
                      xNew[j] = x0[j];
                   }
                   else
                   {
                      xNew[j] = xAlpha[j, 0] + (int)Math.Round(alpha * xAlpha[j, 1]);
                   }
                    
                }
                for (int j = 0; j < count + 1; j++)
                {
                    if (j != count)
                    {
                        fxNew = fxNew + (int)Math.Round((double)xNew[j]) * fx[j];
                    }
                    else
                    {
                        fxNew = fxNew - fx[j];
                        fxNew = (int)Math.Pow(fxNew, 2);
                    }
                }

                return xNew;
            }

            public int[] Gradient(int[] gradient, int count, int[] x0, int[] fx)
            {

                for (int j = 0; j < count; j++)
                {
                    for (int k = 0; k < count; k++)
                    {
                        gradient[j] = gradient[j] + 2 * fx[j] * fx[k] * x0[k];
                    }
                    gradient[j] = gradient[j] - 2 * fx[j] * fx[count];
                }

                return gradient;
            }
            public int[,] SimplexTable(int[,] simplexTable, int count, int[] gradient, int[] x0)
            {
                for (int i = 0; i < 2 * count + 1; i++)
                {
                    for (int j = 0; j < 3 * count + 1; j++)
                    {
                        simplexTable[i, j] = 0;
                    }
                }
                for (int i = 0; i < 2 * count + 1; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < 3 * count + 1; j++)
                        {
                            if (j < count)
                                simplexTable[i, j] = -gradient[j];
                            else
                                simplexTable[i, j] = 0;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (i == 2 * (j + 1) || i == (2 * (j + 1) - 1))
                                simplexTable[i, j] = 1;
                            else
                                simplexTable[i, j] = 0;
                        }
                        for (int j = count; j < 3 * count + 1; j++)
                        {
                            if (i == j - (count - 1) && j != 3 * count)
                            {
                                if (i % 2 != 0)
                                {
                                    simplexTable[i, j] = 1;
                                }
                                else if (i % 2 == 0)
                                {
                                    simplexTable[i, j] = -1;
                                }
                                else
                                    simplexTable[i, j] = 0;

                            }
                            else
                                simplexTable[i, j] = 0;
                        }
                    }

                }

                for (int i = 0; i < count; i++)
                {
                    simplexTable[2 * (i + 1) - 1, 3 * count] = (int)Math.Round(1.01 * x0[i]);
                    simplexTable[2 * (i + 1), 3 * count] = (int)Math.Round(0.99 * x0[i]);
                }
                return simplexTable;

            }
            public int[,] MinSimplexMethod(int[,] simplexTable, int count, int[] bp)
            {
                int[,] temp = simplexTable;
                int plusIndex = 0, minIndex = 0, min = 10000000;
                bool plusIs = false;

                for (int i = 0; i < 3 * count; i++)
                {
                    if (temp[0, i] > 0)
                    {
                        plusIndex = i;
                        plusIs = true;
                        break;
                    }
                }

                while (plusIs == true)
                {
                    temp = simplexTable;
                    simplexTable = new int[2 * count + 1, 3 * count + 1];
                    minIndex = 0; min = 10000000;

                    for (int i = 1; i < 2 * count + 1; i++)
                    {
                        if (temp[i, plusIndex] != 0 && (temp[i, 3 * count] / temp[i, plusIndex]) > 0)
                        {
                            if ((temp[i, 3 * count] / temp[i, plusIndex]) < min)
                            {
                                min = temp[i, 3 * count] / temp[i, plusIndex];
                                minIndex = i;
                            }
                        }
                    }
                    bp[minIndex - 1] = plusIndex;

                    for (int i = 0; i < 3 * count + 1; i++)
                    {
                        simplexTable[minIndex, i] = temp[minIndex, i] / temp[minIndex, plusIndex];
                    }
                    for (int i = 0; i < 2 * count + 1; i++)
                    {
                        if (i != minIndex)
                        {
                            for (int j = 0; j < 3 * count + 1; j++)
                            {
                                simplexTable[i, j] = temp[i, j] - (simplexTable[minIndex, j] * temp[i, plusIndex]);
                            }
                        }

                    }
                    plusIs = false;
                    for (int i = 0; i < 3 * count; i++)
                    {
                        if (temp[0, i] > 0)
                        {
                            plusIndex = i;
                            plusIs = true;
                            break;
                        }
                    }

                }

                return simplexTable;
            }

        public int[,] MaxSimplexMethod(int[,] simplexTable, int count, int[] bp)
        {
            int[,] temp = simplexTable;
            int negativeIndex = 0, minIndex = 0, min = 10000000;
            bool negativeIs = false;

            for (int i = 0; i < 3 * count; i++)
            {
                if (temp[0, i] < 0)
                {
                    negativeIndex = i;
                    negativeIs = true;
                    break;
                }
            }

            while (negativeIs == true)
            {
                temp = simplexTable;
                simplexTable = new int[2 * count + 1, 3 * count + 1];
                minIndex = 0; min = 10000000;

                for (int i = 1; i < 2 * count + 1; i++)
                {
                    if (temp[i, negativeIndex] != 0 && (temp[i, 3 * count] / temp[i, negativeIndex]) > 0)
                    {
                        if ((temp[i, 3 * count] / temp[i, negativeIndex]) < min)
                        {
                            min = temp[i, 3 * count] / temp[i, negativeIndex];
                            minIndex = i;
                        }
                    }
                }
                bp[minIndex - 1] = negativeIndex;

                for (int i = 0; i < 3 * count + 1; i++)
                {
                    simplexTable[minIndex, i] = temp[minIndex, i] / temp[minIndex, negativeIndex];
                }
                for (int i = 0; i < 2 * count + 1; i++)
                {
                    if (i != minIndex)
                    {
                        for (int j = 0; j < 3 * count + 1; j++)
                        {
                            simplexTable[i, j] = temp[i, j] - (simplexTable[minIndex, j] * temp[i, negativeIndex]);
                        }
                    }

                }
                negativeIs = false;
                for (int i = 0; i < 3 * count; i++)
                {
                    if (temp[0, i] < 0)
                    {
                        negativeIndex = i;
                        negativeIs = true;
                        break;
                    }
                }

            }

            return simplexTable;
        }
    }
    }
