﻿using BucuriaDarului.Contexts.BeneficiaryContractContexts;
using Microsoft.AspNetCore.Mvc;

namespace BucuriaDarului.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryContractApiController : ControllerBase
    {
        //Had to change Name :|
        [HttpGet("{id}", Name = "GetExcelParameters")]
        public string Get(string id)
        {
            var beneficiaryContractApiContext = new BeneficiaryContractApiContext();
            var response = beneficiaryContractApiContext.Execute(id);
            return response;
        }
    }
}