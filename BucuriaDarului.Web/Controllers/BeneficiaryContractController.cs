using BucuriaDarului.Contexts.BeneficiaryContractContexts;
using BucuriaDarului.Gateway.BeneficiaryContractGateways;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
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
            model.Query = HttpContext.Session.GetString("queryString");
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
            var beneficiary = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(idOfBeneficiary);
            ViewBag.NameOfBeneficiary = beneficiary.Fullname;
            if (string.IsNullOrEmpty(beneficiary.CNP))
            {
                ViewBag.message = "Missing CNP information for this beneficiary! Please fill in all the data necessary for contract creation.";
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

        public ActionResult Print(IFormFile Files, string fileName, string id)
        {
            var printContext = new BeneficiaryContractPrintContext(new BeneficiaryContractPrintGateway());
            var response = printContext.Execute(Files.OpenReadStream(), id, fileName);
            if (response.IsValid)
                return GetPhysicalFileResult(response.DownloadPath);
            else
                return RedirectToAction("Print", new { id = id, message = response.Message });

        }
        public PhysicalFileResult GetPhysicalFileResult(string path)
        {

            return new PhysicalFileResult(path, "application/doc");
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
                return RedirectToAction("DeleteDisplay", new { id = contractId, message = response });
            return RedirectToAction("Index", new { idOfBeneficiary = ownerId});
        }
    }
}