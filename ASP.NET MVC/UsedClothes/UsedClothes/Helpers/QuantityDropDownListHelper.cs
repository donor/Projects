using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedClothes.Models;

namespace UsedClothes.Helpers
{
    public static class QuantityDropDownListHelper
    {
        public static MvcHtmlString QuantityDDList(this HtmlHelper helper, int? quantity, string propertyName, int clothesId, int currQuantity)
        {   
            TagBuilder select = new TagBuilder("select");
            //select.Attributes.Add("id", propertyName);
            select.Attributes.Add("name", propertyName);
            select.Attributes.Add("clothes-id", clothesId.ToString());
            
            for (int i = 1; i <= quantity;i++ )
            {
                TagBuilder option = new TagBuilder("option");
                option.InnerHtml = i.ToString();
                option.Attributes.Add("value", i.ToString());               
                if (i==currQuantity)
                    option.Attributes.Add("selected", "selected");               
                select.InnerHtml += option.ToString(TagRenderMode.Normal);
             }
            return MvcHtmlString.Create(select.ToString(TagRenderMode.Normal));            
        }
    }
}