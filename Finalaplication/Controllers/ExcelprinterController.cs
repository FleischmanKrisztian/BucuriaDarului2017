using Finalaplication.App_Start;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        // GET: api/Excelprinter

        [HttpGet("{id}", Name = "Get")]
        public string Get(string id)
        {
            dbcontext = new MongoDBContext();
            string jsonstring="";
            id = id.Replace("\"","");
            string[] ids = id.Split(",");
            if (ids[0].Contains("sponsors"))
            {
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                for (int i = 1; i < ids.Length; i++)
                {
                    var sponsor = sponsorcollection.AsQueryable().Where(z => z.SponsorID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(sponsor);
                }
            }
            else if (ids[0].Contains("beneficiaries"))
            {
                benefeciarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
                for (int i = 1; i < ids.Length; i++)
                {
                    var beneficiary = benefeciarycollection.AsQueryable().Where(z => z.BeneficiaryID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(beneficiary);
                }
            }
            else if (ids[0].Contains("volunteers"))
            {
                string properties = ids[ids.Length-1].Substring(27);
                ids[ids.Length-1] = ids[ids.Length-1].Substring(0, 24);
                volunteerscollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                for(int i=1;i<ids.Length;i++)
                {
                    if(properties.Contains("0"))
                    {
                    var volunteer = volunteerscollection.AsQueryable().Where(z => z.VolunteerID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(volunteer);
                    }
                    else
                    {
                        bool first = true;
                        Volunteer volunteer = volunteerscollection.AsQueryable().Where(z => z.VolunteerID == ids[i]).First();
                        jsonstring = jsonstring + "[{";
                        if (properties.Contains("1"))
                        {
                            if(!first)
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
                eventscollection = dbcontext.database.GetCollection<Event>("Events");
                for (int i = 1; i < ids.Length; i++)
                {
                    var eventt = eventscollection.AsQueryable().Where(z => z.EventID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(eventt);
                }
            }
            jsonstring = jsonstring.Replace("][", ",");
            return jsonstring;
        }
    }
}
