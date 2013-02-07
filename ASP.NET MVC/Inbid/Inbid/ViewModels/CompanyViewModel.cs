using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inbid.Models;

namespace Inbid.ViewModels
{
    public class CompanyViewModel
    {
        public IEnumerable<Inbid.Models.vw_CompanyMembers> Members{ get; set; }
        public IEnumerable<Inbid.Models.Auction> Auctions{ get; set; }
        public CompanyDetails CompanyDet { get; set; }
    }

    public class CompanyDetails : RegistrationModel
    {
        public DateTime? DateEditAuctionQuality { get; set; }
        public byte CurrentBidderQuality { get; set; }        
        public byte CurrentBidderViewQuality { get; set; }
        public byte CurrentAuctionQuality { get; set; }
        public string CountryName { get; set; }
    }


}