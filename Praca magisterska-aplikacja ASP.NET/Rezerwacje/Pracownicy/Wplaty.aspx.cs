using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Rezerwacje.Logika;
using Rezerwacje.Data_Access;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.IO;
using System.Web.Profile;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;


namespace Rezerwacje.Pracownicy
{
    public partial class Wplaty : System.Web.UI.Page
    {

        private decimal suma = 0;


        protected void Button1_Click(object sender, EventArgs e)
        {
            tbCena.Visible = true;
            suma = 0;
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("chk");
                        if (cb != null && cb.Checked)
                        {

                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = true;
                            suma += (decimal)zmiana.Cena;
                            rdc.SubmitChanges();
                        }
                        if (cb != null && !cb.Checked)
                        {
                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = false;

                            rdc.SubmitChanges();
                        }
                    }
                }
                catch { }
            }
            tbCena.Text = "Do zapłaty: " + suma;
            tbCena2.Text = "Cena biletów: " + suma;


            GridView1.Visible = true;
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("chk");
                        if (cb != null && cb.Checked)
                        {

                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = true;

                            rdc.SubmitChanges();
                        }
                        if (cb != null && !cb.Checked)
                        {
                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = false;

                            rdc.SubmitChanges();
                        }
                    }
                }
                catch { }
            }

            Panel1.Visible = true;
            Session["ctrl1"] = Panel1;
            Control ctrl1 = (Control)Session["ctrl1"];
            PrintWebControl(ctrl1);



        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            tbCena.Text = "Do zapłaty: 0";
            tbCena2.Text = "Cena biletów: 0";
            suma = 0;
            tbCena.Visible = true;
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {

                        CheckBox cb = (CheckBox)row.FindControl("chk");
                        if (cb != null && cb.Checked)
                        {

                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = true;
                            suma += (decimal)zmiana.Cena;
                            rdc.SubmitChanges();
                        }
                        if (cb != null && !cb.Checked)
                        {
                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = false;

                            rdc.SubmitChanges();
                        }
                    }
                }
                catch { }
            }

            Button1.Visible = true;
            tbCena.Text = "Do zapłaty: " + suma;
            tbCena2.Text = "Cena biletów: " + suma;


        }

        protected void btnIdZamowienia_Click(object sender, EventArgs e)
        {
            tbCena.Visible = false;
            tbCena.Text = "Do zapłaty: 0";
            tbCena2.Text = "Cena biletów: 0";
            GridView1.Visible = true;
            suma = 0;
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("chk");
                        if (cb != null && cb.Checked)
                        {

                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = true;
                            suma += (decimal)zmiana.Cena;
                            rdc.SubmitChanges();
                        }
                        if (cb != null && !cb.Checked)
                        {
                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = false;

                            rdc.SubmitChanges();
                        }
                    }
                }
                catch { }

            }

            Button1.Visible = true;
            tbCena.Visible = false;
            tbCena.Text = "Do zapłaty: " + suma;
            tbCena2.Text = "Cena biletów: 0";



        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            tbCena.Visible = true;
            suma = 0;
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("chk");
                        if (cb != null && cb.Checked)
                        {

                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = true;
                            suma += (decimal)zmiana.Cena;
                            rdc.SubmitChanges();
                         
                        }
                        if (cb != null && !cb.Checked)
                        {
                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = false;

                            rdc.SubmitChanges();
                        }
                    }
                }
                catch { }
            }
            tbCena.Text = "Do zapłaty: " + suma;
            tbCena2.Text = "Cena biletów: " + suma;

        }

        protected void tbIdZamowienia_TextChanged(object sender, EventArgs e)
        {

          



            tbCena.Text = "Do zapłaty: 0";
            tbCena2.Text = "Cena biletów: 0";
            tbCena.Visible = false;
            GridView1.Visible = true;
            suma = 0;
            tbCena.Text = "Do zapłaty: " + suma;
            tbCena2.Text = "Cena biletów: " + suma;
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        
                        CheckBox cb = (CheckBox)row.FindControl("chk");
                        if (cb != null && cb.Checked)
                        {

                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = true;
                            suma += (decimal)zmiana.Cena;
                            rdc.SubmitChanges();
                        
                        }
                        if (cb != null && !cb.Checked)
                        {
                            int ZlecenieSzczegoly = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);
                            var zmiana = (from c in rdc.ZlecenieSzczegolies where c.SzczegolyZleceniaID == ZlecenieSzczegoly select c).First();
                            zmiana.Zaplacono = false;

                            rdc.SubmitChanges();
                        }
                    }
                }
                catch { }
            }


            Button1.Visible = true;

            tbCena.Text = "Do zapłaty: " + suma;
            tbCena2.Text = "Cena biletów: " + suma;

        }
    



        public void PrintWebControl(Control ControlToPrint)
        {

            StringWriter stringWrite = new StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            if (ControlToPrint is WebControl)
            {
                Unit w = new Unit(100, UnitType.Percentage);
                ((WebControl)ControlToPrint).Width = w;

            }


            Page pg = new Page();

            pg.EnableEventValidation = false;
            HtmlForm frm = new HtmlForm();
            pg.Controls.Add(frm);
            frm.Attributes.Add("runat", "server");
            frm.Controls.Add(ControlToPrint);
            pg.DesignerInitialize();
            pg.RenderControl(htmlWrite);


            string strHTML = stringWrite.ToString();
            string wstawka = Server.MapPath("~").ToString();
            



            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(htmlToImage(strHTML, zmianaAdresu(wstawka)));
          
            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();


            Response.Redirect("~/ListaImprez.aspx");


        }



        public static string htmlToImage(string wejscie, string wstawka)
        {
            for (int i = 0; i <= wejscie.Length - 2; i++)
            {
                if ((wejscie[i] == 's') && (wejscie[i + 1] == 'r') && (wejscie[i + 2] == 'c'))
                    wejscie = wejscie.Insert(i + 5, wstawka);

                if (wejscie[i] == '~')
                    wejscie = wejscie.Remove(i, 2);
            }
            return wejscie;
        }

        public static string zmianaAdresu(string adres)
        {
            adres = adres.Insert(adres.Length, "/");
            adres = adres.Replace('\\', '/');
            return adres;
        }
    



        //---------------------------------------------------------------------------------------------------------


        
        }

      
    }

