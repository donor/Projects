using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class Faktura
    {
        public int nrFakturyKolejny;

        private string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public int dodajFakture()
        {
            nrFakturyKolejny = 0;
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
              
                string count = string.Format("SELECT MAX(nrFaktury) FROM Faktury");
                string wstaw = string.Format("Insert Into Faktury"+" (nrFaktury,data) Values"+"(@nrFaktury,@data)");

                using (SqlCommand licznik = new SqlCommand(count, connection))
                {

                    nrFakturyKolejny = Convert.ToInt32(licznik.ExecuteScalar());
                }
                nrFakturyKolejny++;

                using (SqlCommand cmd = new SqlCommand(wstaw, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@nrFaktury";
                    param.Value = nrFakturyKolejny;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@data";
                    param.Value = DateTime.Now.Date;
                    param.SqlDbType = SqlDbType.Date;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return nrFakturyKolejny;
        }
    }
}
