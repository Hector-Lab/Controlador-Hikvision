using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class Checador
    {
        public String Nombre { get; set; }
        public String Usuario { get; set; }
        public String Direccion { get; set; }
        public int Puerto { get; set; }
        public String Contrasenia { get; set; }
        public String Cliente { get; set; }

        public String Estado { get; set; }
        public String Aplica_Sector { get; set; }
        public String Id_Suinpac { get; set; }
        public String Id_Sector { get; set; }
        public void print()
        {
            Console.WriteLine(Nombre + " - " + Usuario + " - " + Direccion + " - " + Puerto + " - " + Contrasenia + " - " + Cliente + " - " + Estado + " - " + Aplica_Sector + " - " + Id_Suinpac + " - " + Id_Sector);
        }
        
    }
}
