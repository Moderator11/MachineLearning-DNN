using System;
using Matrices;

namespace LearningModel
{
    class LearningModelTester
    {
        public void Test_ControlledVariables()
        {
            LearningModel<double> a = new LearningModel<double>(new int[] { 2, 2, 1 }, new string[] { "sigmoid", "sigmoidz", "sigmoid" }, "MSE");
            a.Initialize();
            a.weights[0] = new Matrix<double>(new double[,] { { 0.1, 0.2 }, { 0.3, 0.4 } });
            a.weights[1] = new Matrix<double>(new double[,] { { 0.5 }, { 0.6 } });
            a.biases[0] = new Matrix<double>(new double[,] { { 0.3, 0.4 } });
            a.biases[1] = new Matrix<double>(new double[,] { { 0.5 } });
            Matrix<double> i1 = new Matrix<double>(new double[,] { { 0.5, 1 } });
            Matrix<double> o1 = new Matrix<double>(new double[,] { { 1 } });
            a.Train(i1, o1);
            a.Debug();
            Console.WriteLine(a.weights[1].GetMatrix()[0, 0]);
            Console.ReadKey();
        }

        private void DataSet_ANDGate(out LearningModel<double> a, out Matrix<double> i1, out Matrix<double> o1, out Matrix<double> i2, out Matrix<double> o2,
        out Matrix<double> i3, out Matrix<double> o3, out Matrix<double> i4, out Matrix<double> o4, out Matrix<double> bi1, out Matrix<double> bo1)
        {
            a = new LearningModel<double>(new int[] { 2, 2, 2, 1 }, new string[] { "sigmoid", "sigmoid", "sigmoid", "sigmoid" }, "MSE");

            i1 = new Matrix<double>(new double[,] { { 0, 0 } });
            o1 = new Matrix<double>(new double[,] { { 0 } });

            i2 = new Matrix<double>(new double[,] { { 1, 1 } });
            o2 = new Matrix<double>(new double[,] { { 1 } });

            i3 = new Matrix<double>(new double[,] { { 1, 0 } });
            o3 = new Matrix<double>(new double[,] { { 0 } });

            i4 = new Matrix<double>(new double[,] { { 0, 1 } });
            o4 = new Matrix<double>(new double[,] { { 0 } });

            bi1 = new Matrix<double>(new double[,] { { 0, 0 }, { 1, 1 }, { 1, 0 }, { 0, 1 } });
            bo1 = new Matrix<double>(new double[,] { { 0 }, { 1 }, { 0 }, { 0 } });
        }

        private void DataSet_XORGate(out LearningModel<double> a, out Matrix<double> i1, out Matrix<double> o1, out Matrix<double> i2, out Matrix<double> o2,
        out Matrix<double> i3, out Matrix<double> o3, out Matrix<double> i4, out Matrix<double> o4, out Matrix<double> bi1, out Matrix<double> bo1)
        {
            a = new LearningModel<double>(new int[] { 2, 2, 2, 1 }, new string[] { "sigmoid", "sigmoid", "sigmoid", "sigmoid" }, "MSE");

            i1 = new Matrix<double>(new double[,] { { 0, 0 } });
            o1 = new Matrix<double>(new double[,] { { 0 } });

            i2 = new Matrix<double>(new double[,] { { 1, 1 } });
            o2 = new Matrix<double>(new double[,] { { 0 } });

            i3 = new Matrix<double>(new double[,] { { 1, 0 } });
            o3 = new Matrix<double>(new double[,] { { 1 } });

            i4 = new Matrix<double>(new double[,] { { 0, 1 } });
            o4 = new Matrix<double>(new double[,] { { 1 } });

            bi1 = new Matrix<double>(new double[,] { { 0, 0 }, { 1, 1 }, { 1, 0 }, { 0, 1 } });
            bo1 = new Matrix<double>(new double[,] { { 0 }, { 0 }, { 1 }, { 1 } });
        }

        private void DataSet_Complex(out LearningModel<double> a, out Matrix<double> i1, out Matrix<double> o1, out Matrix<double> i2, out Matrix<double> o2,
        out Matrix<double> i3, out Matrix<double> o3, out Matrix<double> i4, out Matrix<double> o4, out Matrix<double> bi1, out Matrix<double> bo1)
        {
            a = new LearningModel<double>(new int[] { 10, 20, 20, 10 }, new string[] { "sigmoid", "sigmoid", "sigmoid", "sigmoid" }, "MSE");

            i1 = new Matrix<double>(new double[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } });
            o1 = new Matrix<double>(new double[,] { { .0, .1, .2, .3, .4, .5, .6, .7, .8, .9 } });

