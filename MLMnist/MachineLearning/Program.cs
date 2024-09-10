using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LearningModel;
using Matrices;

using MnistDatasetLoader;

namespace MachineLearning
{
    class Program
    {
        static byte[] labels;
        static byte[][] images;
        static Matrix<double> labelMats, imageMats;

        static void Main(string[] args)
        {
            //Test();
            MDsetLoader loader = new MDsetLoader();
            LoadTrainingSet();
            LearningModel<double> a = new LearningModel<double>(new int[] { 28 * 28, 65, 10 }, new string[] { "leakyrelu", "sigmoid", "sigmoid" }, "MSE");
            a.Initialize_Silent();
            a.learningRate = 0.3;

            Random r = new Random(Guid.NewGuid().GetHashCode());

            for (; ; )
            {
                int ind = r.Next(0, 60000);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.X:
                        a.LoadModelData();
                        break;
                    case ConsoleKey.P:
                        for (int i = 0; i < 10000; i++)
                        {
                            a.Train_Silent(loader.GetOneImage(ind), loader.GetOneLabel(ind));
                        }
                        Console.WriteLine("Done");
                        break;
                    case ConsoleKey.Q:
                        a.Train(loader.GetOneImage(ind), loader.GetOneLabel(ind));
                        Console.WriteLine("I'm {0}", labels[ind]);
                        break;
                    case ConsoleKey.C:
                        for (int i = 0; i < 60000; i++)
                        {
                            Console.WriteLine("{0} : {1}", i, labels[i]);
                            a.Train_Silent(loader.GetOneImage(i), loader.GetOneLabel(i));
                            Matrix<double>.EnumerateMatrix(loader.GetOneLabel(i).GetMatrix());
                        }
                        Console.WriteLine("Done");
                        break;
                    case ConsoleKey.S:
                        //a.SaveModelData();
                        a.SaveModelData_Xml();
                        break;
                    case ConsoleKey.L:
                        //a.LoadModelData();
                        a.LoadModelData_Xml();
                        a.learningRate = 0.5;
                        break;
                    case ConsoleKey.E:
                        LoadTrainingSet();
                        EvaluateTrainingSet();
                        break;
                    case ConsoleKey.R:
                        LoadTestSet();
                        EvaluateTestSet();
                        break;
                }
            }
            a.Train_BatchSilent(imageMats, labelMats);
            Console.WriteLine("Done");

            void LoadTestSet()
            {
                loader.LoadTestSet();
                labels = loader.GetLabels();
                images = loader.GetImages();
                labelMats = loader.labelMatrix;
                imageMats = loader.imageMatrix;
                Console.WriteLine("Test set loaded. length = {0}", labels.Length);
            }

            void LoadTrainingSet()
            {
                loader = new MDsetLoader();
                labels = loader.GetLabels();
                images = loader.GetImages();
                labelMats = loader.labelMatrix;
                imageMats = loader.imageMatrix;
                Console.WriteLine("Training set loaded. length = {0}", labels.Length);
            }

            void EvaluateTrainingSet()
            {
                int falsecount = 0;
                for (int i = 0; i < 60000; i++)
                {
                    double[,] p = a.GetPredict(loader.GetOneImage(i)).GetMatrix();
                    int bestIndex = 0;
                    double bestValue = p[0, 0];
                    for (int x = 1; x < p.GetLength(1); x++)
                    {
                        if (bestValue < p[0, x])
                        {
                            bestIndex = x;
                            bestValue = p[0, x];
                        }
                    }
                    Console.WriteLine("Test {0} : Predict = {1}, RealAnswer = {2}", i, bestIndex, labels[i]);
                    if (bestIndex != labels[i])
                    {
                        falsecount++;
                    }
                }
                Console.WriteLine("falsecount = {0}, Accuracy = {1}%", falsecount, (double)(60000 - falsecount) / 600);
            }

            void EvaluateTestSet()
            {
                int falsecount = 0;
                for (int i = 0; i < 10000; i++)
                {
                    double[,] p = a.GetPredict(loader.GetOneImage(i)).GetMatrix();
                    int bestIndex = 0;
                    double bestValue = p[0, 0];
                    for (int x = 1; x < p.GetLength(1); x++)
                    {
                        if (bestValue < p[0, x])
                        {
                            bestIndex = x;
                            bestValue = p[0, x];
                        }
                    }
                    Console.WriteLine("Test {0} : Predict = {1}, RealAnswer = {2}", i, bestIndex, labels[i]);
                    if (bestIndex != labels[i])
                    {
                        falsecount++;
                    }
                }
                Console.WriteLine("falsecount = {0}, Accuracy = {1}%", falsecount, (double)(10000 - falsecount) / 100);
            }
        }

        static void Test()
        {
            LearningModelTester tester = new LearningModelTester();
            tester.ConsoleInterface();
        }
    }
}