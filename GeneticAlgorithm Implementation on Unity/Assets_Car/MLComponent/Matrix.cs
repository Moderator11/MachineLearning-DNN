using System;

namespace Matrices//Written by Soomin Park, In 2022-04-25 11:57 am. Used material : http://matrixmultiplication.xyz/
{
    [Serializable]
    public class Matrix<T>
    {
        T[,] matrix = null;

        public Matrix(T[,] _matrix = null)
        {
            matrix = _matrix;
        }

        public Matrix(int h, int w)
        {
            matrix = new T[h, w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    //matrix[i, j] = (T)Convert.ChangeType(0, typeof(T)
                    //matrix[i, j] = (T)Activator.CreateInstance(typeof(T), 0);
                    matrix[i, j] = default(T);
                }
            }
        }

        public T[,] GetMatrix()
        {
            return matrix;
        }

        public Matrix<T> DeepCopy()
        {
            Matrix<T> newCopy = new Matrix<T>();
            newCopy.matrix = new T[matrix.GetLength(0), matrix.GetLength(1)];
            Array.Copy(matrix, newCopy.matrix, matrix.Length);
            return newCopy;
        }

        static Random random = new Random();

        public void Map(T low, T high)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //matrix[i, j] = (T)(((dynamic)high - low) * (T)Convert.ChangeType(random.NextDouble(), typeof(T)) + low);
                    matrix[i, j] = (T)Convert.ChangeType(((dynamic)high - low) * random.NextDouble() + low, typeof(T));
                }
            }
        }

        public void ApplyFunction(Func<T, T> function)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = function(matrix[i, j]);
                }
            }
        }

        public void InverseByDiagonalLine()
        {
            int ah = matrix.GetLength(0);
            int aw = matrix.GetLength(1);
            T[,] result = new T[aw, ah];
            for (int i = 0; i < aw; i++)
            {
                for (int j = 0; j < ah; j++)
                {
                    result[i, j] = matrix[j, i];
                }
            }
            matrix = result;
        }

        public static void EnumerateMatrix(T[,] matrix)
        {
            if (matrix != null)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        Console.Write("[{0}]", matrix[i, j]);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Matrix is null");
            }
        }

        public static T[,] TwoInputCalculation(T[,] a, T[,] b, Func<T, T, T> func)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (ah == bh && aw == bw)
            {
                T[,] result = new T[ah, aw];
                for (int i = 0; i < ah; i++)
                {
                    for (int j = 0; j < aw; j++)
                    {
                        result[i, j] = func(a[i, j], b[i, j]);
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static T[,] AddMatrix(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (ah == bh && aw == bw)
            {
                T[,] result = new T[ah, aw];
                for (int i = 0; i < ah; i++)
                {
                    for (int j = 0; j < aw; j++)
                    {
                        result[i, j] = (dynamic)a[i, j] + b[i, j];
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static T[,] SubtractMatrix(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (ah == bh && aw == bw)
            {
                T[,] result = new T[ah, aw];
                for (int i = 0; i < ah; i++)
                {
                    for (int j = 0; j < aw; j++)
                    {
                        result[i, j] = (dynamic)a[i, j] - b[i, j];
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static T[,] MultiplyMatrix(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (aw == bh)//Multiplication available
            {
                T[,] result = new T[ah, bw];
                for (int i = 0; i < ah; i++)//loop through a's height
                {
                    for (int j = 0; j < aw; j++)//loop through a's width which is equal to b's height
                    {
                        for (int k = 0; k < bw; k++)//loop through b's width
                        {
                            result[i, k] += (dynamic)a[i, j] * b[j, k];
                        }
                    }
                }

                return result;
            }
            else//Multiplication not available
            {
                throw new Exception();
            }
        }

        public static T[,] MultiplyConstant(T a, T[,] b)
        {
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            T[,] result = new T[bh, bw];
            for (int i = 0; i < bh; i++)
            {
                for (int j = 0; j < bw; j++)
                {
                    result[i, j] = (dynamic)a * b[i, j];
                }
            }
            return result;
        }

        public static T[,] DivideConstant(T a, T[,] b)
        {
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            T[,] result = new T[bh, bw];
            for (int i = 0; i < bh; i++)
            {
                for (int j = 0; j < bw; j++)
                {
                    result[i, j] = (dynamic)a / b[i, j];
                }
            }
            return result;
        }

        public static T[,] DivideConstant(T[,] a, T b)
        {
            int bh = a.GetLength(0);
            int bw = a.GetLength(1);
            T[,] result = new T[bh, bw];
            for (int i = 0; i < bh; i++)
            {
                for (int j = 0; j < bw; j++)
                {
                    result[i, j] = a[i, j] / (dynamic)b;
                }
            }
            return result;
        }

        public static T[,] MultiplyEachtoEach(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (ah == bh && aw == bw)
            {
                T[,] result = new T[ah, aw];
                for (int i = 0; i < ah; i++)
                {
                    for (int j = 0; j < aw; j++)
                    {
                        result[i, j] = (dynamic)a[i, j] * b[i, j];
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static T[,] BroadcastingAdd(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (aw == bw)
            {
                T[,] result;
                if (ah >= bh)
                {
                    result = new T[ah, aw];
                    for (int i = 0; i < ah; i++)
                    {
                        for (int j = 0; j < aw; j++)
                        {
                            result[i, j] = a[i, j] + (dynamic)b[0, j];
                        }
                    }
                }
                else
                {
                    result = new T[bh, aw];
                    for (int i = 0; i < bh; i++)
                    {
                        for (int j = 0; j < aw; j++)
                        {
                            result[i, j] = (dynamic)a[0, j] + b[i, j];
                        }
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static T[,] BroadcastingSubtract(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (aw == bw)
            {
                T[,] result;
                if (ah >= bh)
                {
                    result = new T[ah, aw];
                    for (int i = 0; i < ah; i++)
                    {
                        for (int j = 0; j < aw; j++)
                        {
                            result[i, j] = a[i, j] - (dynamic)b[0, j];
                        }
                    }
                }
                else
                {
                    result = new T[bh, aw];
                    for (int i = 0; i < bh; i++)
                    {
                        for (int j = 0; j < aw; j++)
                        {
                            result[i, j] = (dynamic)a[0, j] - b[i, j];
                        }
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static T[,] BroadcastingMultiplyEachtoEach(T[,] a, T[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (aw == bw)
            {
                T[,] result;
                if (ah >= bh)
                {
                    result = new T[ah, aw];
                    for (int i = 0; i < ah; i++)
                    {
                        for (int j = 0; j < aw; j++)
                        {
                            result[i, j] = a[i, j] * (dynamic)b[0, j];
                        }
                    }
                }
                else
                {
                    result = new T[bh, aw];
                    for (int i = 0; i < bh; i++)
                    {
                        for (int j = 0; j < aw; j++)
                        {
                            result[i, j] = (dynamic)a[0, j] * b[i, j];
                        }
                    }
                }
                return result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(AddMatrix(a.matrix, b.matrix));
        }

        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(SubtractMatrix(a.matrix, b.matrix));
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(MultiplyMatrix(a.matrix, b.matrix));
        }

        public static Matrix<T> operator *(T a, Matrix<T> b)
        {
            return new Matrix<T>(MultiplyConstant(a, b.matrix));
        }

        public static Matrix<T> operator *(Matrix<T> b, T a)
        {
            return new Matrix<T>(MultiplyConstant(a, b.matrix));
        }

        public static Matrix<T> operator /(Matrix<T> a, T b)
        {
            return new Matrix<T>(DivideConstant(a.matrix, b));
        }

        public static Matrix<T> operator /(T a, Matrix<T> b)
        {
            return new Matrix<T>(DivideConstant(a, b.matrix));
        }

        public static Matrix<T> operator ^(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(MultiplyEachtoEach(a.matrix, b.matrix));
        }

        public static Matrix<T> operator &(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(BroadcastingAdd(a.matrix, b.matrix));
        }

        public static Matrix<T> operator |(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(BroadcastingSubtract(a.matrix, b.matrix));
        }

        public static Matrix<T> operator %(Matrix<T> a, Matrix<T> b)
        {
            return new Matrix<T>(BroadcastingMultiplyEachtoEach(a.matrix, b.matrix));
        }
    }
    /*public class Matrix
    {
        int[,] matrix;

        public Matrix(int[,] _matrix = null)
        {
            matrix = _matrix;
        }

        public int[,] GetArrayedMatrix()
        {
            return matrix;
        }

        public static int[,] AddArrayedMatrix(int[,] a, int[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (ah == bh && aw == bw)
            {
                int[,] result = new int[ah, aw];
                for (int i = 0; i < ah; i++)
                {
                    for (int j = 0; j < aw; j++)
                    {
                        result[i, j] = a[i, j] + b[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static int[,] SubtractArrayedMatrix(int[,] a, int[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (ah == bh && aw == bw)
            {
                int[,] result = new int[ah, aw];
                for (int i = 0; i < ah; i++)
                {
                    for (int j = 0; j < aw; j++)
                    {
                        result[i, j] = a[i, j] - b[i, j];
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static int[,] MultiplyArrayedMatrix(int[,] a, int[,] b)
        {
            int ah = a.GetLength(0);
            int aw = a.GetLength(1);
            int bh = b.GetLength(0);
            int bw = b.GetLength(1);
            if (aw == bh)//Multiplication available
            {
                int[,] result = new int[ah, bw];
                for (int i = 0; i < ah; i++)//loop through a's height
                {
                    for (int j = 0; j < aw; j++)//loop through a's width which is equal to b's height
                    {
                        for (int k = 0; k < bw; k++)//loop through b's width
                        {
                            result[i, k] += a[i, j] * b[j, k];
                        }
                    }
                }

                return result;
            }
            else//Multiplication not available
            {
                return null;
            }
        }

        public static void EnumerateMatrix(int[,] matrix)
        {
            if (matrix != null)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        Console.Write("[{0}]", matrix[i, j]);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Matrix is null");
            }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            return new Matrix(AddArrayedMatrix(a.matrix, b.matrix));
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            return new Matrix(SubtractArrayedMatrix(a.matrix, b.matrix));
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            return new Matrix(MultiplyArrayedMatrix(a.matrix, b.matrix));
        }

        public static void testc_()
        {
            int[,] a1 =
{
                {1,2},
                {3,4},
                {5,6}
            };

            int[,] a2 =
            {
                {1,2,3},
                {4,5,6},
            };

            int[,] a3 =
            {
                { 2,3,4},
                { 5,6,7}
            };

            Matrix a = new Matrix(a1);
            Matrix b = new Matrix(a2);
            Matrix c = new Matrix(a3);
            Console.WriteLine("a"); Matrix.EnumerateMatrix(a.matrix);
            Console.WriteLine("b"); Matrix.EnumerateMatrix(b.matrix);
            Console.WriteLine("c"); Matrix.EnumerateMatrix(c.matrix);
            Console.WriteLine("b+c"); Matrix.EnumerateMatrix((b + c).GetArrayedMatrix());
            Console.WriteLine("b-c"); Matrix.EnumerateMatrix((b - c).GetArrayedMatrix());
            Console.WriteLine("a*b"); Matrix.EnumerateMatrix((a * b).GetArrayedMatrix());
        }
    }*/
}