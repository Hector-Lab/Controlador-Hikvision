namespace demo_sdk_hikvision
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_disp_mant_cerrado = new System.Windows.Forms.Button();
            this.btn_disp_abrir = new System.Windows.Forms.Button();
            this.btn_disp_mant_abierto = new System.Windows.Forms.Button();
            this.btn_disp_cerrar = new System.Windows.Forms.Button();
            this.btn_disp_impiar = new System.Windows.Forms.Button();
            this.data_dispositivos = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.direccion_ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puerto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contrasena = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_disp_agregar = new System.Windows.Forms.Button();
            this.btn_disp_eliminar = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_disp_contrasena = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_disp_usuario = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_disp_puerto = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_disp_direcc_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_disp_descripcion = new System.Windows.Forms.TextBox();
            this.groupBox_usuarios = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_evento_tipo_secundario = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_evento_fecha_fin = new System.Windows.Forms.DateTimePicker();
            this.txt_evento_fecha_inicio = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_evento_tipo_principal = new System.Windows.Forms.ComboBox();
            this.btn_eventos_mostrar = new System.Windows.Forms.Button();
            this.btn_usuario_tomar_huella = new System.Windows.Forms.Button();
            this.btn_usuario_buscar_huella = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_usu_sinc_todos_a_todos = new System.Windows.Forms.Button();
            this.btn_usuario_sincronizar = new System.Windows.Forms.Button();
            this.btn_usu_sinc_usuarios_disp = new System.Windows.Forms.Button();
            this.btn_usuario_sinc_1 = new System.Windows.Forms.Button();
            this.btn_usuario_actualizar = new System.Windows.Forms.Button();
            this.btn_usuario_leer_tarjeta = new System.Windows.Forms.Button();
            this.group_log = new System.Windows.Forms.GroupBox();
            this.data_log = new System.Windows.Forms.DataGridView();
            this.log_fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.log_descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_usuario_tomar_foto = new System.Windows.Forms.Button();
            this.btn_usuario_limpiar = new System.Windows.Forms.Button();
            this.btn_usuario_buscar_imagen = new System.Windows.Forms.Button();
            this.picture_usuario_imagen = new System.Windows.Forms.PictureBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txt_usuario_no_tarjeta = new System.Windows.Forms.TextBox();
            this.data_usuarios = new System.Windows.Forms.DataGridView();
            this.id_usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_tarjeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.permiso_tarjeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imagen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_usuario_agregar = new System.Windows.Forms.Button();
            this.btn_usuario_eliminar = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.txt_usuario_niv_acceso = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txt_usuario_no_usuario = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txt_usuario_nombre = new System.Windows.Forms.TextBox();
            this.timer_no_tarjeta = new System.Windows.Forms.Timer(this.components);
            this.timer_barra = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_dispositivos)).BeginInit();
            this.groupBox_usuarios.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.group_log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_log)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_usuario_imagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data_usuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btn_disp_impiar);
            this.groupBox1.Controls.Add(this.data_dispositivos);
            this.groupBox1.Controls.Add(this.btn_disp_agregar);
            this.groupBox1.Controls.Add(this.btn_disp_eliminar);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txt_disp_contrasena);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txt_disp_usuario);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txt_disp_puerto);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txt_disp_direcc_ip);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_disp_descripcion);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 720);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dispositivos";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_disp_mant_cerrado);
            this.groupBox3.Controls.Add(this.btn_disp_abrir);
            this.groupBox3.Controls.Add(this.btn_disp_mant_abierto);
            this.groupBox3.Controls.Add(this.btn_disp_cerrar);
            this.groupBox3.Location = new System.Drawing.Point(12, 612);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(326, 96);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Control de puerta";
            // 
            // btn_disp_mant_cerrado
            // 
            this.btn_disp_mant_cerrado.Location = new System.Drawing.Point(168, 55);
            this.btn_disp_mant_cerrado.Name = "btn_disp_mant_cerrado";
            this.btn_disp_mant_cerrado.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_mant_cerrado.TabIndex = 66;
            this.btn_disp_mant_cerrado.Text = "Mantener cerrado";
            this.btn_disp_mant_cerrado.UseVisualStyleBackColor = true;
            this.btn_disp_mant_cerrado.Click += new System.EventHandler(this.btn_disp_mant_cerrado_Click);
            // 
            // btn_disp_abrir
            // 
            this.btn_disp_abrir.Location = new System.Drawing.Point(12, 21);
            this.btn_disp_abrir.Name = "btn_disp_abrir";
            this.btn_disp_abrir.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_abrir.TabIndex = 15;
            this.btn_disp_abrir.Text = "Abrir\r\n";
            this.btn_disp_abrir.UseVisualStyleBackColor = true;
            this.btn_disp_abrir.Click += new System.EventHandler(this.btn_disp_abrir_Click);
            // 
            // btn_disp_mant_abierto
            // 
            this.btn_disp_mant_abierto.Location = new System.Drawing.Point(12, 55);
            this.btn_disp_mant_abierto.Name = "btn_disp_mant_abierto";
            this.btn_disp_mant_abierto.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_mant_abierto.TabIndex = 65;
            this.btn_disp_mant_abierto.Text = "Mantener abierto";
            this.btn_disp_mant_abierto.UseVisualStyleBackColor = true;
            this.btn_disp_mant_abierto.Click += new System.EventHandler(this.btn_disp_mant_abierto_Click);
            // 
            // btn_disp_cerrar
            // 
            this.btn_disp_cerrar.Location = new System.Drawing.Point(168, 21);
            this.btn_disp_cerrar.Name = "btn_disp_cerrar";
            this.btn_disp_cerrar.Size = new System.Drawing.Size(146, 28);
            this.btn_disp_cerrar.TabIndex = 14;
            this.btn_disp_cerrar.Text = "Cerrar\r\n";
            this.btn_disp_cerrar.UseVisualStyleBackColor = true;
            this.btn_disp_cerrar.Click += new System.EventHandler(this.btn_disp_cerrar_Click);
            // 
            // btn_disp_impiar
            // 
            this.btn_disp_impiar.Location = new System.Drawing.Point(124, 148);
            this.btn_disp_impiar.Name = "btn_disp_impiar";
            this.btn_disp_impiar.Size = new System.Drawing.Size(101, 28);
            this.btn_disp_impiar.TabIndex = 13;
            this.btn_disp_impiar.Text = "Limpiar";
            this.btn_disp_impiar.UseVisualStyleBackColor = true;
            this.btn_disp_impiar.Click += new System.EventHandler(this.btn_disp_impiar_Click);
            // 
            // data_dispositivos
            // 
            this.data_dispositivos.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.data_dispositivos.AllowUserToAddRows = false;
            this.data_dispositivos.AllowUserToDeleteRows = false;
            this.data_dispositivos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.data_dispositivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_dispositivos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.descripcion,
            this.direccion_ip,
            this.puerto,
            this.usuario,
            this.contrasena,
            this.estatus});
            this.data_dispositivos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.data_dispositivos.Location = new System.Drawing.Point(12, 186);
            this.data_dispositivos.Name = "data_dispositivos";
            this.data_dispositivos.ReadOnly = true;
            this.data_dispositivos.RowHeadersVisible = false;
            this.data_dispositivos.RowHeadersWidth = 51;
            this.data_dispositivos.Size = new System.Drawing.Size(326, 421);
            this.data_dispositivos.TabIndex = 12;
            this.data_dispositivos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_dispositivos_CellClick);
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 125;
            // 
            // descripcion
            // 
            this.descripcion.HeaderText = "Descipción";
            this.descripcion.MinimumWidth = 6;
            this.descripcion.Name = "descripcion";
            this.descripcion.ReadOnly = true;
            this.descripcion.Width = 250;
            // 
            // direccion_ip
            // 
            this.direccion_ip.HeaderText = "Dirección IP";
            this.direccion_ip.MinimumWidth = 6;
            this.direccion_ip.Name = "direccion_ip";
            this.direccion_ip.ReadOnly = true;
            this.direccion_ip.Width = 125;
            // 
            // puerto
            // 
            this.puerto.HeaderText = "Puerto";
            this.puerto.MinimumWidth = 6;
            this.puerto.Name = "puerto";
            this.puerto.ReadOnly = true;
            this.puerto.Width = 125;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "Usuario";
            this.usuario.MinimumWidth = 6;
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.Width = 125;
            // 
            // contrasena
            // 
            this.contrasena.HeaderText = "Contraseña";
            this.contrasena.MinimumWidth = 6;
            this.contrasena.Name = "contrasena";
            this.contrasena.ReadOnly = true;
            this.contrasena.Visible = false;
            this.contrasena.Width = 125;
            // 
            // estatus
            // 
            this.estatus.HeaderText = "Estatus";
            this.estatus.MinimumWidth = 6;
            this.estatus.Name = "estatus";
            this.estatus.ReadOnly = true;
            this.estatus.Width = 125;
            // 
            // btn_disp_agregar
            // 
            this.btn_disp_agregar.Location = new System.Drawing.Point(231, 148);
            this.btn_disp_agregar.Name = "btn_disp_agregar";
            this.btn_disp_agregar.Size = new System.Drawing.Size(107, 28);
            this.btn_disp_agregar.TabIndex = 11;
            this.btn_disp_agregar.Text = "Agregar";
            this.btn_disp_agregar.UseVisualStyleBackColor = true;
            this.btn_disp_agregar.Click += new System.EventHandler(this.btn_disp_agregar_Click);
            // 
            // btn_disp_eliminar
            // 
            this.btn_disp_eliminar.Enabled = false;
            this.btn_disp_eliminar.Location = new System.Drawing.Point(12, 148);
            this.btn_disp_eliminar.Name = "btn_disp_eliminar";
            this.btn_disp_eliminar.Size = new System.Drawing.Size(106, 28);
            this.btn_disp_eliminar.TabIndex = 10;
            this.btn_disp_eliminar.Text = "Eliminar";
            this.btn_disp_eliminar.UseVisualStyleBackColor = true;
            this.btn_disp_eliminar.Click += new System.EventHandler(this.btn_disp_eliminar_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(140, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 19);
            this.label16.TabIndex = 9;
            this.label16.Text = "Contraseña";
            // 
            // txt_disp_contrasena
            // 
            this.txt_disp_contrasena.Location = new System.Drawing.Point(143, 120);
            this.txt_disp_contrasena.MaxLength = 16;
            this.txt_disp_contrasena.Name = "txt_disp_contrasena";
            this.txt_disp_contrasena.PasswordChar = '■';
            this.txt_disp_contrasena.Size = new System.Drawing.Size(195, 26);
            this.txt_disp_contrasena.TabIndex = 8;
            this.txt_disp_contrasena.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_disp_contrasena.UseSystemPasswordChar = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 103);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 19);
            this.label15.TabIndex = 7;
            this.label15.Text = "Usuario";
            // 
            // txt_disp_usuario
            // 
            this.txt_disp_usuario.Location = new System.Drawing.Point(12, 120);
            this.txt_disp_usuario.MaxLength = 16;
            this.txt_disp_usuario.Name = "txt_disp_usuario";
            this.txt_disp_usuario.Size = new System.Drawing.Size(125, 26);
            this.txt_disp_usuario.TabIndex = 6;
            this.txt_disp_usuario.Text = "admin";
            this.txt_disp_usuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(253, 62);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 19);
            this.label14.TabIndex = 5;
            this.label14.Text = "Puerto";
            // 
            // txt_disp_puerto
            // 
            this.txt_disp_puerto.Location = new System.Drawing.Point(256, 78);
            this.txt_disp_puerto.MaxLength = 16;
            this.txt_disp_puerto.Name = "txt_disp_puerto";
            this.txt_disp_puerto.Size = new System.Drawing.Size(82, 26);
            this.txt_disp_puerto.TabIndex = 4;
            this.txt_disp_puerto.Text = "8000";
            this.txt_disp_puerto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 19);
            this.label13.TabIndex = 3;
            this.label13.Text = "Dirección IP";
            // 
            // txt_disp_direcc_ip
            // 
            this.txt_disp_direcc_ip.Location = new System.Drawing.Point(12, 78);
            this.txt_disp_direcc_ip.MaxLength = 16;
            this.txt_disp_direcc_ip.Name = "txt_disp_direcc_ip";
            this.txt_disp_direcc_ip.Size = new System.Drawing.Size(238, 26);
            this.txt_disp_direcc_ip.TabIndex = 2;
            this.txt_disp_direcc_ip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Descripción";
            // 
            // txt_disp_descripcion
            // 
            this.txt_disp_descripcion.Location = new System.Drawing.Point(12, 35);
            this.txt_disp_descripcion.Name = "txt_disp_descripcion";
            this.txt_disp_descripcion.Size = new System.Drawing.Size(326, 26);
            this.txt_disp_descripcion.TabIndex = 0;
            // 
            // groupBox_usuarios
            // 
            this.groupBox_usuarios.Controls.Add(this.groupBox4);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_tomar_huella);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_buscar_huella);
            this.groupBox_usuarios.Controls.Add(this.groupBox2);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_actualizar);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_leer_tarjeta);
            this.groupBox_usuarios.Controls.Add(this.group_log);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_tomar_foto);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_limpiar);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_buscar_imagen);
            this.groupBox_usuarios.Controls.Add(this.picture_usuario_imagen);
            this.groupBox_usuarios.Controls.Add(this.label22);
            this.groupBox_usuarios.Controls.Add(this.txt_usuario_no_tarjeta);
            this.groupBox_usuarios.Controls.Add(this.data_usuarios);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_agregar);
            this.groupBox_usuarios.Controls.Add(this.btn_usuario_eliminar);
            this.groupBox_usuarios.Controls.Add(this.label19);
            this.groupBox_usuarios.Controls.Add(this.txt_usuario_niv_acceso);
            this.groupBox_usuarios.Controls.Add(this.label20);
            this.groupBox_usuarios.Controls.Add(this.txt_usuario_no_usuario);
            this.groupBox_usuarios.Controls.Add(this.label21);
            this.groupBox_usuarios.Controls.Add(this.txt_usuario_nombre);
            this.groupBox_usuarios.Location = new System.Drawing.Point(370, 12);
            this.groupBox_usuarios.Name = "groupBox_usuarios";
            this.groupBox_usuarios.Size = new System.Drawing.Size(946, 720);
            this.groupBox_usuarios.TabIndex = 64;
            this.groupBox_usuarios.TabStop = false;
            this.groupBox_usuarios.Text = "Usuarios";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_evento_tipo_secundario);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txt_evento_fecha_fin);
            this.groupBox4.Controls.Add(this.txt_evento_fecha_inicio);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.txt_evento_tipo_principal);
            this.groupBox4.Controls.Add(this.btn_eventos_mostrar);
            this.groupBox4.Location = new System.Drawing.Point(477, 575);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(459, 133);
            this.groupBox4.TabIndex = 76;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Eventos";
            // 
            // txt_evento_tipo_secundario
            // 
            this.txt_evento_tipo_secundario.FormattingEnabled = true;
            this.txt_evento_tipo_secundario.Items.AddRange(new object[] {
            "All",
            "4G_MOUDLE_OFFLINE",
            "4G_MOUDLE_ONLINE",
            "AC_OFF",
            "AC_RESUME",
            "ALARMIN_ARM",
            "ALARMIN_BROKEN_CIRCUIT",
            "ALARMIN_DISARM",
            "ALARMIN_EXCEPTION",
            "ALARMIN_RESUME",
            "ALARMIN_SHORT_CIRCUIT",
            "ALARMOUT_OFF",
            "ALARMOUT_ON",
            "ALWAYS_CLOSE_BEGIN",
            "ALWAYS_CLOSE_END",
            "ALWAYS_OPEN_BEGIN",
            "ALWAYS_OPEN_END",
            "ANTI_SNEAK_FAIL",
            "AUTH_PLAN_DORMANT_FAIL",
            "AUTO_COMPLEMENT_NUMBER",
            "AUTO_RENUMBER",
            "BATTERY_ELECTRIC_LOW",
            "BATTERY_ELECTRIC_RESUME",
            "BATTERY_RESUME",
            "CAMERA_NOT_CONNECT",
            "CAMERA_RESUME",
            "CAN_BUS_EXCEPTION",
            "CAN_BUS_RESUME",
            "CARD_AND_PSW_FAIL",
            "CARD_AND_PSW_OVER_TIME",
            "CARD_AND_PSW_PASS",
            "CARD_AND_PSW_TIMEOUT",
            "CARD_ENCRYPT_VERIFY_FAIL",
            "CARD_FINGERPRINT_PASSWD_VERIFY_FAIL",
            "CARD_FINGERPRINT_PASSWD_VERIFY_PASS",
            "CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT",
            "CARD_FINGERPRINT_VERIFY_FAIL",
            "CARD_FINGERPRINT_VERIFY_PASS",
            "CARD_FINGERPRINT_VERIFY_TIMEOUT",
            "CARD_INVALID_PERIOD",
            "CARD_MAX_AUTHENTICATE_FAIL",
            "CARD_NO_RIGHT",
            "CARD_OUT_OF_DATE",
            "CARD_PLATFORM_VERIFY",
            "CARD_READER_DESMANTLE_ALARM",
            "CARD_READER_DESMANTLE_RESUME",
            "CARD_READER_OFFLINE",
            "CARD_READER_RESUME",
            "CARD_RIGHT_INPUT",
            "CARD_RIGHT_OUTTPUT",
            "CASE_SENSOR_ALARM",
            "CASE_SENSOR_RESUME",
            "CHANNEL_CONTROLLER_DESMANTLE_ALARM",
            "CHANNEL_CONTROLLER_DESMANTLE_RESUME",
            "CHANNEL_CONTROLLER_FIRE_IMPORT_ALARM",
            "CHANNEL_CONTROLLER_FIRE_IMPORT_RESUME",
            "CHANNEL_CONTROLLER_OFF",
            "CHANNEL_CONTROLLER_RESUME",
            "CLIMBING_OVER_GATE",
            "COM_NOT_CONNECT",
            "COM_RESUME",
            "COMBINED_VERIFY_PASS",
            "COMBINED_VERIFY_TIMEOUT",
            "DEV_POWER_OFF",
            "DEV_POWER_ON",
            "DEVICE_NOT_AUTHORIZE",
            "DISTRACT_CONTROLLER_ALARM",
            "DISTRACT_CONTROLLER_OFFLINE",
            "DISTRACT_CONTROLLER_ONLINE",
            "DISTRACT_CONTROLLER_RESUME",
            "DOOR_BUTTON_PRESS",
            "DOOR_BUTTON_RELEASE",
            "DOOR_CLOSE_NORMAL",
            "DOOR_OPEN_ABNORMAL",
            "DOOR_OPEN_NORMAL",
            "DOOR_OPEN_OR_DORMANT_FAIL",
            "DOOR_OPEN_OR_DORMANT_LINKAGE_OPEN_FAIL",
            "DOOR_OPEN_OR_DORMANT_OPEN_FAIL",
            "DOOR_OPEN_TIMEOUT",
            "DOORBELL_RINGING",
            "DROP_ARM_BLOCK",
            "DROP_ARM_BLOCK_RESUME",
            "EMERGENCY_BUTTON_RESUME",
            "EMERGENCY_BUTTON_TRIGGER",
            "EMPLOYEE_NO_NOT_EXIST",
            "FACE_IMAGE_QUALITY_LOW",
            "FINGE_RPRINT_QUALITY_LOW",
            "FINGER_PRINT_MODULE_NOT_CONNECT",
            "FINGER_PRINT_MODULE_RESUME",
            "FINGERPRINT_COMPARE_FAIL",
            "FINGERPRINT_COMPARE_PASS",
            "FINGERPRINT_INEXISTENCE",
            "FINGERPRINT_PASSWD_VERIFY_FAIL",
            "FINGERPRINT_PASSWD_VERIFY_PASS",
            "FINGERPRINT_PASSWD_VERIFY_TIMEOUT",
            "FIRE_BUTTON_RESUME",
            "FIRE_BUTTON_TRIGGER",
            "FIRE_IMPORT_BROKEN_CIRCUIT",
            "FIRE_IMPORT_RESUME",
            "FLASH_ABNORMAL",
            "FORCE_ACCESS",
            "FREE_GATE_PASS_NOT_AUTH",
            "GATE_TEMPERATURE_OVERRUN",
            "HOST_DESMANTLE_ALARM",
            "HOST_DESMANTLE_RESUME",
            "ID_CARD_READER_NOT_CONNECT",
            "ID_CARD_READER_RESUME",
            "ILLEGAL_MESSAGE",
            "INDICATOR_LIGHT_OFF",
            "INDICATOR_LIGHT_RESUME",
            "INTERLOCK_DOOR_NOT_CLOSE",
            "INTRUSION_ALARM",
            "INVALID_CARD",
            "INVALID_MULTI_VERIFY_PERIOD",
            "IR_ADAPTOR_COMM_EXCEPTION",
            "IR_ADAPTOR_COMM_RESUME",
            "IR_EMITTER_EXCEPTION",
            "IR_EMITTER_RESUME",
            "LAMP_BOARD_COMM_EXCEPTION",
            "LAMP_BOARD_COMM_RESUME",
            "LEADER_CARD_OPEN_BEGIN",
            "LEADER_CARD_OPEN_END",
            "LEGAL_CARD_PASS",
            "LEGAL_EVENT_NEARLY_FULL",
            "LEGAL_MESSAGE",
            "LINKAGE_CAPTURE_PIC",
            "LOCAL_CONTROL_NET_BROKEN",
            "LOCAL_CONTROL_NET_RSUME",
            "LOCAL_CONTROL_OFFLINE",
            "LOCAL_CONTROL_RESUME",
            "LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN",
            "LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME",
            "LOCAL_FACE_MODELING_FAIL",
            "LOCAL_LOGIN_LOCK",
            "LOCAL_LOGIN_UNLOCK",
            "LOCAL_RESTORE_CFG",
            "LOCAL_UPGRADE",
            "LOCAL_USB_UPGRADE",
            "LOCK_CLOSE",
            "LOCK_OPEN",
            "LOW_BATTERY",
            "MAC_DETECT",
            "MAINTENANCE_BUTTON_RESUME",
            "MAINTENANCE_BUTTON_TRIGGER",
            "MASTER_RS485_LOOPNODE_BROKEN",
            "MASTER_RS485_LOOPNODE_RESUME",
            "MINOR_REMOTE_ARM",
            "MOD_GPRS_REPORT_PARAM",
            "MOD_NET_REPORT_CFG",
            "MOD_REPORT_GROUP_PARAM",
            "MOTOR_SENSOR_EXCEPTION",
            "MULTI_VERIFY_NEED_REMOTE_OPEN",
            "MULTI_VERIFY_REMOTE_RIGHT_FAIL",
            "MULTI_VERIFY_REPEAT_VERIFY",
            "MULTI_VERIFY_SUCCESS",
            "MULTI_VERIFY_SUPER_RIGHT_FAIL",
            "MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS",
            "MULTI_VERIFY_TIMEOUT",
            "NET_BROKEN",
            "NET_RESUME",
            "NORMAL_CFGFILE_INPUT",
            "NORMAL_CFGFILE_OUTTPUT",
            "NOT_BELONG_MULTI_GROUP",
            "NTP_CHECK_TIME",
            "OFFLINE_ECENT_NEARLY_FULL",
            "PASSING_TIMEOUT",
            "PASSWORD_MISMATCH",
            "PEOPLE_AND_ID_CARD_DEVICE_OFFLINE",
            "PEOPLE_AND_ID_CARD_DEVICE_ONLINE",
            "POS_END_ALARM",
            "POS_START_ALARM",
            "PRINTER_OFFLINE",
            "PRINTER_ONLINE",
            "PRINTER_OUT_OF_PAPER",
            "REMOTE_ACTUAL_GUARD",
            "REMOTE_ACTUAL_UNGUARD",
            "REMOTE_ALARMOUT_CLOSE_MAN",
            "REMOTE_ALARMOUT_OPEN_MAN",
            "REMOTE_ALWAYS_CLOSE",
            "REMOTE_ALWAYS_OPEN",
            "REMOTE_CAPTURE_PIC",
            "REMOTE_CFGFILE_INTPUT",
            "REMOTE_CFGFILE_OUTPUT",
            "REMOTE_CHECK_TIME",
            "REMOTE_CLEAR_CARD",
            "REMOTE_CLOSE_DOOR",
            "REMOTE_CONTROL_ALWAYS_OPEN_DOOR",
            "REMOTE_CONTROL_CLOSE_DOOR",
            "REMOTE_CONTROL_NOT_CODE_OPER_FAILED",
            "REMOTE_CONTROL_OPEN_DOOR",
            "REMOTE_DISARM",
            "REMOTE_HOUSEHOLD_CALL_LADDER",
            "REMOTE_LOGIN",
            "REMOTE_LOGOUT",
            "REMOTE_OPEN_DOOR",
            "REMOTE_REBOOT",
            "REMOTE_RESTORE_CFG",
            "REMOTE_UPGRADE",
            "REMOTE_VISITOR_CALL_LADDER",
            "REVERSE_ACCESS",
            "RS485_DEVICE_ABNORMAL",
            "RS485_DEVICE_REVERT",
            "SD_CARD_FULL",
            "SECURITY_MODULE_DESMANTLE_ALARM",
            "SECURITY_MODULE_DESMANTLE_RESUME",
            "SECURITY_MODULE_OFF",
            "SECURITY_MODULE_RESUME",
            "STAY_EVENT",
            "STRESS_ALARM",
            "SUBMARINEBACK_COMM_BREAK",
            "SUBMARINEBACK_COMM_RESUME",
            "SUBMARINEBACK_REPLY_FAIL",
            "TRAILING",
            "UNLOCK_PASSWORD_OPEN_DOOR",
            "VERIFY_MODE_MISMATCH",
            "WATCH_DOG_RESET",
            "FACE_VERIFY_PASS"});
            this.txt_evento_tipo_secundario.Location = new System.Drawing.Point(126, 96);
            this.txt_evento_tipo_secundario.Name = "txt_evento_tipo_secundario";
            this.txt_evento_tipo_secundario.Size = new System.Drawing.Size(169, 26);
            this.txt_evento_tipo_secundario.TabIndex = 72;
            this.txt_evento_tipo_secundario.Text = "All";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 19);
            this.label5.TabIndex = 71;
            this.label5.Text = "Tipo (secundario)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 19);
            this.label4.TabIndex = 70;
            this.label4.Text = "Tipo (principal)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 19);
            this.label3.TabIndex = 69;
            this.label3.Text = "Hasta";
            // 
            // txt_evento_fecha_fin
            // 
            this.txt_evento_fecha_fin.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.txt_evento_fecha_fin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_evento_fecha_fin.Location = new System.Drawing.Point(292, 22);
            this.txt_evento_fecha_fin.Name = "txt_evento_fecha_fin";
            this.txt_evento_fecha_fin.Size = new System.Drawing.Size(155, 26);
            this.txt_evento_fecha_fin.TabIndex = 68;
            // 
            // txt_evento_fecha_inicio
            // 
            this.txt_evento_fecha_inicio.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.txt_evento_fecha_inicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_evento_fecha_inicio.Location = new System.Drawing.Point(61, 22);
            this.txt_evento_fecha_inicio.Name = "txt_evento_fecha_inicio";
            this.txt_evento_fecha_inicio.Size = new System.Drawing.Size(138, 26);
            this.txt_evento_fecha_inicio.TabIndex = 67;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 19);
            this.label2.TabIndex = 61;
            this.label2.Text = "Desde";
            // 
            // txt_evento_tipo_principal
            // 
            this.txt_evento_tipo_principal.FormattingEnabled = true;
            this.txt_evento_tipo_principal.Items.AddRange(new object[] {
            "All",
            "Alarm",
            "Exception",
            "Operation",
            "Event"});
            this.txt_evento_tipo_principal.Location = new System.Drawing.Point(126, 63);
            this.txt_evento_tipo_principal.Name = "txt_evento_tipo_principal";
            this.txt_evento_tipo_principal.Size = new System.Drawing.Size(169, 26);
            this.txt_evento_tipo_principal.TabIndex = 60;
            this.txt_evento_tipo_principal.Text = "All";
            // 
            // btn_eventos_mostrar
            // 
            this.btn_eventos_mostrar.Location = new System.Drawing.Point(310, 60);
            this.btn_eventos_mostrar.Name = "btn_eventos_mostrar";
            this.btn_eventos_mostrar.Size = new System.Drawing.Size(137, 60);
            this.btn_eventos_mostrar.TabIndex = 15;
            this.btn_eventos_mostrar.Text = "Mostrar";
            this.btn_eventos_mostrar.UseVisualStyleBackColor = true;
            this.btn_eventos_mostrar.Click += new System.EventHandler(this.btn_eventos_mostrar_Click);
            // 
            // btn_usuario_tomar_huella
            // 
            this.btn_usuario_tomar_huella.Location = new System.Drawing.Point(297, 138);
            this.btn_usuario_tomar_huella.Name = "btn_usuario_tomar_huella";
            this.btn_usuario_tomar_huella.Size = new System.Drawing.Size(94, 28);
            this.btn_usuario_tomar_huella.TabIndex = 75;
            this.btn_usuario_tomar_huella.Text = "Tomar huella";
            this.btn_usuario_tomar_huella.UseVisualStyleBackColor = true;
            this.btn_usuario_tomar_huella.Click += new System.EventHandler(this.btn_usuario_tomar_huella_Click);
            // 
            // btn_usuario_buscar_huella
            // 
            this.btn_usuario_buscar_huella.Location = new System.Drawing.Point(149, 138);
            this.btn_usuario_buscar_huella.Name = "btn_usuario_buscar_huella";
            this.btn_usuario_buscar_huella.Size = new System.Drawing.Size(142, 28);
            this.btn_usuario_buscar_huella.TabIndex = 74;
            this.btn_usuario_buscar_huella.Text = "Seleccionar huella";
            this.btn_usuario_buscar_huella.UseVisualStyleBackColor = true;
            this.btn_usuario_buscar_huella.Click += new System.EventHandler(this.btn_usuario_buscar_huella_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_usu_sinc_todos_a_todos);
            this.groupBox2.Controls.Add(this.btn_usuario_sincronizar);
            this.groupBox2.Controls.Add(this.btn_usu_sinc_usuarios_disp);
            this.groupBox2.Controls.Add(this.btn_usuario_sinc_1);
            this.groupBox2.Location = new System.Drawing.Point(13, 613);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 95);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sincronizar";
            // 
            // btn_usu_sinc_todos_a_todos
            // 
            this.btn_usu_sinc_todos_a_todos.Location = new System.Drawing.Point(228, 56);
            this.btn_usu_sinc_todos_a_todos.Name = "btn_usu_sinc_todos_a_todos";
            this.btn_usu_sinc_todos_a_todos.Size = new System.Drawing.Size(212, 28);
            this.btn_usu_sinc_todos_a_todos.TabIndex = 73;
            this.btn_usu_sinc_todos_a_todos.Text = "Todos los usu. a todos los disp.";
            this.btn_usu_sinc_todos_a_todos.UseVisualStyleBackColor = true;
            this.btn_usu_sinc_todos_a_todos.Click += new System.EventHandler(this.btn_usu_sinc_todos_a_todos_Click);
            // 
            // btn_usuario_sincronizar
            // 
            this.btn_usuario_sincronizar.Location = new System.Drawing.Point(10, 21);
            this.btn_usuario_sincronizar.Name = "btn_usuario_sincronizar";
            this.btn_usuario_sincronizar.Size = new System.Drawing.Size(212, 28);
            this.btn_usuario_sincronizar.TabIndex = 67;
            this.btn_usuario_sincronizar.Text = "Todos los usuarios a dispositivo";
            this.btn_usuario_sincronizar.UseVisualStyleBackColor = true;
            this.btn_usuario_sincronizar.Click += new System.EventHandler(this.btn_usuario_sincronizar_Click);
            // 
            // btn_usu_sinc_usuarios_disp
            // 
            this.btn_usu_sinc_usuarios_disp.Location = new System.Drawing.Point(228, 21);
            this.btn_usu_sinc_usuarios_disp.Name = "btn_usu_sinc_usuarios_disp";
            this.btn_usu_sinc_usuarios_disp.Size = new System.Drawing.Size(212, 28);
            this.btn_usu_sinc_usuarios_disp.TabIndex = 72;
            this.btn_usu_sinc_usuarios_disp.Text = "Un usuario a todos los disp.";
            this.btn_usu_sinc_usuarios_disp.UseVisualStyleBackColor = true;
            this.btn_usu_sinc_usuarios_disp.Click += new System.EventHandler(this.btn_usu_sinc_usuarios_disp_Click);
            // 
            // btn_usuario_sinc_1
            // 
            this.btn_usuario_sinc_1.Location = new System.Drawing.Point(10, 56);
            this.btn_usuario_sinc_1.Name = "btn_usuario_sinc_1";
            this.btn_usuario_sinc_1.Size = new System.Drawing.Size(212, 28);
            this.btn_usuario_sinc_1.TabIndex = 68;
            this.btn_usuario_sinc_1.Text = "Un usuario a un dispositivo";
            this.btn_usuario_sinc_1.UseVisualStyleBackColor = true;
            this.btn_usuario_sinc_1.Click += new System.EventHandler(this.btn_usuario_sinc_1_Click);
            // 
            // btn_usuario_actualizar
            // 
            this.btn_usuario_actualizar.Enabled = false;
            this.btn_usuario_actualizar.Location = new System.Drawing.Point(235, 172);
            this.btn_usuario_actualizar.Name = "btn_usuario_actualizar";
            this.btn_usuario_actualizar.Size = new System.Drawing.Size(84, 28);
            this.btn_usuario_actualizar.TabIndex = 71;
            this.btn_usuario_actualizar.Text = "Actualizar";
            this.btn_usuario_actualizar.UseVisualStyleBackColor = true;
            this.btn_usuario_actualizar.Visible = false;
            this.btn_usuario_actualizar.Click += new System.EventHandler(this.btn_usuario_actualizar_Click);
            // 
            // btn_usuario_leer_tarjeta
            // 
            this.btn_usuario_leer_tarjeta.Location = new System.Drawing.Point(322, 75);
            this.btn_usuario_leer_tarjeta.Name = "btn_usuario_leer_tarjeta";
            this.btn_usuario_leer_tarjeta.Size = new System.Drawing.Size(28, 22);
            this.btn_usuario_leer_tarjeta.TabIndex = 70;
            this.btn_usuario_leer_tarjeta.UseVisualStyleBackColor = true;
            this.btn_usuario_leer_tarjeta.Click += new System.EventHandler(this.btn_usuario_leer_tarjeta_Click);
            // 
            // group_log
            // 
            this.group_log.Controls.Add(this.data_log);
            this.group_log.Location = new System.Drawing.Point(477, 19);
            this.group_log.Name = "group_log";
            this.group_log.Size = new System.Drawing.Size(459, 550);
            this.group_log.TabIndex = 64;
            this.group_log.TabStop = false;
            this.group_log.Text = "Log";
            // 
            // data_log
            // 
            this.data_log.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.data_log.AllowUserToAddRows = false;
            this.data_log.AllowUserToDeleteRows = false;
            this.data_log.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.data_log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.log_fecha,
            this.log_descripcion});
            this.data_log.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.data_log.Location = new System.Drawing.Point(13, 23);
            this.data_log.Name = "data_log";
            this.data_log.ReadOnly = true;
            this.data_log.RowHeadersVisible = false;
            this.data_log.RowHeadersWidth = 51;
            this.data_log.Size = new System.Drawing.Size(434, 507);
            this.data_log.TabIndex = 13;
            // 
            // log_fecha
            // 
            this.log_fecha.HeaderText = "Fecha";
            this.log_fecha.MinimumWidth = 6;
            this.log_fecha.Name = "log_fecha";
            this.log_fecha.ReadOnly = true;
            this.log_fecha.Width = 125;
            // 
            // log_descripcion
            // 
            this.log_descripcion.HeaderText = "Descripción";
            this.log_descripcion.MinimumWidth = 6;
            this.log_descripcion.Name = "log_descripcion";
            this.log_descripcion.ReadOnly = true;
            this.log_descripcion.Width = 450;
            // 
            // btn_usuario_tomar_foto
            // 
            this.btn_usuario_tomar_foto.Location = new System.Drawing.Point(297, 104);
            this.btn_usuario_tomar_foto.Name = "btn_usuario_tomar_foto";
            this.btn_usuario_tomar_foto.Size = new System.Drawing.Size(94, 28);
            this.btn_usuario_tomar_foto.TabIndex = 63;
            this.btn_usuario_tomar_foto.Text = "Tomar Foto";
            this.btn_usuario_tomar_foto.UseVisualStyleBackColor = true;
            this.btn_usuario_tomar_foto.Click += new System.EventHandler(this.btn_usuario_tomar_foto_Click);
            // 
            // btn_usuario_limpiar
            // 
            this.btn_usuario_limpiar.Location = new System.Drawing.Point(397, 104);
            this.btn_usuario_limpiar.Name = "btn_usuario_limpiar";
            this.btn_usuario_limpiar.Size = new System.Drawing.Size(64, 62);
            this.btn_usuario_limpiar.TabIndex = 6;
            this.btn_usuario_limpiar.Text = "Limpiar";
            this.btn_usuario_limpiar.UseVisualStyleBackColor = true;
            this.btn_usuario_limpiar.Click += new System.EventHandler(this.btn_usuario_limpiar_Click);
            // 
            // btn_usuario_buscar_imagen
            // 
            this.btn_usuario_buscar_imagen.Location = new System.Drawing.Point(149, 104);
            this.btn_usuario_buscar_imagen.Name = "btn_usuario_buscar_imagen";
            this.btn_usuario_buscar_imagen.Size = new System.Drawing.Size(142, 28);
            this.btn_usuario_buscar_imagen.TabIndex = 4;
            this.btn_usuario_buscar_imagen.Text = "Seleccionar Imagen";
            this.btn_usuario_buscar_imagen.UseVisualStyleBackColor = true;
            this.btn_usuario_buscar_imagen.Click += new System.EventHandler(this.btn_usuario_buscar_imagen_Click);
            // 
            // picture_usuario_imagen
            // 
            this.picture_usuario_imagen.BackColor = System.Drawing.Color.White;
            this.picture_usuario_imagen.Location = new System.Drawing.Point(13, 20);
            this.picture_usuario_imagen.Name = "picture_usuario_imagen";
            this.picture_usuario_imagen.Size = new System.Drawing.Size(131, 179);
            this.picture_usuario_imagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture_usuario_imagen.TabIndex = 60;
            this.picture_usuario_imagen.TabStop = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(146, 59);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(86, 19);
            this.label22.TabIndex = 14;
            this.label22.Text = "No. Tarjeta";
            // 
            // txt_usuario_no_tarjeta
            // 
            this.txt_usuario_no_tarjeta.Location = new System.Drawing.Point(149, 75);
            this.txt_usuario_no_tarjeta.MaxLength = 1024;
            this.txt_usuario_no_tarjeta.Name = "txt_usuario_no_tarjeta";
            this.txt_usuario_no_tarjeta.Size = new System.Drawing.Size(172, 26);
            this.txt_usuario_no_tarjeta.TabIndex = 2;
            this.txt_usuario_no_tarjeta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // data_usuarios
            // 
            this.data_usuarios.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.data_usuarios.AllowUserToAddRows = false;
            this.data_usuarios.AllowUserToDeleteRows = false;
            this.data_usuarios.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.data_usuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_usuarios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_usuario,
            this.nombre,
            this.no_usuario,
            this.no_tarjeta,
            this.permiso_tarjeta,
            this.imagen});
            this.data_usuarios.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.data_usuarios.Location = new System.Drawing.Point(13, 206);
            this.data_usuarios.Name = "data_usuarios";
            this.data_usuarios.ReadOnly = true;
            this.data_usuarios.RowHeadersVisible = false;
            this.data_usuarios.RowHeadersWidth = 51;
            this.data_usuarios.Size = new System.Drawing.Size(446, 401);
            this.data_usuarios.TabIndex = 12;
            this.data_usuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_usuarios_CellClick);
            // 
            // id_usuario
            // 
            this.id_usuario.HeaderText = "ID";
            this.id_usuario.MinimumWidth = 6;
            this.id_usuario.Name = "id_usuario";
            this.id_usuario.ReadOnly = true;
            this.id_usuario.Visible = false;
            this.id_usuario.Width = 150;
            // 
            // nombre
            // 
            this.nombre.HeaderText = "Nombre";
            this.nombre.MinimumWidth = 6;
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            this.nombre.Width = 155;
            // 
            // no_usuario
            // 
            this.no_usuario.HeaderText = "No. de Usuario";
            this.no_usuario.MinimumWidth = 6;
            this.no_usuario.Name = "no_usuario";
            this.no_usuario.ReadOnly = true;
            this.no_usuario.Width = 125;
            // 
            // no_tarjeta
            // 
            this.no_tarjeta.HeaderText = "No. Tarjeta";
            this.no_tarjeta.MinimumWidth = 6;
            this.no_tarjeta.Name = "no_tarjeta";
            this.no_tarjeta.ReadOnly = true;
            this.no_tarjeta.Width = 125;
            // 
            // permiso_tarjeta
            // 
            this.permiso_tarjeta.HeaderText = "Nivel de Acceso";
            this.permiso_tarjeta.MinimumWidth = 6;
            this.permiso_tarjeta.Name = "permiso_tarjeta";
            this.permiso_tarjeta.ReadOnly = true;
            this.permiso_tarjeta.Width = 70;
            // 
            // imagen
            // 
            this.imagen.HeaderText = "Imagen";
            this.imagen.MinimumWidth = 6;
            this.imagen.Name = "imagen";
            this.imagen.ReadOnly = true;
            this.imagen.Visible = false;
            this.imagen.Width = 125;
            // 
            // btn_usuario_agregar
            // 
            this.btn_usuario_agregar.Location = new System.Drawing.Point(325, 172);
            this.btn_usuario_agregar.Name = "btn_usuario_agregar";
            this.btn_usuario_agregar.Size = new System.Drawing.Size(136, 28);
            this.btn_usuario_agregar.TabIndex = 5;
            this.btn_usuario_agregar.Text = "Agregar";
            this.btn_usuario_agregar.UseVisualStyleBackColor = true;
            this.btn_usuario_agregar.Click += new System.EventHandler(this.btn_usuario_agregar_Click);
            // 
            // btn_usuario_eliminar
            // 
            this.btn_usuario_eliminar.Enabled = false;
            this.btn_usuario_eliminar.Location = new System.Drawing.Point(149, 172);
            this.btn_usuario_eliminar.Name = "btn_usuario_eliminar";
            this.btn_usuario_eliminar.Size = new System.Drawing.Size(80, 28);
            this.btn_usuario_eliminar.TabIndex = 7;
            this.btn_usuario_eliminar.Text = "Eliminar";
            this.btn_usuario_eliminar.UseVisualStyleBackColor = true;
            this.btn_usuario_eliminar.Click += new System.EventHandler(this.btn_usuario_eliminar_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(361, 59);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(125, 19);
            this.label19.TabIndex = 5;
            this.label19.Text = "Nivel de Acceso";
            // 
            // txt_usuario_niv_acceso
            // 
            this.txt_usuario_niv_acceso.Enabled = false;
            this.txt_usuario_niv_acceso.Location = new System.Drawing.Point(364, 75);
            this.txt_usuario_niv_acceso.MaxLength = 16;
            this.txt_usuario_niv_acceso.Name = "txt_usuario_niv_acceso";
            this.txt_usuario_niv_acceso.Size = new System.Drawing.Size(97, 26);
            this.txt_usuario_niv_acceso.TabIndex = 3;
            this.txt_usuario_niv_acceso.Text = "1";
            this.txt_usuario_niv_acceso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(347, 17);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(118, 19);
            this.label20.TabIndex = 3;
            this.label20.Text = "No. de Usuario";
            // 
            // txt_usuario_no_usuario
            // 
            this.txt_usuario_no_usuario.Location = new System.Drawing.Point(350, 34);
            this.txt_usuario_no_usuario.MaxLength = 16;
            this.txt_usuario_no_usuario.Name = "txt_usuario_no_usuario";
            this.txt_usuario_no_usuario.Size = new System.Drawing.Size(111, 26);
            this.txt_usuario_no_usuario.TabIndex = 1;
            this.txt_usuario_no_usuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(146, 19);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(66, 19);
            this.label21.TabIndex = 1;
            this.label21.Text = "Nombre";
            // 
            // txt_usuario_nombre
            // 
            this.txt_usuario_nombre.Location = new System.Drawing.Point(149, 35);
            this.txt_usuario_nombre.MaxLength = 32;
            this.txt_usuario_nombre.Name = "txt_usuario_nombre";
            this.txt_usuario_nombre.Size = new System.Drawing.Size(195, 26);
            this.txt_usuario_nombre.TabIndex = 0;
            // 
            // timer_no_tarjeta
            // 
            this.timer_no_tarjeta.Interval = 500;
            this.timer_no_tarjeta.Tick += new System.EventHandler(this.timer_no_tarjeta_Tick);
            // 
            // timer_barra
            // 
            this.timer_barra.Enabled = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 747);
            this.Controls.Add(this.groupBox_usuarios);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SYSCOM - Ingeniería Seguridad";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_dispositivos)).EndInit();
            this.groupBox_usuarios.ResumeLayout(false);
            this.groupBox_usuarios.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.group_log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_log)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_usuario_imagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data_usuarios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_disp_impiar;
        private System.Windows.Forms.DataGridView data_dispositivos;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn direccion_ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn puerto;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn contrasena;
        private System.Windows.Forms.DataGridViewTextBoxColumn estatus;
        private System.Windows.Forms.Button btn_disp_agregar;
        private System.Windows.Forms.Button btn_disp_eliminar;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_disp_contrasena;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txt_disp_usuario;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_disp_puerto;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_disp_direcc_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_disp_descripcion;
        private System.Windows.Forms.GroupBox groupBox_usuarios;
        private System.Windows.Forms.Button btn_usuario_tomar_foto;
        private System.Windows.Forms.Button btn_usuario_limpiar;
        private System.Windows.Forms.Button btn_usuario_buscar_imagen;
        private System.Windows.Forms.PictureBox picture_usuario_imagen;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txt_usuario_no_tarjeta;
        private System.Windows.Forms.DataGridView data_usuarios;
        private System.Windows.Forms.Button btn_usuario_agregar;
        private System.Windows.Forms.Button btn_usuario_eliminar;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txt_usuario_niv_acceso;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txt_usuario_no_usuario;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txt_usuario_nombre;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_disp_mant_cerrado;
        private System.Windows.Forms.Button btn_disp_abrir;
        private System.Windows.Forms.Button btn_disp_mant_abierto;
        private System.Windows.Forms.Button btn_disp_cerrar;
        private System.Windows.Forms.GroupBox group_log;
        private System.Windows.Forms.DataGridView data_log;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_fecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_descripcion;
        private System.Windows.Forms.Button btn_usuario_sinc_1;
        private System.Windows.Forms.Button btn_usuario_sincronizar;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_tarjeta;
        private System.Windows.Forms.DataGridViewTextBoxColumn permiso_tarjeta;
        private System.Windows.Forms.DataGridViewTextBoxColumn imagen;
        private System.Windows.Forms.Button btn_usuario_leer_tarjeta;
        private System.Windows.Forms.Timer timer_no_tarjeta;
        private System.Windows.Forms.Button btn_usuario_actualizar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_usu_sinc_usuarios_disp;
        private System.Windows.Forms.Button btn_usu_sinc_todos_a_todos;
        private System.Windows.Forms.Button btn_usuario_tomar_huella;
        private System.Windows.Forms.Button btn_usuario_buscar_huella;
        private System.Windows.Forms.Timer timer_barra;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox txt_evento_tipo_secundario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker txt_evento_fecha_fin;
        private System.Windows.Forms.DateTimePicker txt_evento_fecha_inicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox txt_evento_tipo_principal;
        private System.Windows.Forms.Button btn_eventos_mostrar;
    }
}

