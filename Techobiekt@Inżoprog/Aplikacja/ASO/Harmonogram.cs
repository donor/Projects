using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASO
{
    class Harmonogram  
    {
      public bool jestDzien=true;
      //  public int wynikLicz=0;

       private string connectionString = ConfigurationManager.AppSettings["connectionString"];

        public void usunByleDni()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            string usun = string.Format("Delete from Harmonogram where data<'{0}'", DateTime.Now.Date.AddDays(-3));

            using (SqlCommand com = new SqlCommand(usun, connection))
            {
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Przepraszam! Nie ma usługi o tym id ", ex);
                }
            }
        }

        public int czyJest4PracownikowWDniu(int idDnia)
        {
           int wynikLicz = 0;


            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

               string count = string.Format("SELECT COUNT(idDnia) FROM Harmonogram_Pracownicy WHERE (idDnia = {0})",idDnia);
               string stan = string.Format("UPDATE Harmonogram SET Stan ='True' Where Harmonogram.idDnia={0}",idDnia);

                using (SqlCommand licz=new SqlCommand(count,connection))
                {
                      wynikLicz = Convert.ToInt32(licz.ExecuteScalar());
                }

                if (wynikLicz == 4)
                {
                    using (SqlCommand cmd = new SqlCommand(stan, connection))
                    {



                        cmd.ExecuteNonQuery();


                    }

                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return wynikLicz;

        }

        public void dodajPracownikaDoDnia(int idPracownika, int idDnia)
        {
           
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into Harmonogram_Pracownicy" + "(idDnia,idPracownika) Values" + "(@idDnia,@idPracownika)");
       


                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idDnia";
                    param.Value = idDnia;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@idPracownika";
                    param.Value = idPracownika;
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

        public void czyDzienJestwHarmonogramie(int i)
        {

            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string sel = string.Format("SELECT data FROM dbo.Harmonogram");

                //SqlCommand cmd = new SqlCommand("SELECT ISNULL(login, '') AS login, ISNULL(haslo,'') AS haslo FROM Pracownicy WHERE login='" + userText + "' and haslo='" + passText + "'", connection);

                SqlCommand cmd = new SqlCommand(sel, connection);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["data"].ToString().Remove(10) == DateTime.Now.AddDays(i).ToShortDateString())
                    {
                        jestDzien = true;
                        return;

                    }
                    
                        jestDzien = false;
               }





                    dr.Close(); 

                connection.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void dodajDniDoHarmonogramu(int i)
        {

            int numer0;
            
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                string wstawka = string.Format("Insert Into Harmonogram" + "(idDnia,data,Stan) Values" + "(@idDnia,@data,@Stan)");
                string count = string.Format("SELECT MAX(idDnia) FROM Harmonogram");
            
                using (SqlCommand licznik = new SqlCommand(count, connection))
                {

                    numer0 = Convert.ToInt32(licznik.ExecuteScalar());
                }
                numer0++;

                using (SqlCommand cmd = new SqlCommand(wstawka, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@idDnia";
                    param.Value = numer0;
                    param.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@data";
                    param.Value = DateTime.Now.AddDays(i);
                    param.SqlDbType = SqlDbType.Date;
                    param.Size = 50;
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.ParameterName = "@Stan";
                    param.Value = "False";
                    param.SqlDbType = SqlDbType.Bit;
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

    }
}
