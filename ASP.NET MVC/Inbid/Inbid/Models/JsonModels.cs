using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inbid.Models
{
     public class RowUpdat
    {
        public int RowId { get; set; }
        public int NextRowId { get; set; }
        public string Value { get; set; }
        public string Change { get; set; }
        public bool Add { get; set; }
        public bool Remove { get; set; }
        public string StartValue { get; set; }
        public string IsOnline { get; set; }
        public string UserName { get; set; }
        public string LabelName { get; set; }
        public DateTime? EditDate { get; set; }


        public RowUpdat(int rowID, int nextRowId, string value, string change, string startValue, bool add, bool remove, string isOnline, string userName, string labelName, DateTime? editDate) 
        {
            RowId = rowID;
            NextRowId = nextRowId;
            Value = value;
            Change=change;
            Add =add;
            Remove = remove;
            StartValue = startValue;
            IsOnline = isOnline;
            UserName = userName;
            LabelName = labelName;
            EditDate = editDate;
        }
    }

    public class Status
    {
        public int RowId { get; set; }
        public string State { get; set; }

        public Status(int rowId, string state)
        {
            RowId = rowId;
            State = state;

        }

    }

    public class JsonForOffers
    {
      public  List<RowUpdat> UpdateRows { get; set; }
      public List<Status> States { get; set; }
      public List<ShortMessage> ShortMessages { get; set; }
      public AuctionStatusViewModel AuctionStatus { get; set; }
      public DateTime AuctionEndTime { get; set; }

    }
    public class JsonFromClientOffers
    {
       // public String AuctionStatus { get; set; }
        public int AuctionId { get; set; }
        public string[] NumberOfMessages { get; set; }
    }

}