#include <string.h>
#include <mem.h>
#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <iostream.h>
#include <dos.h>
#include <string.h>
#include <process.h>


void menu()
{
 clrscr();
 gotoxy(25,3);
 printf("********************************\n");
 gotoxy(27,5);
 printf("JEDNOLINIOWY EDYTOR TEKSTU\n");
 gotoxy(25,7);
 printf("********************************\n");
 gotoxy(5,12);
 printf("1. NOWA EDYCJA [F2]\n");
 gotoxy(5,13);
// printf("2. KONTYNUOWANIE EDYCJI [2]\n");
 gotoxy(2,19);
 printf("Informacje [Enter]");
 gotoxy(55,19);
 printf("Wylacz kalkulator [Esc]");
}

void info()
{
char koniec;
clrscr();
do
{
gotoxy(25,3);
printf("*******************************\n");
gotoxy(35,5);
printf("INFORMACJE");
gotoxy(25,7);
printf("*******************************");
gotoxy(5,10);

printf("Jednoliniowy edytor tekstu, uruchamiany w dowolnym miejscu ekranu\n");
printf("Edytor ma nast©puj¥ce mo¾liwo˜ci: wprowadzanie tekstu, kasowanie\n");
printf("tekstu klawiszami <Backspace> i <Del> przemieszczanie kursora przy\n");
printf("u¾yciu klawiszy nawigacyjnych, uruchamianie warto˜ci pocz¥tkowej \n");
printf("tekstu, zapami©tywanie tekstu po naci˜ni©ciu klawisza <Enter> lub\n");
printf("lub <Tab>, pwr¢t do warto˜ci pocz¥tkowej po naci˜ni©ciu klawisza <Esc>");
 gotoxy(55,25);
printf("Przejdz do Menu [Esc]");
do
{
koniec=getch();

}
while (koniec!=27);
}
while (koniec!=27);
}




