using System.Linq;
using System.Web.Mvc;
using UsedClothes.Models;
using UsedClothes.ViewModels;
using System.Collections.Generic;
using System;

namespace UsedClothes.Controllers
{
    public class ShoppingCartController : Controller
    {
        UCEntities storeDB = new UCEntities();

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UpdateTotal(string quantity, string clothesid)
        {
            if (String.IsNullOrEmpty(quantity) || String.IsNullOrEmpty(clothesid))
                return null;

            int clothesId=System.Int32.Parse(clothesid);
            int quanty=System.Int32.Parse(quantity);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var cartId = cart.GetCartId(this.HttpContext);

            
            storeDB.ShoppingCarts.Where(x => x.ClothesId == clothesId && x.CartId == cartId).FirstOrDefault().Quantity = quanty;

            //position.Quantity = quanty;
            storeDB.SaveChanges();
           //var  cart1 = ShoppingCart.GetCart(this.HttpContext);
          //var data = cart1.GetTotal();
            var data = cart.GetTotal();
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }      
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            List<int> dg=new List<int>(){0,1,2,3,4};
            var modelData = dg.Select(m => new SelectListItem()
            {
                Text = m.ToString(),
                Value = m.ToString(),

            });


            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            
            

            return View(viewModel);
        }

        private bool CanAdd(int clothesId,string cartId)
        {
            var clothesInCart=storeDB.ShoppingCarts.Where(x => x.ClothesId == clothesId && x.CartId == cartId).FirstOrDefault();
            var clothesQuantity = storeDB.Clothes.Single(cloth => cloth.ClothesId == clothesId).Quantity;

            if ((clothesInCart == null) && (clothesQuantity > 0))
                return true;
            //if (clothesInCart != null){
            //    if(clothesInCart.Quantity >= clothesQuantity)
            //    return false;
            //}
            if (clothesInCart != null)
            {
                if (clothesInCart.Quantity < clothesQuantity)
                    return true;
            }
            return false;
        }

        //
        // GET: /Store/AddToCart/5
       [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult AddToCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);            
            string cartId = cart.GetCartId(this.HttpContext);
            // Retrieve the cloth from the database
            var addedCloth = storeDB.Clothes
                .Single(cloth => cloth.ClothesId == id);
            int data;
            //if (addedCloth.Quantity == 0)
           if (!CanAdd(id,cartId))
            {
                data = -1;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            // Add it to the shopping cart
            //var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedCloth);            
            data=cart.GetCount();
            //RedirectToAction("Index");
           
           return Json(data, JsonRequestBehavior.AllowGet);
           // return Json(data);
        }
         //var ControllerContext.RouteData.GetRequiredString("action")
       [AcceptVerbs(HttpVerbs.Get)]
       public JsonResult AddToCart0(int id)
       {
           var cart = ShoppingCart.GetCart(this.HttpContext);
           string cartId = cart.GetCartId(this.HttpContext);
           // Retrieve the cloth from the database
           var addedCloth = storeDB.Clothes
               .Single(cloth => cloth.ClothesId == id);

           int data;
           if (!CanAdd(id, cartId))
           {
               data = -1;
               return Json(data, JsonRequestBehavior.AllowGet);
           }
           // Add it to the shopping cart
          

           //var cart = ShoppingCart.GetCart(this.HttpContext);

           cart.AddToCart(addedCloth);
           //data = cart.GetCount();
           data = 1;

           return Json(data, JsonRequestBehavior.AllowGet);
           //return RedirectToAction("Index");
       }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the positionName to display confirmation
            string positionName = storeDB.ShoppingCarts
                .Single(item => item.RecordId == id).Cloth.ClothesName;

            // Remove from cart
           // int itemCount = cart.RemoveFromCart(id);
            cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(positionName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                //ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}