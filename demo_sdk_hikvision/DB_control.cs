using Microsoft.Data.Sqlite;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_sdk_hikvision.Clases;

namespace demo_sdk_hikvision
{
    public class DB_control
    {
        public DB_control() {
        }
        public MySqlConnection abrirConexion()
        {
            MySqlConnection conectar = new MySqlConnection("server = 127.0.0.1; database=checador; Uid=root; pwd='B2KyaU8%u2&4';");
            try
            {
                conectar.Open();
                return conectar;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos","Error abrir la base de datos",ex.Message);
                return conectar;
            }
        }
        public void AgregarChecador( string nombre,string direccion, string puerto, string usuario,string password, string cliente, string aplicaSector, string sector, string idSuinpac )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO checadores(id,Nombre,Usuario,Direccion,Puerto,Contrasenia,Cliente,Estado,Aplica_Sector,Id_Sector,Id_Suinpac) VALUES (null,@Nombre,@Usuario,@Direccion,@Puerto,@Contrasenia,@Cliente,@Estado,@Aplica_Sector,@Id_Sector,@Id_Suinpac)";
                cmd.Connection = abrirConexion();
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Direccion", direccion);
                cmd.Parameters.AddWithValue("@Puerto", puerto);
                cmd.Parameters.AddWithValue("@Contrasenia", password);
                cmd.Parameters.AddWithValue("@Cliente", cliente);
                cmd.Parameters.AddWithValue("@Estado", 1);
                cmd.Parameters.AddWithValue("@Aplica_Sector", aplicaSector);
                cmd.Parameters.AddWithValue("@Id_Sector", sector);
                cmd.Parameters.AddWithValue("@Id_Suinpac", idSuinpac);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos","Error al agregar nuevo checador",ex.Message);
                Console.WriteLine(ex.Message);
            }

        }
        public Checador ObtenerChecador( string idSuinpac )
        {
            Checador checador = new Checador();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM checadores WHERE Id_Suinpac = @Id_Suinpac";
                cmd.Connection = abrirConexion();
                cmd.Parameters.AddWithValue("@Id_Suinpac", idSuinpac);
                MySqlDataReader result = cmd.ExecuteReader();
                while (result.Read())
                {
                    checador.Nombre = result.GetString(1);
                    checador.Usuario = MD5Crypto.Desencriptar(result.GetString(2));
                    checador.Direccion = MD5Crypto.Desencriptar(result.GetString(3));
                    checador.Puerto = Convert.ToInt32(MD5Crypto.Desencriptar(result.GetString(4)));
                    checador.Contrasenia = MD5Crypto.Desencriptar(result.GetString(5));
                    checador.Cliente = result.GetString(6);
                    checador.Estado = result.GetString(7);
                    checador.Aplica_Sector = result.GetString(8);
                    checador.Id_Sector = result.GetString(9);
                    checador.Id_Suinpac = result.GetString(10);
                }
                cmd.Connection.Close();
                return checador;

            }
            catch( Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al agregar nuevo checador", ex.Message);
                checador.Estado = ex.Message;
                return null;
            }
        }
        public void AgregarValues( string nombre, string value, string descripcion, string idSuinpac )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO storagevalues(id, Nombre, Valor, Descripcion, idSuinpac) VALUES ( @id, @Nombre, @Valor, @Descripcion, @idSuinpac)";
                cmd.Connection = abrirConexion();
                cmd.Parameters.AddWithValue("@id", null);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Valor", value);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@idSuinpac", idSuinpac);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch( Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al agregar configuraciones storagevalues", ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        public string ObtenerValues(string nombre) {
            try
            {
                string valor = "";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM storagevalues WHERE Nombre = @Nombre";
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    valor = reader.GetString(2);
                }
                cmd.Connection.Close();
                return valor;
            }catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al leer configuraciones storagevalues", ex.Message);
                return "";
            }

        }
        public List<Checador> ObtenerListaChecadores()
        {
            List<Checador> listaChecador = new List<Checador>();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM checadores";
                MySqlDataReader result = cmd.ExecuteReader();
                Checador checador;
                while (result.Read())
                {
                    checador = new Checador();
                    checador.Nombre = result.GetString(1);
                    checador.Usuario = MD5Crypto.Desencriptar(result.GetString(2));
                    checador.Direccion = MD5Crypto.Desencriptar(result.GetString(3));
                    checador.Puerto = Convert.ToInt32(MD5Crypto.Desencriptar(result.GetString(4)));
                    checador.Contrasenia = MD5Crypto.Desencriptar(result.GetString(5));
                    checador.Cliente = result.GetString(6);
                    checador.Estado = Convert.ToString(result.GetString(7));
                    checador.Aplica_Sector = Convert.ToString(result.GetString(8));
                    checador.Id_Sector = Convert.ToString(result.GetString(9));
                    checador.Id_Suinpac = Convert.ToString(result.GetString(10));
                    listaChecador.Add(checador);
                    //checador.print();
                }
                cmd.Connection.Close();
                return listaChecador;
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al leer la lista de checadores", ex.Message);
                Console.WriteLine(ex.Message);
                return listaChecador;
            }
        }
        public string ObtenerUltimaFecha()
        {
            try
            {
                string fecha = "";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT Valor FROM storagevalues WHERE Nombre = @nombre";
                cmd.Parameters.AddWithValue("@nombre", Variables.DBROOT+ "FechaActualizacion");
                MySqlDataReader result = cmd.ExecuteReader();
                while (result.Read())
                {
                    fecha = result.GetString(0);
                }
                cmd.Connection.Close();
                return fecha;
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al leer fecha de actualizacion", ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public void insertarFecha(string fecha)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO storagevalues(id, Nombre, Valor, Descripcion) VALUES ( @id, @Nombre, @Valor, @Descripcion )";
                cmd.Connection = abrirConexion();
                cmd.Parameters.AddWithValue("@id", null);
                cmd.Parameters.AddWithValue("@Nombre", Variables.DBROOT +"FechaActualizacion");
                cmd.Parameters.AddWithValue("@Valor",fecha);
                cmd.Parameters.AddWithValue("@Descripcion", "Fecha de la ultima vez que se generaron y enviaron las asistencias a suinpac");
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al insertar fecha de actualizacion", ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        public void actualizarFecha( string fecha)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "UPDATE storagevalues SET Valor = @fecha WHERE Nombre = '" + (Variables.DBROOT + "FechaActualizacion")+"'";
                cmd.Parameters.AddWithValue("@fecha",fecha);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al actualizar fecha de actualizacion", ex.Message);
                Console.WriteLine (ex.Message);
            }
        }
        public void insertarHorario( Horario horario, string idEmpleado,string checador)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "INSERT INTO Horario VALUES(null,@Grupo,@GrupoDetalle,@PuestoEmpleado,@GrupoNombre,@Jornada,@Dia,@HoraEntrada,@HoraSalida,@Retardo,@Tolerancia,@Estatus,@idEmpleado,@LimiteFaltas,@LimiteRetardos,@AplicaAsistencia,@Checador)";
                cmd.Parameters.AddWithValue("@Grupo", horario.Grupo);
                cmd.Parameters.AddWithValue("@GrupoDetalle", horario.GrupoDetalle);
                cmd.Parameters.AddWithValue("@PuestoEmpleado", horario.PuestoEmpleado);
                cmd.Parameters.AddWithValue("@GrupoNombre", horario.GrupoNombre);
                cmd.Parameters.AddWithValue("@Jornada", horario.Jornada);
                cmd.Parameters.AddWithValue("@Dia", horario.Dia);
                cmd.Parameters.AddWithValue("@HoraEntrada", horario.HoraEntrada);
                cmd.Parameters.AddWithValue("@HoraSalida", horario.HoraSalida);
                cmd.Parameters.AddWithValue("@Retardo", horario.Retardo);
                cmd.Parameters.AddWithValue("@Tolerancia", horario.Tolerancia);
                cmd.Parameters.AddWithValue("@Estatus", horario.Estatus);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@LimiteFaltas", horario.LimiteFaltas);
                cmd.Parameters.AddWithValue("@LimiteRetardos", horario.LimiteRetardos);
                cmd.Parameters.AddWithValue("@AplicaAsistencia", horario.AplicaAsistencia);
                cmd.Parameters.AddWithValue("@Checador", checador);
                cmd.ExecuteReader();
                cmd.Connection.Close();
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al insertar horario", ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        public bool verificarHorario( string idEmpleaod)
        {
            try
            {
                bool contiene = true; 
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT COUNT(id) FROM `horario` WHERE idEmpleado = @idEmpleado";
                cmd.Parameters.AddWithValue("@idEmpleado",idEmpleaod);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contiene = (reader.GetInt32(0) > 0);
                }
                cmd.Connection.Close();
                return contiene;

            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al insertar horario", ex.Message);
                Console.WriteLine(ex.Message);
                return true;
            }
        }
        public string eliminarHorarioAnterior( string idEmpleado,string checador)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "DELETE FROM horario WHERE idEmpleado = @idEmpleado AND Checador = @Checador";
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@Checador", checador);
                string result =  Convert.ToString(cmd.ExecuteNonQuery());
                Console.WriteLine("SQL: DELETE FROM horario WHERE idEmpleado = " + idEmpleado + " AND Checador = " + checador);
                Console.WriteLine("Datos Eliminados: " + result);
                cmd.Connection.Close();
                return "OK";
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al eliminar horario", ex.Message);
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }
        public List<Horario> obtenerHorarioEmpleado( string idEmpleado, int dia )
        {
            try
            {
                List<Horario> listaHorario = new List<Horario>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM horario WHERE idEmpleado = @idEmpleado AND Dia = @dia";
                cmd.Parameters.AddWithValue("@idEmpleado",idEmpleado);
                cmd.Parameters.AddWithValue( "@dia", dia );
                MySqlDataReader reader = cmd.ExecuteReader();
                while( reader.Read() )
                {
                    Horario horario = new Horario();
                    horario.Grupo = Convert.ToString(reader.GetInt32(1));
                    horario.GrupoDetalle = Convert.ToString(reader.GetInt32(2));
                    horario.PuestoEmpleado = Convert.ToString(reader.GetInt32(3));
                    horario.GrupoNombre = reader.GetString(4);
                    horario.Jornada = Convert.ToString(reader.GetInt32(5));
                    horario.Dia = reader.GetInt32(6);
                    horario.HoraEntrada = reader.GetString(7);
                    horario.HoraSalida = reader.GetString(8);
                    horario.Retardo = reader.GetInt32(9);
                    horario.Tolerancia = reader.GetInt32(10);
                    horario.Estatus = Convert.ToString(reader.GetInt32(11));
                    horario.LimiteFaltas = reader.GetInt32(13);
                    horario.LimiteRetardos = reader.GetInt32(14);
                    horario.AplicaAsistencia = reader.GetInt32(15);
                    listaHorario.Add(horario);
                }
                cmd.Connection.Close();
                return listaHorario;

            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al obtener horario", ex.Message);
                return null;
            }
        }
        public void GuardarAsistencia( AsistenciaSuinpac asistencia )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO asistencia (id,Fecha,FechaTupla,GrupoPersona,idEmpleado,MultipleHorario) VALUES (null,@Fecha,@FechaTupla,@GrupoPersona,@idEmpleado,@MultipleHorario); select last_insert_id();";
                cmd.Connection = abrirConexion();
                cmd.Parameters.AddWithValue("@Fecha", asistencia.Fecha);
                cmd.Parameters.AddWithValue("@FechaTupla",asistencia.FechaTupla);
                cmd.Parameters.AddWithValue("@GrupoPersona", asistencia.GrupoPersona);
                cmd.Parameters.AddWithValue("@idEmpleado", asistencia.idEmpleado);
                cmd.Parameters.AddWithValue("@MultipleHorario", asistencia.MultipleHorario);
                long idAsistencia = Convert.ToInt64(cmd.ExecuteScalar());
                cmd.Connection.Close();
                //Ingresamos los datos
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.CommandText = "INSERT INTO asistenciadetalle (id,HoraEntrada,HoraSalida,EstatusAsistencia,Tipo,idAsistencia,FechaTupla, HoraAsistencia) VALUES (null,@HoraEntrada,@HoraSalida,@EstatusAsistencia,@Tipo,@idAsistencia,@FechaTupla,@HoraAsistencia)";
                cmd2.Connection = abrirConexion();
                cmd2.Parameters.AddWithValue("@HoraEntrada",asistencia.HoraEntrada);
                cmd2.Parameters.AddWithValue("@HoraSalida", asistencia.HoraSalida);
                cmd2.Parameters.AddWithValue("@EstatusAsistencia", asistencia.EstatusAsistencia);
                cmd2.Parameters.AddWithValue("@Tipo", asistencia.Tipo);
                cmd2.Parameters.AddWithValue("@idAsistencia", idAsistencia);
                cmd2.Parameters.AddWithValue("@FechaTupla", asistencia.FechaTupla);
                cmd2.Parameters.AddWithValue("@HoraAsistencia", Convert.ToDateTime(asistencia.FechaTupla).ToString("HH:mm:ss"));
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al guardar asistencia Empleado: " + asistencia.idEmpleado + " Hora: " + Convert.ToDateTime(asistencia.FechaTupla).ToString("HH:mm:ss"), ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        public bool VerificarDuplicados( string registroActual , string idEmpleado, string Tipo )
        {
            try
            {
                bool found = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT asistenciadetalle.* FROM asistencia JOIN asistenciadetalle on ( asistenciadetalle.idAsistencia = asistencia.id ) WHERE asistencia.idEmpleado = @idEmpleado";
                cmd.Parameters.AddWithValue("@@idEmpleado", idEmpleado);
                MySqlDataReader reader = cmd.ExecuteReader();
                //Registro actual
                DateTime actual = Convert.ToDateTime(registroActual);
                while ( reader.Read() ) {
                    //Registros de la base de datos 
                    DateTime fechaInicioDB = Convert.ToDateTime(reader.GetString(1));
                    DateTime fechaFinDB = Convert.ToDateTime(reader.GetString(2));
                    string tipoDB = reader.GetString(4);
                    //Entradas
                    if ( fechaInicioDB >= actual && fechaFinDB <= actual && Tipo == tipoDB )
                    {
                        found = true;
                        break;
                    }
                    //Para la salidas del dia
                    if( fechaInicioDB > actual && fechaFinDB > actual)
                    {
                        found = true;
                        break;
                    }
                }
                return found;

            }
            catch (Exception ex)
            {

                guardarLog("Error en la base de datos", "Error al verificar duplicados",ex.Message);
                return true;
            }
        }
        public void insertarAsistencias(AsistenciaSuinpac asistenciaActual)
        {
            try
            {
                if ( asistenciaActual.EstatusAsistencia == "-1" )
                {
                    
                    guardarHistoriaAsistencia(asistenciaActual.FechaTupla, asistenciaActual.HoraEntrada, asistenciaActual.HoraSalida, asistenciaActual.Tipo, asistenciaActual.EstatusAsistencia, asistenciaActual.idEmpleado);
                    return;
                }
                //Obtenemos el id de la asistencia
                int idAsistencia = buscarAsistencia(asistenciaActual.idEmpleado);
                if (idAsistencia > 0)
                {
                    int detalle = buscarDetalle(asistenciaActual.Tipo, asistenciaActual.HoraEntrada, asistenciaActual.HoraSalida,asistenciaActual.idEmpleado,Convert.ToString(idAsistencia),asistenciaActual.Fecha);
                    if(detalle <= 0)
                    {
                        insertarDetalleAsistencia(asistenciaActual, idAsistencia);
                        guardarHistoriaAsistencia(asistenciaActual.FechaTupla, asistenciaActual.HoraEntrada, asistenciaActual.HoraSalida, asistenciaActual.Tipo, asistenciaActual.EstatusAsistencia, asistenciaActual.idEmpleado);
                    }
                    else
                    {
                        Console.WriteLine("El empleado ya tomo su asistencia actual");
                    }
                }
                else // Para insertar detalles
                {
                    GuardarAsistencia(asistenciaActual);
                    guardarHistoriaAsistencia(asistenciaActual.FechaTupla,asistenciaActual.HoraEntrada,asistenciaActual.HoraSalida,asistenciaActual.Tipo,asistenciaActual.EstatusAsistencia,asistenciaActual.idEmpleado);
                }
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error insertar asistecias: \nEstado: " + (asistenciaActual.EstatusAsistencia) + "\nFecha: " + asistenciaActual.FechaTupla + "\nEmpleado " + asistenciaActual.idEmpleado, ex.Message);
            }
        }
        public List<AsistenciaSuinpac> obtenerAsistencias( string idEmpleado , string fecha)
        {
            try
            {
                List<AsistenciaSuinpac> listaAsistencia = new List<AsistenciaSuinpac>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM Asistencia WHERE idEmpleado = @idEmpleado AND Fecha = @FechaTupla";
                cmd.Parameters.AddWithValue("@idEmpleado",idEmpleado);
                cmd.Parameters.AddWithValue("@FechaTupla",fecha);
                MySqlDataReader reader = cmd.ExecuteReader();
                while ( reader.Read() )
                {
                    AsistenciaSuinpac asistenciaSuinpac = new AsistenciaSuinpac();
                    asistenciaSuinpac.id = reader.GetInt32(0);
                    asistenciaSuinpac.Fecha = reader.GetString(1);
                    asistenciaSuinpac.GrupoPersona = reader.GetString(2);
                    asistenciaSuinpac.idEmpleado = reader.GetString(3);
                    asistenciaSuinpac.MultipleHorario = reader.GetString(4);
                    asistenciaSuinpac.FechaTupla = reader.GetString(5);
                    listaAsistencia.Add(asistenciaSuinpac);
                }
                return listaAsistencia;

            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al obtener Asistencias del empleado: " + idEmpleado, ex.Message);
                return new List<AsistenciaSuinpac>();
            }
        }
        public List<DetalleAsistencias> obtenerDetalleAsistencia(int idAsistencia)
        {
            try
            {
                List<DetalleAsistencias> listaDetalles = new List<DetalleAsistencias> ();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM asistenciadetalle WHERE idAsistencia = @idAsistencia";
                cmd.Parameters.AddWithValue("@idAsistencia",idAsistencia);
                MySqlDataReader reader = cmd.ExecuteReader();
                if ( reader.Read() )
                {
                    DetalleAsistencias detalle = new DetalleAsistencias();
                    detalle.id = reader.GetInt32(0);
                    detalle.HoraEntrada = reader.GetString(1);
                    detalle.HoraSalida = reader.GetString(2);
                    detalle.EstatusAsistencia = reader.GetString(3);
                    detalle.FechaTupla = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    detalle.Tipo = reader.GetString(4);
                    detalle.idAsistencia = reader.GetString(5);
                    detalle.HoraAsistencia = reader.GetString(7);
                    listaDetalles.Add(detalle);
                }
                return listaDetalles;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error los detalles de la asistencia: idAsitencia " + idAsistencia, ex.Message);
                return new List<DetalleAsistencias>();
            }
        }
        public void insertarDetalleAsistencia( AsistenciaSuinpac asistencia , int idAsitencia )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "INSERT INTO asistenciadetalle (id,HoraEntrada,HoraSalida,EstatusAsistencia,Tipo,idAsistencia,FechaTupla,HoraAsistencia) VALUES (null,@HoraEntrada,@HoraSalida,@EstatusAsistencia,@Tipo,@idAsistencia,@FechaTupla,@HoraAsistencia)";
                cmd.Connection = abrirConexion();
                cmd.Parameters.AddWithValue("@HoraEntrada", asistencia.HoraEntrada);
                cmd.Parameters.AddWithValue("@HoraSalida", asistencia.HoraSalida);
                cmd.Parameters.AddWithValue("@EstatusAsistencia", asistencia.EstatusAsistencia);
                cmd.Parameters.AddWithValue("@Tipo", asistencia.Tipo);
                cmd.Parameters.AddWithValue("@idAsistencia", idAsitencia);
                cmd.Parameters.AddWithValue("@FechaTupla", asistencia.FechaTupla);
                cmd.Parameters.AddWithValue("@HoraAsistencia", Convert.ToDateTime(asistencia.FechaTupla).ToString("HH:mm:ss"));
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al insertar detalle de la asistencia:\nEmpleado" + asistencia.idEmpleado + "\nFecha y Hora: "+ asistencia.FechaTupla , ex.Message);
            }
        }
        public int buscarAsistencia( string empleado )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT id FROM asistencia WHERE idEmpleado = @idEmpleado";
                cmd.Parameters.AddWithValue("@idEmpleado", empleado);
                int idEmpleado = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return idEmpleado;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al buscar asistecia HEAD: " + empleado, ex.Message);
                return -1;
            }
        }
        public int buscarDetalle( string tipo, string horaEntrada, string horaSalida,string idEmpleado, string idAsistencia,string fecha)
        {
            try
            {
               //SELECT* FROM `asistenciadetalle` JOIN asistencia ON(asistencia.id = asistenciadetalle.idAsistencia) WHERE HoraEntrada = "" AND HoraSalida = "" AND Tipo = "" AND asistencia.idEmpleado = "64";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection=abrirConexion();
                cmd.CommandText = "SELECT asistenciadetalle.id FROM asistenciadetalle JOIN asistencia ON (asistencia.id = asistenciadetalle.idAsistencia) WHERE HoraEntrada = @HoraEntrada AND HoraSalida = @HoraSalida AND Tipo = @Tipo AND asistencia.idEmpleado = @idEmpleado AND asistencia.id = @id";
                cmd.Parameters.AddWithValue("@HoraEntrada",horaEntrada);
                cmd.Parameters.AddWithValue("@HoraSalida", horaSalida);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@id", idAsistencia);
                int idDEtalle = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                if(idDEtalle <= 0)
                {
                    cmd = new MySqlCommand();
                    cmd.Connection = abrirConexion();
                    cmd.CommandText = "SELECT id FROM asistencia_historial WHERE Entrada = @Entrada AND Salida = @Salida AND Tipo = @Tipo AND IdEmpleado = @IdEmpleado";
                    cmd.Parameters.AddWithValue("@Entrada",horaEntrada);
                    cmd.Parameters.AddWithValue("@Salida", horaSalida);
                    cmd.Parameters.AddWithValue("@Tipo", tipo);
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);                    
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                cmd.Connection.Close();
                return idDEtalle;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al buscar detalle: " + idEmpleado +"\n Asistencias: " + idAsistencia , ex.Message);
                return -1;
            }
        }
        public List<AsistenciaAPI> ObtenerAsistencias()
        {
            try
            {
                List<AsistenciaAPI> listaAsistencias = new List<AsistenciaAPI>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM asistencia";
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() )
                {
                    AsistenciaAPI asistenciaAPI = new AsistenciaAPI();
                    asistenciaAPI.id = Convert.ToInt32(reader["id"]);
                    asistenciaAPI.FechaTupla = Convert.ToString(reader["FechaTupla"]);
                    asistenciaAPI.Fecha = Convert.ToString(reader["Fecha"]);
                    asistenciaAPI.idEmpleado = Convert.ToInt32(reader["idEmpleado"]);
                    asistenciaAPI.MultipleHorario = Convert.ToInt32(reader["MultipleHorario"]);
                    asistenciaAPI.idGrupoPersona = Convert.ToInt32(reader["GrupoPersona"]);
                    listaAsistencias.Add(asistenciaAPI);
                }
                return listaAsistencias;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al eliminar horario", ex.Message);
                return null;
            }
        }
        public void guardarLog(string descripcion, string extra, string mensajeError)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "INSERT INTO log(id, Problema, DatosExtra, Fecha, ErrorC) VALUES (null,@Problema,@DatosExtra,@Fecha,@ErrorC)";
                cmd.Parameters.AddWithValue("@Problema",descripcion);
                cmd.Parameters.AddWithValue("@DatosExtra", extra);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
                cmd.Parameters.AddWithValue("@ErrorC", mensajeError);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine (err.Message);    
            }
        }
        public void EliminarAsistencia( string idAsistencia )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "DELETE FROM asistencia WHERE id = @idAsitencia";
                cmd.Parameters.AddWithValue("@idAsitencia", idAsistencia);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd = null;
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = abrirConexion();
                cmd2.CommandText = "DELETE FROM asistenciadetalle WHERE idAsistencia = @idAsitencia";
                cmd2.Parameters.AddWithValue("@idAsitencia", idAsistencia);
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                cmd2 = null;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                guardarLog("Error al eliminar la asistencia del empleado: " + idAsistencia,idAsistencia,err.Message);
            }
        }
        public void guardarHistoriaAsistencia( string fechaTupla,  string entrada, string salida, string tipo, string estatus, string idEmpleado)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "INSERT INTO asistencia_historial(id, FechaTupla, Entrada, Salida, Tipo, Estatus, IdEmpleado) VALUES (null,@FechaTupla,@Entrada,@Salida,@Tipo,@Estatus,@IdEmpleado)";
                cmd.Parameters.AddWithValue("@FechaTupla", fechaTupla);
                cmd.Parameters.AddWithValue("@Entrada",entrada);
                cmd.Parameters.AddWithValue("@Salida", salida);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cmd.Parameters.AddWithValue("@Estatus", estatus);
                cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch(Exception ex) {
                guardarLog("Error en la base de datos", "Error al insertar asistencia_historial", ex.Message);
            }
        }
        public int verificarHorarioEmpleado( string idEmpleado )
        {
            try
            {
                int numero = -1;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT COUNT(id) FROM horario WHERE idEmpleado = @idEmpleado";
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                numero = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return numero;

            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al verificar el empleado", ex.Message);
                return -1;
            }
        }
        public bool VerificarChecador(string dir)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT id FROM checadores WHERE Direccion = @direccion";
                cmd.Parameters.AddWithValue("@direccion", dir);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return id > 0;
            }
            catch(Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al verificar el checador", ex.Message);
                return true;
            }
        }
        public void AgregarHistorialEmpleado(Empleado empleado,string checador)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "INSERT INTO historialpersona(id,Nombre,Nfc_uid,Foto,idEmpleado,Checador) VALUES (null,@Nombre,@Nfc_uid,@Foto,@idEmpleado,@Checador)";
                cmd.Parameters.AddWithValue("@Nombre",empleado.Nombre);
                cmd.Parameters.AddWithValue("@Nfc_uid",empleado.Nfc_uid);
                cmd.Parameters.AddWithValue("@Foto",empleado.Foto);
                cmd.Parameters.AddWithValue("@idEmpleado", empleado.idEmpleado);
                cmd.Parameters.AddWithValue("@Checador", checador);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al agregar historialpersona", ex.Message);
            }
        }
        public bool VerificarinsersionEmpleado(string idEmpleado, string checador )
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT id FROM historialpersona WHERE idEmpleado = @idEmpleado AND Checador = @Checador";
                cmd.Parameters.AddWithValue("@idEmpleado",idEmpleado);
                cmd.Parameters.AddWithValue("@Checador",checador);
                int empleado = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                Console.WriteLine("SELECT id FROM historialpersona WHERE idEmpleado = "+idEmpleado + " AND Checador = " + checador);
                return empleado > 0;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al verificar el historial de la persona", ex.Message);
                return true;
            }
        }
        public string ObtenerNFCEmpleado(string idEmpleado)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT Nfc_uid FROM historialpersona WHERE idEmpleado = @idEmpleado";
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                string nfcuid = Convert.ToString(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return nfcuid;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al verificar el nfc de la persona", ex.Message);
                return "-1";
            }
        }
        public bool eliminarHistorialEmpleado( string idEmpleado, string idChecador)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "DELETE FROM historialpersona WHERE idEmpleado = @idEmpleado AND Checador = @Checador";
                cmd.Parameters.AddWithValue("@idEmpleado",idEmpleado);
                cmd.Parameters.AddWithValue("@Checador", idChecador);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                Console.WriteLine("DELETE FROM horario WHERE idEmpleado = "+ idEmpleado + " AND Checador = " + idChecador);
                return true;
            }
            catch (Exception ex)
            {
                guardarLog("Error en la base de datos", "Error al elimnar el historial del empleado: " + idEmpleado, ex.Message);
                return false;
            }
        }   
        public List<Empleado> ObtenerListaEmpleados( string checdorActivo)
        {
            List<Empleado> empleados = new List<Empleado>();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = abrirConexion();
                cmd.CommandText = "SELECT * FROM historialpersona WHERE Checador = @Activo";
                cmd.Parameters.AddWithValue("@Activo", checdorActivo);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Empleado emp = new Empleado();
                    emp.Nombre = Convert.ToString(reader["Nombre"]);
                    emp.Nfc_uid = Convert.ToString(reader["Nfc_uid"]);
                    emp.idEmpleado = Convert.ToString(reader["idEmpleado"]);
                    empleados.Add(emp);
                }
                //NOTE: cerramos la conexion
                cmd.Connection.Close();
                return empleados;
            }
            catch (Exception ex)
            {
                guardarLog("Error al obtener la lista de los empleados local", "ObtenerListaEmpleados", ex.Message);
                return empleados;

            }
        }
    }

}