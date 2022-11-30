using demo_sdk_hikvision.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace demo_sdk_hikvision
{
    public class Controlador
    {
        API_Control servide;
        DB_control storage;
        HCNetSDK_Control device;
        controlEventos CEventos;
        public Controlador()
        {

            servide = new API_Control();
            device = new HCNetSDK_Control();
            storage = new DB_control();
            CEventos = new controlEventos();
            Variables.Iniciado = HCNetSDK_Tarjeta.NET_DVR_Init();
            if (!Variables.Iniciado) //NOTE: Este Se utiliza para iniciar el ambiente del dispositivo 
            {
                Console.WriteLine("Error al inicar SDK!");
                storage.guardarLog("Error al inicar el SDK","SDK: " + Variables.Iniciado,"Error al inicar el SKD");
                return;
            }
            HCNetSDK_Tarjeta.NET_DVR_SetLogToFile(3, "./Logs/", true); //NOTE: se utiliza para guardar el registro de los errores del dispositivo al inicar el porgrama
        }
        //NOTE: metodos para la llamada del dispositivo
        public bool IniciarSDK()
        {
            bool iniciado = HCNetSDK_Tarjeta.NET_DVR_Init();
            storage.guardarLog("EL SDK inicio; " + iniciado, " Verificar inicio del sdk ", Convert.ToString(iniciado));
            return iniciado;
        }
        public string ProbarConexion(string ipAddres, int puerto, string usuario, string contrasenia)
        {
            return Dispositivo.conectar(ipAddres, puerto, usuario, contrasenia);
        }
        public string obtenerRecursos(string dir, string persona)
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Empleados"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Empleados");
                }
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(dir);
                Bitmap bitmap = new Bitmap(stream);
                int noImagen = 0;
                if (Directory.Exists(Directory.GetCurrentDirectory() + "/Empleados/" + (persona) + ".jpg"))
                    while (true)
                    {
                        if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Empleados/" + (persona) + "(" + (noImagen) + ").jpg"))
                        {
                            persona = (persona) + "(" + (noImagen) + ")";
                            Console.WriteLine(persona + " Nueva Direccion");
                            break;
                        }
                        else
                        {
                            Console.WriteLine(persona + " Existe");
                        }
                        noImagen++;
                    }
                if (bitmap != null)
                {
                    bitmap.Save(Directory.GetCurrentDirectory() + "/Empleados/" + (persona) + ".jpg", ImageFormat.Jpeg);
                }
                stream.Flush();
                stream.Close();
                client.Dispose();
                return Directory.GetCurrentDirectory() + "/Empleados/" + (persona) + ".jpg";
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al descargar el foto de empleado","Person: " + persona, ex.Message);
                return null;
            }
        }
        public async Task<string> RegistrarEmpleado(Empleado empd, string ip, string puerto, string user, string password, string nombre,string checadorActivo)
        {
            try
            {
                if (empd.idEmpleado != "-1")
                {
                    //NOTE: agregamos el empleado a la historia 
                    if (!storage.VerificarinsersionEmpleado(empd.idEmpleado,checadorActivo)) // Si 
                    {
                        storage.AgregarHistorialEmpleado(empd,checadorActivo);
                    }
                    //NOTE: Obtenemos los datos
                    string direccion = obtenerRecursos(empd.Foto, empd.idEmpleado);
                    string nfc = calcularDecimal(empd.Nfc_uid);
                    if (direccion == null || nfc == null)
                    {
                        storage.guardarLog("Error al calcular y/o obtener datos del empleado: " + empd.Nombre, ("Error al obtenr recursos\nTarjetas: " + (direccion) + "\nNFC: " + (nfc)), "null");
                        return "Error al obtenr recursos\nTarjetas: " + (direccion) + "\nNFC: " + (nfc);

                    }
                    string cl = storage.ObtenerValues(Variables.DBROOT + "Cliente");
                    if (cl != "")
                    {
                        //NOTE: Obtenemos el horario del empleado y lo guardamos en la base de datos
                        List<Horario> horarios = await servide.ObtenerAsistenciaEmppleado(empd.idEmpleado, cl);
                        if (horarios != null)
                        {
                            storage.eliminarHorarioAnterior(empd.idEmpleado,checadorActivo);
                            foreach (Horario horario in horarios)
                            {
                                if (horario.Grupo != "-1")
                                {
                                    storage.insertarHorario(horario, empd.idEmpleado,checadorActivo);
                                }
                            }
                            return device.agregarUsuarioSuinpac((empd.Nombre.Length >= 32) ? (empd.Nombre.Substring(0, 30)) : empd.Nombre, empd.idEmpleado, "1", nfc, direccion, ip, Convert.ToInt32(puerto), user, password, nombre);
                        }
                        else
                        {
                            storage.guardarLog("Error al descargar el horario del empleado","la lista de horariios no se encontro",empd.Nombre);
                            return "EL horario del empleado " + empd.Nombre + " No Existe";
                        }
                    }
                    else
                    {
                        storage.guardarLog("No se pudo obtener el cliente","Error al conectar con la base de datos","Cliente: " + cl);
                        return "Error al conectar con la base de datos";
                    }
                }
                else
                {
                    return "El empleado no existe Nombre: " + empd.idEmpleado;
                }
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al registrar el empleado: Error General","Error generico",ex.Message);
                return "Error al registrar el empleado";
            }
        }
        public async Task<string>ActualizarRostroEmpleado( Bitacora bitacora)
        {
            try
            {
                string idEmpleado = Convert.ToString(bitacora.Descripcion);
                Console.WriteLine(idEmpleado == "-1 -> " + ("idEmpleado: " + Convert.ToString(idEmpleado) + " == -1"));
                if (idEmpleado != "-1")
                {
                    //Obtenemos los datos del checador
                    Checador checcador = storage.ObtenerChecador(Convert.ToString(bitacora.idChecador));
                    if (checcador != null)
                    {
                        //NOTE: Obtenemos el url de la foto del empleado 
                        Empleado emp = await servide.ObtenerDireccionImagen(checcador.Cliente, idEmpleado);
                        if (emp != null)
                        {
                            if (emp.idEmpleado != "-1")
                            {
                                string nfc = calcularDecimal(emp.Nfc_uid);
                                string direccionFisica = obtenerRecursos(emp.Foto, emp.idEmpleado);
                                if (direccionFisica != "")
                                {
                                    //NOTE: buscamos el horario del empleado en el dispositivo si no lo encuentra insertamos todo
                                    int totalHorarioDB = storage.verificarHorarioEmpleado(emp.idEmpleado);
                                    if (totalHorarioDB <= 0)
                                    {
                                        //NOTE: ingresamos los horarios de los empleados
                                        List<Horario> horarios = await servide.ObtenerAsistenciaEmppleado(emp.idEmpleado, bitacora.cliente);
                                        if (horarios != null)
                                        {
                                            foreach (Horario horario in horarios)
                                            {
                                                if (horario.Grupo != "-1")
                                                {
                                                    storage.insertarHorario(horario, emp.idEmpleado,Convert.ToString(bitacora.idChecador));
                                                }
                                            }
                                        }
                                    }
                                    //NOTE: agregamos el empleado a la historia 
                                    if (storage.VerificarinsersionEmpleado(emp.idEmpleado,Convert.ToString(bitacora.idChecador)))
                                    {
                                        if (storage.eliminarHistorialEmpleado(emp.idEmpleado,Convert.ToString(bitacora.idChecador)))
                                        {
                                            storage.AgregarHistorialEmpleado(emp,Convert.ToString(bitacora.idChecador));
                                        }
                                    }
                                    string respuesta = device.agregarUsuarioSuinpac(emp.Nombre, emp.idEmpleado, "1", nfc, direccionFisica, checcador.Direccion, checcador.Puerto, checcador.Usuario, checcador.Contrasenia, checcador.Nombre);
                                    if (respuesta.Contains("Registrado"))
                                    {
                                        SRespuesta res = new SRespuesta();
                                        res.Respuesta = "200";
                                        res.Cliente = bitacora.cliente;
                                        res.Fecha_Respuesta = DateTime.Today.ToString("yyyy-MM-dd H:mm:ss");
                                        res.Fecha_Solicitud = bitacora.FechaTupla;
                                        res.ERespuesta = "200";
                                        res.Tarea = "106";
                                        // string tarea, string descripcion, string idChecador, string rtarea, string dtarea
                                        return respuesta + " --> " + await servide.EnviarRespuestaSunipac(bitacora.cliente, Convert.ToString(bitacora.id), Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.Tarea + 100), Newtonsoft.Json.JsonConvert.SerializeObject(res)); //NOTE: le enviamos los datos de respuesta en json
                                    }
                                    else
                                    {
                                        storage.guardarLog("Error al registrar el empleado: " + emp.Nombre,"Cerrando session: " + (Dispositivo.CerrarSesion(checcador.Direccion, Convert.ToInt32(checcador.Puerto), checcador.Usuario, checcador.Contrasenia)), respuesta);
                                        return "Error al regitrar el empleado " + respuesta;
                                    }
                                }
                                else
                                {
                                    storage.guardarLog("Foto de empleado no encontrada: " + emp.Nombre, "Cerrando session: " + (Dispositivo.CerrarSesion(checcador.Direccion, Convert.ToInt32(checcador.Puerto), checcador.Usuario, checcador.Contrasenia)), Convert.ToString(emp.Nombre));
                                    return "Foto de empleado no encontrada";
                                }
                            }
                            else
                            {
                                storage.guardarLog("Empleado no encontrado: " +bitacora.Descripcion, "CErrando session: " + (Dispositivo.CerrarSesion(checcador.Direccion, Convert.ToInt32(checcador.Puerto), checcador.Usuario, checcador.Contrasenia)), Convert.ToString(emp));
                                return "Empleado no encontrado";
                            }
                        }
                        else
                        {
                            storage.guardarLog("Error del API", "Cerrando conexion: " + (Dispositivo.CerrarSesion(checcador.Direccion, Convert.ToInt32(checcador.Puerto), checcador.Usuario, checcador.Contrasenia)), Convert.ToString(emp));
                            return "Error al obtenr datos de la API";
                        }
                    }
                    else
                    {
                        storage.guardarLog("Error al conectar con la base de datos", " Error al conectar con la base de datos", Convert.ToString(bitacora.idChecador));
                        return "Error al conectar con la base de datos";
                    }

                }
                else
                {
                    storage.guardarLog("Error al actuzizar el rostro del empleado: Empleado no encontrado", "Empleado no encontrado: ", "la informacion del empleado no existe");
                    return "Empleado no encontrado";
                }
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al actuzizar el rostro del empleado: Error General","Verifique la conexion: ",ex.Message);
                return "Error al actualizar rostro del empleado:\nFavor de verificar la red";
            }
        }
        public string BorrarEmpleadoSDK(Bitacora bitacora, IntPtr handle)
        {
            try
            {
                string salida = "";
                //NOTE: Obtenemos los empleados
                string idEmpleado = Convert.ToString(bitacora.Descripcion);
                Console.WriteLine(idEmpleado == "-1 -> " + ("idEmpleado: " + Convert.ToString(idEmpleado) + " == -1"));
                if (idEmpleado != "-1")
                {
                    //OBTENEMOS EL CHECADOR DE LA TAREA
                    Checador checador = storage.ObtenerChecador(Convert.ToString(bitacora.idChecador));
                    if (checador != null)
                    {
                        string nfcEmpleado = storage.ObtenerNFCEmpleado(idEmpleado);
                        if (nfcEmpleado != null)
                        {
                            if (nfcEmpleado != "-1")
                            {
                                //NOTE: creamos la conexion
                                string conexionRespuesta = Dispositivo.conectar(checador.Direccion, Convert.ToInt32(checador.Puerto), checador.Usuario, checador.Contrasenia);
                                if (conexionRespuesta == "OK")
                                {
                                    salida = device.eliminar_usuario_dispositivo("1", calcularDecimal(nfcEmpleado), handle);
                                }
                                else
                                {
                                    storage.guardarLog("Error al conectar con el dispositivo", "Checador: " + checador.Nombre, conexionRespuesta);
                                    salida += "Error al conectar con el dispositivo";
                                }
                            }
                        }
                        else
                        {
                            storage.guardarLog("Hubo un error al obtener el historial de la persona: " + idEmpleado,"El valor del historial es de 0",Convert.ToString(nfcEmpleado));
                        }
                    }
                    else
                    {
                        storage.guardarLog("Error al comunicarse con la base de datos", "El checador fue null", "null");
                    }
                    //NOTE: Calculamos el decimal del empleado
                    //Empleado emp = await servide.ObtenerDireccionImagen(checcador.Cliente, idEmpleado); // Buscar en la base de datos local
                }
                else
                {
                    storage.guardarLog("El empleado en la bitacora no es valido", "El empleado no es valido", "-1");
                }
                return salida;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al eliminar", bitacora.Descripcion, ex.Message);
                return "";
            }
        }
        public string calcularDecimal(string nfc_hex)
        {
            try
            {
                //NOTE: Rellenamos los datos faltantes
                string rellenoZero = "";
                if (nfc_hex.Length < 10)
                {
                    //NOTE: hacemos un ciclo para rellenar
                    for (int tamanio = nfc_hex.Length; tamanio < 10; tamanio++)
                    {
                        rellenoZero += "0";
                    }
                }
                nfc_hex = (rellenoZero + nfc_hex);
                //NOTE: descomponemos el numero
                //char[] numeros = nfc_hex.ToCharArray();
                Int64 numero = 0;
                var data = new Dictionary<char, int>
                {
                    { '0',0 },{ '1',1 },{ '2',2 },{ '3',3 },{ '4',4 },{ '5',5 },{ '6',6 },{ '7',7 },{ '8',8 },{ '9',9 },{ 'a',10 }
                    ,{ 'b',11 },{ 'c',12 },{ 'd',13 },{ 'e',14 },{ 'f',15 },{ 'A',10 }
                    ,{ 'B',11 },{ 'C',12 },{ 'D',13 },{ 'E',14 },{ 'F',15 }
                };
                for (int indexCaracteres = 0; indexCaracteres < nfc_hex.Length; indexCaracteres++)
                {
                    numero += Convert.ToInt64(data[nfc_hex[indexCaracteres]] * (Math.Pow(16, (nfc_hex.Length - 1) - indexCaracteres)));
                }
                //Relleno
                string relleno = "";
                if (Convert.ToString(numero).Length < 10)
                {
                    for (int indexn = Convert.ToString(numero).Length; indexn < 10; indexn++)
                    {
                        relleno += "0";
                    }
                }

                return Convert.ToString(relleno + numero);
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al calcular tarjeta: " + (nfc_hex), "Datos del nfc: " + nfc_hex,ex.Message);
                return null;
            }
        }
        //Metodos con llamado con el API
        public async Task<List<Cliente>> ObtenerListaClientes()
        {
            return await servide.ObtenerCliente();
        }
        public async Task<List<Sector>> ObtenerSectores(string cls)
        {
            return await servide.ListaSectores(cls);
        }
        public async Task<string> ResgistrarDispositivo(string cl, string nombre, string dirreccion, string sector, string puerto, string contrasenia, string usuario)
        {
            try
            {
                string encripted = MD5Crypto.Encriptar(contrasenia);
                string userEncript = MD5Crypto.Encriptar(usuario);
                string idSuinpac = await servide.RegistrarDispositivo(cl, nombre, dirreccion, sector, puerto, encripted, userEncript);
                if (Convert.ToInt32(idSuinpac) > 0)
                {
                    //Lo registramos en la base de datos
                    storage.AgregarChecador(nombre, MD5Crypto.Encriptar(dirreccion), MD5Crypto.Encriptar(puerto), userEncript, encripted, cl, (sector != "-1" || sector != "") ? "1" : sector, sector, idSuinpac);
                    storage.AgregarValues(Variables.DBROOT + "Cliente", cl, "Cliente general",idSuinpac);
                    return idSuinpac;
                }
                else
                {
                    storage.guardarLog("Error al registrar el dispositivo en suinpac: " + idSuinpac,"Error en la API",Convert.ToString(idSuinpac));
                }
                return "";
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al agregar el dispositivo",
                    "ResgistrarDispositivo:Cadena de conexion " + ("Cliente: " + cl + "\n Nombre: " + nombre +" \n Direccion: " + dirreccion + " \n Sector: " + sector+ "\n Puerton:" + puerto + " \n contrasenia: " + contrasenia + "\n Usuario: " + usuario ),
                    ex.Message);
                return "";
            }

        }
        //NOTE: Manejador de bitacora
        public async Task<Bitacora> obtenerBitacora()
        {
            try
            {
                //NOTE: VErificamos la tarea y obtenemos los datos necesarios
                string cli = storage.ObtenerValues(Variables.DBROOT + "Cliente");
                //NOTE: Obtenemos id del checador 
                if (cli != "")
                {
                    Bitacora bit = await servide.ObtenerBitacora(cli);
                    if (bit != null)
                    {
                        bit.cliente = cli;
                        return bit;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    storage.guardarLog("Error al obtener general ", "Erroa al obtener la bitacora", Convert.ToString(cli));
                    return null;
                }
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error al obtener general ","Erroa al obtener la bitacora",ex.Message);
                return null;
            }
        }
        public async Task<string> ejecutarTarea(Bitacora bitacora, IntPtr handle)
        {
            string resultado = "";
            switch (bitacora.Tarea)
            {
                case 100:
                    resultado = await verificarComunicacion(bitacora);
                    break;
                case 101:
                    //storage.AgregarValues(Variables.DBROOT + "Checador", Convert.ToString(bitacora.idChecador), "Checador en procesos");
                    resultado = "Descargando empleados en segundo plano";
                    break;
                case 102:
                    break;
                case 103:
                    //Tarea para elminar un empleado del checador Descripcion es el id de la persona
                    int empleado = Convert.ToInt32(bitacora.Descripcion);
                    Console.WriteLine("Eliminando al empleado: " + empleado);
                    resultado += BorrarEmpleadoSDK(bitacora,handle);
                    resultado += eliminarEmpleadoDB(Convert.ToString(empleado), Convert.ToString(bitacora.idChecador));
                    //NOTE: eliminamos de la base de datos
                    Console.WriteLine("Eliminado: " + resultado);
                    break;
                case 104:
                    //Ejemplo: PuestoEmpleado_1867 - Actualizar El empleado Pedro Diaz Diaz
                    string[] tarea = bitacora.Descripcion.Split('-'); //pare retornar los que esta haciendo el programa
                    int idEmpleado = Convert.ToInt32(tarea[0].Split('_')[1]);
                    //NOTE: si el resultado es -1 entonces ingresamos al pendejo
                    string actualizado = await ActualizarEmpleadoSuinpac(idEmpleado, Convert.ToString(bitacora.idChecador), bitacora.cliente,Convert.ToString(bitacora.idChecador));
                    resultado += actualizado;
                    if (actualizado == "-1")
                    {
                        Console.WriteLine("Intentando agregar al empleado");
                        resultado += await ActualizarRostroEmpleado(bitacora); //Esto lo inserta face final
                    }
                    //Estructura de respuesta
                    SRespuesta respuestaS = new SRespuesta();
                    respuestaS.Cliente = bitacora.cliente;
                    respuestaS.Dispositivo = "ID SUINPAC: " + bitacora.idChecador;
                    respuestaS.Fecha_Respuesta = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");
                    respuestaS.Fecha_Solicitud = bitacora.FechaTupla;
                    respuestaS.ERespuesta = Convert.ToString(bitacora.Tarea + 100);
                    respuestaS.Respuesta = (actualizado == "-1") ? Convert.ToString(bitacora.Tarea + 300) + ": El empleado no existe en el dispositivo registrando..." : Convert.ToString(bitacora.Tarea + 100) + " Horario del empleado actualizado";
                    respuestaS.Tarea = Convert.ToString(bitacora.Tarea);
                    string jsonRespuesta = JsonConvert.SerializeObject(respuestaS);
                    await enviarRespuestaSuinpac(Convert.ToString(bitacora.id), "Actualiza Empleado:" + tarea[1], Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.Tarea + 100), jsonRespuesta);
                    resultado = (actualizado == "-1") ? Convert.ToString(bitacora.Tarea + 300) + ": El empleado no existe en el dispositivo registrando..." : Convert.ToString(bitacora.Tarea + 100) + ": Horario del empleado actualizado " + tarea[1];
                    break;
                case 106:
                    // (string idEmpleado, string ip, string puerto, string user, string password, string nombre, string cl )
                    //INDEV: Eliminar y  volver a registrar al empleado
                    Console.WriteLine(bitacora.Descripcion + ": Numero de empleado");
                    Console.WriteLine("Intentando borrar al empleado");
                    resultado += BorrarEmpleadoSDK(bitacora,handle);
                    Console.WriteLine("Borrado: " + resultado);
                    Thread.Sleep(10000);
                    //Intentando agregar empleado
                    Console.WriteLine("Intentando agregar al empleado");
                    resultado += await ActualizarRostroEmpleado(bitacora); //Esto lo inserta face final
                    break;
                default:
                    storage.guardarLog("Error al ejecutar tarea","Tarea no soportada por la vercion de software",bitacora.ToString());
                    Console.WriteLine(bitacora.Tarea + " - " + bitacora.Descripcion + " --> Tarea no soportada para la vercion del software actual");
                    break;

            }
            return resultado;
        }
        public async Task<string> verificarComunicacion(Bitacora bitacora)
        {            
            try
            {
                //NOTE: Obtenemos los datos del checador
                Checador checador = storage.ObtenerChecador(Convert.ToString(bitacora.idChecador));
                string resultado = "";
                Console.WriteLine(bitacora.idChecador);
                SRespuesta respuesta = new SRespuesta();
                respuesta.Cliente = bitacora.cliente;
                respuesta.Fecha_Solicitud = bitacora.FechaTupla;
                respuesta.Dispositivo = checador.Nombre;
                respuesta.Tarea = Convert.ToString(bitacora.Tarea);
                respuesta.Fecha_Respuesta = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");
                //respuesta.Tiempo_Respuesta = (Convert.ToDateTime(Convert.ToDateTime(bitacora.FechaTupla)) - DateTime.Today).ToString(@"dd\d\ hh\h\ mm\m\ ss\s\");
                respuesta.Tiempo_Respuesta = "0m";
                respuesta.ERespuesta = Variables.OK100;
                if (Convert.ToInt32(bitacora.idChecador) > 0)
                {
                    resultado = Dispositivo.conectar(checador.Direccion, checador.Puerto, checador.Usuario, checador.Contrasenia);
                    if (resultado == "OK")
                    {
                        resultado = "Dispositivo -> " + resultado;
                        respuesta.Respuesta = Variables.OK100;
                        resultado += "  :::  Suinpac -> " + await servide.EnviarRespuestaSunipac(bitacora.cliente, Convert.ToString(bitacora.id), Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.Tarea + 100), Newtonsoft.Json.JsonConvert.SerializeObject(respuesta)); //NOTE: le enviamos los datos de respuesta en json
                        Console.WriteLine(resultado);
                    }
                    else
                    {
                        resultado = "Dispositivo -> " + resultado;
                        respuesta.Respuesta = Variables.DV100;
                        resultado += "  :::  Suinpac -> " + await servide.EnviarRespuestaSunipac(bitacora.cliente, Convert.ToString(bitacora.id), Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.Tarea + 100), Newtonsoft.Json.JsonConvert.SerializeObject(respuesta)); //NOTE: le enviamos los datos de respuesta en json
                        Console.WriteLine(resultado);
                    }
                }
                else
                {
                    respuesta.Respuesta = Variables.DB100;
                    resultado += " Dispositivo -> Checador no encontrado en la base de datos ::: Suinpac -> " + await servide.EnviarRespuestaSunipac(bitacora.cliente, Convert.ToString(bitacora.id), Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.Tarea + 100), Newtonsoft.Json.JsonConvert.SerializeObject(respuesta)); //NOTE: le enviamos los datos de respuesta en json
                    Console.WriteLine(resultado);
                }
                Console.WriteLine("Estado del dispostivo: " + Dispositivo.CerrarSesion(checador.Direccion, Convert.ToInt32(checador.Puerto), checador.Usuario, checador.Contrasenia));
                return resultado;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error de conexion", "Error de conexion", ex.Message);
                return "";
            }


        }
        public async Task<List<Empleado>> ObtenerListaEmpleados() //NOTE: Pasar aa deprecated
        {
            string cli = storage.ObtenerValues(Variables.DBROOT + "Cliente");
            return await servide.ObtenerEmpleadosMasivo(cli);
        }
        public string ObtenerChecadorAsignado(string checadorAsignado)
        {
            return storage.ObtenerValues(Variables.DBROOT + "Checador");
        }
        public Checador ObtenerDatosChecador(string idChecador)
        {
            return storage.ObtenerChecador(idChecador);
        }
        public async Task<String> enviarRespuestaSuinpac(string tarea, string descripcion, string idChecador, string rtarea, string dtarea)
        {
            string respuesta = "Error al enviar la respuesta";
            string cl = storage.ObtenerValues(Variables.DBROOT + "Cliente");
            if (!string.IsNullOrEmpty(cl))
            {
                respuesta = await servide.EnviarRespuestaSunipac(cl, tarea, idChecador, rtarea, dtarea);
            }
            return respuesta;
        }
        public async Task<string> calcularAsistenciasRostro()
        {
            try
            {
                //NOTE: Obtenemos la ultima fecha de proceso
                string salida = "";
                bool existenRostros = false;
                bool existenTarjetas = false;
                List<Asistencia> listaAsistencia = new List<Asistencia>();
                List<Asistencia> listaAsistenciaTarjeta = new List<Asistencia>();
                int dia = (int)DateTime.Now.DayOfWeek;
                List<Checador> checadores = storage.ObtenerListaChecadores();
                //NOTE: Obtenemos los values de cada checador
                string fechaInicio = storage.ObtenerUltimaFecha();
                string fechaFin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (fechaInicio == null || fechaInicio == "")
                {
                    fechaInicio = DateTime.Now.AddMinutes(-2).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    fechaInicio = Convert.ToDateTime(fechaInicio).AddMinutes(-2).ToString("yyyy-MM-dd HH:mm:ss");
                }
                foreach (Checador checador in checadores)
                {
                    salida += "FECHA DE CONSULTA FACIAL: " + fechaInicio + " --> CHECADOR " + (checador.Nombre);
                    listaAsistencia = CEventos.cargar(checador.Direccion, checador.Puerto, checador.Usuario, checador.Contrasenia, fechaInicio, fechaFin, "Event", "FACE_VERIFY_PASS");
                    Console.WriteLine("\nNumero de eventos faciales encontrados: " + listaAsistenciaTarjeta.Count + " --->  " + checador.Direccion);
                    salida += "Numero de eventos faciales encontrados: " + listaAsistencia.Count + "\n";
                    if (listaAsistencia != null && listaAsistencia.Count > 0)
                    {
                        existenRostros = true;
                        //NOTE: Obtenemos los datos del reconocimiento facila
                        salida += "Procesando Asistencias Rostro ...\n";

                        foreach (Asistencia asistencia in listaAsistencia)
                        {
                            List<Horario> horarioEmplado = storage.obtenerHorarioEmpleado(asistencia.Empleado, dia);
                            //NOTE: obtenemos la lista de horarios del empleado
                            AsistenciaSuinpac aSuinpac = calcularAsistencia(storage.obtenerHorarioEmpleado(asistencia.Empleado, dia), asistencia);
                            salida += "______________________________________________________________________\n";
                            if (aSuinpac == null)
                            {
                                salida += "Día no hábil para el empleado: " + asistencia.Empleado;
                            }
                            else
                            {
                                salida += "Metodo: Facial \n";
                                salida += "Empleado: " + aSuinpac.idEmpleado + "\n";
                                salida += "Fecha: " + aSuinpac.FechaTupla + "\n";
                                salida += "Estado: " + aSuinpac.EstatusAsistencia + "\n";
                                salida += "Hora Entrada: " + aSuinpac.HoraEntrada + "\n";
                                salida += "Hora Salida: " + aSuinpac.HoraSalida + "\n";
                                //NOTE: metodo para agregar cola a las asistencias 
                                storage.insertarAsistencias(aSuinpac);
                                salida += "";
                                Console.WriteLine("Asistencia Guardada");
                            }
                            salida += "______________________________________________________________________\n";
                        }
                    }
                    else
                    {
                        existenRostros = false;
                    }
                    salida += " Cerrando session: " + Dispositivo.CerrarSesion(checador.Direccion, Convert.ToInt32(checador.Puerto), checador.Usuario, checador.Contrasenia);
                }
                //NOTE: despues procesamos los eventos de las tarjetas
                foreach (Checador checadorT in checadores)
                {
                    salida += "FECHA DE CONSULTA TARJETAS: " + fechaInicio + " --> CHECADOR " + (checadorT.Nombre);
                    listaAsistenciaTarjeta = CEventos.cargar(checadorT.Direccion, checadorT.Puerto, checadorT.Usuario, checadorT.Contrasenia, fechaInicio, fechaFin, "Event", "LEGAL_CARD_PASS");
                    Console.WriteLine("\nNumero de eventos en credencial encontrados: " + listaAsistenciaTarjeta.Count + " ---> " +checadorT.Direccion);
                    salida += "\n\nNumero de eventos en credencial encontrados: " + listaAsistenciaTarjeta.Count + "\n";
                    if (listaAsistenciaTarjeta != null && listaAsistenciaTarjeta.Count > 0)
                    {
                        existenTarjetas = true;
                        salida += "Procesando Asistencias Credencial ...\n";
                        foreach (Asistencia asistenciaT in listaAsistenciaTarjeta)
                        {
                            //NOTE: obtenemos la lista de horarios del empleado
                            AsistenciaSuinpac aSuinpac = calcularAsistencia(storage.obtenerHorarioEmpleado(asistenciaT.Empleado, dia), asistenciaT);
                            salida += "______________________________________________________________________\n";
                            salida += "Metodo: Tarjeta \n";
                            salida += "Empleado: " + aSuinpac.idEmpleado + "\n";
                            salida += "Fecha: " + aSuinpac.FechaTupla + "\n";
                            salida += "Estado: " + aSuinpac.EstatusAsistencia + "\n";
                            salida += "Hora Entrada: " + aSuinpac.HoraEntrada + "\n";
                            salida += "Hora Salida: " + aSuinpac.HoraSalida + "\n";
                            if (aSuinpac != null)
                            {
                                //NOTE: metodo para agregar cola a las asistencias 
                                storage.insertarAsistencias(aSuinpac);
                                salida += "";
                                Console.WriteLine("Asistencia Guardada");
                            }
                            salida += "______________________________________________________________________\n";
                        }
                    }
                    else
                    {
                        existenTarjetas = false;
                    }
                    salida += " Cerrando session: " + Dispositivo.CerrarSesion(checadorT.Direccion, Convert.ToInt32(checadorT.Puerto), checadorT.Usuario, checadorT.Contrasenia);
                }
                //NOTE: si no ecuentra nada actualizamos la hora de actualizacion
                if (listaAsistencia.Count > 0 || listaAsistenciaTarjeta.Count > 0)
                {
                    if (existenTarjetas || existenRostros)
                    {
                        if (VerificarConexionInternet()) //NOTE: intentamos conectar a internet para enviar las asistencias
                        {
                            if (await enviarAsistencias())
                            {
                                //NOTE: aqui actualizamos los datos la fecha de actualizacion
                                if (fechaFin == fechaInicio)
                                {
                                    //NOTE: insertamos la fecha actual por que es el primer ciclo
                                    storage.insertarFecha(fechaFin);
                                }
                                else
                                {
                                    //NOTE: actualizamos la fecha de la base de datos
                                    storage.actualizarFecha(fechaFin);
                                }
                            }
                        }
                    }

                }
                else
                {
                    //NOTE: Actualizamos la fecha
                    if (fechaFin == fechaInicio)
                    {
                        //NOTE: insertamos la fecha actual por que es el primer ciclo
                        storage.insertarFecha(fechaFin);
                    }
                    else
                    {
                        //NOTE: actualizamos la fecha de la base de datos
                        storage.actualizarFecha(fechaFin);
                    }
                    return "-1";
                }
                return salida;
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error al recolectar asistencias del dispostivo", "Error en la recolecion atomatica de las asistencias", ex.Message);
                return "";
            }
        }
        public AsistenciaSuinpac calcularAsistencia(List<Horario> horarios, Asistencia asistencia)
        {
            try
            {
                AsistenciaSuinpac objAsistencia = null;
                if (horarios != null && horarios.Count > 0)
                {
                    int retardo = -1;
                    int faltas = -1;
                    Console.WriteLine("Asistencia del empleado: " + horarios[0].PuestoEmpleado);
                    string idEmpleado = horarios[0].PuestoEmpleado;
                    DateTime horaActual = Convert.ToDateTime(asistencia.Fecha);
                    int tipo = -1;
                    foreach (Horario horario in horarios)
                    {
                        DateTime horaEntrada = Convert.ToDateTime(horario.HoraEntrada);
                        DateTime horaEntradaMinimo = horaEntrada.AddMinutes(-30);
                        DateTime horaEntradaTolerancia = horaEntrada.AddMinutes(horario.Tolerancia);
                        DateTime horaEntradaRetardo = horaEntradaTolerancia.AddMinutes(horario.Retardo);
                        DateTime horaEntradaMaximo = horaEntradaRetardo.AddMinutes(30);
                        DateTime horaSalida = Convert.ToDateTime(horario.HoraSalida);
                        //NOTE: para entradas
                        if (horaActual >= horaEntradaMinimo && horaActual <= horaEntradaTolerancia) //NOTE: Estado 1 Asistencia 
                        {
                            objAsistencia = new AsistenciaSuinpac();
                            objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                            objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                            objAsistencia.idEmpleado = idEmpleado;
                            objAsistencia.GrupoPersona = horario.Grupo;
                            objAsistencia.HoraEntrada = horario.HoraEntrada;
                            objAsistencia.HoraSalida = horario.HoraSalida;
                            objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                            objAsistencia.EstatusAsistencia = "1";
                            objAsistencia.Grupo = horario.Grupo;
                            objAsistencia.Tipo = "1";
                            tipo = 1;
                            Console.WriteLine("Asistencia -> Entrada " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                            if (horario.AplicaAsistencia == 0)
                            {
                                objAsistencia.EstatusAsistencia = "1";
                            }
                        }
                        else if (horaActual >= horaEntradaTolerancia && horaActual <= horaEntradaRetardo) //NOTE: Estado 2 retardo
                        {
                            objAsistencia = new AsistenciaSuinpac();
                            objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                            objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                            objAsistencia.idEmpleado = idEmpleado;
                            objAsistencia.GrupoPersona = horario.Grupo;
                            objAsistencia.HoraEntrada = horario.HoraEntrada;
                            objAsistencia.HoraSalida = horario.HoraSalida;
                            objAsistencia.Grupo = horario.Grupo;
                            objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                            objAsistencia.EstatusAsistencia = "2";
                            objAsistencia.Tipo = "1";
                            tipo = 1;
                            Console.WriteLine("Retardo -> Entrada " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                            if (horario.AplicaAsistencia == 0)
                            {
                                objAsistencia.EstatusAsistencia = "1";
                            }
                        }
                        else if (horaActual >= horaEntrada && horaActual <= horaSalida && horaActual <= horaEntradaMaximo) //NOTE: Estado 3 Falta
                        {
                            objAsistencia = new AsistenciaSuinpac();
                            objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                            objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                            objAsistencia.idEmpleado = idEmpleado;
                            objAsistencia.GrupoPersona = horario.Grupo;
                            objAsistencia.HoraEntrada = horario.HoraEntrada;
                            objAsistencia.HoraSalida = horario.HoraSalida;
                            objAsistencia.Grupo = horario.Grupo;
                            objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                            objAsistencia.EstatusAsistencia = "3";
                            objAsistencia.Tipo = "1";
                            tipo = 1;
                            Console.WriteLine("Falta -> Entrada " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                            if (horario.AplicaAsistencia == 0)
                            {
                                objAsistencia.EstatusAsistencia = "1";
                            }
                        }
                        else if (horaActual >= horaEntrada && horaActual <= horaSalida && horaActual > horaEntradaMaximo)// NOTE: Estado 4 no checo
                        {
                            objAsistencia = new AsistenciaSuinpac();
                            objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                            objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                            objAsistencia.idEmpleado = idEmpleado;
                            objAsistencia.GrupoPersona = horario.Grupo;
                            objAsistencia.HoraEntrada = horario.HoraEntrada;
                            objAsistencia.HoraSalida = horario.HoraSalida;
                            objAsistencia.Grupo = horario.Grupo;
                            objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                            objAsistencia.EstatusAsistencia = "9";
                            objAsistencia.Tipo = "1";
                            tipo = 1;
                            Console.WriteLine("No Checo -> Entrada " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                            if (horario.AplicaAsistencia == 0)
                            {
                                objAsistencia.EstatusAsistencia = "1";
                            }
                        }
                    }
                    //NOTE: Para calcular las salidas
                    if (tipo == -1)
                    {
                        String horaSalidaAnterior = "";
                        DateTime salidaAnterior = new DateTime(); //Tiene que ser despeus de los 30 minutos maximos
                        foreach (Horario horario in horarios)
                        {
                            retardo = horario.LimiteRetardos;
                            faltas = horario.LimiteFaltas;
                            DateTime horaSalida = Convert.ToDateTime(horario.HoraSalida);
                            DateTime horaEntrada = Convert.ToDateTime(horario.HoraEntrada);
                            DateTime horaEntradaMinimo = horaEntrada.AddMinutes(-30);
                            DateTime horaSalidaMaximo = horaSalida.AddMinutes(60);
                            if (!String.IsNullOrEmpty(horaSalidaAnterior)) //Caso de multiples horarios
                            {
                                if (horaActual >= salidaAnterior && horaActual >= horaEntradaMinimo && horaActual < horaSalida) //NOTE: salida de la comida
                                {
                                    objAsistencia = new AsistenciaSuinpac();
                                    objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                                    objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                                    objAsistencia.idEmpleado = idEmpleado;
                                    objAsistencia.GrupoPersona = horario.Grupo;
                                    objAsistencia.HoraEntrada = horario.HoraEntrada;
                                    objAsistencia.HoraSalida = horario.HoraSalida;
                                    objAsistencia.Grupo = horario.Grupo;
                                    objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                                    objAsistencia.EstatusAsistencia = "1";
                                    objAsistencia.Tipo = "2";
                                    tipo = 2;
                                    Console.WriteLine("Comida -> salida" + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                                    if (horario.AplicaAsistencia == 0)
                                    {
                                        objAsistencia.EstatusAsistencia = "1";
                                    }
                                }
                                else if (horaActual >= horaSalida && horaActual <= horaSalidaMaximo) //NOTE: Hora de salida normal (varios horarios)
                                {
                                    objAsistencia = new AsistenciaSuinpac();
                                    objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                                    objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                                    objAsistencia.idEmpleado = idEmpleado;
                                    objAsistencia.GrupoPersona = horario.Grupo;
                                    objAsistencia.HoraEntrada = horario.HoraEntrada;
                                    objAsistencia.HoraSalida = horario.HoraSalida;
                                    objAsistencia.Grupo = horario.Grupo;
                                    objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                                    objAsistencia.EstatusAsistencia = "1";
                                    objAsistencia.Tipo = "2";
                                    tipo = 2;
                                    Console.WriteLine("Normal  -> Salida " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                                    if (horario.AplicaAsistencia == 0)
                                    {
                                        objAsistencia.EstatusAsistencia = "1";
                                    }
                                }
                                else if (horaActual > horaSalida && horaActual > horaSalidaMaximo && horaSalida > salidaAnterior) //NOTE: Hora de chacado se cerro Salida (varios horarios) 
                                {
                                    objAsistencia = new AsistenciaSuinpac();
                                    objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                                    objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                                    objAsistencia.idEmpleado = idEmpleado;
                                    objAsistencia.GrupoPersona = horario.Grupo;
                                    objAsistencia.HoraEntrada = horario.HoraEntrada;
                                    objAsistencia.HoraSalida = horario.HoraSalida;
                                    objAsistencia.Grupo = horario.Grupo;
                                    objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                                    objAsistencia.EstatusAsistencia = "9";
                                    objAsistencia.Tipo = "2";
                                    tipo = 2;
                                    Console.WriteLine("Checador Cerrado -> Salida " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                                    if (horario.AplicaAsistencia == 0)
                                    {
                                        objAsistencia.EstatusAsistencia = "1";
                                    }
                                }
                            }
                            else //NOTE: caso de un solo horario
                            {
                                if (horaActual >= horaSalida && horaActual <= horaSalidaMaximo)
                                {
                                    objAsistencia = new AsistenciaSuinpac();
                                    objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                                    objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                                    objAsistencia.idEmpleado = idEmpleado;
                                    objAsistencia.GrupoPersona = horario.Grupo;
                                    objAsistencia.HoraEntrada = horario.HoraEntrada;
                                    objAsistencia.HoraSalida = horario.HoraSalida;
                                    objAsistencia.Grupo = horario.Grupo;
                                    objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                                    objAsistencia.EstatusAsistencia = "1";
                                    objAsistencia.Tipo = "2";
                                    tipo = 2;
                                    Console.WriteLine("Asistencia -> Salida " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                                    if (horario.AplicaAsistencia == 0)
                                    {
                                        objAsistencia.EstatusAsistencia = "1";
                                    }
                                }
                                else if (horaActual > horaSalida && horaActual > horaSalidaMaximo)
                                {
                                    objAsistencia = new AsistenciaSuinpac();
                                    objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                                    objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                                    objAsistencia.idEmpleado = idEmpleado;
                                    objAsistencia.GrupoPersona = horario.Grupo;
                                    objAsistencia.HoraEntrada = horario.HoraEntrada;
                                    objAsistencia.HoraSalida = horario.HoraSalida;
                                    objAsistencia.Grupo = horario.Grupo;
                                    objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                                    objAsistencia.EstatusAsistencia = "9";
                                    objAsistencia.Tipo = "2";
                                    tipo = 2;
                                    Console.WriteLine("Checador Cerrado -> Salida " + " \nEntrada: " + horario.HoraEntrada + " - Salida: " + horario.HoraSalida + " Registro ->  " + horaActual.ToString());
                                    if (horario.AplicaAsistencia == 0)
                                    {
                                        objAsistencia.EstatusAsistencia = "1";
                                    }
                                }
                            }
                            horaSalidaAnterior = horario.HoraSalida;
                            salidaAnterior = Convert.ToDateTime(horaSalidaAnterior);
                        }
                        if (tipo == -1)
                        {
                            //NOTE: entrada minimo
                            objAsistencia = new AsistenciaSuinpac();
                            objAsistencia.FechaTupla = horaActual.ToString("yyyy-MM-dd HH:mm:ss");
                            objAsistencia.Fecha = horaActual.ToString("yyyy-MM-dd");
                            objAsistencia.idEmpleado = idEmpleado;
                            objAsistencia.GrupoPersona = horarios[0].Grupo;
                            objAsistencia.HoraEntrada = horarios[0].HoraEntrada;
                            objAsistencia.HoraSalida = horarios[0].HoraSalida;
                            objAsistencia.Grupo = horarios[0].Grupo;
                            objAsistencia.MultipleHorario = horarios.Count > 1 ? "2" : "1";
                            objAsistencia.EstatusAsistencia = "-1";
                            objAsistencia.Tipo = "1";
                            tipo = 1;
                            Console.WriteLine("Aun no se habilita la hora de entrada -> Entrada " + " \nEntrada: ");
                        }
                    }
                }
                return objAsistencia;
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error al calcualar estado de la asistencia","Error en el calculo",ex.Message);
                return null;
            }
        }
        public bool VerificarConexionInternet()
        {
            Uri urlSuinpac = new Uri("https://suinpac.com/");
            WebRequest WebRequest;
            WebRequest = System.Net.WebRequest.Create(urlSuinpac);
            WebResponse objetoResp;
            try
            {
                objetoResp = WebRequest.GetResponse();
                objetoResp.Close();
                WebRequest = null;
                return true;

            }
            catch (Exception ex)
            {
                storage.guardarLog("Error de conexion", "Suinpac no responde o no hay conexion a internet",ex.Message);
                WebRequest = null;
                return false;
            }
        }
        public async Task<bool> enviarAsistencias()
        {
            try{
                //NOTE: las obtenemos de la base de datos, estas ya estan depuradas
                List<AsistenciaAPI> listaAsistencias = storage.ObtenerAsistencias();
                int indexAsistencias = 0;
                foreach (AsistenciaAPI asistencia in listaAsistencias)
                {
                    //NOTE: Obtenemos los detalles de las asistencias
                    List<DetalleAsistencias> listaAsistenciaDetalles = storage.obtenerDetalleAsistencia(asistencia.id);
                    String cliente = storage.ObtenerValues(Variables.DBROOT + "Cliente");
                    bool data = await servide.EnviarAsistencia(cliente, asistencia, listaAsistenciaDetalles);
                    if (data) //NOTE: si la asistencia o el detalle se envio correctamente si no se guardar los datos en la bitacora
                    {
                        //NOTE: Eliminamos la asistencia de la base de datos y la pasamoa al historial
                        storage.EliminarAsistencia(Convert.ToString(asistencia.id));
                        indexAsistencias++;
                    }
                    else
                    {
                        storage.guardarLog("Error al enviar asistencia favor de verificar horario asignado: " + asistencia.id, JsonConvert.SerializeObject(asistencia) + "\n" + JsonConvert.SerializeObject(listaAsistenciaDetalles), "Se envio: " + Convert.ToString(data));
                    }

                }
                Console.WriteLine("Tamaño de la lsita: " + listaAsistencias.Count + " - " + indexAsistencias);
                return (indexAsistencias == listaAsistencias.Count);
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error en las asistencias", "Error al enviar asistencia", ex.Message);
                return false;
            }
            
        }
        public async Task<string> ActualizarEmpleadoSuinpac(int idEmpleado, string idChecador, string cliente,string checadore)
        {

            try
            {
                //NOTE: verificamos que el empleado exista
                if(storage.VerificarinsersionEmpleado(Convert.ToString(idEmpleado), idChecador))
                {
                    //NOTE: Obtenermos el horario del empleado mediante el API
                    List<Horario> listaaHorarios = await servide.ObtenerHorarioEmpleado(cliente, idEmpleado, idChecador);
                    Console.WriteLine("Existen horarios del empleado: " + listaaHorarios.Count);
                    if (listaaHorarios.Count > 0)
                    {
                        //NOTE: borramos el horario anterior del empleado
                        storage.eliminarHorarioAnterior(Convert.ToString(idEmpleado), checadore);
                    }
                    foreach (Horario horario in listaaHorarios)
                    {
                        if (horario.Grupo != "-1")
                        {
                            storage.insertarHorario(horario, Convert.ToString(idEmpleado), checadore);
                        }
                    }
                    return "Horario Actualizado";
                }
                else
                {
                    //NOTE: no esta en el dispositivo usamos el sdk
                    return "-1";
                }
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error al actualizar el horario del empleado: " + idEmpleado, "Error general", ex.Message);
                return "El empleado no tiene horario asignado";
            }
        }
        public  List<Checador> obtenerLsitaChecadores()
        {
            return storage.ObtenerListaChecadores();
        }
        public void CrearBitacoraDescargarEmpleados( string cl, string checador )
        {
            servide.EnviarVitacora101(cl,checador);
        }
        public bool verificarDispositivo( string direccion)
        {
            return storage.VerificarChecador(MD5Crypto.Encriptar(direccion));
        }
        public void guardarLog(string refereci,string desc = "", string log = "")
        {
            storage.guardarLog((desc == "" ) ? "Datos desde el formulario" : desc, refereci,(log == "") ? "Mensaje de log" : log );
        }
        public string BorrarEmpleadoDispositivo( string idTarjeta, Checador checador , IntPtr handle, string idEmpleado )// INDEV:
        {
            try
            {
                string salida = "";
                //Conectamos con el dispositivo
                string conexionRespuesta = Dispositivo.conectar(checador.Direccion, Convert.ToInt32(checador.Puerto), checador.Usuario, checador.Contrasenia);
                if (conexionRespuesta == "OK")
                {
                    salida = device.eliminar_usuario_dispositivo("1",idTarjeta, handle);
                    Console.WriteLine("Resultado SDK: " + salida);
                    if(salida == "NET_DVR_DEL_CARD success:NET_DVR_DEL_CARD finish")
                    {
                        //NOTE: Eliminamos los datos en local
                        Console.WriteLine("Eliminando Empleado local: " + idEmpleado + " --> result: " +(storage.eliminarHistorialEmpleado(idEmpleado,checador.Id_Suinpac)));
                        return "OK";
                    }
                    else
                    {
                        return salida;
                    }
                }
                else
                {
                    storage.guardarLog("Error al conectar con el dispositivo", "Checador: " + checador.Nombre, conexionRespuesta);
                    salida += "Error al conectar con el dispositivo";
                }
                return salida;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error al empleado: ", "Decimal de la tarjeta : " + idTarjeta , ex.Message);
                return "";
            }
        }
        public List<Empleado> obtenerListaEmpleadosDB( string checadorActivo )
        {
            return storage.ObtenerListaEmpleados(checadorActivo);
        }
        public string eliminarEmpleadoDB( string idEmpleado, string idChecador )
        {
            //NOTE: borramos el horario del empleado
            string resultado = "";
            string horarioBorrado =  storage.eliminarHorarioAnterior(idEmpleado,idChecador);
            bool personaBorrada = storage.eliminarHistorialEmpleado(idEmpleado, idChecador);
            return "Horario eliminado: " + horarioBorrado + "\n\n Persona eliminada: " + personaBorrada; 
        }
    }
}
