using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;


namespace ObslugaBazyDanychSQL
{
    class Updaty:Inserty
    {
        XmlDocument xml = new XmlDocument();
        public readonly static string fileName = "BazaDanych0.xml";
        

        public Updaty()
        {

        }

        public Updaty(List<string> zapytanieSql, string fileName)
        {
            Hashtable kolumny = new Hashtable();
            bool kontrolkaTable = false;
          
            xml.Load(fileName);

            XmlNode wezelGlowny = xml.DocumentElement.FirstChild;

            do
            {
                if ((wezelGlowny.Name.ToLower()) == (zapytanieSql.ElementAt(1)))
                {
                    kontrolkaTable = true;
                    break;
                }
                wezelGlowny = wezelGlowny.NextSibling;
            }
            while (wezelGlowny != null);

            if (kontrolkaTable == false)
            {
                new Error("Nie ma tabeli o takiej nazwie");
                return;
            }

            if (zapytanieSql.ElementAt(2) != "set")
            {
                new Error("Brak słowa 'set'");
                return;
            }




            XmlNodeList lst = wezelGlowny.FirstChild.ChildNodes;
            bool dalej0 = false;
            string kolumna0 = null;

            for (int l = 0; l < lst.Count; l++)
            {
                if ((lst[l].Name.ToLower() == zapytanieSql.ElementAt(3).ToLower()) && (zapytanieSql.ElementAt(4).ToLower() == "="))
                {
                     if (!typParametru(zapytanieSql.ElementAt(1),zapytanieSql.ElementAt(3),zapytanieSql.ElementAt(5)))
                                      return;
                     if (zapytanieSql.ElementAt(5)[0] == '\x0027')
                     {
                         zapytanieSql.Insert(5,zapytanieSql.ElementAt(5).Remove(0, 1));
                         zapytanieSql.RemoveAt(6);
                         zapytanieSql.Insert(5, zapytanieSql.ElementAt(5).Remove(zapytanieSql.ElementAt(5).Length - 1));
                         zapytanieSql.RemoveAt(6);

                     }
                    kolumny.Add(lst[l].Name.ToLower(), zapytanieSql.ElementAt(5));
                }
            }


            int index = 7;
            do
            {
                dalej0 = false;
                if ((zapytanieSql.ElementAt(index - 1).ToLower() == ","))
                {
                    for (int l = 0; l < lst.Count; l++)
                    {
                        if ((lst[l].Name.ToLower() == zapytanieSql.ElementAt(index).ToLower()) && (zapytanieSql.ElementAt(index + 1).ToLower() == "="))
                        {
                            if (!typParametru(zapytanieSql.ElementAt(1),zapytanieSql.ElementAt(index),zapytanieSql.ElementAt(index+2)))
                                      return;

                            try
                            {
                                if (zapytanieSql.ElementAt(index + 2)[0] == '\x0027')
                                {
                                    zapytanieSql.Insert(index + 2, zapytanieSql.ElementAt(index + 2).Remove(0, 1));
                                    zapytanieSql.RemoveAt(index + 3);
                                    zapytanieSql.Insert(index + 2, zapytanieSql.ElementAt(index + 2).Remove(zapytanieSql.ElementAt(index + 2).Length - 1));
                                    zapytanieSql.RemoveAt(index + 3);

                                }

                                kolumny.Add(lst[l].Name.ToLower(), zapytanieSql.ElementAt(index + 2));
                            }
                            catch (ArgumentException exp)
                            {
                                string msg = string.Format("Dla tej samej kolumny wprowadzono 2  parametr do aktualizacji");
                                new Error(msg);
                                return;
                            }
                            dalej0 = true;
                        }
                    }
                    if (!dalej0)
                    {
                        string msg = string.Format("Nie istnieja kolumna o nazwie {0} w tabeli {1} ",zapytanieSql.ElementAt(index),zapytanieSql.ElementAt(1));

                        new Error(msg);
                        return;
                    }
                }
                else if ((zapytanieSql.ElementAt(index-1) != "where") && (zapytanieSql.ElementAt(index-1) != ";"))
                {
                    new Error("Problem z znakiem ,  .");
                    return;


                }
                if (kolumny.Count > 1)
                    index += 4;
                else
                    break;
            }
            while ((index<zapytanieSql.Count)&&(zapytanieSql.ElementAt(index) != "where") && (zapytanieSql.ElementAt(index) != ";"));
          
            int temp = index;
            index =zapytanieSql.IndexOf("where");

            if ((zapytanieSql.ElementAt(temp-1) == ";")&&(index==-1))
            {
                wstawDoWszystkichRekordow(wezelGlowny.ChildNodes, kolumny);

            }
            else if (zapytanieSql.ElementAt(index) == "where")
            {
                //sprawdzenie czy kolumna z sekcji where istnieje
                string operat = zapytanieSql.ElementAt(index +2);
       
                kolumna0 = sprawdzKolumne(zapytanieSql.ElementAt(index+1), wezelGlowny.ChildNodes);
                if (kolumna0 == null)
                    return;

                string typKolumny0 = typKolumnyPar(zapytanieSql.ElementAt(1), xml, kolumna0);


                if ((operat != "=") && (operat != "<") && (operat != ">"))
                {
                    string msg = string.Format("problem z operatorem {0}", operat);
                    new Error(msg);
                    return;
                }

                string parametr = null;
                if ((operat == "="))//&&((zapytanieSql.ElementAt(6)!=""))
                {
                    parametr = sprawdzParametry(index + 3, typKolumny0, zapytanieSql, kolumna0);

                    if (parametr == null)
                        return;
                    wprowadzZmiany(0, parametr, kolumna0, wezelGlowny, kolumny);
                }
                if ((operat == "<") && ((zapytanieSql.ElementAt(index + 3) != ">") && (zapytanieSql.ElementAt(index + 3) != "=")))
                {
                    parametr = sprawdzParametry(index + 3, typKolumny0, zapytanieSql, kolumna0);

                    if (parametr == null)
                        return;
                    wprowadzZmiany(1, parametr, kolumna0, wezelGlowny, kolumny);
                }
                if ((operat == "<") && (zapytanieSql.ElementAt(index + 3) == ">"))
                {
                    parametr = sprawdzParametry(index + 4, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wprowadzZmiany(2, parametr, kolumna0, wezelGlowny, kolumny);
                }
                if ((operat == "<") && (zapytanieSql.ElementAt(index + 3) == "="))
                {
                    parametr = sprawdzParametry(index + 4, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wprowadzZmiany(3, parametr, kolumna0, wezelGlowny, kolumny);
                }
                if ((operat == ">") && (zapytanieSql.ElementAt(index + 3) != "="))
                {
                    parametr = sprawdzParametry(index + 3, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wprowadzZmiany(4, parametr, kolumna0, wezelGlowny, kolumny);
                }
                if ((operat == ">") && (zapytanieSql.ElementAt(index + 3) == "="))
                {
                    parametr = sprawdzParametry(index + 4, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wprowadzZmiany(5, parametr, kolumna0, wezelGlowny, kolumny);
                }

            }
            else
            {
                new Error("Błędna formuła zapytania");
                return;
            }
            

  
            }

        public void wprowadzZmiany(int wybor, string param, string kolumna0, XmlNode wezelGlowny, Hashtable kolumny)
        {
            ICollection klucze = kolumny.Keys;
            IDictionaryEnumerator Enumerator;
            int licznik = 0;
            int mnoz = 0;


            int licznikUsunietych = 0;
            bool usunieto = false;
            XmlNodeList xnList = wezelGlowny.ChildNodes;
            List<string> tempKolumny = null;

            switch (wybor)
            {
                case 0:              

                foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;

                    foreach (XmlNode xn1 in xnChild)
                    {
                        if ((xn1.InnerText.ToLower() == param) )
                        {
                             licznik += 1;
                             Enumerator = kolumny.GetEnumerator();       
                                foreach (string klucz in klucze)
                                     {
                                        Enumerator.MoveNext();
                                            foreach (XmlNode node in xn)
                                             {
                                              
                                            if (node.Name.ToLower() == Enumerator.Key.ToString())
                                                {
                                                  node.InnerText = Enumerator.Value.ToString();
                             
                                                 }
                                     }
                            }
                    }
                    }          
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik);
            xml.Save(fileName);                    
                    break;
                case 1:
                    
                foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;

                    foreach (XmlNode xn1 in xnChild)
                    {
                        if ((String.CompareOrdinal(xn1.InnerText.ToLower(), param) < 0) && (xn1.Name.ToLower() == kolumna0))
                        {
                             licznik += 1;
                             Enumerator = kolumny.GetEnumerator();       
                                foreach (string klucz in klucze)
                                     {
                                        Enumerator.MoveNext();
                                            foreach (XmlNode node in xn)
                                             {
                                              
                                            if (node.Name.ToLower() == Enumerator.Key.ToString())
                                                {
                                                  node.InnerText = Enumerator.Value.ToString();
                             
                                                 }
                                     }
                            }
                    }
                    }          
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik);
            xml.Save(fileName);                    

            
                    break;
                case 2:

                    foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;

                    foreach (XmlNode xn1 in xnChild)
                    {
                        if ((xn1.InnerText.ToLower() != param) && (xn1.Name.ToLower() == kolumna0))
                        {
                             licznik += 1;
                             Enumerator = kolumny.GetEnumerator();       
                                foreach (string klucz in klucze)
                                     {
                                        Enumerator.MoveNext();
                                            foreach (XmlNode node in xn)
                                             {
                                              
                                            if (node.Name.ToLower() == Enumerator.Key.ToString())
                                                {
                                                  node.InnerText = Enumerator.Value.ToString();
                             
                                                 }
                                     }
                            }
                    }
                    }          
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik);
            xml.Save(fileName);                    

                    break;
                case 3:
                        
                foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;

                    foreach (XmlNode xn1 in xnChild)
                    {
                        if ((String.CompareOrdinal(xn1.InnerText.ToLower(), param) <= 0) && (xn1.Name.ToLower() == kolumna0))
                        {
                             licznik += 1;
                             Enumerator = kolumny.GetEnumerator();       
                                foreach (string klucz in klucze)
                                     {
                                        Enumerator.MoveNext();
                                            foreach (XmlNode node in xn)
                                             {
                                              
                                            if (node.Name.ToLower() == Enumerator.Key.ToString())
                                                {
                                                  node.InnerText = Enumerator.Value.ToString();
                             
                                                 }
                                     }
                            }
                    }
                    }          
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik);
            xml.Save(fileName);                    
                    break;
                case 4:
                      foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;

                    foreach (XmlNode xn1 in xnChild)
                    {
                        if ((String.CompareOrdinal(xn1.InnerText.ToLower(), param) > 0) && (xn1.Name.ToLower() == kolumna0))
                        {
                             licznik += 1;
                             Enumerator = kolumny.GetEnumerator();       
                                foreach (string klucz in klucze)
                                     {
                                        Enumerator.MoveNext();
                                            foreach (XmlNode node in xn)
                                             {
                                              
                                            if (node.Name.ToLower() == Enumerator.Key.ToString())
                                                {
                                                  node.InnerText = Enumerator.Value.ToString();
                             
                                                 }
                                     }
                            }
                    }
                    }          
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik);
            xml.Save(fileName);                    
                    break;
                case 5:
                   foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;

                    foreach (XmlNode xn1 in xnChild)
                    {
                        if ((String.CompareOrdinal(xn1.InnerText.ToLower(), param) >= 0) && (xn1.Name.ToLower() == kolumna0))
                        {
                             licznik += 1;
                             Enumerator = kolumny.GetEnumerator();       
                                foreach (string klucz in klucze)
                                     {
                                        Enumerator.MoveNext();
                                            foreach (XmlNode node in xn)
                                             {
                                              
                                            if (node.Name.ToLower() == Enumerator.Key.ToString())
                                                {
                                                  node.InnerText = Enumerator.Value.ToString();
                             
                                                 }
                                     }
                            }
                    }
                    }          
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik);
            xml.Save(fileName);                    
                    break;

