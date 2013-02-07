using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inbid.Models
{
     [MetadataType(typeof(CompanyMetadata))]
    public partial class Company
    {
         class CompanyMetadata
         {
             public int CompanyId { get; set; }

             public string Name { get; set; }

             public int AdressId { get; set; }
         }
    }
}