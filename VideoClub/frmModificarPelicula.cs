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
    public partial class frmModificarPelicula : Form
    {

        Clases.Conexion c = new Clases.Conexion();
        Regex validar = new Regex(@"^[a-zA-Z0-9._ñÑáéíóúÁÉÍÓÚ ]+$"); /*Solo texto y numeros y _*/
        Regex validar2 = new Regex(@"^[0-9]+$"); /*Solo numeros*/

        string TituloOriginal
        {
            get; set;
        }


        //METODO QUE RECIBE EL VALOR DEL ID y demas
        public frmModificarPelicula(string id, string Titulo, string Anio, string Genero, string Existencias)
        {
            TituloOriginal = Titulo;//Guardamos el titulo original antes de cualquier modificacion para comparar si ya esta en renta dicha pelicula y si es true no permitir la modificacion de la pelicula excepto las existencias.
           
            InitializeComponent();
            
            txtId.Text = id;
            txtTitulo.Text = Titulo;
            txtAnio.Text = Anio;
            txtGenero.Text = Genero;
            txtExistencias.Text = Existencias;
        
           
        }
        

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmModificarPelicula_Load(object sender, EventArgs e)
        {
           
        }



        public void RecibirDatos(string Titulo,string Genero)
        {
           
            

        }

        //BOTON GUARDAR PARA EDITAR
        private void button1_Click(object sender, EventArgs e)
        {
           
            Boolean vRec = EspaciosVacios();
            String fecha = DateTime.Now.ToString("dd-MM-yyyy");
            if (vRec == false)
            {
                if (validar.IsMatch(txtTitulo.Text) && validar2.IsMatch(txtAnio.Text) && validar.IsMatch(txtGenero.Text) && validar2.IsMatch(txtExistencias.Text))
                {
                    
                    try
                    {
                        c.actualizar( TituloOriginal,Convert.ToInt32(txtId.Text),txtTitulo.Text, Convert.ToInt32(txtAnio.Text), txtGenero.Text, Convert.ToInt32(txtExistencias.Text)  );

                        this.Close();
         
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Imposible actualizar registro.");
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


        //Clase que valida que esten llenos todos los campos
        public Boolean EspaciosVacios()
        {
            Boolean rEspaciosVacios = false;

            if (String.IsNullOrWhiteSpace(txtTitulo.Text) || String.IsNullOrWhiteSpace(txtAnio.Text) || String.IsNullOrWhiteSpace(txtExistencias.Text) || String.IsNullOrWhiteSpace(txtGenero.Text))
            {
                rEspaciosVacios = true;
            }
            return rEspaciosVacios;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
