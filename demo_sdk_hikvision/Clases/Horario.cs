using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_sdk_hikvision.Clases
{
    public class Horario
    {
        public String Grupo { get; set; }
        public String GrupoDetalle { get; set; }
        public String PuestoEmpleado { get; set; }
        public String GrupoNombre { get; set; }
        public String Jornada { get; set; }
        public int Dia { get; set; }
        public String HoraEntrada { get; set; }
        public String HoraSalida { get; set; }
        public int Tolerancia { get; set; }
        public int Retardo { get; set; }
        public String Estatus { get; set; }
        public int LimiteFaltas { get; set; }
        public int LimiteRetardos { get; set; }
        public int AplicaAsistencia { get; set; }
        public void print()
        {
            Console.WriteLine(
                "Grupo: " + Grupo + " GrupoDetalle: " + GrupoDetalle + " PuestoEmpleado: " + PuestoEmpleado + " GrupoNombre " + GrupoNombre + " Jornada: " + Jornada +
                "Dia: " + Dia + " Hora Entrada: " + HoraEntrada + " Hora Salida: " + HoraSalida + " Tolerancia " + Tolerancia + " Retardo: " + Retardo + " Estado: " + Estatus +
                "Limite de falta: " + LimiteFaltas + " Limite Retados: " + LimiteRetardos + " AplicaAsistencia: " );
        }
    }
}
