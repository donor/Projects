using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inbid.Models;
using Inbid.ViewModels;
using Inbid;
using Newtonsoft.Json;
using Inbid.Infrastructure;

using System.Data;
using System.Web.Security;
using Inbid.Helpers;
using System.Threading;
using System.Globalization;
using Resources;
using System.Collections;
using System.Text;


namespace Inbid.Controllers
{
    [HandleErrorWithELMAH]
    [Authorize]
    public class OfferController : BaseController
    {

        string[] stats = new string[] { "Nie rozpocżeta", "Trwa", "Zawieszona", "Zakończona" };

        TimeSpan deathTimeExtend=TimeSpan.FromSeconds(30);

        InBidEntities InBidEntities = null;


        // GET: /Auction/

        string SetState(string[] stats, int id)
        {
            var auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).First();

            if (auction.StartTime > System.DateTime.Now.ToUniversalTime())
            {
                return stats[0];
            }
            else if (auction.EndTime < System.DateTime.Now.ToUniversalTime())
            {
                return stats[3];
            }
            else if ((auction.EnableDisable == true) && (auction.StartTime < System.DateTime.Now.ToUniversalTime()) && (auction.EndTime > System.DateTime.Now.ToUniversalTime()))
            {
                return stats[1];
            }
            else if ((auction.EnableDisable == false) && (auction.StartTime < System.DateTime.Now.ToUniversalTime()) && (auction.EndTime > System.DateTime.Now.ToUniversalTime()))
            {
                return stats[2];
            }
            return null;
        }

        public OfferController()
            : this(new InBidEntities())
        {
        }

        public OfferController(InBidEntities entities)
        {
            InBidEntities = entities;
        }


        bool CanBiding(int id)
        {
            var auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).FirstOrDefault();

            if ((auction.EnableDisable) && (auction.StartTime.ToUniversalTime() < System.DateTime.Now.ToUniversalTime()) && (auction.EndTime.ToUniversalTime() > System.DateTime.Now.ToUniversalTime()))
            {
                return true;
            }

