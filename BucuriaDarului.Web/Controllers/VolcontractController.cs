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
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL);

        private VolunteerManager volunteerManager = new VolunteerManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private VolContractManager volContractManager = new VolContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        [HttpGet]
        public IActionResult Index(string idofvol)
        {
            try
            {
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                List<Volcontract> volcontracts = volContractManager.GetListOfVolunteersContracts();
                Volunteer vol = volunteerManager.GetOneVolunteer(idofvol);
                volcontracts = volcontracts.Where(z => z.OwnerID.ToString() == idofvol).ToList();
                ViewBag.nameofvol = vol.Fullname;
                ViewBag.idofvol = idofvol;
                return View(volcontracts);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult ContractExp()
        {
            try
            {
                List<Volcontract> volcontracts = volContractManager.GetListOfVolunteersContracts();
                volcontracts = VolcontractFunctions.GetExpiringContracts(volcontracts);
                return View(volcontracts);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult Create(string volunteerId,string message)
        {
            ViewBag.message = message;
            ViewBag.idOfVol = volunteerId;
                return View();
        }

        [HttpPost]
        public ActionResult Create(VolunteerContractCreateRequest request)
        {
            var contractCreateContext = new VolunteerContractCreateContext(new VolunteerContractCreateGateway());
            var contractCreateResponse = contractCreateContext.Execute(request);

            if (!contractCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { volunteerId = request.VolunteerId, message = contractCreateResponse.Message });
            }
            return RedirectToAction("Index", new { volunteerId = request.VolunteerId });

            
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            try
            {
                var contract = volContractManager.GetVolunteerContract(id);
                return View(contract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
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
                volContractManager.DeleteAVolContract(id);
                return RedirectToAction("Index", new { idOfVol });
            
        }
    }
}