                default:
                    break;
            }

        }



        public void wstawDoWszystkichRekordow(XmlNodeList xnList, Hashtable kolumny)
        {
            ICollection klucze=kolumny.Keys;
            IDictionaryEnumerator Enumerator=kolumny.GetEnumerator();
            int licznik = 0;
            int mnoz = 0;
            foreach (string klucz in klucze)
            {
                Enumerator.MoveNext();
                mnoz += 1;

                foreach (XmlNode xn in xnList)
                {
                    XmlNodeList xnChild= xn.ChildNodes;
                    foreach (XmlNode xn1 in xnChild)
                    {
                        if (xn1.Name.ToLower() == Enumerator.Key.ToString())
                            xn1.InnerText = Enumerator.Value.ToString();

                    }
 licznik += 1;
                }
               
            }
            Console.WriteLine("Aktualizowano {0} wiersze",licznik/mnoz);
            xml.Save(fileName);
        }


        public bool typParametru(string nazwaTabeli,string kolumna,string parametr)
        {
            string path = string.Format("root/Types/{0}/strings/string", nazwaTabeli);

            XmlNodeList xnList = xml.SelectNodes(path);
            for (int j = 0; j < xnList.Count; j++)
            {
                
                    if (xnList[j].InnerText.ToLower() == kolumna)
                    {
                        string kontrol = "'";
                        if ((((potnijParametr(parametr)).ElementAt(0)) != kontrol) || (((potnijParametr(parametr).Last()) != kontrol)))
                        {
                            string msg = string.Format("Kolumna {0} jest typu string, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumna);
                            new Error(msg);
                            return false;

                        }
                        //else
                        //{
                        //    parametry.Add(wytnijCdzslowia(parametry.ElementAt(i)));
                        //    parametry.RemoveAt(i);
                        //}

                    }
                
            }

            path = string.Format("root/Types/{0}/numbers/number", nazwaTabeli);
            XmlNodeList xnList1 = xml.SelectNodes(path);
            for (int j = 0; j < xnList1.Count; j++)
            {
                                   if (xnList1[j].InnerText.ToLower() == kolumna)
                    {
                        string kontrol = "\x0027";
                        if ((((potnijParametr(parametr)).ElementAt(0)) == kontrol) || (((potnijParametr(parametr)).Last()) == kontrol))
                        {
                            string msg = string.Format("Kolumna {0} jest typu liczbowego, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumna);
                            new Error(msg);
                            return false;
                        }
                        else
                        {
                            try
                            {
                                System.Single.Parse(zamienKropke(parametr));
                            }
                            catch (Exception exp)
                            {
                                string msg = string.Format("Kolumna {0} jest typu liczbowego, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumna);
                                new Error(msg);
                                return false;
                            }
                        }
                    
                }
            }
          //  string msg0 = string.Format("Typ  kolumny {0} nie jest zgodny z parametrem {1} ",kolumna,parametr);
            //new Error(msg0);
            return true;
        }

        public string sprawdzParametry(int index, string typKolumny, List<string> zapytanie, string kolumna)
        {
            string parametr = null;

            if (typKolumny == "string")
            {
                string kontrol = "'";
                if ((((potnijParametr(zapytanie.ElementAt(index))).ElementAt(0)) != kontrol) || (((potnijParametr(zapytanie.ElementAt(index))).Last()) != kontrol))
                {
                    string msg = string.Format("Kolumna {0} jest typu string, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumna);
                    new Error(msg);
                    return null;
                }
                return parametr = (wytnijCdzslowia(zapytanie.ElementAt(index)));
            }

            if (typKolumny == "number")
            {
                string kontrol = "\x0027";
                if ((((potnijParametr(zapytanie.ElementAt(index))).ElementAt(0)) == kontrol) || (((potnijParametr(zapytanie.ElementAt(index))).Last()) == kontrol))
                {
                    string msg = string.Format("Kolumna {0} jest typu liczbowego, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumna);
                    new Error(msg);
                    return null;
                }
                else
                {
                    try
                    {
                        System.Single.Parse(zamienKropke(zapytanie.ElementAt(index)));
                        return parametr = zapytanie.ElementAt(index);
                    }
                    catch (Exception exp)
                    {
                        string msg = string.Format("Kolumna {0} jest typu liczbowego, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumna);
                        new Error(msg);
                        return null;
                    }
                }
                //return parametr;
            }
            return parametr;
        }

        public string sprawdzKolumne(string nazwaKolummny, XmlNodeList xnList0)
        {
            string wynik = null;
            foreach (XmlNode xnList in xnList0)
            {
                XmlNodeList lstCar = xnList.ChildNodes;
                bool dalej = false;

                //List<string> kolumny = new List<string>();

                for (int l = 0; l < lstCar.Count; l++)
                {

                    if (lstCar[l].Name.ToLower() == nazwaKolummny.ToLower())
                    {
                        //kolumny.Add(lstCar[l].Name.ToLower());
                        wynik = lstCar[l].Name.ToLower();
                        dalej = true;
                        return wynik;
                    }

                }

                if (dalej == false)
                {
                    string msg = "błąd w nazwie kolumny po słowie 'where'";
                    new Error(msg);
                    return wynik;
                }

            }
            return wynik;

        }

        public string typKolumnyPar(string nazwaTabeli, XmlDocument xml, string kolumna0)
        {
            string typKolumny0 = null;
            int counter = 0;
            string path = string.Format("root/Types/{0}/strings/string", nazwaTabeli);

            XmlNodeList xnList = xml.SelectNodes(path);
            counter = xnList.Count;
            for (int j = counter - 1; j >= 0; j--)
            {
                if (xnList[j].InnerText.ToLower() == kolumna0.ToLower())
                    typKolumny0 = "string";
            }

            path = string.Format("root/Types/{0}/numbers/number", nazwaTabeli);
            XmlNodeList xnList1 = xml.SelectNodes(path);
            counter = xnList1.Count;

            for (int j = counter - 1; j >= 0; j--)
            {
                if (xnList1[j].InnerText.ToLower() == kolumna0.ToLower())
                    typKolumny0 = "number";
            }
            return typKolumny0;
        }

            
   }
}

