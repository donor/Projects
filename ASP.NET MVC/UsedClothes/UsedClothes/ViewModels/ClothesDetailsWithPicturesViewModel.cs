using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedClothes.ViewModels
{
    public class ClothesDetailsWithPicturesViewModel
    {
      //  public System.Data.Objects.ObjectResult<UsedClothes.Models.ClothesDetailsViewModel> ClothesDetails { get; set; }
        public UsedClothes.Models.ClothesDetailsViewModel ClothesDetails { get; set; }
        public IEnumerable<Models.PicturesViewModel> PicturesForClothes { get; set; }
    }
}