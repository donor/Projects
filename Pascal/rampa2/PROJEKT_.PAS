program rampa;
uses crt;
type

     TPaleta=record
               identyfikator:1..3;
               masa:integer;
              end;

     TWsk=^TElement;
     TElement=record
               d:TPaleta;
               w:TWsk;
             end;

     TPlik=file of TPaleta;

var P,P_1,K_1,P_2,K_2,P_3,K_3:TWsk;
    i,n:integer;







procedure wprowadz_rodzaj (var x:TPaleta);
 begin
  with x do
   begin

    repeat
    identyfikator:=Random(4);
    until identyfikator<>0;

    repeat
    masa:=Random(500);
    until masa<>0;


    end;
 end;

procedure Na_Stos (var P:TWsk; x:TPaleta);
var Q:TWsk;
begin
 New(Q);
 Q^.d:=x; Q^.w:=P;
 P:=Q;
end;

procedure Wprowadz_dane (var P:TWsk);
var i,n:integer;
    x:TPaleta;
begin
 P:=NIL;
 repeat
 n:=Random(150);
 until n<>0;
 for i:=1 to n do
  begin

   wprowadz_rodzaj (x);
   Na_Stos (P,x);

  end;
 end;




procedure Usun_ze_stosu (var P:TWsk; var x:TPaleta);
var Q:Twsk;
begin
 Q:=P;
 x:=P^.d;
 P:=P^.w;
 Dispose(Q);
end;

procedure Pokaz_rodzaj(x:TPaleta);
begin
 with x do
  begin
   gotoxy(15,wherey+1); write(identyfikator); gotoxy(20,wherey); write(masa)

  end;

 end;

procedure usun_stos(var P:TWsk);
var x:TPaleta;
    i:integer;
begin
 i:=0;
 write('identyfikator '); write('masa ');
 writeln;
 writeln;
 while P<>nil do
  begin
   i:=i+1;
   write(i:3);
   Usun_ze_stosu(P,x);
   Pokaz_rodzaj(x);
  end;
 end;




procedure wyswietl_Stos(P:TWsk);
var x:TPaleta;
    i:integer;
begin
i:=0;

gotoxy(5,wherey); write('identyfikator');
gotoxy(20,wherey);   write('masa');

 while P<>nil do
  begin
   i:=i+1;
   x:=P^.d; P:=P^.w;
   Pokaz_rodzaj(x);
  end;
end;

procedure Do_kolejki(var P,K:TWsk; x:TPaleta);
var Q:TWsk;
begin
 New(Q);
 Q^.d:=x; Q^.w:=nil;
 if P=Nil then P:=Q
          else K^.w:=Q;
           K:=Q;
end;

procedure do_pliku1 (P_1:TWsk);
var plik:TPlik;

begin

assign(plik,'palety1.dat');
rewrite(plik);
while P_1<>Nil do
 begin
   write(plik,P_1^.d);
   P_1:=P_1^.w;
  end;
 close(plik);
end;

procedure do_pliku3 (P_3:TWsk);
var plik:TPlik;

begin

assign(plik,'palety3.dat');
rewrite(plik);
while P_3<>Nil do
 begin
   write(plik,P_3^.d);
   P_3:=P_3^.w;
  end;
 close(plik);
end;

procedure do_pliku2 (P_2:TWsk);
var plik2:TPlik;

begin

assign(plik2,'palety2.dat');
rewrite(plik2);
while P_2<>Nil do
 begin
   write(plik2,P_2^.d);
   P_2:=P_2^.w;
  end;
 close(plik2);
end;

procedure usun_z_kolejki (var P:TWsk; var x:TPaleta);
var Q:TWsk;
begin
 x:=P^.d;
 Q:=P;
 P:=P^.w;
 Dispose(Q);
end;

procedure wyswietl_palete(x:TPaleta);
begin
 with x do
  write(masa);
  writeln;
end;

procedure usun_kolejke(var P:TWsk;l,t:byte);
var x:TPaleta;
    z,z1:byte;

begin

writeln;
gotoxy(l,7);
gotoxy(l,wherey);


while P<>nil do
 begin
   gotoxy(l,wherey);
   usun_z_kolejki(P,x);
   wyswietl_palete(x);
 end;
gotoxy(3,43);
writeln('Kolejka typu ',t,' jest pe�na.');
gotoxy(3,44);
write('Naci�nij dowolny klawisz aby j� opr��ni� i kontynuowa� rozdzia�....');