            return false;
        }

       


        [HttpPost]
        public ActionResult EnableDisableAuction(int id)
        {
            Auction auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).First();

            if (auction.EnableDisable)
            {
                auction.EnableDisable = false;
                auction.DisableStartTime = DateTime.UtcNow;
            }
            else
            {
                auction.EnableDisable = true;
                TimeSpan suspendTime = DateTime.UtcNow - auction.DisableStartTime.Value;
                //auction.EndTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
                auction.EndTime+=suspendTime;
                
            }
            //auction.EnableDisable = (auction.EnableDisable) ? false : true;
            InBidEntities.SaveChanges();
            return null;
        }


        decimal getCurrentPrice(int auctionNumber)
        {
            IQueryable<Offer> offers = GetOffers(auctionNumber);
            if (offers == null)
                return 0;
            else if (offers.Count() > 0)
            {
                return offers.Skip(0).First().CurrentPrice;
            }
            return 0;
        }

        decimal SetCurrentPriceWithJump(int auctionNumber)
        {
            var auction = InBidEntities.Auctions.Where(x => x.AuctionId == auctionNumber).FirstOrDefault();
            int offersCount = InBidEntities.Offers.Where(x => x.AuctionId == auctionNumber).Count();
            decimal currentPrice = auction.CurrentPrice;
            //decimal startPrice = auction.StartPrice;
            decimal minJump = auction.MinJump;
            bool direction = auction.Direction;
            //decimal newCurrentPrice = 0;
            if (direction)
            {
                if (offersCount == 0)
                    return currentPrice;
                if (currentPrice - minJump > 0)
                {
                    return currentPrice - minJump;
                }
                else
                    return 0;
            }
            else
            {
                if (offersCount == 0)
                    return currentPrice;
                else
                    return currentPrice + minJump;
            }
        }

        // [Authorize]       
        public ActionResult Create(int auctionNumber)
        {
            if (!CanBiding(auctionNumber)) //aukcja nie trwa
                return View("CantBiding");

            Offer offer = null;
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                int freeUsers = PopulateUserWithoutAuctionDropDownList(auctionNumber);

                if (freeUsers == 0)
                    return View("OverLimitTotal");



                offer = new Offer()
                {
                    AuctionId = auctionNumber,
                    StartPrice = SetCurrentPriceWithJump(auctionNumber)

                };

                return View(offer);
            }
            if (!CanAddOffer(auctionNumber))
                return View("OverLimit");

            if (Roles.IsUserInRole(User.Identity.Name, @"Bidder"))
            {
                offer = new Offer()
                {
                    AuctionId = auctionNumber,
                    StartPrice = SetCurrentPriceWithJump(auctionNumber)
                };

                return View(offer);
            }
            return View("AccessDenied");
        }

        //
        // POST: Offer/Create

        [HttpPost]
        public ActionResult Create(Offer offer)
        {
            if (!CanBiding(offer.AuctionId))
                return View("CantBiding");

            if (!CanAddOffer(offer.AuctionId, offer.UserId))
                return View("OverLimit");
            if ((!Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin")) && (!Roles.IsUserInRole(User.Identity.Name, @"Bidder")))
                return View("AccessDenied");

            if (ModelState.IsValid)
            {

                offer.CurrentPrice = offer.StartPrice;
                offer.BDCounter = 0;
                offer.JoinedDate = System.DateTime.UtcNow;
                SetAuctionCurrentPrice(offer.AuctionId, 0, offer.CurrentPrice, false);


                if ((Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin")) && (CanAddOffer(offer.AuctionId, offer.UserId)))
                {

                }
                else
                {
                    MembershipUser mu = Membership.GetUser();
                    Guid currentUserId = (Guid)mu.ProviderUserKey;
                    offer.UserId = currentUserId;
                }
                offer.BDCounter = 0;

                var offerAuction = InBidEntities.Auctions.Where(x => x.AuctionId == offer.AuctionId).FirstOrDefault();
                var timeToEndAuction = offerAuction.EndTime - DateTime.UtcNow;
                if (timeToEndAuction <= deathTimeExtend)
                    offerAuction.EndTime += timeToEndAuction;

                InBidEntities.AddToOffers(offer);
                InBidEntities.SaveChanges();

                return RedirectToAction("Index", new { auctionNumber = offer.AuctionId });
            }
            PopulateUserWithoutAuctionDropDownList(offer.AuctionId, offer.UserId);
            return View(offer);
        }

        void changeDateIn1Auction(Auction a)
        {
            a.StartTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
            a.EndTime += System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
        }
        [HttpGet]
        public ActionResult Index(int auctionNumber)
        {
            //System.DateTime.Now.T
            var offers = GetOffers(auctionNumber);
            var auction = InBidEntities.Auctions.Where(x => x.AuctionId == auctionNumber).SingleOrDefault();

            //var messages = Messages.Informations;
            var m = new Message();
            m.AuctionId = auctionNumber;

            var usersInAuction = (from o in InBidEntities.Offers
                                  join a in InBidEntities.aspnet_Users on o.UserId equals a.UserId
                                  where o.AuctionId == auctionNumber
                                  select new
                                  {
                                      a.UserId,
                                      a.UserName
                                  });

            Guid currentUserId = new Guid();


            MembershipUser mu = Membership.GetUser();
            if (mu != null)
            {
                currentUserId = (Guid)mu.ProviderUserKey;
            }

            List<Message> msg = new List<Message>();

            var messages = ((List<Message>)HttpContext.Application["Messages"]).Where(x => x.AuctionId == auctionNumber);

            foreach (var s in messages)
            {
                foreach (var g in s.Subscribers)
                {
                    if (g == currentUserId)
                    {
                        msg.Add(s);
                        break;
                    }
                }

            }





            int counter = offers.Count();
            auction.OfferCounter = counter;
            changeDateIn1Auction(auction);

            HttpContext.Session["offerList"] = offers.ToList();

            OfferViewModel ovm = new OfferViewModel()
            {
                Offers = offers,
                Auction = auction,
                M = m,

                Subscribers = usersInAuction.Select(x => new Subscriber
                {
                    Id = x.UserId,
                    UserName = x.UserName

                }),

                Messages = msg

            };
            return View(ovm);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult SendMessage(Guid[] Subscribers, Message m)
        {
            if (Request.IsAjaxRequest())
            {
                m.Subscribers = Subscribers;
                var ms = (List<Message>)HttpContext.Application["Messages"];
                m.MessageId = ms.Count;
                ms.Add(m);


            }
            return null;
        }


        public ActionResult Edit(int id)
        {


            Offer offer = InBidEntities.Offers.Where(x => x.OfferId == id).First();

            if (!CanBiding(offer.AuctionId))
                return View("CantBiding");

            if (offer == null)
            {
                return View("NotFound");
            }

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                offer.CurrentPrice = SetCurrentPriceWithJump(offer.AuctionId);

                return View(offer);
            }
            if (!offer.IsHostedBy())
                return View("InvalidOwner");

            offer.CurrentPrice = SetCurrentPriceWithJump(offer.AuctionId);

            return View(offer);

        }

        [HttpPost]
        public ActionResult Edit(int id, Offer offer/* FormCollection collection*/)
        {
            Offer o = InBidEntities.Offers.Where(x => x.OfferId == id).First();

            if (!CanBiding(o.AuctionId))
                return View("CantBiding");

            if (null == offer)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                if ((Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))/* && (CanAddOffer(offer.AuctionId, offer.UserId))*/)
                {
                    try
                    {
                        o.CurrentPrice = offer.CurrentPrice;
                        o.BDCounter += 1;
                        o.EditDate = System.DateTime.UtcNow;
                        SetAuctionCurrentPrice(offer.AuctionId, 0, o.CurrentPrice, false);

                        var offerAuction = InBidEntities.Auctions.Where(x => x.AuctionId == offer.AuctionId).FirstOrDefault();
                        var timeToEndAuction = offerAuction.EndTime - DateTime.UtcNow;
                        if (timeToEndAuction <= deathTimeExtend)
                            offerAuction.EndTime += timeToEndAuction;

                        InBidEntities.SaveChanges();
                        return RedirectToAction("Index", new { auctionNumber = offer.AuctionId });
                    }
                    catch
                    {
                        return View(offer);
                    }


                }
                if (!o.IsHostedBy())  //tu jest błąd
                    return View("InvalidOwner");

                try
                {
                    o.CurrentPrice = offer.CurrentPrice;
                    o.BDCounter += 1;
                    o.EditDate = System.DateTime.UtcNow;
                    SetAuctionCurrentPrice(offer.AuctionId, 0, o.CurrentPrice, false);

                    var offerAuction = InBidEntities.Auctions.Where(x => x.AuctionId == offer.AuctionId).FirstOrDefault();
                    var timeToEndAuction = offerAuction.EndTime - DateTime.UtcNow;
                    if (timeToEndAuction <= deathTimeExtend)
                        offerAuction.EndTime += timeToEndAuction;

                    InBidEntities.SaveChanges();
                    return RedirectToAction("Index", new { auctionNumber = offer.AuctionId });
                }
                catch
                {
                    return View(offer);
                }
            }
            return View(offer);
        }


        [HttpPost]
        public ActionResult Update(/*AuctionStatus auctionStatus,*/string[] ids,int id)
        {

            object _look = new object();
            lock (_look)
            {
                JsonForOffers jfo = new JsonForOffers();

                string json;
                List<Offer> temp = null;
                List<RowUpdat> rUp = new List<RowUpdat>();

                List<Offer> offerList = (List<Offer>)HttpContext.Session["offerList"];

                var offers = GetOffers(id);
                if (offerList != null)
                {
                    if (0 < offerList.Count)
                    {
                        temp = offers.ToList();


                        foreach (Offer o in offerList)
                        {
                            Offer tempOffer = temp.Find(x => x.OfferId == o.OfferId);

                            if (tempOffer == null)
                            {
                                //usuniéto element
                                rUp.Add(new RowUpdat(o.OfferId, -1, null, null, null, false, true, null, null, null, null));

                                continue;
                            }

                            if (o.CurrentPrice != tempOffer.CurrentPrice) //zmiana oferty
                            {
                                int rowId = o.OfferId;
                                int nextRowId = (temp.FindIndex(x => x == tempOffer) == (temp.Count - 1)) ? -1 : temp[temp.FindIndex(x => x == tempOffer) + 1].OfferId;

                                string value = tempOffer.CurrentPrice.ToString("N", CultureInfo.CurrentCulture.NumberFormat);
                                string change = (tempOffer.StartPrice != 0) ? Math.Round(100 * Math.Abs((tempOffer.StartPrice - tempOffer.CurrentPrice)) / tempOffer.StartPrice, 2).ToString("N", CultureInfo.CurrentCulture.NumberFormat) : "0";

                                rUp.Add(new RowUpdat(rowId, nextRowId, value, change, null, false, false, null, null, null, tempOffer.EditDate));
                                continue;
                            }
                        }

                        foreach (Offer o in temp)
                        {
                            if (!offerList.Exists(x => x.OfferId == o.OfferId))
                            {
                                int nextRowId = (temp.FindIndex(x => x == o) == (temp.Count - 1)) ? -1 : temp[temp.FindIndex(x => x == o) + 1].OfferId;

                                string startValue = o.StartPrice.ToString("N", CultureInfo.CurrentCulture.NumberFormat);
                                string value = o.CurrentPrice.ToString("N", CultureInfo.CurrentCulture.NumberFormat);
                                string isOnline = o.IsOnline() ? "online" : "offline";
                                string userName = o.GetUserName();
                                string labelName = Resources.Resources.Name;

                                string change = (o.StartPrice != 0) ? Math.Round(100 * Math.Abs((o.StartPrice - o.CurrentPrice)) / o.StartPrice, 2).ToString("N", CultureInfo.CurrentCulture.NumberFormat) : "0";

                                rUp.Add(new RowUpdat(o.OfferId, nextRowId, value, change, startValue, true, false, isOnline, userName, labelName, o.EditDate));
                            }
                        }
                    }
                }
                HttpContext.Session["offerList"] = temp;

              //  System.DateTime date = new System.DateTime();
             //   rUp.Add(new RowUpdat(1, 2, "45", "45", "12", true, false, "online", "dg", "rth", date));
               // rUp.Add(new RowUpdat(1, 2, "45", "45", "12", true, false, "online", "dg", "rth", date));

                jfo.UpdateRows = rUp;
                jfo.States=UpdateStatus(id);
                jfo.AuctionStatus = SwitchAuction(id/*, auctionStatus*/);
                if (ids!=null)
               jfo.ShortMessages = GetMessages(id, ids);
                var auctionEndTime=InBidEntities.Auctions.Where(x=>x.AuctionId==id).FirstOrDefault().EndTime;
                auctionEndTime+= System.TimeSpan.FromHours(System.Double.Parse(Request.Cookies["TimeZone"].Value));
                jfo.AuctionEndTime = auctionEndTime;

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                json = oSerializer.Serialize(jfo);


                json = JsonFormatter.PrettyPrint(json);


                return Json(json);

                /* return Json("[{\"RowId\":314,\"NextRowId\":-1,\"Value\":null,\"Change\":null,\"Add\":false,\"Remove\":true,\"StartValue\":null,\"IsOnline\":null,\"UserName\":null,\"LabelName\":null,\"EditDate\":null},"+
                     "{\"RowId\":332,\"NextRowId\":-1,\"Value\":null,\"Change\":null,\"Add\":false,\"Remove\":true,\"StartValue\":null,\"IsOnline\":null,\"UserName\":null,\"LabelName\":null,\"EditDate\":null},"+
                     "{\"RowId\":316,\"NextRowId\":-1,\"Value\":null,\"Change\":null,\"Add\":false,\"Remove\":true,\"StartValue\":null,\"IsOnline\":null,\"UserName\":null,\"LabelName\":null,\"EditDate\":null}]");*/
            }

        }

        //[HttpPost]
        public List<Status> UpdateStatus(int id = 0)
        {

            List<Status> stats = new List<Status>();

            var offers = InBidEntities.Offers.Where(x => x.AuctionId == id);

            foreach (Offer o in offers)
            {
                string status = null;
                status = o.IsOnline() ? "online" : "offline";

                stats.Add(new Status(o.OfferId, status));
            }
            return stats;

        }


        public ActionResult GoToDeath(int id)
        {
            AuctionStatusViewModel asvm = new AuctionStatusViewModel();
            asvm = SwitchAuction(id);
            
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = oSerializer.Serialize(asvm);


            json = JsonFormatter.PrettyPrint(json);


            return Json(json);
        }

        //[HttpPost]
        public AuctionStatusViewModel SwitchAuction(int id/*, AuctionStatus auctionStatus*/)
        {
            var auction = InBidEntities.Auctions.Where(x => x.AuctionId == id).First();

            if (auction.StartTime > System.DateTime.Now.ToUniversalTime())
            {
                return new AuctionStatusViewModel { Value = false, Status = stats[0] };
            }
            if (auction.EndTime < System.DateTime.Now.ToUniversalTime())
            {
                return new AuctionStatusViewModel { Value = false, Status = stats[3] };
            }
            if ((auction.StartTime < System.DateTime.Now.ToUniversalTime()) && (auction.EndTime > System.DateTime.Now.ToUniversalTime()))
            {
                if (auction.EnableDisable)
                    return new AuctionStatusViewModel { Value = auction.EnableDisable, Status = stats[1] };
                else
                    return new AuctionStatusViewModel { Value = auction.EnableDisable, Status = stats[2] };

            }
            //if (auctionStatus.CurrentStatus.ToLower() != auction.EnableDisable.ToString().ToLower())
            //{

            //    return Json(new AuctionStatusViewModel {Value=auction.EnableDisable, Status = SetState(stats, id) });
            //}

            return null;
        }


        //[HttpPost]
        public List<ShortMessage> GetMessages(int id, string[] ids)
        {
            var messages = ((List<Message>)HttpContext.Application["Messages"]).Where(x => x.AuctionId == id);
            List<ShortMessage> msg = new List<ShortMessage>();
            //var temp=messages;
            List<int> messagesIds = new List<int>();

            Guid currentUserId = new Guid();

            MembershipUser mu = Membership.GetUser(User.Identity.Name);
            if (mu != null)
            {
                currentUserId = (Guid)mu.ProviderUserKey;
            }

            if ((ids.Length == 1) && (ids[0] == ""))
                ids = new string[] { "-1" };

            else
            {

                //if (ids != null)
                //{
                foreach (var m in messages)
                {
                    messagesIds.Add(m.MessageId);
                    foreach (var i in ids)
                    {
                        if (System.Int32.Parse(i) == m.MessageId)
                        {
                            messagesIds.Remove(m.MessageId);
                            //break;
                        }

                    }
                }
                foreach (var mId in messagesIds)
                {
                    //var m = messages.Where(x => x.MessageId == mId).First();
                    // var m = messages.Where(x => x.MessageId == mId);

                    foreach (Message ms in messages)
                    {
                        foreach (Guid g in ms.Subscribers)
                        {
                            if ((mId == ms.MessageId) && (currentUserId == g))
                                msg.Add(new ShortMessage { Information = ms.Information, MessageId = ms.MessageId });
                        }

                    }



                    //  msg.Add(new ShortMessage { Information = m.Information, MessageId = m.MessageId });
                }
            }

            //System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string json = oSerializer.Serialize(msg);

            return msg;
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Offer offer = InBidEntities.Offers.Where(x => x.OfferId == id).SingleOrDefault();

            if (!CanBiding(offer.AuctionId))
                return View("CantBiding");

            if (offer == null)
                return View("NotFound");

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
                return View(offer);

            if (!offer.IsHostedBy())
                return View("InvalidOwner");

            return View(offer);
        }

        // 
        // HTTP POST: /Dinners/Delete/1
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, string confirmButton)
        {
            Offer offer = InBidEntities.Offers.Where(x => x.OfferId == id).SingleOrDefault();

            if (!CanBiding(offer.AuctionId))
                return View("CantBiding");

            if (offer == null)
                return View("NotFound");

            int auctionNum = offer.AuctionId;

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                SetAuctionCurrentPrice(auctionNum, id, 0, true);
                InBidEntities.Offers.DeleteObject(offer);
                InBidEntities.SaveChanges();
                return RedirectToAction("Index", new { auctionNumber = auctionNum });
            }
            if (!offer.IsHostedBy())
                return View("InvalidOwner");

            SetAuctionCurrentPrice(auctionNum, id, 0, true);
            InBidEntities.Offers.DeleteObject(offer);
            InBidEntities.SaveChanges();

            return RedirectToAction("Index", new { auctionNumber = auctionNum });
        }




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

        //set Current Price for auction
        void SetAuctionCurrentPrice(int auctionNumber, int offerId, decimal currPrice, bool delete)
        {
            bool bidDirection = InBidEntities.Auctions.Where(x => x.AuctionId == auctionNumber).SingleOrDefault().Direction;
            Auction auction = InBidEntities.Auctions.Where(x => x.AuctionId == auctionNumber).SingleOrDefault();

            switch (delete)
            {
                case false:
                    if (bidDirection)
                    {
                        if (auction.CurrentPrice > currPrice)
                            auction.CurrentPrice = currPrice;
                    }
                    else
                    {
                        if (auction.CurrentPrice < currPrice)
                            auction.CurrentPrice = currPrice;
                    }
                    break;
                case true:
                    IQueryable<Offer> offers = null;
                    Offer topOffer = null;
                    //decimal secondCurrentPrice = 0;
                    if (bidDirection)
                    {
                        offers = InBidEntities.Offers.Where(x => x.AuctionId == auctionNumber).OrderBy(x => x.CurrentPrice);
                        topOffer = offers.FirstOrDefault();

                        if (topOffer.OfferId == offerId && (offers.Count() > 1))
                        {
                            //secondCurrentPrice = offers.Skip(1).First().CurrentPrice;  //offers.ElementAt(1).CurrentPrice;
                            auction.CurrentPrice = offers.Skip(1).First().CurrentPrice;
                        }
                        if (topOffer.OfferId == offerId && (offers.Count() == 1))
                            auction.CurrentPrice = auction.StartPrice;
                    }
                    else
                    {
                        offers = InBidEntities.Offers.Where(x => x.AuctionId == auctionNumber).OrderByDescending(x => x.CurrentPrice);
                        topOffer = offers.FirstOrDefault();

                        if (topOffer.OfferId == offerId && (offers.Count() > 1))
                        {
                            //secondCurrentPrice = offers.ElementAt(1).CurrentPrice;
                            auction.CurrentPrice = offers.Skip(1).First().CurrentPrice;
                        }
                        if (topOffer.OfferId == offerId && (offers.Count() == 1))
                            auction.CurrentPrice = auction.StartPrice;
                    }
                    break;
                default: break;
            }
        }

        IQueryable<Offer> GetOffers(int? auctionNumber)
        {
            try
            {
                bool bidDirection = InBidEntities.Auctions.Where(x => x.AuctionId == auctionNumber).SingleOrDefault().Direction;
                IQueryable<Offer> offers = null;

                if (bidDirection)
                    return offers = InBidEntities.Offers.Where(x => x.AuctionId == auctionNumber).OrderBy(x => x.CurrentPrice);
                else
                    return offers = InBidEntities.Offers.Where(x => x.AuctionId == auctionNumber).OrderByDescending(x => x.CurrentPrice);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        bool CanAddOffer(int auctionNumber, object userId = null) //sprawdzanie czy biderzy tej firmy są w aukcji
        {



            var companyOffersInAuction = InBidEntities.vw_CompaniesInAuction.Where(x => x.AuctionId == auctionNumber);



            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin") && (userId != null))
            {
                var bidderCompanyId = InBidEntities.aspnet_Users.Where(x => x.UserId == (Guid)userId).FirstOrDefault().CompanyId;
                foreach (vw_CompaniesInAuction c in companyOffersInAuction)
                {
                    if (c.CompanyId == bidderCompanyId)
                        return false;
                }
                return true;
            }
            else if (Roles.IsUserInRole(User.Identity.Name, @"Bidder"))
            {
                MembershipUser mu = Membership.GetUser();
                Guid currentUserId = (Guid)mu.ProviderUserKey;

                var bidderCompanyId = InBidEntities.aspnet_Users.Where(x => x.UserId == currentUserId).FirstOrDefault().CompanyId;
                foreach (vw_CompaniesInAuction c in companyOffersInAuction)
                {
                    if (c.CompanyId == bidderCompanyId)
                        return false;
                }
            }
            return true;
        }

        private int PopulateUserWithoutAuctionDropDownList(int auctionNumber, object selectedUser = null)
        {
            var companiesInAuction = InBidEntities.vw_CompaniesInAuction.Where(x => x.AuctionId == auctionNumber);

            string[] allBidders = Roles.GetUsersInRole(@"Bidder");

            List<User> freeUsers = new List<User>();

            bool insert = false;

            foreach (string ab in allBidders)
            {
                insert = true;

                var userCompanyId = InBidEntities.aspnet_Users.Where(x => x.UserName == ab).FirstOrDefault().CompanyId;

                foreach (vw_CompaniesInAuction c in companiesInAuction)
                {
                    if (userCompanyId == c.CompanyId)
                        insert = false;
                }


                if (insert)
                {
                    Guid currentUserId = new Guid();

                    MembershipUser mu = Membership.GetUser(ab);
                    if (mu != null)
                    {
                        currentUserId = (Guid)mu.ProviderUserKey;
                    }
                    freeUsers.Add(new User(currentUserId, mu.UserName));
                }
            }
            Users us = new Users(freeUsers);
            ViewBag.UserId = new SelectList(us, "UserId", "UserName", selectedUser);
            return freeUsers.Count;
        }

        //private int PopulateUserWithoutAuctionDropDownList(int auctionNumber, object selectedUser = null)
        //{
        //    List<User> freeUsers = new List<User>();
        //    List<aspnet_Users> users=new List<aspnet_Users>();
        //    List<Offer> usersInAuction = new List<Offer>();

        //    users = InBidEntities.aspnet_Users.Select(x => x).ToList();

        //    usersInAuction = InBidEntities.Offers.Where(x => x.AuctionId == auctionNumber).ToList();            

        //    bool insert = false;

        //    foreach (aspnet_Users u in users)
        //    {
        //        insert = true;
        //        foreach (Offer uis in usersInAuction)
        //        {
        //            if (u.UserId == uis.UserId)
        //                insert = false;
        //        }
        //        if (insert)
        //            freeUsers.Add(new User(u.UserId, u.UserName));
        //    }

        //    Users us = new Users(freeUsers);

        //    ViewBag.UserId = new SelectList(us, "UserId", "UserName", selectedUser);

        //    return freeUsers.Count;
        //}


        //List<Subscriber> getUsersInAuction(int auctionNumber)
        //{
        //    List<Subscriber> users = new List<Subscriber>();
        //    //Users us = null;
        //    var usersInAuction = (from o in InBidEntities.Offers
        //                         join a in InBidEntities.aspnet_Users on o.UserId equals a.UserId
        //                         where o.AuctionId == auctionNumber
        //                         select new
        //                         {
        //                             a.UserId,
        //                             a.UserName                                      
        //                         });
        //    //return usersInAuction;
        //    foreach (var u in usersInAuction)
        //    {
        //        users.Add(new Subscriber {u.UserId, /*u.UserName*/true} /*, Checkbox = true */);

        //    }

        //    //us = new Users(users);
        //   return users;

        //  //  ViewBag.Subscribers=new SelectList(users,
        //}


    }
}


    