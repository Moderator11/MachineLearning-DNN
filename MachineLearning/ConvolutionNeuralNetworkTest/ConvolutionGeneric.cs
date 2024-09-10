using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convolution
{
    class Conv2<T>
    {
        public static T[,] GetZeroPadding(T[,] target, int count = 1)
        {
            T[,] padded = new T[target.GetLength(0) + 2 * count, target.GetLength(1) + 2 * count];
            for (int y = count; y < padded.GetLength(0) - count; y++)
            {
                for (int x = count; x < padded.GetLength(1) - count; x++)
                {
                    padded[y, x] = target[y - count, x - count];
                }
            }
            return padded;
        }

        public static T[,] GetFeature(T[,] target, T[,] filter, int stride = 1)
        {
            if (target.GetLength(0) < filter.GetLength(0) || target.GetLength(1) < filter.GetLength(1)) { throw new Exception("filter is bigger than target"); };
            if ((target.GetLength(0) - filter.GetLength(0)) % stride != 0 || (target.GetLength(1) - filter.GetLength(1)) % stride != 0) { throw new Exception("target image does not fit to stride factor"); };
            T[,] feature = new T[(target.GetLength(0) - filter.GetLength(0)) / stride + 1, (target.GetLength(1) - filter.GetLength(1)) / stride + 1];
            for (int y = 0; y < feature.GetLength(0); y++)
            {
                for (int x = 0; x < feature.GetLength(1); x++)
                {
                    for (int fy = 0; fy < filter.GetLength(0); fy++)
                    {
                        for (int fx = 0; fx < filter.GetLength(1); fx++)
                        {
                            feature[y, x] += (dynamic)target[fy + y * stride, fx + x * stride] * filter[fy, fx];
                        }
                    }
                }
            }
            return feature;
        }

        public static T[,] GetFeatureCropRest(T[,] target, T[,] filter, int stride = 1)
        {
            if (target.GetLength(0) < filter.GetLength(0) || target.GetLength(1) < filter.GetLength(1)) { throw new Exception("filter is bigger than target"); };
            T[,] feature = new T[(target.GetLength(0) - filter.GetLength(0)) / stride + 1, (target.GetLength(1) - filter.GetLength(1)) / stride + 1];
            for (int y = 0; y < feature.GetLength(0); y++)
            {
                for (int x = 0; x < feature.GetLength(1); x++)
                {
                    for (int fy = 0; fy < filter.GetLength(0); fy++)
                    {
                        for (int fx = 0; fx < filter.GetLength(1); fx++)
                        {
                            if (fy + y * stride < target.GetLength(0) && fx + x * stride < target.GetLength(1))
                            {
                                feature[y, x] += (dynamic)target[fy + y * stride, fx + x * stride] * filter[fy, fx];
                            }
                        }
                    }
                }
            }
            return feature;
        }

        public static T[,] GetMaxPooling(T[,] target, int filterSizeX, int filterSizeY)
        {
            bool isPoolable = target.GetLength(0) % filterSizeY == 0 && target.GetLength(1) % filterSizeX == 0;
            if (!isPoolable) { throw new Exception("cannot divide target XY with filterSizes"); };
            T[,] pooled = new T[target.GetLength(0) / filterSizeY, target.GetLength(1) / filterSizeX];
            for (int y = 0; y < pooled.GetLength(0); y++)
            {
                for (int x = 0; x < pooled.GetLength(1); x++)
                {
                    pooled[y, x] = target[y * filterSizeY, x * filterSizeX];
                    for (int fy = 0; fy < filterSizeY; fy++)
                    {
                        for (int fx = 0; fx < filterSizeX; fx++)
                        {
                            pooled[y, x]
                                = ((dynamic)pooled[y, x]
                                < target[fy + y * filterSizeY, fx + x * filterSizeX])
                                ? target[fy + y * filterSizeY, fx + x * filterSizeX]
                                : pooled[y, x];
                        }
                    }
                }
            }
            return pooled;
        }

        public static T[,] GetMaxPoolingCropRest(T[,] target, int filterSizeX, int filterSizeY)
        {
            T[,] pooled = new T[target.GetLength(0) / filterSizeY, target.GetLength(1) / filterSizeX];
            for (int y = 0; y < pooled.GetLength(0); y++)
            {
                for (int x = 0; x < pooled.GetLength(1); x++)
                {
                    pooled[y, x] = target[y * filterSizeY, x * filterSizeX];
                    for (int fy = 0; fy < filterSizeY; fy++)
                    {
                        for (int fx = 0; fx < filterSizeX; fx++)
                        {
                            if (fy + y * filterSizeY < target.GetLength(0) && fx + x * filterSizeX < target.GetLength(1))
                            {
                                pooled[y, x]
                                = ((dynamic)pooled[y, x]
                                < target[fy + y * filterSizeY, fx + x * filterSizeX])
                                ? target[fy + y * filterSizeY, fx + x * filterSizeX]
                                : pooled[y, x];
                            }
                        }

                    }
                }
            }
            return pooled;
        }

        public static T[,] GetAveragePooling(T[,] target, int filterSizeX, int filterSizeY)
        {
            bool isPoolable = target.GetLength(0) % filterSizeY == 0 && target.GetLength(1) % filterSizeX == 0;
            if (!isPoolable) { throw new Exception("cannot divide target XY with filterSizes"); };
            T[,] pooled = new T[target.GetLength(0) / filterSizeY, target.GetLength(1) / filterSizeX];
            for (int y = 0; y < pooled.GetLength(0); y++)
            {
                for (int x = 0; x < pooled.GetLength(1); x++)
                {
                    for (int fy = 0; fy < filterSizeY; fy++)
                    {
                        for (int fx = 0; fx < filterSizeX; fx++)
                        {
                            pooled[y, x] += (dynamic)target[fy + y * filterSizeY, fx + x * filterSizeX];
                        }
                    }
                    pooled[y, x] /= (dynamic)(T)Convert.ChangeType(filterSizeX, typeof(T)) * filterSizeY;
                }
            }
            return pooled;
        }

        public static T[,] GetAveragePoolingCropRest(T[,] target, int filterSizeX, int filterSizeY)
        {
            T[,] pooled = new T[target.GetLength(0) / filterSizeY, target.GetLength(1) / filterSizeX];
            for (int y = 0; y < pooled.GetLength(0); y++)
            {
                for (int x = 0; x < pooled.GetLength(1); x++)
                {
                    for (int fy = 0; fy < filterSizeY; fy++)
                    {
                        for (int fx = 0; fx < filterSizeX; fx++)
                        {
                            if (fy + y * filterSizeY < target.GetLength(0) && fx + x * filterSizeX < target.GetLength(1))
                            {
                                pooled[y, x] += (dynamic)target[fy + y * filterSizeY, fx + x * filterSizeX];
                            }
                        }
                    }
                    pooled[y, x] /= (dynamic)(T)Convert.ChangeType(filterSizeX, typeof(T)) * filterSizeY;
                }
            }
            return pooled;
        }

        public static T[,] InverseBy_Horizontal(T[,] target)
        {
            int h = target.GetLength(0);
            int w = target.GetLength(1);
            T[,] result = new T[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    result[h - y - 1, x] = target[y, x];
                }
            }

            return result;
        }

        public static T[,] InverseBy_Vertical(T[,] target)
        {
            int h = target.GetLength(0);
            int w = target.GetLength(1);
            T[,] result = new T[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    result[y, w - x - 1] = target[y, x];
                }
            }

            return result;
        }

        public static T[,] InverseBy_Diagonal(T[,] target)
        {
            int h = target.GetLength(0);
            int w = target.GetLength(1);
            T[,] result = new T[w, h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    result[x, y] = target[y, x];
                }
            }

            return result;
        }

        public static T[,] InverseBy_InvertDiagonal(T[,] target)
        {
            int h = target.GetLength(0);
            int w = target.GetLength(1);
            T[,] result = new T[w, h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    result[w - x - 1, h - y - 1] = target[y, x];
                }
            }

            return result;
        }

        /// <summary>rotate 2D array by 90 * multiplier degree. negative represents counterclockwise, note that multiplier exceed range of [-3, 3] will be cropped into [-3,3] but the results are same</summary>
        /// <param name="target">target array to be rotated</param>
        /// <param name="multiplier">how many times will array be rotated</param>
        /// <returns>rotated array</returns>
        public static T[,] RotateClockwise(T[,] target, int multiplier = 1)
        {
            T[,] result = target;
            if (multiplier >= 0)
            {
                for (int i = 0; i < multiplier % 4; i++)
                {
                    result = InverseBy_Vertical(InverseBy_Diagonal(result));
                }
            }
            else
            {
                for (int i = 0; i < -multiplier % 4; i++)
                {
                    result = InverseBy_Diagonal(InverseBy_Vertical(result));
                }
            }
            return result;
        }
    }
}