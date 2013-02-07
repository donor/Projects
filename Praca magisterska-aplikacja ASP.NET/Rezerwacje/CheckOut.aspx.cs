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
using WebsitesScreenshot.SupportClasses ;  //biblioteka zewnetrzna, służąca do generowania jpg
using System.Web.Profile;

namespace Rezerwacje
{
    public partial class CheckOut : System.Web.UI.Page
    {
      

       public const string Potwierdzenie = "kontrola";
     
           
         protected void Page_Load(object sender, EventArgs e)
         {
            if (Session[Potwierdzenie] == null)
              Session[Potwierdzenie] = 0;


             tdImie.InnerText = (string)HttpContext.Current.Profile.GetPropertyValue("Imie");
             tdEmail.InnerText = Membership.GetUser().Email;
             tdNazwisko.InnerText = (string)HttpContext.Current.Profile.GetPropertyValue("Nazwisko");
             tdUlica.InnerText = (string)HttpContext.Current.Profile.GetPropertyValue("Ulica");
             tdNrDomu.InnerText= (string)HttpContext.Current.Profile.GetPropertyValue("NrDomu");
             tdKod.InnerText= (string)HttpContext.Current.Profile.GetPropertyValue("KodPocztowy");
             tdMiejscowosc.InnerText= (string)HttpContext.Current.Profile.GetPropertyValue("Miejscowosc");        

          }
           
        

        protected void btnAkceptuj_Click(object sender, EventArgs e)
        {            
            Session[Cart.Ident] = null;
            Session[Cart.ZamowienieId] = null;
            btnAkceptuj.Visible = false;
            UkryjKontrolkeWListView("SelectButton");
            btnWysliMail.Visible = false;
            Label1.Visible = false;

            Session["ctrl"] = Panel1;
            Control ctrl = (Control)Session["ctrl"];
            PrintWebControl(ctrl);

            Session[Potwierdzenie] = 1;
        }

       

        public  void PrintWebControl(Control ControlToPrint)
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
            string path = String.Format("{0}\\Images\\Rezerwacje\\{1}.gif", Server.MapPath("~"), Session["ZamowienieId"]);

            //korzystanie z biblioteki websiteScreenshote

            WebsitesScreenshot.WebsitesScreenshot _Obj = new WebsitesScreenshot.WebsitesScreenshot();
            WebsitesScreenshot.WebsitesScreenshot.Result _Result = _Obj.CaptureHTML(htmlToImage(strHTML, zmianaAdresu(wstawka)));

            if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Captured)
            {
                _Obj.ImageFormat = WebsitesScreenshot.WebsitesScreenshot.ImageFormats.GIF;
                _Obj.SaveImage(path);
            }
            _Obj.Dispose();

           

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(strHTML);
            Session[Cart.Ident] = null;
            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();
            

            Response.Redirect("~/ListaImprez.aspx");

            
        }

        protected void btnWysliMail_Click(object sender, EventArgs e)
        {

            UkryjKontrolkeWListView("SelectButton");
            btnAkceptuj.Visible = false;
            btnWysliMail.Visible = false;
            Label1.Visible = false;
           

            string path = String.Format("{0}\\Images\\Rezerwacje\\{1}.gif", Server.MapPath("~"), Session["ZamowienieId"]);
           
            //----------------------wstawka----------------------------------- --------------------
            btnAkceptuj.Visible = false;

            Session["ctrl"] = Panel1;
            Control ctrl = (Control)Session["ctrl"];
        


            StringWriter stringWrite = new StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
            if (ctrl is WebControl)
            {
                Unit w = new Unit(100, UnitType.Percentage);
                ((WebControl)ctrl).Width = w;

            }


            Page pg = new Page();

            pg.EnableEventValidation = false;
            HtmlForm frm = new HtmlForm();
            pg.Controls.Add(frm);
            frm.Attributes.Add("runat", "server");
            frm.Controls.Add(ctrl);
            pg.DesignerInitialize();
            pg.RenderControl(htmlWrite);

         

            string strHTML = stringWrite.ToString();
            string wstawka = Server.MapPath("~").ToString();

            //korzystanie z biblioteki websiteScreenshote

            WebsitesScreenshot.WebsitesScreenshot _Obj = new WebsitesScreenshot.WebsitesScreenshot();
            WebsitesScreenshot.WebsitesScreenshot.Result _Result = _Obj.CaptureHTML(htmlToImage(strHTML, zmianaAdresu(wstawka)));

            if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Captured)
            {
                _Obj.ImageFormat = WebsitesScreenshot.WebsitesScreenshot.ImageFormats.GIF;
                _Obj.SaveImage(path);
            }
            _Obj.Dispose();

         
  

            //----------------------wstawka----------------------------------- -----------------------------------------------


            StringBuilder emailMessage = new StringBuilder();
            emailMessage.Append("Przesyłamy twoje Rezerwacje w załączniku.");
            emailMessage.Append("<br />");
            emailMessage.Append("Wydrukuj plik gif, aby okazać go przy wejściu na impreze");

            MailMessage email = new MailMessage();
            email.From = new MailAddress("serwisrezerwacje@gmail.com");          
            email.To.Add(new MailAddress(tdEmail.InnerText));
            email.Subject = "Twoje rezerwacje";
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;
            email.Attachments.Add(new System.Net.Mail.Attachment(path));


            SmtpClient client = new SmtpClient();


            
            try
            {
                client.Send(email);
              
            }
            catch (Exception ex)
            {
                
          
            }
            finally
            {
                Session[Cart.Ident] = null;
                Session[Cart.ZamowienieId] = null;
                Session[Potwierdzenie] = 1;
                Response.Redirect("~/RezerwacjeWyslane.aspx");           
                
            }



        }


//----------------------metody zmieniające string html w związku z  ddodaniem pełnego adresy kodu kreskowego
        public static string htmlToImage(string wejscie, string wstawka)
        {
            for (int i = 0; i <= wejscie.Length - 2; i++)
            {
                if ((wejscie[i] == 's') && (wejscie[i + 1] == 'r') && (wejscie[i + 2] == 'c'))
                    wejscie = wejscie.Insert(i + 5, wstawka);
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


       

        protected void ListViewRezerwacje_SelectedIndexChanged(object sender, EventArgs e)
        {
            int szczegolyZlecenieId = System.Int32.Parse(ListViewRezerwacje.SelectedDataKey.Value.ToString());

            Cart k = new Cart();
            k.usunElement(szczegolyZlecenieId);
            DetailsView1.DataBind();
        }

        public void UkryjKontrolkeWListView(string nazwaPola)
        {
           
            ImageButton ib = null;
            ListViewItem Item = null;
            foreach (ListViewDataItem item in ListViewRezerwacje.Items)
            {
                Item = item;
                ib = ((ImageButton)(Item.FindControl(nazwaPola)));
                if ((ib != null) )
                {
                    ib.Visible = false;
                }
            }

       
        }

       
           
     

    }
}