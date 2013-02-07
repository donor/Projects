using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedClothes.Models;
using UsedClothes.ViewModels;

namespace UsedClothes.Controllers
{
    public class StoreController : Controller
    {
        UCEntities storeDB = new UCEntities();

        //
        // GET: /Store/

        //public ActionResult Index()
        //{
        //    var genres = storeDB.Genres.ToList();

        //    return View(genres);
        //}

        //
        // GET: /Store/Browse?genre=Disco

        //public ActionResult Browse(string genre)
        //{
        //    // Retrieve Genre and its Associated Albums from database
        //    var genreModel = storeDB.Genres.Include("Albums")
        //        .Single(g => g.Name == genre);

        //    return View(genreModel);
        //}

        //
        // GET: /Store/Details/5

        // GET: /Store/GetBookPartialView  method
        private void IncrementDisplayCount(int id)
        {
            storeDB.Clothes.Where(x => x.ClothesId == id).FirstOrDefault().DisplayCount += 1;
            storeDB.SaveChanges();
        }

        public PartialViewResult GetDetailsPartialView(int id/*, string param2*/)
        {
            //id=-1;
            try
            {                
                var cloth = storeDB.GetClothesDetails(id).SingleOrDefault();
                var pictures = storeDB.GetPicturesForClothes(id).ToList();

                if (Request.IsAjaxRequest())
                {
                    var item = new ClothesDetailsWithPicturesViewModel()
                    {
                        ClothesDetails = cloth,
                        PicturesForClothes = pictures
                    };
                    IncrementDisplayCount(id);
                    return PartialView("_Details", item);
                }
                else
                {
                    return null;
                }               
            }
            catch(Exception ex)
            {
                  TempData["Message"] = string.Format("Can't display details");
                  return null;
            }
        }

        public ActionResult Details(int id)
        {
            var cloth = storeDB.GetClothesDetails(id).SingleOrDefault();
            var pictures = storeDB.GetPicturesForClothes(id).ToList();

            var item = new ClothesDetailsWithPicturesViewModel()
                   {
                        ClothesDetails=cloth,
                        PicturesForClothes=pictures
                   };
            IncrementDisplayCount(id);
            return View(item);
        }

        //
        // GET: /Store/GenreMenu

        //[ChildActionOnly]
        //public ActionResult GenreMenu()
        //{
        //    var genres = storeDB.Genres.ToList();

        //    return PartialView(genres);
        //}


    }
}
