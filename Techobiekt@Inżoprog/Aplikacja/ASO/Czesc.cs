using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace ASO
{
    class Czesc
    {
        public int idCzesci;
        private int cenaUslugi;

        private string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public void dodajCzesc(string nazwa, int cena,int ilosc)
        {
                        
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into Czesci" + "(idCzesci,nazwa,cena,ilosc) Values" + "(@idCzesci,@nazwa,@cena,@ilosc)");
                string count = string.Format("SELECT MAX(idCzesci) FROM Czesci");
            
                using (SqlCommand licznik = new SqlCommand(count, connection))
                {

                    idCzesci = Convert.ToInt32(licznik.ExecuteScalar());
                }
                idCzesci++;

                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idCzesci";
                    param.Value = idCzesci;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@nazwa";
                    param.Value = nazwa;
                    param.SqlDbType = SqlDbType.Char;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@cena";
                    param.Value = cena;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@ilosc";
                    param.Value = ilosc;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();

                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void dodajCzescDoUslugi(int idUslugi, int idCzesci)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into CdoU" + "(idUslugi,idCzesci) Values" + "(@idUslugi,@idCzesci)");
            
                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idUslugi";
                    param.Value = idUslugi;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idCzesci";
                    param.Value = idCzesci;
                    param.SqlDbType = SqlDbType.Int;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();

                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int wyznaczCeneUslugi(int idUslugi)
        {
            cenaUslugi = 0;


            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string count = string.Format("SELECT SUM(dbo.Czesci.cena*2) FROM dbo.CdoU INNER JOIN dbo.Czesci ON dbo.CdoU.idCzesci = dbo.Czesci.idCzesci INNER JOIN dbo.Uslugi ON dbo.CdoU.idUslugi = dbo.Uslugi.idUslugi WHERE (dbo.Uslugi.idUslugi = '{0}')", idUslugi);
                
                using (SqlCommand lcena = new SqlCommand(count, connection))
                {

                    cenaUslugi = Convert.ToInt32(lcena.ExecuteScalar());
                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return cenaUslugi;

        }

        public void wstawCeneUslugi(int cenaUslugi, int idUslugi)
        {


            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawCene = string.Format("UPDATE Uslugi SET cena =cena+{1} WHERE idUslugi = {0}", idUslugi, cenaUslugi);
                
                using (SqlCommand cmd = new SqlCommand(wstawCene, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void uzupelnijStan(int idCzesci,int iloscSztuk)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                
                string wstawka = string.Format("UPDATE Czesci SET ilosc = ilosc +{0} WHERE (idCzesci = {1})",iloscSztuk,idCzesci );

                SqlCommand cmd = new SqlCommand(wstawka, connection);
                
                cmd.ExecuteNonQuery();
                
                connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
       
    }
}
