using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;
using Inbid.Validation;

namespace Inbid.Models
{
    [MetadataType(typeof(OfferMetadata))]
    public partial class Offer
    {
        class OfferMetadata
        {
            [HiddenInput(DisplayValue = false)]
            public int OfferId { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "UserNameRequired")]
            [Range(0, 999999999999)]
            public Decimal StartPrice { get; set; }

            [CurrentPrice("AuctionId","StartPrice")]
            [Range(0, 999999999999)]
            public Decimal CurrentPrice { get; set; }

            public int BDCounter { get; set; }

            [HiddenInput(DisplayValue = false)]
            public int AuctionId { get; set; }

            public Guid UserId { get; set; }

            

            //pole dla dropdownlist ma przejść na UserId
            //public string UserName { get; set; }
        }


        public bool IsHostedBy()
        {
            MembershipUser mu = Membership.GetUser();
            if (mu != null)
            {
                Guid currentUserId = (Guid)mu.ProviderUserKey;
                return (UserId == currentUserId) ? true : false;
            }
            return false;
        }

        public decimal Change ()
        {
            return StartPrice != 0 ? Math.Round(100 * Math.Abs((StartPrice-CurrentPrice)) / StartPrice, 2) : 0;
        }

        public bool IsOnline()
        {
            return Membership.GetUser(UserId).IsOnline ? true : false;       
        }

        public string GetUserName()
        {
            
            MembershipUser mu = Membership.GetUser(UserId);
            if (mu != null)
            {
                return mu.UserName;
            }
            return "Problem z dostępem do danych użytkownika";
        }

        

    }
}