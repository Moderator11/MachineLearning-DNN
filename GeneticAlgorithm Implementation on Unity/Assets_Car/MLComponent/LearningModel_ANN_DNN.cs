using System;
using System.IO;
using System.Xml.Serialization;
using Matrices;

namespace LearningModel//Written by Soomin Park, In ‎2022‎-07‎-09 saturday, 8:26:50 pm.
{
    /// <summary>DNN Learning Model, T must be numeric</summary>
    public class LearningModel<T> : Base.LearningModelBase<T>
    {
        string path = Environment.CurrentDirectory + @"\Model-Data.mldat";

        int[] layerModel;

        public Matrix<T>[] weights;
        public Matrix<T>[] biases;
        Matrix<T>[] unitSum;
        Matrix<T>[] unitOut;
        Matrix<T>[] unitDelta;

        public T mapMax = (T)Convert.ChangeType(1, typeof(T));
        public T mapMin = (T)Convert.ChangeType(-1, typeof(T));
        public T learningRate = (T)Convert.ChangeType(0.01, typeof(T));

        string[] activationFunctionAssigner;
        private Func<T, T>[] activationFunctions = null;
        private Func<T, T>[] activationPrimeFunctions = null;
        string lossFunctionAssigner;
        private Func<T, T, T> lossFunction = null;
        private Func<T, T, T> lossFunctionPrime = null;

        /// <summary>Preinitializes Model, must call Initialize() before train.</summary>
        /// <param name="_layerModel">Defines model structure</param>
        /// <param name="_activationFunctions">Defines activation function of each layer, All layer requires its own function but [0] does nothing since it is input layer</param>
        /// <param name="_lossFunction">Defines loss calculation method. ex) MSE</param>
        public LearningModel(int[] _layerModel, string[] _activationFunctions, string _lossFunction)
        {
            layerModel = _layerModel;
            activationFunctionAssigner = _activationFunctions;
            activationFunctions = new Func<T, T>[_layerModel.Length];
            activationPrimeFunctions = new Func<T, T>[_layerModel.Length];
            lossFunctionAssigner = _lossFunction;
        }

