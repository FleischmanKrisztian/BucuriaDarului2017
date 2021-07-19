using BucuriaDarului.Web.Common;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace BucuriaDarului.Web.ControllerHelpers.UniversalHelpers
{
    public class UniversalFunctions
    {
        public static int GetNumberOfItemPerPageFromSettings(ITempDataDictionary tempDataDic)
        {
            try
            {
                var numberOfDocumentsAsString = tempDataDic.Peek(Constants.NUMBER_OF_ITEMS_PER_PAGE).ToString();
                return Convert.ToInt16(numberOfDocumentsAsString);
            }
            catch
            {
                return Constants.DEFAULT_NUMBER_OF_ITEMS_PER_PAGE;
            }
        }
    }
}