using Finalaplication.Controllers;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using VolCommon;

namespace Finalaplication.Common
{
    public class ControllerHelper
    {
        public static void setViewBagEnvironment(ITempDataDictionary tempDataDic, dynamic viewBag)
        {
            string message = tempDataDic.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT).ToString();
            viewBag.env = message;
        }
        public static string PropertiesFromType(object atype)
        {
            /*if (atype == null) return new string[] { }; */
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> propNames = new List<string>();
            foreach (PropertyInfo prp in props)
            {
                propNames.Add(prp.Name);
            }
            return propNames.ToString();
        }
        public  Dictionary<int, string> DictionaryFromType(object atype)
        {
            if (atype == null) return new Dictionary<int, string>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            if(atype != null)
            {
                int i = 0;

                string all_property = PropertiesFromType(atype);
                dict.Add(i, all_property);
                foreach (PropertyInfo prp in props)
            {
                i=i+1;
                    if (dict.ContainsKey(i) == true)
                    {
                        dict[i] = prp.Name;
                    }
                    else
                    {
                        dict.Add(i, prp.Name);
                    }
                    

            }
            }
            return dict;
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

        public string[] SplitedHeader(string header)
        {
            string[] splitedHeader = header.Split(",");
            int s = splitedHeader.Count();
            if (header != null)
            {
                return splitedHeader;
            }
            else
            {
                string[] result = new string[s + 1];
                return result;
            }
        }

        public string GetAnswer(string finalHeader, bool toBeCompared)
        {
            string result = string.Empty;
            if (finalHeader.ToString() == "Activ" || finalHeader.Contains("are") == true || finalHeader.Contains("Are") == true || finalHeader.Contains("Cantină") == true || finalHeader.Contains("Pachet") == true || finalHeader.Contains("Fără") == true || finalHeader.Contains("Livrare") == true || finalHeader.Contains("Mașină") == true)
            {
                if (toBeCompared == true)
                { result = "Da"; }
                else
                { result = "Nu"; }
            }
            else
            {
                if (toBeCompared == true)
                { result = "Yes"; }
                else
                { result = "No"; }
            }
            return result;
        }

        public string GetHeaderForExcelPrinterVolunteer(IStringLocalizer<VolunteerController> _localizer)
        {
            string[] header = new string[25];
            header[0] = _localizer["Firstname"];
            header[1] = _localizer["Birthdate"];
            header[2] = _localizer["Address"];
            header[3] = _localizer["Gender"];
            header[4] = _localizer["Desiredarea"];
            header[5] = _localizer["CNP"];
            header[6] = _localizer["Fieldofactivity"];
            header[7] = _localizer["Ocupation"];
            header[8] = _localizer["CI_Info"];
            header[9] = _localizer["Active"];
            header[10] = _localizer["HourCount"];
            header[11] = _localizer["ContactInfo"];
            header[12] = _localizer["Additional_info"];
            header[13] = _localizer["Lastname"];
            header[14] = _localizer["District"];
            header[15] = _localizer["City"];
            header[16] = _localizer["Street"];
            header[17] = _localizer["Number"];

            header[18] = _localizer["CISeria"];
            header[19] = _localizer["CINr"];
            header[20] = _localizer["CIEliberat"];
            header[21] = _localizer["CIeliberator"];
            header[20] = _localizer["Phonenumber"];
            header[21] = _localizer["Emailaddress"];

            header[22] = _localizer["Hasdrivinglicense"];
            header[23] = _localizer["Hascar"];
            header[24] = _localizer["Remarks"];

            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        public string GetHeaderForExcelPrinterEvent(IStringLocalizer<EventController> _localizer)
        {
            string[] header = new string[8];
            header[0] = _localizer["Nameofevent"];
            header[1] = _localizer["Placeofevent"];
            header[2] = _localizer["Dateofevent"];
            header[3] = _localizer["Typeofactivities"];
            header[4] = _localizer["Typeofevent"];
            header[5] = _localizer["Duration"];
            header[6] = _localizer["Allocatedvolunteers"];
            header[7] = _localizer["Allocatedsponsors"];
            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        public string GetHeaderForExcelPrinterSponsor(IStringLocalizer<SponsorController> _localizer)
        {
            string[] header = new string[10];
            header[0] = _localizer["Nameofsponsor"];
            header[1] = _localizer["Date"];
            header[2] = _localizer["Moneyamount"];
            header[3] = _localizer["Whatgoods"];
            header[4] = _localizer["Goodsamount"];
            header[5] = _localizer["HasContract"];
            header[6] = _localizer["ContractDetails"];
            header[7] = _localizer["Phonenumber"];
            header[8] = _localizer["Emailaddress"];
            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        public string GetHeaderForExcelPrinterBeneficiary(IStringLocalizer<BeneficiaryController> _localizer)
        {
            string[] header = new string[35];
            header[0] = _localizer["Fullname"];
            header[1] = _localizer["Active"];
            header[2] = _localizer["Canteen"];
            header[3] = _localizer["HomeDelivery"];
            header[4] = _localizer["Homedeliverydriver"];
            header[5] = _localizer["HasGDPRagreement"];
            header[6] = _localizer["AddressInformation"];
            header[7] = _localizer["CNP"];
            header[8] = _localizer["CISeria"];
            header[9] = _localizer["CIEliberat"];
            header[10] = _localizer["Marca"];
            header[11] = _localizer["IDinvestigation"];
            header[12] = _localizer["IDapplication"];
            header[13] = _localizer["NumberOfPortions"];
            header[14] = _localizer["Lastimeactiv"];
            header[15] = _localizer["Phonenumber"];
            header[16] = _localizer["Birthplace"];
            header[17] = _localizer["Studies"];
            header[18] = _localizer["Profession"];
            header[19] = _localizer["Occupation"];
            header[20] = _localizer["Seniorityinworkfield"];
            header[21] = _localizer["Healthstate"];
            header[22] = _localizer["Disability"];
            header[23] = _localizer["Chroniccondition"];
            header[24] = _localizer["Addictions"];
            header[25] = _localizer["Hashealthinsurance"];
            header[26] = _localizer["Hashealthcard"];
            header[27] = _localizer["Married"];
            header[28] = _localizer["Spousename"];
            header[29] = _localizer["Homeless"];
            header[30] = _localizer["Housingtype"];
            header[31] = _localizer["Income"];
            header[32] = _localizer["Expenses"];
            header[33] = _localizer["Gender"];
            header[34] = _localizer["Weeklypackage"];

            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
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

        public static void GetEventsFromCsv(IMongoCollection<Event> eventcollection, List<string[]> result)
        {
            foreach (var details in result)
            {
                Event ev = new Event();

                try
                {
                    ev.NameOfEvent = details[0];
                }
                catch
                {
                    ev.NameOfEvent = "Invalid name";
                }
                try
                {
                    ev.PlaceOfEvent = details[1];
                }
                catch
                {
                    ev.PlaceOfEvent = "Invalid Place";
                }

                try
                {
                    if (details[2] == null || details[2] == "" || details[2] == "0")
                    {
                        ev.DateOfEvent = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (details[2].Contains("/") == true)
                        {
                            string[] date = details[2].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[2].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }
                        ev.DateOfEvent = data.AddDays(1);
                    }
                }
                catch
                {
                    ev.DateOfEvent = DateTime.MinValue;
                }

                if (details[3] == "" || details[3] == null)
                {
                    ev.NumberOfVolunteersNeeded = 0;
                }
                else
                {
                    int number = 0;
                    bool converted = false;
                    converted = Int32.TryParse(details[3], out number);
                    if (converted == true)
                    { ev.NumberOfVolunteersNeeded = number; }
                    else
                    { ev.NumberOfVolunteersNeeded = 0; }
                }
                try
                {
                    ev.TypeOfActivities = details[4];
                    ev.TypeOfEvent = details[5];
                    ev.Duration = details[6];
                    ev.AllocatedVolunteers = details[7];
                    ev.AllocatedSponsors = details[8];
                }
                catch
                {
                    ev.TypeOfActivities = "An error has occured";
                    ev.TypeOfEvent = "An error has occured";
                    ev.Duration = "0";
                    ev.AllocatedVolunteers = "An error has occured";
                    ev.AllocatedSponsors = "An error has occured";
                }

                eventcollection.InsertOne(ev);
            }
        }

        public static void GetSponsorsFromCsv(IMongoCollection<Sponsor> sponsorcollection, List<string[]> result)
        {
            foreach (var details in result)
            {
                Sponsor sponsor = new Sponsor();
                Sponsorship s = new Sponsorship();

                try
                {
                    sponsor.NameOfSponsor = details[0];
                }
                catch
                {
                    sponsor.NameOfSponsor = "Invalid name";
                }
                try
                {
                    if (details[1] == null || details[1] == "")
                    {
                        s.Date = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (details[1].Contains("/") == true)
                        {
                            string[] date = details[1].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[1].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }

                        s.Date = data.AddDays(1);
                    }
                }
                catch
                {
                    s.Date = DateTime.MinValue;
                }

                try
                {
                    s.MoneyAmount = details[2];
                    s.WhatGoods = details[3];
                    s.GoodsAmount = details[4];
                    sponsor.Sponsorship = s;
                }
                catch
                {
                    s.MoneyAmount = "Invalid entry";
                    s.WhatGoods = "Invalid entry";
                    s.GoodsAmount = "Invalid entry";
                    sponsor.Sponsorship = s;
                }

                try
                {
                    Contract c = new Contract();
                    if (details[5] == "True" || details[5] == "true")
                    {
                        c.HasContract = true;
                    }
                    else
                    {
                        c.HasContract = false;
                    }

                    c.NumberOfRegistration = details[6];

                    if (details[7] == null || details[7] == "")
                    {
                        c.RegistrationDate = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime dataS;
                        if (details[7].Contains("/") == true)
                        {
                            string[] date = details[7].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            dataS = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[7].Split('.');
                            dataS = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }

                        c.RegistrationDate = dataS.AddDays(1); ;
                    }

                    if (details[9] == null || details[8] == "")
                    {
                        c.ExpirationDate = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime dataS;
                        if (details[8].Contains("/") == true)
                        {
                            string[] date = details[8].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            dataS = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[8].Split('.');
                            dataS = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }

                        c.ExpirationDate = dataS.AddDays(1); ;
                    }
                    sponsor.Contract = c;

                    ContactInformation ci = new ContactInformation();
                    ci.PhoneNumber = details[9];
                    ci.MailAdress = details[10];
                    sponsor.ContactInformation = ci;
                    sponsorcollection.InsertOne(sponsor);
                }
                catch
                {
                }
            }
        }
    }
}