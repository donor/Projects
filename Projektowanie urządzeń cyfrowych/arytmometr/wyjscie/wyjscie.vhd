library ieee; 
use ieee.std_logic_1164.all; 

entity wyjscie is 
  
  port (  Wej   : in  std_logic_vector(15 downto 0); 
			SEL : in  natural range 7 downto 0; 
			Y1  : out  std_logic_vector(7 downto 0)); 
end wyjscie; 

architecture behave of wyjscie is 

begin 
process(Wej,SEL) 
begin 

if  SEL=6 then             
  
   Y1(7) <= Wej(15);
   Y1(6) <= Wej(14);
   Y1(5) <= Wej(13);
   Y1(4) <= Wej(12);
   Y1(3) <= Wej(11);
   Y1(2) <= Wej(10);
   Y1(1) <= Wej(9);
   Y1(0) <= Wej(8);
	
end if; 

if SEL=7 then 
   Y1(7) <= Wej(7);
   Y1(6) <= Wej(6);
   Y1(5) <= Wej(5);
   Y1(4) <= Wej(4);
   Y1(3) <= Wej(3);
   Y1(2) <= Wej(2);
   Y1(1) <= Wej(1);
   Y1(0) <= Wej(0);

end if; 

if (SEL=0 or SEL=1 or SEL=2 or SEL=3 or SEL=4 or SEL=5) then
Y1<=x"00";
end if ;

end process; 
end behave; 
