using System;
using LearningModel;
using Matrices;

namespace MachineLearning
{

    class Gene<T>
    {
        public Gene(LearningModel<T> machine, Random rand, T mapMin, T mapMax)
        {
            int dnaSize = 0;

            for (int i = 0; i < machine.weights.Length; i++)
            {
                dnaSize += machine.weights[i].GetMatrix().Length;
            }

            for (int i = 0; i < machine.biases.Length; i++)
            {
                dnaSize += machine.biases[i].GetMatrix().Length;
            }

            genes = new T[dnaSize];

            for (int i = 0; i < dnaSize; i++)
            {
                genes[i] = (float)rand.NextDouble() * ((dynamic)mapMax - mapMin) + mapMin;
            }
        }

        public T[] genes;

        public void ExtractGene(LearningModel<T> machine)
        {
            int readerPosition = 0;

            for (int i = 0; i < machine.weights.Length; i++)
            {
                T[,] matrix = machine.weights[i].GetMatrix();
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    for (int x = 0; x < matrix.GetLength(1); x++)
                    {
                        genes[readerPosition] = matrix[y, x];
                        readerPosition++;
                    }
                }
            }

            for (int i = 0; i < machine.biases.Length; i++)
            {
                T[,] matrix = machine.biases[i].GetMatrix();
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    for (int x = 0; x < matrix.GetLength(1); x++)
                    {
                        genes[readerPosition] = matrix[y, x];
                        readerPosition++;
                    }
                }
            }
        }

        public void InjectGene(LearningModel<T> machine)
        {
            int readerPosition = 0;

            for (int i = 0; i < machine.weights.Length; i++)
            {
                T[,] matrix = machine.weights[i].GetMatrix();
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    for (int x = 0; x < matrix.GetLength(1); x++)
                    {
                        machine.weights[i].GetMatrix()[y, x] = genes[readerPosition];
                        readerPosition++;
                    }
                }
            }

            for (int i = 0; i < machine.biases.Length; i++)
            {
                T[,] matrix = machine.biases[i].GetMatrix();
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    for (int x = 0; x < matrix.GetLength(1); x++)
                    {
                        machine.biases[i].GetMatrix()[y, x] = genes[readerPosition];
                        readerPosition++;
                    }
                }
            }
        }

        public void CrossOver(Gene<T> mother, Gene<T> father)
        {
            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = (i < (i * 0.5)) ? mother.genes[i] : father.genes[i];
            }
        }

        public void Mutation(Random rand, T mapMax, T mapMin)
        {
            if (rand.NextDouble() < 0.5)
            {
                int a = rand.Next(0, genes.Length);
                int b = rand.Next(0, genes.Length);
                while (a == b) { b = rand.Next(0, genes.Length); }
                T tmp = genes[a];
                genes[a] = genes[b];
                genes[b] = tmp;
            }
            else
            {
                genes[rand.Next(0, genes.Length)] = (float)rand.NextDouble() * ((dynamic)mapMax - mapMin) + mapMin;
            }
        }
    }

    class GeneticAlgorithm<T>
    {
        static Random rand = new Random();
        int size;
        Gene<T>[] genePool;
        public T[] score;

        T mapMax, mapMin;
        T crossOverRate;
        T mutationRate;

        public GeneticAlgorithm(int poolSize, LearningModel<T> machineLayout, T mapMax, T mapMin, T crossOverRate, T mutationRate)
        {
            size = poolSize;
            genePool = new Gene<T>[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                genePool[i] = new Gene<T>(machineLayout, rand, mapMax, mapMin);
            }
            score = new T[poolSize];

            this.mapMax = mapMax;
            this.mapMin = mapMin;

            this.crossOverRate = crossOverRate;
            this.mutationRate = mutationRate;
        }

        public void SetupMachine(LearningModel<T> machine, int index)
        {
            genePool[index].InjectGene(machine);
        }

        public void SetScore(int index, T score)
        {
            this.score[index] = score;
        }

        public void EvaluateGeneration()
        {
            Array.Sort(score, genePool);
        }

        public void CrossOver_HillSlice()
        {
            for (int i = 0; i < size / 2; i++)
            {
                if ((dynamic)rand.NextDouble() < crossOverRate)
                {
                    Gene<T> mot = Selection_HillSlice();
                    Gene<T> fat = Selection_HillSlice();
                    genePool[i].CrossOver(mot, fat);
                }
                if ((dynamic)rand.NextDouble() < mutationRate)
                {
                    genePool[i].Mutation(rand, mapMax, mapMin);
                }
            }
        }

        private Gene<T> Selection_HillSlice()
        {
            T sum = score[0];
            for (int i = 1; i < size; i++) { sum += (dynamic)score[i]; }
            T selection = (dynamic)(float)rand.NextDouble() * sum;
            T find = (dynamic)0;
            for (int i = 0; i < size; i++)
            {
                if (selection < (dynamic)find)
                {
                    return genePool[i];
                }
                find += (dynamic)score[i];
            }
            return genePool[size - 1];
        }
    }

    class GATester
    {
        public static void TestGA()
        {
            int gen = 0;
            LearningModel<float> agent = new LearningModel<float>(new int[] { 1, 4, 4, 1 }, new string[] { "relu", "relu", "relu", "relu" }, "MSE");
            agent.Initialize_Silent();
            GeneticAlgorithm<float> ga = new GeneticAlgorithm<float>(20, agent, 1f, -1f, 0.9f, 1f);
            do
            {
                Console.Clear();
                Console.WriteLine("Gen {0}", ++gen);
                for (int i = 0; i < 20; i++)
                {
                    ga.SetupMachine(agent, i);
                    float score = 0;
                    float output1 = agent.GetPredict(new Matrix<float>(new float[,] { { 1 } })).GetMatrix()[0, 0];
                    float output2 = agent.GetPredict(new Matrix<float>(new float[,] { { 2 } })).GetMatrix()[0, 0];
                    float output3 = agent.GetPredict(new Matrix<float>(new float[,] { { 3 } })).GetMatrix()[0, 0];
                    score += 1 - (float)Math.Pow(1 - output1, 2);
                    score += 16 - (float)Math.Pow(4 - output2, 2);
                    score += 81 - (float)Math.Pow(9 - output3, 2);
                    ga.SetScore(i, score);
                }
                ga.EvaluateGeneration();
                ga.CrossOver_HillSlice();
                for (int i = 0; i < 20; i++)
                {
                    Console.WriteLine("Agent {0} score : {1}", i, ga.score[i]);
                }
                ga.SetupMachine(agent, 19);
                float o1 = agent.GetPredict(new Matrix<float>(new float[,] { { 1 } })).GetMatrix()[0, 0];
                float o2 = agent.GetPredict(new Matrix<float>(new float[,] { { 2 } })).GetMatrix()[0, 0];
                float o3 = agent.GetPredict(new Matrix<float>(new float[,] { { 3 } })).GetMatrix()[0, 0];
                Console.WriteLine("Best Agent Answer = {0}, {1}, {2}", o1, o2, o3);
            } while (Console.ReadKey().Key != ConsoleKey.X);

            do
            {
                agent.Train_Batch(new Matrix<float>(new float[,] { { 1 }, { 2 }, { 3 } }), new Matrix<float>(new float[,] { { 1 }, { 4 }, { 9 } }));
            } while (Console.ReadKey().Key != ConsoleKey.X);

            do
            {
                float[,] f = new float[1, 1];
                f[0, 0] = float.Parse(Console.ReadLine());
                float output = agent.GetPredict(new Matrix<float>(f)).GetMatrix()[0, 0];
                Console.WriteLine("Answer = {0}", output);
            } while (true);
        }
    }
}