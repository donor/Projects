program orger;
uses crt, dos;
type Twiadomosc=record
                 tresc:string;
                 data:DateTime;
                 alarm:boolean;
                end;
     TWsk=^TElement;
     TElement=record
              wiadomosc:Twiadomosc;
              next:TWsk;
             end;
     Tplik=file of Twiadomosc;

var  P:TWsk;
     rok,miesiac,dzien,dzien_tygodnia,godz,min,sek,sek100:word;
     znak:char;
     x:Twiadomosc;


procedure play;
var nuta,czas:integer;
const q=300;
      h=600;
      t=900;
      w=1200;
      C=262;
      D=294;
      E=330;
      F=350;
      G=392;
      A=440;
begin
 Sound(330);  Delay(600);  NoSound;
 Sound(392);  Delay(600);  NoSound;
 Sound(392);  Delay(300);  NoSound;
 Sound(262);  Delay(900);  NoSound;
 Sound(294);  Delay(300);  NoSound;
 Sound(330);  Delay(300);  NoSound;
 Sound(350);  Delay(300);  NoSound;
 Sound(392);  Delay(300);  NoSound;
 Sound(440);  Delay(300);  NoSound;
 Sound(294);  Delay(1200);  NoSound;
end;

procedure wprowadz_wiadomosc(var x:Twiadomosc);
var z:char;
    m:byte;
    label czas;

begin
with x do
 begin
  write('Wiadomo˜†: '); readln(tresc);

  with data do
   begin
    czas:
    write('Data i godzina alarmu (rrrr.mm.dd gg:mm:ss) ');
    write('');  m:=wherex;  readln(year);
      if (year<2009) or (year>9999)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+4,wherey-1); write('.');
    write('');  m:=wherex;  readln(month);
      if (month<1) or (month>12)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write('.');
    write('');  m:=wherex;  readln(day);
      if (day<1) or (day>31)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+3,wherey-1);
    write('');  m:=wherex;  readln(hour);
      if (hour<0) or (hour>23)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write(':');
    write('');  m:=wherex;  readln(min);
      if (min<0) or (min>59)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write(':');
    write('');  m:=wherex;  readln(sec);
    if (sec<0) or (sec>59)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1);

  end;
  writeln;
  write('Sygnaˆ dzwi©kowu(T/N): ');
  repeat

  z:=UpCase(readkey);
  until z in ['T','N'];
  writeln(z);
  if z='T' then
    alarm:=true
  else
    alarm:=false;
 end;
writeln;
end;


procedure Pokaz_wiadomosc(x:Twiadomosc);
begin
with x do
 begin
  if alarm=true then
   begin

   writeln('Alarm dzwi©kowy dowi¥zany');
   end
 else
  writeln('Brak alarmu dzwi©kowego');


  with data do
   begin
    write(year,'.');
    if month<10 then
     write('0',month,'.')
    else
     write(month,'.');
    if day<10 then
     write('0',day,'  ')
    else
     write(day,'  ');
    if hour<10 then
     write('0',hour,':')
    else
     write(hour,':');
    if min<10 then
     write('0',min,':')
    else
     write(min,':');
    if sec<10 then
     write('0',sec)
    else
     write(sec);

  end;
  writeln;
  writeln('Wiadomosc:',tresc);
 if alarm=true then   play;
 end;
end;

procedure Pokaz_wiadomosc1(x:Twiadomosc);
begin
with x do
 begin
  if alarm=true then
   writeln('Alarm dzwi©kowy dowi¥zany')
  else
   writeln('Brak alarmu dzwi©kowego');

  with data do
   begin
    write(year,'.');
    if month<10 then
     write('0',month,'.')
    else
     write(month,'.');
    if day<10 then
     write('0',day,'  ')
    else
     write(day,'  ');
    if hour<10 then
     write('0',hour,':')
    else
     write(hour,':');
    if min<10 then
     write('0',min,':')
    else
     write(min,':');
    if sec<10 then
     write('0',sec)
    else
     write(sec);
  end;
  writeln;
  writeln('Wiadomosc:',tresc);

 end;
