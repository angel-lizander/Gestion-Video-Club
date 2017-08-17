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
    class Admins
    {
        public static SqlConnection conexion;
        SqlCommand SNuevo;

        SqlCommand CAExistente;
        SqlDataReader RCAExistente;

        SqlCommand SDelete;

        DataTable DataTable;
        SqlDataAdapter SqlDataAdapter;


        public Admins()
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

        public int GuardarNuevosAdmins(string Nombre, string Apellido, string User, string Password, string Tipo)
        {
            int TodoBien = 0;
            try
            {
                SNuevo = new SqlCommand("INSERT INTO Admins (Nombre,Apellido,[User],Password,Tipo_de_cuenta) VALUES ('" + Nombre + "','" + Apellido + "','" + User + "','" + Password + "','" + Tipo + "') ", conexion);
                SNuevo.ExecuteNonQuery();
                TodoBien = 1;
                MessageBox.Show("Nuevo usuario dado de alta exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible realizar el alta del nuevo usuario del sistema."+ex.ToString());
                throw;
            }

            return TodoBien;


        }
        public int AdminYaExistente(string User)
        {
            int Bandera = 0;
            try
            {
               
                CAExistente = new SqlCommand("SELECT [User] FROM Admins WHERE [User]='" + User + "' ", conexion);
                RCAExistente = CAExistente.ExecuteReader();
                while (RCAExistente.Read())
                {
                    Bandera = 1;
                }
                RCAExistente.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Imposible consultar si empleado ya existe en sistema.");
                throw;
            }
            
            return Bandera;
        }

        public void CargarAdmins(DataGridView dgv)
        {
            try
            {
                SqlDataAdapter = new SqlDataAdapter("SELECT Nombre,Apellido,[User],Password,Tipo_de_cuenta FROM Admins", Properties.Settings.Default.Conexion);

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

        public int DeleteAdmin(string User)
        {
            int ConfirmacionDelete = 0;

            try
            {

                SDelete = new SqlCommand("DELETE FROM Admins WHERE [User]='" + User + "' ", conexion);
                SDelete.ExecuteNonQuery();
                ConfirmacionDelete = 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible eliminar usuario del sistema. "+ex.ToString());
                throw;
            }

            return ConfirmacionDelete;

        }
        
    }
}
