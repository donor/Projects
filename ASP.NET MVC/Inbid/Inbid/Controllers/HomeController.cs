using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inbid.Infrastructure;
using Resources;
using Inbid.Models;

namespace Inbid.Controllers
{
    public class HomeController : BaseController
    {
        [HandleError]
        public ActionResult Index()
        {
            ViewBag.Message =Resources.Resources.Welcome;
            ViewBag.Title = Resources.Resources.Home;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult SetCulture(string id)
        {
            HttpCookie userCookie = Request.Cookies["Culture"];
            userCookie.Value = id;
            userCookie.Expires = DateTime.Now.AddYears(100);
            Response.SetCookie(userCookie);

            return Redirect(Request.UrlReferrer.ToString());

        }

        //Bedzie do usuniecia/////////////////////////////////////////////////////////////////////////////
        public ActionResult DateT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DateT(DateT obiekt)
        {
            if (ModelState.IsValid)
            {
            

              

               
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
