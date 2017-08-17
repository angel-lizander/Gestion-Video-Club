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
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace MaxVideoClub
{
    

    public partial class frmRenta : Form
    {
        Clases.Conexion c = new Clases.Conexion();
        Clases.Prestamos p = new Clases.Prestamos();

      
        public frmRenta()
        {
            InitializeComponent();

        
        }


        private void frmRenta_Load(object sender, EventArgs e)
        {
           
            c.CargarPeliculas(dgvPeliculas); //FUNCION PARA LLENAR DATAGRIDVIEW
            txtFiltro.GotFocus += new EventHandler(this.TextGotFocus);
            txtFiltro.LostFocus += new EventHandler(this.TextLostFocus);

            String fecha = DateTime.Now.ToString("dd-MM-yyyy");
            txtFechaEntrega.Text = fecha;

          

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (cmbFiltro.Text == "Titulo")
            {

                ((DataTable)dgvPeliculas.DataSource).DefaultView.RowFilter = string.Format("Titulo like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Año")
            {

                ((DataTable)dgvPeliculas.DataSource).DefaultView.RowFilter = string.Format("Convert([Anio], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Genero")
            {

                ((DataTable)dgvPeliculas.DataSource).DefaultView.RowFilter = string.Format("Genero like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Existencias")
            {

                ((DataTable)dgvPeliculas.DataSource).DefaultView.RowFilter = string.Format("Convert([Existencias], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Cualquiera...")
            {

                ((DataTable)dgvPeliculas.DataSource).DefaultView.RowFilter = string.Format("Convert([Anio], System.String) LIKE '%{0}%' OR [Titulo] LIKE '%{0}%' OR [Genero] LIKE '%{0}%' OR Convert([Existencias], System.String) LIKE '%{0}%' ", txtFiltro.Text.Trim());

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

                c.CargarPeliculas(dgvPeliculas);


            }

        }
        //SOLICITUD DE EXISTENCIAS POR PELICULA SELECCIONADA
        private void dgvPeliculas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String TituloValue = dgvPeliculas[0, dgvPeliculas.CurrentCell.RowIndex].Value.ToString();
            
            int Existencias1= Convert.ToInt32(p.ConsultaRelleno(TituloValue) );

            txtExistencias.Text = Convert.ToString(Existencias1);


            cmbPelicula.Text = TituloValue;

            btnEfectuar.Enabled = true;
        }

        private void pintar(object sender, DataGridViewRowPostPaintEventArgs e)
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
        //BOTON GUARDAR EN DB
        private void button2_Click(object sender, EventArgs e)
        {
          
            DateTimePicker dtp = sender as DateTimePicker;
            DateTime today = DateTime.Today.Date;
            if ( (dtpDevolucion.Value < today) || (dtpDevolucion.Text==txtFechaEntrega.Text) )
            {
                MessageBox.Show("Favor de incluir una fecha posterior al dia de hoy");
              
            }
            else
            {

                p.insertar(txtNumDeCliente.Text, cmbPelicula.Text, txtFechaEntrega.Text, dtpDevolucion.Text);
                c.CargarPeliculas(dgvPeliculas);
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        //FUNCION PARA VALIDAR QUE EL NUMERO DE CLIENTE INTRODUCIDO EN FRM SEA EXISTENTE
        private void button1_Click(object sender, EventArgs e)
        {
            Regex validar2 = new Regex(@"^[0-9]+$"); /*Solo numeros*/
         
            if (validar2.IsMatch(txtNumDeCliente.Text))
            {
                if (p.ConsultaClienteExistente(txtNumDeCliente.Text) ==true)
                {
                    cmbPelicula.Enabled = true;
                    txtExistencias.Enabled = true;
                    txtFechaEntrega.Enabled = true;
                    dtpDevolucion.Enabled = true;
                    txtFiltro.Enabled = true;
                    cmbFiltro.Enabled = true;
                    dgvPeliculas.Enabled = true;

                    stripName.Text = p.ConsultaClienteNombreApellido(txtNumDeCliente.Text);



                }
                else
                {
                    MessageBox.Show("El cliente no existe");
                }
            }
            else
            {
                MessageBox.Show("Favor de ingresar un numero de cliente valido.");
            }

        }

        private void AlCerrar(object sender, FormClosedEventArgs e)
        {

            txtNumDeCliente.Text = "";
            txtFiltro.Text = "";
            txtExistencias.Text = "";
            cmbPelicula.Enabled = false;
            txtExistencias.Enabled = false;
            txtFechaEntrega.Enabled = false;
            dtpDevolucion.Enabled = false;
            txtFiltro.Enabled = false;
            cmbFiltro.Enabled = false;
            dgvPeliculas.Enabled = false;
        }

        private void dtpDevolucion_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmRenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        //Funcion numeradora de rows en DataGridView

    }
}
