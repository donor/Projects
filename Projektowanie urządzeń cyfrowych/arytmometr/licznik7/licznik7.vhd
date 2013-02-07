LIBRARY ieee;
USE ieee.std_logic_1164.all;

entity licznik7 is
generic (czas_prop: Time := 10 ns);
port	(zegar: in bit;     
			q: out natural range  7 downto 0);
end licznik7;


architecture behawior_licznika of licznik7 is
begin
  licz: process (zegar)
    variable wartosc: natural := 0;  
  begin 
    if ((zegar = '1')) then 
       wartosc := (wartosc) mod 8;
       q <= wartosc after czas_prop;
       wartosc:=wartosc+1;
    end if;   
end process licz;
end behawior_licznika;


