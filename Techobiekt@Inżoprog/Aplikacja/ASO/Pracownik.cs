using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class Pracownik
    {
        string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public void dodajDaneOsobowePracownika(string imie, string nazwisko, string stanowisko, string wynagrodzenie )
        {
            int numer0;
            int numerPracownika;

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawOsobe = string.Format("Insert Into Osoby" + "(idOsoby,imie,nazwisko) Values" + "(@idOsoby,@imie,@nazwisko)");
                string count = string.Format("SELECT MAX(idOsoby) FROM Osoby");
                string wstawPracownika = string.Format("Insert Into Pracownicy" + "(idPracownika,idOsoby,stanowisko,wynagrodzenie) Values" + "(@idPracownika,@idOsoby,@stanowisko,@wynagrodzenie)");
                string countPracownicy = string.Format("SELECT MAX(idPracownika) FROM Pracownicy");

                using (SqlCommand licznik = new SqlCommand(count, connection))
                {
                    numer0 = Convert.ToInt32(licznik.ExecuteScalar());
                }

                numer0++;

                using (SqlCommand licznikPracownicy = new SqlCommand(countPracownicy, connection))
                {

                    numerPracownika = Convert.ToInt32(licznikPracownicy.ExecuteScalar());
                }

                numerPracownika++;


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

                    cmd.ExecuteNonQuery();
                }

                      using (SqlCommand cmd = new SqlCommand(wstawPracownika, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idPracownika";
                    param.Value = numerPracownika;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idOsoby";
                    param.Value = numer0;
                    param.SqlDbType = SqlDbType.Int;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@stanowisko";
                    param.Value = stanowisko;
                    param.SqlDbType = SqlDbType.Char;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@wynagrodzenie";
                    param.Value = wynagrodzenie;
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


        public void usunPracownika(int id)
        {

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            string usunP = string.Format("Delete from Pracownicy where idPracownika='{0}'", id);
            
            try
            {
                using (SqlCommand com = new SqlCommand(usunP, connection))
                {
                    com.ExecuteNonQuery();
                }

               connection.Close();
            }

                catch (SqlException ex)
                {
                    Exception error = new Exception("Przepraszam! Nie ma usługi o tym id ", ex);
                }
        
        }

        public void modyfikujStanowiskoPracownika(int id,string stanowisko)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            string modyfikuj = string.Format("Update Pracownicy Set stanowisko='{0}' where idPracownika='{1}'", stanowisko, id);
                    
            using (SqlCommand cmd = new SqlCommand(modyfikuj, connection))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Przepraszam! Nie można wprowadzić modyfikacji ", ex);
                }
            }
        }

        public void modyfikujWynagrodzeniePracownika(int id, int wynagrodzenie)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            string modyfikuj = string.Format("Update Pracownicy Set wynagrodzenie='{0}' where idPracownika='{1}'", wynagrodzenie, id);

            using (SqlCommand cmd = new SqlCommand(modyfikuj, connection))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Przepraszam! Nie można wprowadzić modyfikacji ", ex);
                }
            }
        }
    }
}
