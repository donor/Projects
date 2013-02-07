using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RaiDSyM
{
    class Przeplyw
    {
        private static readonly string fileName = "Wejscie.xml";
        public static List<Pasek> paski = new List<Pasek>();
        public static List<Pasek> paski1 = new List<Pasek>();

        public static List<byte[]> wejscie;
        public static List<byte[]> dysk0;
        public static List<byte[]> dysk1;
        public static List<byte[]> dysk2;
        public static List<byte[]> dysk3;

        public static int numerPaska = 0;

        private Load ld;
        public int x=0;
        public int y=85;
        private int pasekParzystosci = 0;

        public static bool blad = false;

        public Przeplyw()
        {
            ld = new Load();
           // KontrolaParzystosci kp = new KontrolaParzystosci();
            //Blad bl = new Blad();
            wejscie =ld.pobierzPaskiZPliku(fileName);
            //wejscie = ld.paskiWejsciowe;/

            if ((RaiDSyM.typRaida==0)||(RaiDSyM.typRaida==1))
            {
                dysk0=new List<byte[]>();
                dysk1=new List<byte[]>();
            }
            if ((RaiDSyM.typRaida == 0) || (RaiDSyM.typRaida == 1) || (RaiDSyM.typRaida == 3))
            {
                dysk0 = new List<byte[]>();
                dysk1 = new List<byte[]>();
                dysk2 = new List<byte[]>();
                dysk3 = new List<byte[]>();

            }

            //rds = new RaiDSyM();
            Thread thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }
        public  void Run()
        {
            int kolumna=0;
            int wiersz=0;
             numerPaska = 0; //zmienna która odpawiada za przekazywanie do dysków odpowiedniego paska
            

            while ((true)&&(!Blad.bladWielokrotny))
            {
                if (!blad)
                {
                    switch (RaiDSyM.typRaida)
                    {
                        case 0:
                            for (int i = 0; i < 460; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (!blad)
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk0.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 0 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk0.Last()));
                            numerPaska++;
                            x = 0;
                            y = 85;
                            for (int i = 0; i < 680; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                                wiersz++;
                                kolumna = -1;
                            }
                            if (!blad)
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk1.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 1 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk1.Last()));
                            numerPaska++;

                            x = 0;
                            y = 85;
                            kolumna++;
                            if (numerPaska >= 127)
                            {
                                ld.zapis("Przerwano zapis danych do Raid0, dyski są pełne");
                                MessageBox.Show("Dyski są pełne, przerywamy transfer danych");
                                return;
                            }
                            break;
                        case 1:
                            for (int i = 0; i < 460; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk0.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 0 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk0.Last()));
                            //numerPaska++;
                            x = 0;
                            y = 85;
                            for (int i = 0; i < 680; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                                wiersz++;
                                kolumna = -1;
                            }
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk1.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 1 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk1.Last()));
                            numerPaska++;
                            x = 0;
                            y = 85;
                            kolumna++;
                            if (numerPaska >= 64)
                            {
                                ld.zapis("Przerwano zapis danych do Raid1, dyski są pełne");
                                MessageBox.Show("Dyski są pełne, przerywamy transfer danych");
                                return;
                            }
                            break;
                        case 3:
                            for (int i = 0; i < 460; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk0.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 0 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk0.Last()));
                            x = 460;
                            y = 250;
                            for (int i = 0; i < 165; i++)
                            {

                                y--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 660; i++)
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            ld.zapis("Do Kontrola Parzystości trafił pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk0.Last()));

                            numerPaska++;
                            x = 0;
                            y = 85;

                            for (int i = 0; i < 680; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk1.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 1 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk1.Last()));
                            x = 680;
                            y = 250;
                            for (int i = 0; i < 165; i++)
                            {

                                y--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 440; i++)
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            ld.zapis("Do Kontrola Parzystości trafił pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk1.Last()));

                            numerPaska++;
                            x = 0;
                            y = 85;

                            for (int i = 0; i < 900; i++) //lot w prawo po szynie do dysku 0
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                                //wiersz++;
                                //kolumna = -1;
                            }

                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); ///100 to bedzie numer pasku
                            dysk2.Add(wejscie.ElementAt(numerPaska));
                            ld.zapis("Do dysku 2 zapisano pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk2.Last()));
                            x = 900;
                            y = 250;
                            for (int i = 0; i < 165; i++)
                            {

                                y--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 220; i++)
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            ld.zapis("Do Kontrola Parzystości trafił pasek " + numerPaska + ", zawartość: " + byteArrayToString(dysk2.Last()));

                            dysk3.Add(KontrolaParzystosci.liczBityParzystości(dysk0.Last(), dysk1.Last(), dysk2.Last()));
                            ld.zapis("Do dysku Parzystości zapisano pasek o zawartość: " + byteArrayToString(dysk3.Last()));

                            x = 1120;
                            y = 85;
                            int num = numerPaska;
                            numerPaska = dysk3.Count-1;

                            for (int i = 0; i < 165; i++) //lot w dol po szynie do dysku 0
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < wiersz * 30; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            if (kolumna == 0)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 1)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x--;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 2)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                            }
                            if (kolumna == 3)
                            {
                                for (int i = 0; i < 60; i++)
                                {
                                    x++;
                                    Thread.Sleep(RaiDSyM.sleepTime);
                                }
                                wiersz++;
                                kolumna = -1;
                            }
                            numerPaska = num;
                            int temp = numerPaska;
                            numerPaska = pasekParzystosci;
                            paski.Add(new Pasek(x, y, numerPaska, Color.Black)); 



                            pasekParzystosci++;
                            numerPaska = temp + 1;
                            x = 0;
                            y = 85;
                            kolumna++;








                            if (numerPaska >= 191)
                            {
                                ld.zapis("Przerwano zapis danych do Raid3, dyski są pełne");
                                MessageBox.Show("Dyski są pełne, przerywamy transfer danych");
                                return;
                            }
                            break;
                        default:

                            break;
                    }


                }
                else if (RaiDSyM.typRaida == 3)
                {
int tempx = x;
                        int tempy = y;
                        int tempNrPaska = numerPaska;

                        numerPaska = RaiDSyM.nrPasku;

                    if (Blad.bladDysku == 0)
                    {
                        

                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        
                            x = 680;
                            y = 250;
                            numerPaska++;
                            RaiDSyM.nrPasku=numerPaska;
                            paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                            for (int i = 0; i < 165; i++)
                            {
                                y--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            
                            for (int i = 0; i < 420; i++)
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            string komunikat = string.Format("Do kontroler parzystości z dysku 1 trafił pasek {0} o zawartości {1} ", Blad.numPaska+1, byteArrayToString(dysk1.ElementAt(Blad.nrPas)));
                            ld.zapis(komunikat);
                            x = 900;
                            y = 250;
                            numerPaska++;
                            RaiDSyM.nrPasku = numerPaska;
                            paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                            for (int i = 0; i < 165; i++)
                            {
                                y--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 200; i++)
                            {
                                x++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            komunikat = string.Format("Do kontroler parzystości z dysku 2 trafił pasek {0} o zawartości {1} ", Blad.numPaska + 2, byteArrayToString(dysk2.ElementAt(Blad.nrPas)));
                            ld.zapis(komunikat);
                            x = 1120;
                            y = 250;
                            numerPaska = Blad.numPaska;
                            RaiDSyM.nrPasku = numerPaska;
                            numerPaska = Blad.nrPas;
                            paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                            for (int i = 0; i < 165; i++)
                            {
                                y--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            komunikat = string.Format("Do kontroler parzystości z dysku Parzystości trafił pasek {0} o zawartości {1} ", Blad.nrPas, byteArrayToString(dysk3.ElementAt(Blad.nrPas)));
                            ld.zapis(komunikat);
                            Przeplyw.dysk0.RemoveAt(Blad.nrPas+1 );
                        dysk0.Insert(Blad.nrPas,KontrolaParzystosci.liczBityParzystości(dysk1.ElementAt(Blad.nrPas),dysk2.ElementAt(Blad.nrPas),dysk3.ElementAt(Blad.nrPas)));
                        komunikat = string.Format("W dysku 0 został naprawiony pasek {0}, poprzez obliczenie jego wartości w kontrolerze parzystości, jego zawartość: {1} ", Blad.numPaska, byteArrayToString(dysk0.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk0.RemoveAt(Blad.nrPas + 1);
                           paski1.Add(new Pasek(x, y,numerPaska, Color.Black));
                            x = 1120;
                            y = 85;
                            for (int i = 0; i < 660; i++)
                            {
                                x--;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            for (int i = 0; i < 165; i++)
                            {
                                y++;
                                Thread.Sleep(RaiDSyM.sleepTime);
                            }
                            paski.ElementAt(Blad.nrWLiscie).kolor = Color.Black;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        
                        blad = false;
                         x = tempx;
                        y = tempy;
                        numerPaska = tempNrPaska;
                        MessageBox.Show("W dysku 0 naprawiono błąd", "błąd tolerowalny przez układ");
                    }

                    if (Blad.bladDysku == 1)
                    {


                        //paski1.Add(new Pasek(x, y, numerPaska-1, Color.Black));

                        x = 460;
                        y = 250;
                        numerPaska--;
                      //  RaiDSyM.nrPasku = numerPaska;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }

                        for (int i = 0; i < 640; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        string komunikat = string.Format("Do kontroler parzystości z dysku 0 trafił pasek {0} o zawartości {1} ", Blad.numPaska - 1, byteArrayToString(dysk0.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        x = 900;
                        y = 250;
                        numerPaska+=2;
                      //  RaiDSyM.nrPasku = numerPaska;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 200; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        komunikat = string.Format("Do kontroler parzystości z dysku 2 trafił pasek {0} o zawartości {1} ", Blad.numPaska + 1, byteArrayToString(dysk2.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        x = 1120;
                        y = 250;
                        numerPaska = Blad.numPaska-1;
                       // RaiDSyM.nrPasku = numerPaska;
                        numerPaska = Blad.nrPas;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        komunikat = string.Format("Do kontroler parzystości z dysku Parzystości trafił pasek {0} o zawartości {1} ", Blad.nrPas/4, byteArrayToString(dysk3.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk1.RemoveAt(Blad.nrPas + 1);
                        dysk1.Insert(Blad.nrPas, KontrolaParzystosci.liczBityParzystości(dysk0.ElementAt(Blad.nrPas), dysk2.ElementAt(Blad.nrPas), dysk3.ElementAt(Blad.nrPas)));
                        komunikat = string.Format("W dysku 1 został naprawiony pasek {0}, poprzez obliczenie jego wartości w kontrolerze parzystości, jego zawartość: {1} ", Blad.numPaska, byteArrayToString(dysk1.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk1.RemoveAt(Blad.nrPas + 1);
                        numerPaska++;
                        //RaiDSyM.nrPasku = numerPaska;
                        numerPaska = RaiDSyM.nrPasku;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        x = 1120;
                        y = 85;
                        for (int i = 0; i < 440; i++)
                        {
                            x--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 165; i++)
                        {
                            y++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        paski.ElementAt(Blad.nrWLiscie).kolor = Color.Black;
                        Thread.Sleep(RaiDSyM.sleepTime);

                        blad = false;
                        x = tempx;
                        y = tempy;
                        numerPaska = tempNrPaska;
                        MessageBox.Show("W dysku 1 naprawiono błąd", "błąd tolerowalny przez układ");
                    }

                    if (Blad.bladDysku == 2)
                    {


                        //paski1.Add(new Pasek(x, y, numerPaska-1, Color.Black));

                        x = 460;
                        y = 250;
                        numerPaska-=2;
                        //  RaiDSyM.nrPasku = numerPaska;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }

                        for (int i = 0; i < 640; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        string komunikat = string.Format("Do kontroler parzystości z dysku 0 trafił pasek {0} o zawartości {1} ", Blad.numPaska - 1, byteArrayToString(dysk0.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        x = 680;
                        y = 250;
                       numerPaska++ ;
                        //  RaiDSyM.nrPasku = numerPaska;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 400; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        komunikat = string.Format("Do kontroler parzystości z dysku 1 trafił pasek {0} o zawartości {1} ", Blad.numPaska + 1, byteArrayToString(dysk1.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        x = 1120;
                        y = 250;
                        numerPaska = Blad.numPaska - 1;
                        // RaiDSyM.nrPasku = numerPaska;
                        numerPaska = Blad.nrPas;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        komunikat = string.Format("Do kontroler parzystości z dysku Parzystości trafił pasek {0} o zawartości {1} ", Blad.nrPas / 4, byteArrayToString(dysk3.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk2.RemoveAt(Blad.nrPas + 1);
                        dysk2.Insert(Blad.nrPas, KontrolaParzystosci.liczBityParzystości(dysk0.ElementAt(Blad.nrPas), dysk1.ElementAt(Blad.nrPas), dysk3.ElementAt(Blad.nrPas)));
                        komunikat = string.Format("W dysku 2 został naprawiony pasek {0}, poprzez obliczenie jego wartości w kontrolerze parzystości, jego zawartość: {1} ", Blad.numPaska, byteArrayToString(dysk2.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk2.RemoveAt(Blad.nrPas + 1);
                        numerPaska++;
                        //RaiDSyM.nrPasku = numerPaska;
                        numerPaska = RaiDSyM.nrPasku;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        x = 1120;
                        y = 85;
                        for (int i = 0; i < 220; i++)
                        {
                            x--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 165; i++)
                        {
                            y++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        paski.ElementAt(Blad.nrWLiscie).kolor = Color.Black;
                        Thread.Sleep(RaiDSyM.sleepTime);

                        blad = false;
                        x = tempx;
                        y = tempy;
                        numerPaska = tempNrPaska;
                        MessageBox.Show("W dysku 1 naprawiono błąd", "błąd tolerowalny przez układ");
                    }

                    if (Blad.bladDysku == 3)
                    {


                        //paski1.Add(new Pasek(x, y, numerPaska-1, Color.Black));

                        x = 460;
                        y = 250;
                        numerPaska = Blad.nrPas*3;
                        //  RaiDSyM.nrPasku = numerPaska;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }

                        for (int i = 0; i < 640; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        string komunikat = string.Format("Do kontroler parzystości z dysku 0 trafił pasek {0} o zawartości {1} ", Blad.nrPas*3, byteArrayToString(dysk0.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        x = 680;
                        y = 250;
                        numerPaska=Blad.nrPas*3+1;
                        //  RaiDSyM.nrPasku = numerPaska;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 400; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        komunikat = string.Format("Do kontroler parzystości z dysku 1 trafił pasek {0} o zawartości {1} ", Blad.nrPas*3+1, byteArrayToString(dysk1.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        x = 900;
                        y = 250;
                        numerPaska = Blad.numPaska - 1;
                        // RaiDSyM.nrPasku = numerPaska;
                        numerPaska = Blad.nrPas*3+2;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 200; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        komunikat = string.Format("Do kontroler parzystości z dysku 2 trafił pasek {0} o zawartości {1} ", Blad.nrPas *3+2, byteArrayToString(dysk2.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk3.RemoveAt(Blad.nrPas + 1);
                        dysk3.Insert(Blad.nrPas, KontrolaParzystosci.liczBityParzystości(dysk0.ElementAt(Blad.nrPas), dysk1.ElementAt(Blad.nrPas), dysk2.ElementAt(Blad.nrPas)));
                        komunikat = string.Format("W dysku 3 został naprawiony pasek {0}, poprzez obliczenie jego wartości w kontrolerze parzystości, jego zawartość: {1} ", Blad.numPaska, byteArrayToString(dysk3.ElementAt(Blad.nrPas)));
                        ld.zapis(komunikat);
                        Przeplyw.dysk3.RemoveAt(Blad.nrPas + 1);
                        numerPaska++;
                        //RaiDSyM.nrPasku = numerPaska;
                        numerPaska = RaiDSyM.nrPasku;
                        paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                        x = 1120;
                        y = 85;
                        
                        for (int i = 0; i < 165; i++)
                        {
                            y++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        paski.ElementAt(Blad.nrWLiscie).kolor = Color.Black;
                        Thread.Sleep(RaiDSyM.sleepTime);

                        blad = false;
                        x = tempx;
                        y = tempy;
                        numerPaska = tempNrPaska;
                        MessageBox.Show("W dysku 3 naprawiono błąd", "błąd tolerowalny przez układ");
                    }
                      
                      
                }

                else if (RaiDSyM.typRaida == 1)
                {// wcześniej trzeba zapamiętać x i y
                    int tempx = x;
                    int tempy = y;
                    int tempNrPaska = numerPaska;

                    numerPaska = RaiDSyM.nrPasku;

                    paski1.Add(new Pasek(x, y, numerPaska, Color.Black));
                    if (RaiDSyM.nrDysku == 0)
                    {
                        x = 680;
                        y = 250;
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 220; i++)
                        {
                            x--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 165; i++)
                        {
                            y++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        ld.zapis("Błąd został naprawiony");
                    }
                    else
                    {
                        x = 460;
                        y = 250;
                        for (int i = 0; i < 165; i++)
                        {
                            y--;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 220; i++)
                        {
                            x++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        for (int i = 0; i < 165; i++)
                        {
                            y++;
                            Thread.Sleep(RaiDSyM.sleepTime);
                        }
                        ld.zapis("Błąd został naprawiony");
                    }

                    paski.Add(new Pasek(Blad.x, Blad.y, numerPaska, Color.Black));
                    blad = false;
                    x = tempx;
                    y = tempy;
                    numerPaska = tempNrPaska;
                }
            }
            if (Blad.bladWielokrotny)
            {
                if (RaiDSyM.typRaida == 1)
                {
                    string komunikat = String.Format("Wystąpił błąd przekraczający zakres tolerancji układu,\n "+
                         " tzn. Dwa paski o tym samym numerze\n"
                        +" zostały uszkodzone i nie można odzyskać danych z tych pasków\n"
                        +" dalszy zapis został wstrzymany");
                    MessageBox.Show(komunikat,"Błąd przekraczający zakres symulowanego układu");
                    ld.zapis(komunikat);
                    ld.zapis("Wstrzymano zapis do Raid");                                       
                }

                if (RaiDSyM.typRaida == 3)
                {
                    string komunikat = String.Format("Wystąpił błąd przekraczający zakres tolerancji układu,\n " +
                         " tzn. Dwa paski w różnych dyskach o tym samym miejscu w dysku\n"
                        + " zostały uszkodzone i nie można odzyskać danych z tych pasków\n"
                        + " dalszy zapis został wstrzymany");
                    MessageBox.Show(komunikat, "Błąd przekraczający zakres symulowanego układu");
                    ld.zapis(komunikat);
                    ld.zapis("Wstrzymano zapis do Raid");
                }

            }
        }
        public String byteArrayToString(byte[] rekord)
        {
            string str=String.Empty;
            for (int i = 0; i < rekord.Length; i++)
                str += rekord.ElementAt(i);

                return str;
        }


    }
}
