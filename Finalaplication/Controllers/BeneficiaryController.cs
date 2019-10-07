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
using Newtonsoft.Json;

namespace Finalaplication.Controllers
{
    public class BeneficiaryController : Controller
    {

        private MongoDBContext dbcontext;
        private MongoDBContextOffline dbcontextoffline;
        private MongoDB.Driver.IMongoCollection<Beneficiary> beneficiarycollection;
        private IMongoCollection<Settings> settingcollection;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;
        public BeneficiaryController()
        {
            dbcontextoffline = new MongoDBContextOffline();
            dbcontext = new MongoDBContext();
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
            beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
        }


        public ActionResult ExportBeneficiaries()
        {
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            string path = "./jsondata/Beneficiaries.csv";



            var allLines = (from Beneficiary in beneficiaries
                            select new object[]
                            {
                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42};",
                            Beneficiary.Firstname,
                            Beneficiary.Lastname,
                            Beneficiary.Active,
                            Beneficiary.Weeklypackage.ToString(),
                            Beneficiary.Canteen.ToString(),
                            Beneficiary.HomeDeliveryDriver,
                            Beneficiary.HasGDPRAgreement.ToString(),
                            Beneficiary.Adress.District,
                            Beneficiary.Adress.City,
                            Beneficiary.Adress.Street,
                            Beneficiary.Adress.Number,
                            Beneficiary.CNP,
                            Beneficiary.CI.HasId.ToString(),
                            Beneficiary.CI.CIseria,
                            Beneficiary.CI.CINr.ToString(),
                            Beneficiary.CI.CIEliberat.ToString(),
                            Beneficiary.CI.CIeliberator,
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
                            Beneficiary.PersonalInfo.Gender)
                           
                            }
                             ).ToList();

        var csv1 = new StringBuilder();


        allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));

            }
           );
            System.IO.File.WriteAllText(path, "Firstname,Lastname,Active,Weekly package,Canteen,Home Delivery Driver,HAS GDPR,District,City,Street,Number,CNP,Has ID,IDSerie,IDNr,IDEliberat,IdEliberator,IDAplication,IDInvestigation,IDContract,Number Of Portions,Last Time Active,Comments,Birthdate,Phone Number,Birth place,Studies,Profession,Occupation,Seniority In Workfield,Health State,Disability,Chronic Condition,Addictions,Health Insurance,Health Card,Married,Spouse Name,Has Home,Housing Type,Income,Expenses,Gender,Has Contract,Number Of Registration,Registration Date,Expiration Date\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");

}
        public ActionResult Contracts(string id)
        {
            return RedirectToAction("Index", "Beneficiarycontract", new { idofbeneficiary = id });
        }

        public ActionResult Index(string sortOrder, string searching, bool Active, bool HasContract, bool Homeless, DateTime lowerdate, DateTime upperdate, int page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.searching = searching;
            ViewBag.active = Active;
            ViewBag.hascontract = HasContract;
            ViewBag.Page = page;
            ViewBag.Upperdate = upperdate;
            ViewBag.Lowerdate = lowerdate;
            ViewBag.Homeless = Homeless;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LastnameSort = sortOrder == "Lastname" ? "Lastname_desc" : "Lastname";
            ViewBag.Gendersort = sortOrder == "Gender" ? "Gender_desc" : "Gender";
            ViewBag.Activesort = sortOrder == "Active" ? "Active_desc" : "Active";

            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
            DateTime d1 = new DateTime(0003, 1, 1);
            if (upperdate > d1)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Birthdate <= upperdate).ToList();
            }
            if (searching != null)
            {
               beneficiaries = beneficiaries.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList();
            }
            if (Homeless == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HasHome == false).ToList();
            }
            if (lowerdate > d1)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Birthdate > lowerdate).ToList();
            }
            if (Active == true)
            {
                beneficiaries = beneficiaries.Where(x => x.Active == true).ToList();
            }
            
            switch (sortOrder)
            {
                case "Gender":
                    beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Gender).ToList();
                    break;
                case "Gender_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Gender).ToList();
                    break;
                case "Lastname":
                    beneficiaries = beneficiaries.OrderBy(s => s.Lastname).ToList();
                    break;
                case "Lastname_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Lastname).ToList();
                    break;
                case "Active":
                    beneficiaries = beneficiaries.OrderBy(s => s.Active).ToList();
                    break;
                case "Active_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Active).ToList();
                    break;
                case "name_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Firstname).ToList();
                    break;
                case "Date":
                    beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Birthdate).ToList();
                    break;
                case "date_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Birthdate).ToList();
                    break;
                default:
                    beneficiaries = beneficiaries.OrderBy(s => s.Firstname).ToList();
                    break;
            }
            ViewBag.counter = beneficiaries.Count();
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();
            int nrofdocs = set.Quantity;
            ViewBag.nrofdocs = nrofdocs;
            beneficiaries = beneficiaries.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            beneficiaries = beneficiaries.AsQueryable().Take(nrofdocs).ToList();
            return View(beneficiaries);
        }

         public ActionResult ContractExp()
        {
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            return View(beneficiaries);
        }

        public ActionResult Details(string id)
        {
            var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == id);

            return View(beneficiary);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Beneficiary/Create
        [HttpPost]
        public ActionResult Create(Beneficiary beneficiary)
        {
            try
            {
                ModelState.Remove("Contract.RegistrationDate");
                ModelState.Remove("Contract.ExpirationDate");
                ModelState.Remove("Marca.IdAplication");
                ModelState.Remove("Marca.IdContract");
                ModelState.Remove("Marca.IdInvestigation");
                ModelState.Remove("NumberOfPortions");
                ModelState.Remove("LastTimeActiv");
                ModelState.Remove("Personalinfo.Birthdate");
                ModelState.Remove("CI.ICExpirationDate");

                if (ModelState.IsValid)
                {

                    
                    beneficiarycollection.InsertOne(beneficiary);
                    return RedirectToAction("Index");
                }
                else return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Beneficiary/Edit/5
        public ActionResult Edit(string id)
        {
            var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(v => v.BeneficiaryID == id);
            Beneficiary originalsavedvol = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == id);
            ViewBag.originalsavedvol = JsonConvert.SerializeObject(originalsavedvol);
            return View(beneficiary);
        }

        // POST: Beneficiary/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Beneficiary beneficiary, string Originalsavedvolstring)
        {
            Beneficiary Originalsavedvol = JsonConvert.DeserializeObject<Beneficiary>(Originalsavedvolstring);
            try
            {
                var volunteerId = new ObjectId(id);
                Beneficiary currentsavedvol = beneficiarycollection.Find(x => x.BeneficiaryID == id).Single();
                if (JsonConvert.SerializeObject(Originalsavedvol).Equals(JsonConvert.SerializeObject(currentsavedvol)))
                {
                    ModelState.Remove("Contract.RegistrationDate");
                    ModelState.Remove("Contract.ExpirationDate");
                    ModelState.Remove("Marca.IdAplication");
                    ModelState.Remove("Marca.IdContract");
                    ModelState.Remove("Marca.IdInvestigation");
                    ModelState.Remove("NumberOfPortions");
                    ModelState.Remove("LastTimeActiv");
                    ModelState.Remove("Personalinfo.Birthdate");
                    ModelState.Remove("CI.ICExpirationDate");
                if (ModelState.IsValid)
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
                       .Set("Adress.District", beneficiary.Adress.District)
                       .Set("Adress.City", beneficiary.Adress.City)
                       .Set("Adress.Street", beneficiary.Adress.Street)
                       .Set("Adress.Number", beneficiary.Adress.Number)
                       .Set("CI.HasId", beneficiary.CI.HasId)
                       .Set("CI.CIseria", beneficiary.CI.CIseria)
                       .Set("CI.CINr", beneficiary.CI.CINr)
                       .Set("CI.CIEliberat", beneficiary.CI.CIEliberat.AddHours(5))
                       .Set("CI.CIeliberator", beneficiary.CI.CIeliberator)
                       .Set("Marca.IdAplication", beneficiary.Marca.IdAplication)
                       .Set("Marca.IdContract", beneficiary.Marca.IdContract)
                       .Set("Marca.IdInvestigation", beneficiary.Marca.IdInvestigation)
                       .Set("LastTimeActiv", beneficiary.LastTimeActiv)
                       .Set("PersonalInfo.Birthdate", beneficiary.PersonalInfo.Birthdate.AddHours(5))
                       .Set("PersonalInfo.PhoneNumber", beneficiary.PersonalInfo.PhoneNumber)
                       .Set("PersonalInfo.BirthPlace", beneficiary.PersonalInfo.BirthPlace)
                       .Set("PersonalInfo.Gender", beneficiary.PersonalInfo.Gender)
                       .Set("PersonalInfo.ChronicCondition", beneficiary.PersonalInfo.ChronicCondition)
                       .Set("PersonalInfo.Addictions", beneficiary.PersonalInfo.Addictions)
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
                       ;

                    var result = beneficiarycollection.UpdateOne(filter, update);
                        return RedirectToAction("Index");
                    }
                    else return View();
                }
                else
                {
                    return View("Volunteerwarning");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Beneficiary/Delete/5
        public ActionResult Delete(string id)
        {
            var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == id);
            return View(beneficiary);
        }

        // POST: Beneficiary/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Beneficiary beneficiary, bool Inactive)
        {
            try
            {
                if (Inactive == false)
                {
                    beneficiarycollection.DeleteOne(Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id)));

                    return RedirectToAction("Index");

                }
                else
                {
                    var filter = Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id));
                    var update = Builders<Beneficiary>.Update
                        .Set("Active", beneficiary.Active);
                    var result = beneficiarycollection.UpdateOne(filter, update);
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View();
            }
        }

    }

}
