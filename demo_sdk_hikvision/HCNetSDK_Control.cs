using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using demo_sdk_hikvision.Clases;

namespace demo_sdk_hikvision
{
    public class HCNetSDK_Control
    {
        public HCNetSDK_Control() { }

        //---------------------------------------------- VARIABLES 
        string id_tabla_dispositivos = "";
        string id_tabla_usuarios = "";
        string url_imagen = "";
        string url_huella = "";
        int tiempo_timer_tarjeta = 0;
        public Int32 m_lGetCardCfgHandle = -1;
        public Int32 m_lSetCardCfgHandle = -1;
        public Int32 m_lDelCardCfgHandle = -1;
        private int m_lGetFaceCfgHandle = -1;
        private int m_lSetFaceCfgHandle = -1;
        private int m_lCapFaceCfgHandle = -1;
        public int m_lGetFingerPrintCfgHandle = -1;
        public int m_lSetFingerPrintCfgHandle = -1;
        public int m_lDelFingerPrintCfHandle = -1;
        public int m_lCapFingerPrintCfHandle = -1;

        private HCNetSDK_Evento.MSGCallBack m_falarmData = null;

        //---------------------------------------------- MÉTODOS
        private void mostrar_dispositivos()
        {
           /* string mensaje = "";
            data_dispositivos.Rows.Clear();
            tabla_dispositivos = Consultas.Seleccionar_datatable("select * from dispositivos order by direccion_ip");
            for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
            {
                DataRow row = tabla_dispositivos.Rows[x];
                if (row["estatus"].ToString() == "True")
                {
                    mensaje = "En linea";
                }
                else
                {
                    mensaje = "Fuera de linea";
                }

                data_dispositivos.Rows.Add(row["id"], row["descripcion"], row["direccion_ip"], row["puerto"], row["usuario"], row["contrasena"], mensaje);
            }
            id_tabla_dispositivos = "";*/
        }

        private void mostrar_log()
        {
           /* data_log.Rows.Clear();
            tabla_log = Consultas.Seleccionar_datatable("select * from log order by id desc");
            for (int x = 0; x < tabla_log.Rows.Count; x++)
            {
                DataRow row = tabla_log.Rows[x];
                data_log.Rows.Add(row["fecha"], row["descripcion"]);
            }*/
        }

        private void mostrar_usuarios()
        {
            /*string mensaje = "";
            data_usuarios.Rows.Clear();
            tabla_usuarios = Consultas.Seleccionar_datatable("select * from usuario order by nombre");
            for (int x = 0; x < tabla_usuarios.Rows.Count; x++)
            {
                DataRow row = tabla_usuarios.Rows[x];
                data_usuarios.Rows.Add(row["id"], row["nombre"], row["no_usuario"], row["no_tarjeta"], row["permiso_tarjeta"], row["imagen"], mensaje);
            }
            id_tabla_usuarios = "";*/
        }

        public static bool ping(string _direccion_ip, int tiempo_espera)
        {
            Ping HacerPing = new Ping();
            int iTiempoEspera = tiempo_espera;
            PingReply RespuestaPing;
            string sDireccion = _direccion_ip;
            RespuestaPing = HacerPing.Send(sDireccion, iTiempoEspera);
            if (RespuestaPing.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void crear_usuario_sdk(string _numero_tarjeta, string _permisos_tarjeta, string _numero_usuario, string _nombre_usuario)
        {
            if (m_lSetCardCfgHandle != -1)
            {
                if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle))
                {
                    m_lSetCardCfgHandle = -1;
                }
            }

            HCNetSDK_Tarjeta.NET_DVR_CARD_COND struCond = new HCNetSDK_Tarjeta.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lSetCardCfgHandle = HCNetSDK_Tarjeta.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Tarjeta.NET_DVR_SET_CARD, ptrStruCond, (int)struCond.dwSize, null, IntPtr.Zero);
            if (m_lSetCardCfgHandle < 0)
            {
                Console.WriteLine("NET_DVR_SET_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);
                return;
            }
            else
            {
                enviar_datos_tarjeta(_numero_tarjeta, _permisos_tarjeta, _numero_usuario, _nombre_usuario);
                Marshal.FreeHGlobal(ptrStruCond);
            }
        }

