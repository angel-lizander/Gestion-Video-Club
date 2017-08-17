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
    class RentasActuales
    {
        public static SqlConnection conexion;

        Clases.Conexion c = new Clases.Conexion();

        SqlCommand sentencia;
        SqlDataReader reader;

        DataTable DataTable;
        SqlDataAdapter SqlDataAdapter;

        public RentasActuales()
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
        public void CargarRentas(DataGridView dgv)
        {
            try
            {
                SqlDataAdapter = new SqlDataAdapter("SELECT TituloDePelicula,NumeroDeCliente,NombreDeCliente,Fecha_De_Entrega,Fecha_De_Devolucion,Multa FROM Prestamos", Properties.Settings.Default.Conexion);

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
}
