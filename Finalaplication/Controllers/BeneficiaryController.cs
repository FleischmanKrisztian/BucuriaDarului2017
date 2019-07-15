using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using Finalaplication.App_Start;
using Finalaplication.Models;
using MongoDB.Driver;
using System.IO;

namespace Finalaplication.Controllers
{
    public class BeneficiaryController : Controller
    {

        private MongoDBContext dbcontext;
        private MongoDB.Driver.IMongoCollection<Beneficiary> beneficiarycollection;

        public BeneficiaryController()
        {
            dbcontext = new MongoDBContext();
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("beneficiaries");
        }


//        public ActionResult ExportBeneficiaries()
//        {
//            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
//            string path = "./jsondata/Beneficiaries.csv";



//            var allLines = (from Beneficiary in beneficiaries
//                            select new object[]
//                            {
//                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41};",
//                             beneficiaries.Firstname},
//                          beneficiaries.Lastname,
//                           beneficiaries.Weeklypackage.ToString(),
//                            beneficiaries.Status.ToString(),
//                            beneficiaries.Canteen.ToString(),
//                            beneficiaries.HomeDeliveryDriver.ToString(),
//                            beneficiaries.HasGDPRAgreement.ToString(),
//                           beneficiaries.CNP.ToString(),
//                           beneficiaries.NumberOfPortions.ToString(),
//                           beneficiaries.Coments.ToString(),
//                           beneficiaries.Adress.City.ToString(),
//                          beneficiaries.Adress.Street.ToString(),
//                           beneficiaries.Adress.Number.ToString(),
//                           beneficiaries.CI.HasId.ToString(),
//                         beneficiaries.CI.CIExpirationDate.ToString(),
//                          beneficiaries.Marca.IdAplication.ToString(),
//                          beneficiaries.Marca.IdContract.ToString(),
//                          beneficiaries.Marca.IdInvestigation.ToString(),
//                          beneficiaries.LastTimeActiv.ToString(),
//                          beneficiaries.PersonalInfo.Birthdate.ToString(),
//                          beneficiaries.PersonalInfo.BirthPlace.ToString(),
//                          beneficiaries.PersonalInfo.ChronicCondition.ToString(),
//                           beneficiaries.PersonalInfo.Dependent.ToString(),
//                          beneficiaries.PersonalInfo.Disalility.ToString(),
//                           beneficiaries.PersonalInfo.Expences.ToString(),
//                          beneficiaries.PersonalInfo.HasHome.ToString(),
//                          beneficiaries.PersonalInfo.HealthCard.ToString(),
//                           beneficiaries.PersonalInfo.HealthInsurance.ToString(),
//                          beneficiaries.PersonalInfo.HealthState.ToString(),
//                          beneficiaries.PersonalInfo.HousingType.ToString(),
//                          beneficiaries.PersonalInfo.Income.ToString(),
//                           beneficiaries.PersonalInfo.IsMarried.ToString(),
//                           beneficiaries.PersonalInfo.Ocupation.ToString(),
//                          beneficiaries.PersonalInfo.PhoneNumber.ToString(),
//                          beneficiaries.PersonalInfo.Profesion.ToString(),
//                         beneficiaries.PersonalInfo.SeniorityInWorkField.ToString(),
//                          beneficiaries.PersonalInfo.SpouseName.ToString(),
//                          beneficiaries.PersonalInfo.Studies.ToString(),
//                           beneficiaries.Contract.HasContract.ToString(),
//                           beneficiaries.Contract.NumberOfRegistration.ToString(),
//                          beneficiaries.Contract.RegistrationDate.ToString(),
//                          beneficiaries.Contract.ExpirationDate.ToString()
//                            }
//                             ).ToList();

//        var csv1 = new StringBuilder();


//        allLines.ForEach(line =>
//            {
//                csv1 = csv1.AppendLine(string.Join(";", line));

//            }
//           );
//            System.IO.File.WriteAllText(path, "\n");
//            System.IO.File.AppendAllText(path, csv1.ToString());
//            return RedirectToAction("Index");

//}


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

        public ActionResult ContractExp()
        {
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            return View(beneficiaries);
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
                   .Set("NumberOfPortions", beneficiary.NumberOfPortions)
                   .Set("Coments", beneficiary.Coments)
                   .Set("City", beneficiary.Adress.City)
                   .Set("Street", beneficiary.Adress.Street)
                   .Set("Number", beneficiary.Adress.Number)
                   .Set("HasId", beneficiary.CI.HasId)
                   .Set("ExpirationDate", beneficiary.CI.CIExpirationDate)
                   .Set("IdAplication)", beneficiary.Marca.IdAplication)
                   .Set("IdContract", beneficiary.Marca.IdContract)
                   .Set("IdInvestigation", beneficiary.Marca.IdInvestigation)
                   .Set("LastTimeActiv)", beneficiary.LastTimeActiv)
                   .Set("Birthdate", beneficiary.PersonalInfo.Birthdate)
                   .Set("BirthPLace", beneficiary.PersonalInfo.BirthPlace)
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
                   .Set("Married", beneficiary.PersonalInfo.IsMarried)
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
        public ActionResult Delete(string id, Beneficiary benficiary)
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