using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class Usluga
    { 
        private string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public void dodajUsluge(string opisUslugi, int cena, int idKategorii)
        {
          int numer0;

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into Uslugi" + "(idUslugi,opisUslugi,cena, idKategorii) Values" + "(@idUslugi,@opisUslugi,@cena, @idKategorii)");
                string count = string.Format("SELECT MAX(idUslugi) FROM Uslugi");
         
                using (SqlCommand licznik = new SqlCommand(count, connection))
                {

                    numer0 = Convert.ToInt32(licznik.ExecuteScalar());
                }
                 numer0++;

                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idUslugi";
                    param.Value = numer0;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@opisUslugi";
                    param.Value = opisUslugi;
                    param.SqlDbType = SqlDbType.Char;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@cena";
                    param.Value = cena;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idKategorii";
                    param.Value = idKategorii;
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

        public void usunUsluge(int id)
        {

             SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
            string usun = string.Format("Delete from Uslugi where idUslugi='{0}'",id);

            using (SqlCommand com = new SqlCommand(usun, connection))
            {
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Przepraszam! Nie ma usługi o tym id ",ex);
                }
            }
        }

        public void modyfikujUsluge(int id, string opisUslugi, int cena, int idKategorii)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            string modyfikuj= string.Format("Update Uslugi Set opisUslugi='{0}', cena='{1}',idKategorii='{2}' where idUslugi='{3}'",opisUslugi,cena, idKategorii, id);

            //tylko zmiana ceny
            if ((opisUslugi=="")&&(cena!=-1))
                modyfikuj = string.Format("Update Uslugi Set cena='{0}',idKategorii='{1}' where idUslugi='{2}'",cena, idKategorii, id);
            if ((cena==-1)&&(opisUslugi!=""))
                modyfikuj = string.Format("Update Uslugi Set opisUslugi='{0}',idKategorii='{1}' where idUslugi='{2}'", opisUslugi, idKategorii, id);
            if ((cena == -1) && (opisUslugi == ""))
                return; 

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
