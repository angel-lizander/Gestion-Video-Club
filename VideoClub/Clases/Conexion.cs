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
    public class Conexion
    {
        public static SqlConnection conexion;
        SqlCommand sentencia;
        SqlDataReader reader;

        SqlCommand sentenciaAntiDelete;
        SqlDataReader readerAntiDelete;

        SqlCommand sentenciaTitOriginal;
        SqlDataReader readerTitOriginal;

        SqlCommand sentenciaExistenciasOri;
        SqlDataReader readerExistenciasOri;

        DataTable DataTable;
        SqlDataAdapter SqlDataAdapter;

        public Conexion()
        {
            try
            {
                conexion = new SqlConnection("Data Source=DESKTOP-SEONIQ3;Initial Catalog=videoclub_db1;Integrated Security=True");
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Base de datos no disponible, error en la conexion." + ex.ToString());
               
            }
        }

        //Metodo para guardar peliculas
        public string insertar(string titulo, int anio, string genero, int existencias, string fecha)
        {
            string salida = "Guardado con exito.";
            int disponibles;
            int en_renta;
            
           
            try
            {
                disponibles = existencias;
                en_renta = existencias - disponibles;
                
               
                    sentencia = new SqlCommand("insert into peliculas(Titulo,Anio,Genero,Existencias,Fecha_de_ingreso,Disponibles,En_renta) values('" + titulo + "'," + anio + ",'" + genero + "'," + existencias + ",'" + fecha + "'," + disponibles + "," + en_renta + ")", conexion);

                    sentencia.ExecuteNonQuery();
                
             
            }
            catch (Exception ex)
            {
                salida = ("Error al guardar registro.  " + ex.ToString());
                

            }
            return salida;
        }

        //Metodo que verifica si ya existe la pelicula en DB
        public int PeliculasExistentes(string titulo)
        {
            int contador = 0;
            try
            {
                sentencia = new SqlCommand("select * from peliculas where Titulo='" + titulo + "'  ", conexion);
                reader = sentencia.ExecuteReader();
                while (reader.Read())
                {
                    contador++;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible realizar consulta de existencias.  " + ex.ToString());
                throw;
            }
            return contador;
        }

        //CARGAR PELICULAS DENTRO DE DATAGRIDVIEW
        public void CargarPeliculas(DataGridView dgv)
        {
            try
            {
                SqlDataAdapter = new SqlDataAdapter("Select Titulo,Anio,Genero,Existencias,En_renta,Disponibles,Fecha_de_ingreso from peliculas", Properties.Settings.Default.Conexion);
                DataTable = new DataTable();
                SqlDataAdapter.Fill(DataTable);
                dgv.DataSource = DataTable;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible llenar tabla con el contenido" + ex.ToString());
                throw;
            }
        }


        //CONSULTA ID EN BASE AL TITULO DEL REGISTRO para mandar a frmModificarPelicula
        public void consultaID(string Titulo)
        {
            sentencia = new SqlCommand("select id,Titulo,Anio,Genero,Existencias from peliculas where Titulo='" + Titulo + "'  ", conexion);
            reader = sentencia.ExecuteReader();
            String idValue = "";
            String TituloValue = "";
            String AnioValue = "";
            String GeneroValue = "";
            String ExistenciasValue = "";
           

            while (reader.Read())
            {
                idValue = (String.Format("{0}", reader["id"]));
                TituloValue = (String.Format("{0}", reader["Titulo"]));
                AnioValue = (String.Format("{0}", reader["Anio"]));
                GeneroValue = (String.Format("{0}", reader["Genero"]));
                ExistenciasValue = (String.Format("{0}", reader["Existencias"]));
            }
            reader.Close();

            frmModificarPelicula frmModificarPelicula1 = new frmModificarPelicula(idValue,TituloValue, AnioValue, GeneroValue, ExistenciasValue);

            frmModificarPelicula1.ShowDialog();
            
        }



        //Metodo para Editar peliculas
        public void actualizar(string TituloOriginal,int id,string titulo, int anio, string genero, int existencias)
        {
            //VALIDA SI ESTA PELICULA SE ENCUENTRA ACTUALMENTE RENTADA
            try
            {
                sentenciaTitOriginal = new SqlCommand("SELECT NombreDeCliente FROM prestamos WHERE TituloDePelicula='"+TituloOriginal+"' ", conexion);

                readerTitOriginal= sentenciaTitOriginal.ExecuteReader();
                List<String> Lista = new List<String>();

                int Bandera = 0;
                int x = 0;
                while (readerTitOriginal.Read())
                {
                    Lista.Add(String.Format("{0}", readerTitOriginal["NombreDeCliente"]));
                    Bandera = 1; //EXISTENTE ESTA PELICULA EN RENTAS ACTUALES

                }
                readerTitOriginal.Close();
              

                //SI NO EXISTE ETA PELICULA ACTUALMENTE RENTADA SE PROCEDE CON LOS CAMBIOS QUE SEAN
                String fecha1 = DateTime.Now.ToString("dd-MM-yyyy");
                if (Bandera == 0)//NO EXISTENTE ESTA PELICULA EN RENTAS ACTUALES
                {
                    int disponibles = existencias;
                    int en_renta = existencias - disponibles;

                    sentencia = new SqlCommand("UPDATE peliculas SET Titulo='" + titulo + "', Anio=" + anio + ",Genero='" + genero + "',Existencias=" + existencias + ",Fecha_de_ingreso='" + fecha1 + "',En_renta="+en_renta+",Disponibles="+ disponibles +" WHERE id=" + id + "  ", conexion);

                    sentencia.ExecuteNonQuery();

                    MessageBox.Show("Registro actualizado con exito.");
                }
                else//SI EXISTE LA PELICULA EN RENTAS ACTUALES POR LO TANTO SOLO SE PODRA MODIFICAR LAS EXISTENCIAS O BIEN ARROJA MENSAJE DE ERROR SI LAS EXISTENCIAS SON MENORES A LAS ACTUALES
                {

                    int BanderaExistencias = 0;
                    try
                    {
                        sentenciaExistenciasOri = new SqlCommand("SELECT Existencias,Disponibles,En_renta FROM peliculas WHERE Titulo='" + TituloOriginal + "' ", conexion);

                        readerExistenciasOri = sentenciaExistenciasOri.ExecuteReader();
                        String ConsultaExistencia = "Empty";
                        String ConsultaDisponible = "Empty";
                        String ConsultaEnRenta = "Empty";
                        while (readerExistenciasOri.Read())
                        {
                            ConsultaExistencia = (String.Format("{0}", readerExistenciasOri["Existencias"]));
                            ConsultaDisponible = (String.Format("{0}", readerExistenciasOri["Disponibles"]));
                            ConsultaEnRenta = (String.Format("{0}", readerExistenciasOri["En_renta"]));
                        }
                        readerExistenciasOri.Close();
                        int ConsultaExistencia2 = Convert.ToInt32(ConsultaExistencia);
                        int ConsultaDisponible2 = Convert.ToInt32(ConsultaDisponible);
                        int ConsultaEnRenta2 = Convert.ToInt32(ConsultaEnRenta);
                        int Diferencia = (existencias - ConsultaExistencia2) + ConsultaDisponible2;
                        if (existencias==ConsultaExistencia2)
                        {
                            MessageBox.Show("Imposible editar pelicula debido a que " + Lista.Count + " ejemplar(es)  se encuentran rentados.");
                        }
                        else
                        {
                            if (existencias >= ConsultaEnRenta2)
                            {
                                sentencia = new SqlCommand("UPDATE peliculas SET Existencias=" + existencias + ",Disponibles=" + Diferencia + " WHERE id=" + id + "  ", conexion);

                                sentencia.ExecuteNonQuery();

                                BanderaExistencias = 1;

                                MessageBox.Show("Se modifico el valor de existencias con exito!. Imposible editar los demas campos de pelicula debido a que " + Lista.Count + " ejemplar(es)  se encuentran rentados.");
                            }
                            else if ((existencias < ConsultaEnRenta2))
                            {

                                MessageBox.Show("Imposible realizar cambio de existencias debido a que el numero de existencias es menor a las rentas actuales.");

                            }
                            
                        }
                      
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al editar registro..     "+ex.ToString());
                        throw;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar registro.  " + ex.ToString());

            }
         
        }

        public void DeleteRegistry(string Titulo, DataGridView dgv)
        {   
            try
            {
                //VALIDA SI LA PELICULA A ELIMINAR ESTA ACTUALMENTE EN RENTA
                sentenciaAntiDelete = new SqlCommand("SELECT * FROM Prestamos WHERE TituloDePelicula='" + Titulo + "' ", conexion);
                int Count=0;
                readerAntiDelete = sentenciaAntiDelete.ExecuteReader();
                while (readerAntiDelete.Read())
                {
                    Count += 1;
                }
                readerAntiDelete.Close();

                if (Count>=1)
                {
                    MessageBox.Show("Pelicula imposible de eliminar debido a que se encuentra actualmente en renta");
                }
                else
                {
                    sentencia = new SqlCommand("DELETE FROM peliculas WHERE Titulo='" + Titulo + "'  ", conexion);

                    sentencia.ExecuteNonQuery();

                    MessageBox.Show("Registro eliminado con exito");

                    //METODO PARA RECARGAR DGV
                    try
                    {
                        SqlDataAdapter = new SqlDataAdapter("Select Titulo,Anio,Genero,Existencias,En_renta,Disponibles,Fecha_de_ingreso from peliculas", Properties.Settings.Default.Conexion);
                        DataTable = new DataTable();
                        SqlDataAdapter.Fill(DataTable);
                        dgv.DataSource = DataTable;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Imposible llenar tabla con el contenido" + ex.ToString());
                        throw;
                    }
                }
                
               
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Problemas al eliminar registro"+ex.ToString());
            }
           
        }



    }
}
