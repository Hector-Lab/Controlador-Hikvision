using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class DetalleAsistencias
    {
        public int id { get; set; } 
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public string EstatusAsistencia { get; set; }
        public String FechaTupla { get; set; }
        public string Tipo { get; set; }
        public string idAsistencia { get; set; }
        public String HoraAsistencia { get; set; }
    }
}
