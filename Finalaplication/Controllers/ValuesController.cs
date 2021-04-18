using Finalaplication.App_Start;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.VolMongoConstants.DATABASE_NAME_LOCAL);

        private static MongoDBContext MongoDBContextLocal = new MongoDBContext(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        private VolContractManager volContractManager = new VolContractManager(MongoDBContextLocal);

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