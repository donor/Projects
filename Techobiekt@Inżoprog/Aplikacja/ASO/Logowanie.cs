using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    
    public class Logowanie
    {

       // public string userText;
       // public string passText;
        public bool blad = false;

        public int idPracownika;

        public Logowanie()
        {
        }


        //public Logowanie(string lLogin, string lHaslo)
        //{
        //    userText = lLogin;
        //    passText = lHaslo;
        //}

        
        private bool Porownaj(string string1, string string2)
        {
            return String.Compare(string1, string2, true, System.Globalization.CultureInfo.InvariantCulture) == 0 ? true : false;
        }

        public void Zaloguj(string userText, string passText)
        {

            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                //userText = "start";
                //passText = "stop";

                string sel = string.Format("SELECT Pracownicy.idPracownika, Pracownicy.login, Pracownicy.haslo, Pracownicy.stanowisko, Pracownicy.stanowisko, Osoby.imie, Osoby.nazwisko FROM dbo.Osoby, dbo.Pracownicy WHERE (dbo.Osoby.idOsoby = dbo.Pracownicy.idOsoby)and (Pracownicy.login='{0}')and(Pracownicy.haslo='{1}')", userText, passText);

                SqlCommand cmd = new SqlCommand(sel, connection);           
                SqlDataReader dr = cmd.ExecuteReader();

                while ((dr.Read()) && (!blad))
                {
                    if (this.Porownaj(dr["login"].ToString(), userText) && this.Porownaj(dr["haslo"].ToString(), passText))
                    {
                        blad = true;
                        string dane = string.Format("Witaj {0} {1} jestes {2}", dr["imie"], dr["nazwisko"], dr["stanowisko"]);
                        string idPr = dr["idPracownika"].ToString();
                        idPracownika =int.Parse(idPr);

                        MessageBox.Show(dane);
                    }
                }

                if (!blad)
                {
                    MessageBox.Show("Podałeś zły login i/lub hasło");
                    Application.ExitThread();
                }

                dr.Close();
              
               connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }                          
    }
}
