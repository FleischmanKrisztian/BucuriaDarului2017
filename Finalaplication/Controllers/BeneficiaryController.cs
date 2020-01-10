using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VolCommon;

namespace Finalaplication.Controllers
{
    public class BeneficiaryController : Controller
    {
        private MongoDBContext dbcontext;
        private MongoDB.Driver.IMongoCollection<Beneficiary> beneficiarycollection;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;

        public BeneficiaryController()
        {
            try
            {
                dbcontext = new MongoDBContext();
                beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
                beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            }
            catch { }
        }

        public ActionResult FileUpload()
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(IFormFile Files)
        {
            try
            {
                string path = " ";

                if (Files.Length > 0)
                {
                    path = Path.Combine(
                               Directory.GetCurrentDirectory(), "wwwroot",
                               Files.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Files.CopyTo(stream);
                    }
                }
                else
                {
                    return View();
                }

                CSVImportParser cSV = new CSVImportParser(path);
                List<string[]> result = cSV.ExtractDataFromFile(path);

                foreach (var details in result)
                {
                    Beneficiary beneficiary = new Beneficiary();
                    if (details[7] == "False" || details[7] == "True")
                    {
                        if (details[1] != null)
                        {
                            beneficiary.Fullname = details[1];
                        }
                        if (details[8] != null)
                        {
                            beneficiary.Adress = details[8].Replace("/", ",");
                        }

                        if (details[2] == "activ" || details[2] == "da" || details[2] == "DA" || details[2] == "true" || details[2] == "True")
                        {
                            beneficiary.Active = true;
                        }
                        else { beneficiary.Active = false; }

                        if (details[5] == "Da " || details[5] == " da" || details[5] == "true" || details[5] == "True")
                        {
                            beneficiary.HomeDelivery = true;
                        }
                        else
                        {
                            beneficiary.HomeDelivery = false;
                        }

                        if (details[3] == "da" || details[3] == "DA" || details[3] == "true" || details[3] == "True")
                        {
                            beneficiary.Weeklypackage = true;
                        }
                        else
                        { beneficiary.Weeklypackage = false; }

                        if (details[6] != null)
                        {
                            beneficiary.HomeDeliveryDriver = details[6];
                        }
                        else
                        {
                            beneficiary.HomeDeliveryDriver = " ";
                        }

                        if (details[9] != null || details[9] != " ")
                        {
                            beneficiary.CNP = details[9];
                        }

                        beneficiary.NumberOfPortions = 0;
                        if (details[21] != null || details[21] != " ")
                        {
                            if (int.TryParse(details[21], out int portions))
                            {
                                beneficiary.NumberOfPortions = portions;
                            }
                        }
                        Marca MarcaDetails = new Marca();
                        if (details[17] != null) { MarcaDetails.marca = details[17]; }
                        if (details[18] != null) { MarcaDetails.IdAplication = details[18]; }
                        if (details[19] != null) { MarcaDetails.IdInvestigation = details[19]; }
                        if (details[20] != null) { MarcaDetails.IdContract = details[20]; }
                        beneficiary.Marca = MarcaDetails;

                        Personalinfo personal = new Personalinfo();

                        if (details[25] != null || details[25] != " ")
                        {
                            personal.PhoneNumber = details[25].Replace("/", ",");
                        }

                        if (details[26] != null || details[26] != " ")
                        {
                            personal.BirthPlace = details[26];
                        }

                        if (details[27] != null)
                        {
                            personal.Studies = details[27].Replace("/", ",");
                        }

                        personal.Profesion = details[28].Replace("/", ",");
                        personal.Ocupation = details[29].Replace("/", ",");
                        personal.SeniorityInWorkField = details[30];
                        personal.HealthState = details[31].Replace("/", ",");
                        if (details[32] != " " || details[32] != null)
                        {
                            personal.Disalility = details[32].Replace("/", ",");
                        }
                        else { personal.Disalility = " "; }

                        if (details[33] != " " || details[33] != null)
                        {
                            personal.ChronicCondition = details[33].Replace("/", ",");
                        }
                        else { personal.ChronicCondition = " "; }

                        if (details[34] != " " || details[34] != null)
                        {
                            personal.Addictions = details[34].Replace("/", ",");
                        }
                        else { personal.Addictions = " "; }

                        if (details[35] == "da" || details[35] == "Da" || details[35] == "true" || details[35] == "True")
                        {
                            personal.HealthInsurance = true;
                        }
                        else { personal.HealthInsurance = false; }

                        if (details[36] == "da" || details[36] == "Da" || details[36] == "true" || details[36] == "True")
                        {
                            personal.HealthCard = true;
                        }
                        else { personal.HealthCard = false; }

                        if (details[37] != " " || details[37] != null)
                        {
                            personal.Married = details[37].Replace("/", ",");
                        }
                        else { personal.Married = " "; }

                        if (details[38] != " " || details[38] != null)
                        {
                            personal.SpouseName = details[38].Replace("/", ",");
                        }
                        else { personal.SpouseName = " "; }

                        if (details[40] != " " || details[40] != null)
                        {
                            personal.HousingType = details[40].Replace("/", ",");
                        }
                        else { personal.HousingType = " "; }

                        if (details[39] == "da" || details[39] == "Da" || details[39] == "true" || details[39] == "True")
                        {
                            personal.HasHome = true;
                        }
                        else { personal.HasHome = false; }

                        if (details[41] != " " || details[41] != null)
                        {
                            personal.Income = details[41].Replace("/", ",");
                        }
                        else { personal.Income = " "; }

                        if (details[42] != " " || details[42] != null)
                        {
                            personal.Expences = details[42];
                        }
                        else { personal.Expences = " "; }

                        if (details[24] == null || details[24] == "")
                        {
                            personal.Birthdate = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[24].Contains("/") == true)
                            {
                                string[] date = details[16].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                            }
                            else
                            {
                                string[] anotherDate = details[24].Split('.');
                                data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                            }
                            personal.Birthdate = data;
                        }

                        if (details[43] == "F" || details[43] == "f" || details[43] == "feminin" || details[43] == "Feminin" || details[43] == "1")
                        {
                            personal.Gender = VolCommon.Gender.Female;
                        }
                        else
                        {
                            personal.Gender = VolCommon.Gender.Male;
                        }
                        beneficiary.Coments = details[23];

                        beneficiary.PersonalInfo = personal;

                        CI ciInfo = new CI();

                        if (details[16] == null || details[16] == "")
                        {
                            ciInfo.ExpirationDateCI = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[16].Contains("/") == true)
                            {
                                string[] date = details[16].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                            }
                            else
                            {
                                string[] anotherDate = details[16].Split('.');
                                data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                            }
                            ciInfo.ExpirationDateCI = data;
                        }
                        ciInfo.CIinfo = details[15].Replace("/", ",");

                        if (details[10] != null || details[10] != "")
                        { ciInfo.HasId = true; }

                        beneficiary.CI = ciInfo;
                    }
                    else
                    {
                        if (details[1] != null)
                        {
                            beneficiary.Fullname = details[1];
                        }
                        if (details[8] != null)
                        {
                            beneficiary.Adress = details[8].Replace("/", ",");
                        }

                        if (details[3] == "activ" || details[3] == "da" || details[3] == "DA")
                        {
                            beneficiary.Active = true;
                        }
                        else { beneficiary.Active = false; }

                        if (details[4] == "Da " || details[4] == " da")
                        {
                            beneficiary.HomeDelivery = true;
                        }
                        else
                        {
                            beneficiary.HomeDelivery = false;
                        }

                        if (details[5] == "da" || details[5] == "DA")
                        {
                            beneficiary.Weeklypackage = true;
                        }
                        else
                        { beneficiary.Weeklypackage = false; }
                        if (details[6] != null)
                        {
                            beneficiary.HomeDeliveryDriver = details[6];
                        }
                        else
                        {
                            beneficiary.HomeDeliveryDriver = " ";
                        }

                        if (details[9] != null || details[9] != " ")
                        {
                            beneficiary.CNP = details[9];
                        }

                        beneficiary.NumberOfPortions = 0;
                        if (details[23] != null || details[23] != " ")
                        {
                            if (int.TryParse(details[23], out int portions))
                            {
                                beneficiary.NumberOfPortions = portions;
                            }
                        }
                        Marca MarcaDetails = new Marca();
                        if (details[12] != null) { MarcaDetails.marca = details[12]; }
                        if (details[13] != null) { MarcaDetails.IdAplication = details[13]; }
                        if (details[14] != null) { MarcaDetails.IdInvestigation = details[14]; }
                        if (details[15] != null) { MarcaDetails.IdContract = details[15]; }
                        beneficiary.Marca = MarcaDetails;

                        Personalinfo personal = new Personalinfo();

                        if (details[18] != null || details[18] != " ")
                        {
                            personal.PhoneNumber = details[18].Replace("/", ",");
                        }

                        if (details[19] != null || details[19] != " ")
                        {
                            personal.BirthPlace = details[19];
                        }

                        if (details[20] != null)
                        {
                            personal.Studies = details[20].Replace("/", ",");
                        }

                        personal.Profesion = details[21].Replace("/", ",");
                        personal.Ocupation = details[22].Replace("/", ",");
                        personal.SeniorityInWorkField = details[23];
                        personal.HealthState = details[24].Replace("/", ",");
                        if (details[35] != " " || details[25] != null)
                        {
                            personal.Disalility = details[25].Replace("/", ",");
                        }
                        else { personal.Disalility = " "; }

                        if (details[26] != " " || details[26] != null)
                        {
                            personal.ChronicCondition = details[26].Replace("/", ",");
                        }
                        else { personal.ChronicCondition = " "; }

                        if (details[27] != " " || details[27] != null)
                        {
                            personal.Addictions = details[27].Replace("/", ",");
                        }
                        else { personal.Addictions = " "; }

                        if (details[28] == "da" || details[28] == "Da")
                        {
                            personal.HealthInsurance = true;
                        }
                        else { personal.HealthInsurance = false; }

                        if (details[29] == "da" || details[29] == "Da")
                        {
                            personal.HealthCard = true;
                        }
                        else { personal.HealthCard = false; }

                        if (details[30] != " " || details[30] != null)
                        {
                            personal.Married = details[30].Replace("/", ",");
                        }
                        else { personal.Married = " "; }

                        if (details[31] != " " || details[31] != null)
                        {
                            personal.SpouseName = details[31].Replace("/", ",");
                        }
                        else { personal.SpouseName = " "; }

                        if (details[32] != " " || details[32] != null)
                        {
                            personal.HousingType = details[32].Replace("/", ",");
                        }
                        else { personal.HousingType = " "; }

                        if (details[33] == "da" || details[33] == "Da")
                        {
                            personal.HasHome = true;
                        }
                        else { personal.HasHome = false; }

                        if (details[34] != " " || details[34] != null)
                        {
                            personal.Income = details[34].Replace("/", ",");
                        }
                        else { personal.Income = " "; }

                        if (details[35] != " " || details[35] != null)
                        {
                            personal.Expences = details[35];
                        }
                        else { personal.Expences = " "; }

                        if (details[36] != null || details[36] != "" || details[36] != "-" || details[37] != null || details[37] != " " || details[37] != "-" || details[38] != null || details[38] != " " || details[38] != "-")
                        {
                            personal.Birthdate = Convert.ToDateTime(details[36] + "-" + details[37] + "- " + details[38]);
                        }

                        if (details[42] == "F" || details[42] == "f" || details[42] == "feminin" || details[42] == "Feminin")
                        {
                            personal.Gender = VolCommon.Gender.Female;
                        }
                        else
                        {
                            personal.Gender = VolCommon.Gender.Male;
                        }
                        beneficiary.Coments = details[43];

                        beneficiary.PersonalInfo = personal;

                        CI ciInfo = new CI();

                        if (details[11] == null || details[11] == "")
                        {
                            ciInfo.ExpirationDateCI = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[11].Contains("/") == true)
                            {
                                string[] date = details[11].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                            }
                            else
                            {
                                string[] anotherDate = details[11].Split('.');
                                data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                            }
                            ciInfo.ExpirationDateCI = data;
                        }
                        ciInfo.CIinfo = details[10].Replace("/", ",");

                        if (details[11] != null || details[11] != "")
                        { ciInfo.HasId = true; }

                        beneficiary.CI = ciInfo;
                    }
                   
