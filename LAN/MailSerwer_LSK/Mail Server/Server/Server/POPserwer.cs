using System;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

namespace Server
{
	public class POPserwer
	{
		private TcpClient _client;
			const int LF = 10;
		
		private string _clientIP;
		private string _clientNick;
		
		//przechowuje odpowiedz
		private string partialStr;
		//sluży do wysylania/odbierania danych
		private byte[] data;
		private int count=0; //licznik poleceń
		
		private string  urzytkownik;
		private string  haslo;
		private string temat;
		private string tresc;
		
		public bool kont=false;
		private bool dalej=false;
		
		private  Wiadomosc wiadomosc;
		
		
		public POPserwer (TcpClient client)
		{
			_client=client;
		// pobranie adresu IP klienta
		_clientIP = client.Client.RemoteEndPoint.ToString();
		
		//---rozpoczecie czytania danych od klienta w osbnym wątku
		data = new byte[_client.ReceiveBufferSize];
		_client.GetStream().BeginRead(data, 0,
		System.Convert.ToInt32(_client.ReceiveBufferSize),ReceiveMessage, null);
		SendMessage(Komunikaty.odpowiedziServeraPOP[0]);
		}			
		
		public void SendMessage(string message)
		{
			try
			{
			//wysylanie tekstu
			System.Net.Sockets.NetworkStream ns;
			lock (_client.GetStream())
			{
				ns = _client.GetStream();
				byte[] bytesToSend =
				System.Text.Encoding.ASCII.GetBytes(message);
				ns.Write(bytesToSend, 0, bytesToSend.Length);
				ns.Flush();
				}
			}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
		}		
		
