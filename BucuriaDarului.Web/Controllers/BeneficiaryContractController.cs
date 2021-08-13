using BucuriaDarului.Contexts.BeneficiaryContractContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.BeneficiaryContractGateways;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;

namespace BucuriaDarului.Web.Controllers
{
    public class BeneficiaryContractController : Controller
    {
        private readonly IStringLocalizer<BeneficiaryContractController> _localizer;

        public BeneficiaryContractController(IStringLocalizer<BeneficiaryContractController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string idOfBeneficiary)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var contractsMainDisplayIndexContext = new BeneficiaryContractIndexDisplayContext(new BeneficiaryContractIndexDisplayGateway());
            var model = contractsMainDisplayIndexContext.Execute(new BeneficiaryContractsMainDisplayIndexRequest(idOfBeneficiary, nrOfDocs));
            model.Query = HttpContext.Session.GetString("queryString");
            return View(model);
        }

        public ActionResult ContractExp()
        {
            var nrOfDays = UniversalFunctions.GetNumberOfDaysBeforeExpiration(TempData);
            var contractExpirationContext = new BeneficiaryContractsExpirationContext(new BeneficiaryContractExpirationGateway());
            var contracts = contractExpirationContext.Execute(nrOfDays);
            return View(contracts);
        }

        [HttpGet]
        public ActionResult Create(string idOfBeneficiary, string message)
        {
            var beneficiary = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(idOfBeneficiary);
            ViewBag.NameOfBeneficiary = beneficiary.Fullname;
            if (string.IsNullOrEmpty(beneficiary.CNP))
            {
                ViewBag.message = @_localizer["Missing CNP information for this beneficiary! Please fill in all the data necessary for contract creation."];
                ViewBag.idOfBeneficiary = idOfBeneficiary;
            }
            else
            {
                ViewBag.message = message;
                ViewBag.idOfBeneficiary = idOfBeneficiary;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(BeneficiaryContractCreateRequest request)
        {
            var contractCreateContext = new BeneficiaryContractCreateContext(new BeneficiaryContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(request);

            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { idOfBeneficiary = request.OwnerID, message = @_localizer[contractCreateResponse.Message] });
            }
            return RedirectToAction("Index", new { idOfBeneficiary = request.OwnerID });
        }

        [HttpGet]
        public ActionResult Print(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleBeneficiaryContractReturnerGateway.GetBeneficiaryContract(id);

            return View(model);
        }

        public ActionResult Print(IFormFile Files, string fileName, string id, string optionValue, string otherOptionValue)
        {
            var printContext = new BeneficiaryContractPrintContext(new BeneficiaryContractPrintGateway());
            var response = new BeneficiaryContractPrintResponse();
            if (Files == null)
            {
                var defaultPath = Environment.GetEnvironmentVariable(Constants.BUCURIA_DARULUI_PATH) +
                                  "\\ContractTemplates\\BeneficiaryContract.docx";
                using var stream = System.IO.File.Open(defaultPath, FileMode.Open);
                if (stream == Stream.Null)
                {
                    response.Message =
                        _localizer["No Template has been chosen, and the default template has been moved to an unknown location!"];
                    response.IsValid = false;
                }
                else
                {
                    response = printContext.Execute(stream, id, optionValue, otherOptionValue);
                }
            }
            else
                response = printContext.Execute(Files.OpenReadStream(), id, optionValue, otherOptionValue);
            if (response.IsValid)
                return DownloadFile(response.Stream, response.FileName);
            return RedirectToAction("Print", new { id = id, message = @_localizer[response.Message] });
        }

        public FileContentResult DownloadFile(MemoryStream data, string fileName)
        {
            return File(data.ToArray(), "application/docx", fileName);
        }

        [HttpGet]
        public ActionResult DeleteDisplay(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleBeneficiaryContractReturnerGateway.GetBeneficiaryContract(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string contractId, string ownerId)
        {
            var beneficiaryContractDeleteContext = new BeneficiaryContractDeleteContext(new BeneficiaryContractDeleteGateway());
            var response = beneficiaryContractDeleteContext.Execute(contractId, ownerId);
            if (response.Contains("Error"))
                return RedirectToAction("DeleteDisplay", new { id = contractId, message = @_localizer[response] });
            return RedirectToAction("Index", new { idOfBeneficiary = ownerId });
        }
    }
}