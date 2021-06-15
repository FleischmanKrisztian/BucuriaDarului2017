using Finalaplication.LocalDatabaseManager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryValuesController : ControllerBase
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL);

        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        [HttpGet("{id}", Name = "Getbeneficiary")]
        public string Get(string id)
        {
            string jsonstring;
            var beneficiarycontract = beneficiaryContractManager.GetBeneficiaryContract(id);
            jsonstring = JsonConvert.SerializeObject(beneficiarycontract);
            return jsonstring;
        }
    }
}