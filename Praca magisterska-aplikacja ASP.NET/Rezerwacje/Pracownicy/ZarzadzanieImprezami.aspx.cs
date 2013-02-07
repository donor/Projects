using System;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Rezerwacje.Data_Access;
using System.Threading;
using System.Collections.Generic;

namespace Rezerwacje.Pracownicy
{
    public partial class ZarzadzanieImprezami : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Button1.Enabled = false;
            Panel1.Visible = true;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            PopupControlExtender1.Commit(Calendar1.SelectedDate.ToShortDateString());
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string dataGodzina = textBoxData.Text + " " + textBoxGodzina.Text;
            string miejsce = TextBoxUlica.Text + " " + TextBoxNrdomu.Text + " " + TextBoxKod.Text + " " + TextBoxMiejscowosc.Text;
            decimal cena = 0;
            decimal cenaS = 0;
            decimal cenaU = 0;
            int liczbaBiletow = 0;

            if (!String.IsNullOrWhiteSpace(TextBoxCenaP.Text))
                cena =  Decimal.Parse(TextBoxCenaP.Text);
            if (!String.IsNullOrWhiteSpace(TextBoxCenaS.Text))
               cenaS=Decimal.Parse(TextBoxCenaS.Text);
            if (!String.IsNullOrWhiteSpace(TextBoxCenaU.Text))
                cenaU = Decimal.Parse(TextBoxCenaU.Text);
            if (!String.IsNullOrWhiteSpace(TextBoxLiczbaBiletow.Text))
                liczbaBiletow = Int32.Parse(TextBoxLiczbaBiletow.Text);


            if (FileUpload1.HasFile)
            {
                try
                {

                string path = String.Format("{0}\\Images\\ListaImprez\\{1}.gif", Server.MapPath("~"), textBoxNazwa.Text + textBoxData.Text);
                FileUpload1.SaveAs(path);
                }
                catch (Exception exp)
                {}
            }

            insertImpreza(textBoxNazwa.Text, dataGodzina, miejsce, cena,liczbaBiletow,  TextBoxSzczegoly.Text, cenaS, cenaU);
        
            Response.Redirect("~/Pracownicy/ZarzadzanieImprezami.aspx", true);

        }

        public void insertImpreza( string nazwa, string data, string miejsce, decimal cenaPodstawowa,int liczbaBiletow, string szczegoly, decimal cenaStudent, decimal cenaUlga)
        {

            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    

                    Imprezy i = new Imprezy();
                    i.Nazwa = nazwa;
                    i.Data = DateTime.Parse(data);
                    i.Miejsce = miejsce;
                    i.CenaPodstawowa = cenaPodstawowa;
                    i.CenaStudent = cenaStudent;
                    i.CenaUlgowa = cenaUlga;
                    i.LiczbaBiletow = liczbaBiletow;
                    i.SprzedaneBilety = 0;
                    if (!String.IsNullOrWhiteSpace(FileUpload1.FileName))
                        i.Obrazek = nazwa + textBoxData.Text + ".gif";
                   else
                     i.Obrazek = "brak_zdj.gif";
                    i.Szczegoly = szczegoly;
                    rdc.Imprezies.InsertOnSubmit(i);
                    rdc.SubmitChanges();

                    

                }

                catch (Exception exp)
                {
                    throw new Exception("Błąd: Nie można dodać imprezy - " + exp.Message.ToString(), exp);
                }
            }

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int imprezaID = (int)GridView1.DataKeys[e.RowIndex].Value;

            RezerwacjeDataContext rdc = new RezerwacjeDataContext();

            try
            {


                IEnumerable<Koszyk> doSkasowania = from d in rdc.Koszyks where d.ImprezaID == imprezaID select d;

                rdc.Koszyks.DeleteAllOnSubmit(doSkasowania);
                rdc.SubmitChanges();

                

                rdc.Imprezies.DeleteOnSubmit((from c in rdc.Imprezies where c.ImprezaID == imprezaID select c).First());

                rdc.SubmitChanges();

           

            }

            catch (Exception exp)
            {
                throw new Exception("Błąd: Nie można usunąć imprezy - " + exp.Message.ToString(), exp);
            }
            finally
            {
                Response.Redirect("~/Pracownicy/ZarzadzanieImprezami.aspx", true);
            }
            




        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            Button1.Enabled = true;
        }

        
    }


}