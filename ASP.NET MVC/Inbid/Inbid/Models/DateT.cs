using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Inbid.Models
{
    public class DateT
    {

        //[Required(ErrorMessageResourceName = "ModelEventDateRequired", ErrorMessageResourceType = typeof(Resources.Resources))]
        //[Display(Name = "ModelEventDate", ResourceType = typeof(Resources.Resources))]
       // [DisplayFormat(DataFormatString = "{0:F}", ApplyFormatInEditMode = true)]
         //  [DataType(DataType.DateTime)]

        public DateTime? Data { get; set; }

    }
}