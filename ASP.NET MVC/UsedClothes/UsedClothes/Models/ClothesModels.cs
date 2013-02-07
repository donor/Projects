using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UsedClothes.Models
{

    //[MetadataType(typeof(ClothMetadata))]
    public  class ClothModelView
    {
      
            [HiddenInput(DisplayValue = false)]
            public int ClothesId { get; set; }

            [Required(ErrorMessage = "Nazwa jest wymagana.")]
            [Display(Name = "Nazwa")]
            public string Name { get; set; }

            
            [Display(Name = "Opis")]
            public string Description { get; set; }


           // [Display(Name = "Zdjęcie")]
            //public HttpPostedFileBase Picture { get; set; }

            [Required(ErrorMessage = "Cena jest wymagana")]
            [Display(Name = "Cena")]
            public Decimal Price { get; set; }

            [Required(ErrorMessage = "Stan jest wymagana")]
            [Display(Name = "Stan")]
            public Boolean Condition { get; set; }

            [Required(ErrorMessage = "Płeć jest wymagana")]            
            [Display(Name = "Płeć")]
            public Boolean? Sex { get; set; }

            
            public Boolean IsVintage { get; set; }

            [Required(ErrorMessage = "Ilość jest wymagana")]
            [Display(Name = "Ilość")]
            public int Quantity { get; set; }

            [Display(Name = "Data Dodania")]
            public DateTime AddDate { get; set; }

            [Display(Name = "Sprzedane? T/N")]
            public bool IsSold { get; set; }

            [Required(ErrorMessage = "Marka jest wymagana")]
            [Display(Name = "Marka")]
            //public int BrandId { get; set; }
            public string BrandName { get; set; }

            [Display(Name = "Kolor")]
            public int? ColorId { get; set; }

            [Display(Name = "Wzór")]
            public int? PatternId { get; set; }

            [Required]
            [Display(Name = "Rozmiar")]
            public int? SizeId { get; set; }

            [Display(Name = "RodzajId")]
            public int? TypeId { get; set; }

            [Required]
            [Display(Name = "PodRodzajId")]
            public int? SubTypeId { get; set; }

            [Required(ErrorMessage = "Materiał jest wymagana")]
            [Display(Name = "Material")]
            public int? MaterialId { get; set; }

           // public IEnumerable<HttpPostedFileBase> Photos { get; set; }
            public string PhotoNameF { get; set; }
            public string PhotoNameB { get; set; }
            public string PhotoNameD0 { get; set; }
            public string PhotoNameD1 { get; set; }

            public IList<Gender> Genders { get; set; }

        //    [Display(Name="Kategorie" )]
            public IList<Type> Types { get; set; }

            public IList<SubType> SubTypes { get; set; }

            public IList<SubType> Sizes { get; set; }

            public IList<SubType> Materials { get; set; }

            public IList<SubType> Colors { get; set; }

            public IList<SubType> Patterns { get; set; }
    }
    public class Top25WMYPosition
    {

    }

}