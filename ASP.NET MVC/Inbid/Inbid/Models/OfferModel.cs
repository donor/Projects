using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Inbid.Models
{

    public class OfferModel
    {

        [HiddenInput(DisplayValue = false)]
        public int OfferId { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public decimal StartPrice { get; set; }

        //[DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true, NullDisplayText = "No grade")]    
        [Range(0, 10000, ErrorMessage = "Cena musi zawierać się w przedziale: 1,00 - 100000,00!")]
        //  [RegularExpression(@"^[1-9]{1}[0-9]{0,5}([\,]{1}[0-9]{2})?$", ErrorMessage = "Cena musi zawierać się w przedziale: 1,00 - 100000,00!")]
        public decimal CurrentPrice { get; set; }


        //   [DisplayFormat(DataFormatString = "{0:0.00}")]

        public decimal? Change { get; set; }

        public int AuctionId { get; set; }



    }
}