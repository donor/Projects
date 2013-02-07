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
    class Selecty:Delety
    {
        private XElement doc;
        string kolumna0 = null;
   
        public Selecty()
        {

        }

        public Selecty(List<string> zapytanieSql, string fileName)
        {
            string nazwaTabeli=null;
            try
            {
            bool kontrolkaTable = false;
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            int index =zapytanieSql.IndexOf("from") + 1;
            if (index < 0)
                new Error("Brak słowa from");
            


            XmlNode wezelGlowny = xml.DocumentElement.FirstChild;
            
                do
                {
                    if ((wezelGlowny.Name.ToLower()) == (zapytanieSql.ElementAt(index)))
                    {
                        kontrolkaTable = true;
                        nazwaTabeli=zapytanieSql.ElementAt(index);
                        break;
                    }
                    wezelGlowny = wezelGlowny.NextSibling;
                }
                while (wezelGlowny != null);
           

            if (kontrolkaTable== false)
            {
                new Error("Nie ma tabeli o takiej nazwie");
                return;
            }

            string selectnd = string.Format("root/{0}/{1}", wezelGlowny.Name, wezelGlowny.FirstChild.Name);

            XmlNodeList xnList = xml.SelectNodes(selectnd);

            if (zapytanieSql.ElementAt(index + 1) == ";")
            {
                wypiszKolumny(wezelGlowny, xml, zapytanieSql);
                wypiszWszystkieRekordy(xnList, zapytanieSql);
            }
            else if (zapytanieSql.ElementAt(index + 1) == "where")
            {
                //sprawdzenie czy kolumna z sekcji where istnieje
                string operat = zapytanieSql.ElementAt(index + 3);
                /*string*/ kolumna0=sprawdzKolumne(zapytanieSql.ElementAt(index+2),xnList);
                if (kolumna0 == null)
                    return;

                string typKolumny0=typKolumnyPar(nazwaTabeli, xml, kolumna0);


                if ((operat != "=") && (operat != "<") && (operat != ">"))
                {
                    string msg = string.Format("problem z operatorem {0}", operat);
                    new Error(msg);
                    return;
                }

                wypiszKolumny(wezelGlowny, xml, zapytanieSql);
                string parametr = null;
                if ((operat == "="))//&&((zapytanieSql.ElementAt(6)!=""))
                {
                    parametr = sprawdzParametry(index+4, typKolumny0, zapytanieSql, kolumna0);

                    if (parametr == null)
                        return;
                    wypisz(0, parametr, xnList, xml,zapytanieSql);
                }
                if ((operat == "<") && ((zapytanieSql.ElementAt(index + 4) != ">") && (zapytanieSql.ElementAt(index + 4) != "=")))
                {
                    parametr = sprawdzParametry(index + 4, typKolumny0, zapytanieSql, kolumna0);

                    if (parametr == null)
                        return;
                    wypisz(1, parametr, xnList, xml, zapytanieSql);
                }
                if ((operat == "<") && (zapytanieSql.ElementAt(index + 4) == ">"))
                {
                    parametr = sprawdzParametry(index+5, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wypisz(2, parametr, xnList, xml, zapytanieSql);
                }
                if ((operat == "<") && (zapytanieSql.ElementAt(index + 4) == "="))
                {
                    parametr = sprawdzParametry(index + 5, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wypisz(3, parametr, xnList, xml, zapytanieSql);
                }
                if ((operat == ">") && (zapytanieSql.ElementAt(index + 4) != "="))
                {
                      parametr = sprawdzParametry(index+4, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wypisz(4, parametr, xnList, xml, zapytanieSql);
                }
                if ((operat == ">") && (zapytanieSql.ElementAt(index + 4) == "="))
                {
                    parametr = sprawdzParametry(index + 5, typKolumny0, zapytanieSql, kolumna0);
                    if (parametr == null)
                        return;
                    wypisz(5, parametr, xnList, xml, zapytanieSql);
                }

            }
            else
            {
                new Error("Błędna formuła zapytania");
                return;
            }
           
           }
            catch (Exception exp)
            {
            }
           }

        public void wypisz(int wybor, string prametr, XmlNodeList xnList, XmlDocument xml, List<string> zapytanieSql)
        {

            bool wypisz = false;
            switch (wybor)
            {
                case 0:
                    for (int i = 0; i < xnList.Count; i++)
                    {   //jezeli w sekcji select-from jest tylko *
                        wypisz = false;
                        XmlNodeList lstPar = xnList[i].ChildNodes;
                        foreach (XmlNode xn in lstPar)
                        {
                            if (xn.InnerText.ToLower() == prametr)
                            { 
                                wypisz = true;
                                break; 
                            }

                        }
                        if (wypisz)
                        {
                            if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                            {


                                XmlNodeList lstCar = xnList[i].ChildNodes;



                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                                    else
                                    {
                                        string wstawka = "";

                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");

                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                XmlNodeList lstCar = xnList[i].ChildNodes;
                                bool dalej = false;
                                List<string> kolumny = new List<string>();

                                for (int l = 0; l < lstCar.Count; l++)
                                {
                                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                                    {
                                        if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                        {
                                            kolumny.Add(lstCar[l].Name.ToLower());
                                            dalej = true;
                                        }
                                    }
                                }

                                if (dalej == false)
                                {
                                    string msg = "błąd w nazwie kolumny";
                                    new Error(msg);
                                    return;
                                }

                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    for (int m = 0; m < kolumny.Count; m++)
                                    {
                                        if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                        {

                                            if (lstCar[j].InnerText.Count() > 12)
                                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                            else
                                            {
                                                string wstawka = "";
                                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                                    wstawka = string.Concat(wstawka, " ");
                                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine();
                            }
                        }
                    }
 


                    break;
                case 1:
                    for (int i = 0; i < xnList.Count; i++)
                    {   //jezeli w sekcji select-from jest tylko *
                        wypisz = false;
                        XmlNodeList lstPar = xnList[i].ChildNodes;
                        foreach (XmlNode xn in lstPar)
                        {
                            if ((String.CompareOrdinal(xn.InnerText.ToLower(), prametr) < 0) && (xn.Name.ToLower() == kolumna0)) //mniejsze
                            {
                                wypisz = true;
                                break;
                            }

                        }
                        if (wypisz)
                        {
                            if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                            {


                                XmlNodeList lstCar = xnList[i].ChildNodes;



                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                                    else
                                    {
                                        string wstawka = "";

                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");

                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                XmlNodeList lstCar = xnList[i].ChildNodes;
                                bool dalej = false;
                                List<string> kolumny = new List<string>();

                                for (int l = 0; l < lstCar.Count; l++)
                                {
                                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                                    {
                                        if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                        {
                                            kolumny.Add(lstCar[l].Name.ToLower());
                                            dalej = true;
                                        }
                                    }
                                }

                                if (dalej == false)
                                {
                                    string msg = "błąd w nazwie kolumny";
                                    new Error(msg);
                                    return;
                                }

                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    for (int m = 0; m < kolumny.Count; m++)
                                    {
                                        if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                        {

                                            if (lstCar[j].InnerText.Count() > 12)
                                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                            else
                                            {
                                                string wstawka = "";
                                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                                    wstawka = string.Concat(wstawka, " ");
                                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine();
                            }
                        }
                    }
 
                    break;
                case 2:
                    for (int i = 0; i < xnList.Count; i++)
                    {   //jezeli w sekcji select-from jest tylko *
                        wypisz = false;
                        XmlNodeList lstPar = xnList[i].ChildNodes;
                        foreach (XmlNode xn in lstPar)
                        {
                            if (xn.InnerText.ToLower() == prametr)
                            { 
                                wypisz = true;
                              //  break;
                            }

                        }
                        if (!wypisz)
                        {
                            if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                            {


                                XmlNodeList lstCar = xnList[i].ChildNodes;



                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                                    else
                                    {
                                        string wstawka = "";

                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");

                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                XmlNodeList lstCar = xnList[i].ChildNodes;
                                bool dalej = false;
                                List<string> kolumny = new List<string>();

                                for (int l = 0; l < lstCar.Count; l++)
                                {
                                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                                    {
                                        if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                        {
                                            kolumny.Add(lstCar[l].Name.ToLower());
                                            dalej = true;
                                        }
                                    }
                                }

                                if (dalej == false)
                                {
                                    string msg = "błąd w nazwie kolumny";
                                    new Error(msg);
                                    return;
                                }

                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    for (int m = 0; m < kolumny.Count; m++)
                                    {
                                        if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                        {

                                            if (lstCar[j].InnerText.Count() > 12)
                                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                            else
                                            {
                                                string wstawka = "";
                                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                                    wstawka = string.Concat(wstawka, " ");
                                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine();
                            }
                        }
                    }
 
                    break;
                case 3:
                    for (int i = 0; i < xnList.Count; i++)
                    {   //jezeli w sekcji select-from jest tylko *
                        wypisz = false;
                        XmlNodeList lstPar = xnList[i].ChildNodes;
                        foreach (XmlNode xn in lstPar)
                        {
                             if ( (String.CompareOrdinal(xn.InnerText.ToLower(), prametr)<=0)&&(xn.Name.ToLower()==kolumna0)) //mniejsze
                            {
                            
                                wypisz = true;
                                break;
                            }

                        }
                        if (wypisz)
                        {
                            if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                            {


                                XmlNodeList lstCar = xnList[i].ChildNodes;



                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                                    else
                                    {
                                        string wstawka = "";

                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");

                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                XmlNodeList lstCar = xnList[i].ChildNodes;
                                bool dalej = false;
                                List<string> kolumny = new List<string>();

                                for (int l = 0; l < lstCar.Count; l++)
                                {
                                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                                    {
                                        if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                        {
                                            kolumny.Add(lstCar[l].Name.ToLower());
                                            dalej = true;
                                        }
                                    }
                                }

                                if (dalej == false)
                                {
                                    string msg = "błąd w nazwie kolumny";
                                    new Error(msg);
                                    return;
                                }

                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    for (int m = 0; m < kolumny.Count; m++)
                                    {
                                        if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                        {

                                            if (lstCar[j].InnerText.Count() > 12)
                                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                            else
                                            {
                                                string wstawka = "";
                                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                                    wstawka = string.Concat(wstawka, " ");
                                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine();
                            }
                        }
                    }
 
                    break;
                case 4:
                    for (int i = 0; i < xnList.Count; i++)
                    {   //jezeli w sekcji select-from jest tylko *
                        wypisz = false;
                        XmlNodeList lstPar = xnList[i].ChildNodes;
                        foreach (XmlNode xn in lstPar)
                        {
                            if ((String.CompareOrdinal(xn.InnerText.ToLower(), prametr) >0) && (xn.Name.ToLower() == kolumna0)) //mniejsze
                            {

                                wypisz = true;
                                break;
                            }

                        }
                        if (wypisz)
                        {
                            if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                            {


                                XmlNodeList lstCar = xnList[i].ChildNodes;



                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                                    else
                                    {
                                        string wstawka = "";

                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");

                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                XmlNodeList lstCar = xnList[i].ChildNodes;
                                bool dalej = false;
                                List<string> kolumny = new List<string>();

                                for (int l = 0; l < lstCar.Count; l++)
                                {
                                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                                    {
                                        if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                        {
                                            kolumny.Add(lstCar[l].Name.ToLower());
                                            dalej = true;
                                        }
                                    }
                                }

                                if (dalej == false)
                                {
                                    string msg = "błąd w nazwie kolumny";
                                    new Error(msg);
                                    return;
                                }

                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    for (int m = 0; m < kolumny.Count; m++)
                                    {
                                        if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                        {

                                            if (lstCar[j].InnerText.Count() > 12)
                                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                            else
                                            {
                                                string wstawka = "";
                                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                                    wstawka = string.Concat(wstawka, " ");
                                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine();
                            }
                        }
                    }
 
                    break;
                case 5:
                    for (int i = 0; i < xnList.Count; i++)
                    {   //jezeli w sekcji select-from jest tylko *
                        wypisz = false;
                        XmlNodeList lstPar = xnList[i].ChildNodes;
                        foreach (XmlNode xn in lstPar)
                        {
                            if ((String.CompareOrdinal(xn.InnerText.ToLower(), prametr) >= 0) && (xn.Name.ToLower() == kolumna0)) //mniejsze
                            {

                                wypisz = true;
                                break;
                            }

                        }
                        if (wypisz)
                        {
                            if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                            {


                                XmlNodeList lstCar = xnList[i].ChildNodes;



                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                                    else
                                    {
                                        string wstawka = "";

                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");

                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                XmlNodeList lstCar = xnList[i].ChildNodes;
                                bool dalej = false;
                                List<string> kolumny = new List<string>();

                                for (int l = 0; l < lstCar.Count; l++)
                                {
                                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                                    {
                                        if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                        {
                                            kolumny.Add(lstCar[l].Name.ToLower());
                                            dalej = true;
                                        }
                                    }
                                }

                                if (dalej == false)
                                {
                                    string msg = "błąd w nazwie kolumny";
                                    new Error(msg);
                                    return;
                                }

                                for (int j = 0; j < lstCar.Count; j++)
                                {
                                    for (int m = 0; m < kolumny.Count; m++)
                                    {
                                        if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                        {

                                            if (lstCar[j].InnerText.Count() > 12)
                                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                            else
                                            {
                                                string wstawka = "";
                                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                                    wstawka = string.Concat(wstawka, " ");
                                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    break;
                default:
                    break;

            }
        }



        public List<string> getSelectList(List<string> zapytanieSql)
        {
            List<string> selectList = new List<string>();

            selectList.Add(zapytanieSql.ElementAt(1));

            for (int i = 2; i < zapytanieSql.IndexOf("from"); i+=2)
            {
                if ((zapytanieSql.ElementAt(i)==","))
                    selectList.Add(zapytanieSql.ElementAt(i+1));
            }
              return selectList;
        }
   
        public void wypiszKolumny(XmlNode wezelGlowny,XmlDocument xml,List<string> zapytanieSql)
        {
            string selectnd = string.Format("root/{0}/{1}", wezelGlowny.Name, wezelGlowny.FirstChild.Name);

            XmlNodeList xnList = xml.SelectNodes(selectnd);
            //jeżeli select * from
            if (zapytanieSql.ElementAt(1) == "*")
            {
                XmlNodeList lst = wezelGlowny.FirstChild.ChildNodes;

                for (int n = 0; n < lst.Count; n++)
                {
                    if (lst[n].Name.Trim().Count() > 12)
                        Console.Write("{0}", lst[n].Name.Remove(12) + "\t");
                    else
                    {
                        string wstawka = "";

                        for (int k = 0; k < 12 - lst[n].Name.Count(); k++)
                            wstawka = string.Concat(wstawka, " ");
                        Console.Write("{0}", string.Concat(lst[n].Name, wstawka) + "\t");
                    }
                }

                Console.WriteLine();
                for (int x = 0; x < 80; x++)
                    Console.Write("-");
            }
            else
            {
                XmlNodeList lst = wezelGlowny.FirstChild.ChildNodes;
                bool dalej0 = false;
                List<string> kolumny0 = new List<string>();

                for (int l = 0; l < lst.Count; l++)
                {
                    for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                    {
                        if (lst[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                        {
                            kolumny0.Add(lst[l].Name.ToLower());
                            dalej0 = true;
                        }
                    }
                }

                if (dalej0 == false)
                {
                    string msg = "błąd w nazwie kolumny";
                    new Error(msg);
                    return;
                }

                for (int j = 0; j < lst.Count; j++)
                {
                    for (int m = 0; m < kolumny0.Count; m++)
                    {
                        if (lst[j].Name.ToLower() == kolumny0.ElementAt(m).ToLower())
                        {

                            if (lst[j].Name.Count() > 12)
                                Console.Write("{0}", lst[j].Name.Trim().Remove(12) + "\t");
                            else
                            {
                                string wstawka = "";
                                for (int k = 0; k < 12 - lst[j].Name.Count(); k++)
                                    wstawka = string.Concat(wstawka, " ");
                                Console.Write("{0}\t", string.Concat(lst[j].Name.Trim(), wstawka));
                            }
                        }
                    }
                }
                Console.WriteLine();
                for (int x = 0; x < 80; x++)
                    Console.Write("-");
            }

        }

        public void wypiszWszystkieRekordy(XmlNodeList xnList,List<string> zapytanieSql)
        {
             for (int i = 0; i < xnList.Count; i++)
                {   //jezeli w sekcji select-from jest tylko *
                    if ((getSelectList(zapytanieSql).Count == 1) && (getSelectList(zapytanieSql).ElementAt(0) == "*"))
                    {


                        XmlNodeList lstCar = xnList[i].ChildNodes;



                        for (int j = 0; j < lstCar.Count; j++)
                        {
                            if (lstCar[j].InnerText.Count() > 12)
                                Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");

                            else
                            {
                                string wstawka = "";

                                for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                    wstawka = string.Concat(wstawka, " ");

                                Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                            }
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        XmlNodeList lstCar = xnList[i].ChildNodes;
                        bool dalej = false;
                        List<string> kolumny = new List<string>();

                        for (int l = 0; l < lstCar.Count; l++)
                        {
                            for (int k = 0; k < getSelectList(zapytanieSql).Count; k++)
                            {
                                if (lstCar[l].Name.ToLower() == getSelectList(zapytanieSql).ElementAt(k).ToLower())
                                {
                                    kolumny.Add(lstCar[l].Name.ToLower());
                                    dalej = true;
                                }
                            }
                        }

                        if (dalej == false)
                        {
                            string msg = "błąd w nazwie kolumny";
                            new Error(msg);
                            return;
                        }

                        for (int j = 0; j < lstCar.Count; j++)
                        {
                            for (int m = 0; m < kolumny.Count; m++)
                            {
                                if (lstCar[j].Name.ToLower() == kolumny.ElementAt(m).ToLower())
                                {

                                    if (lstCar[j].InnerText.Count() > 12)
                                        Console.Write("{0}", lstCar[j].InnerText.Trim().Remove(12) + "\t");
                                    else
                                    {
                                        string wstawka = "";
                                        for (int k = 0; k < 12 - lstCar[j].InnerText.Count(); k++)
                                            wstawka = string.Concat(wstawka, " ");
                                        Console.Write("{0}\t", string.Concat(lstCar[j].InnerText.Trim(), wstawka));
                                    }
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
                    }

        public string sprawdzKolumne(string nazwaKolummny,XmlNodeList xnList0)
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


        public string typKolumnyPar(string nazwaTabeli,XmlDocument xml,string kolumna0)
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
