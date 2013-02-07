package dendrologia;

public class Topola extends Drzewo
{
    public void usunLisc()
    {
        String rodzaj="lisciaste";
        super.usunLiscIgla();
        
    }
    public void wiekWysokoscObwod() //wykorzystanie pól protected
    {
        System.out.println("To drzewo ma "+wiek+" , wysokośc wynosi: "+wysokosc+" obwod ma: "+obwod);

    }


public void dodajLisc()
    {
        String rodzaj="lisciaste";
        super.dodajLiscIgla();

    }

    public static void main(String[] args)
    {
        Topola topola=new Topola();
        topola.wiekWysokoscObwod();
        topola.dodajGalaz();
        topola.dodajLisc();
        topola.usunLisc();
        topola.usunGalaz();
    }
}
