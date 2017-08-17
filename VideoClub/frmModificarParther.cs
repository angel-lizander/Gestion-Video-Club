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
    public partial class frmModificarParther : Form
    {
        Clases.Conexion c = new Clases.Conexion();
        Clases.Clientes Cclients = new Clases.Clientes();
        Regex validar = new Regex(@"^[a-zA-Z0-9._ñÑáéíóúÁÉÍÓÚ@ ]+$"); /*Solo texto y numeros y _*/
        Regex validar2 = new Regex(@"^[0-9]+$00"); /*Solo numeros*/

        public frmModificarParther(string id,string Nombre,string Apellido, string Edad,string Telefono, string EmailValue, string cedula, int NumDeCliente)
        {
            InitializeComponent();

            

            String NumDeClienteTxt = Convert.ToString(NumDeCliente);
            txtNumDeCliente.Text = NumDeClienteTxt;
            txtId.Text = id;
            txtNombre.Text = Nombre;
            txtApellido.Text = Apellido;
            txtEdad.Text = Edad;
            txtTelefono.Text = Telefono;
            txtEmail.Text = EmailValue;
            txtCedula.Text = cedula;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmModificarParther_Load(object sender, EventArgs e)
        {

        }
        //FUNCION QUE VALIDA QUE TODOS LOS CAMPOS ESTEN LLENOS 
        public Boolean EspaciosVacios()
        {
            Boolean rEspaciosVacios = false;

            if (String.IsNullOrWhiteSpace(txtNombre.Text) || String.IsNullOrWhiteSpace(txtApellido.Text) || String.IsNullOrWhiteSpace(txtEdad.Text) || String.IsNullOrWhiteSpace(txtEmail.Text) || String.IsNullOrWhiteSpace(txtTelefono.Text) || String.IsNullOrWhiteSpace(txtCedula.Text))
            {
                rEspaciosVacios = true;
            }
            return rEspaciosVacios;
        }

        //Valida un campo sea cero
        public Boolean campo0(int pValor)
            {
            Boolean campo0 = false;
         if (pValor==0) {
        campo0= true;
        }
        return campo0;
        }

        //      }si LA FORMA MAS FEA DE COMENTAR xD bye

        

        //FUNCION QUE VALIDA CEDULA

        public Boolean validarcedula()
        {
            string pCedula = txtCedula.Text; //;

            int vnTotal = 0;

            string vcCedula = pCedula.Replace("-", "");// 
          
            int pLongCed = vcCedula.Trim().Length;

            int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };



            if (pLongCed < 11 || pLongCed > 11)

                return false;



            for (int vDig = 1; vDig <= pLongCed; vDig++)

            {

                int vCalculo = Int32.Parse(vcCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];

                if (vCalculo < 10)

                    vnTotal += vCalculo;

                else

                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));

            }



            if (vnTotal % 10 == 0)

                return true;

            else

                return false;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Boolean cedula = validarcedula();
            Boolean vRec = EspaciosVacios();
            //Boolean  campo0 = txtEdad.Text();
            String fecha = DateTime.Now.ToString("dd-MM-yyyy");
            /*if (txtEdad.Text== true)
               {
                MessageBox.Show("La edad no puede ser cero");
                }
            else{
            */
            if (cedula == false)
            {
                MessageBox.Show("La cédula es inválida");
            }
            else
           if (vRec == false)
            {
                if (validar.IsMatch(txtNombre.Text) && validar.IsMatch(txtApellido.Text) && validar2.IsMatch(txtEdad.Text) && validar.IsMatch(txtEmail.Text) && validar2.IsMatch(txtCedula.Text) && validar2.IsMatch(txtTelefono.Text))
                {

                    try
                    {
                        Cclients.actualizar(txtNombre.Text, txtApellido.Text, Convert.ToInt32(txtEdad.Text), txtEmail.Text, txtCedula.Text,txtTelefono.Text,Convert.ToInt32(txtNumDeCliente.Text));

                        this.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Imposible actualizar registro." + ex.ToString());
                    }



                }
                else
                {
                    MessageBox.Show("No se permiten caracteres especiales.");
                }
            }
            else
            {
                MessageBox.Show("Favor de llenar todos los campos");
            }
        }
    }
}