                    beneficiarycollection.InsertOne(beneficiary);
                }
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        public ActionResult Contracts(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                return RedirectToAction("Index", "Beneficiarycontract", new { idofbeneficiary = id });
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Index(string sortOrder, string searching, bool Active, bool HasContract, bool Homeless, DateTime lowerdate, DateTime upperdate, int page)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ViewBag.SortOrder = sortOrder;
                ViewBag.searching = searching;
                ViewBag.active = Active;
                ViewBag.hascontract = HasContract;
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;
                ViewBag.Upperdate = upperdate;
                ViewBag.Lowerdate = lowerdate;
                ViewBag.Homeless = Homeless;

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.FullnameSort = sortOrder == "Fullname" ? "Fullname_desc" : "Fullname";
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
                    beneficiaries = beneficiaries.Where(x => x.Fullname.Contains(searching)).ToList();
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

                    case "Fullname":
                        beneficiaries = beneficiaries.OrderBy(s => s.Fullname).ToList();
                        break;

                    case "Active":
                        beneficiaries = beneficiaries.OrderBy(s => s.Active).ToList();
                        break;

                    case "Active_desc":
                        beneficiaries = beneficiaries.OrderByDescending(s => s.Active).ToList();
                        break;

                    case "name_desc":
                        beneficiaries = beneficiaries.OrderByDescending(s => s.Fullname).ToList();
                        break;

                    case "Date":
                        beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Birthdate).ToList();
                        break;

