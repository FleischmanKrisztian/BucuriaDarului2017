using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Finalaplication.Common
{
    public class ControllerHelper
    {
        public static void setViewBagEnvironment(ITempDataDictionary tempDataDic, dynamic viewBag)
        {
            string message = tempDataDic.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT).ToString();
            viewBag.env = message;
        }

        public static int getNumberOfItemPerPageFromSettings(ITempDataDictionary tempDataDic)
        {
            try
            {
                string numberOfDocumentsAsString = tempDataDic.Peek(VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE).ToString();
                return Convert.ToInt16(numberOfDocumentsAsString);
            }
            catch
            {
                return VolMongoConstants.DEFAULT_NUMBER_OF_ITEMS_PER_PAGE;
            }
        }

        public static (DateTime[] startdates, DateTime[] enddates, int i) Datereturner(string activedates)
        {
            DateTime[] startdates = new DateTime[20];
            DateTime[] enddates = new DateTime[20];
            int i = 0;

            if (activedates != null)
            {
                while (activedates.Contains(","))
                {
                    bool last = false;
                    int aux = activedates.IndexOf(",");
                    activedates = activedates.Remove(0, 1);
                    int end = activedates.IndexOf("-");
                    int lastcharend = activedates.IndexOf(",");
                    //in the case where there are are no dates left
                    if (lastcharend == -1)
                    {
                        last = true;
                        lastcharend = activedates.Length;
                    }
                    lastcharend = lastcharend - end;
                    int lastcharstart = end - aux;
                    string startdatestring = activedates.Substring(aux, lastcharstart);
                    string enddatestring = activedates.Substring(lastcharstart + 1, lastcharend - 1);
                    startdates[i] = Dateformatter(startdatestring);
                    enddates[i] = Dateformatter(enddatestring);
                    //checks if it was the last if it was it doesn't do the following steps not to break
                    if (!last)
                        activedates = activedates.Substring(activedates.IndexOf(','));

                    i++;
                }
            }
            Array.Resize(ref startdates, i);
            Array.Resize(ref enddates, i);
            return (startdates, enddates, i);
        }

        public static DateTime Dateformatter(string datestring)
        {
            DateTime date;
            if (datestring.Contains("currently"))
            {
                date = DateTime.Today;
                return date;
            }
            else if (datestring.Length == 8)
            {
                datestring = datestring.Insert(0, "0");
                datestring = datestring.Insert(3, "0");
            }
            else if (datestring.Length == 9 && datestring[2] != '/')
            {
                datestring = datestring.Insert(0, "0");
            }
            else if (datestring.Length == 9)
            {
                datestring = datestring.Insert(2, "0");
            }
            date = DateTime.ParseExact(datestring, "dd/MM/yyyy", CultureInfo.DefaultThreadCurrentCulture);
            return date;
        }
    }
}
