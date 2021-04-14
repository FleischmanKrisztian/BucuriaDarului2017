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
    public class BeneficiaryValuesController : ControllerBase
    {
        private MongoDBContextLocal dBContextLocal;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;

        [HttpGet("{id}", Name = "Getbeneficiary")]
        public string Get(string id)
        {
            string jsonstring;
            dBContextLocal = new MongoDBContextLocal();
            beneficiarycontractcollection = dBContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var beneficiarycontract = beneficiarycontractcollection.AsQueryable().Where(z => z.ContractID == id);
            jsonstring = JsonConvert.SerializeObject(beneficiarycontract);
            return jsonstring;
        }
    }
}