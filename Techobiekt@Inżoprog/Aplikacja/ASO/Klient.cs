using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class Klient
    {
        private string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public void dodajKlienta(string imie,string nazwisko, string telefon,string miasto,string ulica,int nrLokalu)
        {
            int numer0;
            int numerKlienta;

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawOsobe = string.Format("Insert Into Osoby" + "(idOsoby,imie,nazwisko,telefon,miasto,ulica,nrLokalu) Values" + "(@idOsoby,@imie,@nazwisko,@telefon,@miasto,@ulica,@nrLokalu)");
                string count = string.Format("SELECT MAX(idOsoby) FROM Osoby");
                string wstawKlienta = string.Format("Insert Into Klienci" + "(idKlienta,idOsoby) Values" + "(@idKlienta,@idOsoby)");
                string countKlienta = string.Format("SELECT MAX(idKlienta) FROM Klienci");

                using (SqlCommand licznik = new SqlCommand(count, connection))
                {

                    numer0 = Convert.ToInt32(licznik.ExecuteScalar());
                }
                numer0++;

                using (SqlCommand licznikKlienci = new SqlCommand(countKlienta, connection))
                {

                    numerKlienta = Convert.ToInt32(licznikKlienci.ExecuteScalar());
                }
                numerKlienta++;


                using (SqlCommand cmd = new SqlCommand(wstawOsobe, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idOsoby";
                    param.Value = numer0;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@imie";
                    param.Value = imie;
                    param.SqlDbType = SqlDbType.Char;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@nazwisko";
                    param.Value = nazwisko;
                    param.SqlDbType = SqlDbType.Char;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@telefon";
                    param.Value = telefon;
                    param.SqlDbType = SqlDbType.Char;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@miasto";
                    param.Value = miasto;
                    param.SqlDbType = SqlDbType.Char;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@ulica";
                    param.Value = ulica;
                    param.SqlDbType = SqlDbType.Char;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@nrLokalu";
                    param.Value = nrLokalu;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(wstawKlienta, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idKlienta";
                    param.Value = numerKlienta;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idOsoby";
                    param.Value = numer0;
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

        public void usunKlienta(int idKlienta)
        {

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            string usun = string.Format("Delete from Klienci where idKlienta='{0}'", idKlienta);

            using (SqlCommand com = new SqlCommand(usun, connection))
            {
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Przepraszam! Nie ma klienta o tym id ", ex);
                }
            }
        }

    }
}
