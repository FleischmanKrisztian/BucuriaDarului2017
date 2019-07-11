using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finalaplication.App_Start;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Finalaplication.Controllers
{
    public class BeneficiaryController : Controller
    {
        private MongoDBContext dbcontext;
        private MongoDB.Driver.IMongoCollection<Beneficiary> beneficiarycollection;

        public BeneficiaryController()
        {
            dbcontext = new MongoDBContext();
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
        }


        public IActionResult Index(string searching)
        {
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            if (searching != null)
            {
                return View(beneficiaries.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList());
            }
            else
            {
                return View(beneficiaries);
            }
        }
        
   
            public ActionResult Details(string id)
            {
                var beneficiaryId = new MongoDB.Bson.ObjectId(id);
                var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == beneficiaryId);

                return View(beneficiary);
            }

            // GET: Volunteer/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Volunteer/Create
            [HttpPost]
            public ActionResult Create(Beneficiary beneficiary)
            {
                try
                {

                    beneficiarycollection.InsertOne(beneficiary);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            // GET: Beneficiary/Edit/5
            public ActionResult Edit(string id)
            {
                var beneficiaryId = new ObjectId(id);
                var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(v => v.BeneficiaryID == beneficiaryId);
                return View(beneficiary);
            }

            // POST: Beneficiary/Edit/5
            [HttpPost]
            public ActionResult Edit(string id, Beneficiary beneficiary)
            {
                try
                {
                    var filter = Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Beneficiary>.Update
                    .Set("Firstname", beneficiary.Firstname)
                    .Set("Lastname", beneficiary.Lastname)
                    .Set("Weeklypackage", beneficiary.Weeklypackage)
                    .Set("Status", beneficiary.Status)
                   .Set("Canteen", beneficiary.Canteen)
                   .Set("HomeDeliveriDriver", beneficiary.HomeDeliveryDriver)
                   .Set("HasGDPRAgreement", beneficiary.HasGDPRAgreement)
                   .Set("CNP", beneficiary.CNP)
                   .Set("ContractPeriode", beneficiary.ContractPeriode)
                   .Set("NumberOfPortions", beneficiary.NumberOfPortions)
                   .Set("Coments", beneficiary.Coments)
                   .Set("City", beneficiary.Adress.City)
                   .Set("Street", beneficiary.Adress.Street)
                   .Set("Number", beneficiary.Adress.Number)
                   .Set("HasId", beneficiary.CI.HasId)
                   .Set("ExpirationDate", beneficiary.CI.ExpirationDate)
                   .Set("IdAplication)", beneficiary.Marca.IdAplication)
                   .Set("IdContract", beneficiary.Marca.IdContract)
                   .Set("IdInvestigation", beneficiary.Marca.IdInvestigation)
                   .Set("LastTimeActiv)", beneficiary.LastTimeActiv)
                   .Set("Age", beneficiary.PersonalInfo.Age)
                   .Set("BirthDate", beneficiary.PersonalInfo.BirthDate)
                   .Set("BirthPLace", beneficiary.PersonalInfo.BirthPLace)
                   .Set("ChronicCondition", beneficiary.PersonalInfo.ChronicCondition)
                   .Set("Dependent", beneficiary.PersonalInfo.Dependent)
                   .Set("Disalility", beneficiary.PersonalInfo.Disalility)
                   .Set("Expences", beneficiary.PersonalInfo.Expences)
                   .Set("HasHome", beneficiary.PersonalInfo.HasHome)
                   .Set("HealthCard", beneficiary.PersonalInfo.HealthCard)
                   .Set("HealthInsurance", beneficiary.PersonalInfo.HealthInsurance)
                   .Set("HealthState", beneficiary.PersonalInfo.HealthState)
                   .Set("HousingType", beneficiary.PersonalInfo.HousingType)
                   .Set("Income", beneficiary.PersonalInfo.Income)
                   .Set("Married", beneficiary.PersonalInfo.Married)
                   .Set("Ocupation", beneficiary.PersonalInfo.Ocupation)
                   .Set("PhoneNumber", beneficiary.PersonalInfo.PhoneNumber)
                   .Set("Profesion", beneficiary.PersonalInfo.Profesion)
                   .Set("SeniorityInWorkField", beneficiary.PersonalInfo.SeniorityInWorkField)
                   .Set("SpouseName", beneficiary.PersonalInfo.SpouseName)
                   .Set("Studies", beneficiary.PersonalInfo.Studies)
                   .Set("HasContract", beneficiary.Contract.HasContract)
                   .Set("NumberOfRegistration", beneficiary.Contract.NumberOfRegistration)
                   .Set("RegistrationDate", beneficiary.Contract.RegistrationDate)
                   .Set("ExpirationDate", beneficiary.Contract.ExpirationDate);

                    var result = beneficiarycollection.UpdateOne(filter, update);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            // GET: Beneficiary/Delete/5
            public ActionResult Delete(string id)
            {
                var beneficiaryId = new ObjectId(id);
                var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == beneficiaryId);
                return View(beneficiary);
            }

            // POST: Beneficiary/Delete/5
            [HttpPost]
            public ActionResult Delete(string id, IFormCollection collection)
            {
                try
                {
                    beneficiarycollection.DeleteOne(Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id)));

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

        }
    }

