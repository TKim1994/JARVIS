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
    public partial class Form3 : Form
    {
        string modo;
        public Form3(string modo_incoming)
        {
            InitializeComponent();
            modo = modo_incoming;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string _strFilePath = Directory.GetCurrentDirectory();
            _strFilePath = _strFilePath.Substring(0, _strFilePath.Length - 24);

            if (modo == "sap")
            {
                List<string> creds = new List<string>();
                using (var reader = new StreamReader(_strFilePath + @"\ExtraJAQHES\credenciales\SAP_creds.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        creds.Add(line);
                    }
                }
                textBox1.Text = creds[0];
                textBox2.Text = creds[1];
            }
            else
            {
               string[] creds = new String[2];
                using (var reader = new StreamReader(_strFilePath + @"\ExtraJAQHES\credenciales\WIN_creds.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        creds = line.Split('.');
                    }
                }
                textBox1.Text = creds[0];
                textBox2.Text = creds[1];
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string _strFilePath = Directory.GetCurrentDirectory();
            _strFilePath = _strFilePath.Substring(0, _strFilePath.Length - 24);

            if (modo == "sap")
            {
                List<string> creds = new List<string>();
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\ExtraJAQHES\credenciales\SAP_creds.txt"))
                {
                    writetext.WriteLine(textBox1.Text + "\r\n" + textBox2.Text);
                }

                //Copia archivo modificado a Carpeta en OneDrive
                string OneDrive = File.ReadAllText(Directory.GetCurrentDirectory() + @"\OneDrive.txt", Encoding.UTF8);
                using (StreamWriter writetext = new StreamWriter(OneDrive + @"\SAP\SAP_creds.txt"))
                {
                    writetext.WriteLine(textBox1.Text + "\r\n" + textBox2.Text);
                }

                File.Delete(_strFilePath + @"\ExtraJAQHES\NO_STRETCHING.txt");

            }
            else
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\ExtraJAQHES\credenciales\WIN_creds.txt"))
                {
                    writetext.WriteLine(textBox1.Text + "." + textBox2.Text);
                }

                //Copia archivo modificado a Carpeta en OneDrive
                string OneDrive = File.ReadAllText(Directory.GetCurrentDirectory() + @"\OneDrive.txt", Encoding.UTF8);
                using (StreamWriter writetext = new StreamWriter(OneDrive + @"\WIN\WIN_creds.txt"))
                {
                    writetext.WriteLine(textBox1.Text + "." + textBox2.Text);
                }
            }
            this.Close();
        }
    }   
}