                    case "date_desc":
                        beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Birthdate).ToList();
                        break;

                    default:
                        beneficiaries = beneficiaries.OrderBy(s => s.Fullname).ToList();
                        break;
                }
                ViewBag.counter = beneficiaries.Count();

                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                string stringofids = "beneficiaries";
                foreach (Beneficiary ben in beneficiaries)
                {
                    stringofids = stringofids + "," + ben.BeneficiaryID;
                }
                ViewBag.stringofids = stringofids;
                ViewBag.nrofdocs = nrofdocs;
                beneficiaries = beneficiaries.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                beneficiaries = beneficiaries.AsQueryable().Take(nrofdocs).ToList();
                return View(beneficiaries);
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
                List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
                return View(beneficiaries);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Details(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == id);

                return View(beneficiary);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Beneficiary/Create
        [HttpPost]
        public ActionResult Create(Beneficiary beneficiary)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ModelState.Remove("Contract.RegistrationDate");
                ModelState.Remove("Contract.ExpirationDate");
                ModelState.Remove("Marca.IdAplication");
                ModelState.Remove("CI.CIEliberat");
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
                return RedirectToAction("Localserver", "Home");
            }
        }

        // GET: Beneficiary/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(v => v.BeneficiaryID == id);
                Beneficiary originalsavedvol = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == id);
                ViewBag.originalsavedvol = JsonConvert.SerializeObject(originalsavedvol);
                return View(beneficiary);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Beneficiary/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Beneficiary beneficiary, string Originalsavedvolstring)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                               .Set("Firstname", beneficiary.Fullname)

                               .Set("Weeklypackage", beneficiary.Weeklypackage)
                               .Set("Active", beneficiary.Active)
                               .Set("Canteen", beneficiary.Canteen)
                               .Set("HomeDeliveryDriver", beneficiary.HomeDeliveryDriver)
                               .Set("HasGDPRAgreement", beneficiary.HasGDPRAgreement)
                               .Set("CNP", beneficiary.CNP)
                               .Set("NumberOfPortions", beneficiary.NumberOfPortions)
                               .Set("Coments", beneficiary.Coments)
                               .Set("Adress.Adress", beneficiary.Adress)

                               .Set("CI.CIinfo", beneficiary.CI.CIinfo)
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
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // GET: Beneficiary/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var beneficiary = beneficiarycollection.AsQueryable<Beneficiary>().SingleOrDefault(x => x.BeneficiaryID == id);
                return View(beneficiary);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Beneficiary/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Beneficiary beneficiary, bool Inactive)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    if (Inactive == false)
                    {
                        beneficiarycollection.DeleteOne(Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id)));
                        beneficiarycontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
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
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}