end;

procedure do_pliku(P:TWsk;np:string);
var plik:TPlik;
begin
assign(plik,np);
rewrite(plik);

while P<>nil do
 begin
  write(plik,P^.wiadomosc);
  P:=P^.next;
 end;
close(plik);
end;

procedure zamien(var p1,p2:TWsk);
var x:Twiadomosc;
begin
 x:=p1^.wiadomosc;
 p1^.wiadomosc:=p2^.wiadomosc;
 p2^.wiadomosc:=x;
end;

procedure uklad(var P:TWsk);
var zamieniono:boolean;
    p1,p2:TWsk;
begin
 zamieniono:=true;
 while zamieniono do
  begin
   zamieniono:=false;
   p1:=P;
   p2:=p1^.next;
   while p2<>nil do
    begin
     if
      (((p1^.wiadomosc.data.year>p2^.wiadomosc.data.year))or
      ((p1^.wiadomosc.data.year=p2^.wiadomosc.data.year)and(p1^.wiadomosc.data.month>p2^.wiadomosc.data.month))or
      ((p1^.wiadomosc.data.year=p2^.wiadomosc.data.year)and(p1^.wiadomosc.data.month=p2^.wiadomosc.data.month)and
      (p1^.wiadomosc.data.day>p2^.wiadomosc.data.day))or
      ((p1^.wiadomosc.data.year=p2^.wiadomosc.data.year)and(p1^.wiadomosc.data.month=p2^.wiadomosc.data.month)and
      (p1^.wiadomosc.data.day=p2^.wiadomosc.data.day)and(p1^.wiadomosc.data.hour>p2^.wiadomosc.data.hour))or
      ((p1^.wiadomosc.data.year=p2^.wiadomosc.data.year)and(p1^.wiadomosc.data.month=p2^.wiadomosc.data.month)and
      (p1^.wiadomosc.data.day=p2^.wiadomosc.data.day)and(p1^.wiadomosc.data.hour=p2^.wiadomosc.data.hour)and
      (p1^.wiadomosc.data.min>p2^.wiadomosc.data.min))or
      ((p1^.wiadomosc.data.year=p2^.wiadomosc.data.year)and(p1^.wiadomosc.data.month=p2^.wiadomosc.data.month)and
      (p1^.wiadomosc.data.day=p2^.wiadomosc.data.day)and(p1^.wiadomosc.data.hour=p2^.wiadomosc.data.hour)and
      (p1^.wiadomosc.data.min=p2^.wiadomosc.data.min)and(p1^.wiadomosc.data.sec>p2^.wiadomosc.data.sec))or
      ((p1^.wiadomosc.data.year=p2^.wiadomosc.data.year)and(p1^.wiadomosc.data.month=p2^.wiadomosc.data.month)and
      (p1^.wiadomosc.data.day=p2^.wiadomosc.data.day)and(p1^.wiadomosc.data.hour=p2^.wiadomosc.data.hour)and
      (p1^.wiadomosc.data.min=p2^.wiadomosc.data.min)and(p1^.wiadomosc.data.sec=p2^.wiadomosc.data.sec)))
     then
      begin
       zamien(p1,p2);
       zamieniono:=true;

     end;
   p1:=p2;
   p2:=p2^.next;
  end;
 end;
end;




procedure do_listy(var P:TWsk; var x:Twiadomosc);
var Q:TWsk;

begin

 new(Q);
 Q^.wiadomosc:=x;

 Q^.next:=P;
 P:=Q;
uklad(P);
end;

{procedure do_listy(var P:TWsk; var x:Twiadomosc);
var Q:TWsk;

begin
 Q:=P;
 new(P);
 with P^ do
  begin
   wiadomosc:=x;
   next:=nil;
  end;
 if Q<>nil then
  Q^.next:=P;

end;}

