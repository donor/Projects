using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class Zlecenie
    {
      
        public string pracownikDane;
        public string klientDane;
        private int idZleceniaKolejne;
        private int cenaZlecenia;

        string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public void pracownikWykonujacyIKlientSzczegoly(int idZlecenia)
        {

            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                //wypisanie pracownika realizującego zlecenie
                string zapytanie=string.Format("SELECT dbo.Osoby.imie, dbo.Osoby.nazwisko, dbo.Pracownicy.stanowisko FROM Pracownicy, Zlecenia, Osoby WHERE (Zlecenia.idPracownika=Pracownicy.idPracownika)and(Pracownicy.idOsoby=Osoby.idOsoby)and(Zlecenia.idZlecenia={0})",idZlecenia);
                string zapytanie1 = string.Format("SELECT dbo.Osoby.imie, dbo.Osoby.nazwisko FROM dbo.Klienci INNER JOIN dbo.Zlecenia ON dbo.Klienci.idKlienta = dbo.Zlecenia.idKlienta INNER JOIN dbo.Osoby ON dbo.Klienci.idOsoby = dbo.Osoby.idOsoby WHERE (dbo.Zlecenia.idZlecenia ={0})", idZlecenia);

                SqlCommand cmd = new SqlCommand(zapytanie, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();
                    pracownikDane = string.Format("{0} {1} stanowisko: {2}", dr[0].ToString(), dr[1].ToString(),dr[2].ToString());
               
                dr.Close();

                SqlCommand cmd1 = new SqlCommand(zapytanie1, connection);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                dr1.Read();
                klientDane = string.Format("{0} {1}", dr1[0].ToString(), dr1[1].ToString());

                dr1.Close();

                connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public int kolejnyIdZlecenia()
        {

            idZleceniaKolejne = 0;
            
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string count = string.Format("SELECT MAX(idZlecenia) FROM zlecenia");
            
                using (SqlCommand licznik = new SqlCommand(count, connection))
                {

                    idZleceniaKolejne = Convert.ToInt32(licznik.ExecuteScalar());
                }
                idZleceniaKolejne++;

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return idZleceniaKolejne;
        }

        public void dodajZlecenie(int idZlecenia, int idPracownika, int idKlienta, int nrFaktury)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into Zlecenia" + "(idZlecenia,idPracownika,idKlienta,data,nrFaktury) Values" + "(@idZlecenia,@idPracownika,@idKlienta,@data, @nrFaktury)");
            
                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idZlecenia";
                    param.Value = idZlecenia;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idPracownika";
                    param.Value = idPracownika;
                    param.SqlDbType = SqlDbType.Int;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idKlienta";
                    param.Value = idKlienta;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@data";
                    param.Value = DateTime.Now.Date;
                    param.SqlDbType = SqlDbType.Date;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@nrFaktury";
                    param.Value = nrFaktury;
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

        public void dodajUslugeDoZlecenia(int idUslugi,int idZlecenia)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into Uslugi_Zlecenia" + "(idUslugi,idZlecenia) Values" + "(@idUslugi,@idZlecenia)");
        
                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idUslugi";
                    param.Value = idUslugi;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idZlecenia";
                    param.Value = idZlecenia;
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

        public int wyznaczCeneZlecenia(int idZlecenia)
        {
           cenaZlecenia= 0;


            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string count = string.Format("SELECT SUM(dbo.Uslugi.cena) FROM dbo.Uslugi_Zlecenia INNER JOIN dbo.Uslugi ON dbo.Uslugi_Zlecenia.idUslugi = dbo.Uslugi.idUslugi INNER JOIN dbo.Zlecenia ON dbo.Uslugi_Zlecenia.idZlecenia = dbo.Zlecenia.idZlecenia WHERE (dbo.Zlecenia.idZlecenia = '{0}')",idZlecenia);
        
                using (SqlCommand lcena = new SqlCommand(count, connection))
                {

                    cenaZlecenia = Convert.ToInt32(lcena.ExecuteScalar());
                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return cenaZlecenia;
           
        }

        public void wstawCeneZlecenia(int cenaZlecenia,int idZlecenia )
        {
         

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawCene = string.Format("UPDATE Zlecenia SET cena ={1} WHERE idZlecenia = {0}", idZlecenia, cenaZlecenia);

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
    }
}
