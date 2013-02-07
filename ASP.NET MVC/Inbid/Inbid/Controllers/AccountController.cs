using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Inbid.Models;
using Inbid.Infrastructure;
using Inbid.Helpers;
using System.Data;
using PagedList;
using Inbid.ViewModels;

namespace Inbid.Controllers
{
    public class AccountController : BaseController
    {
        //private const int PageSize = 10;

        private const string urlApp="http://inbid.apphb.com";

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }


        private InBidEntities inBidDB = new InBidEntities();
        //
        // GET: /Account/LogOn




        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    setLanguage(model.UserName);


                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void setLanguage(string userName)
        {
            int? languageId = inBidDB.aspnet_Users.Where(x => x.UserName == userName).First().LanguageId;
            string cultureName = inBidDB.Languages.Where(x => x.LanguageId == languageId).First().CultureName;

            //Session["Culture"] = "fggh"; 
            HttpCookie userCookie = Request.Cookies["Culture"];
            userCookie.Value = cultureName;
            userCookie.Expires = DateTime.Now.AddYears(100);
            Response.SetCookie(userCookie);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            //FormsAuthentication.SignOut();
            FormsAuth.SignOut();



            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Company(string companyAdmin = null)
        {
            if (Request.IsAuthenticated && Roles.IsUserInRole(@"SuperAdmin"))
            {
                if (!String.IsNullOrWhiteSpace(companyAdmin))
                {
                    var adminData = inBidDB.aspnet_Users.Where(x => x.UserName == companyAdmin).SingleOrDefault();
                    var employees = inBidDB.vw_CompanyMembers.Where(x => (x.CompanyId == adminData.CompanyId && (x.RoleName == "Bidder" || x.RoleName == "BidderView")));
                    var auctions = inBidDB.Auctions.Where(x => x.UserId == adminData.UserId).OrderByDescending(x=>x.StartTime);

                    var companyDetails = inBidDB.vw_CompanyDetails.Where(x => x.UserId == adminData.UserId).SingleOrDefault();
                    var companyBidders = inBidDB.vw_CompanyBidders.Where(x=>x.CompanyId==adminData.CompanyId).Count();
                    var companyBidderViews = inBidDB.vw_CompanyBidderViews.Where(x => x.CompanyId == adminData.CompanyId).Count();      
                                        
                    var cd = new CompanyDetails
                    {
                        UserName=companyDetails.UserName,
                         FirstName=companyDetails.FirstName,
                          LastName=companyDetails.LastName,
                           Email=companyDetails.Email,
                            Phone=companyDetails.Phone,
                           Name=companyDetails.Name,
                            Adress=companyDetails.Adress,
                             City=companyDetails.City,
                              PostalCode=companyDetails.PostalCode,
                               Region=companyDetails.Region,
                                  CountryName=companyDetails.CountryName,
                                  AuctionQuality=(byte)companyDetails.AuctionQuality,
                                  BidderQuality=(byte)companyDetails.BidderQuality,
                                  BidderViewQuality=(byte)companyDetails.BidderViewQuality,
                                  DateEditAuctionQuality=companyDetails.DateEditAuctionQuality,
                                   CurrentBidderQuality=(byte)companyBidders,
                                    CurrentBidderViewQuality=(byte)companyBidderViews,
                                     CurrentAuctionQuality=(byte)auctions.Where(x=>x.DateAdd>companyDetails.DateEditAuctionQuality).Count()
                    };


                    CompanyViewModel cvm = new CompanyViewModel
                    {
                        Members = employees,
                        Auctions=auctions,
                        CompanyDet=cd
                    };

                    return View(cvm);
                    //return View(employees);
                }
            }

            if (Request.IsAuthenticated && Roles.IsUserInRole(@"Admin"))
            {
                var adminData = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault();
                var employees = inBidDB.vw_CompanyMembers.Where(x => (x.CompanyId == adminData.CompanyId && (x.RoleName == "Bidder" || x.RoleName == "BidderView")));
                var auctions = inBidDB.Auctions.Where(x => x.UserId == adminData.UserId);

                var companyDetails = inBidDB.vw_CompanyDetails.Where(x => x.UserId == adminData.UserId).SingleOrDefault();
                var companyBidders = inBidDB.vw_CompanyBidders.Where(x => x.CompanyId == adminData.CompanyId).Count();
                var companyBidderViews = inBidDB.vw_CompanyBidderViews.Where(x => x.CompanyId == adminData.CompanyId).Count();
                
                var cd = new CompanyDetails
                {
                    UserName = companyDetails.UserName,
                    FirstName = companyDetails.FirstName,
                    LastName = companyDetails.LastName,
                    Email = companyDetails.Email,
                    Phone = companyDetails.Phone,
                    Name = companyDetails.Name,
                    Adress = companyDetails.Adress,
                    City = companyDetails.City,
                    PostalCode = companyDetails.PostalCode,
                    Region = companyDetails.Region,
                    CountryName = companyDetails.CountryName,
                    AuctionQuality = (byte)companyDetails.AuctionQuality,
                    BidderQuality = (byte)companyDetails.BidderQuality,
                    BidderViewQuality = (byte)companyDetails.BidderViewQuality,
                    DateEditAuctionQuality = companyDetails.DateEditAuctionQuality,
                    CurrentBidderQuality = (byte)companyBidders,
                    CurrentBidderViewQuality = (byte)companyBidderViews,
                    CurrentAuctionQuality = (byte)auctions.Where(x => x.DateAdd > companyDetails.DateEditAuctionQuality).Count()
                };


                CompanyViewModel cvm = new CompanyViewModel
                {
                    Members = employees,
                    Auctions = auctions,
                    CompanyDet = cd
                };

                return View(cvm);
                //return View(employees);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AccessDenied");

            }
            //return PartialView();
            return View("AccessDenied");
        }

        public byte CheckOverBiddersAndBiddersViews(byte what)
        {
            switch (what)
            {
                case 1:                                            
                        if (Roles.IsUserInRole(User.Identity.Name, @"Admin"))
                        {
                            int? companyId = 0;
                            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
                            {
                                string userName = Request.QueryString["companyAdmin"];
                                companyId = inBidDB.aspnet_Users.Where(x => x.UserName == userName).FirstOrDefault().CompanyId;
                            }
                            else
                            {
                                companyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().CompanyId;
                            }
                            var company = inBidDB.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();
                            var companyBidders = inBidDB.vw_CompanyBidders.Where(x => x.CompanyId == companyId);
                            var companyBidderViews = inBidDB.vw_CompanyBidderViews.Where(x => x.CompanyId == companyId);

                            if ((companyBidders.Count() >= company.BidderQuality) && (companyBidderViews.Count() >= company.BidderViewQuality))
                                return 1;
                        }
                        return 10;                        
            
                case 2:
                        if (Roles.IsUserInRole(User.Identity.Name, @"Admin"))
                        {
                            int? companyId = 0;
                            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
                            {
                                string userName = Request.QueryString["companyAdmin"];
                                companyId = inBidDB.aspnet_Users.Where(x => x.UserName == userName).FirstOrDefault().CompanyId;
                            }
                            else
                            {
                                companyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().CompanyId;
                            }
                            var company = inBidDB.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();
                            var companyBidders = inBidDB.vw_CompanyBidders.Where(x => x.CompanyId == companyId);
                            //var companyBidderViews = inBidDB.vw_CompanyBidderViews.Where(x => x.CompanyId == companyId);

                            if (companyBidders.Count() >= company.BidderQuality)// && companyBidderViews.Count() == company.BidderViewQuality)
                                return 2;
                        }
                        return 10;                        
                case 3:
                        if (Roles.IsUserInRole(User.Identity.Name, @"Admin"))
                        {
                            int? companyId = 0;
                            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
                            {
                                string userName = Request.QueryString["companyAdmin"];
                                companyId = inBidDB.aspnet_Users.Where(x => x.UserName == userName).FirstOrDefault().CompanyId;
                            }
                            else
                            {
                                companyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().CompanyId;
                            }
                            var company = inBidDB.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();
                            //var companyBidders = inBidDB.vw_CompanyBidders.Where(x => x.CompanyId == companyId);
                            var companyBidderViews = inBidDB.vw_CompanyBidderViews.Where(x => x.CompanyId == companyId);

                            if ( companyBidderViews.Count() >= company.BidderViewQuality)
                                return 3;
                        }
                        return 10;                        
                default:
                        return 10;                     
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult RegisterBidderOrBidderView()
        {
            //if (Roles.IsUserInRole(User.Identity.Name, @"SuperADminAdmin"))

            if (Roles.IsUserInRole(User.Identity.Name, @"Admin"))
            {               
                if (CheckOverBiddersAndBiddersViews(1)==1)
                    return View("MembersOver");

                                PopulateLanguagesDropDownList();

                return View();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AccessDenied");

            }
            //return PartialView();
            return View("AccessDenied");
        }

        [Authorize]
        [HttpPost]
        public ActionResult RegisterBidderOrBidderView(RegistrationBidderOrBidderViewModel model)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"Admin"))
            {
                if (model.Bidder)
                {
                    if (CheckOverBiddersAndBiddersViews(2)==2)
                        return View("BiddersOver");
                }
                else
                {
                    if (CheckOverBiddersAndBiddersViews(3) == 3)
                        return View("BidderViewsOver");
                }

                try
                {
                    if (ModelState.IsValid)
                    {
                        // Attempt to register the user
                        MembershipCreateStatus createStatus;


                        Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);

                        int? companyId = 0;
                        Company company = null;

                        //var companyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                        if (Request.QueryString["companyAdmin"] == null)
                        {
                            companyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().CompanyId;
                            company = inBidDB.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();
                        }
                        else
                        {
                            var adminName = Request.QueryString["companyAdmin"];
                            companyId = inBidDB.aspnet_Users.Where(x => x.UserName == adminName).SingleOrDefault().CompanyId;
                            company = inBidDB.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();
                        }

                        var InsertedRow = inBidDB.aspnet_Users.Where(x => x.UserName == model.UserName).First();
                        InsertedRow.FirstName = model.FirstName;
                        InsertedRow.LastName = model.LastName;
                        InsertedRow.LanguageId = model.LanguageId;
                        InsertedRow.CompanyId = companyId;

                        InsertedRow.Phone = model.Phone;

                        if (model.Bidder)
                            Roles.AddUserToRole(model.UserName, @"Bidder");
                        else
                            Roles.AddUserToRole(model.UserName, @"BidderView");

                        inBidDB.SaveChanges();


                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            setLanguage(model.UserName);
                            //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                            // FormsAuth.SignIn(model.UserName, false /* createPersistentCookie */);
                            //Roles.AddUserToRole(model.UserName, "Admin");

                            string path = urlApp + "/Account/ActiveUser?userId=" + InsertedRow.UserId;
                          //  string path = Url.Action("ActiveUser", "Account", new { userId = InsertedRow.UserId }, Request.Url.Scheme);
                            MailHelper.SendRegistrationBidderOrBidderViewMail(model.Bidder, model.Email, model.UserName, model.Password, InsertedRow.UserId, company.Name, path);


                            //if (Request.IsAjaxRequest())
                            //{
                            //    // Same idea as above
                            //    return PartialView("_AdminRegistrationCompleted", model);
                            //}

                            TempData["Message"] = string.Format(model.UserName + " został zarejestrowany w firmie " + company.Name);

                            //return RedirectToAction("Company");
                            if (Request.QueryString["companyAdmin"] == null)
                            {
                                return RedirectToAction("Company");
                            }
                            else
                            {
                                var adminName = Request.QueryString["companyAdmin"];
                                return RedirectToAction("Company", new { companyAdmin = adminName });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", ErrorCodeToString(createStatus));
                        }
                    }
                }
                catch (DataException e)
                {
                    ModelState.AddModelError("", "");
                }

                PopulateLanguagesDropDownList(model.LanguageId);
                //  PopulateCountiresDropDownLists(model.CountryId);
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AccessDenied");

            }
            //return PartialView();
            return View("AccessDenied");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditBidderOrBidderView(string id)
        {
            var adminCompanyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().CompanyId;
            var member = inBidDB.aspnet_Users.Where(x => x.UserName == id).FirstOrDefault();
            

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                var membershipData = inBidDB.aspnet_Membership.Where(x => x.UserId == member.UserId).FirstOrDefault();

                PopulateLanguagesDropDownList(member.LanguageId);

                bool role = false;
                role = (Roles.IsUserInRole(id, @"Bidder")) ? true : false;

                RegistrationBidderOrBidderViewModel rbv = new RegistrationBidderOrBidderViewModel
                {
                    Bidder = role,
                    Email = membershipData.Email,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    UserName = member.UserName,
                    Phone = member.Phone,
                        LanguageId=member.LanguageId,
                    Password = membershipData.Password,
                    ConfirmPassword = membershipData.Password
                };


                return View(rbv);
            }

