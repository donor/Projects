using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using UsedClothes.Models;
using System.Web.Security;
using UsedClothes.ViewModels;

namespace UsedClothes.Controllers
{
    public class ClothesController :Controller
    {

        UCEntities uCEntities = null;
 
        //poprawic w DB podkategorie dla dzieci

         public ClothesController()       : this(new UCEntities())
        {
        }

         public ClothesController(UCEntities entities)
        {
            uCEntities = entities;
        }

         private string StorageRoot
         {
             get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files/")); } //Path should! always end with '/'
         }

        private Guid getUserId()
        {
         MembershipUser user = Membership.GetUser(HttpContext.User.Identity.Name);
          return  (Guid)user.ProviderUserKey;
        }
        //Category gender
         private IList<Gender> GetGenders()
         {
             List<Gender> g = new List<Gender>();
             g.Add(new Gender(0,"Damska"));
             g.Add(new Gender(1,"Męska"));
             g.Add(new Gender(2,"Dziecięca"));

             return g;
         }

         private IList<UsedClothes.Models.Type> GetTypes(int genderId)
         {
             if (genderId == 2)
             {
                 return uCEntities.Types.Where(t => t.Sex==null ).ToList();
             }

             bool? gender=true;

             if (genderId == 0)
                 gender = false;
             else if (genderId == 1)
                 gender = true;
             return uCEntities.Types.Where(t => t.Sex == gender).ToList();

             
         }

         private IList<UsedClothes.Models.SubType> GetSubTypes(/*int genderId,*/int typeId)
         {
             return uCEntities.GetSubTypesInType(typeId).ToList();
             //return null;
         }

         [AcceptVerbs(HttpVerbs.Get)]
         public JsonResult LoadTypesByGender(string id)
         {
             if ( String.IsNullOrEmpty(id))
                 return null;

            // System.Threading.Thread.Sleep(1500);
             var modelList = this.GetTypes(Convert.ToInt32(id));

             var modelData = modelList.Select(m => new SelectListItem()
             {
                 Text = m.TypeName,
                 Value = m.TypeId.ToString(), 

             });

             return Json(modelData, JsonRequestBehavior.AllowGet);
         }

        //
         [AcceptVerbs(HttpVerbs.Get)]
         public JsonResult LoadSubTypesByType(string id)
         {
             if (String.IsNullOrEmpty(id))
                 return null;

             var modelList = this.GetSubTypes(Convert.ToInt32(id));

             var modelData = modelList.Select(m => new SelectListItem()
             {
                 Text = m.SubTypeName,
                 Value = m.SubTypeId.ToString(),

             });

             return Json(modelData, JsonRequestBehavior.AllowGet);
         }

         private IList<UsedClothes.Models.Size> GetSizes(int id)
        {

            return uCEntities.GetSizesForType(id).ToList();
        }

         [AcceptVerbs(HttpVerbs.Get)]
         public JsonResult LoadSizesByType(string id)
         {
             if (String.IsNullOrEmpty(id))
                 return null;

             var modelList = this.GetSizes(Convert.ToInt32(id));

             var modelData = modelList.Select(m => new SelectListItem()
             {
                 Text = m.SizeName,
                 Value = m.SizeId.ToString(),

             });

             return Json(modelData, JsonRequestBehavior.AllowGet);
         }

         public ActionResult Autocomplete(string term)
         {            
             var items = uCEntities.Brands.Select(x => x.BrandName).ToArray();  //ToList().; 

             var filteredItems = items.Where(
                 item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                 );
             return Json(filteredItems, JsonRequestBehavior.AllowGet);
         }

        //
        // GET: /Clothes/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Genders = GetGenders();
           // ViewBag.Types = GetTypes(0);
            ViewBag.Types=uCEntities.Types.ToList();
            //ViewBag.SubTypes=GetSubTypes(1);
            ViewBag.SubTypes = uCEntities.SubTypes.Distinct().ToList();
            ViewBag.Sizes = uCEntities.Sizes.ToList();
            ViewBag.Materials = uCEntities.Materials.ToList();
            ViewBag.Colors = uCEntities.Colors.ToList();
            ViewBag.Patterns = uCEntities.Patterns.ToList();
            return View();
        }


        private byte[] databaseFilePut(string varFilePath)
        {
            var path = StorageRoot + Path.GetFileName(varFilePath);
            byte[] file;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    return file = reader.ReadBytes((int)stream.Length);

                }
            }
        }

        private void deleteFile(string varFilePath)
        {
            var path = StorageRoot + Path.GetFileName(varFilePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        //
        // POST: /Clothes/Create
        


        [HttpPost, Authorize]
        public ActionResult AddClothes(ClothModelView cm)
        {
            try
            {              
               // Guid d=System.Guid.Parse(b8dc0c86-4e18-4807-a94e-44c0b51069ac);
                System.Data.Objects.ObjectParameter newClothesId = new System.Data.Objects.ObjectParameter("NewClothesId", 0);
                uCEntities.InsertNewClothes(cm.Name, cm.Description,getUserId(), cm.Price, cm.Sex, cm.Condition, cm.BrandName, cm.ColorId, cm.SizeId, cm.Quantity, cm.TypeId, System.DateTime.Now, cm.MaterialId, cm.PatternId, cm.IsVintage, cm.SubTypeId, newClothesId);

                if (cm.PhotoNameF != null)
                    { uCEntities.InsertPicture("F", databaseFilePut(cm.PhotoNameF), (int)newClothesId.Value); deleteFile(cm.PhotoNameF); }
                if (cm.PhotoNameB != null)
                    { uCEntities.InsertPicture("B", databaseFilePut(cm.PhotoNameB), (int)newClothesId.Value); deleteFile(cm.PhotoNameB); }
                if (cm.PhotoNameD0 != null)
                    { uCEntities.InsertPicture("D0", databaseFilePut(cm.PhotoNameD0), (int)newClothesId.Value); deleteFile(cm.PhotoNameD0); }
                if (cm.PhotoNameD1 != null)
                    { uCEntities.InsertPicture("D1", databaseFilePut(cm.PhotoNameD1), (int)newClothesId.Value); deleteFile(cm.PhotoNameD1); }


                uCEntities.SaveChanges();

               

                TempData["Message"] = string.Format("Ubranie zostało dodane");
                return RedirectToAction("Index", "Home");// ("Detail");



            }
            catch (Exception e)
            {
                //return View();
                return null;
                //return View("AddClothes2");
            }
        }

        [HttpPost, Authorize]
        public ActionResult AddClothes2(ClothModelView cm/*, HttpPostedFileBase Picture*/)
        {
            return null;
        }


       

        //
        // GET: /Clothes/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
         

        

        private byte[] ImageToBytes(Image img, ImageFormat format)
        {
            MemoryStream mstream = new MemoryStream();
            img.Save(mstream, format);
            mstream.Flush();
            return mstream.ToArray();
        }

        //
        // GET: /Clothes/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Clothes/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Clothes/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Clothes/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Index(string gender, string type =null)
        {
            var Items = uCEntities.LastWomensPositions;
            List<ShortClothesViewModel> model=new List<ShortClothesViewModel>();
            foreach (var item in Items)
            {
                model.Add(new ShortClothesViewModel { 
                     ClothesId=item.ClothesId,
                      ClothesName=item.ClothesName,
                       Image=item.Image,
                        PictureId=item.PictureId,
                         Price=item.Price.Value,
                          SellerId=item.SellerId.Value,
                           SizeName=item.SizeName
                });
            }

            return View();
        }


    }
}