        public void Initialize()
        {
            int connectionLayerCount = layerModel.Length - 1;

            RunInitialize();

            void RunInitialize()
            {
                Init_MatrixVariables();
                Init_BiasnWeights();
                Init_StateVectors();
                Init_AssignActivationFunctions();
                Init_AssignLossFUnction();
            }

            void Init_MatrixVariables()
            {
                weights = new Matrix<T>[connectionLayerCount];
                biases = new Matrix<T>[connectionLayerCount];
                unitSum = new Matrix<T>[layerModel.Length];
                unitOut = new Matrix<T>[layerModel.Length];
                unitDelta = new Matrix<T>[layerModel.Length];
            }

            void Init_BiasnWeights()
            {
                for (int i = 0; i < connectionLayerCount; i++)
                {
                    weights[i] = new Matrix<T>(layerModel[i], layerModel[i + 1]);
                    biases[i] = new Matrix<T>(1, layerModel[i + 1]);
                    weights[i].Map(mapMin, mapMax);
                    biases[i].Map(mapMin, mapMax);
                    Console.WriteLine("\nLayer {0}-{1} weights", i, i + 1);
                    Matrix<T>.EnumerateMatrix(weights[i].GetMatrix());
                    Console.WriteLine("\nLayer {0} biases", i + 1);
                    Matrix<T>.EnumerateMatrix(biases[i].GetMatrix());
                }
                Console.WriteLine();
            }

            void Init_StateVectors()
            {
                for (int i = 0; i < layerModel.Length; i++)
                {
                    unitSum[i] = new Matrix<T>(1, layerModel[i]);
                    unitOut[i] = new Matrix<T>(1, layerModel[i]);
                    unitDelta[i] = new Matrix<T>(1, layerModel[i]);
                    if (i == 0)
                    {
                        Console.WriteLine("InputLayer {0} count = {1}", i, layerModel[i]);
                    }
                    else if (i == layerModel.Length - 1)
                    {
                        Console.WriteLine("OutputLayer {0} count = {1}", i, layerModel[i]);
                    }
                    else
                    {
                        Console.WriteLine("HiddenLayer {0} count = {1}", i, layerModel[i]);
                    }
                }
            }

            void Init_AssignActivationFunctions()
            {
                for (int i = 0; i < activationFunctionAssigner.Length; i++)
                {
                    switch (activationFunctionAssigner[i])
                    {
                        case "relu":
                            activationFunctions[i] = (T x) => ReLu(x);
                            activationPrimeFunctions[i] = (T x) => ReLuPrime(x);
                            Console.WriteLine("Layer{0} activation = ReLu", i + 1);
                            break;
                        case "leakyrelu":
                            activationFunctions[i] = (T x) => LeakyReLu(x);
                            activationPrimeFunctions[i] = (T x) => LeakyReLuPrime(x);
                            Console.WriteLine("Layer{0} activation = LeakyReLu", i + 1);
                            break;
                        case "sigmoid":
                            activationFunctions[i] = (T x) => Sigmoid(x);
                            activationPrimeFunctions[i] = (T x) => SigmoidPrime(x);
                            Console.WriteLine("Layer{0} activation = Sigmoid", i + 1);
                            break;
                        case "tanh":
                            activationFunctions[i] = (T x) => Tanh(x);
                            activationPrimeFunctions[i] = (T x) => TanhPrime(x);
                            Console.WriteLine("Layer{0} activation = Tanh", i + 1);
                            break;
                        default:
                            throw new Exception("Invalid activation function assigned");
                    }
                }
            }

            void Init_AssignLossFUnction()
            {
                switch (lossFunctionAssigner)
                {
                    case "MSE":
                        lossFunction = (T a, T b) => MeanSquareError(a, b);
                        lossFunctionPrime = (T a, T b) => MeanSquareErrorPrime(a, b);
                        Console.WriteLine("Loss Function = MSE");
                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        public void Initialize_Silent()
        {
            int connectionLayerCount = layerModel.Length - 1;

            RunInitialize();

            void RunInitialize()
            {
                Init_MatrixVariables();
                Init_BiasnWeights();
                Init_StateVectors();
                Init_AssignActivationFunctions();
                Init_AssignLossFUnction();
            }

            void Init_MatrixVariables()
            {
                weights = new Matrix<T>[connectionLayerCount];
                biases = new Matrix<T>[connectionLayerCount];
                unitSum = new Matrix<T>[layerModel.Length];
                unitOut = new Matrix<T>[layerModel.Length];
                unitDelta = new Matrix<T>[layerModel.Length];
            }

            void Init_BiasnWeights()
            {
                for (int i = 0; i < connectionLayerCount; i++)
                {
                    weights[i] = new Matrix<T>(layerModel[i], layerModel[i + 1]);
                    biases[i] = new Matrix<T>(1, layerModel[i + 1]);
                    weights[i].Map(mapMin, mapMax);
                    biases[i].Map(mapMin, mapMax);
                }
            }

            void Init_StateVectors()
            {
                for (int i = 0; i < layerModel.Length; i++)
                {
                    unitSum[i] = new Matrix<T>(1, layerModel[i]);
                    unitOut[i] = new Matrix<T>(1, layerModel[i]);
                    unitDelta[i] = new Matrix<T>(1, layerModel[i]);
                }
            }

            void Init_AssignActivationFunctions()
            {
                for (int i = 0; i < activationFunctionAssigner.Length; i++)
                {
                    switch (activationFunctionAssigner[i])
                    {
                        case "relu":
                            activationFunctions[i] = (T x) => ReLu(x);
                            activationPrimeFunctions[i] = (T x) => ReLuPrime(x);
                            break;
                        case "leakyrelu":
                            activationFunctions[i] = (T x) => LeakyReLu(x);
                            activationPrimeFunctions[i] = (T x) => LeakyReLuPrime(x);
                            break;
                        case "sigmoid":
                            activationFunctions[i] = (T x) => Sigmoid(x);
                            activationPrimeFunctions[i] = (T x) => SigmoidPrime(x);
                            break;
                        case "tanh":
                            activationFunctions[i] = (T x) => Tanh(x);
                            activationPrimeFunctions[i] = (T x) => TanhPrime(x);
                            break;
                        default:
                            throw new Exception("Invalid activation function assigned");
                    }
                }
            }

            void Init_AssignLossFUnction()
            {
                switch (lossFunctionAssigner)
                {
                    case "MSE":
                        lossFunction = (T a, T b) => MeanSquareError(a, b);
                        lossFunctionPrime = (T a, T b) => MeanSquareErrorPrime(a, b);
                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        public void Train(Matrix<T> inputData, Matrix<T> desiredOutputData)//Find error can be simplified by using base.MeanSquareError(), Independent actFunctions Implemented, 2022-07-17 4:35pm, 박수민
        {
            RunCycle();

            void RunCycle()
            {
                FeedForward();
                FindError();
                CalculateDelta();
                BackPropagate();
            }

            void FeedForward()
            {
                Console.WriteLine("\nFeeding Forward...");
                for (int i = 0; i < layerModel.Length; i++)
                {
                    if (i == 0)//Input Layer
                    {
                        unitOut[i] = inputData;
                        Console.WriteLine("\nInput");
                        Matrix<T>.EnumerateMatrix(unitOut[i].GetMatrix());
                    }
                    else
                    {
                        unitSum[i] = (unitOut[i - 1] * weights[i - 1]) + biases[i - 1];
                        unitOut[i] = unitSum[i].DeepCopy();
                        unitOut[i].ApplyFunction(activationFunctions[i]);
                    }
                }
                Console.WriteLine("Feed Forward Output");
                Matrix<T>.EnumerateMatrix(unitOut[unitOut.Length - 1].GetMatrix());
            }

            void FindError()//For visualization only
            {
                T[,] output = unitOut[unitOut.Length - 1].GetMatrix();
                T[,] desiredOutput = desiredOutputData.GetMatrix();
                T[,] errortable = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunction);
                Console.WriteLine("Error Found");
                Matrix<T>.EnumerateMatrix(errortable);

                /*<<Old error calculation>>
                Matrix<T> errorTable = unitOut[unitOut.Length - 1];
                errorTable = (errorTable - desiredOutputData);
                errorTable.ApplyFunction((T x) => x * (dynamic)x);
                Console.WriteLine("Error Found");
                Matrix<T>.EnumerateMatrix(errorTable.GetMatrix());*/

            }

            void CalculateDelta()
            {
                for (int i = layerModel.Length - 1; i >= 0; i--)
                {
                    if (i == layerModel.Length - 1)//Output Layer
                    {
                        //since i indicates last index of each array,
                        T[,] output = unitOut[i].GetMatrix();
                        T[,] desiredOutput = desiredOutputData.GetMatrix();
                        T[,] errorDifferential = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunctionPrime);//d error / d output f(z)

                        Matrix<T> errorDifferentialMatrix = new Matrix<T>(errorDifferential);
                        Matrix<T> activationDifferentialMatrix = unitSum[i].DeepCopy();
                        activationDifferentialMatrix.ApplyFunction(activationPrimeFunctions[i]);//d output f(z) / d output z
                        Matrix<T> outputLayerDelta = errorDifferentialMatrix ^ activationDifferentialMatrix;// d error / d z
                        unitDelta[i] = outputLayerDelta.DeepCopy();


                        /* <<Old delta calculation starter>>
                        Matrix<T> errorDiff = unitOut[i].DeepCopy();
                        errorDiff = (dynamic)2 * (errorDiff - desiredOutputData);//d error / d output f(z)

                        Matrix<T> actFDiff = unitSum[i].DeepCopy();
                        actFDiff.ApplyFunction(activationPrimeFunctions[i]);//d output f(z) / d output z
                        T[,] ed = errorDiff.GetMatrix();
                        T[,] fd = actFDiff.GetMatrix();
                        T[,] outD = new T[ed.GetLength(0), ed.GetLength(1)];
                        for (int j = 0; j < ed.GetLength(0); j++)
                        {
                            for (int k = 0; k < ed.GetLength(1); k++)
                            {
                                outD[j, k] = (dynamic)ed[j, k] * fd[j, k];
                            }
                        }
                        unitDelta[i] = new Matrix<T>(outD);*/
                    }
                    else
                    {
                        Matrix<T> foreDelta = unitDelta[i + 1].DeepCopy();
                        Matrix<T> tempWeight = weights[i].DeepCopy();
                        int currentUnitCount = tempWeight.GetMatrix().GetLength(0);
                        Matrix<T> tempDelta = new Matrix<T>(1, currentUnitCount);
                        foreDelta.InverseByDiagonalLine();
                        tempDelta = tempWeight * foreDelta;

                        tempDelta.InverseByDiagonalLine();
                        Matrix<T> actFDiff = unitSum[i].DeepCopy();
                        actFDiff.ApplyFunction(activationPrimeFunctions[i]);
                        unitDelta[i] = tempDelta ^ actFDiff;
                    }
                }
            }

            void BackPropagate()
            {
                Console.WriteLine("Backpropagating...");
                for (int i = layerModel.Length - 1; i > 0; i--)
                {
                    T[,] tempWeights = weights[i - 1].GetMatrix();
                    T[,] tempUnitV = unitOut[i - 1].GetMatrix();
                    T[,] tempDelta = unitDelta[i].GetMatrix();
                    for (int j = 0; j < tempWeights.GetLength(0); j++)
                    {
                        for (int k = 0; k < tempWeights.GetLength(1); k++)
                        {
                            tempWeights[j, k] = tempWeights[j, k] - (dynamic)learningRate * tempDelta[0, k] * tempUnitV[0, j];//---------!!!---------assuring input of only one data
                        }
                    }
                    weights[i - 1] = new Matrix<T>(tempWeights);

                    biases[i - 1] = biases[i - 1] - (learningRate * unitDelta[i]);
                }
                Console.WriteLine("Back Prop Done.");
            }
        }

        public void Train_Silent(Matrix<T> inputData, Matrix<T> desiredOutputData)
        {
            RunCycle_Silent();

            void RunCycle_Silent()
            {
                FeedForward();
                CalculateDelta();
                BackPropagate();
            }

            void FeedForward()
            {
                for (int i = 0; i < layerModel.Length; i++)
                {
                    if (i == 0)
                    {
                        unitOut[i] = inputData;
                    }
                    else
                    {
                        unitSum[i] = (unitOut[i - 1] * weights[i - 1]) + biases[i - 1];
                        unitOut[i] = unitSum[i].DeepCopy();
                        unitOut[i].ApplyFunction(activationFunctions[i]);
                    }
                }
            }

            void CalculateDelta()
            {
                for (int i = layerModel.Length - 1; i >= 0; i--)
                {
                    if (i == layerModel.Length - 1)//Output Layer
                    {
                        //since i indicates last index of each array,
                        T[,] output = unitOut[i].GetMatrix();
                        T[,] desiredOutput = desiredOutputData.GetMatrix();
                        T[,] errorDifferential = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunctionPrime);//d error / d output f(z)

                        Matrix<T> errorDifferentialMatrix = new Matrix<T>(errorDifferential);
                        Matrix<T> activationDifferentialMatrix = unitSum[i].DeepCopy();
                        activationDifferentialMatrix.ApplyFunction(activationPrimeFunctions[i]);//d output f(z) / d output z
                        Matrix<T> outputLayerDelta = errorDifferentialMatrix ^ activationDifferentialMatrix;// d error / d z
                        unitDelta[i] = outputLayerDelta.DeepCopy();
                    }
                    else
                    {
                        Matrix<T> foreDelta = unitDelta[i + 1].DeepCopy();
                        Matrix<T> tempWeight = weights[i].DeepCopy();
                        int currentUnitCount = tempWeight.GetMatrix().GetLength(0);
                        Matrix<T> tempDelta = new Matrix<T>(1, currentUnitCount);
                        foreDelta.InverseByDiagonalLine();
                        tempDelta = tempWeight * foreDelta;

                        tempDelta.InverseByDiagonalLine();
                        Matrix<T> actFDiff = unitSum[i].DeepCopy();
                        actFDiff.ApplyFunction(activationPrimeFunctions[i]);
                        unitDelta[i] = tempDelta ^ actFDiff;
                    }
                }
            }

            void BackPropagate()
            {
                for (int i = layerModel.Length - 1; i > 0; i--)
                {
                    T[,] tempWeights = weights[i - 1].GetMatrix();
                    T[,] tempUnitV = unitOut[i - 1].GetMatrix();
                    T[,] tempDelta = unitDelta[i].GetMatrix();
                    for (int j = 0; j < tempWeights.GetLength(0); j++)
                    {
                        for (int k = 0; k < tempWeights.GetLength(1); k++)
                        {
                            tempWeights[j, k] = tempWeights[j, k] - (dynamic)learningRate * tempDelta[0, k] * tempUnitV[0, j];
                        }
                    }
                    weights[i - 1] = new Matrix<T>(tempWeights);

                    biases[i - 1] = biases[i - 1] - (learningRate * unitDelta[i]);
                }
            }
        }

        public void Train_Batch(Matrix<T> inputData, Matrix<T> desiredOutputData)//Delta, BackProp is renewed
        {
            RunCycle();

            void RunCycle()
            {
                FeedForward();
                FindError();
                CalculateDelta();
                BackPropagate();
            }

            void FeedForward()
            {
                Console.WriteLine("\nFeeding Forward...");
                for (int i = 0; i < layerModel.Length; i++)
                {
                    if (i == 0)//Input Layer
                    {
                        unitOut[i] = inputData;
                        Console.WriteLine("\nInput");
                        Matrix<T>.EnumerateMatrix(unitOut[i].GetMatrix());
                    }
                    else
                    {
                        unitSum[i] = (unitOut[i - 1] * weights[i - 1]) & biases[i - 1];
                        unitOut[i] = unitSum[i].DeepCopy();
                        unitOut[i].ApplyFunction(activationFunctions[i]);
                    }
                }
                Console.WriteLine("Feed Forward Output");
                Matrix<T>.EnumerateMatrix(unitOut[unitOut.Length - 1].GetMatrix());
            }

            void FindError()
            {
                T[,] output = unitOut[unitOut.Length - 1].GetMatrix();
                T[,] desiredOutput = desiredOutputData.GetMatrix();
                T[,] errortable = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunction);
                Console.WriteLine("Error Found");
                Matrix<T>.EnumerateMatrix(errortable);
            }

            void CalculateDelta()
            {
                for (int i = layerModel.Length - 1; i >= 0; i--)
                {
                    if (i == layerModel.Length - 1)//Output Layer
                    {
                        //since i indicates last index of each array,
                        T[,] output = unitOut[i].GetMatrix();
                        T[,] desiredOutput = desiredOutputData.GetMatrix();
                        T[,] errorDifferential = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunctionPrime);//d error / d output f(z)

                        Matrix<T> errorDifferentialMatrix = new Matrix<T>(errorDifferential);
                        Matrix<T> activationDifferentialMatrix = unitSum[i].DeepCopy();
                        activationDifferentialMatrix.ApplyFunction(activationPrimeFunctions[i]);//d output f(z) / d output z
                        Matrix<T> outputLayerDelta = errorDifferentialMatrix ^ activationDifferentialMatrix;// d error / d z
                        unitDelta[i] = outputLayerDelta.DeepCopy();
                    }
                    else
                    {
                        Matrix<T> foreDelta = unitDelta[i + 1].DeepCopy();
                        Matrix<T> tempWeight = weights[i].DeepCopy();
                        int currentUnitCount = tempWeight.GetMatrix().GetLength(0);
                        Matrix<T> tempDelta = new Matrix<T>(1, currentUnitCount);
                        foreDelta.InverseByDiagonalLine();
                        tempDelta = tempWeight * foreDelta;

                        tempDelta.InverseByDiagonalLine();
                        Matrix<T> actFDiff = unitSum[i].DeepCopy();
                        actFDiff.ApplyFunction(activationPrimeFunctions[i]);
                        unitDelta[i] = tempDelta % actFDiff;
                    }
                }
            }

            void BackPropagate()
            {
                int sampleNumber = inputData.GetMatrix().GetLength(0);
                Console.WriteLine("Backpropagating...");
                for (int i = layerModel.Length - 1; i > 0; i--)
                {
                    T[,] tempWeights = weights[i - 1].GetMatrix();
                    T[,] tempUnitV = unitOut[i - 1].GetMatrix();
                    T[,] tempDelta = unitDelta[i].GetMatrix();

                    T[,] tempWeightChange = new T[tempWeights.GetLength(0), tempWeights.GetLength(1)];
                    for (int x = 0; x < sampleNumber; x++)
                    {
                        for (int j = 0; j < tempWeights.GetLength(0); j++)
                        {
                            for (int k = 0; k < tempWeights.GetLength(1); k++)
                            {
                                tempWeightChange[j, k] = tempWeightChange[j, k] - (dynamic)learningRate * tempDelta[x, k] * tempUnitV[x, j];
                            }
                        }
                    }
                    weights[i - 1] = weights[i - 1] + new Matrix<T>(tempWeightChange);

                    T[,] tempBiasChange = new T[1, layerModel[i]];
                    for (int j = 0; j < tempDelta.GetLength(0); j++)
                    {
                        for (int k = 0; k < tempDelta.GetLength(1); k++)
                        {
                            tempBiasChange[0, k] = tempBiasChange[0, k] - (dynamic)learningRate * tempDelta[j, k];
                        }
                    }
                    biases[i - 1] = biases[i - 1] + new Matrix<T>(tempBiasChange);
                }
                Console.WriteLine("Back Prop Done.");
            }
        }

        public void Train_BatchSilent(Matrix<T> inputData, Matrix<T> desiredOutputData)
        {
            RunCycle();

            void RunCycle()
            {
                FeedForward();
                //==============================================================================================================================This must be implemented to every training func
                T[,] output = unitOut[unitOut.Length - 1].GetMatrix();
                T[,] desiredOutput = desiredOutputData.GetMatrix();
                T[,] errortable = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunction);
                double avgError = 0;
                for (int i = 0; i < errortable.GetLength(0); i++)
                {
                    avgError += (double)Convert.ChangeType(errortable[i, 0], typeof(double));
                }
                if (double.IsNaN(avgError))
                {
                    Console.WriteLine("Error was NaN, It may be diverged.");
                    Console.WriteLine("Erorr was too small or big and experienced overflow.");
                    Console.WriteLine("Pressing any key will continue training, but result would be useless");
                    Console.ReadKey();
                }
                else
                {
                    //Console.WriteLine(avgError / errortable.GetLength(0));
                }
                //==============================================================================================================================
                CalculateDelta();
                BackPropagate();
            }

            void FeedForward()
            {
                for (int i = 0; i < layerModel.Length; i++)
                {
                    if (i == 0)//Input Layer
                    {
                        unitOut[i] = inputData;
                    }
                    else
                    {
                        unitSum[i] = (unitOut[i - 1] * weights[i - 1]) & biases[i - 1];
                        unitOut[i] = unitSum[i].DeepCopy();
                        unitOut[i].ApplyFunction(activationFunctions[i]);
                    }
                }
            }

            void CalculateDelta()
            {
                for (int i = layerModel.Length - 1; i >= 0; i--)
                {
                    if (i == layerModel.Length - 1)//Output Layer
                    {
                        //since i indicates last index of each array,
                        T[,] output = unitOut[i].GetMatrix();
                        T[,] desiredOutput = desiredOutputData.GetMatrix();
                        T[,] errorDifferential = Matrix<T>.TwoInputCalculation(output, desiredOutput, lossFunctionPrime);//d error / d output f(z)

                        Matrix<T> errorDifferentialMatrix = new Matrix<T>(errorDifferential);
                        Matrix<T> activationDifferentialMatrix = unitSum[i].DeepCopy();
                        activationDifferentialMatrix.ApplyFunction(activationPrimeFunctions[i]);//d output f(z) / d output z
                        Matrix<T> outputLayerDelta = errorDifferentialMatrix ^ activationDifferentialMatrix;// d error / d z
                        unitDelta[i] = outputLayerDelta.DeepCopy();
                    }
                    else
                    {
                        Matrix<T> foreDelta = unitDelta[i + 1].DeepCopy();
                        Matrix<T> tempWeight = weights[i].DeepCopy();
                        int currentUnitCount = tempWeight.GetMatrix().GetLength(0);
                        Matrix<T> tempDelta = new Matrix<T>(1, currentUnitCount);
                        foreDelta.InverseByDiagonalLine();
                        tempDelta = tempWeight * foreDelta;

                        tempDelta.InverseByDiagonalLine();
                        Matrix<T> actFDiff = unitSum[i].DeepCopy();
                        actFDiff.ApplyFunction(activationPrimeFunctions[i]);
                        unitDelta[i] = tempDelta % actFDiff;
                    }
                }
            }

            void BackPropagate()
            {
                int sampleNumber = inputData.GetMatrix().GetLength(0);
                for (int i = layerModel.Length - 1; i > 0; i--)
                {
                    T[,] tempWeights = weights[i - 1].GetMatrix();
                    T[,] tempUnitV = unitOut[i - 1].GetMatrix();
                    T[,] tempDelta = unitDelta[i].GetMatrix();

                    T[,] tempWeightChange = new T[tempWeights.GetLength(0), tempWeights.GetLength(1)];
                    for (int x = 0; x < sampleNumber; x++)
                    {
                        for (int j = 0; j < tempWeights.GetLength(0); j++)
                        {
                            for (int k = 0; k < tempWeights.GetLength(1); k++)
                            {
                                tempWeightChange[j, k] = tempWeightChange[j, k] - (dynamic)learningRate * tempDelta[x, k] * tempUnitV[x, j];
                            }
                        }
                    }
                    weights[i - 1] = weights[i - 1] + new Matrix<T>(tempWeightChange);

                    T[,] tempBiasChange = new T[1, layerModel[i]];
                    for (int j = 0; j < tempDelta.GetLength(0); j++)
                    {
                        for (int k = 0; k < tempDelta.GetLength(1); k++)
                        {
                            tempBiasChange[0, k] = tempBiasChange[0, k] - (dynamic)learningRate * tempDelta[j, k];
                        }
                    }
                    biases[i - 1] = biases[i - 1] + new Matrix<T>(tempBiasChange);
                }
            }
        }

