using BucuriaDarului.Contexts.VolunteerContractContext;
using BucuriaDarului.Gateway.VolContractGateways;
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
        [HttpGet("{id}", Name = "GetExcelparams")]
        public string Get(string id)
        {
            var volunteerContextContext = new ValuesControllerContext();
            var respons = volunteerContextContext.Execute(id);
            return respons;
        }
    }
}