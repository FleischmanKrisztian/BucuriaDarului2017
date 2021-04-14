using Finalaplication.App_Start;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Linq;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private MongoDBContextLocal dBContextLocal;
        private IMongoCollection<Volcontract> volcontractcollection;

        [HttpGet("{id}", Name = "GetExcelparams")]
        public string Get(string id)
        {
            string jsonstring;
            dBContextLocal = new MongoDBContextLocal();
            volcontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Volcontract>("Contracts");
            var volcontract = volcontractcollection.AsQueryable().Where(z => z.ContractID == id);
            jsonstring = JsonConvert.SerializeObject(volcontract);
            return jsonstring;
        }
    }
}