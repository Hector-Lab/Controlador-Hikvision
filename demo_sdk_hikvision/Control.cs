using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using demo_sdk_hikvision.Clases;

namespace demo_sdk_hikvision
{
    public partial class Control : Form
    {
        Controlador driver;
        btnRegresar addDevice;
        bool activo = false;
        int tarea = 0;
        string respuesta = "";
        int porcentaje = 0;
        string checadorActivo = "-1";
        string idBitacoraActiva = "-1";
        string cliente = "-1";
        string nombreChecador = "";
        string fechaSolicitud = "";
        string textoRecuperado = "";
        IntPtr handler = IntPtr.Zero;
        int dots = 0;
        string[] estado =  { "Procesando", "Procesando.", "Procesando..", "Procesando..." };
        BackgroundWorker worker;
        BackgroundWorker backAsistencias;
        bool debug = true;
        public Control()
        {
            worker = new BackgroundWorker();
            backAsistencias = new BackgroundWorker();
            InitializeComponent();
            driver = new Controlador();
            backAsistencias.DoWork += new DoWorkEventHandler(EnviarAsistenciasSuinpac); //NOTE: para recolexion de asistenias
            worker.DoWork += new DoWorkEventHandler(DescargarEmpleadosSuinpac);         //NOTE: para la descarga masiva de empleados
            worker.ProgressChanged += new ProgressChangedEventHandler(ReportarProgresoSuinpac);
            worker.WorkerReportsProgress = true;
            addDevice = new btnRegresar();
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {
            //NOTE Obtnemos la lista de los dispositivos
            if ( !Variables.Iniciado )
            {
                Variables.Iniciado = driver.IniciarSDK();
            }
            List<Checador> listaChecador = driver.obtenerLsitaChecadores();
            string salida = "Verificando conexion: \n";
            foreach( Checador checador in listaChecador)
            {
                Console.WriteLine("Verificando conexion del dispositivo: "  + checador.Nombre);
                salida += ("    - Conexion del dispositivo " + checador.Nombre + " : " + driver.ProbarConexion(checador.Direccion, checador.Puerto, checador.Usuario, checador.Contrasenia))+" \n";
                salida += ("    - Cerrando session del dispositivo " + checador.Nombre + " : " + Dispositivo.CerrarSesion(checador.Direccion, checador.Puerto, checador.Usuario, checador.Contrasenia)) + " \n";
            }
            txtBox.Text = salida;
            //MessageBox.Show( salida ,"Dispositivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private async void tmrBitacora_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tarea Activa: " + tarea);
            if ( tarea == 0 ) // Si la hay alguna tarea en ejecucion no debe hacer nada
            {
                txtBox.ResetText();
                barEstado.Value = 5;
                lblProgress.Text = "Obteniendo Bitacora";
                txtBox.AppendText("Obteniendo Bitacora...\n");
                Thread.Sleep(2000);
                Bitacora bitacora = await driver.obtenerBitacora();
                if (bitacora != null && bitacora.id != -1)
                {
                    txtBox.AppendText("Procesando tarea: " + bitacora.Tarea + " - " + bitacora.Descripcion);
                    if (!Variables.Iniciado)
                    {
                        Variables.Iniciado = driver.IniciarSDK();
                    }
                    if (Variables.Iniciado)
                    {
                        lblProgress.Text = bitacora.Descripcion;
                        //NOTE: Validamos que no este ocupado el dispositivo
                        lblProgress.Text = await driver.ejecutarTarea(bitacora, this.Handle);
                        barEstado.Value = 100;
                        tarea = bitacora.Tarea;
                        if (bitacora.Tarea == 101)
                        {
                            barEstado.Value = 0;
                            //Enviamos a segundo plano para reportar progreso a suinpac
                            if (worker.IsBusy)
                            {
                                lblProgress.Text = "Sistema ocupado ";
                                Thread.Sleep(2000);
                                lblProgress.Text = "Esperando sigueinte ciclo..";
                                barEstado.Value = 0;
                                txtBox.AppendText("Imposible ejecutar tarea: Sistema ocupado");
                            }
                            else
                            {
                                //NOTE: Enviamos a trabajar me parece que esto hace memoria de mas
                                idBitacoraActiva = Convert.ToString(bitacora.id);
                                cliente = Convert.ToString(bitacora.cliente);
                                checadorActivo = Convert.ToString(bitacora.idChecador);
                                fechaSolicitud = Convert.ToString(bitacora.FechaTupla);
                                worker.RunWorkerAsync();
                                tmrSegundoPlano.Enabled = true;
                                tmrSegundoPlano.Start();
                                Console.WriteLine("Iniciando captura");
                            }
                        }
                    }
                    else
                    {
                        txtBox.AppendText("Hubo un erro al inciar el SDK \n");
                    }
                }
                else
                {
                    lblProgress.Text = "Bitacora vacia";
                    Thread.Sleep(2000);
                    lblProgress.Text = "Esperando..";
                    barEstado.Value = 0;
                    txtBox.AppendText("Bitacora vacia \n Esperando...\n");
                }
            }
            else
            {
                txtBox.AppendText("\n Dispositivos ocupados: Esperando sigueinte ciclo");
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            addDevice.setPantallaPrincipal(this);
            addDevice.setDriver(driver);
            addDevice.Show();
        }
        private async void DescargarEmpleadosSuinpac(object sender, DoWorkEventArgs e)
        {
            //NOTE: obtenemos la lista de los empleados
            //Obtenemos el checador asignado
            string salidaDescarga = "";
            Checador checador = driver.ObtenerDatosChecador(checadorActivo);
            nombreChecador = checador.Nombre;
            //Calculamos el procentaje de avance
            List<Empleado> empleados = await driver.ObtenerListaEmpleados();
            driver.guardarLog("Numero de empleados: " + empleados.Count);
            if ( empleados.Count > 0 )
            {
                int incremento = 100 / empleados.Count;
                int contador = 1;
                if (empleados != null)
                {
                    foreach (Empleado empleado in empleados)
                    {
                        //NOTE: Validamos que los empleados tengan foto y credencial
                        if((empleado.Foto != null && empleado.Foto != "") && ( empleado.Nfc_uid != null && empleado.Nfc_uid != ""))
                        {
                            string respuestaDispositivo = await driver.RegistrarEmpleado(empleado, checador.Direccion, Convert.ToString(checador.Puerto), checador.Usuario, checador.Contrasenia, checador.Nombre,checadorActivo);
                            porcentaje = (contador * incremento);
                            salidaDescarga += "Descargando empleado: " + empleado.Nombre + " --> " + respuestaDispositivo + "\n";
                            //txtBox.AppendText();  
                            contador++;
                            Thread.Sleep(10000);
                        }
                        else
                        {
                            salidaDescarga += (empleado.Nombre) + " No se registro -> Favor de revisar la foto y tarjeta de el empleado \n";
                        }
                    }
                    porcentaje = (100);
                    respuesta = "Lista de empleados descargada";
                    salidaDescarga += "Lista de empleados descargada";
                    salidaDescarga += "Lista de empleados descargada \n";
                    salidaDescarga += "Estado del dispostivo: " + Dispositivo.CerrarSesion(checador.Direccion, checador.Puerto, checador.Usuario, checador.Contrasenia);
                    textoRecuperado = salidaDescarga;
                    driver.guardarLog(salidaDescarga);
                }
            }
        }
        private void EliminarEmpleadosSuinpac(object sender,DoWorkEventArgs e)
        {
            //NOTE: obtenemos la lista de los checadores
            string salidaEliminar = "";
            Console.WriteLine("Eliminando empleados");
            Checador checador = driver.ObtenerDatosChecador(checadorActivo);
            nombreChecador = checador.Nombre;
            List<Empleado> listaEmpleados = driver.obtenerListaEmpleadosDB(checadorActivo);
            Console.WriteLine("Numero de empleados" + listaEmpleados.Count);
            if ( listaEmpleados.Count > 0 )
            {
                int incremento = (100 / listaEmpleados.Count);
                int indiceEmpleados = 0;
                int reintento = 0;
                while ( indiceEmpleados < listaEmpleados.Count )
                {
                    //NOTE: calculamos la tarjeta de los empleados
                    Empleado emp = listaEmpleados[indiceEmpleados]; //NOTE: para hacer un reintento
                    salidaEliminar += "Eliminando Empleado: " + emp.Nombre + "\n";
                    string tarjeta = driver.calcularDecimal(emp.Nfc_uid);
                    string respuestaDispositivo = driver.BorrarEmpleadoDispositivo(tarjeta, checador, handler, emp.idEmpleado) + "\n\n";
                    salidaEliminar += respuestaDispositivo;
                    textoRecuperado += salidaEliminar;
                    if (respuestaDispositivo == "OK")
                    {
                        porcentaje = (indiceEmpleados * incremento);
                        Thread.Sleep(10000); //Para esperar al sigueinte ciclo
                        indiceEmpleados++;
                    }
                    else
                    {
                        if (reintento <= 2)
                        {
                            salidaEliminar += "Error al eliminar el empleado: " + emp.Nombre + " --> Reintentando";
                            textoRecuperado += "\n\nError al eliminar el empleado: " + emp.Nombre + " --> Reintentando";
                            driver.guardarLog("Error al elimnar el empleado: " + (emp.Nombre));
                            reintento++;
                            Thread.Sleep(20000);
                        }
                        else
                        {
                            salidaEliminar += "No fue posible eliminar el empleado: " + emp.Nombre + " --> Favor de eliminarlo manualmente";
                            textoRecuperado += "\n\nNo fue posible eliminar el empleado: " + emp.Nombre + " --> Favor de eliminarlo manualmente";
                            indiceEmpleados++;
                        }
                    }
                }
                porcentaje = (100);
                textoRecuperado = salidaEliminar;
            }
            else
            {
                textoRecuperado = "La lista de empleados esta vacia";
            }
        }

        private void ReportarProgresoSuinpac(object sender, ProgressChangedEventArgs e)
        {
        }
        private async void tmrSegundoPlano_Tick(object sender, EventArgs e)
        {
            lblProgress.Text = respuesta;
            barEstado.Value = porcentaje;
            if ( tarea == 0 )
            {
                if (activo)
                {
                    lblProgress.Text += estado[dots];
                    if (dots == 3)
                    {
                        dots = 0;
                    }
                    dots++;
                }
                else
                {
                    tmrSegundoPlano.Stop();
                    tmrSegundoPlano.Enabled = false;
                    barEstado.Value = 100;
                    txtBox.AppendText(textoRecuperado);
                }
            }
            else
            {
                if (porcentaje >= 100)
                {
                    //NOTE: enviamos la respuesta a suinpac
                    SRespuesta respuestaS = new SRespuesta();
                    respuestaS.Cliente = cliente;
                    respuestaS.Dispositivo = nombreChecador;
                    respuestaS.Fecha_Respuesta = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");
                    respuestaS.Fecha_Solicitud = fechaSolicitud;
                    respuestaS.ERespuesta = Variables.OK101;
                    respuestaS.Respuesta = Variables.OK101;
                    respuestaS.Tarea = "200";
                    string jsonRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuesta);
                    await driver.enviarRespuestaSuinpac(idBitacoraActiva, "Respuesta Tarea: " + tarea, checadorActivo,Convert.ToString((tarea + 100)), jsonRespuesta);
                    lblProgress.Text = "Esperando...";
                    barEstado.Value = 0;
                    tmrSegundoPlano.Enabled = false;
                    tmrSegundoPlano.Stop();
                    tarea = 0;
                    txtBox.AppendText(textoRecuperado);
                    driver.guardarLog("Estado de la tarea: " + tarea,"Bitacora", jsonRespuesta);
                }
            }
        }
        private void tmrCalcula_Tick(object sender, EventArgs e)
        {
            //NOTE: Obtenemos y calculammos las asistencias
            if((tarea != 101))
            {
                if(!backAsistencias.IsBusy)
                {
                    txtBox.AppendText("Recolectando Asistencias");
                    tarea = 0;
                    lblEstado.Text = "Recolectando Asistencias";
                    barEstado.Value = 5;
                    activo = true;
                    tmrSegundoPlano.Enabled = true;
                    tmrSegundoPlano.Start();
                    backAsistencias.RunWorkerAsync();
                }
            }
            else
            {
                txtBox.AppendText("\n Dispositivos ocupados: Esperando siguiente ciclo");
            }
        }
        private async void EnviarAsistenciasSuinpac(object sender, DoWorkEventArgs e)
        {
            string informacionConsola = await driver.calcularAsistenciasRostro();
            textoRecuperado = informacionConsola;
            activo = !(informacionConsola != "");
        }
        private async void btnEventos_Click(object sender, EventArgs e)
        {
            //NOTE:obtenemos las asistencias para los rostros y las trajetas
            
            string resultados = await driver.calcularAsistenciasRostro();
            //txtBox.AppendText( resultados == "-1" ? "Error al conectar con el dispositivo o no existen regisstros de en el dispositivo" : resultados );
            //driver.VerificarConexionInternet();
        }
    }
}
