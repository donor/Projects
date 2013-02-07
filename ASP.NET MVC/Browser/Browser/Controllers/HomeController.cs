using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Browser.Models;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.Remoting.Contexts;

namespace Browser.Controllers
{
    public class ViewModel
    {
       public string outer { get; set; }
    }



    public class HomeController : Controller
    {
        //private int id = 0;
        private const object ElementSessionKey = null;
       
        static Element outer = null;
     

        [HttpGet]
        public ActionResult Index(/*string access_token/*, string code*/)
        {



            return View();
        }

        [HttpGet]
        public ActionResult Application(string AppName="inbid")
        {
            DirectoryInfo dirTop;
            
            Element Ele;
          //  try
           // {

            string filepath = Server.MapPath("\\Projects\\"/* + AppName*/);
           // string filepath = "https://googledrive.com/host/0B9LmXsls_gB8R0NLNnJ3MXdXWGs/";



            dirTop = new DirectoryInfo(filepath);
          
           
             Ele = new Element() { Name = dirTop.Name, Id=0  };
            int id = 0;
            Browse(dirTop, Ele,ref id);


            this.Session["ElementSessionKey"] = Ele;
            //}
            // catch(Exception e)
           // {
             //   return Content(e.Message);
            //}

            return View(Ele);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ExploreFile(string id)
        {
            if (String.IsNullOrEmpty(id))
                return null;

            Element obj = Session["ElementSessionKey"] as Element;
            if (obj == null)
            {
                obj = new Element();
                Session["ElementSessionKey"] = obj;
            }
            int number = System.Int32.Parse(id);
           

            FindElement(ref obj, ref number);
            FileModel fm = new FileModel();
            fm.Name = outer.Name;
            string ex = outer.Extension.ToLower();
            if ((ex == ".dll") || (ex == ".exe") || (ex == ".MDF") || (ex == ".suo") || (ex == ".png") || (ex == ".bmp") || (ex == ".jpeg") || (ex == ".jpg") || (ex == ".gif") || (ex == ".ico"))            
                fm.Content = "Nie można wyświetlić pliku tego typu.";           
            else
            {
                try
                {
                    //string filepath = Server.MapPath("\\Projects\\" + outer.Path + "\\" + outer.Name);
                    //string filepath = Server.MapPath("\\App_Data\\" + outer.Path);
                    //string filepath = Server.MapPath(outer.Path);
                    using (var stream = new StreamReader(outer.Path))
                    {
                        fm.Content = stream.ReadToEnd();
                    }
                }
                catch (Exception exc)
                {
                    fm.Content = "Error";
                }
            }


            return Json(fm, JsonRequestBehavior.AllowGet);
        }



        private void FindElement(ref Element ele,ref int number)
        {
            if (ele.Id == number)
            {
                outer = ele;
                return;
            }
            if (ele.Children != null)
                foreach (var e in ele.Children){
                    Element e0 = e;
                    FindElement(ref e0, ref number);
                }
            if (ele.Children == null)
            {
                return;
            }
        }

        private void Browse(DirectoryInfo dir, Element el, ref int id)
        {
            List<Element> children = new List<Element>();
            foreach (var fi in dir.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
            {
                id += 1;
                //children.Add(new Element() { Name = fi.Name, Extension = fi.Extension, Path = fi.DirectoryName.Remove(0, 37), Id = id });
                children.Add(new Element() { Name = fi.Name, Extension = fi.Extension, Path = fi.FullName/* .Directory.Name*/, Id = id });
            }

            foreach (var di in dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                id += 1;
                children.Add(new Element() { Name = di.Name, Id = id });
                el.Children = children;
                Browse(di, el.Children.LastOrDefault(), ref id);              
            }
            if ((dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).Count() == 0) && (children.Count > 0))
            {
                el.Children = children;
                return;
            }
        }        
        public ActionResult About()
        {
            return View();
        }
    }
}

//  string content = string.Empty;
//try
//{
//    using (var stream = new StreamReader(filepath))
//    {
//        content = stream.ReadToEnd();
//    }
//}
//catch (Exception exc)
//{
//    return Content("Uh oh!");
//}


//var vm = new ViewModel
//{
//    outer = content
//};