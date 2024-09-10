using System;

namespace LearningModel.Base
{
    [Serializable]
    class LearningModelBase<T>
    {
        public static T MeanSquareError(T x, T y)
        {
            return (x - (dynamic)y) * (x - (dynamic)y);
        }

        public static T MeanSquareErrorPrime(T x, T y)
        {
            return 2 * (x - (dynamic)y);
        }

        public static T Sigmoid(T x)
        {
            return 1 / (1 + Math.Pow(Math.E, -(dynamic)x));
        }

        public static T SigmoidPrime(T x)
        {
            return Sigmoid(x) * (1 - (dynamic)Sigmoid(x));
        }

        public static T Tanh(T x)
        {
            return 2 * Sigmoid(2 * (dynamic)x) - 1;
        }

        public static T TanhPrime(T x)
        {
            return 1 - Tanh(x) * (dynamic)Tanh(x);
        }

        public static T ReLu(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)x : 0;
        }

        public static T ReLuPrime(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)1 : 0;
        }

        public static T LeakyReLu(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)x : 0.01 * (dynamic)x;
        }

        public static T LeakyReLuPrime(T x)
        {
            return ((dynamic)x >= 0) ? (dynamic)1 : 0.01;
        }
    }
}