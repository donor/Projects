using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inbid.Infrastructure;
using Inbid.Models;
using Inbid.Helpers;
using System.Security.Principal;
using System.Web.Security;
using PagedList;
using System.Drawing;

namespace Inbid.Controllers
{

    [HandleErrorWithELMAH]
    public class AuctionController : BaseController
    {
        //private const int PageSize = 25;

        

         InBidEntities InBidEntities = null;
       
        // GET: /Auction/

         public AuctionController()
             : this(new InBidEntities())
        {
        }

        public AuctionController(InBidEntities entities)
        {
            InBidEntities = entities;
        }

       //public AuctionController()
       // {
       //     InBidEntities=new InBidEntities();
            
       // }
       //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
       // {
       //     base.Initialize(requestContext);                      
       // }

       void changeDateIn1Auction(Auction a)
       {
           a.StartTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
           a.EndTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
       }

       IQueryable<Auction> changeDate(IQueryable<Auction> auctions)
       {
         
               foreach (Auction a in auctions)
               {
                   changeDateIn1Auction(a);
                   //a.StartTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
                   //a.EndTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
               }
          

           return auctions;
       }
        

        // GET: /Auction/
         //   [HandleError]
        [Authorize]
       public ActionResult Index(int? page, string q = null)
       {
           IQueryable<Auction> auctions = null;

           if (!string.IsNullOrWhiteSpace(q))
           {               
               auctions = InBidEntities.Auctions.Where(x=>x.Name.Contains(q)); //.Select(x => x.Name.Contains(q));
           }
           else
               auctions = InBidEntities.Auctions.OrderByDescending(x => x.EndTime);

           //foreach (Auction a in auctions)
             //  a.RelateColor = "Blue";

           //int pageIndex = (page ?? 1) - 1;

           //return View(changeDate(auctions).ToPagedList(pageIndex, PageSize));
          // RelatedColors(auctions);

           return View(changeDate(auctions));
       }


        IQueryable<Auction> RelatedColors(IQueryable<Auction> auctions)
        {
            var colors = Enum.GetValues(typeof(KnownColor));
            int i=0;

            foreach (Auction a in auctions)
            {
                a.RelateColor=colors.GetValue(i).ToString();
                foreach (Auction au in auctions)
                {
                    var dg=colors.GetValue(i);
                    if (a.RelatedAuction==au.AuctionId)
                        au.RelateColor=colors.GetValue(i).ToString();
                }
                i++;
            }


            return auctions;
        }



        //public ActionResult Index(int? page,string q=null )
        //{
        //    var auctions = InBidEntities.Auctions.OrderByDescending(x=>x.EndTime);

        //    int pageIndex=(page ?? 1)-1;

            

        //    return View(changeDate(auctions).ToPagedList(pageIndex, PageSize));
        //}


        protected override void HandleUnknownAction(string actionName)
        {
            throw new HttpException(404, "Action not found");
        }

        public ActionResult Lost()
        {
            return View();
        }

        public ActionResult Trouble()
        {
            return View("Error");
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            Auction auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).SingleOrDefault();

            if (auction == null)
            {
                return View("NotFound");
            }
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                changeDateIn1Auction(auction);
                return View(auction);
            }

            if (!auction.IsHostedBy())  //trzeba rozbudowac
                return View("InvalidOwner");

