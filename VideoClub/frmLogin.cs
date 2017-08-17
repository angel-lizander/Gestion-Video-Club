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
    
    public partial class frmLogin : Form
    {
      
        Clases.Login Logueo = new Clases.Login();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Boolean vRec = EspaciosVacios();
            if (vRec==true)
            {
                MessageBox.Show("Favor de lenar todos los ampos solicitados.");
            }
            else
            {
                if (Logueo.LoginPass(txtUser.Text, txtPassword.Text) == 1)
                {
                    frmPrincipal principal = new frmPrincipal(txtUser.Text);
                    this.Visible = false;
                    this.Controls.Clear();
                    principal.ShowDialog();
                    this.InitializeComponent();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
            }
           
            


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();

        }
        //Clase que valida que esten llenos todos los campos
        public Boolean EspaciosVacios()
        {
            Boolean rEspaciosVacios = false;

            if (String.IsNullOrWhiteSpace(txtUser.Text) || String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                rEspaciosVacios = true;
            }
            return rEspaciosVacios;
        }
    }
}
