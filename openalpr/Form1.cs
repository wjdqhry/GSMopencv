using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using OpenCvSharp;
using Tesseract;

namespace openalpr
{
    public partial class Form1 : Form
    {
        public static string filePath;
        Opencv opencv;
        Mat mat;

        

        public Form1()
        {

            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "이미지 파일(.jpg,.png)|";
            openFileDialog1.Title = "이미지 열기";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            string fileName = openFileDialog1.SafeFileName;
            filePath = openFileDialog1.FileName;
            if (openFileDialog1.FileName != "")
            {
                pictureBox1.Load(@filePath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            opencv = new Opencv(filePath);
            try
            {
                if (checkBox1.Checked)
                {
                    pictureBox1.Image = opencv.convertbinp(filePath);
                }
                else if((Convert.ToInt16(textBox1.Text) > 0 || Convert.ToInt16(textBox1.Text) <= 255))
                {
                    pictureBox1.Image = opencv.convertbinp(filePath, Convert.ToInt16(textBox1.Text));
                }
                else
                {
                    MessageBox.Show("올바른 정수값을 입력해주세요(1~255)");
                }
            }
            catch
            {
                MessageBox.Show("사진과 세부사항을 기재해주세요");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !textBox1.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {     
            MessageBox.Show(opencv.Method1());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(opencv.Method2(new Bitmap(pictureBox1.Image)));
        }
    }
}
