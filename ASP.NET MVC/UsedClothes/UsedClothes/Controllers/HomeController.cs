using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedClothes.Models;

namespace UsedClothes.Controllers
{
    public class HomeController : Controller
    {
          UCEntities uCEntities = null;

            public HomeController()       : this(new UCEntities())
            {
        
            }

            public HomeController(UCEntities entities)
            {
                uCEntities = entities;
            }

        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";
            LastPositionsViewModel lpvm = new LastPositionsViewModel()
            {
                Womens=uCEntities.LastWomensPositions,
                Mens = uCEntities.LastMensPositions,
                Youths=uCEntities.LastYouthPositions
            };            
            
            return View(lpvm);
        }

        public ActionResult About()
        {
            
            return View();
        }
    }
}
