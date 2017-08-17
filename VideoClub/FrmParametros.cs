using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxVideoClub
{
    public partial class FrmParametros : Form
    {
        public FrmParametros()
        {
            InitializeComponent();
        }

        private void cmdReporte_Click(object sender, EventArgs e)
        {
            FrmVisorReporte frm = new FrmVisorReporte();
            frm.Titulo = txtTitulo.Text;
            frm.Anio = txtAnio.Text;
            frm.Genero = txtGenero.Text;
            frm.ShowDialog();
        }
    }
}
