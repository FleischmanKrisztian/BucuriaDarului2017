using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finalaplication.App_Start;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryValuesController : ControllerBase
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;

        // GET: api/Values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string blabla = "";
            dbcontext = new MongoDBContext();
            beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");

            var beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();
            foreach (var beneficiarycontract in beneficiarycontracts)
            {
                blabla = blabla + JsonConvert.SerializeObject(beneficiarycontract);
            }
            return new string[] { blabla };
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Getbeneficiary")]
        public string Get(string id)
        {
            string jsonstring;
            dbcontext = new MongoDBContext();
            beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            var beneficiarycontract = beneficiarycontractcollection.AsQueryable().Where(z => z.ContractID == id);
            jsonstring = JsonConvert.SerializeObject(beneficiarycontract);
            return jsonstring ;
        }
    }
}
