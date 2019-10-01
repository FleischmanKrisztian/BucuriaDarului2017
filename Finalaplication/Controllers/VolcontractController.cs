using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Finalaplication.Models;
using Finalaplication.App_Start;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Finalaplication.Controllers
{
    public class VolcontractController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Volcontract> volcontractcollection;
        private IMongoCollection<Volunteer> volunteercollection;
        private readonly IMongoCollection<Settings> settingcollection;

        public VolcontractController()
        {
            dbcontext = new MongoDBContext();
            volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
            volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            settingcollection = dbcontext.database.GetCollection<Settings>("Settings");
        }

        [HttpGet]
        public IActionResult Index(string idofvol)
        {           
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable().ToList();
            Volunteer vol = volunteercollection.AsQueryable().FirstOrDefault(z => z.VolunteerID == idofvol);
            volcontracts = volcontracts.Where(z => z.OwnerID.ToString() == idofvol).ToList();
            ViewBag.nameofvol = vol.Firstname + " " + vol.Lastname; 
            ViewBag.idofvol = idofvol;
            try
            {
                Settings sett = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));

                if (sett.Env == "offline")
                    ViewBag.env = "offline";
                else
                    ViewBag.env = "online";
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(volcontracts);
        }

        public ActionResult ContractExp()
        {
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
            try
            {
                Settings sett = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));

                if (sett.Env == "offline")
                    ViewBag.env = "offline";
                else
                    ViewBag.env = "online";
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(volcontracts);
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            ViewBag.idofvol = id;
            try
            {
                Settings sett = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));

                if (sett.Env == "offline")
                    ViewBag.env = "offline";
                else
                    ViewBag.env = "online";
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Volcontract volcontract, string idofvol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Volunteer vol = volunteercollection.AsQueryable().FirstOrDefault(z => z.VolunteerID == idofvol);
                    volcontract.ExpirationDate =  volcontract.ExpirationDate.AddDays(1);
                    volcontract.RegistrationDate = volcontract.RegistrationDate.AddDays(1);
                    volcontract.Birthdate = vol.Birthdate;
                    volcontract.Firstname = vol.Firstname;
                    volcontract.Lastname = vol.Lastname;
                    volcontract.CNP = vol.CNP;
                    volcontract.CIseria = vol.CIseria;
                    volcontract.CINr = vol.CINr;
                    volcontract.CIEliberat = vol.CIEliberat;
                    volcontract.Nrtel = vol.ContactInformation.PhoneNumber;
                    volcontract.Hourcount = vol.HourCount;
                    volcontract.CIeliberator = vol.CIeliberator;
                    volcontract.Address = vol.Address.Country + ", " + vol.Address.City + ", " + vol.Address.Street + ", " + vol.Address.Number;
                    volcontract.OwnerID = idofvol;
                    volcontractcollection.InsertOne(volcontract);
                    try
                    {
                        Settings sett = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));

                        if (sett.Env == "offline")
                            ViewBag.env = "offline";
                        else
                            ViewBag.env = "online";
                    }
                    catch
                    {
                        return RedirectToAction("Localserver");
                    }
                    return RedirectToAction("Index", new { idofvol });
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes! ");
            }
            return View(volcontract);
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            var contract = volcontractcollection.AsQueryable<Volcontract>().SingleOrDefault(x => x.ContractID == id);
            try
            {
                Settings sett = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));

                if (sett.Env == "offline")
                    ViewBag.env = "offline";
                else
                    ViewBag.env = "online";
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(contract);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var contractid = new ObjectId(id);
            var contract = volcontractcollection.AsQueryable<Volcontract>().SingleOrDefault(x => x.ContractID == id);
            try
            {
                Settings sett = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));

                if (sett.Env == "offline")
                    ViewBag.env = "offline";
                else
                    ViewBag.env = "online";
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(contract);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, string idofvol)
        {
            volcontractcollection.DeleteOne(Builders<Volcontract>.Filter.Eq("_id", ObjectId.Parse(id)));

            return RedirectToAction("Index", new { idofvol });
        }
    }
}