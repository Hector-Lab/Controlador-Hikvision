using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class Empleado
    {

        public String idEmpleado { get; set; }
        public String Nombre { get; set; }
        public String Nfc_uid { get; set; }
        public String idChecador { get; set; }
        public String noEmpleado { get; set; }
        public String Cargo { get; set; }
        public String AreaAdministrativa { get; set; }
        public String NombrePlaza { get; set; }
        public String Trabajador { get; set; }
        public String Foto { get; set; }
        public void print()
        {
            Console.WriteLine(idEmpleado + " - " + Nombre + " - " + Nfc_uid + " - " +
                idChecador + " - " + noEmpleado + " - " + Cargo + " - " + AreaAdministrativa +
                " - " + Nombre + " - " + Trabajador + " - " + Foto);
        }
    }
}
