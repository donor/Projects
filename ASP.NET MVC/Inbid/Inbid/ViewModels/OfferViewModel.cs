using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inbid.Models;

namespace Inbid.ViewModels
{
     
    public class OfferViewModel
    {
        public IEnumerable<Offer> Offers { get; set; }
        public Auction Auction { get; set; }
       // public Users Subscribers { get; set; }
        public IEnumerable<Subscriber> Subscribers { get; set; }
        public Message M { get; set; }
        public IEnumerable<Message> Messages { get; set; }
      
    }
}

   
