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
    [MetadataType(typeof(AuctionMetadata))]
    public partial class Auction
    {
        class AuctionMetadata
        {
            [HiddenInput(DisplayValue = false)]
            public int AuctionId { get; set; }

            [Display(Name = "AuctionNumber", ResourceType = typeof(Resources.Resources))]
            public int AuctionNumber { get; set; }

            [Display(Name = "StartPrice", ResourceType = typeof(Resources.Resources))]
            [Range(0, 999999999999)]
            public Decimal StartPrice { get; set; }

            [Display(Name = "CurrentPrice", ResourceType = typeof(Resources.Resources))]
            [Range(0, 999999999999)]
            public Decimal CurrentPrice { get; set; }

            [Display(Name = "AuctionDirection", ResourceType = typeof(Resources.Resources))]
            public bool Direction { get; set; }

             [Display(Name = "Currency", ResourceType = typeof(Resources.Resources))]
            public string Currency { get; set; }

            [Display(Name = "AuctionName", ResourceType = typeof(Resources.Resources))]
            public string Name { get; set; }

            [DataType(DataType.DateTime)]
            [Display(Name = "StartTime", ResourceType = typeof(Resources.Resources))]
           // [DateStartGreaterNow]
            public DateTime StartTime {get; set ;}

               [DataType(DataType.DateTime)]
            [Display(Name = "EndTime", ResourceType = typeof(Resources.Resources))]
           // [DateGreaterThanAttribute("StartTime")]  
            public DateTime EndTime { get; set; }

            [Display(Name = "EnableDisable", ResourceType = typeof(Resources.Resources))]
            public bool? EnableDisable { get; set; }

            public Guid UserId { get; set; }

            [Display(Name = "MinJump", ResourceType = typeof(Resources.Resources))]
            public Decimal MinJump { get; set; }

            [Display(Name = "ViewTopOffer", ResourceType = typeof(Resources.Resources))]
            public bool ViewTopOffer { get; set; }

            //public int NextAuction { get; set; }

            //[Required(ErrorMessageResourceName = "ModelDescriptionRequired", ErrorMessageResourceType = typeof(Resources.Resources))]
            //[StringLength(256, ErrorMessageResourceName = "ModelDescriptionTooLong", ErrorMessageResourceType = typeof(Resources.Resources))]
            //[DataType(DataType.MultilineText)]
            //[Display(Name = "ModelDescription", ResourceType = typeof(Resources.Resources))]

             [Display(Name = "Description", ResourceType = typeof(Resources.Resources))]
             [DataType(DataType.MultilineText)]
            public string Description { get; set; }

             public int RelatedAuction { get; set; }

             

         

        }
          
            public int Days { get; set;}

        public int Hours { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }

        public int IntervalDays { get; set;}

        public int IntervalHours { get; set; }

        public int IntervalMinutes { get; set; }

        public int IntervalSeconds { get; set; }

       

        //public bool IsAfterAuction { get; set; }


        public int OfferCounter { get; set; }

        //pole wykorzystywania powiązanych aukcji
        public string RelateColor { get; set; }


        public bool IsHostedBy()
        {
            MembershipUser mu = Membership.GetUser();
            if (mu != null)
            {
                Guid currentUserId = (Guid)mu.ProviderUserKey;
                return (UserId == currentUserId) ? true : false;
            }
            return  false;
        }

        public string GetUserName()
        {
            

              MembershipUser mu=Membership.GetUser(UserId);
              if (mu != null)
              {
                  return mu.UserName;
              }
              return "Problem z dostępem do danych użytkownika";
        }

        //public int OfferCount()
        //{
            
        //    return 0;
        //}


        
    }

    //public class 
}