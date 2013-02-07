package telefon;
import zasilanie.Ladowarka;

public class Komorka
{
  byte status=1; //komorka naladowana
  private int czasStanuKomorki;

public void naladowana(int czasStanuKomorki)
{
     
    status=1; //komórka nalodowana
    System.out.println("Czas kiedy komorka jest naladowana: "+czasStanuKomorki);
}

 void ladowanie(int czasStanuKomorki)
{
    System.out.println("Komorka jest ladowana");
    

    
    status=0; //komórka jest ladowana
System.out.println("Czas ladowania komorki: "+czasStanuKomorki);
}

protected  void rozladowanie(int czasStanuKomorki)
{
    System.out.println("Komorka jest rozladowana");
    
   status=-1; //komórka rozladowana
   System.out.println("Czas rozladowania komorki: "+czasStanuKomorki);
}


  public  static void main(String[] args)
  {
Komorka komorka=new Komorka();
Ladowarka ladowarka=new Ladowarka();

switch ((int)(Math.random()*3))
    {
        case 0: komorka.status=-1; break;
        case 1: komorka.status=0; break;
        default: komorka.status=1;
    }
if (komorka.status==-1)
{
    komorka.rozladowanie(200);
    ladowarka.wlacz(500);
}

if (komorka.status==0)
    komorka.ladowanie(100);

if (komorka.status==1)
{
    komorka.naladowana(800);

}


    }
}

