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
    public partial class frmRentasActuales : Form
    {
        Clases.RentasActuales Ra = new Clases.RentasActuales();

        public frmRentasActuales()
        {
            InitializeComponent();
        }

        private void frmRentasActuales_Load(object sender, EventArgs e)
        {
            Ra.CargarRentas(dgvRentas);
            txtFiltro.GotFocus += new EventHandler(this.TextGotFocus);
            txtFiltro.LostFocus += new EventHandler(this.TextLostFocus);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ra.CargarRentas(dgvRentas);
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

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (cmbFiltro.Text == "Titulo de pelicula")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("TituloDePelicula like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Numero de cliente")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("Convert([Numerodecliente], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Nombre de cliente")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("NombreDeCliente like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''"));

            }
            else if (cmbFiltro.Text == "Fecha de entrega")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("FechaDeEntrega like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Fecha de devolucion")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("FechaDeDevolucion like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Multa")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("Convert([Multa], System.String) like '%{0}%'", txtFiltro.Text.Trim());

            }
            else if (cmbFiltro.Text == "Cualquiera...")
            {

                ((DataTable)dgvRentas.DataSource).DefaultView.RowFilter = string.Format("Convert([Multa], System.String) LIKE '%{0}%' OR [TituloDePelicula] LIKE '%{0}%' OR [FechaDeEntrega] LIKE '%{0}%' OR [FechaDeDevolucion] LIKE '%{0}%' OR [NombreDeCliente] LIKE '%{0}%' OR Convert([NumeroDeCliente], System.String) LIKE '%{0}%' ", txtFiltro.Text.Trim());

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

                Ra.CargarRentas(dgvRentas);


            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
