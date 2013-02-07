using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsedClothes.Helpers
{
    public static class ConditionHelper
    {
        public static MvcHtmlString ConditionData(this HtmlHelper helper, bool? condition, bool? isVintage)
        { 
            if (isVintage.Value)
                return MvcHtmlString.Create("Vintage");
            else if (!condition.Value)
                return MvcHtmlString.Create("New");
            else if (condition.Value)
                return MvcHtmlString.Create("Used");
            return null;            
        }
    }
}