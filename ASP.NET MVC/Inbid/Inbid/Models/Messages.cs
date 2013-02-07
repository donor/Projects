using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Inbid.ViewModels;

namespace Inbid.Models
{
     //   [Serializable]
    public  class Messages
    {

        public   IList<Message> Informations { get; set; }
    }

    public class Message 
    {
        [HiddenInput(DisplayValue = false)]
       public  int AuctionId { get; set; }
        public int MessageId { get; set; }
        //public Guid Publisher { get; set;}
        [DataType(DataType.MultilineText)]
        public string  Information {get; set;}
        public IEnumerable<Guid> Subscribers { get; set; }      
    }
    public class ShortMessage
    {
        public int MessageId { get; set; }
        public string Information { get; set; }
    }

    public class Subscriber
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        //public bool IsCheckbox { get; set; }
    }
}