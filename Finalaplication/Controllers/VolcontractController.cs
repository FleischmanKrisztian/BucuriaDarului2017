using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
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
        private MongoDBContext dbcontext;
        private IMongoCollection<Volcontract> volcontractcollection;
        private IMongoCollection<Volunteer> volunteercollection;

        public VolcontractController()
        {
            try
            {
                dbcontext = new MongoDBContext();
                volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
                volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            }
            catch { }
        }

        [HttpGet]
        public IActionResult Index(string idofvol)
        {
            try
            {
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volcontract> volcontracts = volcontractcollection.AsQueryable().ToList();
                Volunteer vol = volunteercollection.AsQueryable().FirstOrDefault(z => z.VolunteerID == idofvol);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
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
                        Volunteer vol = volunteercollection.AsQueryable().FirstOrDefault(z => z.VolunteerID == idofvol);
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
                        volcontractcollection.InsertOne(volcontract);
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
                var contract = volcontractcollection.AsQueryable<Volcontract>().SingleOrDefault(x => x.ContractID == id);
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
                var contract = volcontractcollection.AsQueryable<Volcontract>().SingleOrDefault(x => x.ContractID == id);
                return View(contract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, string idofvol)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                volcontractcollection.DeleteOne(Builders<Volcontract>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index", new { idofvol });
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}