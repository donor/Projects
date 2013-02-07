using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inbid.Models;
using System.Collections;

namespace Inbid.ViewModels
{
    public class BidderViewModel
    {
        public aspnet_Users BidderData { get; set; }
        public IEnumerable<Inbid.Models.vw_BidderOffers> Offers { get; set; }
    }
}