using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Finalaplication.Models;
using Finalaplication.App_Start;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Finalaplication.Controllers
{
    public class BeneficiaryController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Beneficiary> beneficiarycollection;

        public BeneficiaryController()
        {
            dbcontext = new MongoDBContext();
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("beneficiaries");
        }

        //public ActionResult ExportVolunteers()
        //{
        //    List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
        //    string path = "./jsondata/Beneficiaries.csv";



        //    var allLines = (from Volunteer in volunteers
        //                    select new object[]
        //                    {
        //                     string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8};",Volunteer.Firstname,
        //                    Volunteer.Lastname,
        //                    Volunteer.Birthdate.ToString(),
        //                    Volunteer.Gender,
        //                    Volunteer.Occupation,
        //                    Volunteer.Field_of_activity,
        //                    Volunteer.Desired_workplace,
        //                    Volunteer.InActivity.ToString(),
        //                    Volunteer.HourCount.ToString())
        //                    }
        //                     ).ToList();

        //    var csv1 = new StringBuilder();


        //    allLines.ForEach(line =>
        //    {
        //        csv1 = csv1.AppendLine(string.Join(";", line));

        //    }
        //   );
        //    System.IO.File.WriteAllText(path, "Firstname,Lastname,Birthdate,Gender,Occupation,Filed_of_activity,Desired_workplace,InActivity,HourCount\n");
        //    System.IO.File.AppendAllText(path, csv1.ToString());
        //    return RedirectToAction("Index");

        //}


        public ActionResult Index(string searching)
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

        //public ActionResult Birthday()
        //{
        //    List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
        //    return View(volunteers);
        //}

        //public ActionResult ContractExp()
        //{
        //    List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
        //    return View(volunteers);
        //}


        // GET: Volunteer/Details/5
        public ActionResult Details(string id)
        {
            var beneficiarId = new ObjectId(id);
            var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == beneficiarId);

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
                beneficiary.PersonalInfo.Birthdate = beneficiary.PersonalInfo.Birthdate.AddHours(5);
                beneficiary.Contract.RegistrationDate = beneficiary.Contract.RegistrationDate.AddHours(5);
                beneficiary.Contract.ExpirationDate = beneficiary.Contract.ExpirationDate.AddHours(5);
                beneficiary.CI.ICExpiration = beneficiary.CI.ICExpiration.AddHours(5);
                beneficiarycollection.InsertOne(beneficiary);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Volunteer/Edit/5
        public ActionResult Edit(string id)
        {
            var benefId = new ObjectId(id);
            var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == benefId);
            return View(beneficiary);
        }

        [HttpPost]
        public ActionResult Edit(string id, Beneficiary beneficiary)
        {
            try
            {
                var filter = Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Beneficiary>.Update
                    .Set("Firstname", beneficiary.Firstname)
                    .Set("Lastname", beneficiary.Lastname)
                    .Set("Active", beneficiary.Active)
                    .Set("Canteen", beneficiary.Canteen)
                    .Set("HomeDeliveryDriver", beneficiary.HomeDeliveryDriver)
                    .Set("HasGDPR", beneficiary.HasGDPR)
                    .Set("Address.Country", beneficiary.Address.Country)
                    .Set("Address.City", beneficiary.Address.City)
                    .Set("Address.Street", beneficiary.Address.Street)
                    .Set("Address.Number", beneficiary.Address.Number)
                    .Set("CI.HasID", beneficiary.CI.HasID)
                    .Set("CI.ICExpiration", beneficiary.CI.ICExpiration)
                    .Set("Marca.IdAplication", beneficiary.Marca.IdAplication)
                    .Set("Marca.IdInvestigation", beneficiary.Marca.IdInvestigation)
                    .Set("Marca.IdContract", beneficiary.Marca.IdContract)
                    .Set("Contract.HasContract", beneficiary.Contract.HasContract)
                    .Set("Contract.NumberOfRegistration", beneficiary.Contract.NumberOfRegistration)
                    .Set("Contract.RegistrationDate", beneficiary.Contract.RegistrationDate.AddHours(5))
                    .Set("Contract.ExpirationDate", beneficiary.Contract.ExpirationDate.AddHours(5))
                    .Set("PersonalInfo.Birthdate", beneficiary.PersonalInfo.Birthdate.AddHours(5))
                    .Set("PersonalInfo.PhoneNumber", beneficiary.PersonalInfo.PhoneNumber)
                    .Set("PersonalInfo.BirthPlace", beneficiary.PersonalInfo.BirthPlace)
                    .Set("PersonalInfo.Studies", beneficiary.PersonalInfo.Studies)
                    .Set("PersonalInfo.Profession", beneficiary.PersonalInfo.Profession)
                    .Set("PersonalInfo.Occupation", beneficiary.PersonalInfo.Occupation)
                    .Set("PersonalInfo.Healthstate", beneficiary.PersonalInfo.Healthstate)
                    .Set("PersonalInfo.Disability", beneficiary.PersonalInfo.Disability)
                    .Set("PersonalInfo.ChronicCondition", beneficiary.PersonalInfo.ChronicCondition)
                    .Set("PersonalInfo.Addict", beneficiary.PersonalInfo.Addict)
                    .Set("PersonalInfo.HealthInsurance", beneficiary.PersonalInfo.HealthInsurance)
                    .Set("PersonalInfo.HealthCard", beneficiary.PersonalInfo.HealthCard)
                    .Set("PersonalInfo.Married", beneficiary.PersonalInfo.Married)
                    .Set("PersonalInfo.SpouseName", beneficiary.PersonalInfo.SpouseName)
                    .Set("PersonalInfo.Homeless", beneficiary.PersonalInfo.Homeless)
                    .Set("PersonalInfo.Income", beneficiary.PersonalInfo.Income)
                    .Set("PersonalInfo.Expenses", beneficiary.PersonalInfo.Expenses)
                    .Set("PersonalInfo.Gender", beneficiary.PersonalInfo.Gender);


                var result = beneficiarycollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            var benefId = new ObjectId(id);
            var beneficiar = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == benefId);
            return View(beneficiar);
        }

        // POST: Volunteer/Delete/5
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