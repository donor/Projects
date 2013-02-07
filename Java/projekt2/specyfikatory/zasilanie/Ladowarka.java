package zasilanie;
import telefon.Komorka;

public class Ladowarka
{
private byte status;
public int czasStanuKomorki;

public void wlacz(int czasStanuKomorki )
        {
        System.out.println("Wlaczono ladowarke");
        status=0;
        System.out.println("Czas wlaczenia komorki: "+czasStanuKomorki);
          
        }

 protected void wylacz(int czasStanuKomorki)
 {
        System.out.println("Wylaczono ladowarke");
        status=1;
        System.out.println("Czas wylaczenia komorki: "+czasStanuKomorki);
        
 }
  public  static void main(String[] args)
  {
      Komorka komorka=new Komorka();
      Ladowarka ladowarka=new Ladowarka();

   komorka.naladowana(400);
       ladowarka.wylacz(700)   ;
        
            }
  }
    

  
