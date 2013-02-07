library ieee; 
use ieee.std_logic_1164.all; 

entity rejestr is 
  
  port (Wej   : in  std_logic_vector(15 downto 0); 
			fun : in integer range 7 downto 0;
		Wyj  : out  std_logic_vector(15 downto 0));    
end rejestr; 

architecture behave of rejestr is 

begin 
process(Wej,fun) 
variable wnetrze:std_logic_vector(15 downto 0);
begin 

if fun=5 then
  
   wnetrze(15) := Wej(15);
   wnetrze(14) := Wej(14);
   wnetrze(13) := Wej(13);
   wnetrze(12) := Wej(12);
   wnetrze(11) := Wej(11);
   wnetrze(10) := Wej(10);
   wnetrze(9) := Wej(9);
   wnetrze(8) := Wej(8);

   wnetrze(7) := Wej(7);
   wnetrze(6) := Wej(6);
   wnetrze(5) := Wej(5);
   wnetrze(4) := Wej(4);
   wnetrze(3) := Wej(3);
   wnetrze(2) := Wej(2);
   wnetrze(1) := Wej(1);
   wnetrze(0) := Wej(0);
 end if;  

if fun=0 then
   Wyj(15) <= wnetrze(15);
   Wyj(14) <= wnetrze(14);
   Wyj(13) <= wnetrze(13);
   Wyj(12) <= wnetrze(12);
   Wyj(11) <= wnetrze(11);
   Wyj(10) <= wnetrze(10);
   Wyj(9) <= wnetrze(9);
   Wyj(8) <= wnetrze(8);

   Wyj(7) <= wnetrze(7);
   Wyj(6) <= wnetrze(6);
   Wyj(5) <= wnetrze(5);
   Wyj(4) <= wnetrze(4);
   Wyj(3) <= wnetrze(3);
   Wyj(2) <= wnetrze(2);
   Wyj(1) <= wnetrze(1);
   Wyj(0) <= wnetrze(0);
 end if;  
end process; 
end behave; 
