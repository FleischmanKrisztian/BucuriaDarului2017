using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Finalaplication.Models;
using Finalaplication.App_Start;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System;
using ReflectionIT.Mvc.Paging;
using System.IO;

namespace Finalaplication.Controllers
{
    public class VolcontractController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Volcontract> volcontractcollection;
        private IMongoCollection<Volunteer> volunteercollection;

        public VolcontractController()
        {
            dbcontext = new MongoDBContext();
            volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
            volunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
        }

        [HttpGet]
        public IActionResult Index(string idofvol)
        {
            
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable().ToList();
            Volunteer vol = volunteercollection.AsQueryable().FirstOrDefault(z => z.VolunteerID == idofvol);
            volcontracts = volcontracts.Where(z => z.OwnerID.ToString() == idofvol).ToList();
            ViewBag.nameofvol = vol.Firstname + " " + vol.Lastname; 
            ViewBag.idofvol = idofvol;
            return View(volcontracts);
        }

        public ActionResult ContractExp()
        {
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();

            return View(volcontracts);
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            ViewBag.idofvol = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Volcontract volcontract, string idofvol)
        {
            try
            {
                if (ModelState.IsValid)
                {
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

        public ActionResult Print(string id)
        {
            var contract = volcontractcollection.AsQueryable<Volcontract>().SingleOrDefault(x => x.ContractID == id);
            var volunteer = volunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == contract.OwnerID);

            ViewBag.volunteerName = volunteer.Firstname + " " + volunteer.Lastname;
            ViewBag.volunteerCNP = volunteer.CNP;
            ViewBag.volunteerBd = volunteer.Birthdate;
            ViewBag.volunteerAddress = volunteer.Address.Country + " " + volunteer.Address.City + " " +volunteer.Address.Street + " " + volunteer.Address.Number;

            return View(contract);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var contractid = new ObjectId(id);
            var contract = volcontractcollection.AsQueryable<Volcontract>().SingleOrDefault(x => x.ContractID == id);
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