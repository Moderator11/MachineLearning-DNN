using System;
using System.Windows.Forms.VisualStyles;
using Matrices;
using Convolution;

namespace LearningModel//Written by Soomin Park, In ‎2023‎-02‎-22 wednesday, 12:47:18 am.
{
    /// <summary>CNN Learning Model, T must be numeric</summary>
    class LearningModel<T> : Base.LearningModelBase<T>
    {
        string path = Environment.CurrentDirectory + @"\Model-Data.mldat";

        int[] layerModel;
        string layerStructure = "Input=500x500--Padding=2--Kernel=5x5--Pooling=2x2--Padding=2--Kernel=5x5--Pooling=2x2--Flatten";
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

        Matrix<T>[] filters;

        public LearningModel(int[] _layerModel, string _layerStructure)
        {
            layerModel = _layerModel;
            layerStructure = _layerStructure;
        }
        //https://www.analyticsvidhya.com/blog/2020/10/what-is-the-convolutional-neural-network-architecture/

        struct Size
        {
            public int x;
            public int y;
            public Size(int _x, int _y)
            {
                x = _x;
                y = _y;
            }
        }

        public void Initialize()
        {
            string[] layerStructures = layerStructure.Split(new string[] { "--" }, StringSplitOptions.None);
            for (int i = 0; i < layerStructures.Length; i++)
            {
                string[] info = layerStructures[i].Split('=');
                switch(info[0])
                {
                    case "Input":
                        string[] inputData = info[1].Split('x');
                        int x = int.Parse(inputData[0]);
                        int y = int.Parse(inputData[1]);

                        break;
                    case "Padding":
                        break;
                    case "Kernel":
                        break;
                    case "Pooling":
                        break;
                    case "Flatten":
                        break;
                    default:
                        throw new Exception("Wrong layer structure format");
                }
            }
        }

        public void Train(Matrix<T> input)
        {
            Matrix<T> result = input.DeepCopy();
            T[,] feature1 = Conv2<T>.GetFeature(result.GetMatrix(), filters[0].GetMatrix(), 1);
            T[,] pooled1 = Conv2<T>.GetMaxPooling(feature1, 2, 2);
        }
    }
}