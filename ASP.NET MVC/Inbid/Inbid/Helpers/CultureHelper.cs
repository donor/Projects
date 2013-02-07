using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Inbid.Helpers
{
    public class CultureHelper
    {
        // Valid cultures
        private static readonly IList<string> _validCultures = new List<string> { "en-GB", "de-DE", "pl-PL"};

        // Include ONLY cultures you are implementing
        private static readonly IList<string> _cultures = new List<string> {
            "en-GB",  // first culture is the DEFAULT
            "de-DE",
            "pl-PL"
            // Specific cultures
           
        };



        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name">Culture's name (e.g. en-US)</param>
        public static string GetImplementedCulture(string name)
        {
            // make sure it's not null
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture(); // return Default culture

            // make sure it is a valid culture first
            if (_validCultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                return GetDefaultCulture(); // return Default culture if it is invalid


            // if it is implemented, accept it
            if (_cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
                return name; // accept it



            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)  
            var n = GetNeutralCulture(name);
            foreach (var c in _cultures)
                if (c.StartsWith(n))
                    return c;



            // else 
            // It is not implemented
            return GetDefaultCulture(); // return Default culture as no match found
        }


        /// <summary>
        /// Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return _cultures[0]; // return Default culture

        }

        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetCurrent2FirstMarkCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2);
        }

        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }


        public static string GetNeutralCulture(string name)
        {
            if (name.Length < 2)
                return name;

            return name.Substring(0, 2); // Read first two chars only. E.g. "en", "es"
        }

      


        public static int GetZoneOfCulture()
        {
            if ((Thread.CurrentThread.CurrentCulture.Name=="pl-PL")||(Thread.CurrentThread.CurrentCulture.Name=="de-DE"))
                return 1;
            else if (Thread.CurrentThread.CurrentCulture.Name == "en-GB")
                return 0;
            return 0;
        }
 
    


    }
}