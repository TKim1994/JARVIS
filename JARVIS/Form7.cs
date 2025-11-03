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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            List<Reporte_PBI> Lst_Reporte_PBI = new List<Reporte_PBI>();
            string _str_xlsxpath = Directory.GetCurrentDirectory() + @"\pbix_listado.xlsx";

            if (File.Exists(_str_xlsxpath))
            {
                //Lee el xlsx
                FileInfo existingFile1 = new FileInfo(_str_xlsxpath);
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

                        Reporte_PBI obj_Reporte_PBI = new Reporte_PBI();
                        obj_Reporte_PBI.ID_Correlativo = Convert.ToString(worksheet1.Cells[y, 1].Value);
                        obj_Reporte_PBI.Ruta_pbix = Convert.ToString(worksheet1.Cells[y, 2].Value);
                        obj_Reporte_PBI.Ruta_data = Convert.ToString(worksheet1.Cells[y, 3].Value);
                        obj_Reporte_PBI.Descripcion = Convert.ToString(worksheet1.Cells[y, 4].Value);

                        Lst_Reporte_PBI.Add(obj_Reporte_PBI);
                    }
                }

                //Lst_Reporte_PBI.Reverse();
                loaddata(Lst_Reporte_PBI);
            }
            else
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = "404 not found";
                row.Cells[1].Value = "404 not found";
                dataGridView1.Rows.Add(row);
            }


        }

        public void loaddata(List<Reporte_PBI> lista1)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();


            dataGridView1.AllowUserToAddRows = true;

            //do what you do in load data in order to update data in datagrid
            foreach (Reporte_PBI item in lista1)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = item.ID_Correlativo;
                row.Cells[1].Value = item.Ruta_pbix;
                row.Cells[2].Value = item.Ruta_data;
                row.Cells[3].Value = item.Descripcion;
                dataGridView1.Rows.Add(row);
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string _strFilePath_ABS = Directory.GetCurrentDirectory();
            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 24);

            string PC_name = System.Environment.MachineName;

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string folderBotName = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();

            if (PC_name=="FRIDAY8") { folderBotName = folderBotName.Replace("Mi unidad", "My Drive"); }

            if (columnindex!=3)
            { 
                Process.Start(folderBotName);
            }
            else
            {
                string folderBotName_1 = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();
                string folderBotName_2 = dataGridView1.Rows[rowindex].Cells[2].Value.ToString();

                if (PC_name == "FRIDAY8") { folderBotName = folderBotName.Replace("Mi unidad", "My Drive"); }

                Process.Start(folderBotName_1);
                Process.Start(folderBotName_2);
            }
        }
    }
}
