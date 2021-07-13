using BucuriaDarului.Gateway.BeneficiaryContractGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Mvc;

namespace BucuriaDarului.Web.Controllers
{
    public class BeneficiaryContractController : Controller
    {
        [HttpGet]
        public IActionResult Index(string idOfBeneficiary)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var contractsMainDisplayIndexContext = new BeneficiaryContractIndexDisplayContext(new BeneficiaryContractIndexDisplayGateway());
            var model = contractsMainDisplayIndexContext.Execute(new BeneficiaryContractsMainDisplayIndexRequest(idOfBeneficiary, nrOfDocs));
            return View(model);
        }

        public ActionResult ContractExp()
        {
            var contractExpirationContext = new BeneficiaryContractsExpirationContext(new BeneficiaryContractExpirationGateway());
            var contracts = contractExpirationContext.Execute();
            return View(contracts);
        }

        [HttpGet]
        public ActionResult Create(string idOfBeneficiary, string message)
        {
            ViewBag.message = message;
            ViewBag.idOfVol = idOfBeneficiary;
            return View();
        }

        [HttpPost]
        public ActionResult Create(BeneficiaryContractCreateRequest request)
        {
            var contractCreateContext = new BeneficiaryContractCreateContext(new BeneficiaryContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(request);

            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { idOfBeneficiary = request.OwnerID, message = contractCreateResponse.Message });
            }
            return RedirectToAction("Index", new { idOfBeneficiary = request.OwnerID });
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            var model = SingleBeneficiaryContractReturnerGateway.GetBeneficiaryContract(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleBeneficiaryContractReturnerGateway.GetBeneficiaryContract(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(BeneficiaryContractDeleteRequest request)
        {
            var beneficiaryContractDeleteContext = new BeneficiaryContractDeleteContext(new BeneficiaryContractDeleteGateway());
            var response = beneficiaryContractDeleteContext.Execute(request);
            if (!response.IsValid)
                return RedirectToAction("Delete", new { id = request.ContractId, message = "Error!This document couldn't be deleted!" });
            return RedirectToAction("Index", new { idOfBeneficiary = response.BeneficiaryId });
        }
    }
}
}