procedure z_pliku(var P:TWsk; np:string);
var
   plik:Tplik;
   x:Twiadomosc;
   lok:TWsk;
begin
assign(plik,np);
reset(plik);

while not eof(plik) do
 begin
  read(plik,x);
  do_listy(P,x);
 end;
close(plik);
end;




procedure zapisz_wiadomosc(var P:TWsk);
var z:char;
    x:Twiadomosc;
   
begin
clrscr;
write('Czy chcesz wprowadzi† nowe alarmy? (T/N): ');
repeat
z:=UpCase(readkey);
until z in ['N','T'];
writeln(z);
while z='T' do
 begin
  writeln('Wprowadz alarm: ');
  wprowadz_wiadomosc(x);
  do_listy(P,x);
  write('Czy wprowadzasz nastepny alarm? (T/N): ');
  repeat
 z:=UpCase(readkey);
 until z in ['N','T'];
 writeln(z);
 end;
end;


procedure wyswietl(P:TWsk);
var
    rok,miesiac,dzien,dzien_tygodnia,godz,minuta,sek,sek100:word;
    i:integer;
    z:char;
begin
clrscr;
i:=0;
gotoxy(1,1);
gotoxy(1,25);
write('                                  ');
gotoxy(1,25);
write('Nacisnij dowolny klawisz aby kontynuowa†....');
gotoxy(1,3);
while P<>Nil do
 begin
  if i=5 then
   begin
   repeat until keypressed;
     clrscr;
     i:=0;

     gotoxy(1,25);
     write('                                  ');
     gotoxy(1,25);
     write('Nacisnij dowolny klawisz aby kontynuowa†...');
     readkey;
     gotoxy(1,3);
   end;

  pokaz_wiadomosc1(P^.wiadomosc);
  writeln;
  i:=i+1;
  P:=P^.next;
 end;
readkey;
end;



procedure wyzwalacz(var P:TWsk; x:Twiadomosc);

var
    rok,miesiac,dzien,dzien_tygodnia,godz,minuta,sek,sek100:word;
    znaleziono:boolean;
    p1,p2,d:TWsk;
    z:char;

begin

getdate(rok,miesiac,dzien,dzien_tygodnia);
gettime(godz,minuta,sek,sek100);


while
     (((rok>P^.wiadomosc.data.year))or
     ((rok=P^.wiadomosc.data.year)and(miesiac>P^.wiadomosc.data.month))or
     ((rok=P^.wiadomosc.data.year)and(miesiac=P^.wiadomosc.data.month)and
     (dzien>P^.wiadomosc.data.day))or
     ((rok=P^.wiadomosc.data.year)and(miesiac=P^.wiadomosc.data.month)and
     (dzien=P^.wiadomosc.data.day)and(godz>P^.wiadomosc.data.hour))or
     ((rok=P^.wiadomosc.data.year)and(miesiac=P^.wiadomosc.data.month)and
     (dzien=P^.wiadomosc.data.day)and(godz=P^.wiadomosc.data.hour)and
     (minuta>P^.wiadomosc.data.min))or
     ((rok=P^.wiadomosc.data.year)and(miesiac=P^.wiadomosc.data.month)and
     (dzien=P^.wiadomosc.data.day)and(godz=P^.wiadomosc.data.hour)and
     (minuta=P^.wiadomosc.data.min)and(sek>P^.wiadomosc.data.sec))or
     ((rok=P^.wiadomosc.data.year)and(miesiac=P^.wiadomosc.data.month)and
     (dzien=P^.wiadomosc.data.day)and(godz=P^.wiadomosc.data.hour)and
     (minuta=P^.wiadomosc.data.min)and(sek=P^.wiadomosc.data.sec)))



  do
   begin

    writeln;
    pokaz_wiadomosc(P^.wiadomosc);
    d:=P;
    P:=P^.next;
    dispose(d);
   end;

 p1:=P;
 p2:=p1^.next;

 while p2<>nil do
     begin
      if
     (((rok>p2^.wiadomosc.data.year))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac>p2^.wiadomosc.data.month))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien>p2^.wiadomosc.data.day))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz>p2^.wiadomosc.data.hour))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta>p2^.wiadomosc.data.min))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta=p2^.wiadomosc.data.min)and(sek>p2^.wiadomosc.data.sec))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta=p2^.wiadomosc.data.min)and(sek=p2^.wiadomosc.data.sec)))


       then
        begin

         writeln;
         pokaz_wiadomosc(p2^.wiadomosc);
         d:=p2;
         p1^.next:=p2^.next;
         p2:=p1;
         dispose(d);

      end;
      p1:=p2;
      p2:=p1^.next;
     end;


