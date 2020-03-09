using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelprinterController :ControllerBase
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Volunteer> volunteerscollection;
        private IMongoCollection<Event> eventscollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private IMongoCollection<Beneficiary> benefeciarycollection;
         
// GET: api/Excelprinter
       [HttpGet("{key}", Name = "Get")]
        public string Get( string key)
        {
            string key1 = string.Empty;
            string key2 = string.Empty;
            if (key.Contains(";") == true)
            {
               string[] splited= key.Split(";");
                key1 = splited[1];
                key2 = splited[2];
            }
            string id;
            DictionaryHelper dictionary;
            DictionaryHelper.d.TryGetValue(key1,out dictionary);

            id = dictionary.Ids.ToString();
            if (id!= "")
            { DictionaryHelper.d.Remove(key1); }
            string header;
            DictionaryHelper.d.TryGetValue(key2, out dictionary);
            header = dictionary.Ids.ToString();
            if (header != "")
            { DictionaryHelper.d.Remove(key2); }
            ControllerHelper helper = new ControllerHelper();
            string[] finalHeader = helper.SplitedHeader(header);
            dbcontext = new MongoDBContext();
            string jsonstring = "";
            id = id.Replace("\"", "");
            string[] ids = id.Split(",");
            if (ids[0].Contains("sponsors"))
            {
                string properties = ids[ids.Length - 1].Substring(27);
                ids[ids.Length - 1] = ids[ids.Length - 1].Substring(0, 24);
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                for (int i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var sponsor = sponsorcollection.AsQueryable().Where(z => z.SponsorID == ids[i]);
                        jsonstring = jsonstring + JsonConvert.SerializeObject(sponsor);
                        var aux = jsonstring.IndexOf("SponsorID");
                        jsonstring = jsonstring.Remove(aux - 1, 39);
                    }
                    else
                    {
                        bool first = true;
                        Sponsor sponsor = sponsorcollection.AsQueryable().Where(z => z.SponsorID == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + sponsor.NameOfSponsor + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
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
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + helper.GetAnswer(finalHeader[5], sponsor.Contract.HasContract) + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[8] + "\":" + "\"" + sponsor.ContactInformation.MailAdress + "\"";
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
            else if (ids[0].Contains("beneficiaries"))
            {
                string properties = ids[ids.Length - 1].Substring(27);
                ids[ids.Length - 1] = ids[ids.Length - 1].Substring(0, 24);
                benefeciarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
                for (int i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var beneficiary = benefeciarycollection.AsQueryable().Where(z => z.BeneficiaryID == ids[i]);
                        jsonstring = jsonstring + JsonConvert.SerializeObject(beneficiary);
                        var aux = jsonstring.IndexOf("BeneficiaryID");
                        jsonstring = jsonstring.Remove(aux - 1, 43);
                    }
                    else
                    {
                        bool first = true;
                        Beneficiary beneficiary = benefeciarycollection.AsQueryable().Where(z => z.BeneficiaryID == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                           
                            jsonstring = jsonstring + "\""+ finalHeader[0] + "\":" + "\"" + beneficiary.Fullname + "\"";
                           
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            
                            jsonstring = jsonstring + "\""+ finalHeader[1]+"\":" + "\"" + helper.GetAnswer(finalHeader[1], beneficiary.Active) + "\""; ;
                            first = false;
                        }
                        
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            
                            jsonstring = jsonstring + "\"" + finalHeader[2] + "\":" + "\"" + helper.GetAnswer(finalHeader[2], beneficiary.Canteen) + "\""; 
                            first = false;
                        }
                        if (properties.Contains("Z"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[34] + "\":" + "\"" + helper.GetAnswer(finalHeader[34], beneficiary.Weeklypackage) + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[3] + "\":" + "\"" + helper.GetAnswer(finalHeader[3], beneficiary.HomeDelivery) + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + helper.GetAnswer(finalHeader[5], beneficiary.HasGDPRAgreement)+ "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[6] + "\":" + "\"" + beneficiary.Adress + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[8] + "\":" + "\"" + beneficiary.CI.CIinfo + "\","; ;

                            jsonstring = jsonstring + "\"" + finalHeader[9] + "\":" + "\"" + beneficiary.CI.CIEliberat + "\"";

                            first = false;
                        }
                        if (properties.Contains("A"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[10] + "\":" + "\"" + beneficiary.Marca.marca + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[12] + "\":" + "\"" + beneficiary.Marca.IdAplication + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[14] + "\":" + "\"" + beneficiary.LastTimeActiv.ToLongDateString() + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[18] + "\":" + "\"" + beneficiary.PersonalInfo.Profesion + "\"";
                            first = false;
                        }
                        if (properties.Contains("J"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[19] + "\":" + "\"" + beneficiary.PersonalInfo.Ocupation + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[22] + "\":" + "\"" + beneficiary.PersonalInfo.Disalility + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[25] + "\":" + "\"" + helper.GetAnswer(finalHeader[25], beneficiary.PersonalInfo.HealthInsurance) + "\"";
                            first = false;
                        }
                        if (properties.Contains("P"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[26] + "\":" + "\"" + helper.GetAnswer(finalHeader[26], beneficiary.PersonalInfo.HealthCard) + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[29] + "\":" + "\"" + helper.GetAnswer(finalHeader[29], beneficiary.PersonalInfo.HasHome)+ "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[32] + "\":" + "\"" + beneficiary.PersonalInfo.Expences + "\"";
                            first = false;
                        }
                        if (properties.Contains("W"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[33]+ "\":" + "\"" + beneficiary.PersonalInfo.Gender + "\"";
                            first = false;
                        }

                        jsonstring = jsonstring + "}]";

                      
                    }
                }
            }
            else if (ids[0].Contains("volunteers"))
            {
                string properties = ids[ids.Length - 1].Substring(27);
                ids[ids.Length - 1] = ids[ids.Length - 1].Substring(0, 24);
                volunteerscollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                for (int i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var volunteer = volunteerscollection.AsQueryable().Where(z => z.VolunteerID == ids[i]);
                        jsonstring = jsonstring + JsonConvert.SerializeObject(volunteer);
                        var aux = jsonstring.IndexOf("VolunteerID");
                        jsonstring = jsonstring.Remove(aux - 1, 41);
                    }
                    else
                    {
                        bool first = true;
                        Volunteer volunteer = volunteerscollection.AsQueryable().Where(z => z.VolunteerID == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + volunteer.Firstname + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[13] + "\":" + "\"" + volunteer.Lastname + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[1]+"\":" + "\""  + volunteer.Birthdate + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[14] + "\":" + "\"" + volunteer.Address.District + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[15] + "\":\"" + volunteer.Address.City + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[16] + "\":\"" + volunteer.Address.Street + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[17] + "\":\"" + volunteer.Address.Number + "\"";
                            
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[3] + "\":" + "\"" + volunteer.Gender + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[4] + "\":" + "\"" + volunteer.Desired_workplace + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[5]+"\":" + "\"" + volunteer.CNP + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[6] + "\":" + "\"" + volunteer.Field_of_activity + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[7] + "\":" + "\"" + volunteer.Occupation + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring +  "\"" + finalHeader[18] + "\":" + "\"" + volunteer.CIseria + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[19] + "\":" + "\"" + volunteer.CINr + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[20] + "\":" + "\"" + volunteer.CIEliberat + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[21] + "\":" + "\"" + volunteer.CIeliberator + "\"";
                            first = false;
                        }
                        if (properties.Contains("A"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[9]+"\":" + "\"" + helper.GetAnswer(finalHeader[9],volunteer.InActivity) + "\"";
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
                            jsonstring = jsonstring + "\"" + finalHeader[20] + "\":" + "\"" + volunteer.ContactInformation.PhoneNumber + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[21] + "\":" + "\"" + volunteer.ContactInformation.MailAdress + "\"";
                            first = false;
                        }
                        if (properties.Contains("D"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\""+ finalHeader[22] + "\":" + "\"" + helper.GetAnswer(finalHeader[22], volunteer.Additionalinfo.HasDrivingLicence) + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[23] + "\":" + "\"" + helper.GetAnswer(finalHeader[23], volunteer.Additionalinfo.HasCar) + "\"";
                            jsonstring = jsonstring + ",";
                            jsonstring = jsonstring + "\"" + finalHeader[24] + "\":" + "\"" + volunteer.Additionalinfo.Remark + "\"";
                               
                            first = false;
                        }
                        jsonstring = jsonstring + "}]";
                    }
                }
            }
            else if (ids[0].Contains("events"))
            {
                string properties = ids[ids.Length - 1].Substring(27);
                ids[ids.Length - 1] = ids[ids.Length - 1].Substring(0, 24);
                eventscollection = dbcontext.database.GetCollection<Event>("Events");
                for (int i = 1; i < ids.Length; i++)
                {
                    if (properties.Contains("0"))
                    {
                        var eventt = eventscollection.AsQueryable().Where(z => z.EventID == ids[i]);
                        jsonstring = jsonstring + JsonConvert.SerializeObject(eventt);
                        var aux = jsonstring.IndexOf("EventID");
                        jsonstring = jsonstring.Remove(aux - 1, 37);
                    }
                    else
                    {
                        bool first = true;
                        Event eventt = eventscollection.AsQueryable().Where(z => z.EventID == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[0] + "\":" + "\"" + eventt.NameOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[1] + "\":" + "\"" + eventt.PlaceOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[2] + "\":" + "\"" + eventt.DateOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[3] + "\":" + "\"" + eventt.TypeOfActivities + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[4] + "\":" + "\"" + eventt.TypeOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[5] + "\":" + "\"" + eventt.Duration + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[6] + "\":" + "\"" + eventt.AllocatedVolunteers + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"" + finalHeader[7] + "\":" + "\"" + eventt.AllocatedSponsors + "\"";
                            first = false;
                        }

                        jsonstring = jsonstring + "}]";
                    }
                }
            }
            jsonstring = jsonstring.Replace("][", ",");
            
            
            return jsonstring;
        }
    }
}