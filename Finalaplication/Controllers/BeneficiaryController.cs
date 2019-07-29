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
using System.Text;

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


        public ActionResult ExportBeneficiaries()
        {
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            string path = "./jsondata/Beneficiaries.csv";



            var allLines = (from Beneficiary in beneficiaries
                            select new object[]
                            {
                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43};",
                            Beneficiary.Firstname,
                            Beneficiary.Lastname,
                            Beneficiary.Active,
                            Beneficiary.Weeklypackage.ToString(),
                            Beneficiary.Canteen.ToString(),
                            Beneficiary.HomeDeliveryDriver,
                            Beneficiary.HasGDPRAgreement.ToString(),
                            Beneficiary.Adress.Country,
                            Beneficiary.Adress.City,
                            Beneficiary.Adress.Street,
                            Beneficiary.Adress.Number,
                            Beneficiary.CNP,
                            Beneficiary.CI.HasId.ToString(),
                            Beneficiary.CI.ICExpirationDate.ToString(),
                            Beneficiary.Marca.IdAplication.ToString(),
                            Beneficiary.Marca.IdContract.ToString(),
                            Beneficiary.Marca.IdInvestigation.ToString(),
                            Beneficiary.NumberOfPortions.ToString(),
                            Beneficiary.LastTimeActiv.ToString(),
                            Beneficiary.Coments,
                            Beneficiary.PersonalInfo.Birthdate.ToString(),
                            Beneficiary.PersonalInfo.PhoneNumber,
                            Beneficiary.PersonalInfo.BirthPlace,
                            Beneficiary.PersonalInfo.Studies,
                            Beneficiary.PersonalInfo.Profesion,
                            Beneficiary.PersonalInfo.Ocupation,
                            Beneficiary.PersonalInfo.SeniorityInWorkField,
                            Beneficiary.PersonalInfo.HealthState,
                            Beneficiary.PersonalInfo.Disalility,
                            Beneficiary.PersonalInfo.ChronicCondition,
                            Beneficiary.PersonalInfo.Addictions,
                            Beneficiary.PersonalInfo.HealthInsurance.ToString(),
                            Beneficiary.PersonalInfo.HealthCard.ToString(),
                            Beneficiary.PersonalInfo.Married.ToString(),
                            Beneficiary.PersonalInfo.SpouseName,
                            Beneficiary.PersonalInfo.HasHome.ToString(),
                            Beneficiary.PersonalInfo.HousingType,
                            Beneficiary.PersonalInfo.Income,
                            Beneficiary.PersonalInfo.Expences,
                            Beneficiary.PersonalInfo.Gender,
                            Beneficiary.Contract.HasContract.ToString(),
                            Beneficiary.Contract.NumberOfRegistration.ToString(),
                            Beneficiary.Contract.RegistrationDate.ToString(),
                            Beneficiary.Contract.ExpirationDate.ToString())
                            }
                             ).ToList();

        var csv1 = new StringBuilder();


        allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));

            }
           );
            System.IO.File.WriteAllText(path, "Firstname,Lastname,Active,Weekly package,Canteen,Home Delivery Driver,HAS GDPR,Country,City,Street,Number,CNP,Has ID,ID Expiration,IDAplication,IDInvestigation,IDContract,Number Of Portions,Last Time Active,Comments,Birthdate,Phone Number,Birth place,Studies,Profession,Occupation,Seniority In Workfield,Health State,Disability,Chronic Condition,Addictions,Health Insurance,Health Card,Married,Spouse Name,Has Home,Housing Type,Income,Expenses,Gender,Has Contract,Number Of Registration,Registration Date,Expiration Date\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");

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
                   .Set("Active", beneficiary.Active)
                   .Set("Canteen", beneficiary.Canteen)
                   .Set("HomeDeliveryDriver", beneficiary.HomeDeliveryDriver)
                   .Set("HasGDPRAgreement", beneficiary.HasGDPRAgreement)
                   .Set("CNP", beneficiary.CNP)
                   .Set("NumberOfPortions", beneficiary.NumberOfPortions)
                   .Set("Coments", beneficiary.Coments)
                   .Set("Adress.Country", beneficiary.Adress.Country)
                   .Set("Adress.City", beneficiary.Adress.City)
                   .Set("Adress.Street", beneficiary.Adress.Street)
                   .Set("Adress.Number", beneficiary.Adress.Number)
                   .Set("CI.HasId", beneficiary.CI.HasId)
                   .Set("CI.ICExpirationDate", beneficiary.CI.ICExpirationDate.AddHours(5))
                   .Set("Marca.IdAplication", beneficiary.Marca.IdAplication )
                   .Set("Marca.IdContract", beneficiary.Marca.IdContract)
                   .Set("Marca.IdInvestigation", beneficiary.Marca.IdInvestigation)
                   .Set("LastTimeActiv", beneficiary.LastTimeActiv)
                   .Set("PersonalInfo.Birthdate", beneficiary.PersonalInfo.Birthdate.AddHours(5))
                   .Set("PersonalInfo.PhoneNumber", beneficiary.PersonalInfo.PhoneNumber)
                   .Set("PersonalInfo.BirthPlace", beneficiary.PersonalInfo.BirthPlace)
                   .Set("PersonalInfo.ChronicCondition", beneficiary.PersonalInfo.ChronicCondition)
                   .Set("PersonalInfo.Dependent", beneficiary.PersonalInfo.Addictions)
                   .Set("PersonalInfo.Disalility", beneficiary.PersonalInfo.Disalility)
                   .Set("PersonalInfo.Expences", beneficiary.PersonalInfo.Expences)
                   .Set("PersonalInfo.HasHome", beneficiary.PersonalInfo.HasHome)
                   .Set("PersonalInfo.HealthCard", beneficiary.PersonalInfo.HealthCard)
                   .Set("PersonalInfo.HealthInsurance", beneficiary.PersonalInfo.HealthInsurance)
                   .Set("PersonalInfo.HealthState", beneficiary.PersonalInfo.HealthState)
                   .Set("PersonalInfo.HousingType", beneficiary.PersonalInfo.HousingType)
                   .Set("PersonalInfo.Income", beneficiary.PersonalInfo.Income)
                   .Set("PersonalInfo.Married", beneficiary.PersonalInfo.Married)
                   .Set("PersonalInfo.Ocupation", beneficiary.PersonalInfo.Ocupation)
                   .Set("PersonalInfo.Profesion", beneficiary.PersonalInfo.Profesion)
                   .Set("PersonalInfo.SeniorityInWorkField", beneficiary.PersonalInfo.SeniorityInWorkField)
                   .Set("PersonalInfo.SpouseName", beneficiary.PersonalInfo.SpouseName)
                   .Set("PersonalInfo.Studies", beneficiary.PersonalInfo.Studies)
                   .Set("Contract.HasContract", beneficiary.Contract.HasContract)
                   .Set("Contract.NumberOfRegistration", beneficiary.Contract.NumberOfRegistration)
                   .Set("Contract.RegistrationDate", beneficiary.Contract.RegistrationDate.AddHours(5))
                   .Set("Contract.ExpirationDate", beneficiary.Contract.ExpirationDate.AddHours(5));

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