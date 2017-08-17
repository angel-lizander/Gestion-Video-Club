using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MaxVideoClub.Clases
{
    class Prestamos
    {
        public static SqlConnection conexion;
        Clases.Conexion c = new Clases.Conexion();
        SqlCommand sentencia;
        SqlDataReader reader;

        SqlCommand SentenciaName;
        SqlDataReader reader2;

        SqlCommand SentenciaDescuento;
        SqlDataReader reader3;

        SqlCommand SSumaRentaCliente;
        

        SqlCommand SentenciaDescontar;
      



        public Prestamos()
        {
            try
            {
                conexion = new SqlConnection("Data Source=DESKTOP-SEONIQ3;Initial Catalog=videoclub_db1;Integrated Security=True");
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Base de datos no disponib777777le, error en la conexion." + ex.ToString());
                throw;
            }
        }
        //CONSULTA DE EXISTENCIAS PARA REGRESAR A FRMRENTAS
        public string ConsultaRelleno(string Titulo)
        {
            String Existencias = "";
             try
            {
                sentencia = new SqlCommand("SELECT Disponibles FROM peliculas WHERE Titulo='"+Titulo+"' ", conexion);
                
                reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    Existencias = (String.Format("{0}", reader["Disponibles"]));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problemas al consultar existencias" + ex.ToString());
            }

            return Existencias;
        }




        //CONSULTA DE NumDeCliente para habilitar frmRentas
        public Boolean ConsultaClienteExistente(string NumDeCliente)
        {
           
            Boolean Cliente=false;
            try
            {
                sentencia = new SqlCommand("SELECT * FROM clientes WHERE NumDeCliente=" + NumDeCliente + " ", conexion);

                reader = sentencia.ExecuteReader();
                
                while (reader.Read())
                {
                    Cliente = true;
                    MessageBox.Show("Cliente encontrado: " + (String.Format("{0}", reader["Nombre"]))+ " "+(String.Format("{0}", reader["Apellido"])));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problemas al consultar cliente" + ex.ToString());
            }
           

            return Cliente;
        }
        //CONSULTA DE nombrede lciente para statusstrip frmRentas
        public string ConsultaClienteNombreApellido(string NumDeCliente)
        {

            String NameyLast = "Ningunos";
            try
            {
                sentencia = new SqlCommand("SELECT Nombre,Apellido FROM clientes WHERE NumDeCliente=" + NumDeCliente + " ", conexion);

                reader = sentencia.ExecuteReader();
                
                while (reader.Read())
                {

                    NameyLast = ( (String.Format("{0}", reader["Nombre"]))+" "+(String.Format("{0}", reader["Apellido"])) );
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problemas al consultar Nombre y apellido." + ex.ToString());
            }


            return NameyLast;
        }

        //GUARDAR CIENTES EN DB
        public void insertar(string NumeroDeCliente, string Titulo, string FEntrega, string FDevolucion)
        {
            string salida = "Renta realizada con exito.";
          
            Int64 Number = Convert.ToInt32(NumeroDeCliente);
            try
            {

               
                //CONSULTA DE QUE HALLA DISPONIBLES
                SentenciaDescuento = new SqlCommand("SELECT Disponibles,En_Renta FROM peliculas WHERE Titulo='" + Titulo + "' ", conexion);

                reader3 = SentenciaDescuento.ExecuteReader();
                String Disponibles = "Empty";
                String EnRenta = "Empty";

                while (reader3.Read())
                {
                    Disponibles = (String.Format("{0}", reader3["Disponibles"]));
                    EnRenta = (String.Format("{0}", reader3["En_renta"]));

                }
                reader3.Close();
                int DisponiblesINT = Convert.ToInt32(Disponibles);
                int EnRenta2 = Convert.ToInt32(EnRenta);

                if (DisponiblesINT>0)
                {
                    //CONSULTA DE NOMBRE DE CLIENTE DESDE EL NUMERO DE CLIENTE
                    SentenciaName = new SqlCommand("SELECT Nombre, Apellido FROM clientes WHERE NumDeCliente=" + Number + " ", conexion);
                    reader2 = SentenciaName.ExecuteReader();
                    String Name = "Empty";
                    String NameApellido = "Empty";
                    while (reader2.Read())
                    {
                        Name = (String.Format("{0}", reader2["Nombre"]));
                        NameApellido = (String.Format("{0}", reader2["Apellido"]));
                    }
                    reader2.Close();

                    //SALVAR RENTA (INGRESAR A TABLA PRESTAMOS)
                    int Multa = 0;
                    String NombreCompleto = Name + " " + NameApellido;
                    sentencia = new SqlCommand("INSERT INTO Prestamos(TituloDePelicula,NumeroDeCliente,NombreDeCliente,Fecha_De_Entrega,Fecha_De_Devolucion,Multa) values('" + Titulo + "'," + Number + ",'" + NombreCompleto + "','" + FEntrega + "','" + FDevolucion + "'," + Multa + ")", conexion);
                    sentencia.ExecuteNonQuery();


                    //DESCUENTO DE PELICULA EN INVENTARIO
                    int Restantes = DisponiblesINT - 1;
                    int EnRenta3 = EnRenta2 + 1;
                    SentenciaDescontar = new SqlCommand("UPDATE peliculas SET Disponibles=" + Restantes + ", En_renta="+EnRenta3+" WHERE Titulo='" + Titulo + "'  ", conexion);
                    SentenciaDescontar.ExecuteNonQuery();


                    //SUMA A LAS RENTAS ACTUALES DEL CLIENTE
                    SSumaRentaCliente = new SqlCommand("UPDATE clientes SET En_renta=En_renta+1 WHERE NumDeCliente="+Number+" ", conexion);
                    SSumaRentaCliente.ExecuteNonQuery();

                    
                    MessageBox.Show("Renta realizada con exito.");

                }
                else
                {
                    MessageBox.Show("La pelicula no cuenta con ejemplares disponibles. Imposible realizar renta.");
                }
                
            }
            catch (Exception ex)
            {
                salida = ("Error al guardar la renta." + ex.ToString());
            }
            
        }

    }
}
