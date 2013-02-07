using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RaiDSyM
{
    class Blad
    {

        public static int x;
        public static int y;
        public static bool  dalej = false;
        public static bool bladWielokrotny = false;
        public static int bladDysku = 0;
        public static int numPaska = 0;
        public static int nrPas = 0;
        public static int nrWLiscie = 0;

        public static bool sprawdzIstnieniePaska(int nrDysku, int nrPaska, int typRaida)
        {
            Load ld = new Load();

            switch (typRaida)
            {
                case 0:
                    if (nrDysku == 0)
                    {
                        for (int i=0;i<127;i+=2)
                        {
                            if (nrPaska == i)
                            {                              
                                Przeplyw.blad = true;
                                dalej = true;
                                Przeplyw.paski.ElementAt(nrPaska).kolor = System.Drawing.Color.Red;
                               
                                String komunikat = string.Format("Wystąpił bład w Dysku 0, w pasku {0},"+
                                                                " Zapis został wstrzymany, ponieważ ten błąd uniemożliwia odczyt zapisanych" +
                                                                 " danych w Raid0 (dalszy zapis nie ma sensu)",nrPaska );

                                MessageBox.Show(komunikat,"Błąd przekraczający zakres tolerancji symulowanego układu");
                                ld.zapis(komunikat);

                                break;
                            }
                        }                        
                    }
                    else if (nrDysku == 1)
                    {
                        for (int i = 1; i < 128; i += 2)
                        {
                            if (nrPaska == i)
                            {
                                Przeplyw.blad = true;
                                dalej = true;
                                Przeplyw.paski.ElementAt(nrPaska).kolor = System.Drawing.Color.Red;

                                String komunikat = string.Format("Wystąpił bład w Dysku 1, w pasku {0}," +
                                                                " Zapis został wstrzymany, ponieważ ten błąd uniemożliwia odczyt zapisanych" +
                                                                 " danych w Raid0 (dalszy zapis nie ma sensu)", nrPaska);

                                MessageBox.Show(komunikat, "Błąd przekraczający zakres tolerancji symulowanego układu");
                                ld.zapis(komunikat);
                                break;
                            }
                        }
                    }
                    break;
                case 1:
                    try
                    {
                        if (RaiDSyM.bladWielokrotny)
                        {
                            for (int i = 0; i < 127; i++)
                            {
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    int dysk=1;
                                    if (i%2==0)
                                        dysk=0;
                                    Przeplyw.paski.ElementAt(i).kolor =System.Drawing.Color.Red;
                                    string komunikat=string.Format("Wystąpił błąd w pasku {0}, w dysku {1}",Przeplyw.paski.ElementAt(i).nrPasku,dysk);
                                    ld.zapis(komunikat);
                                    
                                }
                            bladWielokrotny = true;
                            }
                          
                        }

                        else
                        {
                            if (nrDysku == 0)
                            {
                                for (int i = 0; i < 127; i+=2)
                                {
                                    if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                    {
                                        Przeplyw.blad = true;
                                        dalej = true;
                                        Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                        x = Przeplyw.paski.ElementAt(i).x;
                                        y = Przeplyw.paski.ElementAt(i).y;
                                        String komunikat = string.Format("Wystąpił bład w Dysku 0, w pasku {0}," +
                                                                       "Błąd zostanie niebawem naprawiony poprzez skopiowania paska {0} z Dysku 1 do Dysku 0 ", nrPaska);

                                        MessageBox.Show(komunikat, "Błąd naprawialny przez układ, nie  przekraczający zakresu tolerancji symulowanego układu");
                                        ld.zapis(komunikat);
                                        break;
                                    }
                                }
                            }
                            if (nrDysku == 1)
                            {
                                for (int i = 1; i < 127; i += 2)
                                {
                                    if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                    {
                                        Przeplyw.blad = true;
                                        dalej = true;
                                        Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                        x = Przeplyw.paski.ElementAt(i).x;
                                        y = Przeplyw.paski.ElementAt(i).y;
                                        String komunikat = string.Format("Wystąpił bład w Dysku 1, w pasku {0}," +
                                                                                                           "Błąd zostanie niebawem naprawiony poprzez skopiowania paska {0} z Dysku 0 do Dysku 1 ", nrPaska);

                                        MessageBox.Show(komunikat, "Błąd naprawialny przez układ, nie  przekraczający zakresu tolerancji symulowanego układu");
                                        ld.zapis(komunikat);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    break;
                case 3:
                    try
                    {

                        if (RaiDSyM.bladWielokrotny)
                        {
                         
                             if (nrDysku == 0)
                        {
                            for (int i = 0; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                 
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    Przeplyw.paski.ElementAt(i+1).kolor = System.Drawing.Color.Red;
                        
                                    string komunikat = string.Format("Wystąpił błąd w dysku 0 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    komunikat = string.Format("Wystąpił błąd w dysku 1 w pasku {0} zawartość paska: null", nrPaska+1);
                                    ld.zapis(komunikat);
                                   
                                    bladWielokrotny = true;
                                    break;
                                }

                            }
                        }
                        else if (nrDysku == 1)
                        {
                            for (int i = 1; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {

                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    Przeplyw.paski.ElementAt(i + 1).kolor = System.Drawing.Color.Red;

                                    string komunikat = string.Format("Wystąpił błąd w dysku 1 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    komunikat = string.Format("Wystąpił błąd w dysku 2 w pasku {0} zawartość paska: null", nrPaska + 1);
                                    ld.zapis(komunikat);

                                    bladWielokrotny = true;
                                    break;
                                }

                            }
                        }
                        else if (nrDysku == 2)
                        {
                            for (int i = 2; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    Przeplyw.paski.ElementAt(i + 1).kolor = System.Drawing.Color.Red;

                                    string komunikat = string.Format("Wystąpił błąd w dysku 2 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    komunikat = string.Format("Wystąpił błąd w dysku 3 w pasku {0} zawartość paska: null", nrPaska + 1);
                                    ld.zapis(komunikat);

                                    bladWielokrotny = true;
                                    break;
                                }

                            }
                        }
                        else if (nrDysku == 3)
                        {
                            for (int i = 3; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    Przeplyw.paski.ElementAt(i -1).kolor = System.Drawing.Color.Red;

                                    string komunikat = string.Format("Wystąpił błąd w dysku 3 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    komunikat = string.Format("Wystąpił błąd w dysku 2 w pasku {0} zawartość paska: null", nrPaska*3+2 );
                                    ld.zapis(komunikat);

                                    bladWielokrotny = true;
                                    break;
                                }
                            }
                            }
                        
                            
                        }

                        else
                        {

                        if (nrDysku == 0)
                        {
                            for (int i = 0; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    nrWLiscie = i;
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    x = Przeplyw.paski.ElementAt(i).x;
                                    y = Przeplyw.paski.ElementAt(i).y;
                                  
                                    Przeplyw.dysk0.Insert(nrPas, null);  //{2;2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2};

                                    string komunikat = string.Format("Wystąpił błąd w dysku 0 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    dalej = true;
                                    Przeplyw.blad = true;
                                    bladDysku = 0;
                                    numPaska = nrPaska;
                                    break;
                                }

                            }
                        }
                        else if (nrDysku == 1)
                        {
                            for (int i = 1; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    nrWLiscie = i;
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    x = Przeplyw.paski.ElementAt(i).x;
                                    y = Przeplyw.paski.ElementAt(i).y;
                                    Przeplyw.dysk1.Insert(nrPas, null);  //{2;2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2};
                                    string komunikat = string.Format("Wystąpił błąd w dysku 1 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    dalej = true;
                                    Przeplyw.blad = true;
                                    bladDysku = 1;
                                    numPaska = nrPaska;

                                    break;
                                }

                            }
                        }
                        else if (nrDysku == 2)
                        {
                            for (int i = 2; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    nrWLiscie = i;
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    x = Przeplyw.paski.ElementAt(i).x;
                                    y = Przeplyw.paski.ElementAt(i).y;
                                    Przeplyw.dysk2.Insert(nrPas, null);  //{2;2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2};
                                    string komunikat = string.Format("Wystąpił błąd w dysku 2 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    dalej = true;
                                    Przeplyw.blad = true;
                                    bladDysku = 2;
                                    numPaska = nrPaska;
                                    break;
                                }

                            }
                        }
                        else if (nrDysku == 3)
                        {
                            for (int i = 3; i < Przeplyw.paski.Count; i += 4)
                            {
                                nrPas = i / 4;
                                if (Przeplyw.paski.ElementAt(i).nrPasku == nrPaska)
                                {
                                    nrWLiscie = i;
                                    Przeplyw.paski.ElementAt(i).kolor = System.Drawing.Color.Red;
                                    x = Przeplyw.paski.ElementAt(i).x;
                                    y = Przeplyw.paski.ElementAt(i).y;
                                    Przeplyw.dysk3.Insert(nrPas, null);  //{2;2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2};
                                    string komunikat = string.Format("Wystąpił błąd w dysku 3 w pasku {0} zawartość paska: null", nrPaska);
                                    ld.zapis(komunikat);
                                    dalej = true;
                                    Przeplyw.blad = true;
                                    bladDysku = 3;
                                    numPaska = nrPaska;
                                    break;
                                }
                            }
                            }
                        }
                    }
                    catch (Exception excep)
                    { }
                    break;
                default:

                    break;
            }
            
            return dalej;
        }
    }
}