        private void enviar_datos_tarjeta(string _numero_tarjeta, string _permisos_tarjeta, string _numero_usuario, string _nombre_usuario)
        {
            HCNetSDK_Tarjeta.NET_DVR_CARD_RECORD struData = new HCNetSDK_Tarjeta.NET_DVR_CARD_RECORD();
            struData.Init();
            struData.dwSize = (uint)Marshal.SizeOf(struData);
            struData.byCardType = 1;
            byte[] byTempCardNo = new byte[HCNetSDK_Tarjeta.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struData.byCardNo[i] = byTempCardNo[i];
            }
            ushort.TryParse(_permisos_tarjeta, out struData.wCardRightPlan[0]);
            uint.TryParse(_numero_usuario, out struData.dwEmployeeNo);
            byte[] byTempName = new byte[HCNetSDK_Tarjeta.NAME_LEN];
            byTempName = System.Text.Encoding.Default.GetBytes(_nombre_usuario);
            for (int i = 0; i < byTempName.Length; i++)
            {
                struData.byName[i] = byTempName[i];
            }
            //periodo de validez del usuario
            struData.struValid.byEnable = 1;
            struData.struValid.struBeginTime.wYear = 2000;
            struData.struValid.struBeginTime.byMonth = 1;
            struData.struValid.struBeginTime.byDay = 1;
            struData.struValid.struBeginTime.byHour = 11;
            struData.struValid.struBeginTime.byMinute = 11;
            struData.struValid.struBeginTime.bySecond = 11;
            struData.struValid.struEndTime.wYear = 2030;
            struData.struValid.struEndTime.byMonth = 1;
            struData.struValid.struEndTime.byDay = 1;
            struData.struValid.struEndTime.byHour = 11;
            struData.struValid.struEndTime.byMinute = 11;
            struData.struValid.struEndTime.bySecond = 11;
            //permiso en puerta
            struData.byDoorRight[0] = 1;
            struData.wCardRightPlan[0] = 1;
            IntPtr ptrStruData = Marshal.AllocHGlobal((int)struData.dwSize);
            Marshal.StructureToPtr(struData, ptrStruData, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS struStatus = new HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            int dwState = (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
            uint dwReturned = 0;
            while (true)
            {
                dwState = HCNetSDK_Tarjeta.NET_DVR_SendWithRecvRemoteConfig(m_lSetCardCfgHandle, ptrStruData, struData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                struStatus = (HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS)Marshal.PtrToStructure(ptrdwState, typeof(HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS));
                if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                {
                    Thread.Sleep(10);
                    continue;
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                {
                    Console.WriteLine("NET_DVR_SET_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                {
                    if (struStatus.dwErrorCode != 0)
                    {
                        Console.WriteLine("NET_DVR_SET_CARD success but errorCode:" + struStatus.dwErrorCode);
                    }
                    else
                    {
                        Console.WriteLine("NET_DVR_SET_CARD success");
                    }
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                {
                    Console.WriteLine("NET_DVR_SET_CARD finish");
                    break;
                }
                else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                {
                    Console.WriteLine("NET_DVR_SET_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError());
                    break;
                }
                else
                {
                    Console.WriteLine("unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError()); 
                    break;
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle);
            m_lSetCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruData);
            Marshal.FreeHGlobal(ptrdwState);
            return;
        }

        private void enviar_rostro(string _numero_lectora, string _numero_tarjeta)
        {
            if (url_imagen == "")
            {
                Console.WriteLine("Por favor seleccione la ruta del rostro.");
                return;
            }

            CHCNetSDK_Facial.NET_DVR_FACE_COND struCond = new CHCNetSDK_Facial.NET_DVR_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            struCond.dwFaceNum = 1;
            int.TryParse(_numero_lectora, out struCond.dwEnableReaderNo);
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTemp.Length; i++)
            {
                struCond.byCardNo[i] = byTemp[i];
            }

            int dwInBufferSize = Convert.ToInt32(struCond.dwSize);
            IntPtr ptrstruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrstruCond, false);
            m_lSetFaceCfgHandle = CHCNetSDK_Facial.NET_DVR_StartRemoteConfig(Variables.m_UserID, CHCNetSDK_Facial.NET_DVR_SET_FACE, ptrstruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lSetFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrstruCond);
                Console.WriteLine("NET_DVR_SET_FACE_FAIL, ERROR CODE" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString());
                return;
            }
            CHCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord = new CHCNetSDK_Facial.NET_DVR_FACE_RECORD();
            struRecord.init();
            struRecord.dwSize = Marshal.SizeOf(struRecord);

            byte[] byRecordNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byRecordNo.Length; i++)
            {
                struRecord.byCardNo[i] = byRecordNo[i];
            }

            leer_datos_rostro(ref struRecord);
            int dwInBuffSize = Marshal.SizeOf(struRecord);
            int dwStatus = 0;

            CHCNetSDK_Facial.NET_DVR_FACE_STATUS struStatus = new CHCNetSDK_Facial.NET_DVR_FACE_STATUS();
            struStatus.init();
            struStatus.dwSize = Marshal.SizeOf(struStatus);
            int dwOutBuffSize = Convert.ToInt32(struStatus.dwSize);
            IntPtr ptrOutDataLen = Marshal.AllocHGlobal(sizeof(int));
            bool Flag = true;

            while (Flag)
            {
                dwStatus = CHCNetSDK_Facial.NET_DVR_SendWithRecvRemoteConfig(m_lSetFaceCfgHandle, ref struRecord, dwInBuffSize, ref struStatus, dwOutBuffSize, ptrOutDataLen);
                switch (dwStatus)
                {
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        proceso_enviar_datos_rostro(ref struStatus, ref Flag);
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        Console.WriteLine("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        Console.WriteLine("NET_SDK_GET_NEXT_STATUS_UNKOWN" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString()); mostrar_log();
                        Flag = false;
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrstruCond);
            Marshal.FreeHGlobal(ptrOutDataLen);
        }

        private void proceso_enviar_datos_rostro(ref CHCNetSDK_Facial.NET_DVR_FACE_STATUS struStatus, ref bool flag)
        {
            switch (struStatus.byRecvStatus)
            {
                case 1:
                    Console.WriteLine("SetFaceDataSuccessful");
                    break;
                default:
                    flag = false;
                    Console.WriteLine("NET_SDK_SET_Face_DATA_FAILED" + struStatus.byRecvStatus.ToString());
                    break;
            }
        }

        private void leer_datos_rostro(ref CHCNetSDK_Facial.NET_DVR_FACE_RECORD struRecord)
        {
            if (!File.Exists(url_imagen))
            {
                Console.WriteLine("El Archivo no existe");
                return;
            }
            FileStream fs = new FileStream(url_imagen, FileMode.OpenOrCreate);
            if (0 == fs.Length)
            {
                Console.WriteLine("El archivo parece estar vacio " + (fs.Length) + "KB");
                return;
            }
            if (200 * 1024 < fs.Length)
            {
                Console.WriteLine("La imagen es demasiado pesada: " + ( fs.Length /1024 ) + " KB" );
                return;
            }
            try
            {
                int.TryParse(fs.Length.ToString(), out struRecord.dwFaceLen);
                int iLen = Convert.ToInt32(struRecord.dwFaceLen);
                byte[] by = new byte[iLen];
                struRecord.pFaceBuffer = Marshal.AllocHGlobal(iLen);
                fs.Read(by, 0, iLen);
                Marshal.Copy(by, 0, struRecord.pFaceBuffer, iLen);
                fs.Close();
            }
            catch( Exception error)
            {
                Console.WriteLine("Error al leer el archivo: " + error.Message);
                fs.Close();
                return;
            }
        }

        public string eliminar_usuario_dispositivo(string _numero_lectora, string _numero_tarjeta, IntPtr handle)
        {
            string salida = "";
            if (m_lDelCardCfgHandle != -1)
            {
                if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle))
                {
                    m_lDelCardCfgHandle = -1;
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_CARD_COND struCond = new HCNetSDK_Tarjeta.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_SEND_DATA struSendData = new HCNetSDK_Tarjeta.NET_DVR_CARD_SEND_DATA();
            struSendData.Init();
            struSendData.dwSize = (uint)Marshal.SizeOf(struSendData);
            byte[] byTempCardNo = new byte[HCNetSDK_Tarjeta.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(_numero_tarjeta);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struSendData.byCardNo[i] = byTempCardNo[i];
            }
            IntPtr ptrStruSendData = Marshal.AllocHGlobal((int)struSendData.dwSize);
            Marshal.StructureToPtr(struSendData, ptrStruSendData, false);

            HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS struStatus = new HCNetSDK_Tarjeta.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            m_lGetCardCfgHandle = HCNetSDK_Tarjeta.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Tarjeta.NET_DVR_DEL_CARD, ptrStruCond, (int)struCond.dwSize, null, handle);
            if (m_lGetCardCfgHandle < 0)
            {
                
                Marshal.FreeHGlobal(ptrStruCond);
                return "NET_DVR_DEL_CARD error:" + HCNetSDK_Tarjeta.NET_DVR_GetLastError();
            }
            else
            {
                int dwState = (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
                uint dwReturned = 0;
                while (true)
                {
                    dwState = HCNetSDK_Tarjeta.NET_DVR_SendWithRecvRemoteConfig(m_lGetCardCfgHandle, ptrStruSendData, struSendData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                    if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                    {
                        salida +=  "NET_DVR_DEL_CARD fail error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                    {
                        salida += "NET_DVR_DEL_CARD success:";
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                    {
                        salida += "NET_DVR_DEL_CARD finish";
                        break;
                    }
                    else if (dwState == (int)HCNetSDK_Tarjeta.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                    {
                        salida = "NET_DVR_DEL_CARD exception error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                        break;
                    }
                    else
                    {
                        salida = "unknown status error: " + HCNetSDK_Tarjeta.NET_DVR_GetLastError();
                        break;
                    }
                }
            }
            HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle);
            m_lDelCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruSendData);
            Marshal.FreeHGlobal(ptrdwState);
            return salida;
        }

        private void control_de_puerta(int _salida, int _estado)
        {
            /*Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (HCNetSDK_ControlPuerta.NET_DVR_ControlGateway(Variables.m_UserID, _salida, Convert.ToUInt32(_estado)))
                {
                    MessageBox.Show("NET_DVR_ControlGateway: open door succeed");
                }
                else
                {
                    MessageBox.Show("NET_DVR_ControlGateway: open door failed error:" + HCNetSDK_ControlPuerta.NET_DVR_GetLastError());
                }
                id_tabla_dispositivos = "";
            }
            else
            {
                MessageBox.Show("Seleccione un dispositivo.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Cursor.Current = Cursors.Default;
            */
        }

        private void proceso_capturar_datos_rostro(ref CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG struFaceCfg, ref bool flag)
        {
            /*if (0 == struFaceCfg.dwFacePicSize)
            {
                return;
            }

            DateTime dt = DateTime.Now;
            string nombre_ruta_archivo = Environment.CurrentDirectory + "\\imagenes\\" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";
            try
            {
                using (FileStream fs = new FileStream(nombre_ruta_archivo, FileMode.OpenOrCreate))
                {
                    int FaceLen = struFaceCfg.dwFacePicSize;
                    byte[] by = new byte[FaceLen];
                    Marshal.Copy(struFaceCfg.pFacePicBuffer, by, 0, FaceLen);
                    fs.Write(by, 0, FaceLen);
                    fs.Close();
                }

                picture_usuario_imagen.Image = Image.FromFile(nombre_ruta_archivo);
                url_imagen = nombre_ruta_archivo;
            }
            catch
            {
                flag = false;
                MessageBox.Show("Captura de datos incorrecto.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }*/
        }

        private void MsgCallback(int lCommand, ref HCNetSDK_Evento.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            switch (lCommand)
            {
                case HCNetSDK_Evento.COMM_ALARM_ACS:
                    obtener_numero_tajeta(pAlarmInfo);
                    break;
                default:
                    break;
            }
        }

        private void obtener_numero_tajeta(IntPtr pAlarmInfo)
        {
            HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO struAcsAlarmInfo = new HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO();
            struAcsAlarmInfo = (HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(HCNetSDK_Evento.NET_DVR_ACS_ALARM_INFO));
            HCNetSDK_Evento.NET_DVR_LOG_V30 struFileInfo = new HCNetSDK_Evento.NET_DVR_LOG_V30();
            struFileInfo.dwMajorType = struAcsAlarmInfo.dwMajor;
            struFileInfo.dwMinorType = struAcsAlarmInfo.dwMinor;
            char[] csTmp = new char[256];
            String szInfo = new String(csTmp).TrimEnd('\0');
            String szInfoBuf = null;
            if (struAcsAlarmInfo.struAcsEventInfo.byCardNo[0] != 0)
            {
                szInfoBuf = System.Text.Encoding.UTF8.GetString(struAcsAlarmInfo.struAcsEventInfo.byCardNo).TrimEnd('\0');
                Variables.no_tarjeta = szInfoBuf;
            }
        }

        private void capturar_imagen()
        {
            /*if (m_lCapFaceCfgHandle != -1)
            {
                CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                m_lCapFaceCfgHandle = -1;
            }
            if (picture_usuario_imagen.Image != null)
            {
                picture_usuario_imagen.Image.Dispose();
                picture_usuario_imagen.Image = null;
            }
            url_imagen = "";

            CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_COND struCond = new CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwInBufferSize = struCond.dwSize;
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);
            m_lCapFaceCfgHandle = CHCNetSDK_Facial.NET_DVR_StartRemoteConfig(Variables.m_UserID, CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_INFO, ptrStruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lCapFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_CAP_FACE_FAIL, ERROR CODE" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return;
            }

            CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG struFaceCfg = new CHCNetSDK_Facial.NET_DVR_CAPTURE_FACE_CFG();
            struFaceCfg.init();
            int dwStatus = 0;
            int dwOutBuffSize = Marshal.SizeOf(struFaceCfg);
            bool Flag = true;
            while (Flag)
            {
                dwStatus = CHCNetSDK_Facial.NET_DVR_GetNextRemoteConfig(m_lCapFaceCfgHandle, ref struFaceCfg, dwOutBuffSize);
                switch (dwStatus)
                {
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        proceso_capturar_datos_rostro(ref struFaceCfg, ref Flag);
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        break;
                    case CHCNetSDK_Facial.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_STATUS_UNKOWN" + CHCNetSDK_Facial.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        Flag = false;
                        CHCNetSDK_Facial.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStruCond);*/
        }

        private void enviar_huella(string _lectora, string _id_huella, string _no_tarjeta)
        {
            /*if (m_lSetFingerPrintCfgHandle != -1)
            {
                HCNetSDK_Huella.NET_DVR_StopRemoteConfig((int)m_lSetFingerPrintCfgHandle);
                m_lSetFingerPrintCfgHandle = -1;
            }

            HCNetSDK_Huella.NET_DVR_FINGERPRINT_COND strupond = new HCNetSDK_Huella.NET_DVR_FINGERPRINT_COND();
            strupond.init();
            int dwSize = Marshal.SizeOf(strupond);
            strupond.dwSize = dwSize;
            byte.TryParse(_id_huella, out strupond.byFingerPrintID);
            int.TryParse(_lectora, out strupond.dwEnableReaderNo);
            strupond.dwFingerprintNum = 1;
            byte[] byTempptrRec = System.Text.Encoding.UTF8.GetBytes(_no_tarjeta);
            for (int i = 0; i < byTempptrRec.Length; i++)
            {
                strupond.byCardNo[i] = byTempptrRec[i];
            }

            IntPtr ptrStrucond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(strupond, ptrStrucond, false);

            m_lSetFingerPrintCfgHandle = HCNetSDK_Huella.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Huella.NET_DVR_SET_FINGERPRINT, ptrStrucond, dwSize, null, IntPtr.Zero);
            if (-1 == m_lSetFingerPrintCfgHandle)
            {
                Marshal.FreeHGlobal(ptrStrucond);
                MessageBox.Show("NET_DVR_SET_FINGERPRINT_CFG_V50 FAIL, ERROR CODE" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                return;
            }

            Boolean Flag = true;
            int dwStatus = 0;
            HCNetSDK_Huella.NET_DVR_FINGERPRINT_RECORD StruRecord = new HCNetSDK_Huella.NET_DVR_FINGERPRINT_RECORD();
            StruRecord.init();
            int dwInBuffSize = Marshal.SizeOf(StruRecord);
            StruRecord.dwSize = dwInBuffSize;
            byte.TryParse(_id_huella, out StruRecord.byFingerPrintID);
            int.TryParse(_lectora, out StruRecord.dwEnableReaderNo);
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(_no_tarjeta);
            for (int i = 0; i < byTemp.Length; i++)
            {
                StruRecord.byCardNo[i] = byTemp[i];
            }
            leer_datos_huella(ref StruRecord);

            HCNetSDK_Huella.NET_DVR_FINGERPRINT_STATUS StruStatus = new HCNetSDK_Huella.NET_DVR_FINGERPRINT_STATUS();
            StruStatus.init();
            int dwOutBuffSize = Marshal.SizeOf(StruStatus);
            StruStatus.dwSize = dwOutBuffSize;
            IntPtr ptrOutDataLen = Marshal.AllocHGlobal(sizeof(int));


            while (Flag)
            {
                dwStatus = HCNetSDK_Huella.NET_DVR_SendWithRecvRemoteConfig(m_lSetFingerPrintCfgHandle, ref StruRecord, dwInBuffSize, ref StruStatus, dwOutBuffSize, ptrOutDataLen);
                switch (dwStatus)
                {
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        procesar_envio_datos_huella(ref StruStatus, ref Flag);
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                        Consultas.log("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString()); mostrar_log();
                        Flag = false;
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                        Flag = false;
                        break;
                    default:
                        Consultas.log("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString()); mostrar_log();
                        Flag = false;
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStrucond);
            Marshal.FreeHGlobal(ptrOutDataLen);*/
        }

        private void leer_datos_huella(ref HCNetSDK_Huella.NET_DVR_FINGERPRINT_RECORD Record)
        {
           /* try
            {
                using (FileStream fs = new FileStream(url_huella, FileMode.OpenOrCreate))
                {
                    if (0 == fs.Length)
                    {
                        Record.byFingerData[0] = 0;
                        fs.Close();
                    }
                    Record.dwFingerPrintLen = (int)fs.Length;
                    BinaryReader objBinaryReader = new BinaryReader(fs);
                    if (Record.dwFingerPrintLen > HCNetSDK_Huella.MAX_FINGER_PRINT_LEN)
                    {
                        MessageBox.Show("FingerPrintLen is too long");
                        return;
                    }
                    for (int i = 0; i < Record.dwFingerPrintLen; i++)
                    {
                        if (i >= fs.Length)
                        {
                            break;
                        }
                        Record.byFingerData[i] = objBinaryReader.ReadByte();
                    }
                    fs.Close();
                }
            }
            catch
            {
                if (m_lSetFingerPrintCfgHandle != -1)
                {
                    HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lSetFingerPrintCfgHandle);
                }
                MessageBox.Show("FingerDataPath may be wrong", "Error", MessageBoxButtons.OK);
            }*/
        }

        private void procesar_envio_datos_huella(ref HCNetSDK_Huella.NET_DVR_FINGERPRINT_STATUS ststus, ref bool flag)
        {
           /* switch (ststus.byRecvStatus)
            {
                case 0:
                    Consultas.log("SetFingegDataSuccessful"); mostrar_log();
                    break;
                default:
                    flag = false;
                    Consultas.log("NET_SDK_SET_FINGER_DATA_FAILED" + ststus.byRecvStatus.ToString()); mostrar_log();
                    break;
            }*/
        }

        private void capturar_huella()
        {
         /*   if (m_lCapFingerPrintCfHandle != -1)
            {
                HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                m_lCapFingerPrintCfHandle = -1;
            }

            HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_COND struCond = new HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwInBufferSize = struCond.dwSize;
            struCond.byFingerPrintPicType = 1;
            struCond.byFingerNo = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal(struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lCapFingerPrintCfHandle = HCNetSDK_Huella.NET_DVR_StartRemoteConfig(Variables.m_UserID, HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_INFO, ptrStruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lCapFingerPrintCfHandle)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_CAP_FINGERPRINT FAIL, ERROR CODE" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
            }

            bool flag = true;
            int dwStatus = 0;

            HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_CFG struCFG = new HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_CFG();
            struCFG.init();
            struCFG.dwSize = Marshal.SizeOf(struCFG);
            int dwOutBuffSize = struCFG.dwSize;
            while (flag)
            {
                dwStatus = HCNetSDK_Huella.NET_DVR_GetNextRemoteConfig(m_lCapFingerPrintCfHandle, ref struCFG, dwOutBuffSize);
                switch (dwStatus)
                {
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_SUCCESS:
                        procesar_datos_captura_huella(ref struCFG, ref flag);
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FAILED:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        flag = false;
                        break;
                    case HCNetSDK_Huella.NET_SDK_GET_NEXT_STATUS_FINISH:
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                        flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_UNKOWN" + HCNetSDK_Huella.NET_DVR_GetLastError().ToString(), "Error", MessageBoxButtons.OK);
                        flag = false;
                        HCNetSDK_Huella.NET_DVR_StopRemoteConfig(m_lCapFingerPrintCfHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStruCond);*/
        }

        private void procesar_datos_captura_huella(ref HCNetSDK_Huella.NET_DVR_CAPTURE_FINGERPRINT_CFG struCFG, ref bool flag)
        {
            /*DateTime dt = DateTime.Now;
            string nombre_archivo = Environment.CurrentDirectory + "\\huellas\\" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".dat";
            try
            {
                using (FileStream fs = new FileStream(nombre_archivo, FileMode.OpenOrCreate))
                {
                    fs.Write(struCFG.byFingerData, 0, struCFG.dwFingerPrintDataSize);
                    fs.Close();
                }
                url_huella = nombre_archivo;
                MessageBox.Show("Huella capturada con éxito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("CapFingerprint process failed");
                flag = false;
            }*/
        }

        //---------------------------------------------- EVENTOS

        private void btn_disp_eliminar_Click( )
        {
            /*Cursor.Current = Cursors.WaitCursor;
            DialogResult dialogResult = MessageBox.Show("¿Realmente desea eliminar el dispositivo?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (id_tabla_dispositivos != "")
                {
                    Consultas.Insertar_Eliminar_Modificar("delete from dispositivos where id=" + id_tabla_dispositivos);
                    MessageBox.Show("Dispositivo eliminado correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar_dispositivos();
                    mostrar_dispositivos();
                }
                else
                {
                    MessageBox.Show("Seleccione un dispositivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                limpiar_dispositivos();
            }
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_disp_impiar_Click( )
        {
            //limpiar_dispositivos();
        }
        private void btn_usuario_buscar_imagen_Click( )
        {
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory + "\\imagenes";
            openFileDialog.Filter = "Face file|*.jpg|All documents|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (picture_usuario_imagen.Image != null)
                {
                    picture_usuario_imagen.Image.Dispose();
                    picture_usuario_imagen.Image = null;
                }
                url_imagen = openFileDialog.FileName;
                picture_usuario_imagen.Image = Image.FromFile(url_imagen);
            }
            openFileDialog.Dispose();*/
        }

        private void btn_usuario_limpiar_Click( )
        {
            //limpiar_usuarios();
        }

        private void btn_usuario_eliminar_Click( )
        {
            /*Cursor.Current = Cursors.WaitCursor;
            DialogResult dialogResult = MessageBox.Show("¿Realmente desea eliminar el usuario?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (id_tabla_usuarios != "")
                {
                    for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
                    {
                        string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
                        int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
                        string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
                        string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

                        Consultas.log("Eliminando " + txt_usuario_nombre.Text + " de " + direccion_ip); mostrar_log();
                        string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                        if (conexion == "OK")
                        {
                            eliminar_usuario_dispositivo("1", txt_usuario_no_tarjeta.Text);
                            Consultas.log(txt_usuario_nombre.Text + " fué eliminado de " + direccion_ip); mostrar_log();
                        }
                        else
                        {
                            Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
                        }
                    }
                    Consultas.Insertar_Eliminar_Modificar("delete from usuario where id=" + id_tabla_usuarios);
                    MessageBox.Show("Usuario eliminado correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar_usuarios();
                    mostrar_usuarios();
                }
                else
                {
                    MessageBox.Show("Seleccione un usuario", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                limpiar_dispositivos();
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        public string agregarUsuarioSuinpac( 
            string txt_usuario_nombre, string txt_usuario_no_usuario, string txt_usuario_niv_acceso, string txt_usuario_no_tarjeta, string url,
            string direccion_ip, int puerto, string usuario, string contrasena, string nombreDispositivo)
        {
            if (txt_usuario_nombre != "" && txt_usuario_no_usuario != "" && txt_usuario_niv_acceso != "" && txt_usuario_no_tarjeta != "" && url!= "")
            {
                url_imagen = url;
                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    crear_usuario_sdk(txt_usuario_no_tarjeta, txt_usuario_niv_acceso, txt_usuario_no_usuario, txt_usuario_nombre);
                    enviar_rostro("1", txt_usuario_no_tarjeta);
                    
                    return txt_usuario_nombre + " Registrado";
                }
                else
                {
                    return "Dispisitivo " + nombreDispositivo + " fuera de linea.";
                }
            }
            else
            {
                return "Datos incompletos";
            }
            Variables.m_UserID = -1;
        }
        public string ActualizarRostroEmpleadoSuinpac(
            string txt_usuario_nombre, string txt_usuario_no_tarjeta, string url,
            string direccion_ip, int puerto, string usuario, string contrasena, string nombreDispositivo)
        {
            if ( txt_usuario_no_tarjeta != "" && url != "")
            {
                url_imagen = url;
                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    enviar_rostro("1", txt_usuario_no_tarjeta);
                    return txt_usuario_nombre + ": Rostro Actualizado";
                } else
                {
                    return "Dispisitivo " + nombreDispositivo + " fuera de linea.";
                }
            }
            else
            {
                return "Datos incompletos";
            }
        }

        private void data_dispositivos_CellClick()
        {
           /* try
            {
                id_tabla_dispositivos = data_dispositivos.CurrentRow.Cells["id"].Value.ToString();
                txt_disp_descripcion.Text = data_dispositivos.CurrentRow.Cells["descripcion"].Value.ToString();
                txt_disp_direcc_ip.Text = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                txt_disp_usuario.Text = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                txt_disp_contrasena.Text = data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString();
                txt_disp_puerto.Text = data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString();
                btn_disp_eliminar.Enabled = true;
            }
            catch { }*/
        }

        private void data_usuarios_CellClick( )
        {
            /*try
            {
                id_tabla_usuarios = data_usuarios.CurrentRow.Cells["id_usuario"].Value.ToString();
                txt_usuario_nombre.Text = data_usuarios.CurrentRow.Cells["nombre"].Value.ToString();
                txt_usuario_no_usuario.Text = data_usuarios.CurrentRow.Cells["no_usuario"].Value.ToString();
                txt_usuario_no_tarjeta.Text = data_usuarios.CurrentRow.Cells["no_tarjeta"].Value.ToString();
                txt_usuario_niv_acceso.Text = data_usuarios.CurrentRow.Cells["permiso_tarjeta"].Value.ToString();
                url_imagen = data_usuarios.CurrentRow.Cells["imagen"].Value.ToString();
                picture_usuario_imagen.Image = Image.FromFile(url_imagen);
                btn_usuario_eliminar.Enabled = true;
                btn_usuario_actualizar.Enabled = true;
                btn_usuario_agregar.Enabled = false;
                txt_usuario_no_usuario.Enabled = false;
            }
            catch { }*/
        }

        private void btn_disp_abrir_Click( )
        {
            control_de_puerta(1, 1); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            //limpiar_dispositivos();
        }

        private void btn_disp_cerrar_Click( )
        {
            control_de_puerta(1, 0); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            //limpiar_dispositivos();
        }

        private void btn_disp_mant_abierto_Click( )
        {
            control_de_puerta(1, 2); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            //limpiar_dispositivos();
        }

        private void btn_disp_mant_cerrado_Click( )
        {
            control_de_puerta(1, 3); // _estado: 0-cerrdo, 1-abierto, 2-mantener abierto, 3-mantener cerrado
            Variables.m_UserID = -1;
            //limpiar_dispositivos();
        }

        private void btn_usuario_tomar_foto_Click( )
        {
           /* Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    capturar_imagen();
                }
                else
                {
                    MessageBox.Show("El dispositivo " + direccion_ip + " está fuera de linea", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_usuario_sincronizar_Click( )
        {
           /* Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                Consultas.log("Inicia sincronización con " + direccion_ip); mostrar_log();

                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    for (int x = 0; x < tabla_usuarios.Rows.Count; x++)
                    {
                        string no_tarjeta = data_usuarios.Rows[x].Cells["no_tarjeta"].Value.ToString();
                        string permiso_tarjeta = data_usuarios.Rows[x].Cells["permiso_tarjeta"].Value.ToString();
                        string no_usuario = data_usuarios.Rows[x].Cells["no_usuario"].Value.ToString();
                        string nombre = data_usuarios.Rows[x].Cells["nombre"].Value.ToString();
                        url_imagen = data_usuarios.Rows[x].Cells["imagen"].Value.ToString();

                        crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                        enviar_rostro("1", no_tarjeta);
                        Consultas.log("Termina sincronización con " + direccion_ip); mostrar_log();
                    }
                }
                else
                {
                    Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
                }

                MessageBox.Show("Usuarios sincronizados a dispositivo.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_usuario_sinc_1_Click( )
        {
           /* Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_usuarios != "")
            {
                if (id_tabla_dispositivos != "")
                {
                    string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                    int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                    string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                    string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                    Consultas.log("Inicia sincronización con " + direccion_ip); mostrar_log();

                    string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                    if (conexion == "OK")
                    {
                        string no_tarjeta = data_usuarios.CurrentRow.Cells["no_tarjeta"].Value.ToString();
                        string permiso_tarjeta = data_usuarios.CurrentRow.Cells["permiso_tarjeta"].Value.ToString();
                        string no_usuario = data_usuarios.CurrentRow.Cells["no_usuario"].Value.ToString();
                        string nombre = data_usuarios.CurrentRow.Cells["nombre"].Value.ToString();
                        url_imagen = data_usuarios.CurrentRow.Cells["imagen"].Value.ToString();

                        crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                        enviar_rostro("1", no_tarjeta);
                        Consultas.log("Termina sincronización con " + direccion_ip); mostrar_log();
                        id_tabla_dispositivos = "";
                        id_tabla_usuarios = "";
                    }
                    else
                    {
                        Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
                    }

                    MessageBox.Show("Usuarios sincronizados a dispositivo.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Seleccione un dispositivo.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un usuario de la lista de usuarios, posteriormente seleccione un dispositivo de la lista de dispositivos.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_usuario_leer_tarjeta_Click( )
        {
           /* Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    HCNetSDK_Evento.NET_DVR_SETUPALARM_PARAM struSetupAlarmParam = new HCNetSDK_Evento.NET_DVR_SETUPALARM_PARAM();
                    struSetupAlarmParam.dwSize = (uint)Marshal.SizeOf(struSetupAlarmParam);
                    struSetupAlarmParam.byLevel = 1;
                    struSetupAlarmParam.byAlarmInfoType = 1;
                    struSetupAlarmParam.byDeployType = (byte)1;

                    if (HCNetSDK_Evento.NET_DVR_SetupAlarmChan_V41(Variables.m_UserID, ref struSetupAlarmParam) < 0)
                    {
                        MessageBox.Show("NET_DVR_SetupAlarmChan_V41 fail error: " + HCNetSDK_Evento.NET_DVR_GetLastError(), "Setup alarm chan failed");
                        return;
                    }

                    m_falarmData = new HCNetSDK_Evento.MSGCallBack(MsgCallback);
                    if (!HCNetSDK_Evento.NET_DVR_SetDVRMessageCallBack_V50(0, m_falarmData, IntPtr.Zero))
                    {
                        MessageBox.Show("NET_DVR_SetDVRMessageCallBack_V50 fail", "operation fail", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    Consultas.log("Dispositivo " + direccion_ip + " fuera de linea."); mostrar_log();
                }
                timer_no_tarjeta.Enabled = true;
                groupBox_usuarios.Enabled = false;
            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void timer_no_tarjeta_Tick( )
        {
           /* txt_usuario_no_tarjeta.Text = Variables.no_tarjeta;
            if (Variables.no_tarjeta != "Pase la tarjeta...")
            {
                timer_no_tarjeta.Enabled = false;
                groupBox_usuarios.Enabled = true;
                Variables.no_tarjeta = "Pase la tarjeta...";
                limpiar_dispositivos();
            }
            tiempo_timer_tarjeta++;
            if (tiempo_timer_tarjeta > 20)
            {
                timer_no_tarjeta.Enabled = false;
                groupBox_usuarios.Enabled = true;
                Variables.no_tarjeta = "Pase la tarjeta...";
                txt_usuario_no_tarjeta.Text = "";
                tiempo_timer_tarjeta = 0;
                limpiar_dispositivos();
                MessageBox.Show("Tiempo de espera agotado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void btn_usuario_actualizar_Click( )
        {
            //Cursor.Current = Cursors.WaitCursor;
            //if (id_tabla_usuarios != "")
            //{
            //    for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
            //    {
            //        string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
            //        int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
            //        string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
            //        string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

            //        string no_tarjeta_anterior = Consultas.Seleccionar_String("select no_tarjeta from usuario where id=" + id_tabla_usuarios, "no_tarjeta");

            //        string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
            //        if (conexion == "OK")
            //        {
            //            //eliminar_usuario_dispositivo("1", no_tarjeta_anterior);
            //            crear_usuario_sdk(txt_usuario_no_tarjeta.Text, txt_usuario_niv_acceso.Text, txt_usuario_no_usuario.Text, txt_usuario_nombre.Text);
            //            enviar_rostro("1", txt_usuario_no_tarjeta.Text);

            //            eliminar_usuario_dispositivo("1", no_tarjeta_anterior);
            //        }
            //        else
            //        {
            //            Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
            //        }
            //    }
            //    string consulta = "update usuario set nombre='" + txt_usuario_nombre.Text + "', no_tarjeta='" + txt_usuario_no_tarjeta.Text + "', permiso_tarjeta=" + Convert.ToInt32(txt_usuario_niv_acceso.Text) + ", imagen='" + url_imagen + "' where id=" + id_tabla_usuarios;
            //    Consultas.Insertar_Eliminar_Modificar(consulta);
            //    limpiar_usuarios();
            //    mostrar_usuarios();
            //}
            //else
            //{
            //    MessageBox.Show("Seleccione un usuario", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //Variables.m_UserID = -1;
            //Cursor.Current = Cursors.Default;
        }

        private void btn_usu_sinc_usuarios_disp_Click( )
        {
            /*Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_usuarios != "")
            {
                for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
                {
                    string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
                    int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
                    string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
                    string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

                    Consultas.log("Inicia " + direccion_ip); mostrar_log();

                    string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                    if (conexion == "OK")
                    {
                        string nombre = data_usuarios.CurrentRow.Cells["nombre"].Value.ToString();
                        string no_usuario = data_usuarios.CurrentRow.Cells["no_usuario"].Value.ToString();
                        string no_tarjeta = data_usuarios.CurrentRow.Cells["no_tarjeta"].Value.ToString();
                        string nivel_acceso = data_usuarios.CurrentRow.Cells["permiso_tarjeta"].Value.ToString();
                        url_imagen = data_usuarios.CurrentRow.Cells["imagen"].Value.ToString();

                        crear_usuario_sdk(no_tarjeta, nivel_acceso, no_usuario, nombre);
                        enviar_rostro("1", no_tarjeta);
                        Consultas.log("Termina " + direccion_ip); mostrar_log();
                    }
                    else
                    {
                        Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
                    }
                }

                Consultas.log("Se registró el usuario " + nombre); mostrar_log();
                limpiar_usuarios();
                mostrar_usuarios();
                MessageBox.Show("Usuario sincronizado correctamente.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Complete todos los campos.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_usu_sinc_todos_a_todos_Click( )
        {
           /* Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_usuarios != "")
            {
                for (int x = 0; x < tabla_dispositivos.Rows.Count; x++)
                {
                    string direccion_ip = data_dispositivos.Rows[x].Cells["direccion_ip"].Value.ToString();
                    int puerto = Convert.ToInt32(data_dispositivos.Rows[x].Cells["puerto"].Value.ToString());
                    string usuario = data_dispositivos.Rows[x].Cells["usuario"].Value.ToString();
                    string contrasena = MD5Crypto.Desencriptar(data_dispositivos.Rows[x].Cells["contrasena"].Value.ToString());

                    Consultas.log("Inicia " + direccion_ip); mostrar_log();

                    string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                    if (conexion == "OK")
                    {
                        for (int y = 0; y < tabla_usuarios.Rows.Count; y++)
                        {
                            string no_tarjeta = data_usuarios.Rows[y].Cells["no_tarjeta"].Value.ToString();
                            string permiso_tarjeta = data_usuarios.Rows[y].Cells["permiso_tarjeta"].Value.ToString();
                            string no_usuario = data_usuarios.Rows[y].Cells["no_usuario"].Value.ToString();
                            string nombre = data_usuarios.Rows[y].Cells["nombre"].Value.ToString();
                            url_imagen = data_usuarios.Rows[y].Cells["imagen"].Value.ToString();

                            crear_usuario_sdk(no_tarjeta, permiso_tarjeta, no_usuario, nombre);
                            enviar_rostro("1", no_tarjeta);
                        }
                        Consultas.log("Termina " + direccion_ip); mostrar_log();
                    }
                    else
                    {
                        Consultas.log("Dispisitivo " + direccion_ip + " fuera de linea."); mostrar_log();
                    }
                }

                Consultas.log("Se registró el usuario " + nombre); mostrar_log();
                limpiar_usuarios();
                mostrar_usuarios();
                MessageBox.Show("Usuario sincronizado correctamente.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Complete todos los campos.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_usuario_buscar_huella_Click( )
        {
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory + "\\huellas";
            openFileDialog.Filter = "Fingerprint file|*.dat|All documents|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                url_huella = openFileDialog.FileName;
            }*/
        }

        private void btn_usuario_tomar_huella_Click( )
        {
            /*Cursor.Current = Cursors.WaitCursor;
            if (id_tabla_dispositivos != "")
            {
                string direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString();
                int puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString());
                string usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                string contrasena = MD5Crypto.Desencriptar(data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString());

                string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                if (conexion == "OK")
                {
                    capturar_huella();
                }
                else
                {
                    MessageBox.Show("El dispositivo " + direccion_ip + " está fuera de linea", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un dispisitivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Variables.m_UserID = -1;
            Cursor.Current = Cursors.Default;*/
        }

        private void btn_eventos_mostrar_Click( )
        {
            /*if (id_tabla_dispositivos != "")
            {
                Variables.evento_fecha_inicio = txt_evento_fecha_inicio.Text;
                Variables.evento_fecha_fin = txt_evento_fecha_fin.Text;
                Variables.evento_tipo_principal = txt_evento_tipo_principal.SelectedItem.ToString();
                Variables.evento_tipo_secundario = txt_evento_tipo_secundario.SelectedItem.ToString();
                Variables.direccion_ip = data_dispositivos.CurrentRow.Cells["direccion_ip"].Value.ToString(); ;
                Variables.puerto = Convert.ToInt32(data_dispositivos.CurrentRow.Cells["puerto"].Value.ToString()); ;
                Variables.usuario = data_dispositivos.CurrentRow.Cells["usuario"].Value.ToString();
                Variables.contrasena = data_dispositivos.CurrentRow.Cells["contrasena"].Value.ToString();

                eventos abrir = new eventos();
                abrir.Show();
            }
            else
            {
                MessageBox.Show("Seleccione un dispositivo", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }*/
        }
        public void crearEmpleadosPorLotes( List<Empleado> empleados, string direccion_ip, int puerto, string usuario, string contrasena ) 
        {
            
            if ( direccion_ip != "" && puerto > 0 && usuario != "" && contrasena != "" )
            {
                //Iniciamos la conexion al disposivo
                //NOTE: Verificamos el contexto de la conexion con el dispositivo
                //Recorremos los empleados
                foreach (Empleado empleado in empleados)
                {
                    string conexion = Dispositivo.conectar(direccion_ip, puerto, usuario, contrasena);
                    if (m_lSetCardCfgHandle != -1)
                    {
                        //NOTE: se libera en caso de que se encuentre un contexto
                        if (HCNetSDK_Tarjeta.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle))
                        {
                            m_lSetCardCfgHandle = -1;
                        }
                    }
                    if (conexion == "OK")
                    {
                        url_imagen = empleado.Foto;
                        crear_usuario_sdk(empleado.Nfc_uid, "1", empleado.idEmpleado, empleado.Nombre);
                        Console.WriteLine(empleado.Foto);
                        enviar_rostro("1", empleado.Nfc_uid);
                        Console.WriteLine("Termina " + direccion_ip);
                        Thread.Sleep(500);
                    }
                }
            }
            else
            {
                Console.WriteLine("Informacion del dispositivo no valida o incompleta");
            }
        }
    }
}
