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
    class Inserty
    {
        public Inserty()
        {

        }

        public Inserty(List<string> zapytanieSql, string fileName)
        {

            bool kontrolkaTable = false;
            string nazwaTabeli = null;
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);

            XmlNode wezelGlowny = xml.DocumentElement.FirstChild;

            do
            {
                if ((wezelGlowny.Name.ToLower()) == (zapytanieSql.ElementAt(2)))
                {
                    kontrolkaTable = true;
                    nazwaTabeli = wezelGlowny.Name;
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
            if (zapytanieSql.ElementAt(3)!="(")
            {
                new Error("Błąd w miejscu podawania nazw kolumn (brakuje nawiasu otwierającego liste kolumn tabeli");
                return;
            }

            
            int indexValues = -1;
            indexValues = zapytanieSql.IndexOf("values");
            if (indexValues != -1)
            {
                if ((zapytanieSql.ElementAt(indexValues - 1)) != ")")
                {
                    new Error("Brak nawiasu zamykającego przed słowem 'values'");
                    return;
                }
            }
            else
            {
                new Error("brak słowa 'values'");
                return;
            }

            if ((zapytanieSql.ElementAt(zapytanieSql.IndexOf("values")+1)) != "(")
            {
                new Error("Błąd w miejscu podawania nazw kolumn (brakuje nawiasu otwierającego liste parametrów");
                return;
            }

            if ((zapytanieSql.ElementAt(zapytanieSql.Count-2)) != ")")
            {
                new Error("Błąd w miejscu podawania nazw kolumn (brakuje nawiasu zamykajácego liste parametrów");
                return;
            }

            XmlNodeList lst = wezelGlowny.FirstChild.ChildNodes;
            
            List<string> kolumny = new List<string>();


            
            for (int i = 4; i < zapytanieSql.IndexOf("values") - 1; i += 2)
            {
                bool dalej0 = false;
            for (int l = 0; l < lst.Count; l++)
            {
                if ((lst[l].Name.ToLower() == zapytanieSql.ElementAt(i).ToLower()) && ((zapytanieSql.ElementAt(i + 1) == ",") || ((zapytanieSql.ElementAt(i + 1) == ")"))))
                {
                    kolumny.Add(lst[l].Name.ToLower());
                    dalej0 = true;
                }
             
            }
            if (dalej0==false)       
                {
                    string msg = "błąd przy wprowadzaniu kolumn";
                    new Error(msg);
                    return;
                }
          }
        



                if (zapytanieSql.ElementAt(indexValues + 1) != "(")
                {
                    string msg = "brak nawiasu otwierającego po słowie values";
                    new Error(msg);
                    return;
                }

            if (zapytanieSql.ElementAt(zapytanieSql.IndexOf(zapytanieSql.Last())-1) != ")")
            {
                string msg = "brak nawiasu zamykającego liste parametrów";
                new Error(msg);
                return;
            }

            List<string> parametry = new List<string>();

            for (int i = zapytanieSql.IndexOf("values") +2; i <zapytanieSql.Count-1 ; i += 2)
            {
                bool dalej0 = false;
               
                    if ((zapytanieSql.ElementAt(i + 1) == ",") || ((zapytanieSql.ElementAt(i + 1) == ")")))
                    {
                        parametry.Add(zapytanieSql.ElementAt(i));
                        dalej0 = true;
                    }
                    else if (dalej0 == false)
                {
                    string msg = "błąd prz wprowadzania parametrów";
                    new Error(msg);
                    return;
                }
            }


            if (kolumny.Count != parametry.Count)
            {
                new Error("ilość parametrów jest inna od ilości kolumn do wstawienia");
                return;
            }

            string path = string.Format("root/Types/{0}/strings/string", nazwaTabeli);

            XmlNodeList xnList = xml.SelectNodes(path);
            for (int j = 0; j <  xnList.Count; j++)
            {
                for (int i = 0; i <kolumny.Count; i++)
                {

                    if (xnList[j].InnerText.ToLower() == kolumny.ElementAt(i))
                    {
                        string kontrol="'";
                        if ((((potnijParametr(parametry.ElementAt(i))).ElementAt(0)) != kontrol) || (((potnijParametr(parametry.ElementAt(i))).Last()) != kontrol))
                        {
                            string msg = string.Format("Kolumna {0} jest typu string, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumny.ElementAt(i));
                            new Error(msg);
                            return;

                        }
                        else
                        {
                            parametry.Insert(i, wytnijCdzslowia(parametry.ElementAt(i)));

                            //parametry.Add(wytnijCdzslowia(parametry.ElementAt(i)));
                            parametry.RemoveAt(i+1 );
                        }

                    }
                }
              }

            path = string.Format("root/Types/{0}/numbers/number", nazwaTabeli);
            XmlNodeList xnList1 = xml.SelectNodes(path);
            for (int j = 0; j <xnList1.Count; j++)
            {
                for (int i = 0; i <  kolumny.Count; i++)
                {

                    if (xnList1[j].InnerText.ToLower() == kolumny.ElementAt(i))
                    {
                        string kontrol = "\x0027";
                        if ((((potnijParametr(parametry.ElementAt(i))).ElementAt(0)) == kontrol) || (((potnijParametr(parametry.ElementAt(i))).Last()) == kontrol))
                        {
                            string msg = string.Format("Kolumna {0} jest typu liczbowego, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumny.ElementAt(i));
                            new Error(msg);
                            return;
                        }
                        else
                        {
                           try
                           {
                              System.Single.Parse(  zamienKropke(parametry.ElementAt(i)));
                           }
                            catch (Exception exp)
                           {
                               string msg = string.Format("Kolumna {0} jest typu liczbowego, a parametr jej odpowiadajácy nie jest zgodny z tym typem", kolumny.ElementAt(i));
                               new Error(msg);
                               return;
                            }
                        }
                    }
                }
            }

           XmlNode node = xml.CreateNode(XmlNodeType.Element, wezelGlowny.FirstChild.Name, null);
            XmlNode node1=null;

           for (int i1 = 0; i1 < lst.Count; i1++)
           {
                bool pusta = false;
               string parametr=null;
             
               for (int i2 = 0; i2 < kolumny.Count; i2++)
               {                   
                   if (lst.Item(i1).Name.ToLower() == kolumny.ElementAt(i2).ToLower())
                   {
                       pusta = true;
                       parametr=parametry.ElementAt(i2);
                       break;
                   }
               }
               if (pusta == false)
               {
                   node1 = xml.CreateElement(lst.Item(i1).Name);
                   node1.InnerText = "null";
               }
               else
               {
                   node1 = xml.CreateElement(lst.Item(i1).Name);
                   node1.InnerText = parametr;
               }
               
               node.AppendChild(node1);
           }

            wezelGlowny.AppendChild(node);

            xml.Save(fileName);                        
            Console.WriteLine("rekord został dodany");           
        }    
   
        public List<string> potnijParametr(string wejscie)
        {
             string[] words = Regex.Split(wejscie, @"()");
       
           List<string> wyj=new List<string>();

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i]!="")
                wyj.Add(words[i]);
            }
         return wyj;
        }

        public string wytnijCdzslowia(string wejscie)
        {
            char[] wytnij = {'\x0027' };
            string wyjscie;
            
            wyjscie = wejscie.Trim(wytnij);

            return wyjscie;
        }

        public string zamienKropke(string wejscie)
        {
            string[] words = Regex.Split(wejscie, @"()");

            List<string> wyj = new List<string>();
            string wyjscie="";
                        
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] != "")
                    wyj.Add(words[i]);
                if (words[i] == ".")
                {                    
                    wyj.Add(",");
                    wyj.RemoveAt(wyj.Count - 2);                   
                }                
            }

            for (int i = 0; i < wyj.Count;i++ )
                wyjscie=wyjscie.Insert(wyjscie.Length, wyj.ElementAt(i));

            return wyjscie;
        }

    }
}
