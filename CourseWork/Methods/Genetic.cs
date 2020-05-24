using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWork.Models;

namespace CourseWork.Methods
{
    public class Genetic
    {
        private Task IndividualTask;
        private int parentsQuantity;
        private int NumberOfIterations;
        private int n;
        private Random rand = new Random();

        public Genetic(Task individualTask)
        {
            IndividualTask = individualTask;
            n = IndividualTask.X_start.Length;
            parentsQuantity = 15 * n;
        }

        public Genetic(Task individualTask, int numberOfIterations)
        {
            IndividualTask = individualTask;
            NumberOfIterations = numberOfIterations;
            n = IndividualTask.X_start.Length;
            parentsQuantity = 15 * n;
        }

        public TaskPair ExecuteAlgorithm()
        {
            IList<TaskPair> parents = FindParents();
            long record = parents.First().Fx;
            int lasting = 0;
            TaskPair taskPair = get_children(parents, record, lasting);
            return taskPair;
        }
        
        private TaskPair get_children(IList<TaskPair> parents, long record, int lasting)
        {
            if (parents.Count % 2 != 0)
                parents.RemoveAt(parents.Count - 1);
            ICollection<TaskPair> iterationNodes = new SortedSet<TaskPair>();
            foreach(TaskPair parent in parents)
            {
                iterationNodes.Add(parent);
            }
            TaskPair.Shuffle(parents);
            for (int k = 0; k < parents.Count; k += 2)
            {
                int quantity = rand.Next(1, 5);
                while(quantity != 0)
                {
                    TaskPair child = generateChild(parents[k], parents[k + 1]);
                    ChildMutation(child);
                    if (childIsAdmissible(child))
                    {
                        child.Fx = IndividualTask.count_fx(child.X);
                        iterationNodes.Add(child);
                        quantity--;
                    }
                }

            }
            long newRecord = iterationNodes.First().Fx;
            if ((newRecord == record && lasting == 99) || newRecord == 0)
            {
                return iterationNodes.First();
            }
            else
            {
                IList<TaskPair> iterationNodesList = iterationNodes
                .Take(3 * iterationNodes.Count / 4)
                .ToList();
                IList<TaskPair> new_nodes = iterationNodes
                    .Take(parentsQuantity / 2)
                    .ToList();
                foreach (TaskPair addedNode in new_nodes)
                    iterationNodesList.Remove(addedNode);
                TaskPair.Shuffle(iterationNodesList);
                for (int i = 0; i < Math.Min(parentsQuantity / 2, iterationNodesList.Count); i++)
                    new_nodes.Add(iterationNodesList.ElementAt(i));
                if (new_nodes.Count%2 != 0)
                {
                    new_nodes.RemoveAt(new_nodes.Count - 1);
                }
                if (newRecord == record)
                    return get_children(new_nodes, newRecord, lasting + 1);
                else
                    return get_children(new_nodes, newRecord, 0);

            }
        }

        private bool childIsAdmissible(TaskPair child)
        {
            bool admissible = true;
            for (int i = 0; i < n; i++)
            {
                if (child.X[i] < IndividualTask.Constraints[i][0]
                    || child.X[i] > IndividualTask.Constraints[i][1])
                {
                    admissible = false;
                    break;
                }
            }
            return admissible;
        }

        private void ChildMutation(TaskPair child)
        {
            int IdxToMutate = rand.Next(n);
            child.X[IdxToMutate]--;
        }

        private TaskPair generateChild(TaskPair parent1, TaskPair parent2) //генеруємо нащадка
        {
            TaskPair child = new TaskPair();
            child.X = new int[this.n];
            for (int i = 0; i < n; i++)
            {
                double alpha = Convert.ToDouble(rand.Next(-25, 125))/100;
                child.X[i] = Convert.ToInt32(parent1.X[i] + alpha*(parent2.X[i]- parent1.X[i]));
            }
            child.Fx = IndividualTask.count_fx(child.X);
            return child;
        }

        private IList<TaskPair> FindParents()
        {
            ICollection<TaskPair> parentSet = new SortedSet<TaskPair>
            {
                new TaskPair(IndividualTask.X_start, IndividualTask.count_fx(IndividualTask.X_start))
            };
            for(int i = 0; i < parentsQuantity-1; i++)
            {
                TaskPair newTaskPair = new TaskPair();
                int[] x_new = new int[n];
                Array.Copy(IndividualTask.X_start, x_new, n);
                for (int j = 0; j < n; j++){
                    int boolval = rand.Next(2);
                    if(boolval == 1)
                    {
                        x_new[j] += rand.Next(
                            Math.Max(-5, (IndividualTask.Constraints[j][0]- IndividualTask.X_start[j])),
                            Math.Min(6, (IndividualTask.Constraints[j][1] - IndividualTask.X_start[j])));
                    }
                }
                newTaskPair.X = x_new;
                newTaskPair.Fx = IndividualTask.count_fx(x_new);
                parentSet.Add(newTaskPair);
            }
            parentsQuantity = parentsQuantity / 3;
            if (parentsQuantity % 2 != 0)
                parentsQuantity++;
            return parentSet.Take(parentsQuantity).ToList();
        }
    }
}
