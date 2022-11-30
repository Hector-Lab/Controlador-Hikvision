using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class AsistenciaSuinpac
    {
        public int id { get; set; }
        public String Fecha { get; set; }
        public String FechaTupla { get; set; }
        public String GrupoPersona { get; set; }
        public String Grupo { get; set; }
        public String idEmpleado { get; set; }
        public String MultipleHorario { get; set; }
        public String HoraEntrada  { get; set; }
        public String HoraSalida { get; set; }
        public String EstatusAsistencia { get; set; }
        public String Tipo { get; set; }
    }
}
