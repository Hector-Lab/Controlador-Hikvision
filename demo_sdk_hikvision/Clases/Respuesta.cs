using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class Respuesta
    {
        public Respuesta() { }
        public bool Status { get; set; }
        public String Mensaje { get; set; }
        public int Code { get; set; }
    }
}
