library ieee; 

use ieee.std_logic_1164.all; 

entity notvector is 
  
  port ( 
    A  : in  std_logic_vector(15 downto 0); 
   
    notvectorWyj  : out  std_logic_vector(15 downto 0)); 
   
    
end notvector; 

architecture behave of notvector is 

begin 
process(A) 
begin 
 
   notvectorWyj<=(not A);
   
end process; 
end behave; 
