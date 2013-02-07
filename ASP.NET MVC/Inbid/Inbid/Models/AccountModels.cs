using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Inbid.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        //[Display(Name = "Remember me?")]
       // public DateTime RememberMe1 { get; set; }
    }

   
    public class RegistrationModel
    {
            [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "UserNameRequired")]
            public string UserName { get; set; }


            [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstNameRequired")]
            public string FirstName { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LastNameRequired")]
            public string LastName { get; set; }


            [Required(ErrorMessageResourceType = typeof(Resources.Resources),
               ErrorMessageResourceName = "EmailRequired")]
            [RegularExpression(".+@.+\\..+", ErrorMessageResourceType = typeof(Resources.Resources),
                ErrorMessageResourceName = "EmailInvalid")]
            public string Email { get; set; }

            [RegularExpression(@"^[01]?[- .]?\(?[2-9]\d{2}\)?[- .]?\d{3}[- .]?\d{4}$", ErrorMessageResourceType = typeof(Resources.Resources),
                ErrorMessageResourceName = "PhoneInvalid")]   
            public string Phone { get; set; }


            [Required(ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "PasswordRequired")]
            [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources),
              ErrorMessageResourceName = "PasswordLong")]
            [DataType(DataType.Password)]           
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Compare("Password",ErrorMessageResourceType = typeof(Resources.Resources),
                ErrorMessageResourceName = "ComparePassword")]                    
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LanguageRequired")]
            public int? LanguageId { get; set; }

          // [Required]
          // public int? CompanyId { get; set; }

           //  public int AdressId { get; set; }

           public string Adress { get; set; }

           public string City { get; set; }

           public string Region { get; set; }

           public string PostalCode { get; set; }

           public int CountryId { get; set; }

       
           //trzea dorobić walidacie wzgędem unikatowości
           public string Name { get; set; }

        //  public int AdressId { get; set; }

        //  public bool Bidder { get; set; }

        //  public bool BidderView { get; set; }

           public byte BidderQuality { get; set; }
           public byte BidderViewQuality { get; set; }

           public byte AuctionQuality { get; set; }

    }

    public class RegistrationBidderOrBidderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "UserNameRequired")]
        public string UserName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstNameRequired")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LastNameRequired")]
        public string LastName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources),
           ErrorMessageResourceName = "EmailRequired")]
        [RegularExpression(".+@.+\\..+", ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "EmailInvalid")]
        public string Email { get; set; }

        [RegularExpression(@"^[01]?[- .]?\(?[2-9]\d{2}\)?[- .]?\d{3}[- .]?\d{4}$", ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "PhoneInvalid")]
        public string Phone { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources),
        ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources),
          ErrorMessageResourceName = "PasswordLong")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources),
            ErrorMessageResourceName = "ComparePassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LanguageRequired")]
        public int? LanguageId { get; set; }

       // [Required]
        //public int? CompanyId { get; set; }

        //  public int AdressId { get; set; }

        //public string Adress { get; set; }

        //public string City { get; set; }

        //public string Region { get; set; }

        //public string PostalCode { get; set; }

       // public int CountryId { get; set; }


        //trzea dorobić walidacie wzgędem unikatowości
      //  public string Name { get; set; }

        //  public int AdressId { get; set; }

        public bool Bidder { get; set; }

        //  public bool BidderView { get; set; }

       



    }

}
