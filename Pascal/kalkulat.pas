program kalkulator;
uses wincrt;
var i:integer;
    koniec,znak:char;


procedure dodawanie;
var liczba1,liczba2:string;
    l1,l2,string_bis2,string_bis1,rowny1,z,suma1,suma2:string;
    licznik,i,j,c1,c2,e1,e2,k,suma,wynik,pom,c3,e3:integer;
    dz,suma3:real;
    znak:char;

    label start1, start2;

begin
clrscr;

gotoxy(25,3);
write('*******************');
writeln;
gotoxy(30,5);
writeln('DODAWANIE');
gotoxy(25,7);
write('*******************');

gotoxy(5,10);



repeat
 start1:
 writeln;
 write('podaj a: ');
 readln(liczba1);
 licznik:=length(liczba1);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start1;
    end;

 for i:=1 to Length(liczba1) do

  begin
    l1:=liczba1[i];
    if (l1='8') or(l1='9')  then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start1;
      end;

   end;


 start2:
 writeln;
 write('podaj b: ');
 readln(liczba2);
 licznik:=length(liczba2);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start2;
    end;
  for j:=1 to Length(liczba2) do
    begin
     l2:=liczba2[j];
     if (l2='8') or(l2='9') then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start2;
      end;
    end;


 if (Length(liczba1)-Length(liczba2))>=0 then
  begin
   for k:=1 to (Length(liczba1)-Length(liczba2)+1) do
    begin
    z[0]:=chr(Length(liczba1)-Length(liczba2)+1);
    z[k]:='0';

   end;

   string_bis2:=Concat(z,liczba2);
   string_bis1:=Concat('0',liczba1);


   end

   else

   begin

    for k:=1 to (Length(liczba2)-Length(liczba1)+1) do

     begin

      z[0]:=chr(Length(liczba2)-Length(liczba1)+1);
      z[k]:='0';

     end;

   string_bis1:=Concat(z,liczba1);
   string_bis2:=Concat('0',liczba2);


  end;



 {dodawanie}

  for i:=Length(string_bis1) downto 1  do

   begin
    pom:=0;
    pom:=wynik;
    rowny1:=suma2;

    Val(string_bis1[i],c1,e1);
    Val(string_bis2[i],c2,e2);

    suma:=c1+c2+pom;

     if suma>7 then

      begin

       suma:=suma mod 8;
       wynik:=1

      end

     else

      begin

       suma:=c1+c2+pom;
       wynik:=0;
      end;

    str(suma,suma1);
    suma2:=Concat(rowny1,suma1);

   end;

