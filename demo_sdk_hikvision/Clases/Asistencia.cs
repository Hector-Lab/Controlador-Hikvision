using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{

    public class Asistencia
    {
        public string Fecha { get; set; }
        public string Tarjeta { get; set; }
        public string Empleado { get; set; }
        public void print()
        {
            Console.WriteLine("Tarjeta: "+ Tarjeta + " -> Empleado: " + Empleado + " -> Fecha: " + Fecha);
        }
    }
}
