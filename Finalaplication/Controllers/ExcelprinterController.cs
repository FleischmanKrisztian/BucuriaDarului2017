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
            string[] ids = id.Split(",");
            if (ids[0]=="sponsors")
            {
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                for (int i = 1; i < ids.Length; i++)
                {
                    var sponsor = sponsorcollection.AsQueryable().Where(z => z.SponsorID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(sponsor);
                }
            }
            else if (ids[0] == "beneficiaries")
            {
                benefeciarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
                for (int i = 1; i < ids.Length; i++)
                {
                    var beneficiary = benefeciarycollection.AsQueryable().Where(z => z.BeneficiaryID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(beneficiary);
                }
            }
            else if (ids[0] == "volunteers")
            {
                volunteerscollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                for(int i=1;i<ids.Length;i++)
                {
                    var volunteer = volunteerscollection.AsQueryable().Where(z => z.VolunteerID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(volunteer);
                }
            }
            else if (ids[0] == "events")
            {
                eventscollection = dbcontext.database.GetCollection<Event>("Events");
                for (int i = 1; i < ids.Length; i++)
                {
                    var eventt = eventscollection.AsQueryable().Where(z => z.EventID == ids[i]);
                    jsonstring = jsonstring + JsonConvert.SerializeObject(eventt);
                }
            }
            return jsonstring;
        }
    }
}
