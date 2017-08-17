using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Net;
using System.Net.Mail;

namespace MaxVideoClub.Clases
{
    class Devolucion
    {
        public static SqlConnection conexion;
        Clases.Conexion c = new Clases.Conexion();
        SqlCommand ConsultaIDs;
        SqlDataReader RConsultaIDs;

        SqlCommand CClienteExist;
        SqlDataReader RCClienteExist;

        SqlCommand CRellenoFactura;
        SqlDataReader RCRellenoFactura;

        SqlCommand CRelleno;
        SqlDataReader RCRelleno;

        DataTable DataTable;
        SqlDataAdapter SqlDataAdapter;

        SqlCommand SumaMulta;

        SqlCommand CEliminar;

        SqlCommand SSumarInventario;

        SqlCommand SRestaRentaCliente;

        SqlCommand CCorreo;
        SqlDataReader RCCorreo;


        public Devolucion()
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
        //CARGA DE prestamos actuales EN DGV
        public void CargarDevoluciones(DataGridView dgv,string NumDeCliente)
        {
            try
            {
                int Cliente = Convert.ToInt32(NumDeCliente);
                SqlDataAdapter = new SqlDataAdapter("SELECT * FROM Prestamos WHERE NumeroDeCliente="+NumDeCliente+" ", Properties.Settings.Default.Conexion);

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
        //FUNCION PARA CARGAR MULTA DE $50 SI LA FECH DE DEVOLUCION ES ANTERIOR A LA FECHA ACTUAL 
        public void CargarMulta()
        {
            try
            {
                int x = 1;
                String Fecha = DateTime.Now.ToString("dd-MM-yyyy");
                DateTime hoy = DateTime.ParseExact(Fecha, "dd-MM-yyyy",
                                         System.Globalization.CultureInfo.InvariantCulture);

                ConsultaIDs = new SqlCommand("SELECT id,Fecha_De_Devolucion,Multa FROM Prestamos", conexion);
                RConsultaIDs = ConsultaIDs.ExecuteReader();
                String ID = "";
                String VFdevol = "";
                String Multa="";
                while (RConsultaIDs.Read())
                {
                    ID = (String.Format("{0}", RConsultaIDs["id"]));
                    int Nid = Convert.ToInt32(ID);
                    VFdevol = (String.Format("{0}", RConsultaIDs["Fecha_De_Devolucion"]));

                    DateTime Fdevol = DateTime.ParseExact(VFdevol, "dd-MM-yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture);

                    Multa = (String.Format("{0}", RConsultaIDs["Multa"]));
                    int CompararMulta = Convert.ToInt32(Multa);
                    if (Fdevol > hoy)
                    {
                        //NO APLICA MULTA SIGUE ESTANDO EN PERIODO DE TOLERANCIA SE REGRESA A 0 EN CASO DE QUE TENGA MULTA POR CAMBIOS EN LA FECHA DEL EQUIPO SIN PRECAUCION
                        int y = 0;
                        //FUNCION PARA ASIGNAR MULTA CORRESPONDIENTE
                        SumaMulta = new SqlCommand("UPDATE Prestamos SET Multa=" + y + " WHERE id=" + Nid + "  ", conexion);

                        SumaMulta.ExecuteNonQuery();

                    }
                    else
                    {
                        if (CompararMulta==0)
                        {
                            if (x==1)
                            {
                                MessageBox.Show("Hay nuevas multas!");
                            }
                            x= 2;
                            int y = 50;
                            //FUNCION PARA ASIGNAR MULTA CORRESPONDIENTE
                            SumaMulta = new SqlCommand("UPDATE Prestamos SET Multa=" + y + " WHERE id=" + Nid + "  ", conexion);
                           
                            SumaMulta.ExecuteNonQuery();              
                        }
                        else 
                        {
                           
                        }  
                    }
                }
                RConsultaIDs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible actulizar multas." + ex.ToString());
            }
        }

        public Boolean ConsulRentaExistente(string NumDeCliente)
        {
            Boolean Confirmacion=false;

            CClienteExist = new SqlCommand("SELECT NumeroDeCliente FROM Prestamos WHERE NumeroDeCliente="+NumDeCliente+" ", conexion);
            RCClienteExist = CClienteExist.ExecuteReader();

            while (RCClienteExist.Read())
            {
                Confirmacion = true;
            }
            RCClienteExist.Close();

            return Confirmacion;
        }

        public void Factura(int ValorID)
        {
            //CREACION DE FACTURA
            try
            {
                String Titulo = "";
                String NumDeCliente = "";
                String NombreDeCliente = "";
                String Fecha_De_Entrega = "";
                String Fecha_De_Devolucion = "";
                String Multa = "";

                CRellenoFactura = new SqlCommand("SELECT * FROM Prestamos WHERE id=" + ValorID + " ", conexion);
                RCRellenoFactura = CRellenoFactura.ExecuteReader();
              
                while (RCRellenoFactura.Read())
                {
                   
                    Titulo = (String.Format("{0}", RCRellenoFactura["TituloDePelicula"]));
                    NumDeCliente = (String.Format("{0}", RCRellenoFactura["NumeroDeCliente"]));
                    NombreDeCliente = (String.Format("{0}", RCRellenoFactura["NombreDeCliente"]));
                    Fecha_De_Entrega = (String.Format("{0}", RCRellenoFactura["Fecha_De_Entrega"]));
                    Fecha_De_Devolucion = (String.Format("{0}", RCRellenoFactura["Fecha_De_Devolucion"]));
                    Multa = (String.Format("{0}", RCRellenoFactura["Multa"]));
                    
                }
                RCRellenoFactura.Close();
                //int Cobro = 50+Convert.ToInt32(Multa);
                Random AleatoryNumber = new Random(DateTime.Now.Minute);

                int x = AleatoryNumber.Next();
                int Cobro = 50 + Convert.ToInt32(Multa);

                String Archivo = (NumDeCliente + NombreDeCliente + x + ".pdf");
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Facturas\\"+Archivo, FileMode.Create));
                doc.Open();
               
                Paragraph paragraph = new Paragraph("Ticket de compra MaxvideoClub\n\n");
                Paragraph paragraph1 = new Paragraph("Numero de cliente: "+NumDeCliente);
                Paragraph paragraph2 = new Paragraph("Nombre de cliente: "+NombreDeCliente);
                Paragraph paragraph3 = new Paragraph("Titulo rentado: " + Titulo);
                Paragraph paragraph4 = new Paragraph("Fecha de renta: " + Fecha_De_Entrega);
                Paragraph paragraph5 = new Paragraph("Fecha de devolucion: " + Fecha_De_Devolucion);
                Paragraph paragraph6 = new Paragraph("Monto total a pagar: $" + Cobro+"\n\n");
                Paragraph paragraph7 = new Paragraph("Gracias por su preferencia.");
                doc.Add(paragraph);
                doc.Add(paragraph1);
                doc.Add(paragraph2);
                doc.Add(paragraph3);
                doc.Add(paragraph4);
                doc.Add(paragraph5);
                doc.Add(paragraph6);
                doc.Add(paragraph7);
                doc.Close();

                //SENTENCIA PARA SUMAR 1 AL INVENTARIO
                SSumarInventario = new SqlCommand("UPDATE peliculas SET En_renta=En_renta-1, Disponibles=Disponibles+1 WHERE Titulo='" + Titulo + "' ", conexion);
                SSumarInventario.ExecuteNonQuery();

                //SENTENCIA PARA RESTAR 1 A LAS RENTAS ACTUALES DEL CLIENTE QUE RENTA
                //SUMA A LAS RENTAS ACTUALES DEL CLIENTE
                SRestaRentaCliente = new SqlCommand("UPDATE clientes SET En_renta=En_renta-1 WHERE NumDeCliente=" + NumDeCliente + " ", conexion);
                SRestaRentaCliente.ExecuteNonQuery();




                //DESPUES DELA CREACION DE LA FACTURA SE REALIZA LA ELIMINACION DE LA RENTA
                CEliminar = new SqlCommand("DELETE FROM Prestamos WHERE id=" + ValorID + " ", conexion);
                CEliminar.ExecuteNonQuery();


                //POR ULTIMO ENVIAMOS EL CORREO CON LA FACTURA ADJUNTA
                CCorreo = new SqlCommand("SELECT Email FROM clientes WHERE NumDeCliente="+NumDeCliente+" ", conexion);
                RCCorreo = CCorreo.ExecuteReader();
                String Correo="";
                while (RCCorreo.Read())
                {
                    Correo = (String.Format("{0}", RCCorreo["Email"]));
                }
                RCCorreo.Close();

                try
                {
                   
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("casartchihuahua@gmail.com", "casadearte");

                    MailMessage mm = new MailMessage(Correo, Correo, "Factura MaxVideoClub", "<b>Max VideoClub</b><br><br>Muchas gracias por su compra le enviamos su comprobante adjunto.<br><br><b>Saludos.</b>");
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.IsBodyHtml = true;
                    
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment("Facturas\\"+Archivo);
                    mm.Attachments.Add(attachment);

                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);
                    attachment.Dispose();//PARA TERMINAR EL PROCESO QUE UTILIZA EL FILE SYSTEM Y PODER ENVIAR OTRO CORREO SIGUIENTE SIN CRASH
                    MessageBox.Show("La factura ha sido enviada por correo electronico al cliente.");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al enviar el correo.");
                   
                }
                
                MessageBox.Show("Devolucion realizada con exito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problemas al realizar devolucion" + ex.ToString());
                throw;
            }
           
        }


    }
   
}
