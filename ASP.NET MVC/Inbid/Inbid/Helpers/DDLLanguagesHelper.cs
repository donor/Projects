using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Inbid.Models;
using Inbid.Infrastructure;

namespace Inbid.Helpers
{
    public class DDLLanguagesHelpers
    {
        public static List<Language> TranslateLanguages(List<Language> languages, HttpCookie userCookie )
        {
            if (userCookie.Value == "pl-PL")
            {
                string[] translations = { "Polski", "Angielski", "Niemiecki" };
                int i = 0;
                foreach (Language l in languages)
                {
                    l.Name = translations[i];
                    i++;
                }
            }

            if (userCookie.Value == "de-DE")
            {
                string[] translations = { "PolishDE", "EnglishDE", "DutchDE" };
                int i = 0;
                foreach (Language l in languages)
                {
                    l.Name = translations[i];
                    i++;
                }
            }



            return languages;
        }

    }

}