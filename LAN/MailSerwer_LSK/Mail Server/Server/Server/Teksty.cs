using System;
using System.Collections.Generic;

namespace Server
{
	public static class Teksty
	{
		public static bool sprawdzAdres(string adres)
		{
			if (!adres.Contains("@"))
			return false;
			else if ((adres[0]=='\x0040')||(adres[adres.Length-1]=='\x0040'))
			return false;
			else
				return true;		
		}
		
		public static bool sprawdzNumer(string numer)
		{
		try
			{
			if (System.Int32.Parse(numer)>0)								
				return true;		
				else
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		
		public static bool sprawdzIstnienieAdresu(List<User> users, string adres)
		{
			foreach (User user in users)
			{
			if ((user.Adres.ToLower()+"\r")==adres.ToLower())
				{
				return true;
					break;
				}
			}			
			return false;				
		}
	
	}
}

