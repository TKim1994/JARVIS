using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;
using System.Threading;
using System.Timers;
using System.Net.Mail;
using System.Text;
using System.Diagnostics;


namespace JARVISNamespace
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string _strFilePath = Directory.GetCurrentDirectory();
            _strFilePath = _strFilePath.Substring(0, _strFilePath.Length - 24);

            string extraPath = _strFilePath + @"\ExtraJAQHES\";
            textBox1.Text = extraPath;

            string OneDrive = File.ReadAllText(Directory.GetCurrentDirectory() + @"\OneDrive.txt", Encoding.UTF8);
            textBox3.Text = OneDrive;

            string Watcher = File.ReadAllText(Directory.GetCurrentDirectory() + @"\WatcherPath.txt", Encoding.UTF8);
            textBox4.Text = Watcher;

            string processes2kill = File.ReadAllText(textBox1.Text + @"\StretchingProt_RESET_except.txt", Encoding.UTF8);
            textBox2.Text = processes2kill;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string _strFilePath = Directory.GetCurrentDirectory();
            _strFilePath = _strFilePath.Substring(0, _strFilePath.Length - 24);
            
            //Path de EXTRA_JAQHES no cambia
            using (StreamWriter writetext = new StreamWriter(Directory.GetCurrentDirectory() + @"\OneDrive.txt"))
            {
                writetext.Write(textBox3.Text);
            }
            using (StreamWriter writetext = new StreamWriter(Directory.GetCurrentDirectory() + @"\WatcherPath.txt"))
            {
                writetext.Write(textBox4.Text);
            }
            using (StreamWriter writetext = new StreamWriter(Directory.GetCurrentDirectory() + @"\extraPath.txt"))
            {
                writetext.Write(textBox1.Text);
            }
            using (StreamWriter writetext = new StreamWriter(textBox1.Text + @"\StretchingProt_RESET_except.txt"))
            {
                if (textBox2.Text.Contains("_JAQHES") || textBox2.Text == "")
                {
                    writetext.Write(textBox2.Text);
                }
                else
                {
                    writetext.Write(textBox2.Text + "_JAQHES");
                }
            }

            this.Close();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Folder not found!", "JARVIS TASKER");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(textBox3.Text);
            }
            catch
            {
                MessageBox.Show("Folder not found!", "JARVIS TASKER");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("Folder not found!", "JARVIS TASKER");
            }
        }
    }   
}
