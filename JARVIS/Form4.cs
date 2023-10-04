using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JARVISNamespace
{
    public partial class Form4 : Form
    {
        Timer t = new Timer();

        int WIDTH = 300, HEIGHT = 300, secHAND = 140, minHAND = 110, hrHAND = 80, customHAND = 90;

        //center
        int cx, cy;

        Bitmap bmp, bmp2;
        Graphics g, g2;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //create bitmap
            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);
            bmp2 = new Bitmap(WIDTH + 1, HEIGHT + 1);

            //center
            cx = WIDTH / 2;
            cy = HEIGHT / 2;

            //backcolor
            this.BackColor = Color.White;
            
            //timer
            t.Interval = 10;      //in millisecond
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            //create graphics
            g = Graphics.FromImage(bmp);
            g2 = Graphics.FromImage(bmp2);

            //get time
            //int ss = DateTime.Now.Second;

            //int mm = DateTime.Now.Minute;
            //int hh = DateTime.Now.Hour;

            int mm1 = Form1.mm1;
            int hh1 = Form1.hh1;

            int mm2 = Form1.mm2;
            int hh2 = Form1.hh2;

            if (hh2 >= 24)
            {
                hh2 = hh2 - 24;
            }

            int[] handCoord = new int[2];

            //clear
            g.Clear(Color.White);
            g2.Clear(Color.White);

            //draw circle
            g.DrawEllipse(new Pen(Color.Black, 1f), 0, 0, WIDTH, HEIGHT);
            g2.DrawEllipse(new Pen(Color.Black, 1f), 0, 0, WIDTH, HEIGHT);

            //draw figure
            g.DrawString("00", new Font("Lucida Console", 12), Brushes.Black, new PointF(140, 2));
            g.DrawString("3", new Font("Lucida Console", 12), Brushes.Black, new PointF(286, 140));
            g.DrawString("6", new Font("Lucida Console", 12), Brushes.Black, new PointF(142, 282));
            g.DrawString("9", new Font("Lucida Console", 12), Brushes.Black, new PointF(0, 140));

            g2.DrawString("12", new Font("Lucida Console", 12), Brushes.Black, new PointF(140, 2));
            g2.DrawString("15", new Font("Lucida Console", 12), Brushes.Black, new PointF(286, 140));
            g2.DrawString("18", new Font("Lucida Console", 12), Brushes.Black, new PointF(142, 282));
            g2.DrawString("21", new Font("Lucida Console", 12), Brushes.Black, new PointF(0, 140));

            #region
            float x, y, R, angle;

            R = 146;
            angle = 60;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;
            g.DrawString("1", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;
            g.DrawString("2", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 60;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g.DrawString("5", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g.DrawString("4", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 60;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g.DrawString("7", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g.DrawString("8", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 60;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;
            g.DrawString("11", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;

            g.DrawString("10", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            #endregion 

            #region

            R = 146;
            angle = 60;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;
            g2.DrawString("13", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;
            g2.DrawString("14", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 60;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g2.DrawString("17", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 + Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) - 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g2.DrawString("16", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 60;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g2.DrawString("19", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 + Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) - 5;
            g2.DrawString("20", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 60;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;
            g2.DrawString("23", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            R = 146;
            angle = 30;
            x = 143 - Convert.ToSingle(R * Math.Cos((angle * Math.PI / 180))) + 5;
            y = 140 - Convert.ToSingle(R * Math.Sin((angle * Math.PI / 180))) + 5;

            g2.DrawString("22", new Font("Arial", 12), Brushes.Black, new PointF(x, y));

            #endregion 

            /*
            //second hand
            handCoord = msCoord(ss, secHAND);
            g.DrawLine(new Pen(Color.Red, 1f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
            */

            //colorea el area
            int new_hh1;
            int new_mm1;

            bool flag = true;

            new_mm1 = mm1;
            new_hh1 = hh1;
            while (flag == true)
            {
                new_mm1 = new_mm1 + 1;
                if (new_mm1 == 60)
                {
                    new_mm1 = 0;
                    new_hh1 = new_hh1 + 1;
                    
                    if (new_hh1 == 24)
                    {
                        new_hh1 = 0;
                    }

                    if (new_hh1 >= 0 && new_hh1 <= 11)
                    {
                        handCoord = hrCoord(new_hh1 % 12, new_mm1, customHAND);
                        g.DrawLine(new Pen(Color.LightSalmon, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
                    }
                    else
                    {
                        handCoord = hrCoord(new_hh1 % 12, new_mm1, customHAND);
                        g2.DrawLine(new Pen(Color.LightSalmon, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
                    }
                }
                else
                {
                    if (new_hh1 == 24)
                    {
                       new_hh1 = 0;
                    }

                    if (new_hh1 >= 0 && new_hh1 <= 11)
                    {
                        handCoord = hrCoord(new_hh1 % 12, new_mm1, customHAND);
                        g.DrawLine(new Pen(Color.LightSalmon, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
                    }
                    else
                    {
                        handCoord = hrCoord(new_hh1 % 12, new_mm1, customHAND);
                        g2.DrawLine(new Pen(Color.LightSalmon, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
                    }
                }

                

                if (new_hh1 == hh2 && new_mm1 == mm2)
                {
                    flag = false;
                }
            }

            //minute hand 1 
            //handCoord = msCoord(mm1, minHAND);
            //g.DrawLine(new Pen(Color.Red, 2f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            //hour hand 1
            if (hh1 >= 0 & hh1 <= 11)
            {
                handCoord = hrCoord(hh1 % 12, mm1, customHAND);
                g.DrawLine(new Pen(Color.Red, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
            }
            else
            {
                handCoord = hrCoord(hh1 % 12, mm1, customHAND);
                g2.DrawLine(new Pen(Color.Red, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
            }


            //minute hand 2
            //handCoord = msCoord(mm2, minHAND);
            //g.DrawLine(new Pen(Color.Blue, 2f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            //hour hand 2
            if (hh2 >= 0 & hh2 <= 11)
            {
                handCoord = hrCoord(hh2 % 12, mm2, customHAND);
                g.DrawLine(new Pen(Color.Blue, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
            }
            else
            {
                handCoord = hrCoord(hh2 % 12, mm2, customHAND);
                g2.DrawLine(new Pen(Color.Blue, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));
            }
            

            

            //load bmp in picturebox1
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp2;

            //disp time
            //this.Text = "Analog Clock -  " + hh + ":" + mm + ":" + ss;
            this.Text = "Analog Clock -  " + hh1 + ":" + mm1;

            //dispose
            g.Dispose();
            g2.Dispose();
        }

        //coord for minute and second hand
        private int[] msCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6;   //each minute and second make 6 degree

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        //coord for hour hand
        private int[] hrCoord(int hval, int mval, int hlen)
        {
            int[] coord = new int[2];

            //each hour makes 30 degree
            //each min makes 0.5 degree
            int val = (int)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }
    }
}
