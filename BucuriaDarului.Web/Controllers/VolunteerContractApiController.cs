using BucuriaDarului.Contexts.VolunteerContractContexts;
using Microsoft.AspNetCore.Mvc;

namespace BucuriaDarului.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerContractApiController : ControllerBase
    {
        [HttpGet("{id}", Name = "GetExcelParams")]
        public string Get(string id)
        {
            var volunteerContractApiContext = new VolunteerContractApiContext();
            var response = volunteerContractApiContext.Execute(id);
            return response;
        }
    }
}