readkey;
gotoxy(l,7);
textcolor(black);
for z:=0 to 25 do
 begin
  for z1:=l to l+8 do
   begin
    gotoxy(z1,wherey);
    write(#219);
   end;
  writeln;
 end;
textcolor(white);

end;

procedure oproznij_koniec(var P:TWsk);
var Q:TWsk;
begin
while P<>nil do
 begin
  Q:=P;
  P:=P^.w;
  dispose(Q);
 end;
end;

{procedure wyswietl_kolejke(P:TWsk);
var x:TPaleta;
begin
 while P<>NIL do
 begin
  wyswietl_palete(P^.d);
  P:=P^.w;
 end;
end;}


procedure rozdziel(P:TWsk; var P_1,K_1,P_2,K_2,P_3,K_3:TWsk);
var x:TPaleta;
    np:string;
    y1,y2,y3:integer;
    t,l1,l2,l3,t1,t2,t3:byte;
begin
y1:=0; y2:=0; y3:=0; l1:=3; l2:=30; l3:=60; t1:=1; t2:=2; t3:=3;

 P_1:=nil; P_2:=nil; P_3:=nil;
 While P<>nil do
  begin
   Usun_ze_stosu(P,x);
   if x.identyfikator=1 then
                          begin

                           if (y1+x.masa)<2500 then
                            begin
                             Do_kolejki(P_1,K_1,x);
                             y1:=y1+x.masa;
                            end
                           else
                            begin
                            Clrscr;
                            gotoxy(3,5);
write('Kolejka typu 1');
gotoxy(3,7);
gotoxy(30,5);
write('Kolejka typu 2');
gotoxy(30,7);
gotoxy(60,5);
write('Kolejka typu 3');
                            gotoxy(30,7);

                             usun_kolejke(P_1,l1,t1);
                             y1:=0;
                             Do_kolejki(P_1,K_1,x);
                             y1:=y1+x.masa;
                           end;


                          end

                         else
                          if x.identyfikator=2 then
                                                begin
                                                  if (y2+x.masa)<2500 then
                                                   begin
                                                    Do_kolejki(P_2,K_2,x);
                                                    y2:=y2+x.masa;
                                                   end
                                                   else
                                                    begin
                                                    CLRSCR;
                                                    gotoxy(3,5);
write('Kolejka typu 1');
gotoxy(3,7);
gotoxy(30,5);
write('Kolejka typu 2');
gotoxy(30,7);
gotoxy(60,5);
write('Kolejka typu 3');
                                                     gotoxy(30,whereY);

                                                     usun_kolejke(P_2,l2,t2);
                                                     y2:=0;
                                                     Do_kolejki(P_2,K_2,x);
                                                     y2:=y2+x.masa;
                                                    end;



                                                end

                                               else
                                                begin
                                                  if (y3+x.masa)<2500 then
                                                   begin
                                                    Do_kolejki(P_3,K_3,x);
                                                    y3:=y3+x.masa;
                                                   end
                                                   else
                                                    begin
                                                     clrscr;
                                                     gotoxy(3,5);
write('Kolejka typu 1');
gotoxy(3,7);
gotoxy(30,5);
write('Kolejka typu 2');
gotoxy(30,7);
gotoxy(60,5);
write('Kolejka typu 3');
                                                     usun_kolejke(P_3,l3,t3);
                                                     y3:=0;
                                                     Do_kolejki(P_3,K_3,x);
                                                     y3:=x.masa;
                                                    end;

                                                    end;

 end;
 clrscr;
 gotoxy(3,2);
 write('Rozdzia� zako�czony');
 gotoxy(3,2);
 write('Naci�nij dowolny klawisz aby zobaczy� wydruk ko�cowej zawarto�ci');
 gotoxy(3,3);
 write('kolejek');
 readkey;

 end;

procedure z_pliku1(P_1,K_1:TWsk);
var
    plik:TPlik;
    x:TPaleta;

begin
assign(plik,'palety1.dat');
reset(plik);
gotoxy(3,5);
write('Kolejka typu 1');
gotoxy(3,7);
while not eof(plik) do
 begin
  read(plik,x);
  gotoxy(3,wherey);
  wyswietl_palete(x);
  end;
 close(plik);
end;


procedure z_pliku2(P_2,K_2:TWsk);
var
    plik2:TPlik;
    x:TPaleta;

begin
assign(plik2,'palety2.dat');
reset(plik2);
gotoxy(30,5);
write('Kolejka typu 2');
gotoxy(30,WHEREY);
while not eof(plik2) do
 begin
  read(plik2,x);
  gotoxy(30,wherey);
  wyswietl_palete(x);
  end;
 close(plik2);
end;

procedure z_pliku3(P_3,K_3:TWsk);
var
    plik:TPlik;
    x:TPaleta;

begin
assign(plik,'palety3.dat');
reset(plik);
gotoxy(60,5);
write('Kolejka typu 3');
gotoxy(60,7);
while not eof(plik) do
 begin
  read(plik,x);
  gotoxy(60,wherey);
  wyswietl_palete(x);
  end;
 close(plik);
end;


begin
randomize;
wprowadz_dane (P);
clrscr;
{writeln('Wprowadzono  nast�puj�ce palety: ');
wyswietl_stos(P);
readkey;
clrscr;
writeln;}
gotoxy(25,2);
writeln('Po�redni wydruk kolejek');
n:=80;
for i:=1 to n do
write(#205);
gotoxy(3,5);
write('Kolejka typu 1');
gotoxy(3,7);
gotoxy(30,5);
write('Kolejka typu 2');
gotoxy(30,7);
gotoxy(60,5);
write('Kolejka typu 3');
gotoxy(60,7);
rozdziel(P,P_1,K_1,P_2,K_2,P_3,K_3);

do_pliku1 (P_1);
do_pliku2 (P_2);
do_pliku3 (P_3);

oproznij_koniec(P_1);
oproznij_koniec(P_2);
oproznij_koniec(P_3);

clrscr;
gotoxy(25,2);
writeln('Ko�cowy wydruk kolejek');
n:=80;
for i:=1 to n do
write(#205);
z_pliku1(P_1,K_1);
z_pliku2(P_2,K_2);
z_pliku3(P_3,K_3);

readkey;

end.