using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JARVISNamespace
{
    public class CustomTask
    {
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public string TaskFreq { get; set; }
        public string TaskHourTrigger { get; set; }
        public string TaskDate { get; set; }
        public string TaskRepetition { get; set; }
        public string TaskRepetitionDuration { get; set; }
        public string TaskDaysOfWeek { get; set; }
        public string TaskDaysOfMonth { get; set; }
        public string TaskMonthsOfYear { get; set; }

    }


    public class Proceso
    {
        public string ProcessName { get; set; }
        public string Path { get; set; }
        public string MainWindowTitle { get; set; }
        public int PID { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public string Responding { get; set; }
        public int MemoryUsage { get; set; }
        public string StartTime { get; set; }
    }

    public class Sesion
    {
        public string SessionName { get; set; }
        public string UserName { get; set; }
        public int ID { get; set; }
        public string Status { get; set; }
        public string ConnectTime { get; set; }
        public string DisconnectTime { get; set; }
        public string LoginTime { get; set; }
    }

    public class Execution
    {
        public List<Proceso> LstProcesses { get; set; }
        public List<Sesion> LstSessions { get; set; }
        public DateTime Date_Time { get; set; }
    }

    public class Correlativo
    {
        public string ID_Correlativo { get; set; }
        public DateTime CreadoEl { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Descripcion { get; set; }
        public string Bitacora { get; set; }
        public string Estado { get; set; }
    }

    public class Reporte_PBI
    {
        public string ID_Correlativo { get; set; }
        public string Ruta_pbix { get; set; }
        public string Ruta_data { get; set; }
        public string Descripcion { get; set; }
    }
}
