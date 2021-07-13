using BucuriaDarului.Web.Common;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace BucuriaDarului.Web.ControllerHelpers.UniversalHelpers
{
    public class UniversalFunctions
    {
        public static bool IsAboutToExpire(int currentday, int daytocompareto)
        {
            if (currentday <= daytocompareto && currentday + 10 > daytocompareto || currentday > 355 && daytocompareto < 9)
            {
                return true;
            }
            return false;
        }

        public static int GetNumberOfItemPerPageFromSettings(ITempDataDictionary tempDataDic)
        {
            try
            {
                string numberOfDocumentsAsString = tempDataDic.Peek(Constants.NUMBER_OF_ITEMS_PER_PAGE).ToString();
                return Convert.ToInt16(numberOfDocumentsAsString);
            }
            catch
            {
                return Constants.DEFAULT_NUMBER_OF_ITEMS_PER_PAGE;
            }
        }

        internal static int GetCurrentPage(int page)
        {
            if (page > 0)
                return page;
            else
            {
                page = 1;
                return page;
            }
        }

        internal static int GetDayOfYear(DateTime date)
        {
            string dateasstring = date.ToString("dd-MM-yyyy");
            string[] dates = dateasstring.Split('-');
            int Day = Convert.ToInt16(dates[0]);
            int Month = Convert.ToInt16(dates[1]);
            Day = (Month - 1) * 30 + Day;
            return Day;
        }
    }
}