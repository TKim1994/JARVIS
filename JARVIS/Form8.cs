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

using System.Drawing;

using System.Reflection;

using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Diagnostics;

using System.Media;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace JARVISNamespace
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        List<Correlativo> LstCorrelativos = new List<Correlativo>();
        List<string> Lst_Categorias_ID = new List<string>();
        private void Form8_Load(object sender, EventArgs e)
        {
            Form8_Load_2();
        }

        private void Form8_Load_2()
        {
            string PC_name = System.Environment.MachineName;
            //

            string _strCorrelativos_xlsx = File.ReadAllText(Directory.GetCurrentDirectory() + @"\001_Correlativos_path.txt", Encoding.UTF8);

            if (PC_name == "FRIDAY10")
            {
                _strCorrelativos_xlsx = _strCorrelativos_xlsx.Replace("My Drive", "Mi unidad");
            }
            else if (PC_name == "FRIDAY8")
            {
                _strCorrelativos_xlsx = _strCorrelativos_xlsx.Replace("Mi unidad", "My Drive");
            }

            string _strPendientes_folder = _strCorrelativos_xlsx.Split(new string[] { "001_Correlativos.xlsx" }, StringSplitOptions.None)[0];
            textBox3.Text = _strCorrelativos_xlsx;

            if (File.Exists(_strCorrelativos_xlsx))
            {
                //Lee el xlsx
                FileInfo existingFile1 = new FileInfo(_strCorrelativos_xlsx);
                using (ExcelPackage package1 = new ExcelPackage(existingFile1))
                {
                    ExcelWorksheet worksheet1 = package1.Workbook.Worksheets[1];

                    int cCount1 = worksheet1.Dimension.End.Column;  //get Column Count
                    int rCount1 = worksheet1.Dimension.End.Row;  //get Row Count

                    for (int y = 2; y <= rCount1; y++)
                    {
                        if (Convert.ToString(worksheet1.Cells[y, 1].Value) == "")
                        {
                            break;
                        }

                        Correlativo objCorrelativo = new Correlativo();
                        objCorrelativo.ID_Correlativo = Convert.ToString(worksheet1.Cells[y, 1].Value);
                        objCorrelativo.Descripcion = Convert.ToString(worksheet1.Cells[y, 2].Value);
                        //objCorrelativo.CreadoEl = Convert.ToDateTime(worksheet1.Cells[y, 3].Value);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.CreadoEl = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 3].Value), "dd/MM/yyyy", null);
                        objCorrelativo.Bitacora = Convert.ToString(worksheet1.Cells[y, 4].Value);
                        //objCorrelativo.FechaLimite = Convert.ToDateTime(worksheet1.Cells[y, 5].Value);
                        objCorrelativo.FechaLimite = DateTime.ParseExact(Convert.ToString(worksheet1.Cells[y, 5].Value), "dd/MM/yyyy", null);
                        objCorrelativo.Estado = Convert.ToString(worksheet1.Cells[y, 6].Value);

                        LstCorrelativos.Add(objCorrelativo);
                        Lst_Categorias_ID.Add(objCorrelativo.ID_Correlativo.Substring(0, 5).Replace("_", ""));
                    }
                }

                LstCorrelativos.Reverse();
                Lst_Categorias_ID.Reverse();
                loaddata(LstCorrelativos);
            }
            else
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = "404 not found";
                row.Cells[1].Value = "404 not found";
                dataGridView1.Rows.Add(row);
            }


            // Obtiene todas las categorias de ID
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            Lst_Categorias_ID = Lst_Categorias_ID.Distinct().OrderBy(x => x).ToList();
            foreach (string var in Lst_Categorias_ID) { comboBox1.Items.Add(var); comboBox2.Items.Add(var); }





            comboBox2.Text = "KIM";
            textBox6.Text = "";
            comboBox1.Text = "";
            textBox4.Text = "";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string PC_name = System.Environment.MachineName;

            File.Delete(Directory.GetCurrentDirectory() + @"\001_Correlativos_path.txt");
            using (StreamWriter writetext = File.AppendText(Directory.GetCurrentDirectory() + @"\001_Correlativos_path.txt"))
            {
                writetext.Write(textBox3.Text);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string corr_a_buscar = comboBox1.Text;
            string[] desc_a_buscar = textBox4.Text.Split(' ');
            bool sin_desc_a_buscar = ((desc_a_buscar.Count() == 1) && (desc_a_buscar[0] == ""));

            List<Correlativo> Sub_LstCorrelativos = new List<Correlativo>();
            List<Correlativo> Sub_LstCorrelativos_2 = new List<Correlativo>();

            //Tipos de busqueda: 1) solo por ID, 2) solo por Descrp, 3) por ambos, 4) por ninguno (reestablece todo)
            if (corr_a_buscar != "" && sin_desc_a_buscar == true) // 1)
            {
                foreach (Correlativo item in LstCorrelativos)
                {
                    // Por Correlativo
                    if (item.ID_Correlativo.ToUpper().Contains(corr_a_buscar.ToUpper()))
                    {
                        Sub_LstCorrelativos.Add(item);
                    }
                }
            }
            else if (corr_a_buscar == "" && sin_desc_a_buscar == false) // 2)
            {
                foreach (Correlativo item in LstCorrelativos)
                {
                    // Por Descripcion
                    bool flag = true;
                    foreach (string item2 in desc_a_buscar)
                    {
                        if (!(item.Descripcion.ToUpper().Contains(item2.ToUpper())))
                        {
                            //Tiene que contener todas las palabras colocadas en el text box
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        Sub_LstCorrelativos.Add(item);
                    }
                }
            }
            else if (corr_a_buscar != "" && sin_desc_a_buscar == false) // 3)
            {
                foreach (Correlativo item in LstCorrelativos)
                {
                    bool flag = true;
                    // Por Correlativo
                    if (!(item.ID_Correlativo.ToUpper().Contains(corr_a_buscar.ToUpper())))
                    {
                        //Tiene que contener todas las palabras colocadas en el text box
                        flag = false;
                    }

                    // Por Descripcion
                    foreach (string item2 in desc_a_buscar)
                    {
                        if (!(item.Descripcion.ToUpper().Contains(item2.ToUpper())))
                        {
                            //Tiene que contener todas las palabras colocadas en el text box
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        Sub_LstCorrelativos.Add(item);
                    }
                }
            }
            else // 4)
            {
                foreach (Correlativo item in LstCorrelativos)
                {
                    Sub_LstCorrelativos.Add(item);
                }
            }


            //Filtro por status
            string string_evaluador_status = "";
            if (checkBox10.Checked == true)
            {
                string_evaluador_status = "No_Inic,En_Proc,En_Pausa,Terminado,Cancelado,Continuo";
            }
            else
            {
                if (checkBox9.Checked == true)
                {
                    string_evaluador_status = string_evaluador_status + ",No_Inic";
                }
                if (checkBox8.Checked == true)
                {
                    string_evaluador_status = string_evaluador_status + ",En_Proc";
                }
                if (checkBox7.Checked == true)
                {
                    string_evaluador_status = string_evaluador_status + ",En_Pausa";
                }
                if (checkBox6.Checked == true)
                {
                    string_evaluador_status = string_evaluador_status + ",Terminado";
                }
                if (checkBox11.Checked == true)
                {
                    string_evaluador_status = string_evaluador_status + ",Cancelado";
                }
                if (checkBox14.Checked == true)
                {
                    string_evaluador_status = string_evaluador_status + ",Continuo";
                }
                if (string_evaluador_status == "")
                {
                    string_evaluador_status = "No_Inic,En_Proc,En_Pausa,Terminado,Cancelado,Continuo";
                }
            }
            Sub_LstCorrelativos_2 = Sub_LstCorrelativos.Where(x => string_evaluador_status.Contains(x.Estado)).ToList();


            loaddata(Sub_LstCorrelativos_2);
        }

        public void loaddata(List<Correlativo> lista1)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();


            dataGridView1.AllowUserToAddRows = true;

            //do what you do in load data in order to update data in datagrid
            foreach (Correlativo item in lista1)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = item.ID_Correlativo;
                row.Cells[1].Value = item.CreadoEl;
                row.Cells[2].Value = item.FechaLimite;
                row.Cells[3].Value = item.Descripcion;
                row.Cells[4].Value = item.Estado;
                dataGridView1.Rows.Add(row);
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";

            dataGridView1.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            checkBox3.Checked = false;
            textBox1.Text = "";

            monthCalendar1.MaxSelectionCount = 1;

            //dataGridView1.CurrentCell = dataGridView1.Rows[1].Cells[1];
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            textBox4.Text = "";

            checkBox10.Checked = false;
            checkBox9.Checked = false;
            checkBox8.Checked = false;
            checkBox7.Checked = false;
            checkBox6.Checked = false;
            checkBox11.Checked = false;
            checkBox14.Checked = false;

            loaddata(LstCorrelativos);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            string _strCorrelativos_xlsx = textBox3.Text;

            if (File.Exists(_strCorrelativos_xlsx))
            {
                if (checkBox5.Checked == true)
                {
                    SoundPlayer player = new SoundPlayer();

                    //player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\yourmusic.wav";
                    player.SoundLocation = Directory.GetCurrentDirectory() + @"\Done_GTA.wav";

                    player.Play();
                }

                string new_Descrip = textBox6.Text;
                string new_Descrip_AREA = comboBox2.Text;
                while (new_Descrip_AREA.Length < 5) { new_Descrip_AREA = new_Descrip_AREA + "_"; }
                string new_CreadoEl = DateTime.Now.ToString("dd/MM/yyyy");
                string new_Bitacora_row = textBox1.Text;

                string today_at_00 = DateTime.Now.ToString("dd/MM/yyyy") + " 00:00:00";
                DateTime today_at_00_dt = DateTime.ParseExact(today_at_00, "dd/MM/yyyy HH:mm:ss", null);
                TimeSpan tsElapsed = DateTime.Now - today_at_00_dt;
                string seconds = Convert.ToString(Math.Round(tsElapsed.TotalSeconds, 0));
                string new_Correlativo_ID = new_Descrip_AREA + DateTime.Now.ToString("ddMMyyyy") + seconds;

                int rowindex = dataGridView1.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string folderBotName = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();

                String hoyDate = DateTime.Now.ToString("dd/MM/yyyy"); //Coloca la fecha de hoy
                String hoyTime = DateTime.Now.ToString("hh:mm:ss"); //Coloca la fecha de hoy

                string new_Estado = "";
                if (checkBox1.Checked == true)
                {
                    new_Estado = "No_Inic";
                }
                else if (checkBox2.Checked == true)
                {
                    new_Estado = "En_Proc";
                }
                else if (checkBox4.Checked == true)
                {
                    new_Estado = "En_Pausa";
                }
                else if (checkBox5.Checked == true)
                {
                    new_Estado = "Terminado";
                }
                else if (checkBox12.Checked == true)
                {
                    new_Estado = "Cancelado";
                }
                else if (checkBox13.Checked == true)
                {
                    new_Estado = "Continuo";
                }
                else
                {
                    new_Estado = "No_Inic";
                }

                //Verifica si es entrada nueva o actualizacion
                if (checkBox3.Checked == false)
                {
                    //Actualizacion
                    FileInfo existingFile1 = new FileInfo(_strCorrelativos_xlsx);
                    using (ExcelPackage package1 = new ExcelPackage(existingFile1))
                    {
                        ExcelWorksheet worksheet1 = package1.Workbook.Worksheets[1];

                        int cCount1 = worksheet1.Dimension.End.Column;  //get Column Count
                        int rCount1 = worksheet1.Dimension.End.Row;  //get Row Count

                        for (int y = 2; y <= rCount1 + 10; y++)
                        {
                            if (Convert.ToString(worksheet1.Cells[y, 1].Value) == folderBotName)
                            {
                                worksheet1.Cells[y, 2].Value = new_Descrip;
                                worksheet1.Cells[y, 5].Value = monthCalendar1.SelectionRange.Start.ToString("dd/MM/yyyy");
                                worksheet1.Cells[y, 6].Value = new_Estado;

                                if (new_Bitacora_row != "")
                                {
                                    string old_bitacora = Convert.ToString(worksheet1.Cells[y, 4].Value);
                                    if (old_bitacora == "")
                                    {
                                        worksheet1.Cells[y, 4].Value = hoyTime + " - " + hoyDate + " - [" + new_Bitacora_row + "]";

                                    }
                                    else
                                    {
                                        worksheet1.Cells[y, 4].Value = Convert.ToString(worksheet1.Cells[y, 4].Value) + "\n" + hoyTime + " - " + hoyDate + " - [" + new_Bitacora_row + "]";
                                    }
                                }


                                break;
                            }
                        }
                        package1.Save();
                    }
                    LstCorrelativos = new List<Correlativo>();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    Form8_Load_2();
                }
                else
                {
                    //Entrada nueva
                    FileInfo existingFile1 = new FileInfo(_strCorrelativos_xlsx);
                    using (ExcelPackage package1 = new ExcelPackage(existingFile1))
                    {
                        ExcelWorksheet worksheet1 = package1.Workbook.Worksheets[1];

                        int cCount1 = worksheet1.Dimension.End.Column;  //get Column Count
                        int rCount1 = worksheet1.Dimension.End.Row;  //get Row Count

                        for (int y = 2; y <= rCount1 + 10; y++)
                        {
                            if (Convert.ToString(worksheet1.Cells[y, 1].Value) == "")
                            {
                                worksheet1.Cells[y, 1].Value = new_Correlativo_ID;
                                worksheet1.Cells[y, 2].Value = new_Descrip;
                                worksheet1.Cells[y, 3].Value = new_CreadoEl;
                                if (new_Bitacora_row != "")
                                {
                                    worksheet1.Cells[y, 4].Value = new_Bitacora_row;
                                }
                                worksheet1.Cells[y, 5].Value = monthCalendar1.SelectionRange.Start.ToString("dd/MM/yyyy");
                                worksheet1.Cells[y, 6].Value = new_Estado;

                                break;
                            }
                        }
                        package1.Save();
                    }
                    LstCorrelativos = new List<Correlativo>();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    Form8_Load_2();
                }
            }
            else
            {
                MessageBox.Show("Archivo xlsx no existente!", "J4QH3S_MANAGER");
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string _strFilePath_ABS = Directory.GetCurrentDirectory();
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            string PC_name = System.Environment.MachineName;

            string _strCorrelativos_xlsx = File.ReadAllText(Directory.GetCurrentDirectory() + @"\001_Correlativos_path.txt", Encoding.UTF8);
            string _strPendientes_folder = _strCorrelativos_xlsx.Split(new string[] { "001_Correlativos.xlsx" }, StringSplitOptions.None)[0];

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();


            try
            {
                Process.Start(_strPendientes_folder + @"\" + folderBotName);
            }
            catch
            {
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(_strPendientes_folder + @"\" + folderBotName);
                    MessageBox.Show(_strPendientes_folder + @"\" + folderBotName + " has just been CREATED!");
                    Process.Start(_strPendientes_folder + @"\" + folderBotName);
                }
                catch
                {
                    MessageBox.Show(_strPendientes_folder + @"\" + folderBotName + " could not have been CREATED!");
                }
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = false;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox12.Checked = false;

            try
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                string folderBotName = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();

                //MessageBox.Show(folderBotName);

                Correlativo match = LstCorrelativos.FirstOrDefault(x => x.ID_Correlativo == folderBotName);
                if (match != null)
                {
                    // Llena info de text boxes
                    textBox6.Text = match.Descripcion;
                    textBox7.Text = match.Bitacora.Replace("\n", "\r\n");
                    comboBox2.Text = match.ID_Correlativo.Substring(0, 3);

                    // Llena info de check boxes
                    switch (match.Estado)
                    {
                        case "No_Inic":
                            checkBox1.Checked = true;
                            break;
                        case "En_Proc":
                            checkBox2.Checked = true;
                            break;
                        case "En_Pausa":
                            checkBox4.Checked = true;
                            break;
                        case "Terminado":
                            checkBox5.Checked = true;
                            break;
                        case "Cancelado":
                            checkBox12.Checked = true;
                            break;
                    }

                    // Llena info en month calendar
                    monthCalendar1.SetDate(match.FechaLimite);
                }
            }
            catch { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                textBox1.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                comboBox2.Text = "KIM";

                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox12.Checked = false;

                DateTime example = Convert.ToDateTime("01/01/0001");
                monthCalendar1.SetDate(example);
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked == true)
            {
                checkBox9.Checked = true;
                checkBox8.Checked = true;
                checkBox7.Checked = true;
                checkBox6.Checked = true;
                checkBox11.Checked = true;
                checkBox14.Checked = true;
            }
            else
            {
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                checkBox7.Checked = false;
                checkBox6.Checked = false;
                checkBox11.Checked = false;
                checkBox14.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox5.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                checkBox1.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox1.Checked = false;
                checkBox13.Checked = false;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox1.Checked = false;
            checkBox12.Checked = false;
        }

        private void Form8_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string root_path = "";
            string[] arr_root_path = Directory.GetCurrentDirectory().Split('\\');
            for (int i = 0; i <= arr_root_path.Count() - 4; i++)
            {
                if (i == 0) { root_path = arr_root_path[i]; }
                else { root_path = root_path + "\\" + arr_root_path[i]; }
            }
            root_path = root_path + "\\";
            Process.Start(root_path);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + @"\0_shutdown.cmd");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + @"\0_reboot.cmd");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory() + @"\0_stop.cmd");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Process.Start("D:\\D_Documents\\AKim\\02_KimIndustries\\GIT_Projects\\all_PULL.lnk");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
           Process.Start("D:\\D_Documents\\AKim\\02_KimIndustries\\GIT_Projects\\all_PUSH.lnk");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Process.Start(textBox3.Text);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string _strCorrelativos_xlsx = textBox3.Text;
            string _strPendientes_folder = _strCorrelativos_xlsx.Split(new string[] { "001_Correlativos.xlsx" }, StringSplitOptions.None)[0];

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            //int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folder_correlativo_name = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();

            //Borra entrada
            FileInfo existingFile1 = new FileInfo(_strCorrelativos_xlsx);
            using (ExcelPackage package1 = new ExcelPackage(existingFile1))
            {
                ExcelWorksheet worksheet1 = package1.Workbook.Worksheets[1];

                int cCount1 = worksheet1.Dimension.End.Column;  //get Column Count
                int rCount1 = worksheet1.Dimension.End.Row;  //get Row Count

                for (int y = 2; y <= rCount1 + 10; y++)
                {
                    if (Convert.ToString(worksheet1.Cells[y, 1].Value) != "")
                    {
                        if (Convert.ToString(worksheet1.Cells[y, 1].Value) == folder_correlativo_name)
                        {
                            worksheet1.DeleteRow(y);
                            string folder_a_borrar = _strPendientes_folder + @"\" + folder_correlativo_name;
                            if (Directory.Exists(folder_a_borrar))
                            {
                                DialogResult dialogResult = MessageBox.Show("Sure about DELETION of " + folder_a_borrar + " ? (IRREVERSIBLE ACTION)", "JAQH3S_MANAGER", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    Directory.Delete(folder_a_borrar, true);
                                    MessageBox.Show(_strPendientes_folder + @"\" + folder_a_borrar + " has just been ERASED!");
                                }
                            }
                            break;
                        }
                    }
                }
                package1.Save();
            }

            LstCorrelativos = new List<Correlativo>();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            dataGridView1.ClearSelection();

            Form8_Load_2();
        }
    }
}