        public void Predict(Matrix<T> inputData)
        {
            Console.WriteLine("\nFeeding Forward...");
            for (int i = 0; i < layerModel.Length; i++)
            {
                if (i == 0)//Input Layer
                {
                    unitOut[i] = inputData;
                    Console.WriteLine("\nInput");
                    Matrix<T>.EnumerateMatrix(unitOut[i].GetMatrix());
                }
                else
                {
                    unitSum[i] = (unitOut[i - 1] * weights[i - 1]) + biases[i - 1];
                    unitOut[i] = unitSum[i].DeepCopy();
                    unitOut[i].ApplyFunction(activationFunctions[i]);
                }
            }
            Console.WriteLine("Feed Forward Output");
            Matrix<T>.EnumerateMatrix(unitOut[unitOut.Length - 1].GetMatrix());
        }

        public Matrix<T> GetPredict(Matrix<T> inputData)
        {
            for (int i = 0; i < layerModel.Length; i++)
            {
                if (i == 0)//Input Layer
                {
                    unitOut[i] = inputData;
                }
                else
                {
                    unitSum[i] = (unitOut[i - 1] * weights[i - 1]) + biases[i - 1];
                    unitOut[i] = unitSum[i].DeepCopy();
                    unitOut[i].ApplyFunction(activationFunctions[i]);
                }
            }
            return unitOut[unitOut.Length - 1];
        }

