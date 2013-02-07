using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Inbid.Binders
{
    public class DecimalModelBinder : IModelBinder 
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                  Thread.CurrentThread.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    //public class DecimalModelBinder : IModelBinder
    //{

    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
    //        var modelState = new ModelState { Value = valueResult };

    //        decimal actualValue = 0;
    //        try
    //        {

    //            if (bindingContext.ModelMetadata.DataTypeName == DataType.Currency.ToString())
    //                decimal.TryParse(valueResult.AttemptedValue, NumberStyles.Currency, null, out actualValue);
    //            else
    //                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);


    //        }
    //        catch (FormatException e)
    //        {
    //            modelState.Errors.Add(e);
    //        }

    //        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
    //        return actualValue;
    //    }
    //}

}