using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
                    ev.NumberOfVolunteersNeeded = Convert.ToInt16(details[3]);
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

        public static void GetSponsorsFromCsv(IMongoCollection<Sponsor> sponsorcollection, List<string[]> result )
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
        public static string GetBeneficiaryFromCsv(IMongoCollection<Beneficiary> beneficiarycollection, List<string[]> result, string duplicates, int documentsimported)
        {
            foreach (var details in result)
            {
                if (beneficiarycollection.CountDocuments(z => z.CNP == details[8]) >= 1 && details[8] != "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.Fullname == details[0]) >= 1 && details[8] == "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else
                {
                    if (details[2] == "True" || details[2] == "False")
                    {
                        documentsimported++;
                        var aux = details[42];
                        for (int i = details.Length - 1; i > 0; i--)
                        {
                            details[i] = details[i - 1];
                        }
                        Beneficiary beneficiary = new Beneficiary();

                        try
                        {
                            beneficiary.Fullname = details[1];
                        }
                        catch
                        {
                            beneficiary.Fullname = "Incorrect Name";
                        }
                        try
                        {
                            beneficiary.Adress = details[8];
                        }
                        catch
                        {
                            beneficiary.Adress = "Incorrect address";
                        }

                        if (details[2] == "true" || details[2] == "True")
                        {
                            beneficiary.Active = true;
                        }
                        else
                        {
                            beneficiary.Active = false;
                        }

                        if (details[4] == "true" || details[4] == "True")
                        {
                            beneficiary.Canteen = true;
                        }
                        else
                        {
                            beneficiary.Canteen = false;
                        }

                        if (details[5] == "true" || details[5] == "True")
                        {
                            beneficiary.HomeDelivery = true;
                        }
                        else
                        {
                            beneficiary.HomeDelivery = false;
                        }

                        if (details[3] == "true" || details[3] == "True")
                        {
                            beneficiary.Weeklypackage = true;
                        }
                        else
                        {
                            beneficiary.Weeklypackage = false;
                        }

                        if (details[6] != null || details[6] != "")
                        {
                            beneficiary.HomeDeliveryDriver = details[6];
                        }
                        else
                        {
                            beneficiary.HomeDeliveryDriver = " ";
                        }

                        if (details[9] != null || details[9] != "")
                        {
                            beneficiary.CNP = details[9];
                        }

                        try
                        {
                            if (details[21] != null || details[21] != " ")
                            {
                                if (int.TryParse(details[21], out int portions))
                                {
                                    beneficiary.NumberOfPortions = portions;
                                }
                            }
                        }
                        catch
                        {
                            beneficiary.NumberOfPortions = 0;
                        }

                        try
                        {
                            Marca MarcaDetails = new Marca();
                            if (details[12] != null) { MarcaDetails.marca = details[12]; }
                            if (details[13] != null) { MarcaDetails.IdAplication = details[13]; }
                            if (details[14] != null) { MarcaDetails.IdInvestigation = details[14]; }
                            if (details[25] != null) { MarcaDetails.IdContract = details[15]; }
                            beneficiary.Marca = MarcaDetails;
                        }
                        catch
                        {
                            beneficiary.Marca.marca = "error";
                            beneficiary.Marca.IdAplication = "error";
                            beneficiary.Marca.IdInvestigation = "error";
                            beneficiary.Marca.IdContract = "error";
                        }

                        Personalinfo personal = new Personalinfo();
                        try
                        {
                            if (details[18] != null || details[18] != "")
                            {
                                personal.PhoneNumber = details[18];
                            }

                            if (details[19] != null || details[19] != "")
                            {
                                personal.BirthPlace = details[19];
                            }

                            if (details[20] != null)
                            {
                                personal.Studies = details[20];
                            }

                            personal.Profesion = details[21];
                            personal.Ocupation = details[22];
                            personal.SeniorityInWorkField = details[23];
                            personal.HealthState = details[31];
                        }
                        catch
                        {
                            beneficiary.PersonalInfo.Profesion = "Error";
                            beneficiary.PersonalInfo.Ocupation = "Error";
                            beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                            beneficiary.PersonalInfo.HealthState = "Error";
                        }

                        if (details[32] != " " || details[32] != null)
                        {
                            personal.Disalility = details[32];
                        }
                        else { personal.Disalility = " "; }

                        if (details[33] != " " || details[33] != null)
                        {
                            personal.ChronicCondition = details[33];
                        }
                        else { personal.ChronicCondition = " "; }

                        if (details[34] != " " || details[34] != null)
                        {
                            personal.Addictions = details[34];
                        }
                        else { personal.Addictions = " "; }

                        if (details[35] == "true" || details[35] == "True")
                        {
                            personal.HealthInsurance = true;
                        }
                        else { personal.HealthInsurance = false; }

                        if (details[36] == "true" || details[36] == "True")
                        {
                            personal.HealthCard = true;
                        }
                        else { personal.HealthCard = false; }

                        if (details[37] != " " || details[37] != null)
                        {
                            personal.Married = details[37];
                        }
                        else { personal.Married = " "; }

                        if (details[38] != " " || details[38] != null)
                        {
                            personal.SpouseName = details[38];
                        }
                        else { personal.SpouseName = " "; }

                        if (details[40] != " " || details[40] != null)
                        {
                            personal.HousingType = details[40];
                        }
                        else { personal.HousingType = " "; }

                        if (details[39] == "true" || details[39] == "True")
                        {
                            personal.HasHome = true;
                        }
                        else { personal.HasHome = false; }

                        if (details[41] != " " || details[41] != null)
                        {
                            personal.Income = details[41];
                        }
                        else { personal.Income = " "; }

                        if (details[42] != " " || details[42] != null)
                        {
                            personal.Expences = details[42];
                        }
                        else { personal.Expences = " "; }

                        try
                        {
                            if (details[24] == null || details[24] == "")
                            {
                                personal.Birthdate = DateTime.MinValue;
                            }
                            else
                            {
                                DateTime data;
                                if (details[24].Contains("/") == true)
                                {
                                    string[] date = details[24].Split(" ");
                                    string[] FinalDate = date[0].Split("/");
                                    data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                }
                                else
                                {
                                    string[] anotherDate = details[24].Split('.');
                                    data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                                }
                                personal.Birthdate = data.AddDays(1);
                            }
                        }
                        catch
                        {
                            personal.Birthdate = DateTime.MinValue;
                        }

                        try
                        {
                            if (aux == "1" || aux == "True")
                            {
                                personal.Gender = VolCommon.Gender.Female;
                            }
                            else
                            {
                                personal.Gender = VolCommon.Gender.Male;
                            }
                            beneficiary.Comments = details[23];
                        }
                        catch
                        {
                            beneficiary.Comments = "";
                        }

                        if (details[22] == null || details[22] == "")
                        {
                            beneficiary.LastTimeActiv = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[22].Contains("/") == true)
                            {
                                string[] date = details[22].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                            }
                            else
                            {
                                string[] anotherDate = details[22].Split('.');
                                data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                            }
                            beneficiary.LastTimeActiv = data.AddDays(1);
                        }

                        CI ciInfo = new CI();

                        try
                        {
                            if (details[16] == null || details[16] == "")
                            {
                                ciInfo.ExpirationDateCI = DateTime.MinValue;
                            }
                            else
                            {
                                DateTime data;
                                if (details[16].Contains("/") == true)
                                {
                                    string[] date = details[16].Split(" ");
                                    string[] FinalDate = date[0].Split("/");
                                    data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                }
                                else
                                {
                                    string[] anotherDate = details[16].Split('.');
                                    data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                                }
                                ciInfo.ExpirationDateCI = data.AddDays(1);
                            }
                        }
                        catch
                        {
                            ciInfo.ExpirationDateCI = DateTime.MinValue;
                        }

                        try
                        {
                            ciInfo.CIinfo = details[15];

                            if (details[10] != "True" || details[10] != "true")
                            { ciInfo.HasId = false; }
                        }
                        catch
                        {
                            ciInfo.CIinfo = "";
                        }
                        beneficiary.PersonalInfo = personal;
                        beneficiary.CI = ciInfo;

                        beneficiarycollection.InsertOne(beneficiary);
                    }
                    else
                    {
                        documentsimported++;

                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary.HasGDPRAgreement = false;

                        try
                        {
                            beneficiary.Fullname = details[1];
                        }
                        catch
                        {
                            beneficiary.Fullname = "Incorrect Name";
                        }
                        try
                        {
                            beneficiary.Adress = details[8];
                        }
                        catch
                        {
                            beneficiary.Adress = "Incorrect address";
                        }

                        if (details[3] == "true" || details[3] == "True" || details[3] == "Activ" || details[3] == "activ")
                        {
                            beneficiary.Active = true;
                        }
                        else
                        {
                            beneficiary.Active = false;
                        }

                        if (details[4] == "da" || details[4] == "DA" || details[4] == "Da")
                        {
                            beneficiary.HomeDelivery = true;
                        }
                        else
                        {
                            beneficiary.HomeDelivery = false;
                        }
                        if (beneficiary.HomeDelivery == true)
                        { beneficiary.Canteen = false; }
                        if (details[5] == "da" || details[5] == "DA" || details[5] == "Da")
                        {
                            beneficiary.Weeklypackage = true;
                        }
                        else
                        {
                            beneficiary.Weeklypackage = false;
                        }

                        if (details[7] != null || details[7] != "")
                        {
                            beneficiary.HomeDeliveryDriver = details[7];
                        }
                        else
                        {
                            beneficiary.HomeDeliveryDriver = " ";
                        }

                        if (details[9] != null || details[9] != "")
                        {
                            beneficiary.CNP = details[9];
                        }

                        try
                        {
                            if (details[17] != null || details[17] != " ")
                            {
                                if (int.TryParse(details[17], out int portions))
                                {
                                    beneficiary.NumberOfPortions = portions;
                                }
                            }
                        }
                        catch
                        {
                            beneficiary.NumberOfPortions = 0;
                        }

                        try
                        {
                            Marca MarcaDetails = new Marca();
                            if (details[12] != null) { MarcaDetails.marca = details[12]; }
                            if (details[13] != null) { MarcaDetails.IdAplication = details[13]; }
                            if (details[14] != null) { MarcaDetails.IdInvestigation = details[14]; }
                            if (details[15] != null) { MarcaDetails.IdContract = details[15]; }
                            beneficiary.Marca = MarcaDetails;
                        }
                        catch
                        {
                            beneficiary.Marca.marca = "error";
                            beneficiary.Marca.IdAplication = "error";
                            beneficiary.Marca.IdInvestigation = "error";
                            beneficiary.Marca.IdContract = "error";
                        }

                        Personalinfo personal = new Personalinfo();
                        try
                        {
                            if (details[18] != null || details[18] != "")
                            {
                                personal.PhoneNumber = details[18];
                            }

                            if (details[19] != null || details[19] != "")
                            {
                                personal.BirthPlace = details[19];
                            }

                            if (details[20] != null)
                            {
                                personal.Studies = details[20];
                            }

                            personal.Profesion = details[21];
                            personal.Ocupation = details[22];
                            personal.SeniorityInWorkField = details[23];
                            personal.HealthState = details[24];
                        }
                        catch
                        {
                            beneficiary.PersonalInfo.Profesion = "Error";
                            beneficiary.PersonalInfo.Ocupation = "Error";
                            beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                            beneficiary.PersonalInfo.HealthState = "Error";
                        }

                        if (details[25] != " " || details[25] != null)
                        {
                            personal.Disalility = details[25];
                        }
                        else { personal.Disalility = " "; }

                        if (details[26] != " " || details[26] != null)
                        {
                            personal.ChronicCondition = details[26];
                        }
                        else { personal.ChronicCondition = " "; }

                        if (details[27] != " " || details[27] != null)
                        {
                            personal.Addictions = details[27];
                        }
                        else { personal.Addictions = " "; }

                        if (details[28] == "true" || details[28] == "True" || details[28] == "da" || details[28] == "Da" || details[28] == "DA")
                        {
                            personal.HealthInsurance = true;
                        }
                        else { personal.HealthInsurance = false; }

                        if (details[29] == "true" || details[29] == "True" || details[29] == "da" || details[29] == "Da" || details[29] == "DA")
                        {
                            personal.HealthCard = true;
                        }
                        else { personal.HealthCard = false; }

                        if (details[30] != " " || details[30] != null)
                        {
                            personal.Married = details[30];
                        }
                        else { personal.Married = " "; }

                        if (details[31] != " " || details[31] != null)
                        {
                            personal.SpouseName = details[31];
                        }
                        else { personal.SpouseName = " "; }

                        if (details[32] != " " || details[32] != null)
                        {
                            personal.HousingType = details[32];
                        }
                        else { personal.HousingType = " "; }

                        if (details[33] == "true" || details[33] == "True" || details[33] == "da" || details[33] == "Da" || details[33] == "DA")
                        {
                            personal.HasHome = true;
                        }
                        else { personal.HasHome = false; }

                        if (details[34] != " " || details[34] != null)
                        {
                            personal.Income = details[34];
                        }
                        else { personal.Income = " "; }

                        if (details[35] != " " || details[35] != null)
                        {
                            personal.Expences = details[35];
                        }
                        else { personal.Expences = " "; }

                        try
                        {
                            string date = details[36] + "-" + details[37] + "-" + details[38];
                            DateTime data = Convert.ToDateTime(date);
                            personal.Birthdate = data.AddDays(1);
                        }
                        catch
                        {
                            personal.Birthdate = DateTime.MinValue;
                        }

                        try
                        {
                            if (details[42] == "F" || details[42] == "f")
                            {
                                personal.Gender = VolCommon.Gender.Female;
                            }
                            else
                            {
                                personal.Gender = VolCommon.Gender.Male;
                            }
                            beneficiary.Comments = details[42];
                        }
                        catch
                        {
                            beneficiary.Comments = "";
                        }

                        beneficiary.LastTimeActiv = DateTime.MinValue;

                        CI ciInfo = new CI();

                        try
                        {
                            if (details[11] == null || details[11] == "")
                            {
                                ciInfo.ExpirationDateCI = DateTime.MinValue;
                            }
                            else
                            {
                                DateTime data;
                                if (details[11].Contains("/") == true)
                                {
                                    string[] date = details[11].Split(" ");
                                    string[] FinalDate = date[0].Split("/");
                                    data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                }
                                else
                                {
                                    string[] anotherDate = details[11].Split('.');
                                    data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                                }
                                ciInfo.ExpirationDateCI = data.AddDays(1);
                            }
                        }
                        catch
                        {
                            ciInfo.ExpirationDateCI = DateTime.MinValue;
                        }

                        try
                        {
                            ciInfo.CIinfo = details[10];

                            if (details[10] != "" || details[10] != null)
                            { ciInfo.HasId = true; }
                        }
                        catch
                        {
                            ciInfo.CIinfo = "";
                        }
                        beneficiary.PersonalInfo = personal;
                        beneficiary.CI = ciInfo;

                        beneficiarycollection.InsertOne(beneficiary);
                    }

                }
            }
            return duplicates;
        }


            public static string GetVolunteersFromCsv(IMongoCollection<Volunteer> vollunteercollection, List<string[]> result, string duplicates, int documentsimported)
        {
            foreach (var details in result)
            {
                try
                {
                    if (vollunteercollection.CountDocuments(z => z.CNP == details[9]) >= 1 && details[9] != "")
                    {
                        duplicates = duplicates + details[0] + " " + details[1] + ", ";
                    }
                    else if (vollunteercollection.CountDocuments(z => z.Firstname == details[0]) >= 1 && details[9] == "" && vollunteercollection.CountDocuments(z => z.Lastname == details[1]) >= 1)
                    {
                        duplicates = duplicates + details[0] + " " + details[1] + ", ";
                    }
                    else
                    {
                        documentsimported++;
                        //Mistake;we added a new aux in order to save the value from the last cell;
                        var aux = details[22];
                        for (int i = details.Length - 1; i > 0; i--)
                        {
                            details[i] = details[i - 1];
                        }
                        Volunteer volunteer = new Volunteer();
                        try
                        {
                            volunteer.Firstname = details[1];
                            volunteer.Lastname = details[2];
                        }
                        catch
                        {
                            volunteer.Firstname = "Error";
                            volunteer.Lastname = "Error";
                        }

                        try
                        {
                            if (details[3] != null || details[3] != "")
                            {
                                string[] date;
                                date = details[3].Split(" ");

                                string[] FinalDate = date[0].Split("/");
                                DateTime data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);

                                volunteer.Birthdate = data.AddDays(1);
                            }
                            else
                            { volunteer.Birthdate = DateTime.MinValue; }
                        }
                        catch
                        {
                            volunteer.Birthdate = DateTime.MinValue;
                        }

                        Address a = new Address();

                        if (details[4] != null || details[4] != "")
                        {
                            a.District = details[4];
                        }
                        else { a.District = "-"; }

                        if (details[5] != null || details[5] != "")
                        {
                            a.City = details[5];
                        }
                        else
                        { a.City = "-"; }

                        if (details[6] != null || details[6] != "")
                        {
                            a.Street = details[6];
                        }
                        else { a.Street = "-"; }

                        if (details[7] != null || details[7] != "")
                        {
                            a.Number = details[7];
                        }
                        else
                        { a.Number = "-"; }

                        try
                        {
                            if (details[8] == "True" || details[8] == "1")
                            {
                                volunteer.Gender = VolCommon.Gender.Female;
                            }
                            else
                            {
                                volunteer.Gender = VolCommon.Gender.Male;
                            }
                        }
                        catch
                        {
                            volunteer.Gender = Gender.Male;
                        }

                        if (details[9] != null || details[9] != "")
                        {
                            volunteer.Desired_workplace = details[9];
                        }
                        else { volunteer.Desired_workplace = "-"; }

                        if (details[10] != null || details[10] != "")
                        {
                            volunteer.CNP = details[10];
                        }
                        else { volunteer.CNP = "-"; }

                        if (details[11] != null || details[11] != "")
                        {
                            volunteer.Field_of_activity = details[11];
                        }
                        else { volunteer.Field_of_activity = "-"; }
                        if (details[12] != null || details[12] != "")
                        {
                            volunteer.Occupation = details[12];
                        }
                        else { volunteer.Occupation = "-"; }

                        if (details[13] != null || details[13] != "")
                        {
                            volunteer.CIseria = details[13];
                        }
                        else
                        { volunteer.CIseria = "-"; }

                        if (details[14] != null || details[14] != "")
                        {
                            volunteer.CINr = details[13];
                        }
                        else
                        { volunteer.CINr = "-"; }

                        try
                        {
                            if (details[15] != null || details[15] != "")
                            {
                                string[] date;
                                date = details[15].Split(" ");

                                string[] FinalDate = date[0].Split("/");
                                DateTime data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                volunteer.CIEliberat = data.AddDays(1);
                            }
                            else
                            { volunteer.CIEliberat = DateTime.MinValue; }
                        }
                        catch
                        {
                            volunteer.CIEliberat = DateTime.MinValue;
                        }

                        if (details[16] != null || details[16] != "")
                        {
                            volunteer.CIeliberator = details[16];
                        }
                        else
                        { volunteer.CIeliberator = "-"; }
                        if (details[17] == "True")
                        {
                            volunteer.InActivity = true;
                        }
                        else
                        {
                            volunteer.InActivity = false;
                        }

                        if (details[18] != null || details[18] != "0" || details[18] != "")
                        {
                            volunteer.HourCount = Convert.ToInt16(details[18]);
                        }
                        else
                        {
                            volunteer.HourCount = 0;
                        }
                        ContactInformation c = new ContactInformation();
                        if (details[19] != null || details[19] != "")
                        {
                            c.PhoneNumber = details[19];
                        }
                        else
                        { c.PhoneNumber = "-"; }
                        if (details[20] != null || details[20] != "")
                        {
                            c.MailAdress = details[20];
                        }
                        else
                        { c.MailAdress = "-"; }
                        volunteer.ContactInformation = c;
                        Additionalinfo ai = new Additionalinfo();

                        if (details[21] == "True")
                        {
                            ai.HasDrivingLicence = true;
                        }
                        else
                        {
                            ai.HasDrivingLicence = false;
                        }

                        if (details[22] == "True")
                        {
                            ai.HasCar = true;
                        }
                        else
                        {
                            ai.HasCar = false;
                        }

                        ai.Remark = aux;

                        volunteer.Address = a;
                        volunteer.Additionalinfo = ai;
                        vollunteercollection.InsertOne(volunteer);
                    }
                }
                catch
                {
                    if (vollunteercollection.CountDocuments(z => z.CNP == details[1]) >= 1 && details[1] != "")
                    {
                        duplicates = duplicates + details[1] + ", ";
                    }
                    else if (vollunteercollection.CountDocuments(z => z.Firstname == details[0]) >= 1 && details[1] == "")
                    {
                        duplicates = duplicates + details[0] + ", ";
                    }
                    else
                    {
                        documentsimported++;

                        Volunteer volunteer = new Volunteer();
                        try
                        {
                            if (details[0].Contains(" ") == true)

                            {
                                string[] splitName = details[0].Split();
                                volunteer.Lastname = splitName[0];
                                if (splitName.Count() == 2)
                                {
                                    volunteer.Firstname = splitName[1];
                                }
                                else
                                { volunteer.Firstname = splitName[1] + " " + splitName[2]; }
                            }
                        }
                        catch
                        {
                            volunteer.Firstname = "Error";
                            volunteer.Lastname = "Error";
                        }

                        volunteer.Birthdate = DateTime.MinValue;

                        Address a = new Address();

                        if (details[2] != null || details[2] != "")
                        {

                            a.District = details[2];

                        }
                        else
                        { a.District = "-"; }
                        a.City = "-";
                        a.Street = "-";
                        a.Number = "-";


                        volunteer.Gender = Gender.Male;

                        if (details[9] != null || details[9] != "")
                        {
                            volunteer.Desired_workplace = details[9];
                        }
                        else
                        { volunteer.Desired_workplace = "-"; }
                        if (details[1] != null || details[1] != "")
                        {
                            volunteer.CNP = details[1];
                        }
                        else
                        { volunteer.CNP = "-"; }

                        if (details[3] != null)
                        {
                            if (details[3] != "")
                            {
                                string[] splited = details[3].Split(" ");
                                volunteer.CIseria = splited[0];
                                volunteer.CINr = splited[1];
                            }
                            else
                            {
                                volunteer.CIseria = "Error";
                                volunteer.CINr = "Error";
                            }
                        }

                        volunteer.CIEliberat = DateTime.MinValue;

                        volunteer.HourCount = 0;

                        ContactInformation c = new ContactInformation();
                        if (details[4] != null || details[4] != "")
                        {
                            c.PhoneNumber = details[4];
                        }

                        else
                        { c.PhoneNumber = "-"; }

                        volunteer.ContactInformation = c;
                        Additionalinfo ai = new Additionalinfo();


                        ai.HasDrivingLicence = false;
                        if (details[8] != null)
                        {
                            ai.Remark = details[8];
                        }
                        else { ai.Remark = "-"; }
                        ai.HasCar = false;

                        volunteer.Occupation = "-";



                        volunteer.Address = a;
                        volunteer.Additionalinfo = ai;
                        vollunteercollection.InsertOne(volunteer);

                    }
                }
                
            }
            return  duplicates;
        }
    }
}
