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
    public class ValuesController : ControllerBase
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Volcontract> volcontractcollection;

        [HttpGet("{id}", Name = "GetExcelparams")]
        public string Get(string id)
        {
            string jsonstring;
            dbcontext = new MongoDBContext();
            volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
            var volcontract = volcontractcollection.AsQueryable().Where(z => z.ContractID == id);
            jsonstring = JsonConvert.SerializeObject(volcontract);
            return jsonstring;
        }
    }
}