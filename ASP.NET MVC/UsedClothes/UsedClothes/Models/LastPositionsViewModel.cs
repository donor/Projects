using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace UsedClothes.Models
{
    public class LastPositionsViewModel
    {
        public ObjectSet<LastWomensPosition> Womens { get; set; }
        public IEnumerable<LastMensPosition> Mens { get; set; }
        public IEnumerable<LastYouthPosition> Youths { get; set; }
    }
}
