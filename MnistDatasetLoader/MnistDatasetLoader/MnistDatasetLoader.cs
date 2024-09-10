using System;
using System.IO;

namespace MnistDatasetLoader
{
    class MDsetLoader
    {
        string localcurrentDir0 = Environment.CurrentDirectory + @"\train-labels.idx1-ubyte";
        string localcurrentDir1 = Environment.CurrentDirectory + @"\train-images.idx3-ubyte";

        byte[] labels;
        byte[][] images;

        public MDsetLoader()
        {
            labels = LoadLabels(localcurrentDir0);
            images = LoadImages(localcurrentDir1);
        }

        public MDsetLoader(string imagePath, string labelPath)
        {
            labels = LoadLabels(labelPath);
            images = LoadImages(imagePath);
        }

        public byte[] GetLabels() { return labels; }
        public byte[][] GetImages() { return images; }

        public byte[,] ParseImage(byte[][] images, int index)
        {
            byte[,] image = new byte[28, 28];
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    image[y, x] = images[index][x + y * 28];
                }
            }
            return image;
        }

        byte[] LoadLabels(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                int mN = BitConverter.ToInt32(GetBytesFromStream(fs, 4), 0);
                int noI = BitConverter.ToInt32(GetBytesFromStream(fs, 4), 0);//60000
                byte[] labels = new byte[noI];
                fs.Read(labels, 0, noI);
                return labels;
            }
        }

        byte[][] LoadImages(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                int mN = BitConverter.ToInt32(GetBytesFromStream(fs, 4), 0);
                int noI = BitConverter.ToInt32(GetBytesFromStream(fs, 4), 0);//60000
                int noR = BitConverter.ToInt32(GetBytesFromStream(fs, 4), 0);//28
                int noC = BitConverter.ToInt32(GetBytesFromStream(fs, 4), 0);//28
                byte[][] images = new byte[noI][];

                for (int i = 0; i < noI; i++)
                {
                    images[i] = new byte[noR * noC];
                    fs.Read(images[i], 0, noR * noC);
                }

                return images;
            }
        }

        byte[] GetBytesFromStream(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return ConvertBigToLittleEndian(buffer);
        }

        byte[] ConvertBigToLittleEndian(byte[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = data[data.Length - 1 - i];
            }
            return result;
        }
    }
}
