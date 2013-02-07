using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;
//using System.Runtime.Serialization.Formatters.Binary;


namespace Server
{	

	
	public class Serializacja
	{

		public Serializacja()
		{
		}
		
		public  void serializacja(string path, List<User> u)
		{
		
		XmlRootAttribute oRootAtrr=new XmlRootAttribute();
		oRootAtrr.IsNullable=true;
		XmlSerializer oSerializer=new XmlSerializer(typeof (List<User>),oRootAtrr);
			StreamWriter oStreamWriter=null;
			try
			{
			oStreamWriter=new StreamWriter(path);
				oSerializer.Serialize(oStreamWriter,u);
			}
		catch (Exception ex)
			{
			Console.WriteLine(ex.Message)	;
			}
			finally
			{
			if (oStreamWriter!=null)
					oStreamWriter.Dispose();				
			}		
		}
		
		public void serializacja(string path, List<Wiadomosc> w)
		{
			XmlRootAttribute oRootAtrr=new XmlRootAttribute();
			oRootAtrr.IsNullable=true;
			XmlSerializer oSerializer=new XmlSerializer(typeof (List<Wiadomosc>),oRootAtrr);
			StreamWriter oStreamWriter=null;
			try
			{
			oStreamWriter=new StreamWriter(path);
				oSerializer.Serialize(oStreamWriter,w);
			}
		catch (Exception ex)
			{
			Console.WriteLine(ex.Message)	;
			}
			finally
			{
			if (oStreamWriter!=null)
					oStreamWriter.Dispose();				
			}	
		}
		
		 public List<User> deserializacja(string path)
        {
			StreamReader odczyt=null;
			FileStream plik=null;
			List<User> items=null;
			
			using (plik = new FileStream(path,FileMode.Open, FileAccess.Read,FileShare.ReadWrite, 8))
			{
				try 
				{
				 	odczyt = new StreamReader(plik);
		            XmlSerializer sr = new XmlSerializer(typeof(List<User>));
	  		    	items= (List<User>)sr.Deserialize(odczyt);
					return items;
				}
				catch(Exception es)
				{
					return null;
				}
				finally 
				{
					odczyt.Close();
					plik.Close();					
				}		
			}	
        }
		
		
		public List<Wiadomosc> deserializacjaWiadomosci(string path)
        {
			StreamReader odczyt=null;
			FileStream plik=null;
			List<Wiadomosc> items=null;
			
			using (plik = new FileStream(path,FileMode.Open, FileAccess.Read,FileShare.ReadWrite, 8))
			{
				try 
				{
				 	odczyt = new StreamReader(plik);
		            XmlSerializer sr = new XmlSerializer(typeof(List<Wiadomosc>));
	  		    	items= (List<Wiadomosc>)sr.Deserialize(odczyt);
					return items;
				}
				catch(Exception es)
				{
					return null;
				}
				finally 
				{
					odczyt.Close();
					plik.Close();					
				}		
			}
		}		
	}
}

