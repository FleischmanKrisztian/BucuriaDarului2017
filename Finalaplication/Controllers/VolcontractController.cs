using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolcontractHelpers;
using Finalaplication.DatabaseHandler;
using Finalaplication.DatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class VolcontractController : Controller
    {
        private VolunteerManager volunteerManager = new VolunteerManager();
        private VolContractManager volContractManager = new VolContractManager();

        [HttpGet]
        public IActionResult Index(string idofvol)
        {
            try
            {
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volcontract> volcontracts = volContractManager.GetListOfVolunteersContracts();
                Volunteer vol =volunteerManager.GetOneVolunteer(idofvol);
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
        public ActionResult Create(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ViewBag.idofvol = id;
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Create(Volcontract volcontract, string idofvol)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    if (ModelState.IsValid)
                    {
                        Volunteer vol = volunteerManager.GetOneVolunteer(idofvol);
                        volcontract.ExpirationDate = volcontract.ExpirationDate.AddDays(1);
                        volcontract.RegistrationDate = volcontract.RegistrationDate.AddDays(1);
                        volcontract.Birthdate = vol.Birthdate;
                        volcontract.Fullname = vol.Fullname;
                        volcontract.CNP = vol.CNP;
                        volcontract.CIseria = vol.CIseria;
                        volcontract.CINr = vol.CINr;
                        volcontract.CIEliberat = vol.CIEliberat;
                        volcontract.Nrtel = vol.ContactInformation.PhoneNumber;
                        volcontract.Hourcount = vol.HourCount;
                        volcontract.CIeliberator = vol.CIeliberator;
                        string address = string.Empty;
                        if (vol.Address.District != null && vol.Address.District != "-")
                        { address = vol.Address.District; }
                        if (vol.Address.City != null && vol.Address.City != "-")
                        { address = address + "," + vol.Address.City; }
                        if (vol.Address.Street != null && vol.Address.Street != "-")
                        { address = vol.Address.District; }
                        if (vol.Address.Number != null && vol.Address.Number != "-")
                        { address = address + "," + vol.Address.City; }
                        volcontract.Address = address;
                        volcontract.OwnerID = idofvol;
                        volContractManager.AddVolunteerContractToDB(volcontract);
                        return RedirectToAction("Index", new { idofvol });
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes! ");
                }
                return View(volcontract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var contractid = new ObjectId(id);
                var contract = volContractManager.GetVolunteerContract(id);
                return View(contract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Delete(string id, string idofvol)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                volContractManager.DeleteAVolContract(id);
                return RedirectToAction("Index", new { idofvol });
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}