{delay(600);}



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
writeln('ELEKTRONICZNY ORGANIZER');
gotoxy(25,7);
write('********************************');

gotoxy(5,9);

writeln('1. Dodaj notatk©              [1]');
gotoxy(5,10);
writeln('2. Modyfikuj notatk©          [2]');
gotoxy(5,11);
writeln('3. Usuä notatk©               [3]');
gotoxy(5,12);
writeln('4. Poka¾ alarmy               [4]');
gotoxy(5,13);
writeln('5. Przypomnienia(tryb gˆ¢wny) [Tab]');
gotoxy(2,20);

writeln('Informacje [Enter]');
gotoxy(55,WhereY);
writeln('Wylacz organizer [Esc]');

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
writeln('Elektroniczny organizer umo¾liwiaj¥cy powi¥zanie notatki z okre˜lon¥.');
writeln('dat¥ i godzin¥ oraz dodanie sygnaˆu d«wi©kowego uaktywnianego ');
writeln('o okre˜lonej godzinie. Notatki mo¾na dodawa†, modyfikowa† i usuwa†');
 gotoxy(55,WhereY+5);
writeln('Przejdz do Menu [Esc]');
repeat
koniec:=readkey;
until koniec=#27;
until koniec=#27;
end;


procedure wprowadz_wiadomosc1(var x:Twiadomosc);
var z:char;
    m:byte;
    label czas;

begin
with x do
 begin
  write('Czy chcesz zmodyfikowa† tre˜† wiadomo˜ci (T/N)');
  repeat
  z:=UpCase(readkey);
  until z in ['T','N'];
  write(z); writeln;
  if z='T' then
   begin
    write('Nowa wiadomo˜†: '); readln(tresc);
   end;

  with data do
   begin
    write('Czy chcesz zmodyfikowa† termin alarmu (T/N)');
    repeat
    z:=UpCase(readkey);
    until z in ['T','N'];
    write(z); writeln;
    if z='T'then
     begin
      czas:
      write('Nowa data i godzina alarmu (rrrr.mm.dd gg:mm:ss) ');
      write('');  m:=wherex;  readln(year);
      if (year<2009) or (year>9999)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
      gotoxy(m+4,wherey-1); write('.');
      write('');  m:=wherex;  readln(month);
      if (month<1) or (month>12)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
      gotoxy(m+2,wherey-1); write('.');
      write('');  m:=wherex;  readln(day);
      if (day<1) or (day>31)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
      gotoxy(m+3,wherey-1);
      write('');  m:=wherex;  readln(hour);
      if (hour<1) or (hour>24)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
      gotoxy(m+2,wherey-1); write(':');
      write('');  m:=wherex;  readln(min);
      if (min<1) or (min>60)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
      gotoxy(m+2,wherey-1); write(':');
      write('');  m:=wherex;  readln(sec);
     if (sec<1) or (sec>60)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
      gotoxy(m+2,wherey-1);
     end;
  end;
  writeln;
  write('Sygnaˆ dzwi©kowu(T/N): ');
  repeat

  z:=UpCase(readkey);
  until z in ['T','N'];
  writeln(z);
  if z='T' then
    alarm:=true
  else
    alarm:=false;
 end;
