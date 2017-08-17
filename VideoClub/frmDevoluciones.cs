using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;




namespace MaxVideoClub
{
    
    public partial class frmDevoluciones : Form
    {
        public static SqlConnection conexion;
        Clases.Devolucion Dev=new Clases.Devolucion();
        int ValorID
        {
            get; set;
        }
        SqlCommand CRelleno;
        SqlDataReader RCRelleno;


     

        public frmDevoluciones()
        {
            InitializeComponent();
            try
            {
                conexion = new SqlConnection("Data Source=DESKTOP-SEONIQ3;Initial Catalog=videoclub_db1;Integrated Security=True");
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Base de datos no disponible, error en la conexion." + ex.ToString());
                throw;
            }
        }
        
        private void Devoluciones_Load(object sender, EventArgs e)
        {
            
            Dev.CargarMulta();
            txtFecha.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtFiltro.GotFocus += new EventHandler(this.TextGotFocus);
            txtFiltro.LostFocus += new EventHandler(this.TextLostFocus);
        }

        private void btnEfectuar_Click(object sender, EventArgs e)
        {
            dgvDevoluciones.Refresh();
            Dev.CargarDevoluciones(dgvDevoluciones,txtNumDeCliente.Text);
        }

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

        private void txtFecha_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex validar2 = new Regex(@"^[0-9]+$"); /*Solo numeros*/

            if (validar2.IsMatch(txtNumDeCliente.Text))
            {
                if (Dev.ConsulRentaExistente(txtNumDeCliente.Text) == true)
                {
                    Dev.CargarDevoluciones(dgvDevoluciones,txtNumDeCliente.Text);
                    Dev.CargarMulta();
                    txtFechaDevolucion.Enabled = true;
                    txtPagar.Enabled = true;
                    txtPelicula.Enabled = true;
                    btnEfectuar.Enabled = true;
                    txtFiltro.Enabled = true;
                    cmbFiltro.Enabled = true;
                    dgvDevoluciones.Enabled = true;

                 



                }
                else
                {
                    MessageBox.Show("El cliente no cuenta con rentas actualmente.");
                }
            }
            else
            {
                MessageBox.Show("Favor de ingresar un numero de cliente valido.");
            }
        }

        //FILTRO
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (cmbFiltro.Text == "Titulo de pelicula")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("TituloDePelicula like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Numero de cliente")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("Convert([Numerodecliente], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Nombre de cliente")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("NombreDeCliente like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Fecha de entrega")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("Fecha_De_Entrega like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Fecha de devolucion")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("Fecha_De_Devolucion like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Multa")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("Convert([Multa], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Cualquiera...")
            {

                ((DataTable)dgvDevoluciones.DataSource).DefaultView.RowFilter = string.Format("Convert([Multa], System.String) LIKE '%{0}%' OR [TituloDePelicula] LIKE '%{0}%' OR [Fecha_De_Entrega] LIKE '%{0}%' OR [Fecha_De_Devolucion] LIKE '%{0}%' OR [NombreDeCliente] LIKE '%{0}%' OR Convert([NumeroDeCliente], System.String) LIKE '%{0}%' ", txtFiltro.Text.Trim());

            }

        }

        //FUNCION DE CAMBIAR TEXTO DE "buscar" EN TXTBUSCAR A VACIO
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

                Dev.CargarDevoluciones(dgvDevoluciones,txtNumDeCliente.Text);


            }

        }

        private void dgvDevoluciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            String id = dgvDevoluciones[0, dgvDevoluciones.CurrentCell.RowIndex].Value.ToString();
            int id2 = Convert.ToInt32(id);

            //ESTA CONSULTA SE HIZO EN ESTE FORMULARIO PORQUE NO SUPE COMO MANDAR LLAMAR MAS DE 1 VALOR DE LA CLASE DEVOLUCIONES QUE ES LA QUE HACE TODAS LAS CONSULTAS 
            try
            {
                String Titulo = "";
                String FDev = "";
                String Multa = "";
                CRelleno = new SqlCommand("SELECT TituloDePelicula,Fecha_De_Devolucion,Multa FROM Prestamos WHERE id=" + id + " ", conexion);

                RCRelleno = CRelleno.ExecuteReader();

                while (RCRelleno.Read())
                {
                    Titulo = (String.Format("{0}", RCRelleno["TituloDePelicula"]));
                    FDev = (String.Format("{0}", RCRelleno["Fecha_De_Devolucion"]));
                    Multa = (String.Format("{0}", RCRelleno["Multa"]));
                }
                RCRelleno.Close();

                
                txtFechaDevolucion.Text = FDev;
                txtPelicula.Text = Titulo;
                ValorID = id2;
                int Multa2 = Convert.ToInt32(Multa);
                int total = Multa2 + 50;
                
                txtPagar.Text = Convert.ToString(total);

                btnPagar.Enabled = true;
               


            }
            catch (Exception ex)
            {
                MessageBox.Show("Problemas al seleccionar la devolucion." + ex.ToString());
            }



            
           
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {       
            Dev.Factura(ValorID);
            Dev.CargarDevoluciones(dgvDevoluciones, txtNumDeCliente.Text);//DGV llenado
          
            txtPelicula.Text = "";
            txtFechaDevolucion.Text = "";
            txtPagar.Text = "";
            dgvDevoluciones.Enabled = false;
            btnPagar.Enabled = false;
            this.Controls.Clear();

            this.InitializeComponent();

        }
        //FUNCION PARA RESET EL FRM CUANDO SE CIERRE
        private void frmDevoluciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();


        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void Regresar_Click(object sender, EventArgs e)
        {
            this.Close();
            //Copia este codigo y pegalo en cada formulario para que cierre
        }
    }
}