            i2 = new Matrix<double>(new double[,] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } });
            o2 = new Matrix<double>(new double[,] { { .1, .2, .3, .4, .5, .5, .4, .3, .2, .1 } });

            i3 = new Matrix<double>(new double[,] { { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 } });
            o3 = new Matrix<double>(new double[,] { { .9, .8, .7, .6, .5, .4, .3, .2, .1, .0 } });

            i4 = new Matrix<double>(new double[,] { { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 } });
            o4 = new Matrix<double>(new double[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } });

            bi1 = new Matrix<double>(new double[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 }
            });
            bo1 = new Matrix<double>(new double[,]
            {
                { .0, .1, .2, .3, .4, .5, .6, .7, .8, .9 },
                { .1, .2, .3, .4, .5, .5, .4, .3, .2, .1 },
                { .9, .8, .7, .6, .5, .4, .3, .2, .1, .0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }

            });
        }

        private void DataSet_WhichQuadrantDotBelongs(out LearningModel<double> a, out Matrix<double> i1, out Matrix<double> o1,out Matrix<double> i2, out Matrix<double> o2,
        out Matrix<double> i3, out Matrix<double> o3, out Matrix<double> i4, out Matrix<double> o4, out Matrix<double> bi1, out Matrix<double> bo1)
        {
            a = new LearningModel<double>(new int[] { 2, 6, 8, 4 }, new string[] { "relu", "leakyrelu", "leakyrelu", "sigmoid" }, "MSE");

            i1 = new Matrix<double>(new double[,] { { 1, 1 } });
            o1 = new Matrix<double>(new double[,] { { 1, 0, 0, 0 } });

            i2 = new Matrix<double>(new double[,] { { -1, 1 } });
            o2 = new Matrix<double>(new double[,] { { 0, 1, 0, 0 } });

            i3 = new Matrix<double>(new double[,] { { -1, -1 } });
            o3 = new Matrix<double>(new double[,] { { 0, 0, 1, 0 } });

            i4 = new Matrix<double>(new double[,] { { 1, -1 } });
            o4 = new Matrix<double>(new double[,] { { 0, 0, 0, 1 } });

            bi1 = new Matrix<double>(new double[,] { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 } });
            bo1 = new Matrix<double>(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        }

        private void DataSet_CircularClassification(out LearningModel<double> a, out Matrix<double> i1, out Matrix<double> o1, out Matrix<double> i2, out Matrix<double> o2,
        out Matrix<double> i3, out Matrix<double> o3, out Matrix<double> i4, out Matrix<double> o4, out Matrix<double> bi1, out Matrix<double> bo1)
        {
            a = new LearningModel<double>(new int[] { 2, 4, 1, 2 }, new string[] { "relu", "leakyrelu", "leakyrelu", "sigmoid" }, "MSE");
            GenerateCircularSampleDate(100, 0, 0, out bi1, out bo1);

            i1 = null; o1 = null;
            i2 = null; o2 = null;
            i3 = null; o3 = null;
            i4 = null; o4 = null;

            void GenerateCircularSampleDate(int sampleNumber, double centerX, double centerY, out Matrix<double> _coordinates, out Matrix<double> _classification)
            {
                double[,] coordinates = new double[sampleNumber * 2, 2];
                double[,] classification = new double[sampleNumber * 2, 2];
                Random r = new Random();
                for (int i = 0; i < sampleNumber; i++)
                {
                    double radius = r.NextDouble();
                    double radian = r.NextDouble() * 2 * Math.PI;
                    double x = radius * Math.Cos(radian) + centerX;
                    double y = radius * Math.Sin(radian) + centerY;
                    coordinates[i, 0] = x;
                    coordinates[i, 1] = y;
                    classification[i, 0] = 1;
                    classification[i, 1] = 0;
                }
                for (int i = sampleNumber; i < 2 * sampleNumber; i++)
                {
                    double radius = 10 * r.NextDouble() + 1;
                    double radian = r.NextDouble() * 2 * Math.PI;
                    double x = radius * Math.Cos(radian) + centerX;
                    double y = radius * Math.Sin(radian) + centerY;
                    coordinates[i, 0] = x;
                    coordinates[i, 1] = y;
                    classification[i, 0] = 0;
                    classification[i, 1] = 1;
                }
                _coordinates = new Matrix<double>(coordinates);
                _classification = new Matrix<double>(classification);
            }
        }

        public void ConsoleInterface()
        {
            LearningModel<double> a;
            Matrix<double> i1, i2, i3, i4, o1, o2, o3, o4;
            Matrix<double> bi1, bo1;
            DataSet_CircularClassification(out a, out i1, out o1, out i2, out o2, out i3, out o3, out i4, out o4, out bi1, out bo1);
            do
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.X:
                        Console.WriteLine("Program ended, Press any key to exit...");
                        Console.ReadKey();
                        return;
                    case ConsoleKey.C:
                        Console.Clear();
                        break;
                    case ConsoleKey.D:
                        a.Debug();
                        break;
                    case ConsoleKey.I:
                        a.Initialize();
                        break;
                    case ConsoleKey.S:
                        a.SaveModelData();
                        break;
                    case ConsoleKey.L:
                        a.LoadModelData();
                        break;
                    case ConsoleKey.P:
                        Console.Write("Predict Mode : ");
                        string[] data = Console.ReadLine().Replace(" ", "").Split(',');
                        double[,] test = new double[1, data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            test[0, i] = double.Parse(data[i]);
                        }
                        a.Predict(new Matrix<double>(test));
                        break;
                    case ConsoleKey.V:
                        Console.WriteLine("Learning rate change:");
                        a.learningRate = double.Parse(Console.ReadLine());
                        break;
                    case ConsoleKey.T:
                        Console.Write("Single data training iteration : ");
                        int scount = int.Parse(Console.ReadLine());
                        for (int i = 0; i < 100000 * scount; i++)
                        {
                            /*if (Console.Title.Contains(';'))
                            {
                                Console.Title = Console.Title.Split(';')[0] + string.Format(";{0}%", (float)i / 1000);
                            }
                            else
                            {
                                Console.Title += ';';
                            }*/
                            a.Train_Silent(i1, o1);
                            a.Train_Silent(i2, o2);
                            a.Train_Silent(i3, o3);
                            a.Train_Silent(i4, o4);
                            /*a.Train_BatchSilent(
                            new Matrix<double>(new double[,] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } }),
                            new Matrix<double>(new double[,] { { 0 }, { 0 }, { 0 }, { 1 } }));*/
                        }
                        Console.WriteLine("Done");
                        break;
                    case ConsoleKey.Y:
                        Console.Write("Batch data training iteration : ");
                        int bcount = int.Parse(Console.ReadLine());
                        for (int i = 0; i < 100000 * bcount; i++)
                        {
                            a.Train_BatchSilent(bi1, bo1);
                        }
                        Console.WriteLine("Done");
                        break;
                    case ConsoleKey.B:
                        a.Train_Batch(bi1, bo1);
                        break;
                    case ConsoleKey.H:
                        string help = "X : Exit, C : Clear Screen, D : Debug, I : Initialization, S : Save, L : Load, P : Predict," +
                            "V : Learning rate change, T : Silent train single, Y : Silent train batch, B : Train Batch, H : Help, Default : Train Sigle";
                        Console.WriteLine(help.Replace(", ", "\n"));
                        break;
                    default:
                        a.Train(i1, o1);
                        a.Train(i2, o2);
                        a.Train(i3, o3);
                        a.Train(i4, o4);
                        break;
                }
            } while (true);
        }

        public static void CircularClassification()
        {
            int[] layer = { 2, 4, 1, 2 };
            string[] actf = { "relu", "leakyrelu", "leakyrelu", "sigmoid" };
            LearningModel<double> a = new LearningModel<double>(layer, actf, "MSE");
            Matrix<double> input, output;
            GenerateCircularSampleDate(100, 0, 0, out input, out output);
            a.Initialize_Silent();
            for (int i = 0; i < 10000; i++)
            {
                a.Train_BatchSilent(input, output);
            }
            Console.WriteLine("Train Done.");
            for (; ; )
            {
                string[] data = Console.ReadLine().Replace(" ", "").Split(',');
                double[,] test = new double[1, data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    test[0, i] = double.Parse(data[i]);
                }
                a.Predict(new Matrix<double>(test));
            }

            void GenerateCircularSampleDate(int sampleNumber, double centerX, double centerY, out Matrix<double> _coordinates, out Matrix<double> _classification)
            {
                double[,] coordinates = new double[sampleNumber * 2, 2];
                double[,] classification = new double[sampleNumber * 2, 2];
                Random r = new Random();
                for (int i = 0; i < sampleNumber; i++)
                {
                    double radius = r.NextDouble();
                    double radian = r.NextDouble() * 2 * Math.PI;
                    double x = radius * Math.Cos(radian) + centerX;
                    double y = radius * Math.Sin(radian) + centerY;
                    coordinates[i, 0] = x;
                    coordinates[i, 1] = y;
                    classification[i, 0] = 1;
                    classification[i, 1] = 0;
                }
                for (int i = sampleNumber; i < 2 * sampleNumber; i++)
                {
                    double radius = 10 * r.NextDouble() + 1;
                    double radian = r.NextDouble() * 2 * Math.PI;
                    double x = radius * Math.Cos(radian) + centerX;
                    double y = radius * Math.Sin(radian) + centerY;
                    coordinates[i, 0] = x;
                    coordinates[i, 1] = y;
                    classification[i, 0] = 0;
                    classification[i, 1] = 1;
                }
                _coordinates = new Matrix<double>(coordinates);
                _classification = new Matrix<double>(classification);
            }
        }
    }
}