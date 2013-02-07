using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Browser.Models;
using System.Text;

namespace Browser.Helpers
{
    public static class BrowserHelper
    {
        public static MvcHtmlString ExtractElements(this HtmlHelper helper, Element ele)
        {
            TagBuilder output = new TagBuilder("li");
            output.SetInnerText(ele.Name);
            if (ele.Children != null)
            {
                TagBuilder output1 = new TagBuilder("ul");
                foreach (var item in ele.Children)
                {
                    TagBuilder output2 = new TagBuilder("li");
                    output2.SetInnerText(item.Name);
                    output1.InnerHtml += output2.ToString(TagRenderMode.Normal);
                    ExtractElements(null,item);
                }
                output.InnerHtml = output1.ToString(TagRenderMode.Normal);

            }

            return MvcHtmlString.Create(output.ToString(TagRenderMode.Normal)); ;
        }
    }
}

//<li> 
//        @ele.Name
//        @if (ele.Children != null)
//        {
//            <ul>
//            @foreach (var item in ele.Children)
//            {
//                <li>@item.Name </li>
//                //if (item.Children != null){
//                   // ExtractElements(item);
//                //}
//             }
//            </ul>
//        }
//        </li>

//StringBuilder output = new StringBuilder();
//     if(_object.ListOfObjects.Count > 0)
//     {
//         output.Append("<ul>");

//         foreach(MyObject subItem in _object.listOfObjects)
//         {
//             output.Append("<li>");
//             output.Append(_object.Title);
//             output.Append(html.ShowSubItems(subItem.listOfObjects);
//             output.Append("</li>")
//         }
//         output.Append("</ul>");
//     }
//     return output.ToString();