#include <string.h>
#include <mem.h>
#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <iostream.h>
#include <dos.h>
#include <string.h>
#include <process.h>


void nowy()
{
int s,x,y,k,l,i,l1,l11,l01,l111,edycja,l_z,poz,j,m,x1,y1;                        //l11-liczba wprowadzonych znak¢w
char w[76],z,loc;                                 //lok-okre˜la miejsce edycji
//string str1(char z);

clrscr();
 miejsce:
 gotoxy(1,1);
 printf("[F3]-okresl miejsce edycji, [F4]-potwierdz miejsce");
// printf(", [F5]-rozpocznij edycje");

 gotoxy(3,3);
  do
  {
  z=getch();
  z=getch();
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

     }
     while ((z!=62)&((z!=72)|(z!=75)|(z!=77)|(z!=80)|(z!=62)));
     edycja=1;
     s=wherex();
  }
 x=0; y=0;
 l11=0;
 l_z=1;
 i=0;
 if (edycja=1)
  {

    do
    {
       z=getch();
      poz=wherex();
       if (z==0)
	{
	 z=getch();

	}
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
	   else
	    {
	     if (i<=l_z)
	      {
	       x++;
	       gotoxy(x,y);
	       i++;
	      }
	    }

	}
       if (z==71)  //przes¢wanie na pocz¥tek
	{
	 gotoxy(s,y);
	 i=1;
	}
       if (z==79)  //przes¢wanie na koniec
	{
	 gotoxy(s+l_z,y);
	 x=s+l_z;
	 i=l_z;
	}
       if (z==47)  // znak /
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
       if (z==8)   	//8-backspace usuwanie znaku przed kursorem i przes¢w kurosora o jedn¥ pozycje w lewo
	{
	 if ((wherex()==s)&(l_z==1))
	  {
	   w[1]=0;
	   printf("%c",w[1]);
	   gotoxy(s,y);
	   l_z=0;
	   i=0;
	   ;
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
	     if (i==l_z)
	      {
	       w[l_z]=0;
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
	goto miejsce;
       }
       else
	{
	if ((z!=47)&(z!=75)&(z!=77)&(z!=71)&(z!=79)&(z!=8)&(z!=0)&(wherex()<=78)&(z!=13)&(z!=83))
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
  }
  while (z!=13);
  } //-zamkniecie if (edycja=1)
} //-zamkniecie nowy()

main()
{
nowy();

}




