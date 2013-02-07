/*program odlicza czas na bazie zegara tmr0 po przepelnieniu
przesuwa jedynke na PORTD
n=delta_t/(Tosc*presc)
n=12 500 dla delta _t=5ms
liczba do wpisania do TMRO to 65536-12500=53 035
53 03 = 0xCF8
TMROH=0xcf;
*/

#include <p18f458.h>

unsigned char T0H, T0L;

void TMR0_Reload(unsigned char TOH, unsigned char TOL);
void main (void)
{
T0CON=0b00000001;   //TMR0=off, zliczanie z wewnatrz Presc=1:4
TMR0H=0;
TMR0L=0;
INTCONbits.TMR0IF=0;
CMCON=0b111;  //komparatory wyl
PORTD=0b00000001;
TRISD=0;  //caly portD - wyjscie
TMR0_Reload(0xCF, 0x28);
T0CONbits.TMR0ON=1;
while (1)
	{
	while (!INTCONbits.TMR0IF)
 	        		;
 	PORTD=PORTD<<1;
	TMR0_Reload (0xCF, 0x28);
	INTCONbits.TMR0IF=0;
	}
}

void TMR0_Reload(unsigned char T0H, unsigned char T0L)
{
TMR0H=T0H;
TMR0L=T0L;

}