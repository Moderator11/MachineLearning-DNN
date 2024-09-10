using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MnistDatasetLoader
{
    public partial class Form1 : Form
    {
        MDsetLoader loader = new MDsetLoader();
        byte[] labels;
        byte[][] images;

        public Form1()
        {
            InitializeComponent();
            labels = loader.GetLabels();
            images = loader.GetImages();
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            Bitmap i = new Bitmap(28, 28);
            byte[,] image = loader.ParseImage(images, count);
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    byte pixelColor = image[y, x];
                    i.SetPixel(x, y, Color.FromArgb(pixelColor, pixelColor, pixelColor));
                }
            }
            pictureBox1.Image = i;
            label_imageLabel.Text = labels[count].ToString();
            count++;
        }
        int count = 0;
    }
}