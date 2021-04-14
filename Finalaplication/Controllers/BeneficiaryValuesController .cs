using Finalaplication.DatabaseManager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Finalaplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryValuesController : ControllerBase
    {
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager();

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