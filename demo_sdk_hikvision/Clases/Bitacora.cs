using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class Bitacora
    {
        public Bitacora() { }
        public int id { get; set; }
        public int idChecador { get; set; }
        public int Tarea { get; set; }
        public string Descripcion { get; set; }
        public string FechaTupla { get; set; }
        public string cliente { get; set; }
    }
}

