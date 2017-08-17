using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxVideoClub
{
    public partial class FrmVisorReporte : Form
    {
        public FrmVisorReporte()
        {
            InitializeComponent();
        }

        private void FrmVisorReporte_Load(object sender, EventArgs e)
        {
            SqlConnection ocon = new SqlConnection("Data Source=DESKTOP-SEONIQ3;Initial Catalog=videoclub_db1;Integrated Security=True");
            ocon.Open();
            string sSQL = "select * from peliculas ";
            sSQL += " where 1 = 1 ";
            if (Titulo.Trim().Length > 0)
                sSQL += " and Titulo like '" + Titulo + "%'";
            if (Genero.Trim().Length > 0)
                sSQL += " and Genero like '" + Genero + "%'";
            if (Anio.Trim().Length > 0)
                sSQL += " and Anio like '" + Anio + "%'";

            SqlDataAdapter oda = new SqlDataAdapter(sSQL, ocon);
            DataTable odt = new DataTable();
            oda.Fill(odt);

            ReportDataSource rds = new ReportDataSource();
            rds.Value = odt;
            rds.Name = "DataSet1";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.ReportEmbeddedResource = "Report1.rdlc";
            reportViewer1.LocalReport.ReportPath = @"Report1.rdlc";
            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();
            // TODO: This line of code loads data into the 'videoclub_db1DataSet.peliculas' table. You can move, or remove it, as needed.
            //this.peliculasTableAdapter.Fill(this.videoclub_db1DataSet.peliculas);

            //this.reportViewer1.RefreshReport();
        }

        public string Titulo { get; set; }

        public string Anio { get; set; }

        public string Genero { get; set; }
    }
}
