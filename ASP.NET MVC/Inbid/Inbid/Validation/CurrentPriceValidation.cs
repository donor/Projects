using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Inbid.Models;
using Inbid.Controllers;

namespace Inbid.Validation
{

    public sealed class CurrentPriceAttribute : ValidationAttribute//, IClientValidatable
    {
        private InBidEntities InBidEntities = null;
        private string _basePropertyName;
        private string _basePropertyStartPrice;

        public CurrentPriceAttribute(string basePropertyName, string basePropertyStartPrice)//:this(new InBidEntities())
        {
            _basePropertyName = basePropertyName;
            _basePropertyStartPrice = basePropertyStartPrice;

        }

        //public CurrentPriceAttribute(InBidEntities entities)
        //{
        //    InBidEntities = entities;
        //}

        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(Resources.Resources.CurrentPriceChangeToSmall, name, _basePropertyName);
        }

        //Override IsValid  
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            //Get PropertyInfo Object  
             InBidEntities = new InBidEntities();

            var basePropertyInfo = validationContext.ObjectType.GetProperty(_basePropertyName);
            var AuctionId = (int)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var basePropertyInfoStartPrice = validationContext.ObjectType.GetProperty(_basePropertyStartPrice);
            var StartPrice = (decimal)basePropertyInfoStartPrice.GetValue(validationContext.ObjectInstance, null);

            var ChangeCurrentPrice = (Decimal)value;
           // InBidEntities.SaveChanges();
            var auction = InBidEntities.Auctions.Where(x => x.AuctionId == AuctionId).SingleOrDefault();
            decimal MinJump = auction.MinJump;
            decimal CurrentPrice = auction.CurrentPrice;
            decimal AuctionStartPrice = auction.StartPrice;
            bool Direction = auction.Direction;
            var offers = InBidEntities.Offers.Where(x => x.AuctionId == AuctionId);


            if (offers.Count() > 0)
            {
                if (Direction) //aukcja w dół
                {
                    

                    if ((ChangeCurrentPrice == 0)&&(StartPrice==0))
                    {                       
                        return null;
                    }
                    //edycja oferty
                    if (ChangeCurrentPrice > 0)
                    {
                        //za mała zmiana Zmiana obecnej ceny jest miniejsza od minimalnego skoku
                        if ((CurrentPrice - ChangeCurrentPrice < MinJump) || (CurrentPrice < ChangeCurrentPrice))
                        { 
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }
                    }
                    if (StartPrice > 0)
                    {
                        //za mała oferta Wartość Oferty nie spełna warunków aukcji
                        ChangeCurrentPrice = StartPrice;
                        if ((CurrentPrice - ChangeCurrentPrice < MinJump) || (CurrentPrice < ChangeCurrentPrice))
                        {
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }
                    }
                }
               //dodawanie oferty
                else
                {

                    if ((ChangeCurrentPrice == 0) && (StartPrice == 0))
                    {
                        return null;
                    }
                    //edycja oferty
                    if (ChangeCurrentPrice > 0)
                    {
                        //za mała zmiana Zmiana obecnej ceny jest miniejsza od minimalnego skoku
                        if ((-(CurrentPrice - ChangeCurrentPrice) < MinJump) || (CurrentPrice > ChangeCurrentPrice))
                        {
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }
                    }
                    if (StartPrice > 0)
                    {
                        //za mała oferta Wartość Oferty nie spełna warunków aukcji
                        ChangeCurrentPrice = StartPrice;
                        if ((-(CurrentPrice - ChangeCurrentPrice) < MinJump) || (CurrentPrice > ChangeCurrentPrice))
                        {
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }
                    }
                }
            }
            else //dodawanie pierwzej oferty
            {
                if (Direction) //aukcja malejąca
                {                   
                        ChangeCurrentPrice = StartPrice;
                        if ((CurrentPrice - ChangeCurrentPrice < 0) || (ChangeCurrentPrice > AuctionStartPrice) )
                        {
                            //dodawanie pierwszej oferty mnijeszej od aktualnej ceny
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }
                }
                else //aukcja rosnąca
                {
                    ChangeCurrentPrice = StartPrice;
                    if ((-(CurrentPrice - ChangeCurrentPrice) < 0) || (ChangeCurrentPrice < AuctionStartPrice))
                        {
                            //dodawanie pierwszej oferty wiekszej od aktualnej ceny
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }                                        
                }



            }

            //Actual comparision  
            

            //Default return - This means there were no validation error  
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

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules
        //   (ModelMetadata metadata, ControllerContext context)
        //{
        //    var rule = new ModelClientValidationRule();
        //    rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());

        //    //This string identifies which Javascript function to be executed to validate this 
        //    rule.ValidationType = "greaterthan";
        //    rule.ValidationParameters.Add("otherfield", _basePropertyName);
        //    yield return rule;
        //}


     
    }  
}