if suma2[Length(suma2)]='0' then
     delete(suma2,Length(suma2),1);


 writeln;
 Write('Wynik dodawania w postaci osemkowej wynosi: ');

 for i:=Length(suma2) downto 1 do
     write('',suma2[i]);

 writeln;
 writeln;
 write('Wynik w postaci dziesietnej wynosi: ');
      suma3:=0;
  for i:=1 to Length(suma2) do
   begin

    Val(suma2[i],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma3:=suma3+dz;

    end;
    write(' ',suma3:0:0);
 writeln;
 writeln;

 Write('[ESC]-Przejdz do Menu                      [Enter]-jeszcze raz');
 writeln;
 writeln;
 suma2[0]:=chr(0); liczba1[0]:=chr(0); liczba2[0]:=chr(0); l1[0]:=chr(0); l2[0]:=chr(0); string_bis2[0]:=chr(0);
 string_bis1[0]:=chr(0); rowny1[0]:=chr(0); z[0]:=chr(0); suma1[0]:=chr(0); suma2[0]:=chr(0);
 repeat
  znak:=readkey;
 until (znak=#13) or (znak=#27);
until znak=#27;


end;

procedure odejmowanie;

var liczba1,liczba2:string;
    l1,l2,string_bis2,string_bis1,rowny1,z,suma1,suma2:string;
    licznik,i,j,c1,c2,e1,e2,k,suma,wynik,pom,c3,e3:integer;
    dz,suma3,suma4:real;
    znak:char;

    label start1, start2;

begin
clrscr;

gotoxy(25,3);
write('********************');
writeln;
gotoxy(30,5);
writeln('ODEJMOWANIE');
gotoxy(25,7);
write('********************');

gotoxy(5,10);

repeat
 start1:
 writeln;
 write('podaj a: ');
 readln(liczba1);
 licznik:=length(liczba1);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start1;
    end;

 for i:=1 to Length(liczba1) do

  begin
    l1:=liczba1[i];
    if (l1='8') or(l1='9')  then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start1;
      end;

   end;


 start2:
 writeln;
 write('podaj b: ');
 readln(liczba2);
 licznik:=length(liczba2);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start2;
    end;
  for j:=1 to Length(liczba2) do
    begin
     l2:=liczba2[j];
     if (l2='8') or(l2='9') then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start2;
      end;
    end;


                 suma3:=0;
  for i:=1 to Length(liczba1) do
   begin

    Val(liczba1[Length(liczba1)-i+1],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma3:=suma3+dz;

    end;


          suma4:=0;
  for i:=1 to Length(liczba2) do
   begin

    Val(liczba2[Length(liczba2)-i+1],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma4:=suma4+dz;

    end;


 if suma4>suma3 then
  begin
    writeln;
    writeln('Wynik odejmowania w postaci osemkowej wynosi: 0');
    writeln('Wynik odejmowania w postaci dziesietnej wynosi: 0');
    writeln;
    writeln;
  end 
   

  else

  begin




 if (Length(liczba1)-Length(liczba2))>=0 then
  begin
   for k:=1 to (Length(liczba1)-Length(liczba2)) do
    begin
    z[0]:=chr(Length(liczba1)-Length(liczba2));
    z[k]:='0';

   end;

   string_bis2:=Concat(z,liczba2);
   string_bis1:=liczba1;


   end

   else

   begin

    for k:=1 to (Length(liczba2)-Length(liczba1)) do

     begin

      z[0]:=chr(Length(liczba2)-Length(liczba1));
      z[k]:='0';

     end;

   string_bis1:=Concat(z,liczba1);
   string_bis2:=liczba2;


  end;



 {dodawanie}

  for i:=Length(string_bis1) downto 1  do

   begin
    pom:=0;
    pom:=wynik;
    rowny1:=suma2;

    Val(string_bis1[i],c1,e1);
    Val(string_bis2[i],c2,e2);
  

   suma:=c1-c2+pom;   

     if c1<c2 then
     

      begin
      
        c1:=c1+8+pom;
        suma:=c1-c2;
        wynik:=-1;

      end

     else if c1=c2 then
           begin
            if wynik=-1 then
             begin
              suma:=8+wynik;
              wynik:=wynik;
             end
            else
             begin
              suma:=0;                        
              wynik:=0;
             end;
           end

     else

      begin

       suma:=c1-c2+pom;
       wynik:=0;
     end; 
     
     

    str(suma,suma1);
    suma2:=Concat(rowny1,suma1);

   end;

if suma2[Length(suma2)]='0' then
     delete(suma2,Length(suma2),1);

 writeln;
 Write('Wynik odejmowania w postaci osemkowej wynosi: ');

 for i:=Length(suma2) downto 1 do
     write('',suma2[i]);

 writeln;
 writeln;
 write('Wynik odejmowania w postaci dziesietnej wynosi: ');
      suma3:=0;
  for i:=1 to Length(suma2) do
   begin

    Val(suma2[i],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma3:=suma3+dz;

    end;
    write(' ',suma3:0:0);
 writeln;
 writeln;
 end;
 Write('[ESC]-Przejdz do Menu                      [Enter]-jeszcze raz');
 writeln;
 writeln;
 suma2[0]:=chr(0); liczba1[0]:=chr(0); liczba2[0]:=chr(0); l1[0]:=chr(0); l2[0]:=chr(0); string_bis2[0]:=chr(0);
 string_bis1[0]:=chr(0); rowny1[0]:=chr(0); z[0]:=chr(0); suma1[0]:=chr(0); suma2[0]:=chr(0);

 repeat
  znak:=readkey;
 until (znak=#13) or (znak=#27);
until znak=#27;


end;






procedure menu;
var i,x:integer;
    znak:char;

begin
ClrScr;
gotoxy(25,3);
write('********************************');
writeln;
gotoxy(27,5);
writeln('KALKULATOR LICZB  ¢SEMKOWYCH');
gotoxy(25,7);
write('********************************');

gotoxy(5,20);

writeln('1. Dodawanie [+]');
gotoxy(5,21);
writeln('2. Odejmowanie [-]');
gotoxy(5,22);
writeln('3. Mnozenie [*]');
gotoxy(5,23);
writeln('4. Dzielenie [/]');
gotoxy(2,40);

writeln('Informacje [Enter]');
gotoxy(55,WhereY);
writeln('Wylacz kalkulator [Esc]');






end;

procedure Informacje;
var i:integer;
    koniec:char;
begin

repeat

clrscr;

gotoxy(25,3);
write('*******************************');
writeln;
gotoxy(35,5);
writeln('INFORMACJE');
gotoxy(25,7);
write('*******************************');

gotoxy(5,10);
writeln;
writeln('Kalkulator wykonuje dzia³ania w systemie osemkowym.');
writeln('Podawane liczby nie moga byc dluzsze niz 64 cyfry');
writeln('Wynik jest reprezentowany w systemie osemkowym i dziesietnym');
 gotoxy(55,WhereY+5);
writeln('Przejdz do Menu [Esc]');
repeat
koniec:=readkey;
until koniec=#27;
until koniec=#27;
end;

procedure mnozenie;
var 
    l1,l2,rowny1,z,iloczyn1,iloczyn2,iloczyn3,iloczyn4,wynik1,suma1,suma2,wynik2,liczba1,
    liczba2,liczba10,liczba100,liczba200,liczba20,l:string;
    przep,licznik,i,j,c1,c2,e1,e2,k,iloczyn,wynik,pom,c3,e3,v,suma:integer;
    dz,suma3:double;
    znak:char;

    label start1, start2;

begin
clrscr;

gotoxy(25,3);
write('*******************');
writeln;
gotoxy(30,5);
writeln('MNOZENIE');
gotoxy(25,7);
write('*******************');

gotoxy(5,10);


repeat
 start1:
 writeln;
 write('podaj a: ');
 readln(liczba1);
 licznik:=length(liczba1);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start1;
    end;

 for i:=1 to Length(liczba1) do

  begin
    l1:=liczba1[i];
    if (l1='8') or(l1='9')  then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start1;
      end;

   end;

 start2:
 writeln;
 write('podaj b: ');
 readln(liczba2);
 licznik:=length(liczba2);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start2;
    end;
  for j:=1 to Length(liczba2) do
    begin
     l2:=liczba2[j];
     if (l2='8') or(l2='9') then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start2;
      end;
    end;


 if (Length(liczba1)-Length(liczba2))>=0 then

   begin
     liczba10:=liczba1;
     liczba20:=liczba2;

   end

 else

  begin
   liczba10:=liczba2;
   liczba20:=liczba1;

  end;

 for  k:=1 to ((Length(liczba10)-Length(liczba20))+1) do
     begin
      z[0]:=chr(Length(liczba10)-Length(liczba20)+1);
      z[k]:='0';
     end;

 liczba10:=Concat('0',liczba10);
 liczba20:=Concat(z,liczba20);

   wynik1[0]:=chr(2*(length(liczba20)));
  for i:=1 to length(wynik1) do
   wynik1[i]:='0';

for k:=Length(liczba20) downto 1 do


  begin

  iloczyn2[0]:=chr(0);
      wynik2[0]:=chr(0);

   z[0]:=chr(length(liczba20)-k);
   for v:=(length(liczba20)-k) downto 1 do
     z[v]:='0';

   l[0]:=chr(k);
   for  v:=k  downto 1 do
      l[v]:='0';

   for i:=Length(liczba10) downto 1 do
    begin
     przep:=0;
     przep:=wynik;
     rowny1:=iloczyn2;

     Val(liczba10[i],c1,e1);
     Val(liczba20[k],c2,e2);

     iloczyn:=c1*c2+przep;
     wynik:=iloczyn div 8;

    if iloczyn>7 then
      iloczyn:=iloczyn mod 8

     else
      begin
       iloczyn:=c1*c2+przep;
       wynik:=0;
      end;

        str(iloczyn,iloczyn1);
        iloczyn2:=Concat(rowny1,iloczyn1)


      end;


        iloczyn3:=concat(z,iloczyn2);
        iloczyn4:=concat(iloczyn3,l);


    for i:=1 to length(iloczyn4) do

     begin

      pom:=0;
      pom:=wynik;
      rowny1:=wynik2;

      val(wynik1[i],c1,e1);
      val(iloczyn4[i],c2,e2);

      suma:=c1+c2+pom;

      if suma>7 then

       begin
        suma:=suma mod 8;
        wynik:=1;
       end

      else

       begin
        suma:=c1+c2+pom;
        wynik:=0;
       end;
        str(suma,suma1);
        wynik2:=Concat(rowny1,suma1);

     end;

     wynik1:=wynik2;
 
end;
    
  for i:=1 to Length(wynik2) do
   begin
    if wynik2[length(wynik2)]='0' then
     delete(wynik2,length(wynik2),1);
   end;

 writeln;
 Write('Wynik mnozenia w postaci osemkowej wynosi: ');


 for i:=Length(wynik2) downto 1 do
     write('',wynik2[i]);

 writeln;
 writeln;
 write('Wynik w postaci dziesietnej wynosi: ');
      suma3:=0;
  for i:=1 to Length(wynik2) do
   begin
 
    Val(wynik2[i],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));
      
      suma3:=suma3+dz;
      
    end;
    write(' ',suma3:0:0);
 writeln;
 writeln;

 Write('[ESC]-Przejdz do Menu                      [Enter]-jeszcze raz');
 writeln;
 writeln;

 wynik2[0]:=chr(0); l1[0]:=chr(0); l2[0]:=chr(0); rowny1[0]:=chr(0); z[0]:=chr(0); iloczyn1[0]:=chr(0); iloczyn2[0]:=chr(0);
 iloczyn3[0]:=chr(0); iloczyn4[0]:=chr(0); wynik1[0]:=chr(0); suma1[0]:=chr(0); suma2[0]:=chr(0); wynik2[0]:=chr(0);
 liczba1[0]:=chr(0); liczba2[0]:=chr(0); liczba10[0]:=chr(0); liczba100[0]:=chr(0); liczba200[0]:=chr(0); liczba20[0]:=chr(0);
 l[0]:=chr(0);

 repeat
  znak:=readkey;
 until (znak=#13) or (znak=#27);
until znak=#27;                 

end;

procedure dzielenie;
var liczba1,liczba2,m2,m1:string;
    l1,l2,string_bis2,string_bis1,rowny1,z,suma1,suma2,suma200,suma20,suma30:string;
    licz,d,m,licznik,i,j,c1,c2,e1,e2,k,c4,suma,wynik,pom,c3,e3:integer;
    dl,dz,dw,suma3,suma4:real;
    znak:char;

    label start1, start2;

begin
clrscr;

gotoxy(25,3);
write('*******************');
writeln;
gotoxy(30,5);
writeln('DZIELENIE');
gotoxy(25,7);
write('*******************');

gotoxy(5,10);



repeat
 start1:
 writeln;
 write('podaj a: ');
 readln(liczba1);
 licznik:=length(liczba1);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start1;
    end;

 for i:=1 to Length(liczba1) do

  begin
    l1:=liczba1[i];
    if (l1='8') or(l1='9')  then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start1;
      end;

   end;


 start2:
 writeln;
 write('podaj b: ');
 readln(liczba2);
 licznik:=length(liczba2);
 if licznik>64 then
    begin
    write('Ta liczba ma wiecej niz 64 znaki. Podaj krotsza liczbe');
    goto start2;
    end;
  for j:=1 to Length(liczba2) do
    begin
     l2:=liczba2[j];
     if (l2='8') or(l2='9') then
      begin
       write('To nie jest liczba ¢semkowa. Podaj ja jeszcze raz ');
       goto start2;
      end;
    end;

                suma3:=0;
  for i:=1 to Length(liczba1) do
   begin

    Val(liczba1[Length(liczba1)-i+1],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma3:=suma3+dz;

    end;


          suma4:=0;
  for i:=1 to Length(liczba2) do
   begin

    Val(liczba2[Length(liczba2)-i+1],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma4:=suma4+dz;

    end;


 if suma4>suma3 then
  begin
    writeln;
    writeln('Wynik dzielenia w postaci dziesietnej wynosi: 0');
    writeln('Wynik dzielenia w postaci osemkowej wynosi: 0');
    writeln;
    writeln;
  end 
   

  else

  begin

 if (Length(liczba1)-Length(liczba2))>=0 then
  begin
   for k:=1 to (Length(liczba1)-Length(liczba2)) do
    begin
    z[0]:=chr(Length(liczba1)-Length(liczba2));
    z[k]:='0';

   end;

   string_bis2:=Concat(z,liczba2);
   string_bis1:=liczba1;


   end

   else

   begin

    for k:=1 to (Length(liczba2)-Length(liczba1)) do

     begin

      z[0]:=chr(Length(liczba2)-Length(liczba1));
      z[k]:='0';

     end;

   string_bis1:=Concat(z,liczba1);
   string_bis2:=liczba2;


  end;


   suma3:=0;
 {dodawanie}

for i:=1 to Length(string_bis2) do
   begin

    Val(string_bis2[Length(string_bis2)-i+1],c3,e3);
      dz:=c3*Exp((i-1)*Ln(8));

      suma3:=suma3+dz;

    end;


   licz:=0;
   suma2:=string_bis1;

repeat


    dl:=0;


  for i:=Length(suma2) downto 1  do

   begin
    pom:=0;
    pom:=wynik;
    rowny1:=suma2;

    Val(suma2[i],c1,e1);
    Val(string_bis2[i],c2,e2);

    suma:=c1-c2+pom;

      if c1<c2 then
     

      begin
      
        c1:=c1+8+pom;
        suma:=c1-c2;
        wynik:=-1;

      end

     else if c1=c2 then
           begin
            if wynik=-1 then
             begin
              suma:=8+wynik;
              wynik:=wynik;
             end
            else
             begin
              suma:=0;                        
              wynik:=0;
             end;
           end

     else

      begin

       suma:=c1-c2+pom;
       wynik:=0;
     end;


    str(suma,suma1);
    suma2:=Concat(rowny1,suma1);
 
   end;

      
   if suma2[0]>string_bis2[0] then
   delete(suma2,1,Length(suma2)-Length(string_bis2));


     dl:=0;

   for i:=1 to Length(suma2) do


    begin

     val(suma2[i],c4,e1);
     dw:=c4*Exp((i-1)*Ln(8));
     dl:=dl+dw;
    end;

      suma30:=suma2;


     for i:=length(suma30) downto 1 do
      suma2[length(suma30)-i+1]:=suma30[i];


    licz:=licz+1;



until dl<suma3;
writeln;

write('Wynik dzielenia w postaci dziesietnej wynosi ',licz);


repeat



     rowny1:=m2;

     d:=licz div 8;
     m:=licz mod 8;

     str(m,m1);
     m2:=concat(rowny1,m1);

     licz:=d;


until d=0;

writeln;



 writeln;
 Write('Wynik dzielenia w postaci osemkowej wynosi: ');

 for i:=Length(m2) downto 1 do
     write('',m2[i]);
       m2[0]:=chr(0);
 writeln;
 writeln;


 writeln;
 writeln;

 end;
 Write('[ESC]-Przejdz do Menu                      [Enter]-jeszcze raz');
 writeln;
 writeln;
 suma2[0]:=chr(0);
 liczba1[0]:=chr(0); liczba2[0]:=chr(0); m2[0]:=chr(0); liczba1:=chr(0); liczba2:=chr(0); m2:=chr(0);
 m1[0]:=chr(0); l1[0]:=chr(0); l2[0]:=chr(0); string_bis2[0]:=chr(0); string_bis1[0]:=chr(0); rowny1[0]:=chr(0);
 z[0]:=chr(0); suma1[0]:=chr(0); suma2[0]:=chr(0); suma200[0]:=chr(0); suma20[0]:=chr(0); suma30:=chr(0); m1:=chr(0);
    

 repeat
  znak:=readkey;
 until (znak=#13) or (znak=#27);

until znak=#27;


end;



begin


while znak<>#27 do

begin

menu;
znak:=readkey;

case znak of
'+':dodawanie;
'-':odejmowanie;
'*':mnozenie;
'/':dzielenie;
#13: Informacje;
end;

end;

end.
