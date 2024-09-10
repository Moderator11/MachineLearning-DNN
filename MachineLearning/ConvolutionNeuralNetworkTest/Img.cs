using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Convolution
{
    class Img<T>//https://blog.ggaman.com/965
    {
        private Bitmap image { get; }
        private T[][,] image_matrix = new T[3][,];

        public Img(Bitmap _image)
        {
            image = _image;
            for (int i = 0; i < image_matrix.Length; i++)
            {
                image_matrix[i] = new T[image.Height, image.Width];
            }
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color c = image.GetPixel(x, y);
                    image_matrix[0][y, x] = (T)Convert.ChangeType(c.R, typeof(T));
                    image_matrix[1][y, x] = (T)Convert.ChangeType(c.G, typeof(T));
                    image_matrix[2][y, x] = (T)Convert.ChangeType(c.B, typeof(T));
                }
            }
        }

        public Bitmap GetImage()
        {
            return image;
        }

        public T[][,] GetRGBMatrix()
        {
            return image_matrix;
        }

        public T[,] GreyScaleAvgRGB()
        {
            T[,] result = new T[image.Height, image.Width];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color c = image.GetPixel(x, y);
                    T v = (T)Convert.ChangeType((c.R + c.G + c.B), typeof(T)) / (dynamic)3;
                    result[y, x] = v;
                }
            }

            return result;
        }

        public T[,] GreyScaleYCbCr()
        {
            T[,] result = new T[image.Height, image.Width];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color c = image.GetPixel(x, y);
                    T v = (T)Convert.ChangeType((c.R * (dynamic)0.2126 + c.G * (dynamic)0.7152 + c.B * (dynamic)0.0722), typeof(T));
                    result[y, x] = v;
                }
            }

            return result;
        }

        public T[,] GreyScaleYPrPb()
        {
            T[,] result = new T[image.Height, image.Width];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color c = image.GetPixel(x, y);
                    T v = (T)Convert.ChangeType((c.R * (dynamic)0.299 + c.G * (dynamic)0.587 + c.B * (dynamic)0.114), typeof(T));
                    result[y, x] = v;
                }
            }

            return result;
        }

        public static T GetVariance(T[,] g)
        {
            T sum = (dynamic)0;
            for (int y = 0; y < g.GetLength(0); y++)
            {
                for (int x = 0; x < g.GetLength(1); x++)
                {
                    sum += (dynamic)g[y, x];
                }
            }
            T avg = sum / (dynamic)(g.GetLength(0) * g.GetLength(1));
            sum = (dynamic)0;
            for (int y = 0; y < g.GetLength(0); y++)
            {
                for (int x = 0; x < g.GetLength(1); x++)
                {
                    sum += (avg - (dynamic)g[y, x]) * (avg - (dynamic)g[y, x]);
                }
            }
            return sum / (dynamic)(g.GetLength(0) * g.GetLength(1));
        }

        public static Bitmap GetImageFromMatrix(T[][,] rgb)
        {
            Bitmap result = new Bitmap(rgb[0].GetLength(1), rgb[0].GetLength(0));
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    byte r = (byte)Convert.ChangeType(rgb[0][y, x], typeof(byte));
                    byte g = (byte)Convert.ChangeType(rgb[1][y, x], typeof(byte));
                    byte b = (byte)Convert.ChangeType(rgb[2][y, x], typeof(byte));
                    result.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return result;
        }

        public static Bitmap GetImageFromMatrix(T[,] g)
        {
            Bitmap result = new Bitmap(g.GetLength(1), g.GetLength(0));
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    T n = g[y, x];
                    if (n < (dynamic)0) { n = (dynamic)0; }
                    if (n > (dynamic)255) { n = (dynamic)255; }
                    byte v = (byte)Convert.ChangeType(n, typeof(byte));
                    result.SetPixel(x, y, Color.FromArgb(v, v, v));
                }
            }
            return result;
        }
    }
}