void nowy()
{
int s,x,y,k,l,i,l1,l11,l01,l111,edycja,l_z,poz,j,m,x1,y1;                        //l11-liczba wprowadzonych znak¢w
char w[76],z,loc,a[76];                                 //lok-okre˜la miejsce edycji
//string str1(char z);
 clrscr();
 miejsce:
 gotoxy(1,1);
 printf("                                                                  \n");
 printf("                                                                  \n");
  gotoxy(1,1);
 printf("[F3]-okresl miejsce edycji, [F4]-potwierdz miejsce, [F10]-Menu");

/* z=getch();
 if (z==0)
  z=getch();
       if (z==68) goto meni;*/
 gotoxy(3,3);
  do
  {
  z=getch();
  if (z==0)
  z=getch();
       if (z==68) goto meni;
  }
  while (z!=61);
  if (z==61)                 //F1-uruchomienie trybu miejsce ekranu
  {
   do
   {

   edycja=0;
     z=getch();
     if (z==0)
     z=getch() ;

     if (z==72)                  //72-przesun w gore
      {
       if (wherey()==3)
	 gotoxy(wherex(),23);
       else
	 gotoxy((wherex()),(wherey()-1));
      }
     if (z==75)                  //72-przesun w lewo
      {
       if (wherex()==3)
	 gotoxy(77,wherey());
       else
	 gotoxy((wherex()-1),wherey());
      }
     if (z==77)                  //72-przesun w prawo
      {
       if (wherex()==77)
	 gotoxy(3,wherey());
       else
	 gotoxy((wherex()+1),wherey());
      }
     if (z==80)                  //80-przesun w dol
      {
      if (wherey()==23)
	gotoxy(wherex(),3);
      else
       {
	gotoxy((wherex()),(wherey()+1));
       }
      }
     x1=wherex();
     y1=wherey();
     gotoxy(74,1);
     printf("%i:%i",x1,y1);
     gotoxy(x1,y1);

     if (z==68) goto meni;

     }
     while ((z!=62)&((z!=72)|(z!=75)|(z!=77)|(z!=80)|(z!=62)));
     edycja=1;
     s=wherex();
  }
 x=0; y=0;
 l11=0;
 l_z=1;
 i=1;
 if (edycja=1)
  {
    gotoxy(1,1);
 printf("[Enter],[Tab]-zapisz tekst, [Esc]-warto˜† pocz¥tkowa, [F10]-Menu\n");
 printf("[F1]-okre˜l miejsce kolejnej edycji");

     gotoxy(x1,y1);
     do
 {

       poz=wherex();
       z=getch();

       if (z==0)
	{
	 z=getch();

	}
       if (z==59) goto miejsce;
       if (z==68) goto meni;
       if  (z==75)  //przes¢wanie w lewo
	{
	 if (wherex()>(s))
	  {
	   x--;
	   gotoxy(x,y);
	   i--;
	  }
	 if (wherex()==(s))
	 {
	  gotoxy(x,y);
	  i=1;
	 }
	}
       if  (z==77)  //przes¢wanie w prawo
	{
	   if (wherex()==78)
	    {
	     i=l_z;
	     gotoxy(x,y);
	    }
	   if (i<l_z)
	    {
	     x++;
	     gotoxy(x,y);
	     i++;
	    }
	   if (i==l_z)
	    {

	     gotoxy(s+l_z-1,y);
	    }
	}
       if (z==71)  //przes¢wanie na pocz¥tek
	{
	 gotoxy(s,y);
	 i=1;
	}
       if (z==79)  //przes¢wanie na koniec
	{
	 if (s+l_z==78)
	  {
	   gotoxy(78,y);
	   i=l_z;
	  }
	 else
	  {
	   x=s+l_z-1;
	   gotoxy(x,y);
	   i=l_z;
	  }
	}
       if ((z==47)|(z==73)|(z==80)|(z==72)|(z==64)|(z==65)|(z==66)|(z==81)|(z==60)|(z==61)|(z==62)|(z==64)|(z==133)|(z==134))  // znak /
	{
	 gotoxy(5,25);
	 printf("Znak zabroniony");
	 gotoxy(5,25);
	 delay(1000);
	 printf("                   ");
	 gotoxy(x,y);
	// i--;
	}
       if (wherex()>78)
	{
	// i--;

	 gotoxy(5,25);
	 printf("Koniec wiersza!");
	 gotoxy(5,25);
	 delay(1000);
	 printf("                   ");
	 gotoxy(x,y);
	}
       if ((z==13)|(z==9)) //zapamietanie tekstu po wcisnieciu enter lub tab
	{
	 l111=l_z;
	 for (i=1;i<=l_z;i++)
	  a[i]=w[i];
	  j=i;
	 }
       if (z==27)  //uruchomienie wartosci pocz¥tkowej
	{
	 if ((wherex()+l111)<=78)
	 {
	 for (i=1;i<=l_z;i++)
	   w[i]=0;
       //	  gotoxy(wherex(),y);
	 for (i=1;i<=l111-1;i++)
	  {
	   w[i]=a[i];
	  // printf("%c",w[i]);

	    y=wherey();
	    x=wherex()+1;
	    printf("%c",w[i]);
	   // i++;
	    l_z++;

	  }
	  }
	 if ((78-wherex())<l111)
	  {
	   x1=wherex();
	   y1=wherey();
	   gotoxy(5,25);
	 printf("warto˜† pocz¥tkowa si© nie zmiesci w tej linii");
	 gotoxy(5,25);
	 delay(1000);
	 printf("                                                ");
	 gotoxy(x1,y1);
	  }
      //	 i=1;
	// gotoxy(s,y);
	}
       if (z==8)   	//8-backspace usuwanie znaku przed kursorem i przes¢w kurosora o jedn¥ pozycje w lewo
	{
	 if ((wherex()==s)&(l_z==1))
	  {
	   w[1]=0;
	   printf("%c",w[1]);
	   gotoxy(s,y);
	   l_z=0;
	   i=0;
	  }
	 else
	 {
	  if (wherex()>s)
	   {
	    poz=wherex();
	    m=s+i;
	    j=i;
	    w[l_z]=0;
	    for (i;i<=l_z+1;i++)
	     {
	     w[l_z+1]=0;
	     if (i==l_z-1)
	      {
	       w[l_z]=0;
	       w[l_z-1]=0;
	       gotoxy(m-1,y);
	       printf("%c",w[l_z]);
	       m++;
	      }
	     else
	      {
	       w[i-1]=w[i];
	      }
	     gotoxy(m-1,y);
	     printf("%c",w[i]);
	     m++;
	   }
	  l_z--;
	  i=j-1;
	  x--;
	  gotoxy(s+i-1,y);
	}
	}
       }
       if (z==83)   	//83-delete usuwa znak znajduj¥cy si© za kursorem
	{
	 j=i;
	 m=wherex();
	  for (i;i<=l_z;i++)
	   {
	    w[l_z]=0; w[l_z+1]=0;
	    w[i]=w[i+1];
	    printf("%c",w[i]);
	   }
	  gotoxy(m,y);

	  l_z--;
	 i=j;
	}
       if (z==13)
       {

       }
	if (z==68) goto meni;
       else                                                                                                                                                 //
	{
	if  ((z!=47)&(z!=75)&(z!=77)&(z!=71)&(z!=79)&(z!=8)&(z!=0)&(wherex()<=78)&(z!=13)&(z!=83)&(z!=68)&(z!=63)&(z!=9)&(z!=27)&(z!=73)&(z!=80)&(z!=72)&(z!=65)&(z!=66)&(z!=81)&(z!=60)&(z!=61)&(z!=62)&(z!=64)&(z!=133)&(z!=134))
	 {
	  if (i>=l_z)
	   {
	    y=wherey();
	    x=wherex()+1;
	    w[i]=z; printf("%c",w[i]);
	    i++;
	    l_z++;
	   }
	  else
	   {
	    y=wherey();
	    x=wherex()+1;
	    w[i]=z; printf("%c",w[i]);
	    i++;
	   }
	 }
	}


//      } //-zamkniecie for (i=1;i<=78-s;i++)

   //-zamkniecie if (edycja=1)
  }
  while (z!=63);
 }
meni:
{
;
 }
} //-zamkniecie nowy()








void otworz()
{
clrscr();
printf("otworz");
getch();
}


main()
{
char znak;
do
{

menu();
znak=getch();
znak=getch();
switch(znak)
{
 if (znak==0)
   znak=getch();
case 13:info(); break;
case 60:nowy(); break;  //60-F2
//case 50:otworz();
}

}
while (znak!=27);

}