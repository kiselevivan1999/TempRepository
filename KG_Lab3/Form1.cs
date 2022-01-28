using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KG_Lab3
{
    public partial class Form1 : Form
    {
        public int xn, yn, xk, yk, xDown, yDown; // концы отрезка
        Bitmap myBitmap; // объект Bitmap для вывода отрезка
        Color currentBorderColor, currentFillColor; // текущий цвет отрезка и заливки
        Graphics myGraphics;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }


        private void complexFigure()
        {
            int legLength = 40;
            int hypotenuseLenght = 60;
            int xc = 200;
            int yc = 300;
            int x1, y1, x2, y2;

            x1 = xc;
            y1 = yc;
            y2 = y1 + (hypotenuseLenght / 2);
            x2 = x1 - legLength;
            CDA(x1, y1, x2, y2);

            y2 = y1 - (hypotenuseLenght / 2);
            CDA(x1, y1, x2, y2);

            y1 = y2;
            y2 += hypotenuseLenght;
            x1 = x2;
            CDA(x1, y1, x2, y2);

            legLength *= 3;
            hypotenuseLenght *= 3;

            x1 = xc;
            y1 = yc;
            y2 = y1 + (hypotenuseLenght / 2);
            x2 = x1 + legLength;
            CDA(x1, y1, x2, y2);

            y2 = y1 - (hypotenuseLenght / 2);
            CDA(x1, y1, x2, y2);

            y1 = y2;
            y2 += hypotenuseLenght;
            x1 = x2;
            CDA(x1, y1, x2, y2);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            xn = e.X;
            yn = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked == true) {
                myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (myGraphics)
                {
                    myBitmap = pictureBox1.Image as Bitmap;
                    CDA(xn, yn, e.X, e.Y);
                    pictureBox1.Refresh();
                    pictureBox1.Image = myBitmap;
                }

                pictureBox1.Image = myBitmap;
            }
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //отключаем кнопки
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            //создаем новый экземпляр Bitmap размером с элемент
   
            //на нем выводим попиксельно отрезок
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            myGraphics = Graphics.FromHwnd(pictureBox1.Handle);
            using (myGraphics)
            {
                if (radioButton1.Checked == true)
                {
                    //рисуем прямоугольник
                    CDA(10, 10, 10, 110);
                    CDA(10, 10, 110, 10);
                    CDA(10, 110, 110, 110);
                    CDA(110, 10, 110, 110);
                    //рисуем треугольник
                    CDA(150, 10, 150, 200);
                    CDA(150, 10, 150, 200);
                    CDA(250, 50, 150, 150);
                    CDA(250, 50, 150, 150);
                    CDA(150, 10, 250, 150);
                    CDA(150, 10, 250, 150);

                    //рисуем сложную фигуру
                    complexFigure();
                }
                else if (radioButton2.Checked == true)
                {

                    //получаем растр созданного рисунка в mybitmap
                    myBitmap = pictureBox1.Image as Bitmap;

                    // вызываем рекурсивную процедуру заливки с затравкой
                    Zaliv(xn, yn);

                }
                //передаем полученный растр mybitmap в элемент pictureBox
                pictureBox1.Image = myBitmap;
                //обновляем pictureBox и активируем кнопки
                pictureBox1.Refresh();
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                xn = 0;
                yn = 0;
                xk = 0;
                yk = 0;
            }

        }

        private void Zaliv(int x1, int y1)
        {
            
            try
            {
                Color oldСolor;
                // получаем цвет текущего пикселя с координатами x1, y1
                oldСolor = myBitmap.GetPixel(x1, y1);
            
                // сравнение цветов происходит в формате RGB
                // для этого используем метод ToArgb объекта Color
                if ((oldСolor.ToArgb() != currentBorderColor.ToArgb()) &&
               (oldСolor.ToArgb() != currentFillColor.ToArgb()))
                {
                    //перекрашиваем пиксель
                    myBitmap.SetPixel(x1, y1, currentFillColor);

                    //вызываем метод для 4-х соседних пикселей
                    Zaliv(x1 + 1, y1);
                    Zaliv(x1 - 1, y1);
                    Zaliv(x1, y1 + 1);
                    Zaliv(x1, y1 - 1);
                }
                else
                {
                    //выходим из метода
                    return;
                }
            }
            catch (StackOverflowException)
            {
                Console.WriteLine("Стек переполнен! Попробуйте закрасить фигуру меньших размеров.");
            }
        }

        private void CDA(int x1, int y1, int x2, int y2)
        {
            int i, numberNodes;
            double xt, yt, dx, dy;
            
            xn = x1;
            yn = y1;
            xk = x2;
            yk = y2;
            dx = xk - xn;
            dy = yk - yn;
            numberNodes = 300;
            xt = xn;
            yt = yn;
            for (i = 1; i <= numberNodes; i++)
            {
                myBitmap.SetPixel((int)xt, (int)yt, currentBorderColor);
                xt += dx / numberNodes;
                yt += dy / numberNodes;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = colorDialog1.ShowDialog();

            if (dialogResult.Equals(DialogResult.OK) && radioButton1.Checked)
            {
                currentBorderColor = colorDialog1.Color;
            }
            if (dialogResult.Equals(DialogResult.OK) && radioButton2.Checked)
            {
                currentFillColor = colorDialog1.Color;
            }
        }

    }
}
