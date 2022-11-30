using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using demo_sdk_hikvision.Clases;

namespace demo_sdk_hikvision
{
    public partial class btnRegresar : Form
    {
        Controlador driver;
        Control Principal;
        public btnRegresar()
        {
            InitializeComponent();
            
        }
        private async void AgregarDispositivo_Load(object sender, EventArgs e)
        {
            //NOTE: Cargamos lis clientes desde la API
            List<Cliente> listaClietes = await driver.ObtenerListaClientes();
            //NOTE: Agregamos un vacio
            cmbCliente.Text = "Seleccione un cliente";
            Cliente emty = new Cliente();
            emty.id = -1;
            emty.Descripcion = "Sin selección";
            cmbCliente.Items.Add(emty);
            if (listaClietes.Count > 0)
            {
                foreach (Cliente item in listaClietes)
                {
                    cmbCliente.Items.Add(item);
                    cmbCliente.DisplayMember = "Descripcion";
                    cmbCliente.ValueMember = "id";
                }
            }
            else
            {
                MessageBox.Show("Error de conexion", "Error al cargar los clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void  cmbCliente_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbSector.Text = "Seleccione un sector";
            cmbSector.Items.Clear();
            Cliente cliente = (Cliente)cmbCliente.SelectedItem;
            if (cliente != null)
            {
                List<Sector> sectrs = await driver.ObtenerSectores(Convert.ToString(cliente.id));
                cmbSector.Enabled = (sectrs.Count > 0);
                if (sectrs != null)
                {
                    foreach (Sector sector in sectrs)
                    {
                        cmbSector.Items.Add(sector);
                        cmbSector.DisplayMember = "Nombre";
                        cmbSector.ValueMember = "id";
                    }
                }
            }
        }
        private void AgregarDispositivo_VisibleChanged(object sender, EventArgs e)
        {
            
        }
        public void setPantallaPrincipal( Control mainFrame )
        {
            Principal = mainFrame;
        }
        public void setDriver( Controlador control )
        {
            driver = control;
        }
        private void botonRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Principal.Show();
        }
        private async void botonAgregar_Click(object sender, EventArgs e)
        {
            Cliente cl = (Cliente)cmbCliente.SelectedItem;
            Sector str = (Sector)cmbSector.SelectedItem;
            string cliente = "-1";
            string sector = "-1";
            if ( cl != null)
            cliente = Convert.ToString(cl.id);
            if( str != null )
            sector = Convert.ToString(str.id);
            
            if( cliente != "-1")
            {
                //Verificamos los datos del dispositivo
                if (txtNombre.Text != "" && txtDireccion.Text != "" && txtContrasenia.Text != "" && txtNombre.Text != "" && txtPuerto.Text != "")
                {
                    //NOTE: validamos los la duplicidad del dispositivo
                    if (driver.verificarDispositivo(txtDireccion.Text)) 
                    {
                        MessageBox.Show("Error al registrar el dispositivo\nDispositivo ya registrado", "Error");
                        return;
                    }
                    //Volvemos a conectar el dispositivo
                    string respuesta =  driver.ProbarConexion(txtDireccion.Text, Convert.ToInt32(txtPuerto.Text), txtUsuario.Text, txtContrasenia.Text);
                    Console.WriteLine(respuesta);
                    if (respuesta == "OK")
                    {
                        //barEstado.Value = 50; 
                        //lblProgress.Text = "Registrando dispositivo";
                        //note: Registramos en suinpac
                        string idSuinpac = await driver.ResgistrarDispositivo(cliente, txtNombre.Text, txtDireccion.Text, sector, txtPuerto.Text, txtContrasenia.Text, txtUsuario.Text);
                        Console.WriteLine(idSuinpac);
                        if (idSuinpac != "")
                        {
                            MessageBox.Show("Dispositivo registrado", "Mensaje");
                            //Empezamos a descargar los empleados de forma masiva
                            driver.CrearBitacoraDescargarEmpleados(cliente,idSuinpac);
                            MessageBox.Show("Iniciando descarga de empleados", "Mensaje");
                            //NOTE: limpiamos los campos
                            txtNombre.Text = "";
                            txtDireccion.Text = "";
                            txtContrasenia.Text = "";
                            txtNombre.Text = "";
                            txtPuerto.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Error al registrar el dispositivo", "Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show(respuesta,"Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un cliente", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
