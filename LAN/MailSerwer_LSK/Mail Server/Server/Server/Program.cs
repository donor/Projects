using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Web.Mail;
using System.Xml.Serialization;

namespace Server
{
	
	class Program
	{
		const int portNo=25;
		const int portPop=110;
				
		public static TcpListener listener;
		public static TcpListener listenerPop;
		
		
	
		public static List<User> listaUser=new List<User>();
		public static Serializacja s=new Serializacja();
		public static List<Wiadomosc> wiadomosci=new List<Wiadomosc>();
		
		
		
		public static void Main(string[] args)
		{
	listaUser=s.deserializacja("users.xml");
			wiadomosci=s.deserializacjaWiadomosci("messages.xml");
			
		foreach (User u in listaUser)
			{
			Console.WriteLine(u.Adres+ " "+u.Haslo);	
			}	

		/*	foreach (Wiadomosc u in wiadomosci)
			{
			Console.WriteLine(u.Adresat+ " "+u.Odbiorca+" "+u.Temat+" "+u.Tresc+" "+u.Data);	
			}	
			
		listaUser.Add(new User("jeden@poczta","haslo1"));
			listaUser.Add(new User("dwa@poczta","haslo2"));
			listaUser.Add(new User("trzy@poczta","haslo3"));
			
			wiadomosci.Add(new Wiadomosc("jeden@poczta","dwa@poczta","Temat","Tresc listu",DateTime.Now));*/
			
		listener=new TcpListener(portNo);
		listener.Start();
	
			 listenerPop=new TcpListener(portPop);
			listenerPop.Start();
	
			Thread smtpThrea=new Thread(new ThreadStart(smtp));
			Thread popThrea=new Thread(new ThreadStart(pop));
		Thread zapis=new Thread(new ThreadStart(zapisywanie));
			
			lock(smtpThrea)
			smtpThrea.Start();
			lock (popThrea)
			popThrea.Start();
			
		lock (zapis)
		zapis.Start();
		
				}
		public  static void smtp()
		{
		
		while (true)
			{
		SMTPSerwer user=new SMTPSerwer(listener.AcceptTcpClient());
			}		
		}
		
		public static void pop()
		{
			while (true)
			{	
		POPserwer user=new POPserwer(listenerPop.AcceptTcpClient());
				
			}
		
		}
		
		public static void zapisywanie()
		{
		s.serializacja("users.xml",listaUser);	
		}
		
   }
}



