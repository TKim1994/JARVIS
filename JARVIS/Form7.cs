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

namespace JARVISNamespace
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            string _strFilePath = File.ReadAllText(Directory.GetCurrentDirectory() + @"\WatcherPath.txt", Encoding.UTF8);

            string line;
            int index = 0;

            System.IO.StreamReader file = new System.IO.StreamReader(_strFilePath + @"\Eyes_Everywhere.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line != "")
                {
                    if (index % 3 == 0)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = line;
                        dataGridView1.Rows.Add(row);
                    }
                    index++;
                }
            }
            file.Close();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string _strFilePath = File.ReadAllText(Directory.GetCurrentDirectory() + @"\WatcherPath.txt", Encoding.UTF8);

            string line;
            DateTime obj;
            Proceso objProceso;
            Sesion objSesion;


            List<Execution> LstExecution = new List<Execution>();

            List<string> LstEyesEverywhere = new List<string>();

            if (!File.Exists(_strFilePath + @"\Eyes_Everywhere.txt"))
            {
                MessageBox.Show("Eyes_Everywhere.txt does not exist!");
                return;
            }

            System.IO.StreamReader file = new System.IO.StreamReader(_strFilePath + @"\Eyes_Everywhere.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line != "")
                {
                    LstEyesEverywhere.Add(line);
                }

            }
            file.Close();

            //Va trabajar la lista por paquetes de 3, donde el primero corresponed a hora y fecha, el segundo a la lsita de procesos y el tercero a sesiones
            List<List<string>> list = new List<List<string>>();
            for (int i = 0; i < LstEyesEverywhere.Count; i += 3)
                list.Add(LstEyesEverywhere.GetRange(i, Math.Min(3, LstEyesEverywhere.Count - i)));


            Execution objExecution = new Execution();
            foreach (var item in list)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                int columnindex = dataGridView1.CurrentCell.ColumnIndex;

                string dateTimeString = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();

                if (item[0] == dateTimeString)
                {
                    //Crea el nuevo objeto de ejecucion
                    List<Proceso> LstProcesos = new List<Proceso>();
                    List<Sesion> LstSesions = new List<Sesion>();

                    //1: Fecha y hora       <-----------------------------------------------------------------
                    obj = DateTime.ParseExact(item[0], "HH:mm:ss - dd/MM/yyyy", null);
                    objExecution.Date_Time = obj;

                    //2: Lista de procesos  <-----------------------------------------------------------------
                    objProceso = new Proceso();
                    string[] tempArr1 = item[1].Split(';');
                    int index = 0;
                    for (int m = 0; m < tempArr1.Count() / 9; m++)
                    {
                        PropertyInfo[] properties = typeof(Proceso).GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            try
                            {
                                property.SetValue(objProceso, tempArr1[index]);
                            }
                            catch
                            {
                                property.SetValue(objProceso, Convert.ToInt32(tempArr1[index]));
                            }

                            index++;
                        }

                        if (index % 9 == 0)
                        {
                            if (index != 0)
                            {
                                //Guarda el objeto anterior
                                LstProcesos.Add(objProceso);
                                objProceso = new Proceso();
                            }
                        }
                    }
                    objExecution.LstProcesses = LstProcesos;

                    //3: Lista de procesos  <-----------------------------------------------------------------
                    objSesion = new Sesion();
                    string[] tempArr2 = item[2].Split(';');
                    index = 0;
                    for (int m = 0; m < tempArr2.Count() / 7; m++)
                    {
                        PropertyInfo[] properties = typeof(Sesion).GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            try
                            {
                                property.SetValue(objSesion, tempArr2[index]);
                            }
                            catch
                            {
                                property.SetValue(objSesion, Convert.ToInt32(tempArr2[index]));
                            }

                            index++;
                        }

                        if (index % 7 == 0)
                        {
                            if (index != 0)
                            {
                                //Guarda el objeto anterior
                                LstSesions.Add(objSesion);
                                objSesion = new Sesion();
                            }
                        }
                    }
                    objExecution.LstSessions = LstSesions;

                    //4: Ya se lleno el objeto, se sale <-----------------------------------------------------------------
                    break;
                }
            }

            string[] UniqueEntries = ListEncoder(objExecution.LstProcesses, objExecution.LstSessions);

            //Graba el XLSX
            string _strFilePath1 = Directory.GetCurrentDirectory() + @"\Excel_Template.xlsx";
            FileInfo existingFile1 = new FileInfo(_strFilePath1);
            using (ExcelPackage package1 = new ExcelPackage(existingFile1))
            {
                ExcelWorksheet worksheet1 = package1.Workbook.Worksheets[1];
                worksheet1.Name = "Processes";
                ExcelWorksheet worksheet2 = package1.Workbook.Worksheets[2];
                worksheet2.Name = "Sessions";

                worksheet1.Cells[1, 1].Value = "Process Name";
                worksheet1.Cells[1, 2].Value = "Exe Path";
                worksheet1.Cells[1, 3].Value = "Main Window Title";
                worksheet1.Cells[1, 4].Value = "PID";
                worksheet1.Cells[1, 5].Value = "Session ID";
                worksheet1.Cells[1, 6].Value = "Session Name";
                worksheet1.Cells[1, 7].Value = "Responding";
                worksheet1.Cells[1, 8].Value = "Memory Usage";
                worksheet1.Cells[1, 9].Value = "Start Time";
                int index1 = 2;

                var LstProcesos2 = objExecution.LstProcesses.OrderBy(x => x.ProcessName).ToList();
                foreach (var item in LstProcesos2)
                {
                    worksheet1.Cells[index1, 1].Value = item.ProcessName;
                    worksheet1.Cells[index1, 2].Value = item.Path;
                    worksheet1.Cells[index1, 3].Value = item.MainWindowTitle;
                    worksheet1.Cells[index1, 4].Value = item.PID;
                    worksheet1.Cells[index1, 5].Value = item.SessionId;
                    worksheet1.Cells[index1, 6].Value = item.SessionName;
                    worksheet1.Cells[index1, 7].Value = item.Responding;
                    worksheet1.Cells[index1, 8].Value = item.MemoryUsage;
                    worksheet1.Cells[index1, 9].Value = item.StartTime;
                    index1++;
                }


                worksheet2.Cells[1, 1].Value = "Session Name";
                worksheet2.Cells[1, 2].Value = "User Name";
                worksheet2.Cells[1, 3].Value = "ID";
                worksheet2.Cells[1, 4].Value = "Status";
                worksheet2.Cells[1, 5].Value = "Connect Time";
                worksheet2.Cells[1, 6].Value = "Disconnect Time";
                worksheet2.Cells[1, 7].Value = "Login Time";
                index1 = 2;
                foreach (var item in objExecution.LstSessions)
                {
                    worksheet2.Cells[index1, 1].Value = item.SessionName;
                    worksheet2.Cells[index1, 2].Value = item.UserName;
                    worksheet2.Cells[index1, 3].Value = item.ID;
                    worksheet2.Cells[index1, 4].Value = item.Status;
                    worksheet2.Cells[index1, 5].Value = item.ConnectTime;
                    worksheet2.Cells[index1, 6].Value = item.DisconnectTime;
                    worksheet2.Cells[index1, 7].Value = item.LoginTime;
                    index1++;
                }

                Color amarilloCustom = System.Drawing.ColorTranslator.FromHtml("#FFFF99");

                int colCount1 = worksheet1.Dimension.End.Column;  //get Column Count
                int rowCount1 = worksheet1.Dimension.End.Row;  //get Row Count

                worksheet1.Cells[1, 1, 1, colCount1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet1.Cells[1, 1, 1, colCount1].Style.Fill.BackgroundColor.SetColor(amarilloCustom);

                worksheet1.Cells[1, 8, rowCount1, 8].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";

                worksheet1.Cells[worksheet1.Dimension.Address].AutoFilter = true;
                worksheet1.Cells[worksheet1.Dimension.Address].AutoFitColumns();
                worksheet1.Column(1).Width = 25;
                worksheet1.Column(2).Width = 40;
                worksheet1.Column(3).Width = 40;
                worksheet1.View.FreezePanes(2, 1);



                int colCount2 = worksheet2.Dimension.End.Column;  //get Column Count
                int rowCount2 = worksheet2.Dimension.End.Row;  //get Row Count

                worksheet2.Cells[1, 1, 1, colCount2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet2.Cells[1, 1, 1, colCount2].Style.Fill.BackgroundColor.SetColor(amarilloCustom);

                worksheet2.Cells[1, 5, rowCount2, 5].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
                worksheet2.Cells[1, 6, rowCount2, 6].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
                worksheet2.Cells[1, 7, rowCount2, 7].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";

                worksheet2.Cells[worksheet1.Dimension.Address].AutoFilter = true;
                worksheet2.Cells[worksheet2.Dimension.Address].AutoFitColumns();
                worksheet2.View.FreezePanes(2, 1);

                string path = Directory.GetCurrentDirectory() + @"\Eyes_Everywhere.xlsx";
                Stream stream;
                try
                {
                    stream = File.Create(path);
                }
                catch
                {
                    MessageBox.Show("Please, close the current xlsx view of Watcher");
                    return;
                }
               
                package1.SaveAs(stream);
                stream.Close();
            }

            //Abre el excel recien creado
            Process.Start(Directory.GetCurrentDirectory() + @"\Eyes_Everywhere.xlsx");
        }



        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public string[] ListEncoder(List<Proceso> LstProcesos, List<Sesion> LstSesiones)
        {
            string[] UniqueEntries = new string[2];
            string UniqueEntry1 = "";
            string UniqueEntry2 = "";
            foreach (var item in LstProcesos)
            {
                PropertyInfo[] properties = typeof(Proceso).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (UniqueEntry1 == "")
                    {
                        UniqueEntry1 = Convert.ToString(property.GetValue(item, null));
                    }
                    else
                    {
                        UniqueEntry1 = UniqueEntry1 + ";" + Convert.ToString(property.GetValue(item, null));
                    }
                }
            }
            foreach (var item in LstSesiones)
            {
                PropertyInfo[] properties = typeof(Sesion).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (UniqueEntry2 == "")
                    {
                        UniqueEntry2 = Convert.ToString(property.GetValue(item, null));
                    }
                    else
                    {
                        UniqueEntry2 = UniqueEntry2 + ";" + Convert.ToString(property.GetValue(item, null));
                    }
                }
            }

            UniqueEntries[0] = UniqueEntry1;
            UniqueEntries[1] = UniqueEntry2;

            return UniqueEntries;
        }
    }
}
