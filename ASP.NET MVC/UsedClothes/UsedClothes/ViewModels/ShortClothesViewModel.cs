using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedClothes.ViewModels
{
    public class ShortClothesViewModel
    {
      //  public IEnumerable<ShortClothView> Items { get; set; }
        public int ClothesId { get; set; }
        public string ClothesName { get; set; }
        public decimal Price { get; set; }
        public string SizeName { get; set; }
        public Byte[] Image { get; set; }
        public Guid SellerId { get; set; }
        public int PictureId { get; set; }
    }

    //public class ShortClothView
    //{
    //    public int ClothesTd { get; set; }
    //    public string ClothesName { get; set; }
    //    public decimal Price { get; set; }
    //    public string SizeName { get; set; }
    //    public Byte[] image { get; set; }
    //    public Guid SellerId { get; set; }
    //    public int PictureId { get; set; }
    //}
}