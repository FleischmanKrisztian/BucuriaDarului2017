using Finalaplication.LocalDatabaseManager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL);

        private VolContractManager volContractManager = new VolContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        [HttpGet("{id}", Name = "GetExcelparams")]
        public string Get(string id)
        {
            string jsonstring;
            var volcontract = volContractManager.GetVolunteerContract(id);
            jsonstring = JsonConvert.SerializeObject(volcontract);
            return jsonstring;
        }
    }
}