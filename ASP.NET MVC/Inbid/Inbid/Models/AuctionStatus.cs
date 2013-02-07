using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inbid.Models
{
     //  public int[] Rows{get; set;}

    

    [Serializable]
    public class AuctionStatus
    {
        public string CurrentStatus {get; set;}
    }
   

    public class AuctionStatusViewModel
    {
        public bool Value { get; set; }
        public string Status { get; set; }
    }

    //[Serializable]
    //public class Rows
    //{
     
    //}

}