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
    public partial class frmPeliculas : Form
    {
        Clases.Conexion c = new Clases.Conexion();
        BindingSource bs = new BindingSource();
        Regex validar = new Regex(@"^[a-zA-Z0-9._ñÑáéíóúÁÉÍÓÚ ]+$"); /*Solo texto y numeros y _*/
        Regex validar2 = new Regex(@"^[0-9]+$"); /*Solo numeros*/
        
        public frmPeliculas()
        {
            InitializeComponent();
        }

        private void frmPeliculas_Load(object sender, EventArgs e)
        {
          
            txtFiltro.GotFocus += new EventHandler(this.TextGotFocus);
            txtFiltro.LostFocus += new EventHandler(this.TextLostFocus);
 
            //FUNCION PARA LLENAR DATAGRIDVIEW
            c.CargarPeliculas(dgvPeliculas);



            

        }
        //BOTON PARA GUARDAR
        private void button1_Click(object sender, EventArgs e)
        {
            Boolean vRec= EspaciosVacios();
            String fecha = DateTime.Now.ToString("dd-MM-yyyy");
            if (vRec == false)
            {
                if (validar.IsMatch(txtTitulo.Text) && validar2.IsMatch(txtAnio.Text) && validar.IsMatch(txtGenero.Text) && validar2.IsMatch(txtExistencias.Text))
                {
                    if (c.PeliculasExistentes(txtTitulo.Text) == 0)
                    {
                        MessageBox.Show(c.insertar(txtTitulo.Text, Convert.ToInt32(txtAnio.Text), txtGenero.Text, Convert.ToInt32(txtExistencias.Text),fecha));
                        c.CargarPeliculas(dgvPeliculas);
                        txtGenero.Text = "";
                        txtTitulo.Text = "";
                        txtAnio.Text = "";
                        txtExistencias.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Imposible, la pelicula ya existe.");
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

        //funcion que valida que esten llenos todos los campos
         public  Boolean EspaciosVacios()
        {
            Boolean rEspaciosVacios = false;

            if (String.IsNullOrWhiteSpace(txtTitulo.Text) || String.IsNullOrWhiteSpace(txtAnio.Text) || String.IsNullOrWhiteSpace(txtExistencias.Text) || String.IsNullOrWhiteSpace(txtGenero.Text))
            {
                rEspaciosVacios = true;
            }
            return rEspaciosVacios;
        }


        //BOTON CERRAR VENTANA
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      


        //FILTRO TXTBOX
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (cmbFiltro.Text=="Titulo")
            {
                
                ((DataTable)dgvPeliculas.DataSource).DefaultView.RowFilter = string.Format("Titulo like '%{0}%'", txtFiltro.Text.Trim().Replace("'", "''")     );
               
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
                tb.Text="Buscar...";
                tb.ForeColor = Color.LightGray;
                
                c.CargarPeliculas(dgvPeliculas);
              

            }
                    
        }

   
        //BOTON ACTUALIZAR
        private void button2_Click(object sender, EventArgs e)
        {
            dgvPeliculas.Refresh();
            c.CargarPeliculas(dgvPeliculas);


        }

        //PASA VALOR DE TITULO A CLASE CONEXION O BORRAR REGISTRO
        private void dgvPeliculas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPeliculas.CurrentCell.ColumnIndex == 0)
            {
               
                int columnI4ndex = 2; //Columna de Titulo para consultar registro
                String TituloValue = dgvPeliculas[columnI4ndex, dgvPeliculas.CurrentCell.RowIndex].Value.ToString();
                
                c.consultaID(TituloValue);
            }
            else if (dgvPeliculas.CurrentCell.ColumnIndex == 1)
            {
                int columnI4ndex = 2; //Columna de Titulo para consultar registro
                String TituloValue = dgvPeliculas[columnI4ndex, dgvPeliculas.CurrentCell.RowIndex].Value.ToString();
               
                c.DeleteRegistry(TituloValue, dgvPeliculas);
                
            }
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

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmPeliculas_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Controls.Clear();

            this.InitializeComponent();

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

}

