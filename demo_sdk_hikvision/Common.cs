using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using demo_sdk_hikvision.Clases;

namespace demo_sdk_hikvision
{
    class Variables
    {
        public static string FechaHora = "";
        public static string llave = "DgvMhvJiCaEguKR2OnonsEfVSDRH2Zv93M80qr2mJJMaL4xgU2";
        public static string conexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=datos.mdb";
        public static int m_UserID = -1;
        public static string no_tarjeta = "Pase la tarjeta...";
        public static string evento_fecha_inicio = "";
        public static string evento_fecha_fin = "";
        public static string evento_tipo_principal = "";
        public static string evento_tipo_secundario = "";
        public static string direccion_ip = "";
        public static int puerto = 0;
        public static string usuario = "";
        public static string contrasena = "";
        public static bool Iniciado = false;
        //NOTE: Seccion de aplicacion de asistencias SUINPAC
        public static string APIROOT = "https://api.servicioenlinea.mx/api-movil/test/";
        public static string DBROOT = "@storage:";
        //NOTE: Codigos de errores 
        /**
        100 = Verificar Estado
        101 = Insertar empleado
        102 = Recolectar Asistecias
        103 = Eliminar empleado
        #Respuesta del dispositivo
        200 = Resultado de tarea 100
        201 = Resultado de tarea 101
        202 = Resultado de tarea 102
        203 = Resultado de tarea 103 */
        //Tarea verificar conexion: 100s
        public static string DB100 = "500 -> Checador no encontrado en la base de datos";
        public static string DV100 = "300 -> Dispositivo no encontrado";
        public static string OK100 = "200 -> El dispositivo esta en linea";
        //Tarea Ingresar empleados: 101
        public static string OK101 = "201 -> lista de empleados cargada";
        //Tarea Ingresar empleados: 102
        public static string OK102 = "202 -> Asistencias Enviadas";
        //Tarea Ingresar empleados: 103
        public static string OK103 = "203 -> Empleados eliminados";
        //Tarea Actualizar Empleado:106
        public static string OK106 = "206 -> Empleados eliminados";
        public static List<Asistencia> listaAsistencias = new List<Asistencia>();
    }

