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


        public static int GetNumberOfDaysBeforBirtday(ITempDataDictionary tempDataDic)
        {
            try
            {
                var numberOfDays = tempDataDic.Peek(Constants.ALARM_NUMBER_OF_DAYS_BEFOR_BIRTHDAY).ToString();
                return Convert.ToInt16(numberOfDays);
            }
            catch
            {
                return Constants.BIRTHDAY_ALARM_SETTING_DEFAULT;
            }
        }

        public static int GetNumberOfDaysBeforExpiration(ITempDataDictionary tempDataDic)
        {
            try
            {
                var numberOfDays = tempDataDic.Peek(Constants.NUMBER_OF_DAYS_BEFOR_EXPIRATION).ToString();
                return Convert.ToInt16(numberOfDays);
            }
            catch
            {
                return Constants.DEFAULT_NUMBER_OF_DAYS_BEFOR_EXPIRATION;
            }
        }

    }
}