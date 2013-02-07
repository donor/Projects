using System;
using System.Xml.Serialization;

namespace Server
{
	[XmlRoot("User")] 
	public class User
	{
		string _adres;
		string _haslo;
		
		public 	 User()
		{
			
		}
		
		public User(string adres, string haslo)
		{
	
			Adres=adres;
			Haslo=haslo;
		}
		[XmlElement("Adres")]
		public string Adres
		{
		 get {return _adres;}
			set {_adres=value;}
		}
		[XmlElement("Haslo")]
		public string Haslo
		{
		 get {return _haslo;}
			set {_haslo=value;}
		}
	}
}

