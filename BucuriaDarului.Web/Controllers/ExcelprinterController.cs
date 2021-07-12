using System;
using System.Linq;
using BucuriaDarului.Core;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.DatabaseManager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BucuriaDarului.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelprinterController : ControllerBase
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.DATABASE_NAME_LOCAL);

        private EventManager eventManager = new EventManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private BeneficiaryManager beneficiaryManager = new BeneficiaryManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private SponsorManager sponsorManager = new SponsorManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private VolunteerManager volunteerManager = new VolunteerManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        [HttpGet("{keys}", Name = "Get")]
        public string Get(string keys)
        {
            var ids_ = string.Empty;
            var header = string.Empty;
            var key1 = string.Empty;
            var key2 = string.Empty;
            if (keys.Contains(";") == true)
            {
                var splited = keys.Split(";");
                key1 = splited[0];
                key2 = splited[1];
            }

            DictionaryHelper.d.TryGetValue(key1, out ids_);
            DictionaryHelper.d.TryGetValue(key2, out header);

            var finalHeader = new string[45];
            if (header != null)
            {
                finalHeader = ControllerHelper.SplitHeader(header);
            }

            var jsonstring = "";

            var auxiliaryStrings = ids_.Split("(((");
            var properties = auxiliaryStrings[1];
            var ids = auxiliaryStrings[0].Split(",");

            if (ids[0].Contains("sponsorCSV"))
            {
                var listOfSponsors = sponsorManager.GetListOfSponsors();
                for (var i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var sponsor = listOfSponsors.AsQueryable().Where(z => z.Id == ids[i]);
                        jsonstring += JsonConvert.SerializeObject(sponsor);
                    }
                    else
                    {
                        var first = true;
                        var sponsor = listOfSponsors.AsQueryable().First(z => z.Id == ids[i]);
                        jsonstring += "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + sponsor.NameOfSponsor + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[1] + "\":" + "\"" + sponsor.Sponsorship.Date + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[5], sponsor.Contract.HasContract) + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            var ContractDetails = sponsor.Contract.NumberOfRegistration + "," + sponsor.Contract.RegistrationDate + "," + sponsor.Contract.ExpirationDate + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[6] + "\":" + "\"" + ContractDetails + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[7] + "\":" + "\"" + sponsor.ContactInformation.PhoneNumber + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[8] + "\":" + "\"" + sponsor.ContactInformation.MailAddress + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[2] + "\":" + "\"" + sponsor.Sponsorship.MoneyAmount + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[3] + "\":" + "\"" + sponsor.Sponsorship.WhatGoods + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[4] + "\":" + "\"" + sponsor.Sponsorship.GoodsAmount + "\"";
                            first = false;
                        }
                        jsonstring = jsonstring + "}]";
                    }
                }
            }
            else if (ids[0].Contains("beneficiaryCSV"))
            {
                var benefeciarycollection = beneficiaryManager.GetListOfBeneficiaries();
                for (int i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var beneficiary = benefeciarycollection.AsQueryable().Where(z => z.Id == ids[i]);
                        jsonstring = jsonstring + JsonConvert.SerializeObject(beneficiary);
                    }
                    else
                    {
                        bool first = true;
                        Beneficiary beneficiary = benefeciarycollection.AsQueryable().Where(z => z.Id == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }

                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + beneficiary.Fullname + "\"";

                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }

                            jsonstring = jsonstring + "\"" + finalHeader[1] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[1], beneficiary.Active) + "\""; ;
                            first = false;
                        }

                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }

                            jsonstring = jsonstring + "\"" + finalHeader[2] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[2], beneficiary.Canteen) + "\"";
                            first = false;
                        }
                        if (properties.Contains("Z"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[34] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[34], beneficiary.WeeklyPackage) + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[3] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[3], beneficiary.HomeDelivery) + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[4] + "\":" + "\"" + beneficiary.HomeDeliveryDriver + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[5], beneficiary.HasGDPRAgreement) + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[6] + "\":" + "\"" + beneficiary.Address + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[7] + "\":" + "\"" + beneficiary.CNP + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[8] + "\":" + "\"" + beneficiary.CI.Info + "\","; ;

                            first = false;
                        }
                        if (properties.Contains("A"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[10] + "\":" + "\"" + beneficiary.Marca.MarcaName + "\"";
                            first = false;
                        }
                        if (properties.Contains("B"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[11] + "\":" + "\"" + beneficiary.Marca.IdInvestigation + "\"";
                            first = false;
                        }
                        if (properties.Contains("C"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[12] + "\":" + "\"" + beneficiary.Marca.IdApplication + "\"";
                            first = false;
                        }
                        if (properties.Contains("D"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[13] + "\":" + "\"" + beneficiary.NumberOfPortions + "\"";
                            first = false;
                        }
                        if (properties.Contains("E"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[14] + "\":" + "\"" + beneficiary.LastTimeActive.ToLongDateString() + "\"";
                            first = false;
                        }
                        if (properties.Contains("F"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[15] + "\":" + "\"" + beneficiary.PersonalInfo.PhoneNumber + "\"";
                            first = false;
                        }
                        if (properties.Contains("G"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[16] + "\":" + "\"" + beneficiary.PersonalInfo.BirthPlace + "\"";
                            first = false;
                        }
                        if (properties.Contains("H"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[17] + "\":" + "\"" + beneficiary.PersonalInfo.Studies + "\"";
                            first = false;
                        }
                        if (properties.Contains("I"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[18] + "\":" + "\"" + beneficiary.PersonalInfo.Profession + "\"";
                            first = false;
                        }
                        if (properties.Contains("J"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[19] + "\":" + "\"" + beneficiary.PersonalInfo.Occupation + "\"";
                            first = false;
                        }
                        if (properties.Contains("K"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[20] + "\":" + "\"" + beneficiary.PersonalInfo.SeniorityInWorkField + "\"";
                            first = false;
                        }
                        if (properties.Contains("L"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[21] + "\":" + "\"" + beneficiary.PersonalInfo.HealthState + "\"";
                            first = false;
                        }
                        if (properties.Contains("M"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[22] + "\":" + "\"" + beneficiary.PersonalInfo.Disability + "\"";
                            first = false;
                        }
                        if (properties.Contains("N"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[23] + "\":" + "\"" + beneficiary.PersonalInfo.ChronicCondition + "\"";
                            first = false;
                        }
                        if (properties.Contains("O"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[24] + "\":" + "\"" + beneficiary.PersonalInfo.Addictions + "\"";
                            first = false;
                        }
                        if (properties.Contains("Z"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[25] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[25], beneficiary.PersonalInfo.HealthInsurance) + "\"";
                            first = false;
                        }
                        if (properties.Contains("P"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[26] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[26], beneficiary.PersonalInfo.HealthCard) + "\"";
                            first = false;
                        }
                        if (properties.Contains("Q"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[27] + "\":" + "\"" + beneficiary.PersonalInfo.Married + "\"";
                            first = false;
                        }
                        if (properties.Contains("R"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[28] + "\":" + "\"" + beneficiary.PersonalInfo.SpouseName + "\"";
                            first = false;
                        }
                        if (properties.Contains("S"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[29] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[29], beneficiary.PersonalInfo.HasHome) + "\"";
                            first = false;
                        }
                        if (properties.Contains("T"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[30] + "\":" + "\"" + beneficiary.PersonalInfo.HousingType + "\"";
                            first = false;
                        }
                        if (properties.Contains("U"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[31] + "\":" + "\"" + beneficiary.PersonalInfo.Income + "\"";
                            first = false;
                        }
                        if (properties.Contains("V"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[32] + "\":" + "\"" + beneficiary.PersonalInfo.Expenses + "\"";
                            first = false;
                        }
                        if (properties.Contains("W"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[33] + "\":" + "\"" + beneficiary.PersonalInfo.Gender + "\"";
                            first = false;
                        }

                        jsonstring = jsonstring + "}]";
                    }
                }
            }
            else if (ids[0].Contains("volunteerCSV"))
            {
                var volunteerscollection = volunteerManager.GetListOfVolunteers();
                for (int i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var volunteer = volunteerscollection.AsQueryable().Where(z => z.Id == ids[i]);
                        jsonstring = jsonstring + JsonConvert.SerializeObject(volunteer);
                    }
                    else
                    {
                        bool first = true;
                        Volunteer volunteer = volunteerscollection.AsQueryable().Where(z => z.Id == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + volunteer.Fullname + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[1] + "\":" + "\"" + volunteer.Birthdate + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[14] + "\":" + "\"" + volunteer.Address + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[3] + "\":" + "\"" + volunteer.Gender + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[4] + "\":" + "\"" + volunteer.DesiredWorkplace + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + volunteer.CNP + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[6] + "\":" + "\"" + volunteer.FieldOfActivity + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[7] + "\":" + "\"" + volunteer.Occupation + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[15] + "\":" + "\"" + volunteer.CI.HasId + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[16] + "\":" + "\"" + volunteer.CI.Info + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[17] + "\":" + "\"" + volunteer.CI.ExpirationDate + "\"";

                            first = false;
                        }
                        if (properties.Contains("A"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[9] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[9], volunteer.InActivity) + "\"";
                            first = false;
                        }
                        if (properties.Contains("B"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[10] + "\":" + "\"" + volunteer.HourCount + "\"";
                            first = false;
                        }
                        if (properties.Contains("C"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[19] + "\":" + "\"" + volunteer.ContactInformation.PhoneNumber + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[20] + "\":" + "\"" + volunteer.ContactInformation.MailAddress + "\"";
                            first = false;
                        }
                        if (properties.Contains("D"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[21] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[22], volunteer.AdditionalInfo.HasDrivingLicense) + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[22] + "\":" + "\"" + ControllerHelper.GetAnswer(finalHeader[23], volunteer.AdditionalInfo.HasCar) + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[23] + "\":" + "\"" + volunteer.AdditionalInfo.Remark + "\"";

                            first = false;
                        }
                        jsonstring = jsonstring + "}]";
                    }
                }
            }
            else if (ids[0].Contains("eventCSV"))
            {
                var listOfEvents = eventManager.GetListOfEvents();
                for (var i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var @event = listOfEvents.AsQueryable().Where(z => z.Id == ids[i]);
                        jsonstring += JsonConvert.SerializeObject(@event);
                    }
                    else
                    {
                        var first = true;
                        var @event = listOfEvents.AsQueryable().First(z => z.Id == ids[i]);
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + @event.NameOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[1] + "\":" + "\"" + @event.PlaceOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[2] + "\":" + "\"" + @event.DateOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[3] + "\":" + "\"" + @event.TypeOfActivities + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[4] + "\":" + "\"" + @event.TypeOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + @event.Duration + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[6] + "\":" + "\"" + @event.AllocatedVolunteers + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring += ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[7] + "\":" + "\"" + @event.AllocatedSponsors + "\"";
                            first = false;
                        }

                        jsonstring += "}]";
                    }
                }
            }
            jsonstring = jsonstring.Replace("][", ",");

            return jsonstring;
        }
    }
}