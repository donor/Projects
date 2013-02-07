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
    class AlterTable:CreateTable
    {
        XmlDocument xml = new XmlDocument();

        public AlterTable()
        {

        }

        public AlterTable(List<string> zapytanieSql, string fileName)
        {
           
            bool kontrolkaTable = false;
           
            xml.Load(fileName);

            XmlNode wezelGlowny = xml.DocumentElement.FirstChild;

            do
            {
                if ((wezelGlowny.Name.ToLower()) == (zapytanieSql.ElementAt(2)))
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

            
            
            if (zapytanieSql.ElementAt(3).ToLower() == "add")
            {
                if (zapytanieSql.ElementAt(4) == "column")
                {
                    string nazwaKolumny=sprawdzKolumne(zapytanieSql.ElementAt(5), wezelGlowny.ChildNodes);

                    string typK =typKolumny(zapytanieSql, 6);

                    if ((nazwaKolumny == null)||(typK==null))
                        return;
                                       
                        XmlNodeList list = wezelGlowny.ChildNodes;
                        XmlNode wez = wezelGlowny.FirstChild;
                        do
                        {
                            XmlElement nowakolumna = xml.CreateElement(nazwaKolumny);
                            nowakolumna.InnerText = "null";
                            wez.AppendChild(nowakolumna);
                            wez = wez.NextSibling;
                        }
                        while (wez != null);

                        wstawTyp(typK, zapytanieSql.ElementAt(2), nazwaKolumny/*,XmlDocument xml*/);
                        xml.Save(fileName);
                        Console.WriteLine("Dodano kolumne {0}'", nazwaKolumny);
                        //xml.Save(Console.Out);
                    
                }
                else
                    new Error("Problem z poleceniem Add");
            }
            if (zapytanieSql.ElementAt(3).ToLower() == "drop")
            {
                bool usuwa = false;
                if (zapytanieSql.ElementAt(4).ToLower() == "column")
                {                   
                    XmlNodeList lst0 = wezelGlowny.ChildNodes;
                    for (int x = 0; x < lst0.Count; x++)
                    {
                        XmlNodeList lst1 = lst0[x].ChildNodes;

                        for (int l = 0; l < lst1.Count; l++)
                        {      
                            try
                                {                      
                            if (lst1[l].Name.ToLower() == zapytanieSql.ElementAt(5).ToLower())
                            {                              
                                    lst0[x].RemoveChild(lst1[l]);
                                    x--;
                                    usuwa = true;                                 
                                }
                            }
                                catch (Exception exp)
                                {        }

                                                        
                        }
                    }

                   string  path = string.Format("root/Types/{0}/strings", zapytanieSql.ElementAt(2));
                    XmlNode xnNode = xml.SelectSingleNode(path);
                    XmlNodeList xnl = xnNode.ChildNodes;

                    for (int i = 0; i < xnl.Count; i++)
                    {
                       
                            try
                            {
                                if (xnl[i].InnerText.ToLower() == zapytanieSql.ElementAt(5).ToLower())
                                {

                                    xnNode.RemoveChild(xnl[i]); ;
                                    i--;
                                }
                            }
                            catch (Exception exp)
                            { }
                    }
                    path = string.Format("root/Types/{0}/numbers", zapytanieSql.ElementAt(2));
                    XmlNode xnNode1 = xml.SelectSingleNode(path);
                    XmlNodeList xnl1 = xnNode1.ChildNodes;
                    for (int i = 0; i < xnl1.Count; i++)
                    {
                           try
                            {
                                if (xnl1[i].InnerText.ToLower() == zapytanieSql.ElementAt(5).ToLower())
                                {
                                    xnNode1.RemoveChild(xnl1[i]);
                                    i--;
                                }
                            }
                            catch (Exception exp)
                            { }
                        
                    }

                    xml.Save(fileName);
                    if (usuwa)
                    {
                        Console.WriteLine("usunieto columne '{0}'", zapytanieSql.ElementAt(5));
                        xml.Save(fileName);
                    }
                    if (!usuwa)
                        Console.WriteLine("Nie ma columny o nazwie '{0}'", zapytanieSql.ElementAt(5));

                   // xml.Save(Console.Out);
                }

            }

            if (zapytanieSql.ElementAt(3).ToLower() == "alter")
            {
                if (zapytanieSql.ElementAt(4).ToLower() == "column")
                {
                    string typK = typKolumny(zapytanieSql, 6);

                    if ((!sprawdzKolumne1(zapytanieSql.ElementAt(5), wezelGlowny.ChildNodes))||(typK==null))
                        return;

                    zmianaTypu(typK, zapytanieSql.ElementAt(2),zapytanieSql.ElementAt(5));


                    xml.Save(fileName);
                  
                }
            }




           
        }

        public void zmianaTypu(string typKoly,string nazwaTab,string nazwaKol)
        {
            string path;
            bool zmiana = false;
         

            if (typKoly == "string")
            {
                path = string.Format("root/Types/{0}/strings", nazwaTab);
                XmlNode xnNode = xml.SelectSingleNode(path);
                XmlNodeList xnNodeChilds= xnNode.ChildNodes;

                foreach (XmlNode node in xnNodeChilds)
                {
                    XmlNodeList nodeList0 = node.ChildNodes;

                    foreach (XmlNode node0 in nodeList0)
                    {
                        if (node0.InnerText.ToLower() == nazwaKol)
                        {
                            zmiana = true;
                            Console.WriteLine("Zmieniono typ kolumny");

                            return;
                            //break;

                        }
                    }
                }

                if (!zmiana)
                {
                    XmlElement deklaracjaTypu = xml.CreateElement("string");
                    deklaracjaTypu.InnerText = nazwaKol;
                    xnNode.AppendChild(deklaracjaTypu);

                    path = string.Format("root/Types/{0}/numbers", nazwaTab);
                    XmlNode xnlNode = xml.SelectSingleNode(path);
                    XmlNodeList xnNodeChilders = xnlNode.ChildNodes;

                    foreach (XmlNode node in xnNodeChilders)
                    {
                        XmlNodeList nodeList0 = node.ChildNodes;

                        foreach (XmlNode node0 in nodeList0)
                        {
                            if (node0.InnerText.ToLower() == nazwaKol)
                            {
                                node.RemoveChild(node0);
                                zmiana = true;
                                Console.WriteLine("Zmieniono typ kolumny");

                                return;
                             
                            }
                        }
                    }

                 
                }




             
            }

            if (typKoly == "number")
            {
                path = string.Format("root/Types/{0}/numbers", nazwaTab);
                XmlNode xnNode = xml.SelectSingleNode(path);
                XmlNodeList xnNodeChilds = xnNode.ChildNodes;

                foreach (XmlNode node in xnNodeChilds)
                {
                    XmlNodeList nodeList0 = node.ChildNodes;

                    foreach (XmlNode node0 in nodeList0)
                    {
                        if (node0.InnerText.ToLower() == nazwaKol)
                        {
                            zmiana = true;
                            Console.WriteLine("Zmieniono typ kolumny");

                            return;
                            //break;

                        }
                    }
                }

                if (!zmiana)
                {

                    Console.WriteLine("Nie można wykonać takiej zamiany typu dla tej kolumny");
                }

            }

        }

        public bool sprawdzKolumne1(string nazwaKolummny, XmlNodeList xnList0)
        {
            bool dalej = false;
          
            foreach (XmlNode xnList in xnList0)
            {
                XmlNodeList lstCar = xnList.ChildNodes;

                for (int l = 0; l < lstCar.Count; l++)
                {

                    if (lstCar[l].Name.ToLower() == nazwaKolummny.ToLower())
                    {
                        dalej = true;
                        break;
                    }

                }

                if (!dalej)
                {
                    string msg = "Kolumna o tej nazwie nie istnieje";
                    new Error(msg);
                    //Console.WriteLine("Nie dodano kolumny");
                    return dalej;
                }


            }
           
            return dalej;
        }

        public string sprawdzKolumne(string nazwaKolummny, XmlNodeList xnList0)
        {
            bool dalej = false;
            string wynik = null;
            foreach (XmlNode xnList in xnList0)
            {
                XmlNodeList lstCar = xnList.ChildNodes;
              
                for (int l = 0; l < lstCar.Count; l++)
                {

                    if (lstCar[l].Name.ToLower() == nazwaKolummny.ToLower())
                    {                       
                        dalej = true;
                        break;
                    }

                }

                if (dalej)
                {
                    string msg = "Kolumna o tej nazwie już istnieje";
                    new Error(msg);
                    Console.WriteLine("Nie dodano kolumny");
                    return wynik;
                }
                

            }
            if (!dalej)
                wynik = nazwaKolummny;
            return wynik;
        }

        public void wstawTyp(string typ,string nazwaTab,string nazwaKol)
        {
            string path;
            XmlElement deklaracjaTypu;
            if (typ == "string")
            {
                deklaracjaTypu= xml.CreateElement(typ);
                deklaracjaTypu.InnerText = nazwaKol;
                path = string.Format("root/Types/{0}/strings", nazwaTab);
                XmlNode xnNode = xml.SelectSingleNode(path);
                xnNode.AppendChild(deklaracjaTypu);
            }

            if (typ == "number")
            {
                deklaracjaTypu = xml.CreateElement(typ);
                deklaracjaTypu.InnerText = nazwaKol;
                path = string.Format("root/Types/{0}/numbers", nazwaTab);
                XmlNode xnNode = xml.SelectSingleNode(path);
                xnNode.AppendChild(deklaracjaTypu);
            }
        }
    }
}
