using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxVideoClub
{
    public partial class frmPrincipal : Form
    {

        frmAdminsAccounts frmAdminsAccounts = new frmAdminsAccounts();
        frmPartners frmPartners = new frmPartners();
        frmPeliculas frmPeliculas = new frmPeliculas();
        frmRenta frmRenta = new frmRenta();
        frmRentasActuales frmRentasActuales = new frmRentasActuales();
        frmDevoluciones frmDevoluciones = new frmDevoluciones();
        frmLogin frmLogin = new frmLogin();
        frmacercade frmacercade = new frmacercade();
        FrmParametros FrmParametros = new FrmParametros();
        Clases.Principal CPrincipal=new Clases.Principal();
        string USerValidation
        {
            get; set;
        }
        public frmPrincipal(string User)
        {
            USerValidation = User;
            InitializeComponent();
            lblUser.Text = User;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            String Val = USerValidation;
            if (CPrincipal.TipoDeUsuario(Val)==true)
            {
                button5.Enabled = true;
                MessageBox.Show("Iniciando como administrador.");
            }
            else
            {
                //Desactiven los permisos de usuarios secundarios
                button5.Enabled = false;
                botton6.Enabled = false;
                //Este permiso evita que cualquiera pueda hacer devoluciones.
                devolucionesToolStripMenuItem.Enabled = false;
                //Este permiso evita que cualquiera pueda hacer reporte
                reportesToolStripMenuItem.Enabled = false;
            }
            CPrincipal.TipoDeUsuario(Val);
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmAdminsAccounts.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmPeliculas.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmPartners.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmRenta.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmRentasActuales.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmDevoluciones.ShowDialog();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();
           
            this.InitializeComponent();
            frmLogin.Show();
            this.Close();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();
            frmLogin.Show();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void acercaDeMaxVideoClubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Boton acerca de            
            frmacercade.ShowDialog();
        }

        private void administrarClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPartners.ShowDialog();
        }

        private void nuevaRentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenta.ShowDialog();
        }

        private void devolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDevoluciones.ShowDialog();
        }

        private void rentasActualesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRentasActuales.ShowDialog();
        }

        private void gestionDePeliculasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPeliculas.ShowDialog();
        }

        private void administradorDeSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdminsAccounts.ShowDialog();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           FrmParametros.ShowDialog();
        }
    }
}