            if (((Roles.IsUserInRole(User.Identity.Name, @"Admin")) && (adminCompanyId == member.CompanyId)) )
            {
                var membershipData = inBidDB.aspnet_Membership.Where(x => x.UserId == member.UserId).FirstOrDefault();

                PopulateLanguagesDropDownList(member.LanguageId);

                bool role = false;
                role = (Roles.IsUserInRole(id, @"Bidder")) ? true : false;                

                RegistrationBidderOrBidderViewModel rbv = new RegistrationBidderOrBidderViewModel
                {
                    Bidder=role,
                    Email = membershipData.Email,
                       FirstName=member.FirstName,
                       LastName=member.LastName,
                        UserName=member.UserName,
                         Phone=member.Phone,
                    LanguageId = member.LanguageId,
                    Password = membershipData.Password,
                 ConfirmPassword=membershipData.Password
                };
                return View(rbv);
            }
            return View("AccessDenied");
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditBidderOrBidderView(string id, RegistrationBidderOrBidderViewModel rm)
        {
            var adminCompanyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().CompanyId;
            var member = inBidDB.aspnet_Users.Where(x => x.UserName == id).FirstOrDefault();

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (Roles.IsUserInRole(member.UserName, @"Bidder") && !rm.Bidder)
                        {
                            Roles.RemoveUserFromRole(member.UserName, @"Bidder");
                            Roles.AddUserToRole(member.UserName, @"BidderView");
                        }
                        else if (Roles.IsUserInRole(member.UserName, @"BidderView") && rm.Bidder)
                        {
                            Roles.RemoveUserFromRole(member.UserName, @"BidderView");
                            Roles.AddUserToRole(member.UserName, @"Bidder");
                        }
                        member.UserName = rm.UserName;
                        member.FirstName = rm.FirstName;
                        member.LastName = rm.LastName;
                        member.Phone = rm.Phone;
                        member.LanguageId = rm.LanguageId;
                        setLanguage(rm.UserName); //zmiana jezyka
                        var membershipData = inBidDB.aspnet_Membership.Where(x => x.UserId == member.UserId).FirstOrDefault();
                        membershipData.Email = rm.Email;

                        inBidDB.SaveChanges();

                        TempData["Message"] = string.Format("Dane "+rm.UserName + " zostały zmienione");                       
                        //var adminName = Request.QueryString["companyAdmin"];
                        return RedirectToAction("Company", new { companyAdmin = id });                        
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Dane nie zostaly zapisane. Problem z bazá danych");
                    }
                }
                PopulateLanguagesDropDownList(rm.LanguageId);
                return View(rm);
            }

            if (((Roles.IsUserInRole(User.Identity.Name, @"Admin")) && (adminCompanyId == member.CompanyId)) )
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (Roles.IsUserInRole(member.UserName, @"Bidder") && !rm.Bidder)
                        {
                            Roles.RemoveUserFromRole(member.UserName, @"Bidder");
                            Roles.AddUserToRole(member.UserName, @"BidderView");
                        }
                        else if (Roles.IsUserInRole(member.UserName, @"BidderView") && rm.Bidder)
                        {
                            Roles.RemoveUserFromRole(member.UserName, @"BidderView");
                            Roles.AddUserToRole(member.UserName, @"Bidder");
                        }
                        member.UserName = rm.UserName;
                        member.FirstName = rm.FirstName;
                        member.LastName = rm.LastName;
                        member.Phone = rm.Phone;
                        member.LanguageId = rm.LanguageId;
                        setLanguage(rm.UserName); //zmiana jezyka
                        var membershipData = inBidDB.aspnet_Membership.Where(x => x.UserId == member.UserId).FirstOrDefault();
                        membershipData.Email = rm.Email;

                        inBidDB.SaveChanges();

                        TempData["Message"] = string.Format("Dane " + rm.UserName + " zostały zmienione");
                        return RedirectToAction("Company");                   
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Dane nie zostaly zapisane. Problem z bazá danych");
                    }
                }
                PopulateLanguagesDropDownList(rm.LanguageId);
                return View(rm);
            }
            return View("AccessDenied");
        }


        [Authorize]
        [HttpGet]
        public ActionResult DeleteBidderOrBidderView(string id)
        {
            var adminCompanyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().CompanyId;
            var member = inBidDB.aspnet_Users.Where(x => x.UserName == id).FirstOrDefault();

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                return View("DeleteBidderOrBidderView",member);
            }                       

            if (((Roles.IsUserInRole(User.Identity.Name, @"Admin")) && (adminCompanyId == member.CompanyId)))
            {

                return View("DeleteBidderOrBidderView",member);
            }

            return View("AccessDenied");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteBidderOrBidderView(string confirmButton, string id)
        {
            var adminCompanyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().CompanyId;
            var member = inBidDB.aspnet_Users.Where(x => x.UserName == id).FirstOrDefault();

            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                //Membership.DeleteUser(member.UserName);
                if (Roles.IsUserInRole(member.UserName,@"Bidder"))
                {
                  
                    var offers=inBidDB.Offers.Where(x=>x.UserId==member.UserId);
                    foreach (Offer o in offers)
                        inBidDB.DeleteObject(o);                   
                }
                inBidDB.SaveChanges();
                Membership.DeleteUser(member.UserName);
                 inBidDB.SaveChanges();

                string admin=null;
                 var companyMembers = inBidDB.aspnet_Users.Where(x => x.CompanyId == member.CompanyId);

                 foreach (aspnet_Users a in companyMembers)
                 {
                     if (Roles.IsUserInRole(a.UserName,@"Admin"))
                     {
                         admin=a.UserName;
                         break;
                     }
                 }
                TempData["Message"] = string.Format(member.UserName + " został usuniety");                
                return RedirectToAction("Company", new { companyAdmin = admin });                        
            }

            if (((Roles.IsUserInRole(User.Identity.Name, @"Admin")) && (adminCompanyId == member.CompanyId)))
            {
                //Membership.DeleteUser(member.UserName);
                if (Roles.IsUserInRole(member.UserName, @"Bidder"))
                {
                    Membership.DeleteUser(member.UserName);
                    var offers = inBidDB.Offers.Where(x => x.UserId == member.UserId);
                    foreach (Offer o in offers)
                        inBidDB.DeleteObject(o);
                }
                inBidDB.SaveChanges();
                Membership.DeleteUser(member.UserName);
                inBidDB.SaveChanges();

                TempData["Message"] = string.Format( member.UserName + " został usuniety");
                return RedirectToAction("Company");                   
            }

            return View("AccessDenied");
        }

        //
        // GET: /Account/Register
        [Authorize]
        [HttpGet]
        public ActionResult RegisterAdmin()
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                PopulateLanguagesDropDownList();
                PopulateCountiresDropDownLists();


                RegistrationModel rm = new RegistrationModel
                {
                    AuctionQuality = 10,
                    BidderQuality = 1,
                    BidderViewQuality = 1


                };

                //if (Request.IsAjaxRequest())
                //{
                //    return PartialView("_RegisterAdmin");

                //}

                return View(rm);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AccessDenied");

            }
            //return PartialView();
            return View("AccessDenied");
        }

        //
        // POST: /Account/Register


       


        [Authorize]
        [HttpPost]
        public ActionResult RegisterAdmin(RegistrationModel model)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Attempt to register the user
                        MembershipCreateStatus createStatus;


                        Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);




                        int adresId = 0;
                        int companyId = 0;
                        object _lock = 0;

                        lock (_lock)
                        {
                            Adress a = new Adress
                            {
                                Adress1 = model.Adress,
                                City = model.City,
                                PostalCode = model.PostalCode,
                                Region = model.Region,
                                CountryId = model.CountryId
                            };

                            inBidDB.AddToAdresses(a);
                            inBidDB.SaveChanges();

                            adresId = (from adr in inBidDB.Adresses select adr).OrderByDescending(x => x.AdressId).First().AdressId;


                            Company c = new Company
                            {
                                Name = model.Name,
                                AdressId = adresId,
                                 BidderQuality=model.BidderQuality,
                                  BidderViewQuality=model.BidderViewQuality,                                   
                                     AuctionQuality=model.AuctionQuality,
                                      DateEditAuctionQuality=DateTime.UtcNow
                                   //   DateEndAuctionQuality=DateTime.UtcNow+TimeSpan.FromDays(30)
                                
                            };

                            inBidDB.AddToCompanies(c);
                            inBidDB.SaveChanges();
                            //to trzeba przerobić
                            companyId = (from adr in inBidDB.Companies select adr).OrderByDescending(x => x.CompanyId).First().CompanyId;
                         //   model.CompanyId = companyId;
                        }

                        // var adresId = (from adr in inBidDB.Adresses select adr).OrderByDescending(x => x.AdressId).First().AdressId;

                        //InsertedRow.AdressId = adresId;
                        var InsertedRow = inBidDB.aspnet_Users.Where(x => x.UserName == model.UserName).First();
                        InsertedRow.FirstName = model.FirstName;
                        InsertedRow.LastName = model.LastName;
                        InsertedRow.LanguageId = model.LanguageId;
                        InsertedRow.CompanyId = companyId;

                       // InsertedRow.CompanyId = companyId;

                        InsertedRow.Phone = model.Phone;


                        inBidDB.SaveChanges();


                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            setLanguage(model.UserName);
                            //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                            // FormsAuth.SignIn(model.UserName, false /* createPersistentCookie */);
                            Roles.AddUserToRole(model.UserName, "Admin");

                            string path = urlApp + "/Account/ActiveUser?userId=" + InsertedRow.UserId;
                            //string path = Url.Action("ActiveCompanyAdmin", "Account", new { userId = InsertedRow.UserId }, Request.Url.Scheme);
                            MailHelper.SendRegistrationAdminMail(model.Email, model.UserName, model.Password, InsertedRow.UserId, model.Name, path);


                            //if (Request.IsAjaxRequest())
                            //{
                            //    // Same idea as above
                            //    return PartialView("_AdminRegistrationCompleted", model);
                            //}

                            TempData["Message"] = string.Format("Administrator " + model.UserName + " firmy " + model.Name + " zostal zarejestrowany.");
                            return RedirectToAction("CompanyAdmins");
                        }
                        else
                        {
                            ModelState.AddModelError("", ErrorCodeToString(createStatus));
                        }
                    }
                }
                catch (DataException e)
                {
                    ModelState.AddModelError("", "");
                }

                PopulateLanguagesDropDownList(model.LanguageId);
                PopulateCountiresDropDownLists(model.CountryId);
                // If we got this far, something failed, redisplay form
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AccessDenied");

            }
            //return PartialView();
            return View("AccessDenied");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditCompanyAdmin(string id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                var selectedAdmin = inBidDB.aspnet_Users.Where(x => x.UserName == id).First();
                var company=inBidDB.Companies.Where(x=>x.CompanyId==selectedAdmin.CompanyId).First();
                var adress=inBidDB.Adresses.Where(x=>x.AdressId==company.AdressId).First();
                var membershipData=inBidDB.aspnet_Membership.Where(x=>x.UserId==selectedAdmin.UserId).First();


                PopulateLanguagesDropDownList(selectedAdmin.LanguageId);
                PopulateCountiresDropDownLists(adress.CountryId);
                

                RegistrationModel rm = new RegistrationModel
                {
                    
                UserName=selectedAdmin.UserName,
                FirstName=selectedAdmin.FirstName,
                LastName=selectedAdmin.LastName,
                 Phone=selectedAdmin.Phone,
               //  CompanyId=selectedAdmin.CompanyId,
                 LanguageId=selectedAdmin.LanguageId,
                 Email=membershipData.Email,
                 Password=membershipData.Password,
                 ConfirmPassword=membershipData.Password,
                 Name=company.Name,
                 Adress=adress.Adress1,
                 City=adress.City,
                 Region=adress.Region,
                 PostalCode=adress.PostalCode,
                  CountryId=adress.AdressId,
                   AuctionQuality=(byte)company.AuctionQuality,
                   BidderQuality=(byte)company.BidderQuality,
                   BidderViewQuality=(byte)company.BidderViewQuality       



                };

                return View(rm);
            }
            return View("AccessDenied");
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditCompanyAdmin(string id,RegistrationModel rm)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var selectedAdmin = inBidDB.aspnet_Users.Where(x => x.UserName == id).First();
                        var company = inBidDB.Companies.Where(x => x.CompanyId == selectedAdmin.CompanyId).First();
                        var adress = inBidDB.Adresses.Where(x => x.AdressId == company.AdressId).First();
                        var membershipData = inBidDB.aspnet_Membership.Where(x => x.UserId == selectedAdmin.UserId).First();

                        selectedAdmin.UserName = rm.UserName;
                        selectedAdmin.FirstName = rm.FirstName;
                        selectedAdmin.LastName = rm.LastName;
                        selectedAdmin.Phone = rm.Phone;
                        selectedAdmin.LanguageId = rm.LanguageId;

                        membershipData.Email = rm.Email;

                        company.Name = rm.Name;
                        company.BidderQuality = rm.BidderQuality;
                        company.BidderViewQuality = rm.BidderViewQuality;
                        company.AuctionQuality = rm.AuctionQuality;
                        company.DateEditAuctionQuality = DateTime.UtcNow;


                        adress.Adress1 = rm.Adress;
                        adress.City = rm.City;
                        adress.Region = rm.Region;
                        adress.PostalCode = rm.PostalCode;
                        adress.CountryId = rm.CountryId;

                        inBidDB.SaveChanges();


                        TempData["Message"] = string.Format("Dane firmy {0} zostały zmienione", company.Name);
                        return RedirectToAction("CompanyAdmins", "Account");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "");
                        PopulateLanguagesDropDownList(rm.LanguageId);
                        PopulateCountiresDropDownLists(rm.CountryId);
                        return View(rm);
                    }
                }
             
            }
            
                return View("AccessDenied");
        }

        [Authorize]
        public ActionResult CompanyAdmins(int? page)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {

                var admins = inBidDB.vw_CompanyAdmins.OrderBy(x => x.UserName);

                

                return View(admins);

                //                return View(admins);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AccessDenied");

            }
            //return PartialView();
            return View("AccessDenied");
        }

       


        [Authorize, HttpGet]
        public ActionResult Delete(string id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {            
                var company =(from c in inBidDB.Companies 
                         join a in inBidDB.aspnet_Users on c.CompanyId equals a.CompanyId
                         where a.UserName==id
                         select c).First();
                
                return View(company);
            }
            return View("AccessDenied");
        }

        [Authorize, HttpPost]
        public ActionResult Delete(string confirmButton, string id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                var company = (from c in inBidDB.Companies
                              join a in inBidDB.aspnet_Users on c.CompanyId equals a.CompanyId
                              where a.UserName == id
                              select c).First();
                var companyMemebers = inBidDB.aspnet_Users.Where(x => x.CompanyId == company.CompanyId);            
                


                foreach (aspnet_Users a in companyMemebers)
                {    

                    if (Roles.IsUserInRole(a.UserName, @"Bidder"))
                    {
                    var offers = inBidDB.Offers.Where(x => x.UserId == a.UserId);
                    foreach (Offer o in offers)
                        inBidDB.DeleteObject(o);
                    }

                    if (Roles.IsUserInRole(a.UserName,@"Admin"))
                    {
                        var auctions = inBidDB.Auctions.Where(x => x.UserId == a.UserId);
                        foreach (Auction auc in auctions)
                        {
                            var offersInAuction = inBidDB.Offers.Where(x => x.AuctionId == auc.AuctionId);
                            foreach (Offer or in offersInAuction)
                                inBidDB.DeleteObject(or);
                            inBidDB.DeleteObject(auc);
                        }
                    }
                    Membership.DeleteUser(a.UserName, true);
                }


                inBidDB.DeleteObject(company);

                var adress = inBidDB.Adresses.Where(x => x.AdressId == company.AdressId).FirstOrDefault();
                if (adress != null)
                    inBidDB.DeleteObject(adress);

                inBidDB.SaveChanges();               

                TempData["Message"] = string.Format("Firma {0} została usunięta.",company.Name);
                return RedirectToAction("CompanyAdmins", "Account");
            }
            return View("AccessDenied");
        }




        public ActionResult ActiveUser(string userId)
        {

            if (String.IsNullOrEmpty(Request.Params["userId"]))
            {
                return View("Error");

            }
            else
            {

                try
                {
                    Guid id = new Guid(Request.Params["userId"]);
                    MembershipUser user = Membership.GetUser(id);

                    user.IsApproved = true;

                    Membership.UpdateUser(user);
                    FormsAuth.SignIn(user.UserName, false /* createPersistentCookie */);

                    TempData["Message"] = string.Format("Twoje konto zostało aktywowane.");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception exp)
                {

                    return View("Error");
                }
            }
        }

        private void SetOffersPosition(IEnumerable<vw_BidderOffers> offers,aspnet_Users bidder)
        {
            foreach (vw_BidderOffers o in offers)
            {
                bool bidDirection = inBidDB.Auctions.Where(x => x.AuctionId == o.AuctionId).FirstOrDefault().Direction;
                //bool bidDirection =               

                if (bidDirection)
                {
                    var offersInAuction = inBidDB.Offers.Where(x => x.AuctionId == o.AuctionId).OrderBy(x => x.CurrentPrice);
                    int position = 0;
                    foreach (Offer of in offersInAuction)
                    {
                        position += 1;
                        if (of.UserId == bidder.UserId)
                        {
                            o.StartPrice = position;
                            break;
                        }
                    }
                }
                else
                {
                    var offersInAuction = inBidDB.Offers.Where(x => x.AuctionId == o.AuctionId).OrderByDescending(x => x.CurrentPrice);
                    int position = 0;
                    foreach (Offer of in offersInAuction)
                    {
                        position += 1;
                        if (of.UserId == bidder.UserId)
                        {
                            o.StartPrice = position;
                            break;
                        }
                    }
                }

            }
        }

        [Authorize]
        //[HttpPost]
        public ActionResult Bidder(string id=null)
        {
            if (Roles.IsUserInRole(User.Identity.Name, @"Bidder"))
            {
                var bidder = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                var offers = inBidDB.vw_BidderOffers.Where(x => x.UserId == bidder.UserId).ToList();

                SetOffersPosition(offers, bidder);

                BidderViewModel bvm = new BidderViewModel
                {
                    BidderData = bidder,
                    Offers=offers
                };

                return View(bvm);
            }
            if (Roles.IsUserInRole(User.Identity.Name, @"SuperAdmin"))
            {
                var bidder = inBidDB.aspnet_Users.Where(x => x.UserName == id).FirstOrDefault();
                var offers = inBidDB.vw_BidderOffers.Where(x => x.UserId == bidder.UserId).ToList();

                SetOffersPosition(offers, bidder);

                BidderViewModel bvm = new BidderViewModel
                {
                    BidderData = bidder,
                    Offers = offers
                };

                return View(bvm);

            }
            if (Roles.IsUserInRole(User.Identity.Name, @"Admin"))
            {
                var bidder = inBidDB.aspnet_Users.Where(x => x.UserName == id).FirstOrDefault();
                var adminCompanyId = inBidDB.aspnet_Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().CompanyId;

                if (bidder.CompanyId == adminCompanyId)
                {
                    var offers = inBidDB.vw_BidderOffers.Where(x => x.UserId == bidder.UserId).ToList();

                    SetOffersPosition(offers, bidder);

                    BidderViewModel bvm = new BidderViewModel
                    {
                        BidderData = bidder,
                        Offers = offers
                    };

                    return View(bvm);
                }
                return View("AccessDenied");
            }
            return View("AccessDenied");
        }



        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        private void PopulateCountiresDropDownLists(object selectedCountry = null/*, object selectedCompany = null*/)
        {
            var countries = inBidDB.Countries.Select(x => x);
            ViewBag.CountryId = new SelectList(countries, "CountryId", "Name", selectedCountry);

            // var companies = inBidDB.Companies.Select(x => x);
            //ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name", selectedCompany);
        }

        //private void PopulateCountiresAndCompaniesDropDownLists(object selectedCountry = null, object selectedCompany = null)
        //{


        //    var countries = inBidDB.Countries.Select(x => x);
        //    ViewBag.CountryId = new SelectList(countries, "CountryId", "Name", selectedCountry);

        //    // var companies = inBidDB.Companies.Select(x => x);
        //    //ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name", selectedCompany);

        //}

        private void PopulateLanguagesDropDownList(object selectedLanguage = null/*, object selectedCountry = null*//*, object selectedCompany = null*/)
        {
            var languages = inBidDB.Languages.Select(x => x);

            var langs = languages.ToList();
            HttpCookie userCookie = Request.Cookies["Culture"];

            DDLLanguagesHelpers.TranslateLanguages(langs, userCookie);
            ViewBag.LanguageId = new SelectList(langs, "LanguageId", "Name", selectedLanguage);

            /*  var countries = inBidDB.Countries.Select(x => x);           

               ViewBag.CountryId = new SelectList(countries, "CountryId", "Name", selectedCountry);
              */

            //if (HttpContext.Session["companyId"] != null)
            //    selectedCompany = (int)HttpContext.Session["companyId"];

            //var companies = inBidDB.Companies.Select(x => x);

            //ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name", selectedCompany);

            //HttpContext.Session["companyId"] = null;

        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }




    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1, //version
                userName, // user name
                DateTime.Now,             //creation
                DateTime.Now.AddMinutes(30), //Expiration
                createPersistentCookie, //Persistent
                userName); //since Classic logins don't have a "Friendly Name".  OpenID logins are handled in the AuthController.

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        string GetCanonicalUsername(string userName);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public string GetCanonicalUsername(string userName)
        {
            var user = _provider.GetUser(userName, true);
            if (user != null)
            {
                return user.UserName;
            }

            return null;
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }
    }

}
