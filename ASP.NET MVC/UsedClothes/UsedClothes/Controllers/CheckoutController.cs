using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedClothes.Models;
using System.Web.Security;

namespace UsedClothes.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        UCEntities storeDB = null;
        // private  Guid userId;   
            public CheckoutController()       : this(new UCEntities())
            {
        
            }

            public CheckoutController(UCEntities entities  )
            {
                storeDB = entities;                
            }
        //
        // GET: /Checkout/
       
        

        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                order.BuyerId = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    storeDB.AddToOrders(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    
                    var items=cart.GetCartItems();
                    foreach (var i in items)
                    {
                        int? AvailableItems = storeDB.Clothes.Where(x => x.ClothesId == i.ClothesId).FirstOrDefault().Quantity;
                        if (i.Quantity >= AvailableItems)
                        {
                            storeDB.Clothes.Where(x => x.ClothesId == i.ClothesId).FirstOrDefault().Quantity = 0;
                            storeDB.Clothes.Where(x => x.ClothesId == i.ClothesId).FirstOrDefault().IsSold = true;
                        }
                        else if (i.Quantity < AvailableItems)
                        {
                            storeDB.Clothes.Where(x => x.ClothesId == i.ClothesId).FirstOrDefault().Quantity -= i.Quantity;

                        }
                    }
                    storeDB.SaveChanges();
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            Guid userId=(Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.BuyerId ==userId) ;

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
