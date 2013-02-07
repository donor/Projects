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
    class CreateTable
    {
        public CreateTable()
        {

        }

        public CreateTable(List<string> zapytanieSql, string fileName)
        {
           // bool kontrolkaTable = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            Hashtable kolumny=new Hashtable();
            string nazwaTab=zapytanieSql.ElementAt(2);

            XmlNode wezelGlowny = xml.DocumentElement.FirstChild;

            do  
            {
                if ((wezelGlowny.Name.ToLower()) == (nazwaTab.ToLower()))
                {
                    string msg = string.Format(" Tabela o nazwie {0} już istnieje.", nazwaTab);
                    new Error(msg);
                    return;
                }
                wezelGlowny = wezelGlowny.NextSibling;
            }
            while (wezelGlowny != null);

            if (typKolumny(zapytanieSql, 5) == null)
                return;
            
            kolumny.Add(zapytanieSql.ElementAt(4),typKolumny(zapytanieSql,5));

           int z = 6;
           do
           {
               if (((zapytanieSql.ElementAt(z) == ",") && (zapytanieSql.ElementAt(z - 1) == ")")) || ((zapytanieSql.ElementAt(z) == ",") && ((zapytanieSql.ElementAt(z - 1) == "bigint") || (zapytanieSql.ElementAt(z - 1) == "int") || (zapytanieSql.ElementAt(z - 1) == "smallint") || (zapytanieSql.ElementAt(z - 1) == "tinyint") || (zapytanieSql.ElementAt(z - 1) == "bit") || (zapytanieSql.ElementAt(z - 1) == "money") || (zapytanieSql.ElementAt(z - 1) == "smallmoney") || (zapytanieSql.ElementAt(z - 1) == "float") || (zapytanieSql.ElementAt(z - 1) == "real") || (zapytanieSql.ElementAt(z - 1) == "text"))))
               {
                   if (typKolumny(zapytanieSql, z + 2) == null)
                       return;
                   try
                   {           
                       kolumny.Add(zapytanieSql.ElementAt(z + 1), typKolumny(zapytanieSql, z + 2));
                   }
                   catch (ArgumentException exp)
                   {
                       string msg = string.Format("Nie można dodać 2 kolumn o tej samej nazwie");
                       new Error(msg);
                       return;
                   }
               }
               z++;
           }
           while (zapytanieSql.Count != z);

           XmlNode wezelGlowny0 = xml.DocumentElement;
           XmlElement nazwaTabeli = xml.CreateElement(nazwaTab);

           wezelGlowny0.AppendChild(nazwaTabeli);

           XmlNodeList list = wezelGlowny0.ChildNodes;
           XmlNode wez = wezelGlowny0.LastChild;
           XmlElement rekord = xml.CreateElement("rekord");
           wez = wez.AppendChild(rekord);
        
           ICollection klucze;
           IDictionaryEnumerator Enumerator;

           klucze = kolumny.Keys;
           Enumerator = kolumny.GetEnumerator();
           string path = null;

           XmlNode typDekla = xml.SelectSingleNode("root/Types");
           XmlElement typTaby = xml.CreateElement(nazwaTab);
           //nowakolumna.InnerText = string.Empty;
           typDekla.AppendChild(typTaby);
           XmlNode typDeklaChild = typDekla.LastChild;
           XmlElement xeString = xml.CreateElement("strings");
           typDeklaChild.AppendChild(xeString);
           XmlElement xeNumber = xml.CreateElement("numbers");
           typDeklaChild.AppendChild(xeNumber);

           xml.Save(fileName);


           foreach (string klucz in klucze)
           {
               XmlElement nowakolumna = xml.CreateElement(klucz);
               nowakolumna.InnerText = string.Empty;
               wez.AppendChild(nowakolumna);

               Enumerator.MoveNext();
               XmlElement deklaracjaTypu = xml.CreateElement(Enumerator.Value.ToString());
               deklaracjaTypu.InnerText = nowakolumna.Name;

               if (deklaracjaTypu.Name == "string")
               {
                    path = string.Format("root/Types/{0}/strings", nazwaTab);
                   XmlNode xnNode = xml.SelectSingleNode(path);
                   xnNode.AppendChild(deklaracjaTypu);
               }

               if (deklaracjaTypu.Name == "number")
               {
                   path = string.Format("root/Types/{0}/numbers", nazwaTab);
                   XmlNode xnNode = xml.SelectSingleNode(path);
                   xnNode.AppendChild(deklaracjaTypu);
                   
               }

           }

           xml.Save(fileName);
           Console.WriteLine("Dodano tabele '{0}'", nazwaTab);          
        }

        public string typKolumny(List<string> typwejscia, int index)
        {
            string nazwaTypu = String.Empty;

            if ((typwejscia.ElementAt(index) == "bigint") || (typwejscia.ElementAt(index) == "int") || (typwejscia.ElementAt(index) == "smallint") || (typwejscia.ElementAt(index) == "tinyint") || (typwejscia.ElementAt(index) == "bit") || (typwejscia.ElementAt(index) == "money") || (typwejscia.ElementAt(index) == "smallmoney") || (typwejscia.ElementAt(index) == "float") || (typwejscia.ElementAt(index) == "real"))
            {
                nazwaTypu = "number";
                return nazwaTypu ;
            }

            if ((typwejscia.ElementAt(index) == "decimal") || (typwejscia.ElementAt(index) == "numeric"))
            {
                if ((typwejscia.ElementAt(index + 1) != "(") || (typwejscia.ElementAt(index + 5) != ")"))
                {
                    string msg = string.Format(" Problem z deklaracja typu kolumny {0}.", typwejscia.ElementAt(index - 1));
                    new Error(msg);
                    return null;
                }

                try
                {
                    int zakres = System.Byte.Parse(typwejscia.ElementAt(index + 2));
                    int poPrzecinku = System.Byte.Parse(typwejscia.ElementAt(index+4));

                    if ((zakres < 0) || (zakres > 255))
                    {
                        string msg = string.Format(" Zakres deklaracji typu kolumny {0} jest bledny.", typwejscia.ElementAt(index - 1));
                        new Error(msg);
                        return null;
                    }
                    if (zakres < poPrzecinku)
                    {
                        string msg = string.Format(" Zakres deklaracji typu kolumny {0} jest bledny.", typwejscia.ElementAt(index - 1));
                        new Error(msg);
                        return null;
                    }
                }
                catch (Exception exp)
                {
                    string msg = string.Format(" Zakres deklaracji typu kolumny {0} jest bledny.", typwejscia.ElementAt(index - 1));
                    new Error(msg);
                    return null;
                }

                nazwaTypu = "number";
                return nazwaTypu;
            }

            if (typwejscia.ElementAt(index) == "text")
            {
                nazwaTypu = "string";
                return nazwaTypu;
            }

            if ((typwejscia.ElementAt(index) == "char") || (typwejscia.ElementAt(index) == "varchar") || (typwejscia.ElementAt(index) == "nchar") || (typwejscia.ElementAt(index) == "binary") || (typwejscia.ElementAt(index) == "varbinary"))
            {
                if ((typwejscia.ElementAt(index + 1) != "(") || (typwejscia.ElementAt(index + 3) != ")"))
                {
                    string msg = string.Format(" Problem z deklaracja typu kolumny {0}.", typwejscia.ElementAt(index - 1));
                    new Error(msg);
                    return null;
                }

                try
                {
                    int zakres = System.Byte.Parse(typwejscia.ElementAt(index + 2));
                    if ((zakres < 0) || (zakres > 255))
                    {
                        string msg = string.Format(" Zakres deklaracji typu kolumny {0} jest bledny.", typwejscia.ElementAt(index - 1));
                        new Error(msg);
                        return null;
                    }
                }
                catch (Exception exp)
                {
                    string msg = string.Format(" Zakres deklaracji typu kolumny {0} jest bledny.", typwejscia.ElementAt(index - 1));
                    new Error(msg);
                    return null;
                }

                nazwaTypu = "string";
                return nazwaTypu;
            }
            else
            {

                string msg = string.Format(" Zakres deklaracji typu kolumny {0} jest bledny.", typwejscia.ElementAt(index - 1));
                new Error(msg);
                return null;
            }

           // return nazwaTypu;
        }
    }
}
