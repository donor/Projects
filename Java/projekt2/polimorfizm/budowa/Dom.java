package budowa;

public class Dom extends Budynek
{

public void dodajPomieszczenie(int szerokosc,int dlugosc) //dotyczy przeciazania
{
    int szerokosc1=szerokosc;
    int dlugosc1=dlugosc;
    System.out.println("Dodano pomieszczenie o dlugosci "+dlugosc1+" m i szerokosci "+szerokosc1+" m");

}

public int zmienWysokosc(int zmiana) //przeslanianie
{

    return zmiana-30;
}

public void dodajDach(RodzajDachu rd)  //dotyczy rzutowania
{
    System.out.println("Dodano dach pokryty "+rd);
}

    public static void dach(Budynek b) //dotyczy rzutowania
    {
        b.dodajDach(RodzajDachu.dachowka);
    }

    public static void main(String[] args)
    {
        Dom dom=new Dom();

        dom.dodajPomieszczenie(); //przeciazanie
        dom.dodajPomieszczenie(5, 8); //przeciazanie
        
        System.out.println("Zmienion wysokosc domu o "+dom.zmienWysokosc(100)+" cm"); //przeslanianie

        dach(dom); //rzutowanie
    }
}