            changeDateIn1Auction(auction);
            return View(auction);
        }

        [Authorize]
        //[OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult _Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            Auction auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).SingleOrDefault();

            if (auction == null)
            {
                return View("NotFound");
            }
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                changeDateIn1Auction(auction);
                return PartialView(auction);
            }

            if (!auction.IsHostedBy())  //trzeba rozbudowac
                return View("InvalidOwner");

            changeDateIn1Auction(auction);
            return PartialView(auction);
        }


        bool CanAddAuction()
        {
            MembershipUser mu = Membership.GetUser();
            Guid currentUserId = (Guid)mu.ProviderUserKey;

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
                return true;

            var Auctions = InBidEntities.Auctions.Where(x => x.UserId == currentUserId);

            var adminCompanyId = InBidEntities.aspnet_Users.Where(x => x.UserId == currentUserId).FirstOrDefault().CompanyId;
            var company = InBidEntities.Companies.Where(x => x.CompanyId == adminCompanyId).FirstOrDefault();

            int count = 0;
            foreach (Auction auction in Auctions)
            {
                if (auction.DateAdd.Value.ToUniversalTime() > company.DateEditAuctionQuality)  //System.DateTime.UtcNow)
                    count+=1;
                    //return false;
            }

            if (count >= company.AuctionQuality)
                return false;

            return true;
        }

        

        [Authorize]
        public ActionResult Create(string companyAdmin = null)
        {
           // var jest=Roles.IsUserInRole(User.Identity.Name, "Admin");
            if (!Roles.IsUserInRole(User.Identity.Name, "Admin"))
                return View("InvalidOwner");
            if (!CanAddAuction())
                return View("OverLimit");


            if (companyAdmin == null)
                companyAdmin = User.Identity.Name;

            int? companyId=InBidEntities.aspnet_Users.Where(x=>x.UserName==companyAdmin).FirstOrDefault().CompanyId;

            PopulateRelatedAuctionsDropDownLists(companyId);

            ViewData["Currency"] = setCurrency();
            ViewData["Direction"] = "True";
            ViewData["ViewTopOffer"] = "True";
            ViewData["EnableDisable"] = "True";

            Auction auction = new Auction()
            {
                MinJump=0,
                StartTime=DateTime.Now,
                //EndTime = DateTime.Now.AddDays(2)
                Days=30,
                Hours=0,
                Minutes=0,
                Seconds=0           
                
                   

                //EventDate = DateTime.Now.AddDays(7)
            };

            //changeDateIn1Auction(auction);

            return View(auction);
        }

        //
        // POST: /Dinners/Create

        [HttpPost, Authorize]
        public ActionResult Create(Auction auction,int IsAfterAuction=0, string companyAdmin=null )
        {

            if (!Roles.IsUserInRole(User.Identity.Name, "Admin"))
                return View("InvalidOwner");
            if (!CanAddAuction())
                return View("OverLimit");

            int? companyId;

            if (ModelState.IsValid)
            {
                Guid currentUserId;
                if (Roles.IsUserInRole(User.Identity.Name, "SuperAdmin"))
                {
                    currentUserId = InBidEntities.aspnet_Users.Where(x => x.UserName == companyAdmin).FirstOrDefault().UserId;
                }
                else
                {
                    MembershipUser mu = Membership.GetUser();
                     currentUserId = (Guid)mu.ProviderUserKey;
                }
                auction.UserId = currentUserId;

                if (companyAdmin == null)
                    companyAdmin = User.Identity.Name;

                companyId = InBidEntities.aspnet_Users.Where(x => x.UserName == companyAdmin).FirstOrDefault().CompanyId;
                var companyAuctionsCounter = InBidEntities.vw_CompanyAuctions.Where(x => x.CompanyId == companyId).Count();

                //InbidIdentity nerd = (InbidIdentity)User.Identity;
//                auction.StartTime=auction.StartTime.ToUniversalTime();
                //auction.EndTime = auction.EndTime.ToUniversalTime();
                auction.CurrentPrice = auction.StartPrice;
                //auction.EnableDisable = true;
                auction.AuctionNumber = companyId.ToString() + "-" + (companyAuctionsCounter + 1).ToString();

                auction.DateAdd = System.DateTime.UtcNow;
                auction.DateEdit = System.DateTime.UtcNow;

                if (IsAfterAuction == -1)
                {
                    if (auction.RelatedAuction == null)
                    {
                        ModelState.AddModelError("", "Nie wybrałeś powiązanej aukcji");                        

                        companyId = InBidEntities.aspnet_Users.Where(x => x.UserId == currentUserId).FirstOrDefault().CompanyId;
                        PopulateRelatedAuctionsDropDownLists(companyId);

                        return View(auction);
                    }
                    System.TimeSpan timeInterval = new TimeSpan(auction.IntervalDays, auction.IntervalHours, auction.IntervalMinutes, auction.IntervalSeconds);
                    auction.TimeInterval = timeInterval.Ticks;
                    var relatedAuctionEndTime = InBidEntities.Auctions.Where(x => x.AuctionId == auction.RelatedAuction).FirstOrDefault().EndTime;
                    auction.StartTime = relatedAuctionEndTime + timeInterval;

                    System.TimeSpan ts = new TimeSpan(auction.Days, auction.Hours, auction.Minutes, auction.Seconds);                    
                    auction.EndTime = auction.StartTime + ts;
                    
                    
                }
                else
                {
                 

                    auction.StartTime = auction.StartTime.ToUniversalTime();
                    System.TimeSpan ts = new TimeSpan(auction.Days, auction.Hours, auction.Minutes, auction.Seconds);
                    auction.EndTime = auction.StartTime + ts;
                    auction.RelatedAuction = null;
                }

                InBidEntities.AddToAuctions(auction);

                InBidEntities.SaveChanges();

                return RedirectToAction("Index");
            }

            if (companyAdmin == null)
                companyAdmin = User.Identity.Name;

            companyId = InBidEntities.aspnet_Users.Where(x => x.UserName == companyAdmin).FirstOrDefault().CompanyId;

            PopulateRelatedAuctionsDropDownLists(companyId,auction.RelatedAuction);

            return View(auction);
        }


        //
        // GET: /Auction/Edit/5

       // private DateTime tempAuctionStartTime {get; set;}

        private void PopulateRelatedAuctionsDropDownLists(int? companyId, object selectedAuctions = null)
        {
            var relatedAuctions = InBidEntities.vw_CompanyAuctions.Where(x => x.CompanyId == companyId);
            ViewBag.RelatedAuction = new SelectList(relatedAuctions, "AuctionId", "Name", selectedAuctions);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {

            Auction auction = InBidEntities.Auctions.Where(x=>x.AuctionId==id).SingleOrDefault();
            int? companyId;

            bool haveRelatedAuction = false;

           // tempAuctionStartTime = auction.StartTime;

            TimeSpan duration;

            if (auction == null)
                return View("NotFound");

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                companyId = InBidEntities.aspnet_Users.Where(x => x.UserId == auction.UserId).FirstOrDefault().CompanyId;
                if (auction.RelatedAuction != null)
                {                    
                    PopulateRelatedAuctionsDropDownLists(companyId, auction.RelatedAuction);
                    var timeInterval = TimeSpan.FromTicks(auction.TimeInterval.Value);
                    auction.IntervalDays = timeInterval.Days;
                    auction.IntervalHours = timeInterval.Hours;
                    auction.IntervalMinutes = timeInterval.Minutes;
                    auction.IntervalSeconds = timeInterval.Seconds;
                    haveRelatedAuction = true;
                }

                changeDateIn1Auction(auction);

                 duration = auction.EndTime - auction.StartTime;
                auction.Days = duration.Days;
                auction.Hours = duration.Hours;
                auction.Minutes = duration.Minutes;
                auction.Seconds = duration.Seconds;

                if (!haveRelatedAuction)
                PopulateRelatedAuctionsDropDownLists(companyId);             

               

                return View(auction);
            }
            if (!auction.IsHostedBy())
                return View("InvalidOwner");

             companyId = InBidEntities.aspnet_Users.Where(x => x.UserId == auction.UserId).FirstOrDefault().CompanyId;
             if (auction.RelatedAuction != null)
             {
                 PopulateRelatedAuctionsDropDownLists(companyId, auction.RelatedAuction);
                 var timeInterval = TimeSpan.FromTicks(auction.TimeInterval.Value);
                 auction.IntervalDays = timeInterval.Days;
                 auction.IntervalHours = timeInterval.Hours;
                 auction.IntervalMinutes = timeInterval.Minutes;
                 auction.IntervalSeconds = timeInterval.Seconds;
                 haveRelatedAuction = true;
             }

           changeDateIn1Auction(auction);

           duration = auction.EndTime - auction.StartTime;
           auction.Days = duration.Days;
           auction.Hours = duration.Hours;
           auction.Minutes = duration.Minutes;
           auction.Seconds = duration.Seconds;

           if (!haveRelatedAuction)
           PopulateRelatedAuctionsDropDownLists(companyId);

            return View(auction);
        }

        //
        // POST: /Auction/Edit/5

        [HttpPost, Authorize]
        public ActionResult Edit(int id, Auction auction, int IsAfterAuction = 0)
        {

            Auction a = InBidEntities.Auctions.Where(x => x.AuctionId == id).SingleOrDefault();

            Guid currentUserId;
                          

            if (Roles.IsUserInRole(User.Identity.Name, "SuperAdmin"))
            {
                currentUserId = InBidEntities.aspnet_Users.Where(x => x.UserId == a.UserId).FirstOrDefault().UserId;
            }
            else
            {
                MembershipUser mu = Membership.GetUser();
                currentUserId = (Guid)mu.ProviderUserKey;
            }
            int? companyId = InBidEntities.aspnet_Users.Where(x => x.UserId == currentUserId).FirstOrDefault().CompanyId;

            
                if (ModelState.IsValid)
                {
                    //if (!Roles.IsUserInRole(User.Identity.Name, "SuperAdmin"))
                    //{
                    //    if (!auction.IsHostedBy())
                    //        return View("InvalidOwner");                       
                    //}                                
               
                    a.Name = auction.Name;                  
                    a.MinJump = auction.MinJump;
                    a.Currency = auction.Currency;
                    a.Description = auction.Description;                   
                    a.EnableDisable = auction.EnableDisable;
                    a.ViewTopOffer = auction.ViewTopOffer;
                    a.DateEdit = System.DateTime.UtcNow;

                    if (IsAfterAuction == -1)
                    {
                        if (auction.RelatedAuction == null)
                        {
                            ModelState.AddModelError("", "Nie wybrałeś powiązanej aukcji");

                         

                             companyId = InBidEntities.aspnet_Users.Where(x => x.UserId == currentUserId).FirstOrDefault().CompanyId;
                            PopulateRelatedAuctionsDropDownLists(companyId);

                            return View(auction);
                        }
                        System.TimeSpan timeInterval = new TimeSpan(auction.IntervalDays, auction.IntervalHours, auction.IntervalMinutes, auction.IntervalSeconds);
                        auction.TimeInterval = timeInterval.Ticks;
                        var relatedAuctionEndTime = InBidEntities.Auctions.Where(x => x.AuctionId == auction.RelatedAuction).FirstOrDefault().EndTime;
                        auction.StartTime = relatedAuctionEndTime + timeInterval;

                        System.TimeSpan ts = new TimeSpan(auction.Days, auction.Hours, auction.Minutes, auction.Seconds);
                        auction.EndTime = auction.StartTime + ts;


                    }
                    else
                    {


                        auction.StartTime = auction.StartTime.ToUniversalTime();
                        System.TimeSpan ts = new TimeSpan(auction.Days, auction.Hours, auction.Minutes, auction.Seconds);
                        auction.EndTime = auction.StartTime + ts;
                        auction.RelatedAuction = null;
                    }

                  
                
                    InBidEntities.SaveChanges();

                    return RedirectToAction("Index");
                }

                PopulateRelatedAuctionsDropDownLists(companyId,auction.RelatedAuction);
         

            return View(auction);
        }

      

        [Authorize]
        public ActionResult Delete(int id)
        {
            Auction auction = InBidEntities.Auctions.Where(x=>x.AuctionId==id).SingleOrDefault();

            if (auction == null)
                return View("NotFound");

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
                return View(auction);

            if (!auction.IsHostedBy())
                return View("InvalidOwner");

            changeDateIn1Auction(auction);
            return View(auction);
        }

        // 
        // HTTP POST: /Dinners/Delete/1

        [HttpPost, Authorize]
        public ActionResult Delete(int id, string confirmButton)
        {

            Auction auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).SingleOrDefault();
            var offers = InBidEntities.Offers.Where(x => x.AuctionId == id);

            if (auction == null)
                return View("NotFound");
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
               
                foreach (Offer o in offers)
                       InBidEntities.Offers.DeleteObject(o);
                                
                InBidEntities.Auctions.DeleteObject(auction);
                InBidEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            if (!auction.IsHostedBy())
                return View("InvalidOwner");

            foreach (Offer o in offers)
                InBidEntities.Offers.DeleteObject(o);               
            
            InBidEntities.Auctions.DeleteObject(auction);
            InBidEntities.SaveChanges();
                        
            return RedirectToAction("Index");
        }




        string setCurrency()
        {
            if (Request.Cookies["Culture"].Value.ToString() == "en-GB")
                return "$";
            else if (Request.Cookies["Culture"].Value.ToString() == "de-DE")
                return "€";
            else if (Request.Cookies["Culture"].Value.ToString() == "pl-PL")
                return "PLN";
            return "€";
            
        }

        //int SetAuctionNumber()
        //{
        //    int counter=0;
        //    MembershipUser mu = Membership.GetUser();
        //    Guid currentUserId = (Guid)mu.ProviderUserKey;

        //    try
        //    {
        //        counter = InBidEntities.Auctions.Where(x => x.UserId == currentUserId).Max(x => x.AuctionNumber);
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        return 1;
        //    }
        //    //foreach (Auction auction in Auctions)
        //    //{            
        //    //        counter += 1;
        //    //}
        //    return counter+=1;
        //}

       


    }
}
