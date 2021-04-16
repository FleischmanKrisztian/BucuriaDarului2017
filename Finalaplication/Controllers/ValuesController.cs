using Finalaplication.App_Start;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private VolContractManager volContractManager = new VolContractManager();

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