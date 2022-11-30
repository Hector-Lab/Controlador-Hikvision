namespace demo_sdk_hikvision
{
    partial class Control
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Control));
            this.lblEstado = new System.Windows.Forms.Label();
            this.btnConectar = new System.Windows.Forms.Button();
            this.tmrBitacora = new System.Windows.Forms.Timer(this.components);
            this.btnAgregar = new System.Windows.Forms.Button();
            this.barEstado = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.tmrSegundoPlano = new System.Windows.Forms.Timer(this.components);
            this.tmrCalcula = new System.Windows.Forms.Timer(this.components);
            this.btnEventos = new System.Windows.Forms.Button();
            this.txtBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(23, 145);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(53, 16);
            this.lblEstado.TabIndex = 10;
            this.lblEstado.Text = "Estado:";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(16, 29);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(120, 60);
            this.btnConectar.TabIndex = 11;
            this.btnConectar.Text = "Verificar Conexión";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // tmrBitacora
            // 
            this.tmrBitacora.Enabled = true;
            this.tmrBitacora.Interval = 780000;
            this.tmrBitacora.Tick += new System.EventHandler(this.tmrBitacora_Tick);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(256, 29);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(120, 60);
            this.btnAgregar.TabIndex = 16;
            this.btnAgregar.Text = "Registrar Dispositivo";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.button1_Click);
            // 
            // barEstado
            // 
            this.barEstado.Location = new System.Drawing.Point(89, 142);
            this.barEstado.Name = "barEstado";
            this.barEstado.Size = new System.Drawing.Size(522, 23);
            this.barEstado.TabIndex = 17;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(88, 114);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(64, 16);
            this.lblProgress.TabIndex = 18;
            this.lblProgress.Text = "Proceso: ";
            // 
            // tmrSegundoPlano
            // 
            this.tmrSegundoPlano.Tick += new System.EventHandler(this.tmrSegundoPlano_Tick);
            // 
            // tmrCalcula
            // 
            this.tmrCalcula.Enabled = true;
            this.tmrCalcula.Interval = 420000;
            this.tmrCalcula.Tick += new System.EventHandler(this.tmrCalcula_Tick);
            // 
            // btnEventos
            // 
            this.btnEventos.Location = new System.Drawing.Point(496, 29);
            this.btnEventos.Name = "btnEventos";
            this.btnEventos.Size = new System.Drawing.Size(115, 60);
            this.btnEventos.TabIndex = 19;
            this.btnEventos.Text = "ObtenerEventos";
            this.btnEventos.UseVisualStyleBackColor = true;
            this.btnEventos.Click += new System.EventHandler(this.btnEventos_Click);
            // 
            // txtBox
            // 
            this.txtBox.Location = new System.Drawing.Point(20, 193);
            this.txtBox.Name = "txtBox";
            this.txtBox.Size = new System.Drawing.Size(592, 227);
            this.txtBox.TabIndex = 20;
            this.txtBox.Text = "";
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 450);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.btnEventos);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.barEstado);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.lblEstado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Control";
            this.Text = "Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Timer tmrBitacora;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.ProgressBar barEstado;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Timer tmrSegundoPlano;
        private System.Windows.Forms.Timer tmrCalcula;
        private System.Windows.Forms.Button btnEventos;
        private System.Windows.Forms.RichTextBox txtBox;
    }
}