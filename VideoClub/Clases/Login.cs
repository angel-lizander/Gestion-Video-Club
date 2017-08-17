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
    class Login
    {
        public static SqlConnection conexion;
        SqlCommand CAcceso;
        SqlDataReader RCAcceso;

        public Login()
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

        public int LoginPass(string User, string Password)
        {
            int Pase = 0;
            try
            {
                CAcceso = new SqlCommand("SELECT [User] FROM Admins WHERE [User]='" + User + "' AND Password='" + Password + "' ", conexion);
                RCAcceso = CAcceso.ExecuteReader();
                while (RCAcceso.Read())
                {
                    Pase = 1;
                }
                RCAcceso.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Imposible consultar credenciales.");
                throw;
            }

            return Pase;
        }
    }
}