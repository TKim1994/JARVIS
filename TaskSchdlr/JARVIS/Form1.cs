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


namespace JARVISNamespace
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public static string SetValueForText1 = "";

        string _strFilePath_ABS = Directory.GetCurrentDirectory();

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            Estrategia para cambiar los detalles del Task:
                1) Se crea un nuevo trigger
                2) Se modifica el segundo trigger (el recien creado)
                3) Se elimina el primer trigger (el antiguo)
            */

            string _strFilePath = _strFilePath_ABS.Substring(0, _strFilePath_ABS.Length - 17);

            string _strFilePathLATEST = Directory.GetCurrentDirectory();

            string modo = File.ReadAllText(_strFilePathLATEST + @"\mode.txt", Encoding.UTF8);

            if (modo == "upd")
            {
                string parameters = File.ReadAllText(_strFilePathLATEST + @"\parameters.txt", Encoding.UTF8);
                string[] parameters2 = parameters.Split(',');

                using (TaskService ts = new TaskService())
                {
                    try //Busca la Task por el nombre
                    {
                        Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(parameters2[0]);
                        TaskDefinition td = task.Definition;

                        if (parameters2[2] == "Daily")
                        {
                            DailyTrigger dailyTriger = new DailyTrigger();

                            //Edita el "Repetir cada"
                            string date = parameters2[3];
                            dailyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[4] == "days")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[5]));
                            }
                            else if (parameters2[4] == "hours")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[5]));
                            }
                            else if (parameters2[4] == "mins")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[5]));
                            }

                            //Edita el "durante"
                            if (parameters2[6] == "days")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "hours")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "mins")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[7]));
                            }

                            td.Triggers.Add(dailyTriger);
                            td.Triggers.RemoveAt(Convert.ToInt32(parameters2[1]));
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }
                        if (parameters2[2] == "Weekly")
                        {
                            WeeklyTrigger weeklyTriger = new WeeklyTrigger();

                            if (parameters2[3].Contains("1"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Monday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Monday;
                                }

                            }
                            if (parameters2[3].Contains("2"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Tuesday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Tuesday;
                                }
                            }
                            if (parameters2[3].Contains("3"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Wednesday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Wednesday;
                                }
                            }
                            if (parameters2[3].Contains("4"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Thursday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Thursday;
                                }
                            }
                            if (parameters2[3].Contains("5"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Friday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Friday;
                                }
                            }
                            if (parameters2[3].Contains("6"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Saturday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Saturday;
                                }
                            }
                            if (parameters2[3].Contains("7"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Sunday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Sunday;
                                }
                            }



                            //Edita el "Repetir cada"
                            string date = parameters2[4];
                            weeklyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[5] == "days")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[6]));
                            }
                            else if (parameters2[5] == "hours")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[6]));
                            }
                            else if (parameters2[5] == "mins")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[6]));
                            }

                            //Edita el "durante"
                            if (parameters2[7] == "days")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[8]));
                            }
                            else if (parameters2[7] == "hours")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[8]));
                            }
                            else if (parameters2[7] == "mins")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[8]));
                            }

                            td.Triggers.Add(weeklyTriger);
                            td.Triggers.RemoveAt(Convert.ToInt32(parameters2[1]));
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }
                        if (parameters2[2] == "Monthly")
                        {
                            MonthlyTrigger monthlyTriger = new MonthlyTrigger();

                            if (parameters2[3].Contains("01."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.January;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.January;
                                }
                            }
                            if (parameters2[3].Contains("02."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.February;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.February;
                                }
                            }
                            if (parameters2[3].Contains("03."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.March;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.March;
                                }
                            }
                            if (parameters2[3].Contains("04."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.April;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.April;
                                }
                            }
                            if (parameters2[3].Contains("05."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.May;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.May;
                                }
                            }
                            if (parameters2[3].Contains("06."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.June;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.June;
                                }
                            }
                            if (parameters2[3].Contains("07."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.July;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.July;
                                }
                            }
                            if (parameters2[3].Contains("08."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.August;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.August;
                                }
                            }
                            if (parameters2[3].Contains("09."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.September;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.September;
                                }
                            }
                            if (parameters2[3].Contains("10."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.October;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.October;
                                }
                            }
                            if (parameters2[3].Contains("11."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.November;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.November;
                                }
                            }
                            if (parameters2[3].Contains("12."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.December;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.December;
                                }
                            }

                            //Agrupa la cadena parameters2[4] en grupos de 2 y las guarda como elementos de lstDaysOfMonth
                            List<string> lstDaysOfMonth = new List<string>();
                            for (int u = 0; u < parameters2[4].Count(); u = u + 2)
                            {
                                string grupo = parameters2[4].Substring(u, 2);
                                lstDaysOfMonth.Add(grupo);
                            }

                            List<int> daysOfMonth = new List<int>();
                            foreach (var item in lstDaysOfMonth)
                            {
                                daysOfMonth.Add(Convert.ToInt32(item));
                            }

                            int[] daysOfMonthArray = daysOfMonth.ToArray();

                            monthlyTriger.DaysOfMonth = daysOfMonthArray;

                            //Edita el "Repetir cada"
                            string date = parameters2[5];
                            monthlyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[6] == "days")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "hours")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "mins")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[7]));
                            }

                            //Edita el "durante"
                            if (parameters2[8] == "days")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[9]));
                            }
                            else if (parameters2[8] == "hours")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[9]));
                            }
                            else if (parameters2[8] == "mins")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[9]));
                            }

                            td.Triggers.Add(monthlyTriger);
                            td.Triggers.RemoveAt(Convert.ToInt32(parameters2[1]));
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }

                        MessageBox.Show("Task " + parameters2[0] + ": Updated!", "JARVIS TASKER");
                    }
                    catch //No encontro la Task por el nombre, entonces la crea
                    {
                        MessageBox.Show("Task " + parameters2[0] + ": Could not be updated!", "JARVIS TASKER");
                    }

                    this.Close();
                }
            }
            else if (modo == "cre")
            {
                string parameters = File.ReadAllText(_strFilePathLATEST + @"\parameters.txt", Encoding.UTF8);
                string[] parameters2 = parameters.Split(',');

                using (TaskService ts = new TaskService())
                {
                    try // Crea un nuevo trigger para una tarea existente
                    {
                        Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(parameters2[0]);
                        TaskDefinition td = task.Definition;

                        if (parameters2[2] == "Daily")
                        {
                            DailyTrigger dailyTriger = new DailyTrigger();

                            //Edita el "Repetir cada"
                            string date = parameters2[3];
                            dailyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[4] == "days")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[5]));
                            }
                            else if (parameters2[4] == "hours")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[5]));
                            }
                            else if (parameters2[4] == "mins")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[5]));
                            }

                            //Edita el "durante"
                            if (parameters2[6] == "days")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "hours")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "mins")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[7]));
                            }

                            td.Triggers.Add(dailyTriger);
                            td.RegistrationInfo.Author = "Created_Automatically_by_J4QH3S_Manager";
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }
                        if (parameters2[2] == "Weekly")
                        {
                            WeeklyTrigger weeklyTriger = new WeeklyTrigger();

                            if (parameters2[3].Contains("1"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Monday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Monday;
                                }

                            }
                            if (parameters2[3].Contains("2"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Tuesday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Tuesday;
                                }
                            }
                            if (parameters2[3].Contains("3"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Wednesday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Wednesday;
                                }
                            }
                            if (parameters2[3].Contains("4"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Thursday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Thursday;
                                }
                            }
                            if (parameters2[3].Contains("5"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Friday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Friday;
                                }
                            }
                            if (parameters2[3].Contains("6"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Saturday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Saturday;
                                }
                            }
                            if (parameters2[3].Contains("7"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Sunday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Sunday;
                                }
                            }



                            //Edita el "Repetir cada"
                            string date = parameters2[4];
                            weeklyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[5] == "days")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[6]));
                            }
                            else if (parameters2[5] == "hours")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[6]));
                            }
                            else if (parameters2[5] == "mins")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[6]));
                            }

                            //Edita el "durante"
                            if (parameters2[7] == "days")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[8]));
                            }
                            else if (parameters2[7] == "hours")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[8]));
                            }
                            else if (parameters2[7] == "mins")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[8]));
                            }

                            td.Triggers.Add(weeklyTriger);
                            td.RegistrationInfo.Author = "Created_Automatically_by_J4QH3S_Manager";
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }
                        if (parameters2[2] == "Monthly")
                        {
                            MonthlyTrigger monthlyTriger = new MonthlyTrigger();

                            if (parameters2[3].Contains("01."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.January;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.January;
                                }
                            }
                            if (parameters2[3].Contains("02."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.February;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.February;
                                }
                            }
                            if (parameters2[3].Contains("03."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.March;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.March;
                                }
                            }
                            if (parameters2[3].Contains("04."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.April;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.April;
                                }
                            }
                            if (parameters2[3].Contains("05."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.May;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.May;
                                }
                            }
                            if (parameters2[3].Contains("06."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.June;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.June;
                                }
                            }
                            if (parameters2[3].Contains("07."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.July;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.July;
                                }
                            }
                            if (parameters2[3].Contains("08."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.August;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.August;
                                }
                            }
                            if (parameters2[3].Contains("09."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.September;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.September;
                                }
                            }
                            if (parameters2[3].Contains("10."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.October;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.October;
                                }
                            }
                            if (parameters2[3].Contains("11."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.November;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.November;
                                }
                            }
                            if (parameters2[3].Contains("12."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.December;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.December;
                                }
                            }

                            //Agrupa la cadena parameters2[4] en grupos de 2 y las guarda como elementos de lstDaysOfMonth
                            List<string> lstDaysOfMonth = new List<string>();
                            for (int u = 0; u < parameters2[4].Count(); u = u + 2)
                            {
                                string grupo = parameters2[4].Substring(u, 2);
                                lstDaysOfMonth.Add(grupo);
                            }

                            List<int> daysOfMonth = new List<int>();
                            foreach (var item in lstDaysOfMonth)
                            {
                                daysOfMonth.Add(Convert.ToInt32(item));
                            }

                            int[] daysOfMonthArray = daysOfMonth.ToArray();

                            monthlyTriger.DaysOfMonth = daysOfMonthArray;

                            //Edita el "Repetir cada"
                            string date = parameters2[5];
                            monthlyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[6] == "days")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "hours")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "mins")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[7]));
                            }

                            //Edita el "durante"
                            if (parameters2[8] == "days")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[9]));
                            }
                            else if (parameters2[8] == "hours")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[9]));
                            }
                            else if (parameters2[8] == "mins")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[9]));
                            }

                            td.Triggers.Add(monthlyTriger);
                            td.RegistrationInfo.Author = "Created_Automatically_by_J4QH3S_Manager";
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }

                        MessageBox.Show("Task " + parameters2[0] + ": New trigger created!", "JARVIS TASKER");
                    }
                    catch //Crea toda una tarea nueva
                    {

                        TaskDefinition td = ts.NewTask();

                        if (parameters2[2] == "Daily")
                        {
                            DailyTrigger dailyTriger = new DailyTrigger();

                            //Edita el "Repetir cada"
                            string date = parameters2[3];
                            dailyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[4] == "days")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[5]));
                            }
                            else if (parameters2[4] == "hours")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[5]));
                            }
                            else if (parameters2[4] == "mins")
                            {
                                dailyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[5]));
                            }

                            //Edita el "durante"
                            if (parameters2[6] == "days")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "hours")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "mins")
                            {
                                dailyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[7]));
                            }

                            td.Triggers.Add(dailyTriger);
                            td.RegistrationInfo.Author = "Created_Automatically_by_J4QH3S_Manager";
                            td.Actions.Add(new ExecAction(parameters2[8], null, null));
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }
                        if (parameters2[2] == "Weekly")
                        {
                            WeeklyTrigger weeklyTriger = new WeeklyTrigger();

                            if (parameters2[3].Contains("1"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Monday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Monday;
                                }

                            }
                            if (parameters2[3].Contains("2"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Tuesday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Tuesday;
                                }
                            }
                            if (parameters2[3].Contains("3"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Wednesday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Wednesday;
                                }
                            }
                            if (parameters2[3].Contains("4"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Thursday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Thursday;
                                }
                            }
                            if (parameters2[3].Contains("5"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Friday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Friday;
                                }
                            }
                            if (parameters2[3].Contains("6"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Saturday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Saturday;
                                }
                            }
                            if (parameters2[3].Contains("7"))
                            {
                                var dayss = weeklyTriger.DaysOfWeek;
                                if (dayss.ToString() == "Sunday")
                                {
                                    weeklyTriger.DaysOfWeek = DaysOfTheWeek.Sunday;
                                }
                                else
                                {
                                    weeklyTriger.DaysOfWeek = dayss | DaysOfTheWeek.Sunday;
                                }
                            }



                            //Edita el "Repetir cada"
                            string date = parameters2[4];
                            weeklyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[5] == "days")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[6]));
                            }
                            else if (parameters2[5] == "hours")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[6]));
                            }
                            else if (parameters2[5] == "mins")
                            {
                                weeklyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[6]));
                            }

                            //Edita el "durante"
                            if (parameters2[7] == "days")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[8]));
                            }
                            else if (parameters2[7] == "hours")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[8]));
                            }
                            else if (parameters2[7] == "mins")
                            {
                                weeklyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[8]));
                            }

                            td.Triggers.Add(weeklyTriger);
                            td.RegistrationInfo.Author = "Created_Automatically_by_J4QH3S_Manager";
                            td.Actions.Add(new ExecAction(parameters2[9], null, null));

                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }
                        if (parameters2[2] == "Monthly")
                        {
                            MonthlyTrigger monthlyTriger = new MonthlyTrigger();

                            if (parameters2[3].Contains("01."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.January;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.January;
                                }
                            }
                            if (parameters2[3].Contains("02."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.February;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.February;
                                }
                            }
                            if (parameters2[3].Contains("03."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.March;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.March;
                                }
                            }
                            if (parameters2[3].Contains("04."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.April;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.April;
                                }
                            }
                            if (parameters2[3].Contains("05."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.May;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.May;
                                }
                            }
                            if (parameters2[3].Contains("06."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.June;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.June;
                                }
                            }
                            if (parameters2[3].Contains("07."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.July;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.July;
                                }
                            }
                            if (parameters2[3].Contains("08."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.August;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.August;
                                }
                            }
                            if (parameters2[3].Contains("09."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.September;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.September;
                                }
                            }
                            if (parameters2[3].Contains("10."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.October;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.October;
                                }
                            }
                            if (parameters2[3].Contains("11."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.November;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.November;
                                }
                            }
                            if (parameters2[3].Contains("12."))
                            {
                                var dayss = monthlyTriger.MonthsOfYear;
                                if (dayss.ToString() == "AllMonths")
                                {
                                    monthlyTriger.MonthsOfYear = MonthsOfTheYear.December;
                                }
                                else
                                {
                                    monthlyTriger.MonthsOfYear = dayss | MonthsOfTheYear.December;
                                }
                            }

                            //Agrupa la cadena parameters2[4] en grupos de 2 y las guarda como elementos de lstDaysOfMonth
                            List<string> lstDaysOfMonth = new List<string>();
                            for (int u = 0; u < parameters2[4].Count(); u = u + 2)
                            {
                                string grupo = parameters2[4].Substring(u, 2);
                                lstDaysOfMonth.Add(grupo);
                            }

                            List<int> daysOfMonth = new List<int>();
                            foreach (var item in lstDaysOfMonth)
                            {
                                daysOfMonth.Add(Convert.ToInt32(item));
                            }

                            int[] daysOfMonthArray = daysOfMonth.ToArray();

                            monthlyTriger.DaysOfMonth = daysOfMonthArray;

                            //Edita el "Repetir cada"
                            string date = parameters2[5];
                            monthlyTriger.StartBoundary = Convert.ToDateTime(date);

                            if (parameters2[6] == "days")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromDays(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "hours")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromHours(Convert.ToInt32(parameters2[7]));
                            }
                            else if (parameters2[6] == "mins")
                            {
                                monthlyTriger.Repetition.Interval = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[7]));
                            }

                            //Edita el "durante"
                            if (parameters2[8] == "days")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromDays(Convert.ToInt32(parameters2[9]));
                            }
                            else if (parameters2[8] == "hours")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromHours(Convert.ToInt32(parameters2[9]));
                            }
                            else if (parameters2[8] == "mins")
                            {
                                monthlyTriger.Repetition.Duration = TimeSpan.FromMinutes(Convert.ToInt32(parameters2[9]));
                            }

                            td.Triggers.Add(monthlyTriger);
                            td.RegistrationInfo.Author = "Created_Automatically_by_J4QH3S_Manager";
                            td.Actions.Add(new ExecAction(parameters2[10], null, null));

                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);
                        }

                        MessageBox.Show("Task " + parameters2[0] + ": Whole task created!", "JARVIS TASKER");
                    }

                    this.Close();
                }
            }
            else if (modo == "del")
            {
                string parameters = File.ReadAllText(_strFilePathLATEST + @"\parameters.txt", Encoding.UTF8);
                string[] parameters2 = parameters.Split(',');

                using (TaskService ts = new TaskService())
                {
                    try //Busca la Task por el nombre
                    {
                        Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(parameters2[0]);
                        TaskDefinition td = task.Definition;

                        int qtyTriggers = task.Definition.Triggers.Count();
                        if (qtyTriggers == 1)
                        {
                            ts.RootFolder.DeleteTask(parameters2[0]);
                            MessageBox.Show("Task " + parameters2[0] + ": Whole task deleted!", "JARVIS TASKER");
                        }
                        else
                        {
                            td.Triggers.RemoveAt(Convert.ToInt32(parameters2[1]));
                            ts.RootFolder.RegisterTaskDefinition(parameters2[0], td);

                            MessageBox.Show("Task " + parameters2[0] + ": Trigger deleted!", "JARVIS TASKER");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Task " + parameters2[0] + ": Could not be deleted!", "JARVIS TASKER");
                    }

                    this.Close();
                }
            }
        }


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

        public void executeAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

    }
}
