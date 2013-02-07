package dendrologia;

class Drzewo
{
    protected int wiek;
    protected int wysokosc;
    protected int obwod;
    private String rodzajDrzewa;
    private String nazwa;

    Drzewo(int dwiek,int dwysokosc,int dobwod,String drodzajDrzewa,String dnazwa)
    {
    wiek=dwiek; wysokosc=dwysokosc; obwod=dobwod; rodzajDrzewa=drodzajDrzewa; nazwa=dnazwa;
    System.out.println("Dodano "+nazwa+" ktore ma "+wiek+" lat/a, obwod "+obwod+" cm, wysokosc ma  "+wysokosc+"m. Jest to drzewo "+rodzajDrzewa);
    }

    Drzewo()
    {
        this(15,19,90,"lisciaste","topola");
    } 

    public void dodajGalaz()
    {
        System.out.println("Dodano galaz");
    }

    public void usunGalaz()
    {
        System.out.println("Usunieto galaz");
    }


    public void usunLiscIgla()
    {

        String rodzaj=rodzajDrzewa;

        if (rodzaj=="iglaste")
             System.out.println("Usunieto igle");

        if (rodzaj=="lisciaste")
             System.out.println("Usunieto lisc");

    }

    public void dodajLiscIgla()
    {

        String rodzaj=rodzajDrzewa;

        if (rodzaj=="iglaste")
             System.out.println("Dodano igle");

        if (rodzaj=="lisciaste")
             System.out.println("Dodano lisc");

    }

}
