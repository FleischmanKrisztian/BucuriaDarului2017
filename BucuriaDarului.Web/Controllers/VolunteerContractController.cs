using BucuriaDarului.Contexts.VolunteerContractContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;

namespace BucuriaDarului.Web.Controllers
{
    public class VolunteerContractController : Controller
    {
        private readonly IStringLocalizer<VolunteerContractController> _localizer;

        public VolunteerContractController(IStringLocalizer<VolunteerContractController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string idOfVolunteer)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var contractsMainDisplayIndexContext = new VolunteerContractIndexDisplayContext(new VolunteerContractIndexDisplayGateway());
            var model = contractsMainDisplayIndexContext.Execute(new VolunteerContractsMainDisplayIndexRequest(idOfVolunteer, nrOfDocs));
            model.Query = HttpContext.Session.GetString("queryString");
            return View(model);
        }

        public ActionResult ContractExp()
        {
            var nrOfDays = UniversalFunctions.GetNumberOfDaysBeforeExpiration(TempData);
            var contractExpirationContext = new VolunteerContractsExpirationContext(new VolunteerContractExpirationGateway());
            var contracts = contractExpirationContext.Execute(nrOfDays);
            return View(contracts);
        }

        [HttpGet]
        public ActionResult Create(string idOfVolunteer, string message)
        {
            var volunteer = SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
            ViewBag.NameOfVolunteer = volunteer.Fullname;
            if (string.IsNullOrEmpty(volunteer.CNP))
            {
                ViewBag.message = @_localizer["Missing CNP information for this volunteer! Please fill in all the data necessary for contract creation."];
                ViewBag.idOfVol = idOfVolunteer;
            }
            else
            {
                ViewBag.message = message;
                ViewBag.idOfVol = idOfVolunteer;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(VolunteerContractCreateRequest request)
        {
            var contractCreateContext = new VolunteerContractCreateContext(new VolunteerContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(request);

            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { idOfVolunteer = request.OwnerId, message = @_localizer[contractCreateResponse.Message] });
            }
            return RedirectToAction("Index", new { idOfVolunteer = request.OwnerId });
        }

        [HttpGet]
        public ActionResult Print(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Print(IFormFile Files, string fileName, string id)
        {
            var printContext = new VolunteerContractPrintContext(new VolunteerContractPrintGateway());
            var response = new VolunteerContractPrintResponse();
            if (Files == null)
            {
                var defaultPath = Environment.GetEnvironmentVariable(Constants.BUCURIA_DARULUI_PATH) + "\\ContractTemplates\\VolunteerContract.docx";
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
            var model = SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(VolunteerContractDeleteRequest request)
        {
            var volunteerContractDeleteContext = new VolunteerContractDeleteContext(new VolunteerContractDeleteGateway());
            var response = volunteerContractDeleteContext.Execute(request);
            if (!response.IsValid)
                return RedirectToAction("Delete", new { id = request.ContractId, message = @_localizer["Error! This document couldn't be deleted!"] });
            return RedirectToAction("Index", new { idOfVolunteer = response.VolunteerId });
        }
    }
}