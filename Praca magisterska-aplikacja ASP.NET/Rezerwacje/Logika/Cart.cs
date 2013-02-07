using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Rezerwacje.Data_Access;
using Rezerwacje.Logika;





namespace Rezerwacje
{

   
       
    public partial class Cart
    {
       
         public const string Ident  = "Identyfikator";
         public  const string ZamowienieId = "zamowienieId";
          
      
        


        public String getIdentyfikator()
        {
          

            if (HttpContext.Current.Session[Ident]==null)
            {
                HttpContext.Current.Session[Ident] = System.Web.HttpContext.Current.Request.IsAuthenticated ? HttpContext.Current.User.Identity.Name : Guid.NewGuid().ToString();

            }
            return HttpContext.Current.Session[Ident].ToString();
        }

        public String getZamowienieID(int zamowId)
        {
            HttpContext.Current.Session[ZamowienieId] = zamowId.ToString();
            return HttpContext.Current.Session[ZamowienieId].ToString();
        }
           


        public void insertToCart(String identyfikator, int imprezaId, string typKlienta, int ilosc)
        {
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    Koszyk k = new Koszyk();
                    k.Identyfikator = identyfikator;
                    k.ImprezaID = imprezaId;
                    k.DataUtworzenia = DateTime.Now;
                    k.Ilosc = ilosc;
                    k.TypKlienta = typKlienta;
                    rdc.Koszyks.InsertOnSubmit(k);
                    rdc.SubmitChanges();
                                        
                }
                catch (Exception exp)
                {
                    throw new Exception("Błąd: Nie można dodać imprezy do Koszyka - " + exp.Message.ToString(), exp);
                }
            }
        }

     


        public bool acceptOrder(string user)
        {
            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                   
                    int zamow=0;
                  
                    bool dalej = false;
                   
                    if (HttpContext.Current.Session[ZamowienieId] == null)
                    {
                        Zamowienia z = new Zamowienia();
                        z.Klient = user;
                        z.DataZamowienia = DateTime.Now;
                        rdc.Zamowienias.InsertOnSubmit(z);
                        rdc.SubmitChanges();
                        zamow=z.ZamowienieID;
                        dalej = true;

                    }

                    if (dalej)
                        HttpContext.Current.Session[ZamowienieId] = zamow;
                    
                    IEnumerable<ZlecenieSzczegoly> doUsuniecia = from c in rdc.ZlecenieSzczegolies where c.ZamowienieID == (int)HttpContext.Current.Session[ZamowienieId] select c;

                    rdc.ZlecenieSzczegolies.DeleteAllOnSubmit(doUsuniecia);
                    

                    String ident = getIdentyfikator();

                    var cart = (from c in rdc.WidokCenaKoszykas where c.Identyfikator == ident select c);
                 
                                     
                    int numer = -1;

                    foreach (WidokCenaKoszyka impr in cart)
                    {
                        

                        for (int i = 1; i <= impr.Ilosc;i++ )
                        {
                            numer++;
                            string barCod = String.Format("{0}-{1}", HttpContext.Current.Session[ZamowienieId].ToString(), numer.ToString());
                            ZlecenieSzczegoly zs = new ZlecenieSzczegoly();
                            zs.ZamowienieID = (int)HttpContext.Current.Session[ZamowienieId];
                            zs.ImprezaID = impr.ImprezaID;
                            zs.Zaplacono = false;

                            if (impr.TypKlienta == "Normalny")
                                { zs.Cena = impr.CenaPodstawowa; zs.TypKlienta = "Normalny"; }
                            if (impr.TypKlienta == "Studencki")
                                {zs.Cena = impr.CenaStudent; zs.TypKlienta="Studencki";}  
                            if (impr.TypKlienta == "Ulgowy")
                                {zs.Cena = impr.CenaUlgowa; zs.TypKlienta = "Ulgowy"; }  

                            zs.Kod = barCod;
                            rdc.ZlecenieSzczegolies.InsertOnSubmit(zs);

                            string path = String.Format("{0}\\Images\\KodyKreskowe\\{1}.jpg", HttpContext.Current.Server.MapPath("~"), barCod);
                            BarCodeCreater bcg = new BarCodeCreater();
                            Bitmap barCode = bcg.generateBarcode(barCod);
                            barCode.Save(path, ImageFormat.Jpeg);
                            barCode.Dispose();

                        }
                       
                     
                    }

                    rdc.SubmitChanges();
                }
                catch (Exception exp)
                {
                    throw new Exception("Błąd: nie można zaakceptować zamówienia - " + exp.Message.ToString(), exp);
                }


            }


            return true;
        }

        public void usunElement(int szczegolyZleceniaId)
        {

            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                try
                {
                    var elementdoUsyniecia = (from szczegol in rdc.ZlecenieSzczegolies where (szczegol.SzczegolyZleceniaID == szczegolyZleceniaId) select szczegol).First();

                    var usunzKoszyka = (from element in rdc.Koszyks where ((element.ImprezaID == elementdoUsyniecia.ImprezaID) && (element.Identyfikator == HttpContext.Current.Session[Ident]) && (element.TypKlienta == elementdoUsyniecia.TypKlienta)) select element).First();

                    var aktalizujImpreze = (from impreza in rdc.Imprezies where (impreza.ImprezaID == elementdoUsyniecia.ImprezaID) select impreza).First();

                    if (usunzKoszyka.Ilosc == 1)
                    {
                        rdc.Koszyks.DeleteOnSubmit(usunzKoszyka);
                    }
                    if (usunzKoszyka.Ilosc > 1)
                    {
                        usunzKoszyka.Ilosc -= 1;

                    }

                    aktalizujImpreze.SprzedaneBilety -= 1;

                    rdc.ZlecenieSzczegolies.DeleteOnSubmit(elementdoUsyniecia);
                    rdc.SubmitChanges();
                }
                catch { }
            }



        }
        
        
        
        
   
    }
}


