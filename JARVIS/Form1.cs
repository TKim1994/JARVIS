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


using System.Speech.Synthesis;


namespace JARVISNamespace
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        
        public static string SetValueForText1 = "";

        public static int hh1;
        public static int mm1;
        public static int hh2;
        public static int mm2;

        string _strFilePath_ABS = Directory.GetCurrentDirectory();

        private void Form1_Load(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);
            this.dataGridView1.Font = new Font("Consolas", 8);


            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //Hace que se selecciona toda la fila y no solo una celda
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;

            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.MultiSelect = false;
            //Hace que se selecciona toda la fila y no solo una celda

            checkBox1.Visible = false;
            pictureBox3.Visible = true;
            pictureBox6.Visible = true;

            for (int u = 0; u <=23; u++)
            {
                comboBox1.Items.Add(u.ToString("00"));
            }

            for (int u = 0; u < 60; u = u+1)
            {
                comboBox2.Items.Add(u.ToString("00"));
            }

            for (int u = 0; u <= 23; u++)
            {
                comboBox4.Items.Add(u.ToString("00"));
            }

            for (int u = 0; u < 60; u = u + 5)
            {
                comboBox3.Items.Add(u.ToString("00"));
            }


            comboBox9.Items.Add("--");
            comboBox9.Items.Add("Daily");
            comboBox9.Items.Add("Weekly");
            comboBox9.Items.Add("Monthly");
            

            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(_strFilePath);
            int index_count = 1;
            foreach (var item in subdirectoryEntries)
            {
                string[] item2 = item.Split('\\');
                int index = item2.Count();
                string folder = item2[index - 1];

                //Selecciona solo las carpetas cuyos 2 primeros caracteres son numeros (JAQHES)
                int n;
                string status = "";
                if (int.TryParse(folder.Substring(0, 2), out n)) 
                {
                    //Fecha de ultima modificacion
                    DateTime dt = Directory.GetLastWriteTime(_strFilePath + @"\" + folder);

                    //Tamaño de carpeta
                    long length = Directory.GetFiles(_strFilePath + @"\" + folder, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
                    decimal size = Decimal.Divide(length, 1048576);
                    size = decimal.Round(size, 2, MidpointRounding.AwayFromZero);

                    //Status
                    string path = item + @"\menworking.txt";
                    if (File.Exists(path))
                    {
                        status = "Dev";
                    }
                    else
                    {
                        string key = File.ReadAllText(item + @"\key.txt", Encoding.UTF8);
                        string[] keys = key.Split(',');

                        if (keys[0] == "on")
                        {
                            status = "Oper";
                        }
                        else
                        {
                            status = "Func";
                        }
                    }
                   

                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = index_count;
                    row.Cells[1].Value = folder;
                    row.Cells[2].Value = status;
                    row.Cells[3].Value = dt;
                    row.Cells[4].Value = size;
                    dataGridView1.Rows.Add(row);

                    index_count++;
                }
            }

            dataGridView1.Columns["Id"].DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.Columns["Id"].DefaultCellStyle.BackColor = Color.SteelBlue;

            dataGridView1.Columns["Sist"].DefaultCellStyle.BackColor = Color.LightBlue;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            Process.Start(_strFilePath_ABS + @"\Sync.exe");

            //Llama a la funcion temporizada
            //InitTimer();
        }

        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 5000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process.Start(_strFilePath_ABS + @"\Sync.exe");
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            checkBox1.Visible = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";

            string path = "";

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = "";
            try
            {
                folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();
            }
            catch
            {
                return;
            }

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            try
            {
                //Actualiza los CheckBoxes
                string info = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\info.txt", Encoding.UTF8);
                string key = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\key.txt", Encoding.UTF8);
                string[] keys = key.Split(',');


                //Permite darle un refresh al estado del checkbox
                checkBox1.Checked = true;
                checkBox1.Checked = false;
                //Permite darle un refresh al estado del checkbox

                textBox2.Text = info;

                if (keys[0] == "on")
                {
                    checkBox1.Checked = true;
                }
                else if (keys[0] == "off")
                {
                    checkBox1.Checked = false;
                }

                if (keys[1] == "on")
                {
                    checkBox2.Checked = true;
                }
                else if (keys[1] == "off")
                {
                    checkBox2.Checked = false;
                }

                //Actualiza duracion estimada
                string dur = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\dur.txt", Encoding.UTF8);
                string[] durs = dur.Split(',');
                comboBox4.Text = durs[0];
                comboBox3.Text = durs[1];


                //Actualiza el Modo de almacenamiento
                path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
                if (Directory.Exists(path))
                {
                    label1.Text = "Modo de almacenamiento 2";
                }
                else
                {
                    label1.Text = "Modo de almacenamiento 1";
                }

                //Actualiza el LOG
                string log = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\log.txt", Encoding.UTF8);
                textBox1.Text = log;

                //Actualiza el IniDate y FinDate
                string inidate = "";
                string findate = "";
                path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
                if (Directory.Exists(path))
                {
                    if (File.Exists(path + @"\inidate.txt"))
                    {
                        inidate = File.ReadAllText(path + @"\inidate.txt", Encoding.UTF8);
                    }
                    if (File.Exists(path + @"\findate.txt"))
                    {
                        findate = File.ReadAllText(path + @"\findate.txt", Encoding.UTF8);
                    }
                }
                else
                {
                    string path2 = _strFilePath + @"\" + folderBotName + @"\sys\";

                    if (File.Exists(path2 + @"\inidate.txt"))
                    {
                        inidate = File.ReadAllText(path2 + @"\inidate.txt", Encoding.UTF8);
                    }
                    if (File.Exists(path2 + @"\findate.txt"))
                    {
                        findate = File.ReadAllText(path2 + @"\findate.txt", Encoding.UTF8);
                    }
                }
                textBox5.Text = inidate;
                textBox6.Text = findate;

                //Actualiza el Mode
                string mode = "";
                path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
                if (Directory.Exists(path))
                {
                    if (File.Exists(path + @"\Mode.txt"))
                    {
                        mode = File.ReadAllText(path + @"\Mode.txt", Encoding.UTF8);
                    }
                }
                else
                {
                    string path2 = _strFilePath + @"\" + folderBotName + @"\sys\";

                    if (File.Exists(path2 + @"\Mode.txt"))
                    {
                        mode = File.ReadAllText(path2 + @"\Mode.txt", Encoding.UTF8);
                    }
                }
                textBox9.Text = mode;

                //Actualiza el estado del boton REGLAS
                if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\ReglasDinamicas.xlsx"))
                {
                    groupBox1.Visible = true;
                    button10.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;

                    /*
                    string reglasDate = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\sys\reglasDate.txt", Encoding.UTF8);
                    string reglasDate_fecha1 = reglasDate.Substring(0, 4);
                    string reglasDate_fecha2 = reglasDate.Substring(4, 2);
                    string reglasDate_fecha3 = reglasDate.Substring(6, 2);

                    string reglasDate_hora1 = reglasDate.Substring(8, 2);
                    string reglasDate_hora2 = reglasDate.Substring(10, 2);
                    string reglasDate_hora3 = reglasDate.Substring(12, 2);
                    */

                    DateTime modification = File.GetLastWriteTime(_strFilePath + @"\" + folderBotName + @"\sys\ReglasDinamicas.xlsx");
                    string asString = modification.ToString("dd MM yyyy HH mm ss");
                    string[] asStringArray = asString.Split(' ');
                    
                    label4.Text = "Ult. mod : " + asStringArray[0] + "/" + asStringArray[1] + "/" + asStringArray[2];
                    label5.Text = "Ult. mod : " + asStringArray[3] + ":" + asStringArray[4] + ":" + asStringArray[5];
                    
                }
                else
                {
                    groupBox1.Visible = false;
                    button10.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                }

                //Actualiza los pendings
                string pendings = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\pendings.txt", Encoding.UTF8);
                textBox4.Text = pendings;

            }
            catch { }


            //Actualiza datos del TaskScheduler
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            comboBox1.Text = "00";
            comboBox2.Text = "00";
            comboBox9.Text = "--";

            path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                if (File.Exists(path + folderBotName + ".exe"))
                {
                    label10.Text = "N";
                }
                else
                {
                    if (File.Exists(path + folderBotName + "_1.exe"))
                    {
                        label10.Text = "1";
                    }
                    else
                    {
                        if (File.Exists(path + folderBotName + "_2.exe"))
                        {
                            label10.Text = "2";
                        }
                        else
                        {
                            label10.Text = "X";
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + ".exe"))
                {
                    label10.Text = "N";
                }
                else
                {
                    if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_1.exe"))
                    {
                        label10.Text = "1";
                    }
                    else
                    {
                        if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_2.exe"))
                        {
                            label10.Text = "2";
                        }
                        else
                        {
                            label10.Text = "X";
                        }
                    }
                }
            }

            string folderBotName_2 = "";
            if (label10.Text == "N")
            {
                folderBotName_2 = folderBotName;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;

                        checkedListBox1.Items.Clear();
                        checkedListBox2.Items.Clear();

                        checkedListBox1.Visible = false;
                        checkedListBox2.Visible = false;
                    }
                }
            }
            else if (label10.Text != "X")
            {
                folderBotName_2 = folderBotName + "_" + label10.Text;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;

                        checkedListBox1.Items.Clear();
                        checkedListBox2.Items.Clear();

                        checkedListBox1.Visible = false;
                        checkedListBox2.Visible = false;
                    }
                }
            }
            else
            {
                comboBox1.Text = "00";
                comboBox2.Text = "00";
                comboBox9.Text = "--";

                checkBox3.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            String hoyFecha = DateTime.Now.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            try //Para tipo de almacenamiento 1 (con batch para ejecucion de c#)
            {
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\" + hoyFecha);
            }
            catch
            {
                try //Para tipo de almacenamiento 2 (ejecucion directa de c#)
                {
                    string folderBotNum = folderBotName.Substring(0, 2);
                    int num;
                    int.TryParse(folderBotNum.Substring(0, 2), out num);
                    string numStr = num.ToString().PadLeft(3, '0');
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + hoyFecha);
                }
                catch
                {
                    MessageBox.Show("Specified folder not found");
                }
            }
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            DateTime yesterday = DateTime.Today.AddDays(-1);
            String hoyFecha = yesterday.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            try //Para tipo de almacenamiento 1 (con batch para ejecucion de c#)
            {
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\" + hoyFecha);
            }
            catch
            {
                try //Para tipo de almacenamiento 2 (ejecucion directa de c#)
                {
                    string folderBotNum = folderBotName.Substring(0, 2);
                    int num;
                    int.TryParse(folderBotNum.Substring(0, 2), out num);
                    string numStr = num.ToString().PadLeft(3, '0');
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + hoyFecha);
                }
                catch
                {
                    MessageBox.Show("Specified folder not found");
                }
            }
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string key = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\key.txt", Encoding.UTF8);
            string[] keys = key.Split(',');

            if (checkBox1.Checked)
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\key.txt"))
                {
                    writetext.Write("on," + keys[1]);
                    checkBox1.ForeColor = Color.LightGreen;
                    checkBox1.Text = "Enabled";
                }
            }
            else
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\key.txt"))
                {
                    writetext.Write("off," + keys[1]);
                    checkBox1.ForeColor = Color.Red;
                    checkBox1.Text = "Disabled";
                }
            }
        }
        
        private void CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            try
            {
                Process.Start(_strFilePath + @"\" + folderBotName);
            }
            catch { }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string key = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\key.txt", Encoding.UTF8);
            string[] keys = key.Split(',');

            if (checkBox2.Checked)
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\key.txt"))
                {
                    writetext.Write(keys[0] + ",on");
                }
            }
            else
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\key.txt"))
                {
                    writetext.Write(keys[0] + ",off");
                    checkBox2.Checked = false;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            String hoyFecha = DateTime.Now.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');




            //Obtiene la ruta del ejecutable de la entidad-----------------------------------------
            string exe_path = "";
            string task_name = "";

            //Obtiene el modo de almacenamiento
            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                if (label10.Text == "N")
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + ".exe";
                    task_name = folderBotName;
                }
                else
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + "_" + label10.Text + ".exe";
                    task_name = folderBotName + "_" + label10.Text;
                }
            }
            else
            {
                if (label10.Text == "N")
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + ".exe";
                    task_name = folderBotName;
                }
                else
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_" + label10.Text + ".exe";
                    task_name = folderBotName + "_" + label10.Text;
                }
            }

            Process.Start(exe_path);


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form3 newForm3 = new Form3("sap");
            newForm3.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\pendings.txt"))
            {
                writetext.Write(textBox4.Text);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            List<string> creds = new List<string>();

            DialogResult dialogResult = MessageBox.Show("The Stretching Protocol, sir?", "JARVIS", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Directory.SetCurrentDirectory(_strFilePath + @"\ExtraJAQHES");
                Process.Start(_strFilePath + @"\ExtraJAQHES\StretchingProt.exe");

                Directory.SetCurrentDirectory(_strFilePath_ABS);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            String hoyFecha = DateTime.Now.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            try //Para tipo de almacenamiento 1
            {
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\ReglasDinamicas.xlsx");
            }
            catch
            {
                try //Para tipo de almacenamiento 2
                {
                    string folderBotNum = folderBotName.Substring(0, 2);
                    int num;
                    int.TryParse(folderBotNum.Substring(0, 2), out num);
                    string numStr = num.ToString().PadLeft(3, '0');
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\ReglasDinamicas.xlsx");
                }
                catch
                {
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string extrapath = File.ReadAllText(Directory.GetCurrentDirectory() + @"\extraPath.txt", Encoding.UTF8);
            string excepted_Jaqhes = File.ReadAllText(extrapath + @"\StretchingProt_RESET_except.txt", Encoding.UTF8);

            string message = "";
            if (excepted_Jaqhes =="")
            {
                message = "Sure u wanna kill all of JAQHES instances (+ related proccesses) with no exceptions? ";
            }
            else
            {
                message = "Sure u wanna kill all of JAQHES instances (+ related proccesses) excepting " + excepted_Jaqhes + ".exe? ";
            }
            

            DialogResult dialogResult = MessageBox.Show(message, "JARVIS", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    { FileName = extrapath + @"\StretchingProt_RESET.exe", Arguments = "", WorkingDirectory = extrapath }
                };
                process.Start();
                process.WaitForExit();

                /*
                List<String> Lista1 = new List<String>();

                string processes2kill = File.ReadAllText(Directory.GetCurrentDirectory() + @"\processes2kill.txt", Encoding.UTF8);
                string[] processes2kill_arr = processes2kill.Split(',');
                foreach (var item in processes2kill_arr)
                {
                    if (item != "")
                    {
                        string temp = item.Trim();
                        Lista1.Add(temp);
                    }
                }


                foreach (var item in Lista1)
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        { FileName = _strFilePath_ABS + @"\CurrentUserProcessKiller.exe", Arguments = item }
                    };
                    process.Start();
                    process.WaitForExit();
                }


                List<String> Lista2 = new List<String>();

                Lista2.Add("CMD");
                Lista2.Add("JAQHES");
                Lista2.Add("wscript");
                Lista2.Add("Quick_Healer");
                Lista2.Add("StretchingProt");

                foreach (var item in Lista2)
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        { FileName = _strFilePath_ABS + @"\ProcessKiller.exe", Arguments = item }
                    };
                    process.Start();
                    process.WaitForExit();
                }

                MessageBox.Show("All killed succesfully", "JARVIS");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM wscript.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM wscript.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM wscript.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM wscript.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM Quick_Healer.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM StretchingProt.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 01_JAQHES_V2.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES01.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM BOT_cantCalifs.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM BOT_WEKA.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 02_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES02.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 03_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES03.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 04_JAQHES_1.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 04_JAQHES_2.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES04.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 05_JAQHES_V2.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES05.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM Bring2Front.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 06_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES06.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 07_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES07.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 08_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES08.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 09_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES09.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 10_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES10.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 11_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES11.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 12_JAQHES_1.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 12_JAQHES_2.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES12.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 13_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES13.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 14_JAQHES_1.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 14_JAQHES_2.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES14.exe");

                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM 15_JAQHES.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM JAQHES15.exe");
                System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /F /IM BOT_DTS_2_2.exe");
                */
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            //Actualiza el IniDate y FinDate
            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                using (StreamWriter writetext = new StreamWriter(path + @"\inidate.txt"))
                {
                    writetext.Write(textBox5.Text);
                }
            }
            else
            {
                string path2 = _strFilePath + @"\" + folderBotName + @"\sys\";

                using (StreamWriter writetext = new StreamWriter(path2 + @"\inidate.txt"))
                {
                    writetext.Write(textBox5.Text);
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            //Actualiza el IniDate y FinDate
            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                using (StreamWriter writetext = new StreamWriter(path + @"\findate.txt"))
                {
                    writetext.Write(textBox6.Text);
                }
            }
            else
            {
                string path2 = _strFilePath + @"\" + folderBotName + @"\sys\";

                using (StreamWriter writetext = new StreamWriter(path2 + @"\findate.txt"))
                {
                    writetext.Write(textBox6.Text);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            compiler();
            MessageBox.Show("Compiled!", "JARVIS");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                string romanNum = ToRoman(Convert.ToInt32(num));
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\JAQHES_" + romanNum + ".sln");
            }
            else
            {
                try
                {
                    string romanNum = ToRoman(Convert.ToInt32(numStr));
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\JAQHES_" + numStr + @"\JAQHES_" + romanNum + ".sln");
                }
                catch
                {
                    MessageBox.Show("Specified project not found");
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            comboBox1.Text = "00";
            comboBox2.Text = "00";
            comboBox9.Text = "--";

            // define la ruta
            string ruta = "";
            if (Directory.Exists(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\"))
            {
                ruta = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            }
            else
            {
                ruta = _strFilePath + @"\" + folderBotName + @"\sys\";
            }

            // define la cantidad de instancias
            if (label10.Text == "N")
            {
                //No hace nada porque no hay instanacias enumeradas
            }
            else if (label10.Text == "1")
            {
                if (File.Exists(ruta + folderBotName + "_2.exe"))
                {
                    label10.Text = "2";
                }
            }
            else if (label10.Text == "2")
            {
                if (File.Exists(ruta + folderBotName + "_3.exe"))
                {
                    label10.Text = "3";
                }
                else
                {
                    label10.Text = "1";
                }
            }
            else if (label10.Text == "3")
            {
                if (File.Exists(ruta + folderBotName + "_4.exe"))
                {
                    label10.Text = "4";
                }
                else
                {
                    label10.Text = "1";
                }
            }
            else if (label10.Text == "4")
            {
                if (File.Exists(ruta + folderBotName + "_5.exe"))
                {
                    label10.Text = "5";
                }
                else
                {
                    label10.Text = "1";
                }
            }

            
            /*
            if (label10.Text == "1")
            {
                string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
                if (Directory.Exists(path))
                {
                    if (File.Exists(path + folderBotName + "_2.exe"))
                    {
                        label10.Text = "2";
                    }
                    else
                    {
                        if (File.Exists(path + folderBotName + ".exe"))
                        {
                            label10.Text = "N";
                        }
                        else
                        {
                            label10.Text = "1";
                        }
                    }
                }
                else
                {
                    if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_2.exe"))
                    {
                        label10.Text = "2";
                    }
                    else
                    {
                        if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + ".exe"))
                        {
                            label10.Text = "N";
                        }
                        else
                        {
                            label10.Text = "1";
                        }
                    }
                }
            }
            else if (label10.Text == "2")
            {
                string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
                if (Directory.Exists(path))
                {
                    if (File.Exists(path + folderBotName + "_1.exe"))
                    {
                        label10.Text = "1";
                    }
                    else
                    {
                        if (File.Exists(path + folderBotName + ".exe"))
                        {
                            label10.Text = "N";
                        }
                        else
                        {
                            label10.Text = "2";
                        }
                    }
                }
                else
                {
                    if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_1.exe"))
                    {
                        label10.Text = "1";
                    }
                    else
                    {
                        if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + ".exe"))
                        {
                            label10.Text = "N";
                        }
                        else
                        {
                            label10.Text = "2";
                        }
                    }
                }
            }
            else if (label10.Text == "N")
            {
                string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
                if (Directory.Exists(path))
                {
                    if (File.Exists(path + folderBotName + "_1.exe"))
                    {
                        label10.Text = "1";
                    }
                    else
                    {
                        if (File.Exists(path + folderBotName + "_2.exe"))
                        {
                            label10.Text = "2";
                        }
                        else
                        {
                            label10.Text = "N";
                        }
                    }
                }
                else
                {
                    if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_1.exe"))
                    {
                        label10.Text = "1";
                    }
                    else
                    {
                        if (File.Exists(_strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_2.exe"))
                        {
                            label10.Text = "2";
                        }
                        else
                        {
                            label10.Text = "N";
                        }
                    }
                }
            }
            */



            //Actualiza grid2
            string folderBotName_2 = "";
            if (label10.Text == "N")
            {
                folderBotName_2 = folderBotName;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;
                    }
                }
            }
            else if (label10.Text != "X")
            {
                folderBotName_2 = folderBotName + "_" + label10.Text;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;
                    }
                }
            }
            else
            {
                // No llena ninguna informacion
            }

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();


            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            string time = comboBox1.Text + ":" + comboBox2.Text;

            string triggerType = comboBox9.Text;
            if (label10.Text == "X")
            {
                MessageBox.Show("EXE does not exist!", "JARVIS");
                return;
            }


            //Obtiene la ruta del ejecutable de la entidad-----------------------------------------
            string exe_path = "";
            string task_name = "";

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            //Obtiene el modo de almacenamiento
            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                if (label10.Text == "N")
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + ".exe";
                    task_name = folderBotName;
                }
                else
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + "_" + label10.Text + ".exe";
                    task_name = folderBotName + "_" + label10.Text;
                }
            }
            else
            {
                if (label10.Text == "N")
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + ".exe";
                    task_name = folderBotName;
                }
                else
                {
                    exe_path = _strFilePath + @"\" + folderBotName  + @"\sys\" + folderBotName + "_" + label10.Text + ".exe";
                    task_name = folderBotName + "_" + label10.Text;
                }
            }
            




           



            int task_index = 0;
            if (checkBox3.Checked == false)
            {
                try
                {
                    task_index = dataGridView2.CurrentCell.RowIndex;
                }
                catch
                {
                    MessageBox.Show("No trigger selected!", "JARVIS");
                    return;
                }
            }

            if (comboBox9.Text == "--")
            {
                MessageBox.Show("No frequency selected!", "JARVIS");
                return;
            }



            if (File.Exists(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt"))
            {
                File.Delete(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt");
            }
            if (File.Exists(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt"))
            {
                File.Delete(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt");
            }

            //Identifica el nombre del TASK-----------------------------------------

            /*
            if (label10.Text == "1")
            {
                if (checkBox3.Checked != true)
                {
                    try
                    {
                        using (TaskService ts = new TaskService())
                        {
                            TaskDefinition td = ts.GetTask(folderBotName + "_1").Definition;
                            folderBotName = folderBotName + "_1";
                        }
                    }
                    catch
                    {
                        using (TaskService ts = new TaskService())
                        {
                            TaskDefinition td = ts.GetTask(folderBotName).Definition;
                        }
                    }
                }
                else
                {
                    folderBotName = folderBotName;
                }
            }
            else
            {
                if (checkBox3.Checked != true)
                {
                    using (TaskService ts = new TaskService())
                    {
                        TaskDefinition td = ts.GetTask(folderBotName + "_2").Definition;
                        folderBotName = folderBotName + "_2";
                    }
                }
                else
                {
                    folderBotName = folderBotName + "_2";
                }
            }
            */
            //Identifica el nombre del TASK-----------------------------------------


            string days = "";
            string months = "";
            string daysOfMonths = "";

            string cadena = "";

            if (triggerType == "Daily")
            {
                cadena = task_name + "," + task_index + "," + triggerType + "," + time + ",0,00,0,00";
            }
            else if (triggerType == "Weekly")
            {
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    if (Convert.ToString(itemChecked) == "Monday")
                    {
                        days = days + "1";
                    }
                    if (Convert.ToString(itemChecked) == "Tuesday")
                    {
                        days = days + "2";
                    }
                    if (Convert.ToString(itemChecked) == "Wednesday")
                    {
                        days = days + "3";
                    }
                    if (Convert.ToString(itemChecked) == "Thursday")
                    {
                        days = days + "4";
                    }
                    if (Convert.ToString(itemChecked) == "Friday")
                    {
                        days = days + "5";
                    }
                    if (Convert.ToString(itemChecked) == "Saturday")
                    {
                        days = days + "6";
                    }
                    if (Convert.ToString(itemChecked) == "Sunday")
                    {
                        days = days + "7";
                    }
                }
                cadena = task_name + "," + task_index + "," + triggerType + "," + days + "," + time + ",0,00,0,00";
            }
            else if (triggerType == "Monthly")
            {
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    if (Convert.ToString(itemChecked) == "January")
                    {
                        months = months + "01.";
                    }
                    if (Convert.ToString(itemChecked) == "February")
                    {
                        months = months + "02.";
                    }
                    if (Convert.ToString(itemChecked) == "March")
                    {
                        months = months + "03.";
                    }
                    if (Convert.ToString(itemChecked) == "April")
                    {
                        months = months + "04.";
                    }
                    if (Convert.ToString(itemChecked) == "May")
                    {
                        months = months + "05.";
                    }
                    if (Convert.ToString(itemChecked) == "June")
                    {
                        months = months + "06.";
                    }
                    if (Convert.ToString(itemChecked) == "July")
                    {
                        months = months + "07.";
                    }
                    if (Convert.ToString(itemChecked) == "August")
                    {
                        months = months + "08.";
                    }
                    if (Convert.ToString(itemChecked) == "September")
                    {
                        months = months + "09.";
                    }
                    if (Convert.ToString(itemChecked) == "October")
                    {
                        months = months + "10.";
                    }
                    if (Convert.ToString(itemChecked) == "November")
                    {
                        months = months + "11.";
                    }
                    if (Convert.ToString(itemChecked) == "December")
                    {
                        months = months + "12.";
                    }
                }

                foreach (object itemChecked in checkedListBox2.CheckedItems)
                {
                    for (int h = 1 ; h <= 31 ; h++)
                    {
                        string nroDia = h.ToString("00");

                        if (Convert.ToInt16(itemChecked).ToString("00") == nroDia)
                        {
                            daysOfMonths = daysOfMonths + nroDia;
                        }
                    }
                }
                cadena = cadena = task_name + "," + task_index + "," + triggerType + "," + months + "," + daysOfMonths + "," + time + ",0,00,0,00";

            }




            if (checkBox3.Checked == true)
            {
                //Añade la ruta de ejecucion
                cadena = cadena + "," + exe_path;

                using (StreamWriter writetext = File.AppendText(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt"))
                {
                    writetext.Write(cadena);
                }

                using (StreamWriter writetext = File.AppendText(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt"))
                {
                    writetext.Write("cre");
                }
            }
            else
            {
                using (StreamWriter writetext = File.AppendText(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt"))
                {
                    writetext.Write(cadena);
                }

                using (StreamWriter writetext = File.AppendText(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt"))
                {
                    writetext.Write("upd");
                }
            }
           

            executeAsAdmin(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\JARVIS_TASKER.exe", _strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug");

            //Recarga el Grid2
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            //Actualiza grid2
            string folderBotName_2 = "";
            if (label10.Text == "N")
            {
                folderBotName_2 = folderBotName;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;
                    }
                }
            }
            else if (label10.Text != "X")
            {
                folderBotName_2 = folderBotName + "_" + label10.Text;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;
                    }
                }
            }
            else
            {
                // No llena ninguna informacion
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            try
            {
                if (Directory.Exists(path))
                {
                    var directories = Directory.GetDirectories(path);
                    List<DateTime> folderDateTime = new List<DateTime>();

                    foreach (var item in directories)
                    {
                        string folder = item.Substring(item.Length - 10, 10);
                        try
                        {
                            DateTime folderTemp = Convert.ToDateTime(folder);
                            folderDateTime.Add(folderTemp);
                        }
                        catch
                        { }
                    }
                    folderDateTime.Sort((a, b) => b.CompareTo(a));

                    String latestFolder = folderDateTime.First().ToString("dd-MM-yyyy");

                    Process.Start(path + @"\" + latestFolder);
                }
                else
                {
                    var directories = Directory.GetDirectories(_strFilePath + @"\" + folderBotName + @"\sys\");
                    List<DateTime> folderDateTime = new List<DateTime>();

                    foreach (var item in directories)
                    {
                        string folder = item.Substring(item.Length - 10, 10);
                        try
                        {
                            DateTime folderTemp = Convert.ToDateTime(folder);
                            folderDateTime.Add(folderTemp);
                        }
                        catch
                        { }
                    }
                    folderDateTime.Sort((a, b) => b.CompareTo(a));

                    String latestFolder = folderDateTime.First().ToString("dd-MM-yyyy");

                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\" + @"\" + latestFolder);
                }
            }
            catch
            {
                MessageBox.Show("Specified folder not found");
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            // Compila todos los ahk de todos los JAQHES
            for (int nRow = 0; nRow < dataGridView1.Rows.Count; nRow++)
            {
                if (nRow == 0)
                {
                    dataGridView1.Rows[0].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
                }
                else
                {
                    dataGridView1.Rows[nRow - 1].Selected = false;
                    dataGridView1.Rows[nRow].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[nRow].Cells[1];
                }

                compiler();
            }
            MessageBox.Show("All compiled!", "JARVIS");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            string ahk_compiler_cmd = File.ReadAllText(Directory.GetCurrentDirectory() + @"\ahk_compiler_cmd.txt", Encoding.UTF8);

            List<String> ahk_files = new List<String>();
            ahk_files.Add(_strFilePath + @"\ExtraJAQHES\StretchingProt.ahk");
            ahk_files.Add(_strFilePath + @"\ExtraJAQHES\StretchingProt_RESET.ahk");
            ahk_files.Add(_strFilePath + @"\ExtraJAQHES\StretchingProt_SOCKET_24x7.ahk");


            foreach (string item in ahk_files)
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(ahk_compiler_cmd + " " + item);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            }

            MessageBox.Show("All Stretching Protocols compiled!", "JARVIS");
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

                String hoyDate = DateTime.Now.ToString("dd/MM/yyyy"); //Coloca la fecha de hoy
                String hoyTime = DateTime.Now.ToString("hh:mm:ss"); //Coloca la fecha de hoy

                int rowindex = dataGridView1.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

                string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

                using (StreamWriter writetext = File.AppendText(_strFilePath + @"\" + folderBotName + @"\log.txt"))
                {
                    writetext.WriteLine(hoyTime + " - " + hoyDate + " - [" + textBox3.Text + "]");
                }
                textBox3.Text = "";
                //Actualiza el LOG
                string log = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\log.txt", Encoding.UTF8);
                textBox1.Text = log;
            }
        }

        //FUNCTIONSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        //FUNCTIONSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        //FUNCTIONSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        //FUNCTIONSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public void compiler()
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = "";
            if (Directory.Exists(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\"))
            {
                path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            }
            else
            {
                path = _strFilePath + @"\" + folderBotName + @"\sys\";
            }

            string ahk_compiler_cmd = File.ReadAllText(Directory.GetCurrentDirectory() + @"\ahk_compiler_cmd.txt", Encoding.UTF8);

            string[] ahk_files = Directory.GetFiles(path, "*.ahk");
            foreach (string item in ahk_files)
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(ahk_compiler_cmd + " " + item);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            }

            //MessageBox.Show("Termino!");
        }

        public void TaskSchedulerReader(Microsoft.Win32.TaskScheduler.Task task)
        {
            if (label10.Text != "0")
            {
                TaskDefinition td = task.Definition;

                List<CustomTask> LstobjCustomTask = new List<CustomTask>();

                int number = 0;
                foreach (Microsoft.Win32.TaskScheduler.Trigger trigger0 in task.Definition.Triggers)
                {
                    //CustomTask objCustomTask = new CustomTask();
                    if (Convert.ToString(trigger0.TriggerType) == "Daily")
                    {
                        DailyTrigger trigger = trigger0 as DailyTrigger;

                        CustomTask objCustomTask = new CustomTask();

                        objCustomTask.TaskFreq = "Daily";
                        objCustomTask.TaskStatus = trigger.Enabled.ToString();
                        objCustomTask.TaskDate = trigger.StartBoundary.ToString("dd/MM/yyyy");
                        objCustomTask.TaskHourTrigger = trigger.StartBoundary.ToString("HH:mm:ss");
                        objCustomTask.TaskRepetition = trigger.Repetition.Interval.ToString();
                        objCustomTask.TaskRepetitionDuration = trigger.Repetition.Duration.ToString();

                        LstobjCustomTask.Add(objCustomTask);
                    }
                    else if (Convert.ToString(trigger0.TriggerType) == "Weekly")
                    {
                        WeeklyTrigger trigger = trigger0 as WeeklyTrigger;

                        CustomTask objCustomTask = new CustomTask();

                        objCustomTask.TaskFreq = "Weekly";
                        objCustomTask.TaskStatus = trigger.Enabled.ToString();
                        objCustomTask.TaskDate = trigger.StartBoundary.ToString("dd/MM/yyyy");
                        objCustomTask.TaskHourTrigger = trigger.StartBoundary.ToString("HH:mm:ss");
                        objCustomTask.TaskRepetition = trigger.Repetition.Interval.ToString();
                        objCustomTask.TaskRepetitionDuration = trigger.Repetition.Duration.ToString();

                        string[] Day3 = trigger.DaysOfWeek.ToString().Split(',');
                        string Day = Day3[0].Trim().Substring(0, 2);
                        for (int h = 1; h < Day3.Count(); h++)
                        {
                            Day = Day + "," + Day3[h].Trim().Substring(0, 2);
                        }
                        objCustomTask.TaskDaysOfWeek = Day;

                        LstobjCustomTask.Add(objCustomTask);
                    }
                    else if (Convert.ToString(trigger0.TriggerType) == "Monthly")
                    {
                        MonthlyTrigger trigger = trigger0 as MonthlyTrigger;

                        CustomTask objCustomTask = new CustomTask();

                        objCustomTask.TaskFreq = "Monthly";
                        objCustomTask.TaskStatus = trigger.Enabled.ToString();
                        objCustomTask.TaskDate = trigger.StartBoundary.ToString("dd/MM/yyyy");
                        objCustomTask.TaskHourTrigger = trigger.StartBoundary.ToString("HH:mm:ss");
                        objCustomTask.TaskRepetition = trigger.Repetition.Interval.ToString();
                        objCustomTask.TaskRepetitionDuration = trigger.Repetition.Duration.ToString();


                        string Days = "";
                        foreach (var item0 in trigger.DaysOfMonth)
                        {
                            if (Days == "")
                            {
                                Days = Convert.ToString(item0);
                            }
                            else
                            {
                                Days = Days + "," + item0;
                            }
                        }
                        objCustomTask.TaskDaysOfMonth = Days;

                        string[] Month3 = trigger.MonthsOfYear.ToString().Split(',');
                        string Month = Month3[0].Trim().Substring(0, 3);
                        for (int h = 1; h < Month3.Count(); h++)
                        {
                            Month = Month + "," + Month3[h].Trim().Substring(0, 3);
                        }
                        objCustomTask.TaskMonthsOfYear = Month;

                        LstobjCustomTask.Add(objCustomTask);
                    }
                    number++;
                }




                // Carga el DataGridView2 (de Taks)
                foreach (var item in LstobjCustomTask)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = item.TaskStatus;
                    row.Cells[1].Value = item.TaskFreq;
                    //arma el hora/dias/meses
                    string HoraDiasMeses = item.TaskHourTrigger;
                    if (item.TaskDaysOfWeek != null)
                    {
                        HoraDiasMeses = HoraDiasMeses + "/" + item.TaskDaysOfWeek;
                    }
                    if (item.TaskDaysOfMonth != null)
                    {
                        HoraDiasMeses = HoraDiasMeses + "/" + item.TaskDaysOfMonth + "/" + item.TaskMonthsOfYear;
                    }
                    row.Cells[2].Value = HoraDiasMeses;
                    dataGridView2.Rows.Add(row);
                }
            }
        }

        public void TaskSchedulerEditor(Microsoft.Win32.TaskScheduler.Task task, string triggerType)
        {
            //MessageBox.Show(folderBotName);
            TaskDefinition td = task.Definition;
            int vez = 1;
            foreach (Microsoft.Win32.TaskScheduler.Trigger trigger0 in task.Definition.Triggers)
            {
                if (Convert.ToString(trigger0.TriggerType) == "Daily" && triggerType == "Daily")
                {
                    checkedListBox1.Visible = false;
                    checkedListBox2.Visible = false;
                    foreach (DailyTrigger trigger in task.Definition.Triggers)
                    {
                        if (vez == 1)
                        {

                            string date = trigger.StartBoundary.ToString("HH:mm:ss");
                            string[] date2 = date.Split(':');
                            comboBox1.Text = date2[0];
                            comboBox2.Text = date2[1];

                            comboBox9.Text = Convert.ToString(trigger.TriggerType);

                            //CheckListBox
                            checkedListBox1.Items.Clear();

                            vez++;
                        }
                    }
                }
                else if (Convert.ToString(trigger0.TriggerType) == "Weekly" && triggerType == "Weekly")
                {
                    checkedListBox1.Visible = true;
                    checkedListBox2.Visible = false;
                    foreach (WeeklyTrigger trigger in task.Definition.Triggers)
                    {
                        if (vez == 1)
                        {

                            string date = trigger.StartBoundary.ToString("HH:mm:ss");
                            string[] date2 = date.Split(':');
                            comboBox1.Text = date2[0];
                            comboBox2.Text = date2[1];


                            comboBox9.Text = Convert.ToString(trigger.TriggerType);

                            //CheckListBox
                            string[] Weekdays = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                            checkedListBox1.Items.Clear();
                            var items = checkedListBox1.Items;
                            foreach (var item in Weekdays)
                            {
                                if (trigger.DaysOfWeek.ToString().Contains(item))
                                {
                                    items.Add(item, true);
                                }
                                else
                                {
                                    items.Add(item, false);
                                }
                            }

                            vez++;
                        }

                    }
                }
                else if (Convert.ToString(trigger0.TriggerType) == "Monthly" && triggerType == "Monthly")
                {
                    checkedListBox1.Visible = true;
                    checkedListBox2.Visible = true;
                    foreach (MonthlyTrigger trigger in task.Definition.Triggers)
                    {
                        if (vez == 1)
                        {

                            string date = trigger.StartBoundary.ToString("HH:mm:ss");
                            string[] date2 = date.Split(':');
                            comboBox1.Text = date2[0];
                            comboBox2.Text = date2[1];



                            //CheckListBox2
                            string[] days = new string[31] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                            checkedListBox2.Items.Clear();
                            var items2 = checkedListBox2.Items;
                            foreach (var item in days)
                            {
                                bool comprobacion = false;
                                foreach (var item0 in trigger.DaysOfMonth)
                                {
                                    if (item0 == Convert.ToInt32(item))
                                    {
                                        items2.Add(item, true);
                                        comprobacion = true;
                                        break;
                                    }
                                }
                                if (comprobacion == false)
                                {
                                    items2.Add(item, false);
                                }
                            }



                            comboBox9.Text = Convert.ToString(trigger.TriggerType);

                            //CheckListBox
                            string[] Months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                            checkedListBox1.Items.Clear();
                            var items = checkedListBox1.Items;
                            foreach (var item in Months)
                            {
                                if (trigger.MonthsOfYear.ToString().Contains(item) || trigger.MonthsOfYear.ToString().Contains("AllMonths"))
                                {
                                    items.Add(item, true);
                                }
                                else
                                {
                                    items.Add(item, false);
                                }
                            }

                            vez++;
                        }
                    }
                }
            }
            //ts.RootFolder.RegisterTaskDefinition("YourTaskName", td);
        }

        private void executeAsMortal(string file)
        {
            System.Security.SecureString password = new System.Security.SecureString();

            char[] characters = "Descart3s1812".ToCharArray();

            for (int o = 0; o < characters.Count(); o++)
            {
                password.AppendChar(characters[o]);
            }

            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = file;
            myProcess.StartInfo.UserName = Environment.UserName;
            myProcess.StartInfo.Domain = Environment.UserDomainName;
            myProcess.StartInfo.Password = password;
            myProcess.Start();
        }

        public void executeAsAdmin(string fileName, string at)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.WorkingDirectory = at;
                proc.StartInfo.Verb = "runas";
                proc.Start();

                proc.WaitForExit();
            }
            catch
            {
                MessageBox.Show("Canceled", "JARVIS TASKER");
            }
            
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            try
            {
                if (Directory.Exists(path))
                {
                    var directories = Directory.GetDirectories(path);
                    List<DateTime> folderDateTime = new List<DateTime>();

                    foreach (var item in directories)
                    {
                        string folder = item.Substring(item.Length - 10, 10);
                        try
                        {
                            DateTime folderTemp = Convert.ToDateTime(folder);
                            folderDateTime.Add(folderTemp);
                        }
                        catch
                        { }
                    }
                    folderDateTime.Sort((a, b) => b.CompareTo(a));

                    String latestFolder = folderDateTime.First().ToString("dd-MM-yyyy");
                    String latestDate = folderDateTime.First().ToString("dd_MM_yyyy");

                    Process.Start(path + @"\" + latestFolder + @"\Log_" + latestDate + ".txt");
                }
                else
                {
                    var directories = Directory.GetDirectories(_strFilePath + @"\" + folderBotName + @"\sys\");
                    List<DateTime> folderDateTime = new List<DateTime>();

                    foreach (var item in directories)
                    {
                        string folder = item.Substring(item.Length - 10, 10);
                        try
                        {
                            DateTime folderTemp = Convert.ToDateTime(folder);
                            folderDateTime.Add(folderTemp);
                        }
                        catch
                        { }
                    }
                    folderDateTime.Sort((a, b) => b.CompareTo(a));

                    String latestFolder = folderDateTime.First().ToString("dd-MM-yyyy");
                    String latestDate = folderDateTime.First().ToString("dd_MM_yyyy");

                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\" + @"\" + latestFolder + @"\Log_" + latestDate + ".txt");
                }
            }
            catch
            {
                MessageBox.Show("Specified log not found");
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            if (comboBox9.Text == "Daily")
            {
                //CheckListBox
                checkedListBox1.Items.Clear();
                checkedListBox2.Items.Clear();

                checkedListBox1.Visible = false;
                checkedListBox2.Visible = false;
            }
            else if (comboBox9.Text == "Weekly")
            {
                //CheckListBox
                string[] Weekdays = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                checkedListBox1.Items.Clear();
                checkedListBox2.Items.Clear();
                var items = checkedListBox1.Items;
                foreach (var item in Weekdays)
                {
                    items.Add(item, false);
                }

                checkedListBox1.Visible = true;
                checkedListBox2.Visible = false;
            }
            else if (comboBox9.Text == "Monthly")
            {
                //CheckListBox
                string[] Months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                checkedListBox1.Items.Clear();
                var items = checkedListBox1.Items;
                foreach (var item in Months)
                {
                    items.Add(item, false);
                }

                //CheckListBox2
                string[] days = new string[31] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                checkedListBox2.Items.Clear();
                var items2 = checkedListBox2.Items;
                foreach (var item in days)
                {
                    items2.Add(item, false);
                }

                checkedListBox1.Visible = true;
                checkedListBox2.Visible = true;
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            try
            {
                Process.Start(_strFilePath + @"\" + folderBotName + @"\FlowDiag.png");
            }
            catch
            {
                MessageBox.Show("No workflow");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string key = File.ReadAllText(_strFilePath + @"\" + folderBotName + @"\key.txt", Encoding.UTF8);
            string[] keys = key.Split(',');

            if (checkBox1.Checked)
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\key.txt"))
                {
                    writetext.Write("on," + keys[1]);
                    checkBox1.ForeColor = Color.LightGreen;
                    checkBox1.Text = "Enabled";
                }
            }
            else
            {
                using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\key.txt"))
                {
                    writetext.Write("off," + keys[1]);
                    checkBox1.ForeColor = Color.Red;
                    checkBox1.Text = "Disabled";
                }
            }


            Form1.hh1 = Convert.ToInt16(comboBox1.Text);
            Form1.mm1 = Convert.ToInt16(comboBox2.Text);

            if (Form1.mm1 + Convert.ToInt16(comboBox3.Text) >=60)
            {
                int horas_ext = (Form1.mm1 + Convert.ToInt16(comboBox3.Text)) / 60;
                Form1.hh2 = Form1.hh1 + Convert.ToInt16(comboBox4.Text) + horas_ext;
                Form1.mm2 = Form1.mm1 + Convert.ToInt16(comboBox3.Text) - 60;
            }
            else
            {
                Form1.hh2 = Form1.hh1 + Convert.ToInt16(comboBox4.Text);
                Form1.mm2 = Form1.mm1 + Convert.ToInt16(comboBox3.Text);
            }

            Form4 newForm4 = new Form4();
            newForm4.ShowDialog();
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\dur.txt"))
            {
                writetext.Write(comboBox4.Text + "," + comboBox3.Text);
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            using (StreamWriter writetext = new StreamWriter(_strFilePath + @"\" + folderBotName + @"\dur.txt"))
            {
                writetext.Write(comboBox4.Text + "," + comboBox3.Text);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            //int screenWidth = 1920;
            int screenHeight = SystemInformation.VirtualScreen.Height;
            //int screenHeight = 1080;
            int x = Screen.PrimaryScreen.WorkingArea.Width;

            int y = Screen.PrimaryScreen.WorkingArea.Height;

            MessageBox.Show(screenLeft + "," + screenTop + "," + x + "," + y);
            
            // Create a bitmap of the appropriate size to receive the full-screen screenshot.
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap.Size);
                }

                //Save the screenshot as a Jpg image
                var uniqueFileName = "C:\\temp\\a.Jpg";
                try
                {
                    bitmap.Save(uniqueFileName, ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Form5 newForm5 = new Form5();
            newForm5.ShowDialog();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            //Actualiza el IniDate y FinDate
            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                using (StreamWriter writetext = new StreamWriter(path + @"\Mode.txt"))
                {
                    writetext.Write(textBox9.Text);
                }
            }
            else
            {
                string path2 = _strFilePath + @"\" + folderBotName + @"\sys\";

                using (StreamWriter writetext = new StreamWriter(path2 + @"\Mode.txt"))
                {
                    writetext.Write(textBox9.Text);
                }
            }
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                { FileName = _strFilePath_ABS + @"\CurrentUserProcessKiller.exe", Arguments = "wscript" }
            };
            process.Start();
            process.WaitForExit();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Form6 newForm6 = new Form6();
            newForm6.ShowDialog();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\");
            }
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            //define la ruta
            string path = "";
            string path_0 = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path_0))
            {
                path = path_0;
            }
            else
            {
                path = _strFilePath + @"\" + folderBotName + @"\sys\\";
            }

            //string editor_name = "notepad++";
            string editor_name = "D:\\Program Files\\Microsoft VS Code\\Code";

            //Verifica si existe(n) el/los ahk
            if (File.Exists(path + folderBotName + ".ahk"))
            {
                Process.Start(editor_name+".exe", path + folderBotName + ".ahk");
            }
            else
            {
                if (File.Exists(path + folderBotName + "_1.ahk"))
                {
                    Process.Start(editor_name + ".exe", path + folderBotName + "_1.ahk");
                }
                if (File.Exists(path + folderBotName + "_2.ahk"))
                {
                    Process.Start(editor_name + ".exe", path + folderBotName + "_2.ahk");
                }
                if (File.Exists(path + folderBotName + "_3.ahk"))
                {
                    Process.Start(editor_name + ".exe", path + folderBotName + "_3.ahk");
                }
                if (File.Exists(path + folderBotName + "_4.ahk"))
                {
                    Process.Start(editor_name + ".exe", path + folderBotName + "_4.ahk");
                }
                if (File.Exists(path + folderBotName + "_5.ahk"))
                {
                    Process.Start(editor_name + ".exe", path + folderBotName + "_4.ahk");
                }
            }
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                string romanNum = ToRoman(Convert.ToInt32(num));
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\JAQHES_" + romanNum + ".sln");
            }
            else
            {
                try
                {
                    string romanNum = ToRoman(Convert.ToInt32(numStr));
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\JAQHES_" + numStr + @"\JAQHES_" + romanNum + ".sln");
                }
                catch
                {
                    MessageBox.Show("Specified project not found");
                }
            }
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                int rowindex = dataGridView2.CurrentCell.RowIndex;

                //Lena la hora
                string date = dataGridView2.Rows[rowindex].Cells[2].Value.ToString();
                string[] date2 = date.Split(':');
                comboBox1.Text = date2[0];
                comboBox2.Text = date2[1];

                //Llena la freq
                string freq = dataGridView2.Rows[rowindex].Cells[1].Value.ToString();
                comboBox9.Text = freq;

                //Llena los DaysOfWeek
                if (freq=="Weekly")
                {
                    string[] temp = date2[2].Split('/');
                    string[] DaysWeeks = temp[1].Split(',');

                    string[] Weekdays = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                    checkedListBox1.Items.Clear();
                    var items = checkedListBox1.Items;
                    foreach (var item in Weekdays)
                    {
                        bool flag = false;
                        foreach (var item2 in DaysWeeks)
                        {
                            if (item.Substring(0,2) == item2)
                            {
                                flag = true;
                            }
                        }

                        if (flag)
                        {
                            items.Add(item, true);
                        }
                        else
                        {
                            items.Add(item, false);
                        }
                    }
                }
                //Llena los DaysOfMonth y MonthsOfYear
                else if (freq == "Monthly")
                {
                    //DaysOfMonth
                    string[] temp = date2[2].Split('/');
                    string[] DaysMonths = temp[1].Split(',');

                    List<string> MonthDays = new List<string>();

                    for (int k = 1; k <= 31; k++)
                    {
                        MonthDays.Add(Convert.ToString(k));
                    }

                    checkedListBox2.Items.Clear();
                    var items = checkedListBox2.Items;
                    foreach (var item in MonthDays)
                    {
                        bool flag = false;
                        foreach (var item2 in DaysMonths)
                        {
                            if (item == item2)
                            {
                                flag = true;
                            }
                        }

                        if (flag)
                        {
                            items.Add(item, true);
                        }
                        else
                        {
                            items.Add(item, false);
                        }
                    }
                    //MonthsOfYear
                    string[] temp2 = date2[2].Split('/');
                    string[] MonthsYear = temp2[2].Split(',');

                    string[] Months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                    checkedListBox1.Items.Clear();
                    var items2 = checkedListBox1.Items;
                    foreach (var item in Months)
                    {
                        bool flag = false;
                        foreach (var item2 in MonthsYear)
                        {
                            if (item.Substring(0, 3) == item2 || item2 == "All")
                            {
                                flag = true;
                            }
                        }

                        if (flag)
                        {
                            items2.Add(item, true);
                        }
                        else
                        {
                            items2.Add(item, false);
                        }
                    }
                }
                else if (freq == "")
                {
                    comboBox1.Text = "00";
                    comboBox2.Text = "00";
                    comboBox9.Text = "--";

                    checkBox3.Checked = false;
                }

            }
            catch
            {
                comboBox1.Text = "00";
                comboBox2.Text = "00";
                comboBox9.Text = "--";

                checkBox3.Checked = false;
            }
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();


            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            string time = comboBox1.Text + ":" + comboBox2.Text;

            string triggerType = comboBox9.Text;


            if (label10.Text == "X")
            {
                MessageBox.Show("EXE does not exist!", "JARVIS");
                return;
            }


            //Obtiene la ruta del ejecutable de la entidad-----------------------------------------
            string exe_path = "";
            string task_name = "";

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            //Obtiene el modo de almacenamiento
            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                if (label10.Text == "N")
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + ".exe";
                    task_name = folderBotName;
                }
                else
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + folderBotName + "_" + label10.Text + ".exe";
                    task_name = folderBotName + "_" + label10.Text;
                }
            }
            else
            {
                if (label10.Text == "N")
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + ".exe";
                    task_name = folderBotName;
                }
                else
                {
                    exe_path = _strFilePath + @"\" + folderBotName + @"\sys\" + folderBotName + "_" + label10.Text + ".exe";
                    task_name = folderBotName + "_" + label10.Text;
                }
            }




            int task_index = 0;
            try
            {
                task_index = dataGridView2.CurrentCell.RowIndex;
            }
            catch
            {
                MessageBox.Show("No trigger selected!", "JARVIS");
                return;
            }


            if (File.Exists(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt"))
            {
                File.Delete(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt");
            }
            if (File.Exists(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt"))
            {
                File.Delete(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt");
            }

            string cadena = "";
            using (StreamWriter writetext = File.AppendText(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\parameters.txt"))
            {
                cadena = task_name + "," + task_index;


                writetext.Write(cadena);
            }


            using (StreamWriter writetext = File.AppendText(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\mode.txt"))
            {
                writetext.Write("del");
            }

            executeAsAdmin(_strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug\JARVIS_TASKER.exe", _strFilePath + @"\JARVIS\TaskSchdlr\JARVIS\bin\Debug");

            //Recarga el Grid2
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            //Actualiza grid2
            string folderBotName_2 = "";
            if (label10.Text == "N")
            {
                folderBotName_2 = folderBotName;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;
                    }
                }
            }
            else if (label10.Text != "X")
            {
                folderBotName_2 = folderBotName + "_" + label10.Text;

                using (TaskService ts = new TaskService())
                {
                    try
                    {
                        TaskSchedulerReader(ts.GetTask(folderBotName_2));
                    }
                    catch
                    {
                        comboBox1.Text = "00";
                        comboBox2.Text = "00";
                        comboBox9.Text = "--";

                        checkBox3.Checked = false;
                    }
                }
            }
            else
            {
                // No llena ninguna informacion
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            try
            {
                Process.Start(_strFilePath + @"\JARVIS\");
            }
            catch { }
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);


            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            string folderBotNum = folderBotName.Substring(0, 2);
            int num;
            int.TryParse(folderBotNum.Substring(0, 2), out num);
            string numStr = num.ToString().PadLeft(3, '0');

            string path = _strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\";
            if (Directory.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                try
                {
                    string path_2 = _strFilePath + @"\" + folderBotName + @"\sys\JAQHES_" + numStr + @"\jaqhes_" + numStr + @"\bin\Debug\";
                    Process.Start(path_2);
                }
                catch
                {
                    MessageBox.Show("Specified project not found");
                }
            }
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            try
            {
                Process.Start(_strFilePath + @"\ExtraJAQHES\");
            }
            catch { }
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            Form7 newForm7 = new Form7();
            newForm7.ShowDialog();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            Form3 newForm3 = new Form3("windows");
            newForm3.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            String hoyFecha = DateTime.Now.ToString("dd-MM-yyyy"); //Coloca la fecha de hoy

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();

            try //Para tipo de almacenamiento 1 (con batch para ejecucion de c#)
            {
                DirectoryInfo di = Directory.CreateDirectory(_strFilePath + @"\" + folderBotName + @"\sys\" + hoyFecha);
                Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\" + hoyFecha);
            }
            catch
            {
                try //Para tipo de almacenamiento 2 (ejecucion directa de c#)
                {
                    string folderBotNum = folderBotName.Substring(0, 2);
                    int num;
                    int.TryParse(folderBotNum.Substring(0, 2), out num);
                    string numStr = num.ToString().PadLeft(3, '0');

                    DirectoryInfo di = Directory.CreateDirectory(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + hoyFecha);
                    Process.Start(_strFilePath + @"\" + folderBotName + @"\sys\jaqhes_" + numStr + @"\bin\Debug\" + hoyFecha);
                }
                catch
                {
                    MessageBox.Show("Could not create the folder!");
                }
            }

            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + @"\manager_killer.ahk");
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            //Abre el excel de correlativos
            //Process.Start(@"D:\Mis Documentos D\Documentos EVA\001_Correlativos.xlsx");

            Form8 newForm8 = new Form8();
            newForm8.Show();

            //(new System.Threading.Thread(() => {(new Form8()).Show();})).Start();

        }
    }
}
