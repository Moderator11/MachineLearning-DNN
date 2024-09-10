using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Convolution;

namespace ConvolutionNeuralNetworkTest
{
    public partial class Form1 : Form//https://greentec.github.io/reinforcement-learning-third/
    {//https://sdc-james.gitbook.io/onebook/4.-and/5.4.-tensorflow/5.4.2.-cnn-convolutional-neural-network
        public Form1()//https://jarikki.tistory.com/26
        {//https://metamath1.github.io/cnn/index.html
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //https://www.youtube.com/watch?v=kGvwOwv7KQ8
            //Text Template Usage
            Img<double> image = new Img<double>(new Bitmap(@"C:\Users\soo\source\repos\MachineLearning\ConvolutionNeuralNetworkTest\test.jpg"));
            Bitmap r, g, b;
            double[,] grayScaled = image.GreyScaleAvgRGB();
            double[,] r_fv = Conv.GetFeatureCropRest(grayScaled, Conv.filter_Vertical);
            double[,] r_fh = Conv.GetFeatureCropRest(grayScaled, Conv.filter_Horizontal);
            double[,] r_sb = Conv.GetFeatureCropRest(grayScaled, Conv.filter_Sobel);
            r = Img<double>.GetImageFromMatrix(r_fv);
            g = Img<double>.GetImageFromMatrix(r_fh);
            b = Img<double>.GetImageFromMatrix(r_sb);
            pictureBox1.Image = image.GetImage();
            pictureBox2.Image = r;
            pictureBox3.Image = g;
            pictureBox4.Image = b;
        }
    }
}