using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class BeneficiarycontractController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;
        private IMongoCollection<Beneficiary> beneficiarycollection;

        public BeneficiarycontractController()
        {
            try
            {
                dbcontext = new MongoDBContext();
                beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
                beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            }
            catch { }
        }

        [HttpGet]
        public IActionResult Index(string idofbeneficiary)
        {
            try
            {
                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Beneficiarycontract> benficiarycontracts = beneficiarycontractcollection.AsQueryable().ToList();
                Beneficiary benenficiary = beneficiarycollection.AsQueryable().FirstOrDefault(z => z.BeneficiaryID == idofbeneficiary);
                benficiarycontracts = benficiarycontracts.Where(z => z.OwnerID.ToString() == idofbeneficiary).ToList();
                ViewBag.nameofbeneficiary = benenficiary.Fullname ;
                ViewBag.idofbeneficiary = idofbeneficiary;
                return View(benficiarycontracts);
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
                List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

                return View(beneficiarycontracts);
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
                ViewBag.idofbeneficiary = id;
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Create(Beneficiarycontract benenficiarycontract, string idofbeneficiary)
        {
            try
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Beneficiary beneficiary = beneficiarycollection.AsQueryable().FirstOrDefault(z => z.BeneficiaryID == idofbeneficiary);
                        benenficiarycontract.ExpirationDate = benenficiarycontract.ExpirationDate.AddDays(1);
                        benenficiarycontract.RegistrationDate = benenficiarycontract.RegistrationDate.AddDays(1);
                        benenficiarycontract.Birthdate = beneficiary.PersonalInfo.Birthdate;
                        benenficiarycontract.Fullname = beneficiary.Fullname;
                         benenficiarycontract.CNP = beneficiary.CNP;
                        benenficiarycontract.CIinfo = beneficiary.CI.CIinfo;
                        benenficiarycontract.Nrtel = beneficiary.PersonalInfo.PhoneNumber;

                        
                        benenficiarycontract.Address = beneficiary.Adress;
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
                var contract = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().SingleOrDefault(x => x.ContractID == id);

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
                var contract = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().SingleOrDefault(x => x.ContractID == id);
                return View(contract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, string idofbeneficiary)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                beneficiarycontractcollection.DeleteOne(Builders<Beneficiarycontract>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index", new { idofbeneficiary });
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}
