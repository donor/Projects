using System;
namespace Server
{
	public static class Komunikaty
	{
		const string domena="poczta";    //pseudodomena
		
		public static string[] komunikatyKlientaSMTP=
		{
			/*0*/"MAIL FROM: ",
			/*1*/"RCPT TO: ",
			/*2*/"SUBJECT: ",
			/*3*/"DATA",
			/*4*/"QUIT\r",
			/*5*/".",
			/*6*/"EHLO "
		}; //Moze brakowac "."
		public static string[] odpowiedziServeraSMTP=
		{
			
			/*0*/"220 Serwer Pocztowy gotowy\n",
			/*1*/"250 OK\n",
			/*2*/"220 Serwer Gotowy\n",
			/*3*/"354 Wyslij dane. Zakoncz CRLF.CRLF\n",
			/*4*/"502 Polecenie nie obslugiwane\n",
			/*5*/"501 Blad w skladni parametrow lub opcji polecenia\n",
			/*6*/"354 Rozpoczecie przyjmowania tresci wiadomosci e-mail, zakoncz przyciskiem <Enter>\n",
			/*7*/"553 Operacja nie zostala podjeta, nazwa skrzynki nie jest dopuszczalna\n",	
				/*8*/"EHLO "
		
		};
		
		public static string[] odpowiedziServeraPOP=
		{
			/*0*/"+OK POP3 serwer gotowy\r\n",
			/*1*/"ERR\r\n",
			/*2*/"+OK \r\n",
			/*3*/ "ERR [AUTH] Blad autoryzacji. Autorization failed\r\n"
				
			
		};
		
			public static string[] komunikatyKlientaPOP=
		{
			/*0*/ "USER ",
			/*1*/ "PASS ",	
			/*2*/ "LIST",
			/*3*/ "RETR ",
			/*4*/ "DELE "
		};
		
	}
}