writeln;
end;


procedure modyfikuj(P:TWsk);
var  data1:DateTime;
     z:char;
     m:byte;
     znaleziono:boolean;
     label czas,czas1;

begin
clrscr;
czas1:
Writeln('Podaj termin notatki kt¢r¥ chcesz zmodyfikowa†:');
with data1 do
   begin
    czas:
    write('Data i godzina alarmu (rrrr.mm.dd gg:mm:ss) ');


    write('');  m:=wherex;  readln(year);
      if (year<2009) or (year>9999)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+4,wherey-1); write('.');
    write('');  m:=wherex;  readln(month);
      if (month<1) or (month>12)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write('.');
    write('');  m:=wherex;  readln(day);
      if (day<1) or (day>31)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+3,wherey-1);
    write('');  m:=wherex;  readln(hour);
      if (hour<1) or (hour>24)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write(':');
    write('');  m:=wherex;  readln(min);
      if (min<1) or (min>60)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write(':');
    write('');  m:=wherex;  readln(sec);
    if (sec<1) or (sec>60)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1);

  end;
znaleziono:=false;
while (P<>nil)and(znaleziono=false) do
 begin
  if (data1.year=P^.wiadomosc.data.year)and(data1.month=P^.wiadomosc.data.month)and
     (data1.day=P^.wiadomosc.data.day)and(data1.hour=P^.wiadomosc.data.hour)and
     (data1.min=P^.wiadomosc.data.min)and(data1.sec=P^.wiadomosc.data.sec) then
   znaleziono:=true
  else
   P:=P^.next;
 end;
 if znaleziono=true then
  begin
   writeln;
   pokaz_wiadomosc1(P^.wiadomosc);
   readkey;
   writeln('Wprowadz nowe atrybuty tego przypomnienia:');
   wprowadz_wiadomosc1(P^.wiadomosc);
   writeln;
   write('Czy chcesz zmodyfikowa† inn¥ wiadomo˜† (T/N)');
   repeat
   z:=UpCase(readkey);
   until z in ['T','N'];
   write(z); writeln;
   if z='T' then goto czas1;

  end;
 if znaleziono=false then
  begin
   writeln;
   writeln('Nie ma notatki o tym terminie ');
   write('Czy chcesz zmodyfikowa† inn¥ wiadomo˜† (T/N)');
   repeat
   z:=UpCase(readkey);
   until z in ['T','N'];
   write(z); writeln;
   if z='T' then goto czas1;

  end;
end;


procedure usun(var P:TWsk;x:Twiadomosc);
var  data1:DateTime;
     z:char;
     m:byte;
     a:integer;
     znaleziono:boolean;
     p1,p2,d:TWsk;
     label czas, czas1;
begin

clrscr;
czas1:
Writeln('Podaj termin notatki kt¢r¥ chcesz usun¥†:');
with data1 do
 begin
  czas:
    write('Data i godzina alarmu (rrrr.mm.dd gg:mm:ss) ');


    write('');  m:=wherex;  readln(year);
      if (year<2009) or (year>9999)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+4,wherey-1); write('.');
    write('');  m:=wherex;  readln(month);
      if (month<1) or (month>12)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write('.');
    write('');  m:=wherex;  readln(day);
      if (day<1) or (day>31)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+3,wherey-1);
    write('');  m:=wherex;  readln(hour);
      if (hour<1) or (hour>24)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write(':');
    write('');  m:=wherex;  readln(min);
      if (min<1) or (min>60)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1); write(':');
    write('');  m:=wherex;  readln(sec);
    if (sec<1) or (sec>60)  then
       begin
        writeln; writeln('Bˆ¥d'); goto czas;
       end;
    gotoxy(m+2,wherey-1);
 end;
