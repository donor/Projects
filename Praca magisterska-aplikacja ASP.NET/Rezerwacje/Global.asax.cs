using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Profile;
//using Rezerwacje.Logika;
using Rezerwacje.Data_Access;
using Rezerwacje.Logika;

namespace Rezerwacje
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

            Session[Cart.Ident] = null;
            Session[Cart.ZamowienieId] = null;
            Session[CheckOut.Potwierdzenie] = 0;
        


      
         }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {


            if ((int)Session[CheckOut.Potwierdzenie]!=1)
            {
                
                using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
                {
                    var wszystkieZlecenia = (from item in rdc.ZlecenieSzczegolies where item.ZamowienieID == (int)Session[Cart.ZamowienieId] select item);
                    var wszystkieImprezy=(from impr in rdc.Imprezies select impr);

                    try
                    {
                        foreach (ZlecenieSzczegoly item in wszystkieZlecenia)
                        {
                            foreach (Imprezy impreza in wszystkieImprezy)
                            {
                                if (item.ImprezaID == impreza.ImprezaID)
                                {
                                    impreza.SprzedaneBilety -= 1;
                                    rdc.SubmitChanges();
                                }
                            }

                        }

                        IEnumerable<ZlecenieSzczegoly> doUsuniecia = from c in rdc.ZlecenieSzczegolies where c.ZamowienieID == (int)Session[Cart.ZamowienieId] select c;

                        rdc.ZlecenieSzczegolies.DeleteAllOnSubmit(doUsuniecia);
                        var zamowienie = (from zam in rdc.Zamowienias where zam.ZamowienieID == (int)Session[Cart.ZamowienieId] select zam).First();
                        rdc.Zamowienias.DeleteOnSubmit(zamowienie);
                    }
                    catch (Exception exp)
                    { }
                }
                

            }
            Session[Cart.Ident] = null;
            Session[Cart.ZamowienieId] = null;
        }

        protected void Application_End(object sender, EventArgs e)
        {
          
        }
       
       
         
    }
}