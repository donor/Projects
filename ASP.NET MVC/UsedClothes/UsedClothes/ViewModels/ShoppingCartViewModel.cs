using System.Collections.Generic;
using UsedClothes.Models;

namespace UsedClothes.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}