znaleziono:=false;
while  (data1.year=P^.wiadomosc.data.year)and(data1.month=P^.wiadomosc.data.month)and
     (data1.day=P^.wiadomosc.data.day)and(data1.hour=P^.wiadomosc.data.hour)and
     (data1.min=P^.wiadomosc.data.min)and(data1.sec=P^.wiadomosc.data.sec)  do
   begin
    znaleziono:=true;
    writeln;
    pokaz_wiadomosc1(P^.wiadomosc);
    write('Potwierdz usuniecie(T/N):');
    repeat
    z:=UpCase(readkey);
    until z in ['N','T'];
    writeln(z);
    if z='T' then
     begin
      writeln('Notatka usunieta');
      d:=P;
      P:=P^.next;
      dispose(d);
     end;
    write('Czy chcesz usun¥† inn¥ wiadomo˜† (T/N)');
    repeat
    z:=UpCase(readkey);
    until z in ['T','N'];
    write(z); writeln;
    if z='T' then goto czas1;

   end;
p1:=P;
p2:=p1^.next;

while p2<>nil do
     begin
      if  (data1.year=p2^.wiadomosc.data.year)and(data1.month=p2^.wiadomosc.data.month)and
        (data1.day=p2^.wiadomosc.data.day)and(data1.hour=p2^.wiadomosc.data.hour)and
        (data1.min=p2^.wiadomosc.data.min)and(data1.sec=p2^.wiadomosc.data.sec) then
        begin
         znaleziono:=true;
         writeln;
         pokaz_wiadomosc1(p2^.wiadomosc);
         write('Potwierdz usuniecie(T/N):');
         repeat
         z:=UpCase(readkey);
         until z in ['N','T'];
         writeln(z);
         if z='T' then
          begin
           writeln('Notatka usunieta');
           d:=p2;
           p1^.next:=p2^.next;
           p2:=p1;
           dispose(d);
          end;
         p1:=p2;
         p2:=p1^.next;
         write('Czy chcesz usun¥† inn¥ wiadomo˜† (T/N)');
         repeat
         z:=UpCase(readkey);
         until z in ['T','N'];
         write(z); writeln;
         if z='T' then goto czas1;
        end;
      p1:=p2;
      p2:=p1^.next;
     end;
 if znaleziono=false then
  begin
    writeln;
    writeln('Nie ma notatki o tym terminie ');
    write('Czy chcesz usun¥† inn¥ wiadomo˜† (T/N)');
    repeat
    z:=UpCase(readkey);
    until z in ['T','N'];
    write(z); writeln;
    if z='T' then goto czas1;
   end;

end;




begin
clrscr;
P:=nil;


z_pliku(P,'wiadomos.dat');

while znak<>#27 do
begin

menu;
znak:=readkey;

case znak of
'1':zapisz_wiadomosc(P);
'2':modyfikuj(P);
'3':usun(P,x);
'4':wyswietl(P);
#9: begin clrscr;  repeat wyzwalacz(P,x); until keypressed; end;
#13: Informacje;
end;

end;
do_pliku(P,'wiadomos.dat');
end.



{   (((rok>P^.wiadomosc.data.year))or
     ((rok=P^.wiadomosc.data.year)and(miesiac>P^.wiadomosc.data.month))or
     ((rok=P^.wiadomosc.data.year)and(miesiac=P^.wiadomosc.data.month)and
     (dzien>P^.wiadomosc.data.day))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz>p2^.wiadomosc.data.hour))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta>p2^.wiadomosc.data.min))or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta=p2^.wiadomosc.data.min)and(sek>p2^.wiadomosc.data.sec))){or
     ((rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta=p2^.wiadomosc.data.min)and(sek=p2^.wiadomosc.data.sec)))}



    { (rok=p2^.wiadomosc.data.year)and(miesiac=p2^.wiadomosc.data.month)and
     (dzien=p2^.wiadomosc.data.day)and(godz=p2^.wiadomosc.data.hour)and
     (minuta=p2^.wiadomosc.data.min)and(sek=p2^.wiadomosc.data.sec)    }
