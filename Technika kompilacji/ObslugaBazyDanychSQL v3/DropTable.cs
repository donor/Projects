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
    class DropTable
    {
        string nazwaTabeli = null;
        public DropTable()
        {

        }

        public DropTable(List<string> zapytanieSql, string fileName)
        {
            bool kontrolkaTable = false;
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
            bool usunieto = false;

            if (zapytanieSql.Count == 4)
            {         
            xml.DocumentElement.RemoveChild(wezelGlowny);
            usunDeklaracjeTypow(wezelGlowny.FirstChild.ChildNodes,xml);


            usunieto = true;
         
            }
                if (usunieto)
                {
                    Console.WriteLine("Usunieto tabele");
                    xml.Save(fileName);

                   // xml.Save(Console.Out);
                }
            
        }

        public void usunDeklaracjeTypow(XmlNodeList listawezlow, XmlDocument xml)
        {
            string path = null;
            List<string> listaKolumn = new List<string>();

            foreach (XmlNode xn in listawezlow)
                listaKolumn.Add(xn.Name);

            path = string.Format("root/Types/{0}/strings",nazwaTabeli);
                XmlNode xnNode = xml.SelectSingleNode(path);
                XmlNodeList xnl = xnNode.ChildNodes;

                for (int i = 0; i < xnl.Count; i++)
                {
                    for (int j = 0; j < listaKolumn.Count; j++)
                    {
                        try
                            {
                            if (xnl[i].InnerText.ToLower() == listaKolumn.ElementAt(j).ToLower())
                            {
                           
                                xnNode.RemoveChild(xnl[i]); ;
                                i--;
                            }                            
                        }
                        catch (Exception exp)
                        {     }
                    }
                }
                path = string.Format("root/Types/{0}/numbers", nazwaTabeli);
                XmlNode xnNode1 = xml.SelectSingleNode(path);
                XmlNodeList xnl1 = xnNode1.ChildNodes;
                for (int i = 0; i < xnl1.Count; i++)
                {
                    for (int j = 0; j < listaKolumn.Count; j++)
                    {
                     try
                       {
                        if (xnl1[i].InnerText.ToLower() == listaKolumn.ElementAt(j).ToLower())
                          {
                              xnNode1.RemoveChild(xnl1[i]);
                              i--;
                          }
                     }
                    catch (Exception exp)
                    {        }
                    }
                }

                path = string.Format("root/Types");
                XmlNode xnTypy = xml.SelectSingleNode(path);
                XmlNodeList xnListaTypow = xnTypy.ChildNodes;

                foreach (XmlNode xn in xnListaTypow)
                {
                    if (xn.Name == nazwaTabeli)
                    {
                        xnTypy.RemoveChild(xn);
                        break;
                    }
                }
            
        }
    }
}

