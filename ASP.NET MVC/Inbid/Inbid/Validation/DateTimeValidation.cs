using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Inbid.Validation
{
    //public class DateTimeValidation
    //{

    //}
    public sealed class DateStartGreaterNowAttribute : ValidationAttribute
    {
        public DateStartGreaterNowAttribute()
        {  
            
        }  
   
        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(Resources.Resources.ErrorStartTimeGreaterNow, name);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)  
        {         
            var thisDate = (DateTime)value;  
   
            //Actual comparision  
            if (thisDate <= DateTime.Now)  
            {  
                var message = FormatErrorMessage(validationContext.DisplayName);  
                return new ValidationResult(message);  
            }  
               //Default return - This means there were no validation error  
            return null;  
        }
    }

    

    public sealed class DateGreaterThanAttribute : ValidationAttribute,IClientValidatable
    {       
        private string _basePropertyName;  
   
        public DateGreaterThanAttribute(string basePropertyName)
        {  
            _basePropertyName = basePropertyName;  
        }  
   
        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(Resources.Resources.ErrorEndTimeGreaterStartTime, name, _basePropertyName);
        }  
   
        //Override IsValid  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)  
        {  
            //Get PropertyInfo Object  
            var basePropertyInfo = validationContext.ObjectType.GetProperty(_basePropertyName);  
   
            //Get Value of the property  
            var startDate = (DateTime)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);     
              
            var thisDate = (DateTime)value;  
   
            //Actual comparision  
            if (thisDate <= startDate)  
            {  
                var message = FormatErrorMessage(validationContext.DisplayName);  
                return new ValidationResult(message);  
            }  
   
            //Default return - This means there were no validation error  
            return null;  
        }  

         public IEnumerable<ModelClientValidationRule> GetClientValidationRules
            (ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());

            //This string identifies which Javascript function to be executed to validate this 
            rule.ValidationType = "greaterthan";
            rule.ValidationParameters.Add("otherfield", _basePropertyName);
            yield return rule;
        }
   
    }  

}
