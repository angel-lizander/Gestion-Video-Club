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
    class Clientes
    {
       
        public static SqlConnection conexion;
        Clases.Conexion c = new Clases.Conexion();
        SqlCommand sentencia;
        SqlDataReader reader;

        SqlCommand CClienteRenta;
        SqlDataReader RCClienteRenta;


        SqlCommand CAdeudo;
        SqlDataReader RCAdeudo;

        DataTable DataTable;
        SqlDataAdapter SqlDataAdapter;

        public Clientes()
        {
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
        //CARGA DE CLIENTES EN DGV
        public void CargarClientes(DataGridView dgv)
        {
            try
            {
                SqlDataAdapter = new SqlDataAdapter("Select NumDeCliente,Nombre,Apellido,Edad,Telefono,Email,En_renta,Fecha_Alta,Cedula from clientes", Properties.Settings.Default.Conexion);

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
        

        //GUARDAR CIENTES EN DB
        public string insertar(string Nombre,string Apellido, int Edad,string Telefono,string Email, string Cedula)
        {
            string salida = "Guardado con exito.";
            
            int en_renta=0;

            String fecha = DateTime.Now.ToString("dd-MM-yyyy");

            Random AleatoryNumber = new Random(DateTime.Now.Minute);//por aqui esta el asunto, este es quien esta dando un codigo aleatorio a los clientes

            try
            {
              
                    sentencia = new SqlCommand("insert into clientes(Nombre,Apellido,Edad,NumDeCliente,Telefono,Email,En_renta,Fecha_Alta,Cedula) values('" + Nombre + "','" + Apellido + "'," + Edad + "," + AleatoryNumber.Next() + ",'" + Telefono + "','" + Email + "'," + en_renta + ",'" + fecha + "','"+ Cedula +"')", conexion);

                    sentencia.ExecuteNonQuery();
                

            }
            catch (Exception ex)
            {
                salida = ("Error al guardar registro de cliente.  " + ex.ToString());


            }
            return salida;
        }

        //VALIDADOR DE REGISTRO YA EXISTENTE AL GUARDAR
        public int ClienteExistente(string Cedula)
        {

           
            int contador = 0;
            try
            {
                sentencia = new SqlCommand("select * from clientes where Cedula='" + Cedula + "'  ",conexion);
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

        //CONSULTA ID EN BASE AL NumDeCliente DEL REGISTRO para mandar a frmModificarParther
        public void consultaID(int NumDeCliente)
        {
            sentencia = new SqlCommand("select id,Nombre,Apellido,Edad,Telefono,Email,Cedula from clientes where NumDeCliente=" + NumDeCliente + "  ", conexion);

            reader = sentencia.ExecuteReader();
            String idValue = "";
            String NameValue = "";
            String ApellidoValue = "";
            String AgeValue = "";
            String PhoneValue = "";
            String EmailValue = "";
            String Cedula = "";


            while (reader.Read())
            {
                idValue = (String.Format("{0}", reader["id"]));
                NameValue = (String.Format("{0}", reader["Nombre"]));
                ApellidoValue = (String.Format("{0}", reader["Apellido"]));
                AgeValue = (String.Format("{0}", reader["Edad"]));
                PhoneValue = (String.Format("{0}", reader["Telefono"]));
                EmailValue = (String.Format("{0}", reader["Email"]));
                Cedula = (String.Format("{0}", reader["Cedula"]));
            }
            reader.Close();

            frmModificarParther frmModificarParther = new frmModificarParther(idValue, NameValue, ApellidoValue, AgeValue, PhoneValue, EmailValue,Cedula,NumDeCliente);

            frmModificarParther.ShowDialog();

        }



        //Metodo para Editar peliculas
        public void actualizar(string Nombre, string Apellido, int Edad, string Mail, string Cedula,string Telefono, int NumDeCliente)
        {
            try
            {
                CClienteRenta = new SqlCommand("SELECT NumeroDeCliente FROM Prestamos WHERE NumeroDeCliente=" + NumDeCliente + " ", conexion);
                RCClienteRenta = CClienteRenta.ExecuteReader();
                int SiTieneRentas=0;
                while (RCClienteRenta.Read())
                {
                    SiTieneRentas = 1;
                 
                }
                RCClienteRenta.Close();
                if (SiTieneRentas==0)
                {
                    sentencia = new SqlCommand("UPDATE clientes SET Nombre='" + Nombre + "', Apellido='" + Apellido + "',Edad=" + Edad + ",Email='" + Mail + "',Cedula='" + Cedula + "',Telefono='" + Telefono + "' WHERE NumDeCliente=" + NumDeCliente + "  ", conexion);

                    sentencia.ExecuteNonQuery();

                    MessageBox.Show("Datos de cliente modificados con exito");

                }
                else
                {
                    MessageBox.Show("Imposible modificar datos de cliente si cuenta con rentas actuales.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar registro.  " + ex.ToString());
            }
            
        }
        //BORRAR REGISTRO DE DB
        public void DeleteRegistry(int NumDeClientes)
        {
            try
            {
                //PRIMERO SE CONSULTA SI EL CLIENTE A ELIMINAR NO CUENTA CON UNA RENTA ACTUALMENTE
                CAdeudo = new SqlCommand("SELECT TituloDePelicula FROM Prestamos WHERE NumeroDeCliente=" + NumDeClientes + " ", conexion);
                RCAdeudo = CAdeudo.ExecuteReader();
                List<string> lista = new List<string>();
                int bandera1=0;

                while (RCAdeudo.Read())
                {
                    lista.Add(String.Format("{0}", RCAdeudo["TituloDePelicula"]));
                    bandera1 = 1;          
                }
                RCAdeudo.Close();

                if (bandera1==1)
                {
                    var ListElements = string.Join(Environment.NewLine, lista);
                    MessageBox.Show("Imposible eliminar a el socio debido a que actualmente cuenta con una renta existente."+ "\n\n" + "Los titulos actualmente en renta son:"+ "\n\n"+ListElements);
                }
                else
                {
                    sentencia = new SqlCommand("DELETE FROM clientes WHERE NumDeCliente=" + NumDeClientes + "  ", conexion);

                    sentencia.ExecuteNonQuery();

                    MessageBox.Show("Socio eliminado con exito");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Problemas al eliminar registro" + ex.ToString());
            }

        }

    }

}
