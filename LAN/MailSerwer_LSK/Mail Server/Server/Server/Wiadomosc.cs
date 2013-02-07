using System;
using System.Xml.Serialization;

namespace Server
{
	[XmlRoot("Wiadomosc")] 
	public class Wiadomosc
	{
		string _adresat;
		string _odbiorca;
		string _temat;
		string _tresc;
		DateTime _data;
		
		[XmlElement("adresat")]
		public string Adresat 
		{
			get {return _adresat ;}
			set { _adresat=value;}
		}
		[XmlElement("odbiorca")]
		public string Odbiorca
		{
			get {return _odbiorca;}
			set { _odbiorca=value; }
		}
		[XmlElement("temat")]
		public string Temat 
		{
			get {return _temat ;} 
			set {_temat=value;}
		}
		[XmlElement("tresc")]
		public string Tresc 
		{
			get {return _tresc;}
			set {_tresc=value;}
		}
		[XmlElement("data")]
		public DateTime Data
		{
			get {return _data;}
			set {_data=value;}
		}
		
		public Wiadomosc ()
		{
		}
		public Wiadomosc (string adresat, string odbiorca, string temat, string tresc, DateTime data)
		{
			Adresat=adresat;
			Odbiorca=odbiorca;
			Temat=temat;
			Tresc=tresc;
			Data=data;
		}
	}
}

