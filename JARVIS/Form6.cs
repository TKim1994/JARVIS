using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32.TaskScheduler;
using System.Security.Principal;
using System.Security.Permissions;
using System.Drawing.Imaging;
using System.Windows;

using System;
using System.Threading;

namespace JARVISNamespace
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        string _strFilePath_ABS = Directory.GetCurrentDirectory();

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox3.Text = "";
            label2.Text = "";
            label3.Text = "";

            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            label2.Text = Convert.ToString(lst1.Count());
            label3.Text = Convert.ToString(lst2.Count());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff = lst1.Except(lst2).ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", _strFilePath_ABS + @"\list1.csv");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", _strFilePath_ABS + @"\list2.csv");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", _strFilePath_ABS + @"\list3.csv");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff = lst2.Except(lst1).ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff = lst1.Intersect(lst2).ToList();


            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            label2.Text = Convert.ToString(lst1.Count());
            label3.Text = Convert.ToString(lst2.Count());

            textBox3.Text = " ";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff_0 = lst1.Intersect(lst2).ToList();
            var list_diff = lst1.Except(list_diff_0).ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff_0 = lst1.Intersect(lst2).ToList();
            var list_diff = lst2.Except(list_diff_0).ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Copiado a memoria!";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();

            var list_diff = lst1.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff = lst2.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();

            var list_diff = lst1.Distinct().ToList();


            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff = lst2.Distinct().ToList();

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);
            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();
            List<string> lst2 = arr_2.OfType<string>().ToList();

            var list_diff = new HashSet<string>(lst1).SetEquals(lst2);

            if (list_diff)
            {
                textBox3.Text = "Equal!";
            }
            else
            {
                textBox3.Text = "Not equal!";
                
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string list1 = File.ReadAllText(_strFilePath_ABS + @"\list1.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_1 = list1.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst1 = arr_1.OfType<string>().ToList();

           

            var list_diff = lst1.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            var list_diff_int = lst1.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Count())
              .ToList();

            List<string> list_diff2 = new List<string>();
            int index = 0;
            foreach (var item in list_diff)
            {
                list_diff2.Add(item + "\t" + list_diff_int[index]);
                index++;
            }

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff2)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string list2 = File.ReadAllText(_strFilePath_ABS + @"\list2.csv", Encoding.UTF8);

            //Convierte el txtBox1 a una lista de valroes
            string[] stringSeparators = new string[] { "\r\n" };

            string[] arr_2 = list2.Split(stringSeparators, StringSplitOptions.None);

            List<string> lst2 = arr_2.OfType<string>().ToList();



            var list_diff = lst2.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            var list_diff_int = lst2.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Count())
              .ToList();

            List<string> list_diff2 = new List<string>();
            int index = 0;
            foreach (var item in list_diff)
            {
                list_diff2.Add(item + "\t" + list_diff_int[index]);
                index++;
            }

            FileStream stream = new FileStream(_strFilePath_ABS + @"\list3.csv", FileMode.Create);
            using (StreamWriter writetext = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var item in list_diff2)
                {
                    writetext.WriteLine(item);
                }
            }

            string list3 = File.ReadAllText(_strFilePath_ABS + @"\list3.csv", Encoding.UTF8);

            if (list3 == "")
            {
                list3 = "empty";
            }
            System.Windows.Forms.Clipboard.SetText(list3);
            textBox3.Text = "Done!";
        }
    }
}
