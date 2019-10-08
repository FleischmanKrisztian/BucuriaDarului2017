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
    public class BeneficiarycontractController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;
        private IMongoCollection<Beneficiary> beneficiarycollection;

        public BeneficiarycontractController()
        {
            dbcontext = new MongoDBContext();
            beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
        }

        [HttpGet]
        public IActionResult Index(string idofbeneficiary)
        {           
            List<Beneficiarycontract> benficiarycontracts = beneficiarycontractcollection.AsQueryable().ToList();
            Beneficiary benenficiary  = beneficiarycollection.AsQueryable().FirstOrDefault(z => z.BeneficiaryID == idofbeneficiary);
            benficiarycontracts = benficiarycontracts.Where(z => z.OwnerID.ToString() == idofbeneficiary).ToList();
            ViewBag.nameofbeneficiary = benenficiary.Firstname + " " + benenficiary.Lastname; 
            ViewBag.idofbeneficiary = idofbeneficiary;
            return View(benficiarycontracts);
        }

        public ActionResult ContractExp()
        {
            List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

            return View(beneficiarycontracts);
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            ViewBag.idofbeneficiary = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Beneficiarycontract benenficiarycontract, string idofbeneficiary)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Beneficiary beneficiary = beneficiarycollection.AsQueryable().FirstOrDefault(z => z.BeneficiaryID == idofbeneficiary);
                    benenficiarycontract.ExpirationDate = benenficiarycontract.ExpirationDate.AddDays(1);
                    benenficiarycontract.RegistrationDate = benenficiarycontract.RegistrationDate.AddDays(1);
                    benenficiarycontract.Birthdate = beneficiary.PersonalInfo.Birthdate;
                    benenficiarycontract.Firstname = beneficiary.Firstname;
                    benenficiarycontract.Lastname = beneficiary.Lastname;
                    benenficiarycontract.CNP = beneficiary.CNP;
                    benenficiarycontract.CIseria = beneficiary.CI.CIseria;
                    benenficiarycontract.CINr = beneficiary.CI.CINr;
                    benenficiarycontract.CIEliberat = beneficiary.CI.CIEliberat;
                    benenficiarycontract.Nrtel = beneficiary.PersonalInfo.PhoneNumber;
                    
                    benenficiarycontract.CIeliberator = beneficiary.CI.CIeliberator;
                    benenficiarycontract.Address = beneficiary.Adress.District + ", " + beneficiary.Adress.City + ", " + beneficiary.Adress.Street + ", " + beneficiary.Adress.Number;
                    benenficiarycontract.OwnerID = idofbeneficiary;
                    beneficiarycontractcollection.InsertOne(benenficiarycontract);
                    return RedirectToAction("Index", new { idofbeneficiary });
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes! ");
            }
            return View(benenficiarycontract);
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            var contract = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().SingleOrDefault(x => x.ContractID == id);

            return View(contract);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var contractid = new ObjectId(id);
            var contract = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().SingleOrDefault(x => x.ContractID == id);
            return View(contract);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, string idofbeneficiary)
        {
            beneficiarycontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", ObjectId.Parse(id)));

            return RedirectToAction("Index", new { idofbeneficiary });
        }
    }
}