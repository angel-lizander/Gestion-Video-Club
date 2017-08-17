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
    class Principal
    {
        public static SqlConnection conexion;
        SqlCommand CTipo;
        SqlDataReader RCTipo;

        public Principal()
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
        public Boolean TipoDeUsuario(string user)
        {
            Boolean UoA = false;

            try
            {
                CTipo = new SqlCommand("SELECT Tipo_de_cuenta FROM Admins WHERE [User]='"+user+"' ", conexion);
                RCTipo = CTipo.ExecuteReader();

                while (RCTipo.Read())
                {
                    if (String.Format("{0}", RCTipo["Tipo_de_cuenta"])=="Administrador")
                    {
                        UoA = true;
                    }
                }
                RCTipo.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible identificar el tipo de cuenta." + ex.ToString());
                throw;
            }

            return UoA;
        }
    }
}