    class MD5Crypto
    {
        public static string Encriptar(string _texto)
        {
            byte[] llave;
            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(_texto);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Variables.llave));
            md5.Clear();
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();
            return Convert.ToBase64String(resultado, 0, resultado.Length);
        }

        public static string Desencriptar(string _texto)
        {

            byte[] llave;
            byte[] arreglo = Convert.FromBase64String(_texto);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Variables.llave));
            md5.Clear();
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado);
            return cadena_descifrada;
        }
    }

    public static class Dispositivo
    {
        public static string verificar_conexion(string _direccion_ip, int _puerto, string _usuario, string _contrasena)
        {
            HCNetSDK_Tarjeta.NET_DVR_USER_LOGIN_INFO struLoginInfo = new HCNetSDK_Tarjeta.NET_DVR_USER_LOGIN_INFO();
            HCNetSDK_Tarjeta.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new HCNetSDK_Tarjeta.NET_DVR_DEVICEINFO_V40();
            struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[HCNetSDK_Tarjeta.SERIALNO_LEN];

            struLoginInfo.sDeviceAddress = _direccion_ip;
            struLoginInfo.sUserName = _usuario;
            struLoginInfo.sPassword = _contrasena;
            ushort.TryParse(_puerto.ToString(), out struLoginInfo.wPort);
            string mensaje = "";

            int lUserID = -1;
            lUserID = HCNetSDK_Tarjeta.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);
            if (lUserID >= 0)
            {
                mensaje = "OK";
            }
            else
            {
                uint nErr = HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                if (nErr == HCNetSDK_Tarjeta.NET_DVR_PASSWORD_ERROR)
                {
                    mensaje = "Usuario o contraseña incorrectos.";

                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        mensaje += " " + string.Format("Queda {0} intento(s).", struDeviceInfoV40.byRetryLoginTime);
                    }
                }
                else if (nErr == HCNetSDK_Tarjeta.NET_DVR_USER_LOCKED)
                {
                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        mensaje = string.Format("El usuario está bloqueado, el tiempo de bloqueo restante es de {0} minuto(s).", struDeviceInfoV40.dwSurplusLockTime);
                    }
                }
                else
                {
                    Console.WriteLine(nErr);
                    mensaje = "Hubo un error en la red o el grabador no se encontró";
                }
            }

            return mensaje;
        }

        public static string conectar(string _direccion_ip, int _puerto, string _usuario, string _contrasena,string desde = "Sin rastro")
        {
            DB_control storage = new DB_control();
            try
            {
                HCNetSDK_Tarjeta.NET_DVR_USER_LOGIN_INFO struLoginInfo = new HCNetSDK_Tarjeta.NET_DVR_USER_LOGIN_INFO();
                HCNetSDK_Tarjeta.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new HCNetSDK_Tarjeta.NET_DVR_DEVICEINFO_V40();
                struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[HCNetSDK_Tarjeta.SERIALNO_LEN];

                struLoginInfo.sDeviceAddress = _direccion_ip;
                struLoginInfo.sUserName = _usuario;
                struLoginInfo.sPassword = _contrasena;
                ushort.TryParse(_puerto.ToString(), out struLoginInfo.wPort);
                string mensaje = "";

                Variables.m_UserID = HCNetSDK_Tarjeta.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);
                if (Variables.m_UserID >= 0)
                {
                    Console.WriteLine("Probando Conexion: " + desde);
                    mensaje = "OK";
                }
                else
                {
                    uint nErr = HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                    Console.WriteLine(nErr);
                    if (nErr == HCNetSDK_Tarjeta.NET_DVR_PASSWORD_ERROR)
                    {
                        mensaje = "Usuario o contraseña incorrectos.";

                        if (1 == struDeviceInfoV40.bySupportLock)
                        {
                            mensaje += " " + string.Format("Queda {0} intento(s).", struDeviceInfoV40.byRetryLoginTime);
                        }
                    }
                    else if (nErr == HCNetSDK_Tarjeta.NET_DVR_USER_LOCKED)
                    {
                        if (1 == struDeviceInfoV40.bySupportLock)
                        {
                            mensaje = string.Format("El usuario está bloqueado, el tiempo de bloqueo restante es de {0} minuto(s).", struDeviceInfoV40.dwSurplusLockTime);
                        }
                    }
                    else
                    {
                        storage.guardarLog("Error al probar la conexion del checador Origen: " + desde , "conectar: \n direccion: " + (_direccion_ip) + " \n _puerto: " + _puerto + " \n Usuario:" + _usuario + " \n Contraseña : " + _contrasena,"Codigo: " + nErr);
                        mensaje = "Hubo un error en la red o el grabador no se encontró";
                    }
                }

                return mensaje;
            }catch (Exception ex)
            {
                storage.guardarLog("Error al probar la conexion del checador Origen: " + desde, "conectar: \n direccion: " + (_direccion_ip) + " \n _puerto: " + _puerto + " \n Usuario:" + _usuario + " \n Contraseña : " + _contrasena,ex.Message);
                return ex.Message;
            }
        }
        public static string CerrarSesion( string _direccion_ip, int _puerto, string _usuario, string _contrasena )
        {
            bool result = HCNetSDK_Tarjeta.NET_DVR_Logout_V30( Variables.m_UserID );
            Console.WriteLine(result +" CErrando session");
            if (result)
            {
                //bool limpio = HCNetSDK_Tarjeta.NET_DVR_Cleanup();
                uint nErro = HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                Variables.Iniciado = false;
                //NOTE: liberamos los recursos del SDK
                return "Dispositivo desconectado: " + nErro;
            }
            else
            {
                uint nErro = HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                if( nErro == HCNetSDK_Tarjeta.NET_DVR_USERNOTEXIST)
                {
                    return "El usuario ingresado no existe: " + nErro;

                }
                else{
                    return "Error al cerrar sesion codigo: " + nErro; 
                }
            }
        }
        
    }

    public static class Consultas
    {
        public static void log(string _descripcion)
        {
            string fecha = DateTime.Now.ToString("g"); ;
            Consultas.Insertar_Eliminar_Modificar("insert into log (fecha,descripcion) values ('" + fecha + "','" + _descripcion + "')");
        }

        public static bool Verificar_Dato(string _consulta, string _campo)
        {
            //utilizar consulta count
            string variable = string.Empty;
            OleDbConnection conexion = new OleDbConnection(Variables.conexion);
            OleDbCommand comando = new OleDbCommand(_consulta, conexion);
            conexion.Open();
            OleDbDataReader lectura = comando.ExecuteReader();
            lectura.Read();
            variable = lectura[_campo].ToString();
            lectura.Close();
            conexion.Close();
            if (Convert.ToInt32(variable) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Seleccionar_String(string _consulta, string _campo)
        {
            string variable = string.Empty;
            OleDbConnection conexion = new OleDbConnection(Variables.conexion);
            OleDbCommand comando = new OleDbCommand(_consulta, conexion);
            conexion.Open();
            OleDbDataReader lectura = comando.ExecuteReader();
            lectura.Read();
            variable = lectura[_campo].ToString();
            lectura.Close();
            conexion.Close();
            return variable;
        }

        public static string test_conexion(string _consulta, string _campo, string cadena_conexion)
        {
            string variable = string.Empty;
            OleDbConnection conexion = new OleDbConnection(cadena_conexion);
            OleDbCommand comando = new OleDbCommand(_consulta, conexion);
            conexion.Open();
            OleDbDataReader lectura = comando.ExecuteReader();
            lectura.Read();
            variable = lectura[_campo].ToString();
            lectura.Close();
            conexion.Close();
            return variable;
        }

        public static void Insertar_Eliminar_Modificar(string _consulta)
        {
            Console.WriteLine(_consulta);
            OleDbConnection conexion = new OleDbConnection(Variables.conexion);
            OleDbCommand comando = new OleDbCommand(_consulta, conexion);
            conexion.Open();
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        public static void Seleccionar_Datagridview(string _consulta, DataGridView datagridview)
        {
            OleDbConnection conexion = new OleDbConnection(Variables.conexion);
            OleDbDataAdapter adaptador = new OleDbDataAdapter(_consulta, Variables.conexion);
            DataTable tabla = new DataTable();
            conexion.Open();
            adaptador.Fill(tabla);
            datagridview.DataSource = tabla;
            conexion.Close();
        }

        public static DataTable Seleccionar_datatable(string _consulta)
        {
            OleDbConnection conexion = new OleDbConnection(Variables.conexion);
            OleDbDataAdapter adaptador = new OleDbDataAdapter(_consulta, Variables.conexion);
            DataTable tabla = new DataTable();
            conexion.Open();
            adaptador.Fill(tabla);
            return tabla;
        }
    }
}
