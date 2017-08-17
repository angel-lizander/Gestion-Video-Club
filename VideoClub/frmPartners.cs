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
    public partial class frmPartners : Form
    {
        Clases.Conexion c = new Clases.Conexion();
        Clases.Clientes Cclients = new Clases.Clientes();
        Regex validar = new Regex(@"^[a-zA-Z0-9._ñÑáéíóúÁÉÍÓÚ@ ]+$"); /*Solo texto y numeros y _*/
        Regex validar2 = new Regex(@"^[0-9]+$0"); /*Solo numeros*/

        private void frmPartners_Load(object sender, EventArgs e)
        {
            Cclients.CargarClientes(dgvParthers);
            txtFiltro.GotFocus += new EventHandler(this.TextGotFocus);
            txtFiltro.LostFocus += new EventHandler(this.TextLostFocus);
        }

        public frmPartners()
        {
            InitializeComponent();
        }

        private void txtAnio_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

       

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        //FUNCION QUE VALIDA CEDULA

        public Boolean validarcedula()
        {
            string pCedula = txtCedula.Text;

        int vnTotal = 0;

        string vcCedula = pCedula.Replace("-", "");

        int pLongCed = vcCedula.Trim().Length;

        int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };



            if (pLongCed< 11 || pLongCed> 11)

                return false;



            for (int vDig = 1; vDig <= pLongCed; vDig++)

            {

                int vCalculo = Int32.Parse(vcCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];

                if (vCalculo< 10)

                    vnTotal += vCalculo;

                else

                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));

            }



            if (vnTotal % 10 == 0)

                return true;

            else

                return false;

        }
    



    //BOTON GUARDAR
    private void button1_Click(object sender, EventArgs e)
        {
            //Aqui viene a parar la validación de la cedula, xD lo logré
            Boolean cedula = validarcedula();
            Boolean vRec = EspaciosVacios();
            String fecha = DateTime.Now.ToString("dd-MM-yyyy");
            if (cedula==false) {
                MessageBox.Show("La cédula es inválida");
            }
            else
            {            
            if (vRec == false)
            {
                if (validar.IsMatch(txtNombre.Text) && validar.IsMatch(txtApellido.Text) && validar2.IsMatch(txtTelefono.Text) && validar2.IsMatch(txtEdad.Text) && validar.IsMatch(txtEmail.Text) && validar2.IsMatch(txtCedula.Text))
                {
                    if (Cclients.ClienteExistente(txtCedula.Text) == 0)
                    {
                        MessageBox.Show(Cclients.insertar(
                            txtNombre.Text,
                            txtApellido.Text,
                            Convert.ToInt32(txtEdad.Text),
                            txtTelefono.Text,
                            txtEmail.Text,
                            txtCedula.Text)
                            );

                        Cclients.CargarClientes(dgvParthers);

                        txtNombre.Text = "";
                        txtApellido.Text = "";
                        txtEdad.Text = "";
                        txtTelefono.Text = "";
                        txtEmail.Text = "";
                        txtCedula.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Imposible, la cedula ya existe en el sistema.");
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






        public void TextGotFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Buscar...")
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
            }
        }
        public void TextLostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Buscar...";
                tb.ForeColor = Color.LightGray;

                Cclients.CargarClientes(dgvParthers);


            }

        }


        //FILTRO TXTBOX
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (cmbFiltro.Text == "Nombre")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Nombre like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Edad")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Convert([Edad], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Apellido")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Apellido like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Email")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Email like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "NumDeCliente")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Convert([NumDeCliente], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Telefono")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Convert([Telefono], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Cedula")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Convert([Cedula], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Cualquiera...")
            {

                ((DataTable)dgvParthers.DataSource).DefaultView.RowFilter = string.Format("Convert([Edad], System.String) LIKE '%{0}%' OR [Nombre] LIKE '%{0}%' OR [Apellido] LIKE '%{0}%' OR Convert([Cedula], System.String) LIKE '%{0}%' OR [Email] LIKE '%{0}%' OR Convert([NumDeCliente], System.String) LIKE '%{0}%' OR Convert([Telefono], System.String) LIKE '%{0}%' OR Convert([Cedula], System.String) LIKE '%{0}%'", txtFiltro.Text.Trim());

            }
        }
        //BOTON ACTUALIZAR
        private void btbActualizar_Click(object sender, EventArgs e)
        {
            dgvParthers.Refresh();
            Cclients.CargarClientes(dgvParthers);
        }
        //Funcion numeradora de rows en DataGridView
        private void pintar_numeros(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dg = (DataGridView)sender;
            // Current row record
            string rowNumber = (e.RowIndex + 1).ToString();

            // Format row based on number of records displayed by using leading zeros
            while (rowNumber.Length < dg.RowCount.ToString().Length) rowNumber = "0" + rowNumber;

            // Position text
            SizeF size = e.Graphics.MeasureString(rowNumber, this.Font);
            if (dg.RowHeadersWidth < (int)(size.Width + 20)) dg.RowHeadersWidth = (int)(size.Width + 20);

            // Use default system text brush
            Brush b = SystemBrushes.ControlText;

            // Draw row number
            e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }
       
     
        //PASA VALOR DE NumDeCliente A CLASE Clientes O BORRAR REGISTRO
        private void dgvParthers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvParthers.CurrentCell.ColumnIndex == 0)
            {

                int columnI4ndex = 2; //Columna de NumDeCliente para consultar registro
                String someString = dgvParthers[columnI4ndex, dgvParthers.CurrentCell.RowIndex].Value.ToString();
                int NumIDValue = Convert.ToInt32(someString);
                Cclients.consultaID(NumIDValue);
            }
            else if (dgvParthers.CurrentCell.ColumnIndex == 1)
            {
                int columnI4ndex = 2; //Columna de NumDeCliente para consultar registro
                String NumDeCliente = dgvParthers[columnI4ndex, dgvParthers.CurrentCell.RowIndex].Value.ToString();
                int NumIDValue = Convert.ToInt32(NumDeCliente);
                Cclients.DeleteRegistry(NumIDValue);
                Cclients.CargarClientes(dgvParthers);

            }
        }

        private void frmPartners_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();

        }

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
