using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MaxVideoClub
{
    public partial class frmAdminsAccounts : Form
    {
        Clases.Admins CAdmins = new Clases.Admins();
        Regex validar = new Regex(@"^[a-zA-Z0-9._ñÑáéíóúÁÉÍÓÚ ]+$"); /*Solo texto y numeros y _*/
        Regex validar2 = new Regex(@"^[0-9]+$"); /*Solo numeros*/
        public frmAdminsAccounts()
        {
            InitializeComponent();
           
        }

        private void frmAdminsAccounts_Load(object sender, EventArgs e)
        {
            CAdmins.CargarAdmins(dgvAdmins);
        }
        //funcion que valida que esten llenos todos los campos
        public Boolean EspaciosVacios()
        {
            Boolean rEspaciosVacios = false;

            if (String.IsNullOrWhiteSpace(txtNombre.Text) || String.IsNullOrWhiteSpace(txtUser.Text) || String.IsNullOrWhiteSpace(txtApellido.Text) || String.IsNullOrWhiteSpace(txtPassword.Text) || String.IsNullOrWhiteSpace(cmbTipo.Text))
            {
                rEspaciosVacios = true;
            }
            return rEspaciosVacios;
        }
        //BOTON PARA GUARDAR NUEVO MIEMBRO
        private void button1_Click(object sender, EventArgs e)
        {
            Boolean vRec = EspaciosVacios();
            String fecha = DateTime.Now.ToString("dd-MM-yyyy");
            if (vRec == false)
            {
                if (cmbTipo.SelectedItem!=null)
                {
                    if (validar.IsMatch(txtNombre.Text) && validar.IsMatch(txtApellido.Text) && validar.IsMatch(txtUser.Text) && validar.IsMatch(txtPassword.Text) && validar.IsMatch(cmbTipo.Text))
                    {
                        if (CAdmins.AdminYaExistente(txtUser.Text) == 0)
                        {
                            if (CAdmins.GuardarNuevosAdmins(txtNombre.Text, txtApellido.Text, txtUser.Text, txtPassword.Text, cmbTipo.Text)==1)
                            {

                                CAdmins.CargarAdmins(dgvAdmins);
                                txtUser.Text = "";
                                txtApellido.Text = "";
                                txtNombre.Text = "";
                                txtPassword.Text = "";
                                cmbTipo.Text = "";
                                
                            }

                        }
                        else
                        {
                            MessageBox.Show("Imposible, el empleado(Usuario) ya existe.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se permiten caracteres especiales.");
                    }
                }
                else
                {
                    MessageBox.Show("Favor de elegir un tipo de usuario.");
                }
               
            }
            else
            {
                MessageBox.Show("Favor de llenar todos los campos");
            }







        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void frmAdminsAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();
        }
        //BOTON DE DGV PARA ELIMINAR USUARIO
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAdmins.CurrentCell.ColumnIndex == 0)
            {

                int columnI4ndex = 3; //Columna de [User] para consultar registro
                String UserValue = dgvAdmins[columnI4ndex, dgvAdmins.CurrentCell.RowIndex].Value.ToString();

                if (CAdmins.DeleteAdmin(UserValue)==1)
                {
                    MessageBox.Show("Usuario eliminado correctamente.");
                    CAdmins.CargarAdmins(dgvAdmins);
                }
                else
                {
                    MessageBox.Show("El usuario no se elimino.");
                }
                
            
            

            }
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
