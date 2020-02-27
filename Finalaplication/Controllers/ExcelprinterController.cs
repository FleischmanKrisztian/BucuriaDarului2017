using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelprinterController : ControllerBase
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Volunteer> volunteerscollection;
        private IMongoCollection<Event> eventscollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private IMongoCollection<Beneficiary> benefeciarycollection;
        //private readonly IHttpContextAccessor _contextAccessor;
        //public ExcelprinterController(IHttpContextAccessor contextAccessor)
        //{
        //    _contextAccessor = contextAccessor;
        //}
        // GET: api/Excelprinter



        [HttpGet("{key}", Name = "Get")]
        public string Get( string key)
        {
            string id;
            // string id = HttpContext.Session.GetString(key);
            //string id = HttpContext.Session.GetObject(key);

            // string id = _contextAccessor.HttpContext.Session.GetString(key); 
            // string id = HttpHelper.HttpContext.Session.GetString(key);
            DictionaryHelper dictionary;
                DictionaryHelper.d.TryGetValue(key,out dictionary);
            id = dictionary.Ids.ToString();
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
                            jsonstring = jsonstring + "\"NameOfSponsor\":" + "\"" + sponsor.NameOfSponsor + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Date\":" + "\"" + sponsor.Sponsorship.Date + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"HasContract \":" + "\"" + sponsor.Contract.HasContract + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            var ContractDetails = sponsor.Contract.NumberOfRegistration + "," + sponsor.Contract.RegistrationDate + "," + sponsor.Contract.ExpirationDate + ",";
                            jsonstring = jsonstring + "\"ContractDetails \":" + "\"" + ContractDetails + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"PhoneNumber\":" + "\"" + sponsor.ContactInformation.PhoneNumber + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"MailAdress \":" + "\"" + sponsor.ContactInformation.MailAdress + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"MoneyAmount\":" + "\"" + sponsor.Sponsorship.MoneyAmount + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"WhatGoods\":" + "\"" + sponsor.Sponsorship.WhatGoods + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"GoodsAmount\":" + "\"" + sponsor.Sponsorship.GoodsAmount + "\"";
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
                            jsonstring = jsonstring + "\"Fullname\":" + "\"" + beneficiary.Fullname + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"In Activity\":" + "\"" + beneficiary.Active + "\""; ;
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Canteen\":" + "\"" + beneficiary.Canteen + "\""; ;
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Home Delivery\":" + "\"" + beneficiary.HomeDelivery + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Home Delivery Driver\":" + "\"" + beneficiary.HomeDeliveryDriver + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Has GDPR Agreement\":" + "\"" + beneficiary.HasGDPRAgreement + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Address\":" + "\"" + beneficiary.Adress + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"CNP\":" + "\"" + beneficiary.CNP + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"CIseria\":" + "\"" + beneficiary.CI.CIinfo + "\","; ;

                            jsonstring = jsonstring + "\"CIEliberat\":" + "\"" + beneficiary.CI.CIEliberat + "\"";

                            first = false;
                        }
                        if (properties.Contains("A"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Marca\":" + "\"" + beneficiary.Marca.marca + "\"";
                            first = false;
                        }
                        if (properties.Contains("B"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"IdInvestigation\":" + "\"" + beneficiary.Marca.IdInvestigation + "\"";
                            first = false;
                        }
                        if (properties.Contains("C"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"IdAplication\":" + "\"" + beneficiary.Marca.IdAplication + "\"";
                            first = false;
                        }
                        if (properties.Contains("D"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Number Of Portions\":" + "\"" + beneficiary.NumberOfPortions + "\"";
                            first = false;
                        }
                        if (properties.Contains("E"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Last Time Activ\":" + "\"" + beneficiary.LastTimeActiv + "\"";
                            first = false;
                        }
                        if (properties.Contains("F"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"PhoneNumber\":" + "\"" + beneficiary.PersonalInfo.PhoneNumber + "\"";
                            first = false;
                        }
                        if (properties.Contains("G"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Birth Place\":" + "\"" + beneficiary.PersonalInfo.BirthPlace + "\"";
                            first = false;
                        }
                        if (properties.Contains("H"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Studies\":" + "\"" + beneficiary.PersonalInfo.Studies + "\"";
                            first = false;
                        }
                        if (properties.Contains("I"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Profesion\":" + "\"" + beneficiary.PersonalInfo.Profesion + "\"";
                            first = false;
                        }
                        if (properties.Contains("J"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Occupation\":" + "\"" + beneficiary.PersonalInfo.Ocupation + "\"";
                            first = false;
                        }
                        if (properties.Contains("K"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Seniority In WorkField\":" + "\"" + beneficiary.PersonalInfo.SeniorityInWorkField + "\"";
                            first = false;
                        }
                        if (properties.Contains("L"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Health State\":" + "\"" + beneficiary.PersonalInfo.HealthState + "\"";
                            first = false;
                        }
                        if (properties.Contains("M"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Disalility\":" + "\"" + beneficiary.PersonalInfo.Disalility + "\"";
                            first = false;
                        }
                        if (properties.Contains("N"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Chronic Condition\":" + "\"" + beneficiary.PersonalInfo.ChronicCondition + "\"";
                            first = false;
                        }
                        if (properties.Contains("O"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Addictions\":" + "\"" + beneficiary.PersonalInfo.Addictions + "\"";
                            first = false;
                        }
                        if (properties.Contains("Z"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"HealthInsurance\":" + "\"" + beneficiary.PersonalInfo.HealthInsurance + "\"";
                            first = false;
                        }
                        if (properties.Contains("P"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Health Card\":" + "\"" + beneficiary.PersonalInfo.HealthCard + "\"";
                            first = false;
                        }
                        if (properties.Contains("Q"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Married\":" + "\"" + beneficiary.PersonalInfo.Married + "\"";
                            first = false;
                        }
                        if (properties.Contains("R"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Spouse Name \":" + "\"" + beneficiary.PersonalInfo.SpouseName + "\"";
                            first = false;
                        }
                        if (properties.Contains("S"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Has Home\":" + "\"" + beneficiary.PersonalInfo.HasHome + "\"";
                            first = false;
                        }
                        if (properties.Contains("T"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Housing Type\":" + "\"" + beneficiary.PersonalInfo.HousingType + "\"";
                            first = false;
                        }
                        if (properties.Contains("U"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Income\":" + "\"" + beneficiary.PersonalInfo.Income + "\"";
                            first = false;
                        }
                        if (properties.Contains("V"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Expences\":" + "\"" + beneficiary.PersonalInfo.Expences + "\"";
                            first = false;
                        }
                        if (properties.Contains("W"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Gender\":" + "\"" + beneficiary.PersonalInfo.Gender + "\"";
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
                            jsonstring = jsonstring + "\"Firstname\":" + "\"" + volunteer.Firstname + "\",\"Lastname\":\"" + volunteer.Lastname + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Birthdate\":" + "\"" + volunteer.Birthdate + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Address\":{" + "\"District\":" + "\"" + volunteer.Address.District + "\",\"City\":\"" + volunteer.Address.City + "\",\"Street\":\"" + volunteer.Address.Street + "\",\"Number\":\"" + volunteer.Address.Number + "\"}";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Gender\":" + "\"" + volunteer.Gender + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Desired_workplace\":" + "\"" + volunteer.Desired_workplace + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"CNP\":" + "\"" + volunteer.CNP + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Field_of_activity\":" + "\"" + volunteer.Field_of_activity + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Occupation\":" + "\"" + volunteer.Occupation + "\"";
                            first = false;
                        }
                        if (properties.Contains("9"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"CIseria\":" + "\"" + volunteer.CIseria + "\",";
                            jsonstring = jsonstring + "\"CINr\":" + "\"" + volunteer.CINr + "\",";
                            jsonstring = jsonstring + "\"CIEliberat\":" + "\"" + volunteer.CIEliberat + "\",";
                            jsonstring = jsonstring + "\"CIeliberator\":" + "\"" + volunteer.CIeliberator + "\"";
                            first = false;
                        }
                        if (properties.Contains("A"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"InActivity\":" + "\"" + volunteer.InActivity + "\"";
                            first = false;
                        }
                        if (properties.Contains("B"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"HourCount\":" + "\"" + volunteer.HourCount + "\"";
                            first = false;
                        }
                        if (properties.Contains("C"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"ContactInformation\":{" + "\"PhoneNumber\":\"" + volunteer.ContactInformation.PhoneNumber + "\",\"MailAdress\":\"" + volunteer.ContactInformation.MailAdress + "\"}";
                            first = false;
                        }
                        if (properties.Contains("D"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Additionalinfo\":{" + "\"HasDrivingLicence\":\"" + volunteer.Additionalinfo.HasDrivingLicence + "\",\"HasCar\":\"" + volunteer.Additionalinfo.HasCar + "\",\"Remark\":\"" + volunteer.Additionalinfo.Remark + "\"}";
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
                            jsonstring = jsonstring + "\"NameOfEvent\":" + "\"" + eventt.NameOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("2"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"PlaceOfEvent\":" + "\"" + eventt.PlaceOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("3"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"DateOfEvent\":" + "\"" + eventt.DateOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("4"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"TypeOfActivities\":" + "\"" + eventt.TypeOfActivities + "\"";
                            first = false;
                        }
                        if (properties.Contains("5"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"TypeOfEvent\":" + "\"" + eventt.TypeOfEvent + "\"";
                            first = false;
                        }
                        if (properties.Contains("6"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"Duration\":" + "\"" + eventt.Duration + "\"";
                            first = false;
                        }
                        if (properties.Contains("7"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"AllocatedVolunteers\":" + "\"" + eventt.AllocatedVolunteers + "\"";
                            first = false;
                        }
                        if (properties.Contains("8"))
                        {
                            if (!first)
                            {
                                jsonstring = jsonstring + ",";
                            }
                            jsonstring = jsonstring + "\"AllocatedSponsors \":" + "\"" + eventt.AllocatedSponsors + "\"";
                            first = false;
                        }

                        jsonstring = jsonstring + "}]";
                    }
                }
            }
            jsonstring = jsonstring.Replace("][", ",");
            if(jsonstring!="")
            { DictionaryHelper.d.Remove(key); }
            
            return jsonstring;
        }
    }
}