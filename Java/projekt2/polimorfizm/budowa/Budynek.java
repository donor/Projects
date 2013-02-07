package budowa;


public class Budynek
{
public enum RodzajDachu
{
    gont, dachowka,blacha
}

private int dlugosc;
private int szerokosc;
private int wysokosc;
private int szerokosc1=3;
private int dlugosc1=6;
private int iloscPomieszczen;
private String kolor;
private String material;
private String nazwa;

Budynek(int bdlugosc,int bszerokosc,int bwysokosc,int biloscPomieszczen,String bkolor,String bmaterial,String bnazwa)
  {
    dlugosc=bdlugosc; szerokosc=bszerokosc;wysokosc=bwysokosc; iloscPomieszczen=biloscPomieszczen; kolor=bkolor; material=bmaterial;nazwa=bnazwa;
    System.out.println("Stworzono "+nazwa+" ktorego dlugosc wynosi "+dlugosc+" m,szerokosc wynosi "+szerokosc+" m, wysokosc wynosi "+wysokosc+" cm ma "+iloscPomieszczen+" pomieszcen, jest "+kolor+" i "+material);
  }

Budynek()
    {
        this(15,10,500,4,"bialy","drewniany","dom");
    }

public void dodajPomieszczenie() //dotyczy przeciazania
{
    System.out.println("Dodano pomieszczenie o szerokosci "+this.szerokosc1+" i dlugosci "+this.dlugosc1);
}

public int zmienWysokosc(int zmiana)
{
    
    return zmiana;
}

public void dodajDach(RodzajDachu rd) //dotyczy rzutowania
{
    System.out.println("Dodano dach");
}



}