using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Inbid.ActionFilter;
using System.Web.Mvc;
using Inbid.Helpers;
using System.Threading;
using Inbid.ActionFilter;


namespace Inbid.Infrastructure
{  
   //[SetCulture]
    public class BaseController : Controller
    {
        protected override void ExecuteCore()
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["Culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
            {
                
                cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

                HttpCookie userCookie = new HttpCookie("Culture");
                userCookie.Value = cultureName;
                userCookie.Expires = DateTime.Now.AddYears(100);
                Response.SetCookie(userCookie);

            }

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

           
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = " ";

            base.ExecuteCore();
        }
    }
}

