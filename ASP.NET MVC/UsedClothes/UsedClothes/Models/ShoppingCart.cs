using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsedClothes.Models
{
    public partial class ShoppingCart
    {
        UCEntities storeDB = new UCEntities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Cloth cloth)
        {
             //Get the matching cart and album instances
            var cartItem = storeDB.ShoppingCarts.SingleOrDefault(
                c => c.CartId ==ShoppingCartId
                && c.ClothesId == cloth.ClothesId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new ShoppingCart
                { 
                    ClothesId=cloth.ClothesId,                    
                    CartId = ShoppingCartId, 
                    Quantity=1,                    
                    DateCreated = DateTime.Now
                };
//                storeDB.ShoppingCarts.Add(cartItem);
                storeDB.AddToShoppingCarts(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Quantity++;
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public void RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = storeDB.ShoppingCarts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);
           
            storeDB.ShoppingCarts.DeleteObject(cartItem);
            storeDB.SaveChanges();            
        }
        public void EmptyCart()
        {
            var cartItems = storeDB.ShoppingCarts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.ShoppingCarts.DeleteObject(cartItem);
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public List<ShoppingCart> GetCartItems()
        {
            return storeDB.ShoppingCarts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.ShoppingCarts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in storeDB.ShoppingCarts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Quantity *
                              cartItems.Cloth.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {  
                    ClotheId=item.ClothesId,
                    OrderDetailsId=order.OrderId,
                    UnitCost=item.Cloth.Price,                    
                    Quantity = item.Quantity
                };
                // Set the order total of the shopping cart
                orderTotal += (int)(item.Quantity * item.Cloth.Price);

                //storeDB.OrderDetails.Add(orderDetail);
                storeDB.AddToOrderDetails(orderDetail);
              //  storeDB.SaveChanges();

            }
            // Set the order's total to the orderTotal count
            order.TotalPrice = orderTotal;

            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.ShoppingCarts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (ShoppingCart item in shoppingCart)
            {
                item.CartId = userName;
            }
            storeDB.SaveChanges();
        }
    }
}