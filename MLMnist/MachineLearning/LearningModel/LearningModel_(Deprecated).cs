using System;

namespace LearningModel_Deprecated
{
    class Tester//Do Tester.RunTest();
    {
        public static void RunTest()
        {
            double learningRate = 0.01f;
            Layer n1 = new Layer(1);
            Layer n2 = new Layer(2, n1);
            Layer n3 = new Layer(1, n2);
            do
            {
                TestCase(new double[] { 0.5f }, new double[] { 0.5f });
            } while (Console.ReadKey().Key == ConsoleKey.Enter);

            void TestCase(double[] data, double[] desiredOut)
            {
                Console.WriteLine("\n-----------------------------------------\n------------<Running TestCase>-----------");
                if (n1.perceptrons.Length == data.Length && n3.perceptrons.Length == desiredOut.Length)
                {
                    for (int i = 0; i < n1.perceptrons.Length; i++)
                    {
                        n1.perceptrons[i].result = data[i];
                    }
                    n2.CalculateLayer();
                    n3.CalculateLayer();

                    Console.WriteLine("\n-----------------------------------------\n------------<Back Propagation>-----------");//only 3n perceptrons
                    for (int i = 0; i < n3.perceptrons.Length; i++)
                    {
                        Console.WriteLine("Output : {0}\nDesired Output : {1}\nCost : {2}", n3.perceptrons[i].result, desiredOut[i], Math.Pow(n3.perceptrons[i].result - desiredOut[i], 2));
                        Console.WriteLine("dcost/dbias={0}", 2 * (n3.perceptrons[i].result - desiredOut[i]) * SigmoidDerivative(n3.perceptrons[i].sumofAll));
                        for (int k = 0; k < n3.perceptrons[i].weights.Length; k++)
                        {
                            double dcdw = 2 * (n3.perceptrons[i].result - desiredOut[i]) * SigmoidDerivative(n3.perceptrons[i].sumofAll) * n2.perceptrons[k].result;
                            Console.WriteLine("dcost/dw{0}={1}", k, dcdw);
                            Console.WriteLine("Applying new weight...");
                            Console.WriteLine("Before weight = {0}", n3.perceptrons[i].weights[k]);
                            n3.perceptrons[i].weights[k] += learningRate * dcdw;
                            Console.WriteLine("After weight = {0}", n3.perceptrons[i].weights[k]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("There is something wrong with the neural structure.");
                }
                double Sigmoid(double x)
                {
                    return 1 / (1 - Math.Pow(Math.E, -x));
                }
                double SigmoidDerivative(double x)
                {
                    return Sigmoid(x) * (1 - Sigmoid(x));
                }
            }

            Console.ReadKey();
        }
    }

    class Layer
    {
        public static bool implyRandom = true;
        static Random r = new Random();

        public Layer inputLayer;
        public Perceptron[] perceptrons;
        public Layer(int count, Layer inLayer = null)
        {
            perceptrons = new Perceptron[count];
            inputLayer = inLayer;
            for (int i = 0; i < perceptrons.Length; i++)
            {
                if (implyRandom)//imply random
                {
                    double b = r.NextDouble();
                    int pn = r.Next() % 2 == 0 ? 1 : -1;
                    Console.WriteLine("\n<P{0}> is initialized by bias = {1}", i, b);
                    perceptrons[i] = new Perceptron(pn * b, inputLayer);
                }
                else
                {
                    Console.WriteLine("\n<P{0}> is initialized by bias = 0", i);
                    perceptrons[i] = new Perceptron(0, inputLayer);
                }
            }
        }

        public void CalculateLayer()
        {
            Console.WriteLine("\n<Layer Calculation>");
            for (int i = 0; i < perceptrons.Length; i++)
            {
                Console.Write("P{0} = ", i);
                perceptrons[i].Calculate(inputLayer);
            }
        }

        public class Perceptron
        {
            public double bias = 0f;
            public double result = 0f;
            public double[] weights = new double[0];
            public double sumofAll = 0f;//for backpropagation

            public Perceptron(double _bias = 0, Layer inputLayer = null)
            {
                bias = _bias;
                if (inputLayer != null)
                {
                    weights = new double[inputLayer.perceptrons.Length];
                    if (implyRandom)//imply random
                    {
                        for (int i = 0; i < weights.Length; i++)
                        {
                            double w = r.NextDouble();
                            int pn = r.Next() % 2 == 0 ? 1 : -1;
                            weights[i] = pn * w;
                            Console.WriteLine("    weight[{0}] = {1}", i, weights[i]);
                        }
                    }
                }
            }

            public void Calculate(Layer inputLayer)
            {
                double sum = 0;
                for (int i = 0; i < inputLayer.perceptrons.Length; i++)
                {
                    sum += inputLayer.perceptrons[i].result * weights[i];
                }
                sumofAll = sum + bias;
                //Activation Function
                /*if (sum + bias <= 0)
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }*/
                //Activation Function
                result = 1 / (1 + Math.Pow(Math.E, -(sum + bias)));
                //Activation Function
                Console.WriteLine("{0}", result);
            }
        }
    }
}