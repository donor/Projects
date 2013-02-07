using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inbid.Models
{
    [MetadataType(typeof(AdressMetadata))]
    public partial class Adress
    {
           class AdressMetadata
           {
               public int AdressId { get; set; }

               public string Adress { get; set; }

               public string City { get; set; }

               public string Region { get; set; }


               public string PostalCode { get; set; }

               public int CountryId { get; set; }
           }
    }
}