library ieee;
use ieee.std_logic_1164.all;

entity mux2x16 is
port(A0: in std_logic_vector(15 downto 0);
		A1: in std_logic_vector(15 downto 0);
		fun: in integer range 1 downto 0;
		Y: out std_logic_vector(15 downto 0));
end mux2x16;

architecture mux2x16_arch of mux2x16 is
begin
with fun select
Y <= A0 when 0,
	A1 when 1,
	 
	null when others;    
end mux2x16_arch;
