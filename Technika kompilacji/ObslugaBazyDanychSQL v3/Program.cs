using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ObslugaBazyDanychSQL
{
    class Program
    {
        public readonly static string fileName = "BazaDanych0.xml";

        private static Selecty select;
        private static Inserty insert;
        private static Delety delete;
        private static Updaty update;
        private static CreateTable creatTable;
        private static DropTable dropTable;
        private static AlterTable alterTable;

        private static List<string> slowa;
        private static List<string> wowa;

        static void Main(string[] args)
        {
             Console.WriteLine("*Program Do Obsługi Bazy danych w pliku XML przy pomocy zapytań SQL*");
            
            do
            {
                try
                {
                    slowa = null;
               
                    slowa =zapytanieToSlowa(Console.ReadLine());
                   // int wev= wowa.Count;
                    
                    bool kontynuluj = false;
                    if ((slowa != null)&&(slowa.Count>0))
                    {


                    if ((slowa.Last() != ";"))
                    {
                        new Error("Brak znaku ';' na końcu ciągu");
                        kontynuluj = true;
                    }
                    if (kontynuluj == false)
                    {
                        bool blad = false;
                        if (slowa.ElementAt(0).ToLower() == "select")
                        {
                            select = new Selecty(slowa, fileName);
                            select.getSelectList(slowa);
                            blad = true;
                        }
                        if ((slowa.ElementAt(0).ToLower() == "insert") && (slowa.ElementAt(1).ToLower() == "into"))
                        {
                            insert = new Inserty(slowa, fileName);
                            blad = true;
                        }

                        if ((slowa.ElementAt(0).ToLower() == "delete") && (slowa.ElementAt(1).ToLower() == "from"))
                        {
                            delete = new Delety(slowa, fileName);
                            blad = true;
                        }

                        if ((slowa.ElementAt(0).ToLower() == "update"))
                        {
                            update = new Updaty(slowa, fileName);
                            blad = true;
                        }
                        if ((slowa.ElementAt(0).ToLower() == "drop") && (slowa.ElementAt(1).ToLower() == "table"))
                        {
                            dropTable = new DropTable(slowa, fileName);
                            blad = true;
                        }
                        if ((slowa.ElementAt(0).ToLower() == "alter") && (slowa.ElementAt(1).ToLower() == "table"))
                        {
                            alterTable = new AlterTable(slowa, fileName);
                            blad = true;
                        }
                        List<string> toCreater = new List<string>();
                        toCreater = wytnijSpacie(wowa);
                        if ((toCreater.ElementAt(0).ToLower() == "create") && (toCreater.ElementAt(1).ToLower() == "table") && (toCreater.ElementAt(3).ToLower() == "(") && (toCreater.ElementAt(toCreater.Count - 2).ToLower() == ")"))
                        {
                            creatTable = new CreateTable(toCreater, fileName);
                            blad = true;
                        }
                        else if (!blad)
                            new Error("Błąd w pierwszynm słowie polecenia");
                    }
                    }
                }
                catch (ArgumentOutOfRangeException exp)
                {

                    new Error(exp.ToString());
                }
slowa = null;
             
            }
            while (true);
        }
        public static List<string> wytnijSpacie(List<string> lista)
        {
            List<string> poEdycji=new List<string>();

            for (int i = 0; i < lista.Count; i++)
            {
                if ((!String.IsNullOrEmpty(lista.ElementAt(i))) && (lista.ElementAt(i) != " ") && (lista.ElementAt(i) != ""))
                {
                    poEdycji.Add(lista.ElementAt(i));
                }
            }
            return poEdycji;
        }

        public static List<string> zapytanieToSlowa(String zapytanieSql)
        {
            string[] words = Regex.Split(zapytanieSql, @"( )|(,)|([(])|([)])|(;)|(=)|(<)|(>)|(')"); /*|("")*/

            wowa= new List<string>();
           
             char[] wytnij={/*' ',',','"','.',',*/'\x0027'};  //nie potrzbne

             for (int i = 0; i < words.Length; i++)
             {
                 //words[i] = words[i];
                    wowa.Add(words[i]);
             }

          
            //for (int i = 0; i < words.Length; i++)
            //     words[i]= words[i].Trim(wytnij);
             int licznik = 0;
             for (int i = 0; i < words.Length; i++)
             {
                 if (words[i] == "'")
                 {
                     licznik += 1;
                    
                 }
             }
             if (licznik % 2 != 0)
             {
                 new Error("Problem z znakiem ' ");
                 return null;
             }

            List<string>  list = new List<string>();
            string temp = null;
             for (int i = 0; i < words.Length; i++)
             {
                 if (words[i] == "'")
                 {
                     temp = "'";
                     i++;
                     do
                     {
                         temp=temp.Insert(temp.Length, words[i]);
                         i++;

                     }
                     while (words[i] != "'");
                     temp = temp.Insert(temp.Length,"'");
                     list.Add(temp.ToLower());

                 }
                 else if ((!string.IsNullOrEmpty(words[i]) && (words[i] != " ")))
                     list.Add(words[i].ToLower());
             }

          

           //foreach (string ele in list)
           //   {
           //       Console.WriteLine(ele);
           //   }

            return list;
        }
    }
}
