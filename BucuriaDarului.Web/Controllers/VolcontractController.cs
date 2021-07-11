using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolcontractHelpers;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using BucuriaDarului.Gateway.VolunteerGateways;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.VolContractGateways;
using BucuriaDarului.Contexts.VolunteerContractContext;

namespace Finalaplication.Controllers
{
    public class VolcontractController : Controller
    {
        
        [HttpGet]
        public IActionResult Index(string idOfVolunteer)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);

            //List<Volcontract> volcontracts = volContractManager.GetListOfVolunteersContracts();
            //    Volunteer vol = volunteerManager.GetOneVolunteer(idofvol);
            //    volcontracts = volcontracts.Where(z => z.OwnerID.ToString() == idofvol).ToList();
            //    ViewBag.nameofvol = vol.Fullname;
            //    ViewBag.idofvol = idofvol;
          
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
        public ActionResult Create(VolunteerContract volunteerContract, string idOfVolunteer)
        {
            var contractCreateContext = new VolunteerContractCreateContext(new VolunteerContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(volunteerContract,  idOfVolunteer);

            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { idOfVolunteer = idOfVolunteer, message = contractCreateResponse.Message });
            }
            return RedirectToAction("Index", new { idOfVolunteer = idOfVolunteer });

        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            var model = SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
                return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
                var model= SingleVolunteerContractReturnerGateway.GetVolunteerContract(id);
                return View(model);
            
        }

        [HttpPost]
        public ActionResult Delete(string id, string idOfVol)
        {
                VolunteerContractDeleteGateway.Delete(id);
                return RedirectToAction("Index", new { idOfVolunteer= idOfVol });
            
        }
    }
}