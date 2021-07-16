using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.ExcelPrinterGateways;
using Newtonsoft.Json;
using System.Linq;

namespace BucuriaDarului.Contexts.ExcelPrinterControllerContext
{
    public class ExcelPrinterContext
    {
        private readonly IExcelPrinterGateway dataGateway;

        public ExcelPrinterContext(IExcelPrinterGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public string Execute(string dataString, string header)
        {
            var finalHeader = SplitHeader(header);
            var jsonString = string.Empty;
            var properties = GetProperties(dataString);
            var ids = GetIds(dataString);

            if (ids[0].Contains("sponsorCSV"))
                jsonString=GetSponsorsJson(properties,ids,finalHeader);
            if (ids[0].Contains("beneficiaryCSV"))
                jsonString=GetBeneficiariesJson( properties, ids, finalHeader);
            if (ids[0].Contains("volunteerCSV"))
                jsonString=GetVolunteersJson( properties, ids, finalHeader);
            if (ids[0].Contains("eventCSV"))
                jsonString=GetEventsJson( properties, ids, finalHeader);

            return jsonString;
        }

        public string GetAnswer(string finalHeader, bool toBeCompared)
        {
            var result = string.Empty;
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

        public static string[] SplitHeader(string header)
        {
            var splitHeader = header.Split(",");

            return splitHeader;
        }

        public string GetProperties(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var properties = auxiliaryStrings[1];
            return properties;
        }

        public string[] GetIds(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var ids = auxiliaryStrings[0].Split(",");
            return ids;
        }

        public string GetSponsorsJson( string properties, string[] ids, string[] finalHeader)
        {
            string jsonString = string.Empty;
            var listOfSponsors = dataGateway.GetListOfSponsors();
            for (var i = 1; i < ids.Length; i++)
            {
                if (properties.Contains("0"))
                {
                    var sponsor = listOfSponsors.AsQueryable().Where(z => z.Id == ids[i]);
                    jsonString += JsonConvert.SerializeObject(sponsor);
                }
                else
                {
                    var first = true;
                    var sponsor = listOfSponsors.AsQueryable().First(z => z.Id == ids[i]);
                    jsonString += "[{";
                    if (properties.Contains("1"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[0] + "\":" + "\"" + sponsor.NameOfSponsor + "\"";
                        first = false;
                    }
                    if (properties.Contains("2"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[1] + "\":" + "\"" + sponsor.Sponsorship.Date + "\"";
                        first = false;
                    }
                    if (properties.Contains("3"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[5] + "\":" + "\"" + GetAnswer(finalHeader[5], sponsor.Contract.HasContract) + "\"";
                        first = false;
                    }
                    if (properties.Contains("4"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        var ContractDetails = sponsor.Contract.NumberOfRegistration + "," + sponsor.Contract.RegistrationDate + "," + sponsor.Contract.ExpirationDate + ",";
                        jsonString = jsonString + "\"" + finalHeader[6] + "\":" + "\"" + ContractDetails + "\"";
                        first = false;
                    }
                    if (properties.Contains("5"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[7] + "\":" + "\"" + sponsor.ContactInformation.PhoneNumber + "\"";
                        first = false;
                    }
                    if (properties.Contains("6"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[8] + "\":" + "\"" + sponsor.ContactInformation.MailAddress + "\"";
                        first = false;
                    }
                    if (properties.Contains("7"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[2] + "\":" + "\"" + sponsor.Sponsorship.MoneyAmount + "\"";
                        first = false;
                    }
                    if (properties.Contains("8"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[3] + "\":" + "\"" + sponsor.Sponsorship.WhatGoods + "\"";
                        first = false;
                    }
                    if (properties.Contains("9"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[4] + "\":" + "\"" + sponsor.Sponsorship.GoodsAmount + "\"";
                        first = false;
                    }
                    jsonString = jsonString + "}]";
                }
            }

            jsonString = jsonString.Replace("][", ",");
            return jsonString;
        }

        public string GetBeneficiariesJson(string properties, string[] ids, string[] finalHeader)
        {
            string jsonString = string.Empty;
            var benefeciarycollection = dataGateway.GetListOfBeneficiaries();
            for (int i = 1; i < ids.Length; i++)
            {
                if (properties.Contains("0"))
                {
                    var beneficiary = benefeciarycollection.AsQueryable().Where(z => z.Id == ids[i]);
                    jsonString = jsonString + JsonConvert.SerializeObject(beneficiary);
                }
                else
                {
                    bool first = true;
                    Beneficiary beneficiary = benefeciarycollection.AsQueryable().Where(z => z.Id == ids[i]).First();
                    jsonString = jsonString + "[{";
                    if (properties.Contains("1"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }

                        jsonString = jsonString + "\"" + finalHeader[0] + "\":" + "\"" + beneficiary.Fullname + "\"";

                        first = false;
                    }
                    if (properties.Contains("2"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }

                        jsonString = jsonString + "\"" + finalHeader[1] + "\":" + "\"" + GetAnswer(finalHeader[1], beneficiary.Active) + "\""; ;
                        first = false;
                    }

                    if (properties.Contains("3"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }

                        jsonString = jsonString + "\"" + finalHeader[2] + "\":" + "\"" + GetAnswer(finalHeader[2], beneficiary.Canteen) + "\"";
                        first = false;
                    }
                    if (properties.Contains("Z"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[34] + "\":" + "\"" + GetAnswer(finalHeader[34], beneficiary.WeeklyPackage) + "\"";
                        first = false;
                    }
                    if (properties.Contains("4"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[3] + "\":" + "\"" + GetAnswer(finalHeader[3], beneficiary.HomeDelivery) + "\"";
                        first = false;
                    }
                    if (properties.Contains("5"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[4] + "\":" + "\"" + beneficiary.HomeDeliveryDriver + "\"";
                        first = false;
                    }
                    if (properties.Contains("6"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[5] + "\":" + "\"" + GetAnswer(finalHeader[5], beneficiary.HasGDPRAgreement) + "\"";
                        first = false;
                    }
                    if (properties.Contains("7"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[6] + "\":" + "\"" + beneficiary.Address + "\"";
                        first = false;
                    }
                    if (properties.Contains("8"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[7] + "\":" + "\"" + beneficiary.CNP + "\"";
                        first = false;
                    }
                    if (properties.Contains("9"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[8] + "\":" + "\"" + beneficiary.CI.Info + "\","; ;

                        first = false;
                    }
                    if (properties.Contains("A"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[10] + "\":" + "\"" + beneficiary.Marca.MarcaName + "\"";
                        first = false;
                    }
                    if (properties.Contains("B"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[11] + "\":" + "\"" + beneficiary.Marca.IdInvestigation + "\"";
                        first = false;
                    }
                    if (properties.Contains("C"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[12] + "\":" + "\"" + beneficiary.Marca.IdApplication + "\"";
                        first = false;
                    }
                    if (properties.Contains("D"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[13] + "\":" + "\"" + beneficiary.NumberOfPortions + "\"";
                        first = false;
                    }
                    if (properties.Contains("E"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[14] + "\":" + "\"" + beneficiary.LastTimeActive.ToLongDateString() + "\"";
                        first = false;
                    }
                    if (properties.Contains("F"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[15] + "\":" + "\"" + beneficiary.PersonalInfo.PhoneNumber + "\"";
                        first = false;
                    }
                    if (properties.Contains("G"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[16] + "\":" + "\"" + beneficiary.PersonalInfo.BirthPlace + "\"";
                        first = false;
                    }
                    if (properties.Contains("H"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[17] + "\":" + "\"" + beneficiary.PersonalInfo.Studies + "\"";
                        first = false;
                    }
                    if (properties.Contains("I"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[18] + "\":" + "\"" + beneficiary.PersonalInfo.Profession + "\"";
                        first = false;
                    }
                    if (properties.Contains("J"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[19] + "\":" + "\"" + beneficiary.PersonalInfo.Occupation + "\"";
                        first = false;
                    }
                    if (properties.Contains("K"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[20] + "\":" + "\"" + beneficiary.PersonalInfo.SeniorityInWorkField + "\"";
                        first = false;
                    }
                    if (properties.Contains("L"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[21] + "\":" + "\"" + beneficiary.PersonalInfo.HealthState + "\"";
                        first = false;
                    }
                    if (properties.Contains("M"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[22] + "\":" + "\"" + beneficiary.PersonalInfo.HealthState + "\"";
                        first = false;
                    }
                    if (properties.Contains("N"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[23] + "\":" + "\"" + beneficiary.PersonalInfo.ChronicCondition + "\"";
                        first = false;
                    }
                    if (properties.Contains("O"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[24] + "\":" + "\"" + beneficiary.PersonalInfo.Addictions + "\"";
                        first = false;
                    }
                    if (properties.Contains("Z"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[25] + "\":" + "\"" + GetAnswer(finalHeader[25], beneficiary.PersonalInfo.HealthInsurance) + "\"";
                        first = false;
                    }
                    if (properties.Contains("P"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[26] + "\":" + "\"" + GetAnswer(finalHeader[26], beneficiary.PersonalInfo.HealthCard) + "\"";
                        first = false;
                    }
                    if (properties.Contains("Q"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[27] + "\":" + "\"" + beneficiary.PersonalInfo.Married + "\"";
                        first = false;
                    }
                    if (properties.Contains("R"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[28] + "\":" + "\"" + beneficiary.PersonalInfo.SpouseName + "\"";
                        first = false;
                    }
                    if (properties.Contains("S"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[29] + "\":" + "\"" + GetAnswer(finalHeader[29], beneficiary.PersonalInfo.HasHome) + "\"";
                        first = false;
                    }
                    if (properties.Contains("T"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[30] + "\":" + "\"" + beneficiary.PersonalInfo.HousingType + "\"";
                        first = false;
                    }
                    if (properties.Contains("U"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[31] + "\":" + "\"" + beneficiary.PersonalInfo.Income + "\"";
                        first = false;
                    }
                    if (properties.Contains("V"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[32] + "\":" + "\"" + beneficiary.PersonalInfo.Expenses + "\"";
                        first = false;
                    }
                    if (properties.Contains("W"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[33] + "\":" + "\"" + beneficiary.PersonalInfo.Gender + "\"";
                        first = false;
                    }

                    jsonString = jsonString + "}]";
                }
            }
            jsonString = jsonString.Replace("][", ",");
            return jsonString;
        }

        public string GetVolunteersJson( string properties, string[] ids, string[] finalHeader)
        {
            string jsonString = string.Empty;
            var volunteerscollection = dataGateway.GetListOfVolunteers();
            for (int i = 1; i < ids.Length; i++)
            {
                if (properties.Contains("0"))
                {
                    var volunteer = volunteerscollection.AsQueryable().Where(z => z.Id == ids[i]);
                    jsonString = jsonString + JsonConvert.SerializeObject(volunteer);
                }
                else
                {
                    bool first = true;
                    Volunteer volunteer = volunteerscollection.AsQueryable().Where(z => z.Id == ids[i]).First();
                    jsonString = jsonString + "[{";
                    if (properties.Contains("1"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[0] + "\":" + "\"" + volunteer.Fullname + "\"";
                        first = false;
                    }
                    if (properties.Contains("2"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[1] + "\":" + "\"" + volunteer.Birthdate + "\"";
                        first = false;
                    }
                    if (properties.Contains("3"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[14] + "\":" + "\"" + volunteer.Address + "\"";
                        first = false;
                    }
                    if (properties.Contains("4"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[3] + "\":" + "\"" + volunteer.Gender + "\"";
                        first = false;
                    }
                    if (properties.Contains("5"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[4] + "\":" + "\"" + volunteer.DesiredWorkplace + "\"";
                        first = false;
                    }
                    if (properties.Contains("6"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[5] + "\":" + "\"" + volunteer.CNP + "\"";
                        first = false;
                    }
                    if (properties.Contains("7"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[6] + "\":" + "\"" + volunteer.FieldOfActivity + "\"";
                        first = false;
                    }
                    if (properties.Contains("8"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[7] + "\":" + "\"" + volunteer.Occupation + "\"";
                        first = false;
                    }
                    if (properties.Contains("9"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[15] + "\":" + "\"" + volunteer.CI.HasId + "\"";
                        jsonString = jsonString + ",";
                        jsonString = jsonString + "\"" + finalHeader[16] + "\":" + "\"" + volunteer.CI.Info + "\"";
                        jsonString = jsonString + ",";
                        jsonString = jsonString + "\"" + finalHeader[17] + "\":" + "\"" + volunteer.CI.ExpirationDate + "\"";

                        first = false;
                    }
                    if (properties.Contains("A"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[9] + "\":" + "\"" + GetAnswer(finalHeader[9], volunteer.InActivity) + "\"";
                        first = false;
                    }
                    if (properties.Contains("B"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[10] + "\":" + "\"" + volunteer.HourCount + "\"";
                        first = false;
                    }
                    if (properties.Contains("C"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[19] + "\":" + "\"" + volunteer.ContactInformation.PhoneNumber + "\"";
                        jsonString = jsonString + ",";
                        jsonString = jsonString + "\"" + finalHeader[20] + "\":" + "\"" + volunteer.ContactInformation.MailAddress + "\"";
                        first = false;
                    }
                    if (properties.Contains("D"))
                    {
                        if (!first)
                        {
                            jsonString = jsonString + ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[21] + "\":" + "\"" + GetAnswer(finalHeader[22], volunteer.AdditionalInfo.HasDrivingLicense) + "\"";
                        jsonString = jsonString + ",";
                        jsonString = jsonString + "\"" + finalHeader[22] + "\":" + "\"" + GetAnswer(finalHeader[23], volunteer.AdditionalInfo.HasCar) + "\"";
                        jsonString = jsonString + ",";
                        jsonString = jsonString + "\"" + finalHeader[23] + "\":" + "\"" + volunteer.AdditionalInfo.Remark + "\"";

                        first = false;
                    }
                    jsonString = jsonString + "}]";
                }
            }
            jsonString = jsonString.Replace("][", ",");
            return jsonString;
        }

        public string GetEventsJson( string properties, string[] ids, string[] finalHeader)
        {
            string jsonString = string.Empty;
            var listOfEvents = dataGateway.GetListOfEvents();
            for (var i = 1; i < ids.Length; i++)
            {
                if (properties.Contains("0"))
                {
                    var @event = listOfEvents.AsQueryable().Where(z => z.Id == ids[i]);
                    jsonString += JsonConvert.SerializeObject(@event);
                }
                else
                {
                    var first = true;
                    var @event = listOfEvents.AsQueryable().First(z => z.Id == ids[i]);
                    jsonString = jsonString + "[{";
                    if (properties.Contains("1"))
                    {
                        jsonString = jsonString + "\"" + finalHeader[0] + "\":" + "\"" + @event.NameOfEvent + "\"";
                        first = false;
                    }
                    if (properties.Contains("2"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[1] + "\":" + "\"" + @event.PlaceOfEvent + "\"";
                        first = false;
                    }
                    if (properties.Contains("3"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[2] + "\":" + "\"" + @event.DateOfEvent + "\"";
                        first = false;
                    }
                    if (properties.Contains("4"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[3] + "\":" + "\"" + @event.TypeOfActivities + "\"";
                        first = false;
                    }
                    if (properties.Contains("5"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[4] + "\":" + "\"" + @event.TypeOfEvent + "\"";
                        first = false;
                    }
                    if (properties.Contains("6"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[5] + "\":" + "\"" + @event.Duration + "\"";
                        first = false;
                    }
                    if (properties.Contains("7"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[6] + "\":" + "\"" + @event.AllocatedVolunteers + "\"";
                        first = false;
                    }
                    if (properties.Contains("8"))
                    {
                        if (!first)
                        {
                            jsonString += ",";
                        }
                        jsonString = jsonString + "\"" + finalHeader[7] + "\":" + "\"" + @event.AllocatedSponsors + "\"";
                        first = false;
                    }

                    jsonString += "}]";
                }
            }

            jsonString = jsonString.Replace("][", ",");
            return jsonString;
        }
    }
}