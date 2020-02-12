using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
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