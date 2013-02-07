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
    class Delety:Inserty
    {
        string kolumna0 = null;

        public Delety()
        {

        }

        public Delety(List<string> zapytanieSql, string fileName)
        {

            bool kontrolkaTable = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            string nazwaTabeli=null;

            XmlNode wezelGlowny = xml.DocumentElement.FirstChild;

            do
            {
                if ((wezelGlowny.Name.ToLower()) == (zapytanieSql.ElementAt(2)))
                {
                    kontrolkaTable = true;
                    nazwaTabeli=zapytanieSql.ElementAt(2);
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
            bool znaki4 = false;

            if (zapytanieSql.Count == 4)  //usuwanie całej tabeli
            {
                int liczer = 0;
                XmlNodeList ls = wezelGlowny.ChildNodes;

                List<string> tempListaKolumn = new List<string>(); //lista do przechowywania nazw kolumn
                int rozmiar = ls.Count;
               
                for (int x = rozmiar-1; x >=0; x--)
                {
                          if (x == 0)
                            {
                               XmlNode node = wezelGlowny.FirstChild;
                                tempListaKolumn.Add(node.Name);

                                XmlNodeList temp = node.ChildNodes;
                             foreach (XmlNode wez in temp)
                              {
                                  tempListaKolumn.Add(wez.Name);
                              }

                            }    
                    wezelGlowny.RemoveChild(ls[x]);
                            liczer++;
                          
                }
                Console.WriteLine("Usunieto {0} rekordów", liczer);
                try
                {
                    XmlElement rekord = xml.CreateElement(tempListaKolumn.ElementAt(0));
                    wezelGlowny.AppendChild(rekord);

                    XmlNode wezelN = wezelGlowny.FirstChild;

                    for (int i = 1; i < tempListaKolumn.Count;i++ )
                    {
                        XmlElement nowakolumna = xml.CreateElement(tempListaKolumn.ElementAt(i));
                       // nowakolumna.InnerText = "null";
                        wezelN.AppendChild(nowakolumna);
                    }
                }
                catch (Exception exp)
                {

                }
                XmlTextWriter witer = new XmlTextWriter(fileName, null);
                witer.Formatting = Formatting.Indented;
                xml.Save(witer);
                witer.Close();

               znaki4=true;
               return;
                
            }

            if ((zapytanieSql.ElementAt(3)!="where")&&(!znaki4))
            {
                    new Error("Brak słowa 'where'");
                    return;             
            }
            
            XmlNodeList lst = wezelGlowny.FirstChild.ChildNodes;
            
            bool dalej0 = false;
          

            if (!znaki4)
            {
                for (int l = 0; l < lst.Count; l++)
                {
                    if (lst[l].Name.ToLower() == zapytanieSql.ElementAt(4).ToLower())
                    {
                        kolumna0 = lst[l].Name.ToLower();
                        dalej0 = true;
                        break;
                    }
                }
            }
            if ((dalej0 == false) && (!znaki4))
            {
                string msg = "błąd w nazwie kolumny";
                new Error(msg);
                return;
            }

            string typKolumny0=null;
            int counter = 0;
            string path = string.Format("root/Types/{0}/strings/string",nazwaTabeli);

            XmlNodeList xnList = xml.SelectNodes(path);
            counter=xnList.Count;
            for (int j =counter-1 ; j >=0; j--)
            {
                   if (xnList[j].InnerText.ToLower() == kolumna0.ToLower())
                       typKolumny0="string";                    
            }

            path = string.Format("root/Types/{0}/numbers/number", nazwaTabeli);
            XmlNodeList xnList1 = xml.SelectNodes(path);
            counter = xnList1.Count;

            for (int j = counter-1; j >=0; j--)
            {
                if (xnList1[j].InnerText.ToLower() == kolumna0.ToLower())
                    typKolumny0 = "number";
            }

            
            if (!znaki4)
            {
                string operat = zapytanieSql.ElementAt(5);
                if ((operat != "=") && (operat != "<") && (operat != ">"))
                {
                    string msg = string.Format("problem z operatorem {0}", operat);
                    new Error(msg);
                    return;
                }

            
                string parametr = null;
                if ((operat == "="))//&&((zapytanieSql.ElementAt(6)!=""))
                {
                   parametr= sprawdzParametry(6, typKolumny0, zapytanieSql, kolumna0);
              
                   if (parametr == null)
                       return;
                   usun(parametr, 0, wezelGlowny, xml);
                }
                if ((operat == "<") && ((zapytanieSql.ElementAt(6) != ">") && (zapytanieSql.ElementAt(6) != "=")))
                {
                    parametr = sprawdzParametry(6, typKolumny0, zapytanieSql, kolumna0);
                  
                    if (parametr == null)
                        return;
                    usun(parametr, 1, wezelGlowny, xml);
                }
                if ((operat == "<") && (zapytanieSql.ElementAt(6) == ">"))
                {
                    parametr = sprawdzParametry(7, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    usun(parametr, 2, wezelGlowny, xml);
                }
                if ((operat == "<") && (zapytanieSql.ElementAt(6) == "="))
                {
                    parametr = sprawdzParametry(7, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    usun(parametr, 3, wezelGlowny, xml);
                }
                if ((operat == ">") && (zapytanieSql.ElementAt(6) != "="))
                {
                    parametr = sprawdzParametry(6, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    usun(parametr, 4, wezelGlowny, xml);
                }
                if ((operat == ">") && (zapytanieSql.ElementAt(6) == "="))
                {
                    parametr = sprawdzParametry(7, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    usun(parametr, 5, wezelGlowny, xml);
                }
            }                  
        }

        public void usun(string param,int wybor, XmlNode wezelGlowny, XmlDocument xml)
        {
            int licznikUsunietych = 0;
            bool usunieto = false;
            XmlNodeList lst0 = wezelGlowny.ChildNodes;
            List<string> tempKolumny = null;

            switch(wybor)
            {
                case 0:
                     for (int x = 0; x < lst0.Count; x++)
                     {
                         XmlNodeList lst1 = lst0[x].ChildNodes;

                        for (int l = 0; l < lst1.Count; l++)
                        {
                           if (lst1[l].InnerText.ToLower() == param)
                            {
                                wezelGlowny.RemoveChild(lst0[x]);
                                x--;
                                licznikUsunietych++;
                                usunieto = true;
                            }
                         }
                     }

                     if (usunieto)
                     {
                         Console.WriteLine("Usunieto {0} rekordów", licznikUsunietych);
                         xml.Save(Program.fileName);
                     }
                    break;
                case 1:
                    for (int x = 0; x < lst0.Count; x++)
                    {
                        XmlNodeList lst1 = lst0[x].ChildNodes;
                        tempKolumny = new List<string>();
                        foreach (XmlNode k in lst1)
                            tempKolumny.Add(k.Name);
                        bool usuwac = false;
                        for (int l = 0; l < lst1.Count; l++)
                        {
                            if ( (String.CompareOrdinal(lst1[l].InnerText.ToLower(), param)<0)&&(lst1[l].Name.ToLower()==kolumna0)) //mniejsze
                            {
                                usuwac = true;
                                break;
                            }
                        }
                        if (usuwac)
                        {
                            try
                            {
                                wezelGlowny.RemoveChild(lst0[x]);
                                    x--;
                                licznikUsunietych++;
                                usunieto = true;
                            }
                            catch (Exception exp)
                            {

                            }
                        }
                    }

                         if (lst0.Count == 0)
                        {
                           XmlElement nowakolumna = xml.CreateElement("rekord");
                           wezelGlowny.AppendChild(nowakolumna);

                           XmlNode wezelN = wezelGlowny.FirstChild;

                         foreach (string kola in tempKolumny)
                         {
                             XmlElement nowakolumna1 = xml.CreateElement(kola);
                             //nowakolumna1.InnerText = "null";
                             wezelN.AppendChild(nowakolumna1);
                         }                     
                     }
                     if (usunieto)
                     {
                         Console.WriteLine("Usunieto {0} rekordów", licznikUsunietych);
                         xml.Save(Program.fileName);
                     }
                    break;
                case 2:                     
                    for (int x = 0; x < lst0.Count; x++)
                    {
                        XmlNodeList lst1 = lst0[x].ChildNodes;
                        tempKolumny = new List<string>();
                        foreach (XmlNode k in lst1)
                            tempKolumny.Add(k.Name);
                        bool usuwac = false;
                        for (int l = 0; l < lst1.Count; l++)
                        {
                            if (lst1[l].InnerText.ToLower() == param)
                            {
                                usuwac = true;
                                
                            }
                        }
                        if (!usuwac)
                        {
                            try
                            {
                                wezelGlowny.RemoveChild(lst0[x]);
                                licznikUsunietych++;
                                x--;
                                usunieto = true;
                            }
                            catch (Exception exp)
                            {

                            }
                        }
                    }
                         if (lst0.Count == 0)
                        {
                           XmlElement nowakolumna = xml.CreateElement("rekord");
                           wezelGlowny.AppendChild(nowakolumna);

                           XmlNode wezelN = wezelGlowny.FirstChild;

                         foreach (string kola in tempKolumny)
                         {
                             XmlElement nowakolumna1 = xml.CreateElement(kola);
                             //nowakolumna1.InnerText = "null";
                             wezelN.AppendChild(nowakolumna1);
                         }                     
                     }
                     if (usunieto)
                     {
                         Console.WriteLine("Usunieto {0} rekordów", licznikUsunietych);
                         xml.Save(Program.fileName);
                     }
                    break;
                case 3:
                      for (int x = 0; x < lst0.Count; x++)
                    {
                        XmlNodeList lst1 = lst0[x].ChildNodes;
                        tempKolumny = new List<string>();
                        foreach (XmlNode k in lst1)
                            tempKolumny.Add(k.Name);
                        bool usuwac = false;
                        for (int l = 0; l < lst1.Count; l++)
                        {
                            if ( (String.CompareOrdinal(lst1[l].InnerText.ToLower(), param)<=0)&&(lst1[l].Name.ToLower()==kolumna0)) //mniejsze
                            {
                                usuwac = true;
                                break;
                            }
                        }
                        if (usuwac)
                        {
                            try
                            {
                                wezelGlowny.RemoveChild(lst0[x]);
                                    x--;
                                licznikUsunietych++;
                                usunieto = true;
                            }
                            catch (Exception exp)
                            {

                            }
                        }
                    }
                         if (lst0.Count == 0)
                        {
                           XmlElement nowakolumna = xml.CreateElement("rekord");
                           wezelGlowny.AppendChild(nowakolumna);

                           XmlNode wezelN = wezelGlowny.FirstChild;

                         foreach (string kola in tempKolumny)
                         {
                             XmlElement nowakolumna1 = xml.CreateElement(kola);
                             //nowakolumna1.InnerText = "null";
                             wezelN.AppendChild(nowakolumna1);
                         }                    
                     }
                     if (usunieto)
                     {
                         Console.WriteLine("Usunieto {0} rekordów", licznikUsunietych);
                         xml.Save(Program.fileName);
                     }
                    break;
                case 4:
                    for (int x = 0; x < lst0.Count; x++)
                    {
                        XmlNodeList lst1 = lst0[x].ChildNodes;
                        tempKolumny = new List<string>();
                        foreach (XmlNode k in lst1)
                            tempKolumny.Add(k.Name);
                        bool usuwac = false;
                        for (int l = 0; l < lst1.Count; l++)
                        {
                            if ( (String.CompareOrdinal(lst1[l].InnerText.ToLower(), param)>0)&&(lst1[l].Name.ToLower()==kolumna0)) //mniejsze
                            {
                                usuwac = true;
                                break;
                            }
                        }
                        if (usuwac)
                        {
                            try
                            {
                                wezelGlowny.RemoveChild(lst0[x]);
                                    x--;
                                licznikUsunietych++;
                                usunieto = true;
                            }
                            catch (Exception exp)
                            {

                            }
                        }
                    }
                         if (lst0.Count == 0)
                        {
                           XmlElement nowakolumna = xml.CreateElement("rekord");
                           wezelGlowny.AppendChild(nowakolumna);

                           XmlNode wezelN = wezelGlowny.FirstChild;

                         foreach (string kola in tempKolumny)
                         {
                             XmlElement nowakolumna1 = xml.CreateElement(kola);
                             //nowakolumna1.InnerText = "null";
                             wezelN.AppendChild(nowakolumna1);
                          }
                      }
                      if (usunieto)
                     {
                         Console.WriteLine("Usunieto {0} rekordów", licznikUsunietych);
                         xml.Save(Program.fileName);
                     }
                    break;
                case 5:
                    for (int x = 0; x < lst0.Count; x++)
                    {
                        XmlNodeList lst1 = lst0[x].ChildNodes;
                        tempKolumny = new List<string>();
                        foreach (XmlNode k in lst1)
                            tempKolumny.Add(k.Name);
                        bool usuwac = false;
                        for (int l = 0; l < lst1.Count; l++)
                        {
                            if ( (String.CompareOrdinal(lst1[l].InnerText.ToLower(), param)>=0)&&(lst1[l].Name.ToLower()==kolumna0)) //mniejsze
                            {
                                usuwac = true;
                                break;
                            }
                        }
                        if (usuwac)
                        {
                            try
                            {
                                wezelGlowny.RemoveChild(lst0[x]);
                                    x--;
                                licznikUsunietych++;
                                usunieto = true;
                            }
                            catch (Exception exp)
                            {

                            }
                        }
                    }

                         if (lst0.Count == 0)
                        {
                           XmlElement nowakolumna = xml.CreateElement("rekord");
                           wezelGlowny.AppendChild(nowakolumna);

                           XmlNode wezelN = wezelGlowny.FirstChild;

                         foreach (string kola in tempKolumny)
                         {
                             XmlElement nowakolumna1 = xml.CreateElement(kola);
                             //nowakolumna1.InnerText = "null";
                             wezelN.AppendChild(nowakolumna1);
                         }                     
                     }
                     if (usunieto)
                     {
                         Console.WriteLine("Usunieto {0} rekordów", licznikUsunietych);
                         xml.Save(Program.fileName);
                     }
                    break;

                default:
                                        break;
                    }
                    
        }

        public string sprawdzParametry(int index,string typKolumny,List<string> zapytanie, string kolumna)
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

     


    }
}
