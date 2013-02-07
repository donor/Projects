using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections;
using System.Net.Mail;
using System.Threading;

namespace Server
{
	public class SMTPSerwer
	{
		//---stała koniec linii---
		const int LF = 10;
		//---informacje o kliencie
		private TcpClient _client;
		private string _clientIP;
		private string _clientNick;
		
		//przechowuje odpowiedz
		private string partialStr;
		//sluży do wysylania/odbierania danych
		private byte[] data;
		private int count=0; //licznik poleceń
		//elementy wiadomosci
		private string adresat;
		private string  odbiorca;
		private string temat;
		private string tresc;
		
		
		
		private bool dalej=false;
		
		//---gdy klient jest połaćzony
		public SMTPSerwer(TcpClient client)
		{
		_client = client;
		// pobranie adresu IP klienta
		_clientIP = client.Client.RemoteEndPoint.ToString();
		//---rozpoczecie czytania danych od klienta w osbnym wątku
		data = new byte[_client.ReceiveBufferSize];
		_client.GetStream().BeginRead(data, 0,
		System.Convert.ToInt32(_client.ReceiveBufferSize),ReceiveMessage, null);
		SendMessage(Komunikaty.odpowiedziServeraSMTP[2]);				
		}		
		
		//wysylanie wiadomosci do klienta 
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
				//	int count=0;
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
			{		//	count++;
				messageReceived = partialStr +System.Text.Encoding.ASCII.GetString(data, start, i-start); //i-start
				//Console.WriteLine("received <-----  "+messageReceived);
							
							
				dalej=false;				
			
							
				if (messageReceived.StartsWith(Komunikaty.komunikatyKlientaSMTP[0]))
				{
				
								if (!Teksty.sprawdzAdres(messageReceived.Substring(11)))
								{
									SendMessage(Komunikaty.odpowiedziServeraSMTP[7]);
								}
								else
								{
									count=1;
				    				 adresat=messageReceived.Substring(11);													
					 				adresat=adresat.Remove(adresat.Length-1);
									SendMessage(Komunikaty.odpowiedziServeraSMTP[1]);
								}
				dalej=true;
				}
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaSMTP[1]))&&(count==1))
				{
					if (!Teksty.sprawdzAdres(messageReceived.Substring(9))||(!Teksty.sprawdzIstnienieAdresu(Program.listaUser,messageReceived.Substring(9))))
								{
									SendMessage(Komunikaty.odpowiedziServeraSMTP[7]);									
								}
								else
								{
									count=2;									
									 odbiorca=messageReceived.Substring(9);		
									odbiorca=odbiorca.Remove(odbiorca.Length-1);
									SendMessage(Komunikaty.odpowiedziServeraSMTP[1]);
								}
								dalej=true;
				}
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaSMTP[2]))&&(count==2))
				{					
									temat=messageReceived.Substring(8);		
								temat=temat.Remove(temat.Length-1);
									SendMessage(Komunikaty.odpowiedziServeraSMTP[1]);							
								dalej=true;
				}
							
				if (count==3)
				{
						tresc=messageReceived.Remove(messageReceived.Length-1);	
								count=4;							
								Program.wiadomosci.Add(new Wiadomosc(adresat,odbiorca,temat,tresc, DateTime.Now));							
								lock(Program.wiadomosci)
								Program.s.serializacja("messages.xml",Program.wiadomosci);
								dalej=true;
				}
							
				if ((messageReceived.Trim()==Komunikaty.komunikatyKlientaSMTP[3])&&(count==2))
				{
									SendMessage(Komunikaty.odpowiedziServeraSMTP[6]);
					count=3;
								dalej=true;
				} 
				if ((count==4))
				{
					SendMessage(Komunikaty.odpowiedziServeraSMTP[1]);
				count=0;			
								dalej=true;
				}
				if ((messageReceived.StartsWith(Komunikaty.komunikatyKlientaSMTP[4]))&&(messageReceived.Length<=5)) //zamkniecie połąćzenia
				{														
				_client.Close();	
					return;	
				}
				
				else if ((count!=3)&&(!dalej))
								SendMessage(Komunikaty.odpowiedziServeraSMTP[5]);
							
		
				
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
	

		
		

		
		
	}
		
}

