using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Drawing;
using System.Web.Hosting;
using UsedClothes.Models;

namespace UsedClothes.Helpers
{
    public static  class ImageHelper
    {
        
        /// <summary>
        /// The Extension method which makes call to Database and
        /// Get the Data from the table by querting to it. e
        /// It will read the byte array from table and concert into 
        /// Bitmap
        /// </summary>
        /// <param name="html"></param>
        /// <param name="ImageId"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageData(this HtmlHelper helper, int imageId,string clothesName)
        {
            TagBuilder imageData = null; //To Build the Image Tag
            var imgUrl = new UrlHelper(helper.ViewContext.RequestContext);

            UCEntities uCEntities = new UCEntities();

                    byte[] imageArray = uCEntities.Pictures.Where(x => x.PictureId == imageId).FirstOrDefault().Image;                    
                    //Convert to Image
                    TypeConverter bmpConverter = TypeDescriptor.GetConverter(typeof(Bitmap));
                    Bitmap imageReceived = (Bitmap)bmpConverter.ConvertFrom(imageArray);

                    //Now Generate the Image Tag for Mvc Html String
                    imageReceived.Save(HostingEnvironment.MapPath("~/Images") + @"\I" + imageId.ToString() + ".jpg");
                    imageData = new TagBuilder("img");
                    //Set the Image Url for <img> tag as <img src="">
                    imageData.MergeAttribute("src", imgUrl.Content("~/Images") + @"/I" + imageId.ToString() + ".jpg");
                    imageData.Attributes.Add("alt", clothesName);
                    imageData.Attributes.Add("style", "opacity:1;");

            return MvcHtmlString.Create(imageData.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ImageDataForCart(this HtmlHelper helper, int clothesId,string clothesName)
        {
            TagBuilder imageData = null; //To Build the Image Tag
            var imgUrl = new UrlHelper(helper.ViewContext.RequestContext);

            UCEntities uCEntities = new UCEntities();
            int? imageId = uCEntities.Pictures.FirstOrDefault(x => x.ClothesId == clothesId).PictureId;

           // if (imageId!=0)
           //{
            byte[] imageArray = uCEntities.Pictures.Where(x => x.PictureId == imageId).FirstOrDefault().Image;
            //Convert to Image
            TypeConverter bmpConverter = TypeDescriptor.GetConverter(typeof(Bitmap));
            Bitmap imageReceived = (Bitmap)bmpConverter.ConvertFrom(imageArray);

            //Now Generate the Image Tag for Mvc Html String
            imageReceived.Save(HostingEnvironment.MapPath("~/Images") + @"\I" + imageId.ToString() + ".jpg");
          //}
            imageData = new TagBuilder("img");
            //Set the Image Url for <img> tag as <img src="">
            imageData.MergeAttribute("src", imgUrl.Content("~/Images") + @"/I" + imageId.ToString() + ".jpg");
            imageData.Attributes.Add("alt", clothesName);
            //imageData.Attributes.Add("style", "opacity:1;");

            return MvcHtmlString.Create(imageData.ToString(TagRenderMode.SelfClosing));
        }

    }
}