        public Matrix<T> GetBatchPredict(Matrix<T> inputData)
        {
            for (int i = 0; i < layerModel.Length; i++)
            {
                if (i == 0)//Input Layer
                {
                    unitOut[i] = inputData;
                }
                else
                {
                    unitSum[i] = (unitOut[i - 1] * weights[i - 1]) & biases[i - 1];
                    unitOut[i] = unitSum[i].DeepCopy();
                    unitOut[i].ApplyFunction(activationFunctions[i]);
                }
            }
            return unitOut[unitOut.Length - 1];
        }

        public void Debug()
        {
            Console.WriteLine("--------------------<Neural Network Debug Report>--------------------");
            Console.WriteLine("\n1.Pre-described info");
            Console.WriteLine("Mapping Range = {0} ~ {1}", mapMin, mapMax);
            Console.WriteLine("Learning Rate = {0}", learningRate);
            Console.WriteLine("loss Function = {0}", lossFunctionAssigner);

            Console.WriteLine("\n2.Pre-designed info");
            Console.WriteLine("LayerCount : {0}", layerModel.Length);
            for (int i = 0; i < layerModel.Length; i++)
            {
                Console.WriteLine("Layer {0} unit count : {1}, activation Function = {2}", i, layerModel[i], activationFunctionAssigner[i]);
            }
            for (int i = 0; i < layerModel.Length - 1; i++)
            {
                Console.WriteLine("\nConnectionLayer {0} : L{0}-L{1}", i, i + 1);
                Console.WriteLine("\nWeight");
                Matrix<T>.EnumerateMatrix(weights[i].GetMatrix());
                Console.WriteLine("\nBias");
                Matrix<T>.EnumerateMatrix(biases[i].GetMatrix());
            }

            Console.WriteLine("\n3. Latest train info");
            for (int i = 0; i < layerModel.Length; i++)
            {
                Console.WriteLine("\nLayer {0} train result", i);
                Console.WriteLine("\nunit Sum");
                Matrix<T>.EnumerateMatrix(unitSum[i].GetMatrix());
                Console.WriteLine("\nunit Value=func(sum)");
                Matrix<T>.EnumerateMatrix(unitOut[i].GetMatrix());
                Console.WriteLine("\nunit Delta");
                Matrix<T>.EnumerateMatrix(unitDelta[i].GetMatrix());
            }
            Console.WriteLine("--------------------<Neural Network Debug Report>--------------------");
        }

