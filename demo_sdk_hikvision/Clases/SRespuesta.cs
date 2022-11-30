using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class SRespuesta
    {
        public string Fecha_Solicitud { get; set; }
        public string Fecha_Respuesta { get; set; }
        public string Tiempo_Respuesta { get; set; }
        public string Tarea { get; set; }
        public string Dispositivo { get; set; }
        public string Cliente { get; set; }
        public string Respuesta { get; set; }
        public string ERespuesta { get; set; }
    }
}
  