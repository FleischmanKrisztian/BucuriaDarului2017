using BucuriaDarului.Contexts.VolunteerAdditionalContractContexts;
using BucuriaDarului.Contexts.VolunteerContractContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.VolunteerContractGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BucuriaDarului.Web.Controllers
{
    public class VolunteerAdditionalContractController : Controller
    {
        private readonly IStringLocalizer<VolunteerAdditionalContractController> _localizer;

        public VolunteerAdditionalContractController(IStringLocalizer<VolunteerAdditionalContractController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var contractsMainDisplayIndexContext = new VolunteerAdditionalContractIndexDisplayContext(new VolunteerAdditionalContractIndexDisplayGateway());
            var model = contractsMainDisplayIndexContext.Execute(new VolunteerAdditionalContractsMainDisplayIndexRequest(id, nrOfDocs));
            model.Query = HttpContext.Session.GetString("queryString");
            return View(model);
        }

        //public ActionResult ContractExp()
        //{
        //    var nrOfDays = UniversalFunctions.GetNumberOfDaysBeforeExpiration(TempData);
        //    var contractExpirationContext = new VolunteerContractsExpirationContext(new VolunteerContractExpirationGateway());
        //    var contracts = contractExpirationContext.Execute(nrOfDays);
        //    return View(contracts);
        //}

        [HttpGet]
        public ActionResult Create(string idOfVolunteerContract, string message)
        {
            var volunteerContract = SingleVolunteerContractReturnerGateway.GetVolunteerContract(idOfVolunteerContract);
            ViewBag.NameOfVolunteer = volunteerContract.Fullname;
            ViewBag.NumberOfRegistration= volunteerContract.NumberOfRegistration;
            ViewBag.message = message;
                ViewBag.idOfContract = idOfVolunteerContract;
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(VolunteerAdditionalContractCreateRequest request)
        {
            var contractCreateContext = new VolunteerAdditionalContractCreateContext(new VolunteerAdditionalContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(request);
            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { idOfVolunteerContract = request.ContractID, message = @_localizer[contractCreateResponse.Message] });
            }
            return RedirectToAction("Index", new { id = request.ContractID});
        }

        [HttpGet]
        public ActionResult Print(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleVolunteerAdditionalContractReturnerGateway.GetAdditionalContract(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Print(IFormFile Files, string fileName, string id)
        {
            var printContext = new VolunteerAdditionalContractPrintContext(new VolunteerAdditionalContractPrintGateway());
            var response = new VolunteerAdditionalContractPrintResponse();
            if (Files == null)
            {
                var defaultPath = Environment.GetEnvironmentVariable(Constants.BUCURIA_DARULUI_PATH) + "\\ContractTemplates\\ACT ADITIONAL prelungire perioada.docx";
                if (System.IO.File.Exists(defaultPath))
                {
                    using var stream = System.IO.File.Open(defaultPath, FileMode.Open);
                    response = printContext.Execute(stream, id, fileName);
                }
                else
                {
                    response.Message = "No Template has been chosen, and the default template has been moved to an unknown location!";
                    response.IsValid = false;
                }
            }
            else
                response = printContext.Execute(Files.OpenReadStream(), id, fileName);
            if (response.IsValid)
            {
                response.Message = @_localizer["Contract exported successfully!"];
                return DownloadFile(response.Stream, response.FileName);
            }
            return RedirectToAction("Print", new { id, message = response.Message });
        }

        public FileContentResult DownloadFile(MemoryStream data, string fileName)
        {
            return File(data.ToArray(), "application/docx", fileName);
        }

        [HttpGet]
        public ActionResult Delete(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleVolunteerAdditionalContractReturnerGateway.GetAdditionalContract(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(VolunteerAdditionalContractDeleteRequest request)
        {
            var additionalContractDeleteContext = new VolunteerAdditionalContractDeleteContext(new VolunteerAdditionalContractDeleteGateway());
            var response = additionalContractDeleteContext.Execute(request);
            if (!response.IsValid)
                return RedirectToAction("Delete", new { id = request.Id, message = @_localizer["Error! This document couldn't be deleted!"] });
            return RedirectToAction("Index", new { id = response.ContractID});
        }
    }
}