		//---odbieranie danych od klienta
		public void ReceiveMessage(IAsyncResult ar)
		{
		//---czytanie od klienta
		int bytesRead;
			dalej=true;
		try
		{
			lock (_client.GetStream())
			{
				bytesRead = _client.GetStream().EndRead(ar);
			}

		//---klient sie rozłąćzył
		if (bytesRead < 1)
		{
			_client.Close();	
			return;
		}
		else
		{
			
			string messageReceived;
			int i = 0;
			int start = 0;
			//---petla dopuki istnieją znaki
			while (data[i] != 0)
			{
				//--- przerwij skanowanie jeżeli i wieksze od rozmiary dany
				if (i + 1 > bytesRead)
				{
				break;
			}
			//---jeżeli wystopi LF
			if (data[i] == LF)  //LF
			{
				messageReceived = partialStr +System.Text.Encoding.ASCII.GetString(data, start, i-start); //i-start
			//	Console.WriteLine("odebrano <-----  "+messageReceived);
					dalej=false;									
			
				if (messageReceived.StartsWith(Komunikaty.komunikatyKlientaPOP[0]))
				{ //to jest USER
				
																		count=1;
												 urzytkownik=messageReceived.Substring(5);	
									urzytkownik=urzytkownik.Remove(urzytkownik.Length-1);
									SendMessage(Komunikaty.odpowiedziServeraPOP[2]);
								
				dalej=true;
				}
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaPOP[1]))&&(count==1))
				{ //tu jest PASS								
									haslo=messageReceived.Substring(4);		
								haslo=haslo.Remove(haslo.Length-1);
								if (zalogujUrzytkownika(Program.listaUser,urzytkownik,haslo,Program.wiadomosci)==-1)
								{
									SendMessage(Komunikaty.odpowiedziServeraPOP[3]);						
									_client.Close();
								}
								else
								{
									string msg=string.Format("+OK {0} maildrop has {1} messages\r\n",urzytkownik, zalogujUrzytkownika(Program.listaUser,urzytkownik,haslo,Program.wiadomosci));
									SendMessage(msg);						
								dalej=true;
									count=2;
								}
				}
				if ((messageReceived.Trim()==Komunikaty.komunikatyKlientaPOP[2])&&(count==2))
				{ //tu jest LIST 
							string msg=string.Format("+OK {0} messages\r\n",zalogujUrzytkownika(Program.listaUser,urzytkownik,haslo,Program.wiadomosci));
									SendMessage(msg);								
								dalej=true;
				}
							
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaPOP[3]))&&(count==2))
				{//tu jest RETR
					try
					{
						if ((Teksty.sprawdzNumer(messageReceived.Substring(5)))&&(System.Int32.Parse(messageReceived.Substring(5))<=zalogujUrzytkownika(Program.listaUser,urzytkownik,haslo,Program.wiadomosci)))
						{//powodzenie
									
								SendMessage(Komunikaty.odpowiedziServeraPOP[2]);		
								SendMessage(wyswietlWiadomosc(Program.wiadomosci,urzytkownik,System.Int32.Parse(messageReceived.Substring(5))));		
						}
						else
						{
								SendMessage(Komunikaty.odpowiedziServeraPOP[1]);		
						}
					}
					catch(Exception e)
					{	SendMessage(Komunikaty.odpowiedziServeraPOP[1]);	}
								
					finally
					{
						dalej=true;
					}
				}
							
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaPOP[4]))&&(count==2))
				{//DELE
						try
						{
						if ((Teksty.sprawdzNumer(messageReceived.Substring(5)))&&(System.Int32.Parse(messageReceived.Substring(5))<=zalogujUrzytkownika(Program.listaUser,urzytkownik,haslo,Program.wiadomosci)))		
						{//powodzenie
								usunWiadomosc(Program.wiadomosci,urzytkownik,System.Int32.Parse(messageReceived.Substring(5)));
						}
						else
						{
								SendMessage(Komunikaty.odpowiedziServeraPOP[1]);		
						}
						}
						catch (Exception e)
						{	SendMessage(Komunikaty.odpowiedziServeraPOP[1]);	}		
						finally
						{
							dalej=true;
						}
				} 
				
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaSMTP[4]))&&(messageReceived.Trim().Length<=5)) //zamkniecie połąćzenia
				{
				_client.Close();	
					return;	
				}
				
				else if (!dalej)
								SendMessage(Komunikaty.odpowiedziServeraPOP[1]);									
				start = i + 1;
			}
		i += 1;
		} //koniec while
		//---partial string---
		if (start != i)
		{
		partialStr =
		System.Text.Encoding.ASCII.GetString(data,
		start, i - start);
		}
		}
		//---kontynuowanie odbierania od klienta
		lock (_client.GetStream())
		{
		_client.GetStream().BeginRead(
		data, 0, System.Convert.ToInt32(
		_client.ReceiveBufferSize), ReceiveMessage,
		null);
		}
		}
		catch (Exception ex)
		{
			_client.Close();				
		}
		}

		public int zalogujUrzytkownika(List<User> users, string urzytkownik, string haslo, List<Wiadomosc>  wiadomosci)
		{
			int licznik=-1;
			foreach (User user in users)
			{
			if 	((user.Adres.ToString().Trim()==urzytkownik.ToString().Trim())&&(user.Haslo.ToString().Trim()==haslo.ToString().Trim()))
				{
					licznik=0;
					break;
				}
			}
			
			foreach(Wiadomosc w in wiadomosci)
			{
			if (w.Odbiorca.ToString()==urzytkownik)
					licznik++;				
			}			
			return licznik;
		}
		
		public string wyswietlWiadomosc(List<Wiadomosc> wiadomosci,string urzytkownik, int nrListu)
		{
			string wys=null;
			int licznik=0;
			foreach(Wiadomosc w in wiadomosci)
			{
			if (w.Odbiorca.ToString()==urzytkownik)
				{
					licznik++;
				if (licznik==nrListu)
					{
						wys="Nadawca: "+w.Adresat+"\r\n"
							+"Odbiorca: "+w.Odbiorca+"\r\n"
							+"Data: "+w.Data+"\r\n"
							+"Temat: "+w.Temat+"\r\n"
							+"Tresc: "+w.Tresc+"\r\n";	
						return wys;
						break;
					}					
				}				
			}
			return wys;
		}
		
		public void usunWiadomosc(List<Wiadomosc> wiadomosci,string urzytkownik, int nrListu)
		{
			int licznik=0;
		foreach(Wiadomosc w in wiadomosci)
			{
			if (w.Odbiorca.ToString()==urzytkownik)
				{
					licznik++;
				if (licznik==nrListu)
					{
						wiadomosci.Remove(w);
						Program.s.serializacja("messages.xml",wiadomosci);
						SendMessage("+OK message "+nrListu+" deleted\r\n");		
						break;
					}					
				}			
			}
			
			if (licznik!=nrListu)
						SendMessage(Komunikaty.odpowiedziServeraPOP[1]);			
		}
	}
}

