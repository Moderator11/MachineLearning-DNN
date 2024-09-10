using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Matrices;
using LearningModel;
using MnistDatasetLoader;
using static System.Net.Mime.MediaTypeNames;

namespace DrawToText
{
    public partial class form_Main : Form
    {
        Bitmap draw;
        int px = 0;
        int py = 0;
        Random r = new Random(Guid.NewGuid().GetHashCode());
        MnistDatasetLoader.MDsetLoader mds; //For training// = new MnistDatasetLoader.MDsetLoader();

        public form_Main()
        {
            InitializeComponent();
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var cord = pictureBox_draw.PointToClient(Cursor.Position);
            px = cord.X;
            py = cord.Y;
            timer_draw.Start();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer_draw.Stop();
        }

        private void timer_draw_Tick(object sender, EventArgs e)
        {
            if (draw != null)
            {
                var cord = pictureBox_draw.PointToClient(Cursor.Position);

                using (Graphics g = Graphics.FromImage(draw))
                {
                    Pen pen = new Pen(Color.FromArgb(0, 0, 0), 10);
                    g.DrawLine(pen, px, py, cord.X, cord.Y);
                }
                var co = pictureBox_draw.PointToClient(Cursor.Position);
                px = co.X;
                py = co.Y;
                pictureBox_draw.Image = draw;
            }
        }
        Matrix<double>[] brightness = new Matrix<double>[10];
        LearningModel<double> lm = new LearningModel<double>(new int[] { 28 * 28, 65, 10 }, new string[] { "leakyrelu", "sigmoid", "sigmoid" }, "MSE");
        private void form_Main_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Enter:
                    if (draw == null)
                    {
                        Reset();
                    }
                    else
                    {
                        pictureBox0.Image = new Bitmap(draw, new Size(28, 28));
                    }
                    break;
                case Keys.C:
                    Reset();
                    break;
                case Keys.I:
                    lm.Initialize();
                    MessageBox.Show("init success");
                    break;
                case Keys.T:
                    for (int k = 0; k < 2000; k++)
                    {
                        Text = ((float)k / 20).ToString();
                        for (int i = 0; i < 10; i++)
                        {
                            double[,] output = new double[1, 10];
                            for (int j = 0; j < 10; j++)
                            {
                                if (i == j)
                                {
                                    output[0, j] = 1;
                                }
                                else
                                {
                                    output[0, j] = 0;
                                }
                            }
                            lm.Train_Silent(brightness[i], new Matrix<double>(output));
                        }
                    }
                    timer_beep.Start();
                    if (MessageBox.Show("Trained Successfully") == DialogResult.OK)
                    {
                        timer_beep.Stop();
                    }
                    break;
                case Keys.L:
                    lm.LoadModelData_Xml();
                    MessageBox.Show("Loaded Successfully");
                    break;
                case Keys.S:
                    lm.SaveModelData_Xml();
                    MessageBox.Show("Saved Successfully");
                    break;
                case Keys.P:
                    Bitmap img = new Bitmap((Bitmap)pictureBox_draw.Image, new Size(28,28));
                    double[,] tst = new double[1, 784];
                    Matrix<double> test;
                    for (int i = 0; i < 28; i++)
                    {
                        for (int j = 0; j < 28; j++)
                        {
                            if (i == 0 || j == 0)
                            {
                                //?? why da fuc is first row and col are filled with 1 ??
                            }
                            else
                            {
                                tst[0, 28 * i + j] = 1 - (double)(img.GetPixel(j, i).R / 255);//0-white, 1-dark
                            }
                        }
                    }
                    /*=======================Preview==============
                    string a = "";
                    for (int i = 0; i < 28; i++)
                    {
                        for (int j = 0; j < 28; j++)
                        {
                            a += tst[0, 28*i+j].ToString() + ", ";
                        }
                        a += "\n";
                    }
                    MessageBox.Show(a);
                    ============================================*/

                    test = new Matrix<double>(tst).DeepCopy();
                    Matrix<double> res = lm.GetPredict(test);
                    string msg = "";
                    double[,] outp = res.GetMatrix();
                    double maxO = outp[0, 0];
                    int maxOIndex = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        if (outp[0, i] > maxO)
                        {
                            maxO = outp[0, i];
                            maxOIndex = i;
                        }
                        msg += "\n" + outp[0, i];
                    }
                    msg += "\n" + "I guess It is " + maxOIndex;
                    label_draworder.Text = msg;
                    /*double max = -1, mi = -1;
                    for (int i = 0; i < 10; i++)
                    {
                        if (res.GetMatrix()[0, i] > max)
                        {
                            max = res.GetMatrix()[0, i];
                            mi = i;
                        }
                    }
                    MessageBox.Show(mi.ToString());*/
                    break;
                case Keys.E:
                    ShowExampleCases();
                    break;
            }

            void ShowExampleCases()
            {
                int index = r.Next(0, 60000);
                byte[,] eximg = mds.ParseImage(mds.GetImages(), index);
                Bitmap bmp = new Bitmap(28, 28);
                for (int y = 0; y < eximg.GetLength(0); y++)
                {
                    for (int x = 0; x < eximg.GetLength(1); x++)
                    {
                        Color c = Color.FromArgb(255-eximg[y, x], 255-eximg[y, x], 255-eximg[y, x]);
                        bmp.SetPixel(x, y, c);
                    }
                }
                pictureBox0.Image = bmp;
                pictureBox_draw.Image = new Bitmap(bmp, new Size(250, 250));
            }

            void Reset()
            {
                draw = new Bitmap(pictureBox_draw.Size.Width, pictureBox_draw.Size.Height);
                using (Graphics g = Graphics.FromImage(draw))
                {
                    g.Clear(Color.White);
                }
                pictureBox_draw.Image = draw;
            }
        }

        private void timer_beep_Tick(object sender, EventArgs e)
        {
            Console.Beep();
        }

        private void form_Main_Load(object sender, EventArgs e)
        {

        }
    }
}