        public void Log(Matrix<T> inputData, Matrix<T> desiredOutputData)
        {
            //Train
            //Log
            //Train
            //Log
        }

        public void SaveModelData()
        {
            object[] data = new object[15];
            data[0] = layerModel;
            data[1] = weights;
            data[2] = biases;
            data[3] = unitSum;
            data[4] = unitOut;
            data[5] = unitDelta;
            data[6] = mapMax;
            data[7] = mapMin;
            data[8] = learningRate;
            data[9] = activationFunctionAssigner;
            data[10] = activationFunctions;
            data[11] = activationPrimeFunctions;
            data[12] = lossFunctionAssigner;
            data[13] = lossFunction;
            data[14] = lossFunctionPrime;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(ms, data);
                System.IO.File.WriteAllBytes(path, ms.ToArray());
            }
            Console.WriteLine("Model data saved.");
        }

        public void SaveModelData_Xml()
        {
            ModelData<T> saveData = new ModelData<T>();

            saveData.layerModel = layerModel;
            saveData.mapMin = mapMin;
            saveData.mapMax = mapMax;
            saveData.learningRate = learningRate;
            saveData.activationFunctionAssigner = activationFunctionAssigner;
            saveData.lossFunctionAssigner = lossFunctionAssigner;
            saveData.SetMatrix(weights, biases);

            using (StreamWriter wr = new StreamWriter(Environment.CurrentDirectory + @"\ModelData.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ModelData<T>));
                xs.Serialize(wr, saveData);
            }
        }

        public void LoadModelData()
        {
            if (System.IO.File.Exists(path) == true)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    byte[] sData = System.IO.File.ReadAllBytes(path);
                    ms.Write(sData, 0, sData.Length);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
                    object[] data = (object[])bf.Deserialize(ms);
                    layerModel = (int[])data[0];
                    weights = (Matrix<T>[])data[1];
                    biases = (Matrix<T>[])data[2];
                    unitSum = (Matrix<T>[])data[3];
                    unitOut = (Matrix<T>[])data[4];
                    unitDelta = (Matrix<T>[])data[5];
                    mapMax = (T)data[6];
                    mapMin = (T)data[7];
                    learningRate = (T)data[8];
                    activationFunctionAssigner = (string[])data[9];
                    activationFunctions = (Func<T, T>[])data[10];
                    activationPrimeFunctions = (Func<T, T>[])data[11];
                    lossFunctionAssigner = (string)data[12];
                    lossFunction = (Func<T, T, T>)data[13];
                    lossFunctionPrime = (Func<T, T, T>)data[14];
                }
                Console.WriteLine("Model data loaded.");
            }
            else
            {
                Console.WriteLine("Model data not found at {0}", path);
            }
        }

        public void LoadModelData_Xml()
        {
            ModelData<T> saveData = new ModelData<T>();

            saveData.layerModel = layerModel;
            saveData.mapMin = mapMin;
            saveData.mapMax = mapMax;
            saveData.learningRate = learningRate;
            saveData.activationFunctionAssigner = activationFunctionAssigner;
            saveData.lossFunctionAssigner = lossFunctionAssigner;
            saveData.SetMatrix(weights, biases);

            using (var reader = new StreamReader(Environment.CurrentDirectory + @"\ModelData.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ModelData<T>));
                ModelData<T> importedData = (ModelData<T>)xs.Deserialize(reader);
                layerModel = importedData.layerModel;
                mapMin = importedData.mapMin;
                mapMax = importedData.mapMax;
                learningRate = importedData.learningRate;
                activationFunctionAssigner = importedData.activationFunctionAssigner;
                lossFunctionAssigner = importedData.lossFunctionAssigner;
                Initialize_Silent();
                importedData.GetMatrix(out weights, out biases);
            }
        }
    }

    public class ModelData<T>
    {
        public int[] layerModel;
        private Matrix<T>[] weights;
        private Matrix<T>[] biases;
        public T mapMin, mapMax, learningRate;
        public string[] activationFunctionAssigner;
        public string lossFunctionAssigner;
        public T[][][] w, b;

        public void SetMatrix(Matrix<T>[] _weights, Matrix<T>[] _biases)
        {
            weights = _weights;
            biases = _biases;
            ConvertMatrixToJaggedArray();
        }

        public void GetMatrix(out Matrix<T>[] _weights, out Matrix<T>[] _biases)
        {
            ConvertJaggedArrayToMatrix();
            _weights = weights;
            _biases = biases;
        }

        private void ConvertMatrixToJaggedArray()
        {
            w = new T[weights.Length][][];
            b = new T[biases.Length][][];
            for (int i = 0; i < w.Length; i++)
            {
                T[,] temp = weights[i].GetMatrix();
                w[i] = new T[temp.GetLength(0)][];
                for (int y = 0; y < temp.GetLength(0); y++)
                {
                    w[i][y] = new T[temp.GetLength(1)];
                    for (int x = 0; x < temp.GetLength(1); x++)
                    {
                        w[i][y][x] = temp[y, x];
                    }
                }
            }

            for (int i = 0; i < b.Length; i++)
            {
                T[,] temp = biases[i].GetMatrix();
                b[i] = new T[temp.GetLength(0)][];
                for (int y = 0; y < temp.GetLength(0); y++)
                {
                    b[i][y] = new T[temp.GetLength(1)];
                    for (int x = 0; x < temp.GetLength(1); x++)
                    {
                        b[i][y][x] = temp[y, x];
                    }
                }
            }
        }

        private void ConvertJaggedArrayToMatrix()
        {
            weights = new Matrix<T>[w.Length];
            biases = new Matrix<T>[b.Length];
            for (int i = 0; i < w.Length; i++)
            {
                T[,] temp = new T[w[i].Length, w[i][0].Length];
                for (int y = 0; y < temp.GetLength(0); y++)
                {
                    for (int x = 0; x < temp.GetLength(1); x++)
                    {
                        temp[y, x] = w[i][y][x];
                    }
                }
                weights[i] = new Matrix<T>(temp);
            }

            for (int i = 0; i < b.Length; i++)
            {
                T[,] temp = new T[b[i].Length, b[i][0].Length];
                for (int y = 0; y < temp.GetLength(0); y++)
                {
                    for (int x = 0; x < temp.GetLength(1); x++)
                    {
                        temp[y, x] = b[i][y][x];
                    }
                }
                biases[i] = new Matrix<T>(temp);
            }
        }
    }
}