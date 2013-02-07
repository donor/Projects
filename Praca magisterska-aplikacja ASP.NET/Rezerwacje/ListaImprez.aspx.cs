using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rezerwacje.Data_Access;

namespace Rezerwacje
{
    public partial class ListaImprez : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            tbData.Visible = false;

            tbData.Text = System.DateTime.Now.ToString();
           
        }

       

      

        protected void lbKoszyk_Click(object sender, EventArgs e)
        {
            Cart c = new Cart();
            if (c.acceptOrder(User.Identity.Name) == true)
            {
                
                Response.Redirect("CheckOut.aspx");
            }
           
        }

        public bool kontrolaIlosci(int imprID, int iloscB, RezerwacjeDataContext rdc)
        {
            bool wyjscie = true;
            var iloscBiletow = (from impreza in rdc.Imprezies where (impreza.ImprezaID == imprID) select impreza).First();

            if (iloscBiletow.LiczbaBiletow < (iloscBiletow.SprzedaneBilety + iloscB))
                wyjscie = false;

            if (wyjscie)
            {

                
                iloscBiletow.SprzedaneBilety += iloscB;

            }
            
            rdc.SubmitChanges();

            ListView_Products.DataBind();
            return wyjscie;
        }



        protected void lbDodaj_Click(object sender, EventArgs e)
        {
          
            int imprezaId = System.Int32.Parse(ListView_Products.SelectedDataKey.Value.ToString());


            Cart k = new Cart();
            String identyfikator = k.getIdentyfikator();

            using (RezerwacjeDataContext rdc = new RezerwacjeDataContext())
            {
                bool dalej = false;
                var listaKoszykow = rdc.Koszyks;
                var istniejeRekordN = (from koszyk in listaKoszykow where ((koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Normalny")) select koszyk).Count();

             
                if (istniejeRekordN == 0)
                {
                    if (GetTextBoxValuesFromListView("tbCenaP") > 0)
                    {
                        if (kontrolaIlosci(imprezaId, GetTextBoxValuesFromListView("tbCenaP"), rdc))
                        {

                            k.insertToCart(identyfikator, imprezaId, "Normalny", GetTextBoxValuesFromListView("tbCenaP"));
                            lblMsg.Text = "Bilety zostały dodane do koszyka";
                            dalej = true;
                        }
                        else
                            lblMsg.Text = "Nie ma już tylu biletów do sprzedania na tą Impreze";
                    }
                }

                var istniejeRekordS = (from koszyk in listaKoszykow where ((koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Studencki")) select koszyk).Count();
                if (istniejeRekordS == 0)
                {
                    if (GetTextBoxValuesFromListView("tbCenaS") > 0)
                    {
                        if (kontrolaIlosci(imprezaId, GetTextBoxValuesFromListView("tbCenaS"), rdc))
                        {

                            k.insertToCart(identyfikator, imprezaId, "Studencki", GetTextBoxValuesFromListView("tbCenaS"));
                            lblMsg.Text = "Bilety zostały dodane do koszyka";
                            dalej = true;
                        }
                        else
                            lblMsg.Text = "Nie ma już tylu biletów do sprzedania na tą Impreze";

                    }
                }

                var istniejeRekordU = (from koszyk in listaKoszykow where ((koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Ulgowy")) select koszyk).Count();
                if (istniejeRekordU == 0)
                {
                    if (GetTextBoxValuesFromListView("tbCenaU") > 0)
                    {
                        if (kontrolaIlosci(imprezaId, GetTextBoxValuesFromListView("tbCenaU"), rdc))
                        {
                            k.insertToCart(identyfikator, imprezaId, "Ulgowy", GetTextBoxValuesFromListView("tbCenaU"));
                            lblMsg.Text = "Bilety zostały dodane do koszyka";
                            dalej = true;
                        }
                        else
                            lblMsg.Text = "Nie ma już tylu biletów do sprzedania na tą Impreze";

                    }
                }


                if (!dalej)
                {
                    if ((istniejeRekordN > 0) || (istniejeRekordS > 0) || (istniejeRekordU > 0))
                    {
                        var edytujP = (from koszyk in listaKoszykow where ((GetTextBoxValuesFromListView("tbCenaP") > 0) && (koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Normalny")) select koszyk).Count();
                        var edytujS = (from koszyk in listaKoszykow where ((GetTextBoxValuesFromListView("tbCenaS") > 0) && (koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Studencki")) select koszyk).Count();
                        var edytujU = (from koszyk in listaKoszykow where ((GetTextBoxValuesFromListView("tbCenaU") > 0) && (koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Ulgowy")) select koszyk).Count();

                        if (edytujP > 0)
                        {
                            if (kontrolaIlosci(imprezaId, GetTextBoxValuesFromListView("tbCenaP"), rdc))
                            {
                                var edytujP1 = (from koszyk in listaKoszykow where ((koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Normalny")) select koszyk).First();

                                edytujP1.Ilosc += GetTextBoxValuesFromListView("tbCenaP");
                            }
                            else
                                lblMsg.Text = "Nie ma już tylu biletów do sprzedania na tą Impreze";
                        }

                        if (edytujS > 0)
                        {
                            if (kontrolaIlosci(imprezaId, GetTextBoxValuesFromListView("tbCenaS"), rdc))
                            {
                                var edytujS1 = (from koszyk in listaKoszykow where ((koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Studencki")) select koszyk).First();
                                edytujS1.Ilosc += GetTextBoxValuesFromListView("tbCenaS");
                            }
                            else
                                lblMsg.Text = "Nie ma już tylu biletów do sprzedania na tą Impreze";
                        }

                        if (edytujU > 0)
                        {
                            if (kontrolaIlosci(imprezaId, GetTextBoxValuesFromListView("tbCenaU"), rdc))
                            {
                                var edytujU1 = (from koszyk in listaKoszykow where ((koszyk.Identyfikator == identyfikator) && (koszyk.ImprezaID == imprezaId) && (koszyk.TypKlienta == "Ulgowy")) select koszyk).First();
                                edytujU1.Ilosc += GetTextBoxValuesFromListView("tbCenaU");
                            }
                            else
                                lblMsg.Text = "Nie ma już tylu biletów do sprzedania na tą Impreze";
                        }

                        rdc.SubmitChanges();
                    }
                }


            }
        }







        public  int GetTextBoxValuesFromListView(string nazwaPola)
        {
            int zawartosc = 0;
            TextBox tb = null;
            ListViewItem Item = null;
            foreach (ListViewDataItem item in ListView1.Items)
            {
                Item = item;
                tb = ((TextBox)(Item.FindControl(nazwaPola)));
                if ((tb.Text != null) && (!String.IsNullOrEmpty(tb.Text)))
                {
                    zawartosc = System.Int32.Parse(tb.Text);
                }
            }

            return zawartosc;
        }
         


  




    }
}