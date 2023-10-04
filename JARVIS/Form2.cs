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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string _strFilePath = Directory.GetCurrentDirectory();
            _strFilePath = _strFilePath.Substring(0, _strFilePath.Length - 24);

            String hoyFecha = DateTime.Now.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            string folderBotName = Form1.SetValueForText1;

            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string _strFilePath = Directory.GetCurrentDirectory();
            _strFilePath = _strFilePath.Substring(0, _strFilePath.Length - 24);

            String hoyFecha = DateTime.Now.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            string folderBotName = Form1.SetValueForText1;
            


            try //Para tipo de almacenamiento 1 (con batch para ejecucion de c#)
            {
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_" + comboBox1.Text + ".exe");
                this.Close();
            }
            catch
            {
                try //Para tipo de almacenamiento 2 (ejecucion directa de c#)
                {
                    string folderBotNum = folderBotName.Substring(0, 2);
                    int num;
                    int.TryParse(folderBotNum.Substring(0, 2), out num);
                    string numStr = num.ToString().PadLeft(3, '0');
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + "_" + comboBox1.Text + ".exe");
                    this.Close();
                }
                catch // Si no existe el ejecutable
                {
                    MessageBox.Show("Specified JAQHES not found");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = button5;
        }

    }   
}
