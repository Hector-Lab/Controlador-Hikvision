using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using demo_sdk_hikvision.Clases;
using Newtonsoft.Json;

namespace demo_sdk_hikvision
{
    internal class API_Control
    {
        DB_control storage;
        public API_Control() { storage = new DB_control(); }
        public async Task<Empleado> ObtenerEmpleado(string cl, string empleado)
        {
            Empleado empleadoResult = new Empleado();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/test/DatosEmpleado";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new Dictionary<String, String>
                {
                    {"Cliente",cl},
                    {"Empleado",empleado }
                };
                var values = new FormUrlEncodedContent(data);
                var result = await cliente.PostAsync(dir, values);
                List<Empleado> empleados = await result.Content.ReadAsAsync<List<Empleado>>();
                empleadoResult = empleados.ElementAt<Empleado>(0);
                cliente = null;
                values = null;
                result = null;
                return empleadoResult;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al obtener el empleado", ex.Message);
                empleadoResult.idEmpleado = "-1";
                return empleadoResult;
            }
        }
        public async Task<List<Empleado>> ObtenerEmpleadosMasivo( string cl )
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new Dictionary<string, string>
                {
                    { "Cliente",cl }
                };
                var values = new FormUrlEncodedContent(data);
                //var result = await cliente.PostAsync(Variables.APIROOT+ "EmpleadoMasivo", values);
                var result = await cliente.PostAsync(Variables.APIROOT + "EmpleadoMasivoV2", values);
                List<Empleado> empleados = await result.Content.ReadAsAsync<List<Empleado>>();
                cliente = null;
                values = null;
                result = null;
                return empleados;
            }catch(Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al obtener el empleado masivo", ex.Message);
                return null;
            }
        }
        public async Task<List<Cliente>> ObtenerCliente(  )
        {
            List<Cliente> ListaClientes = new List<Cliente>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/clientes";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    { "Cliente","41" }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                ListaClientes = await data.Content.ReadAsAsync<List<Cliente>>();
                if (ListaClientes.ElementAt<Cliente>(0).id == -1)
                {
                    ListaClientes = new List<Cliente>();
                }
                cliente = null;
                content = null;
                data = null;
                return ListaClientes;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al obtener el empleado masivo", ex.Message);
                return ListaClientes;
            }
        }
        public async Task<List<Sector>> ListaSectores(String cl)
        {
            List<Sector> ListaSectores = new List<Sector>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/ChecadorSectores";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Cliente", cl},
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                ListaSectores = await data.Content.ReadAsAsync<List<Sector>>();
                if (ListaSectores.ElementAt<Sector>(0).id == -1)
                {
                    ListaSectores = new List<Sector>();
                }
                cliente = null;
                content = null;
                data = null;
                return ListaSectores;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error los sectores del cliente", ex.Message); 
                return ListaSectores;
            }
        }
        public async Task<string> RegistrarDispositivo(string cl, string nombre, string dirreccion,string sector, string puerto, string contrasenia, string usuario )
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<String, String>
                {
                    { "Cliente",cl },
                    { "Nombre",nombre },
                    { "Direccion",dirreccion },
                    { "Sector",sector },
                    { "Puerto",puerto },
                    { "Contrasenia",contrasenia },
                    { "Usuario",usuario },
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(Variables.APIROOT + "RegistrarDispositivoInt", content);
                Respuesta rs = await data.Content.ReadAsAsync<Respuesta>();
                return rs.Mensaje;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al registrar dispositivo", ex.Message);
            }
            return "-1";
        }
        public async Task<Bitacora> ObtenerBitacora( string cl )
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<String, String>
                {
                    { "Cliente",cl }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(Variables.APIROOT + "BitacoraInt", content);
                List<Bitacora> lista = await data.Content.ReadAsAsync<List<Bitacora>>();
                Bitacora bitacora = null;
                foreach ( Bitacora bit in lista)
                {
                    bitacora = bit;
                }
                return bitacora;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al obtener la bitacora", ex.Message);
                return null;
            }
            
        }
        public async Task<String> EnviarRespuestaSunipac( string cl, string tarea, string checador, string  rtarea,string dtarea)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<string,string>
                {
                    { "Cliente", cl },
                    { "Tarea", tarea },
                    { "Checador", checador },
                    { "RTarea", rtarea },
                    { "DTarea", dtarea },
                };
                var content = new FormUrlEncodedContent(values);
                var data = await client.PostAsync(Variables.APIROOT + "EnviarRespuestaSuinpac", content);
                Respuesta respuesta = await data.Content.ReadAsAsync<Respuesta>();
                Console.WriteLine(await data.Content.ReadAsStringAsync());
                return respuesta.Mensaje;
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al registrar dispositivo", ex.Message);
                return ex.Message;
            }
        }
        public async Task<List<Horario>> ObtenerAsistenciaEmppleado( string idPersona, string cl )
        {
            try
            {
                List<Horario> listaHorarios;
                String dir = "https://api.servicioenlinea.mx/api-movil/horarioEmpleado";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Cliente",cl },
                    {"Empleado",idPersona }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                string info = await data.Content.ReadAsStringAsync();
                listaHorarios = await data.Content.ReadAsAsync<List<Horario>>();
                if (listaHorarios.ElementAt<Horario>(0).Grupo == "-1")
                {
                    listaHorarios = new List<Horario>();
                }
                cliente = null;
                content = null;
                data = null;
                return listaHorarios;
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al obtener el horario del empleado", ex.Message);
                return null;
            }
        }
        public async Task<Boolean> EnviarAsistencia(String cl, AsistenciaAPI asistencia, List<DetalleAsistencias> detalles, String Omision = "0")
        {
            try
            {
                var jsonStringDetalles = JsonConvert.SerializeObject(detalles);
                var dir = "https://api.servicioenlinea.mx/api-movil/RegistrarAsistenciaChecador";
                //var dir = "https://api.servicioenlinea.mx/api-movil/RegistrarAsistenciaAgregarAsistenciaTest";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<string, string>
                {
                    {"cliente",cl},
                    {"Fecha",asistencia.Fecha },
                    {"FechaTupla",DateTime.Now.ToString("yyyy-MM-dd")},
                    {"idGrupoPersona",Convert.ToString(asistencia.idGrupoPersona)},
                    {"MultipleHorario",Convert.ToString(asistencia.MultipleHorario)},
                    {"Detalles",jsonStringDetalles.ToString()},
                    {"Omision", Omision}

                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                String result = await data.Content.ReadAsStringAsync();
                cliente = null;
                content = null;
                data = null;
                Console.WriteLine(result);
                return result.Equals("1");

            }
            catch (Exception er)
            {
                //dBservice.GuardarRegistrosErrores("-1", "Error al enviar la sistencia :/ " + Convert.ToString(asistencia.idGrupoPersona), DateTime.Now.ToString(), er.Message);
                Console.WriteLine(er.Message + " Error de conexion");
                storage.guardarLog("Error al enviar la asistencia del empleado: " + asistencia.idEmpleado, JsonConvert.SerializeObject(asistencia) + "\n" + JsonConvert.SerializeObject(detalles),er.Message);
                return false;
            }
        }
        public async Task<List<Horario>> ObtenerHorarioEmpleado( string cl, int empleado, string checador )
        {
            try
            {
                var dir = "https://api.servicioenlinea.mx/api-movil/horarioEmpleado";
                HttpClient cliente =  new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<string, string>
                {
                    { "Cliente", cl },
                    { "Checador",checador },
                    { "Empleado", Convert.ToString(empleado) }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                return await data.Content.ReadAsAsync<List<Horario>>();
                
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al obtener el horario del empleado", ex.Message);
                return null;
            }
        }
        public async Task<Empleado> ObtenerDireccionImagen( string cl, string empleado)
        {
            
            try
            {
                var dir = "https://api.servicioenlinea.mx/api-movil/wAsistencias/ObtenerDireccionFoto";

                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<string, string>
                {
                    { "Cliente", cl },
                    { "Empleado", empleado }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir,content);
                Console.WriteLine(await data.Content.ReadAsStringAsync());
                List<Empleado> lista = new List<Empleado>();
                lista = await data.Content.ReadAsAsync<List<Empleado>>();
                return lista.First();
                
            }
            catch (Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al fotografia del empleado" + empleado, ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async void EnviarVitacora101( string cl, string checador )
        {
            try
            {
                var dir = "https://api.servicioenlinea.mx/api-movil/wAsistencias/crearBitacoraChecador";

                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<string, string>
                {
                    { "Cliente", cl },
                    { "Checador", checador},
                    { "Tarea", "101" },
                    { "Descripcion", "Actualizacion masiva de empleados" }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                Console.WriteLine(await data.Content.ReadAsStringAsync());
            }
            catch(Exception ex)
            {
                storage.guardarLog("Error en la API", "Error al fotografia al crear la bitacora" + checador, ex.Message);
                Console.WriteLine(ex.Message);
            }
        }   
    }
}
