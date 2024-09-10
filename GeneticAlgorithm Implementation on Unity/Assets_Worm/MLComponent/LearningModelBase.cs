using System;

namespace LearningModel.Base
{
    class LearningModelBase<T>
    {
        public static T MeanSquareError(T x, T y)
        {
            return (T)((y - (dynamic)x) * (y - (dynamic)x));
        }
        /// <summary>Note that this is MSE derivative with respect to x</summary><param name="x">It will be considered as variable</param><param name="y">It will be considered as constant</param>
        public static T MeanSquareErrorPrime(T x, T y)
        {
            return (T)(- 2 * (y - (dynamic)x));
        }

        public static T Sigmoid(T x)
        {
            return (T)(1 / (1 + Math.Pow(Math.E, -(dynamic)x)));
        }

        public static T SigmoidPrime(T x)
        {
            return (T)(Sigmoid(x) * (1 - (dynamic)Sigmoid(x)));
        }

        public static T Tanh(T x)
        {
            return (T)(2 * Sigmoid(2 * (dynamic)x) - 1);
        }

        public static T TanhPrime(T x)
        {
            return (T)(1 - Tanh(x) * (dynamic)Tanh(x));
        }

        public static T ReLu(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)x : (dynamic)0;
        }

        public static T ReLuPrime(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)1 : (dynamic)0;
        }

        public static T LeakyReLu(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)x : (dynamic)0.01f * (dynamic)x;
        }

        public static T LeakyReLuPrime(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)1 : (dynamic)0.01;
        }

        public static T[] Softmax(T[] x)
        {
            T sum = (dynamic)0;
            T[] res = new T[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                sum += Math.Pow(Math.E, (dynamic)x[i]);
            }
            for (int i = 0; i < x.Length; i++)
            {
                res[i] = Math.Pow(Math.E, (dynamic)x[i]) / sum;
            }
            return res;
        }

        public static T[] SoftmaxPrime(T[] x)
        {
            return null;
        }
    }
}