using System;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class ZmianaIlosci
    {
        private int ilosc;
        private  ArrayList lista;
      
        public void wskazCzesci(int idUslugi)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                
                string sel = string.Format("SELECT Czesci.idCzesci FROM Uslugi INNER JOIN CdoU ON Uslugi.idUslugi = CdoU.idUslugi INNER JOIN Czesci ON CdoU.idCzesci = Czesci.idCzesci AND CdoU.idCzesci = Czesci.idCzesci WHERE (CdoU.idUslugi = '{0}')",idUslugi);
              
                SqlCommand cmd = new SqlCommand(sel, connection);
                SqlDataReader dr = cmd.ExecuteReader();

                 lista = new ArrayList();
                
                while (dr.Read())
                {
                    
                        string idPr = dr["idCzesci"].ToString();
                         lista.Add(ilosc = int.Parse(idPr));
                                           
                 }

                dr.Close();

            connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        aktualizuj();
        }

        public void aktualizuj()
        {
        
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                
                for (int i = 0; i < lista.Count; i++)
                {
                    string minus = string.Format("UPDATE Czesci SET ilosc = ilosc - 1 WHERE (idCzesci = {0})", lista[i] );

                    SqlCommand dec = new SqlCommand(minus, connection);
                    dec.ExecuteNonQuery();
                }
                
                connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         }
    }
}
