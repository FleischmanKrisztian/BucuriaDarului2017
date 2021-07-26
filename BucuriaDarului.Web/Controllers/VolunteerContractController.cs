using BucuriaDarului.Contexts.VolunteerContractContexts;
using BucuriaDarului.Gateway.VolunteerContractGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Mvc;

namespace BucuriaDarului.Web.Controllers
{
    public class VolunteerContractController : Controller
    {
        [HttpGet]
        public IActionResult Index(string idOfVolunteer)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var contractsMainDisplayIndexContext = new VolunteerContractIndexDisplayContext(new VolunteerContractIndexDisplayGateway());
            var model = contractsMainDisplayIndexContext.Execute(new VolunteerContractsMainDisplayIndexRequest(idOfVolunteer, nrOfDocs));
            return View(model);
        }

        public ActionResult ContractExp()
        {
            var contractExpirationContext = new VolunteerContractsExpirationContext(new VolunteerContractExpirationGateway());
            var contracts = contractExpirationContext.Execute();
            return View(contracts);
        }

        [HttpGet]
        public ActionResult Create(string idOfVolunteer, string message)
        {
            ViewBag.message = message;
            ViewBag.idOfVol = idOfVolunteer;
            return View();
        }

        [HttpPost]
        public ActionResult Create(VolunteerContractCreateRequest request)
        {
            var contractCreateContext = new VolunteerContractCreateContext(new VolunteerContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(request);

            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { idOfVolunteer = request.OwnerId, message = contractCreateResponse.Message });
            }
            return RedirectToAction("Index", new { idOfVolunteer = request.OwnerId });
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            var model = SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string id,string message)
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
                return RedirectToAction("Delete", new { id = request.ContractId, message="Error!This document couldn't be deleted!" });
            return RedirectToAction("Index", new { idOfVolunteer = response.VolunteerId });
        }
    }
}