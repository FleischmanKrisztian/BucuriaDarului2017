﻿using Elm.Core.Parsers;
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
                    var aux = details[42];
                    for (int i = details.Length - 1; i > 0; i--)
                    {
                        details[i] = details[i - 1];
                    }
                    Beneficiary beneficiary = new Beneficiary();
                    if (details[7] == "True")
                    {
                        beneficiary.HasGDPRAgreement = true;
                    }
                    else
                    {
                        beneficiary.HasGDPRAgreement = false;
                    }
                    try
                    {
                        beneficiary.Fullname = details[1];
                    }
                    catch
                    {
                        beneficiary.Fullname = "Incorrect Name";
                    }
                    try
                    {
                        beneficiary.Adress = details[8];
                    }
                    catch
                    {
                        beneficiary.Adress = "Incorrect address";
                    }

                    if (details[2] == "true" || details[2] == "True")
                    {
                        beneficiary.Active = true;
                    }
                    else
                    {
                        beneficiary.Active = false;
                    }

                    if (details[4] == "true" || details[4] == "True")
                    {
                        beneficiary.Canteen = true;
                    }
                    else
                    {
                        beneficiary.Canteen = false;
                    }

                    if (details[5] == "true" || details[5] == "True")
                    {
                        beneficiary.HomeDelivery = true;
                    }
                    else
                    {
                        beneficiary.HomeDelivery = false;
                    }

                    if (details[3] == "true" || details[3] == "True")
                    {
                        beneficiary.Weeklypackage = true;
                    }
                    else
                    {
                        beneficiary.Weeklypackage = false;
                    }

                    if (details[6] != null || details[6] != "")
                    {
                        beneficiary.HomeDeliveryDriver = details[6];
                    }
                    else
                    {
                        beneficiary.HomeDeliveryDriver = " ";
                    }

                    if (details[9] != null || details[9] != "")
                    {
                        beneficiary.CNP = details[9];
                    }

                    try
                    {
                        if (details[21] != null || details[21] != " ")
                        {
                            if (int.TryParse(details[21], out int portions))
                            {
                                beneficiary.NumberOfPortions = portions;
                            }
                        }
                    }
                    catch
                    {
                        beneficiary.NumberOfPortions = 0;
                    }

                    try
                    {
                        Marca MarcaDetails = new Marca();
                        if (details[17] != null) { MarcaDetails.marca = details[17]; }
                        if (details[18] != null) { MarcaDetails.IdAplication = details[18]; }
                        if (details[19] != null) { MarcaDetails.IdInvestigation = details[19]; }
                        if (details[20] != null) { MarcaDetails.IdContract = details[20]; }
                        beneficiary.Marca = MarcaDetails;
                    }
                    catch
                    {
                        beneficiary.Marca.marca = "error";
                        beneficiary.Marca.IdAplication = "error";
                        beneficiary.Marca.IdInvestigation = "error";
                        beneficiary.Marca.IdContract = "error";
                    }

                    Personalinfo personal = new Personalinfo();
                    try
                    {
                        if (details[25] != null || details[25] != "")
                        {
                            personal.PhoneNumber = details[25];
                        }

                        if (details[26] != null || details[26] != "")
                        {
                            personal.BirthPlace = details[26];
                        }

                        if (details[27] != null)
                        {
                            personal.Studies = details[27];
                        }

                        personal.Profesion = details[28];
                        personal.Ocupation = details[29];
                        personal.SeniorityInWorkField = details[30];
                        personal.HealthState = details[31];
                    }
                    catch
                    {
                        beneficiary.PersonalInfo.Profesion = "Error";
                        beneficiary.PersonalInfo.Ocupation = "Error";
                        beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                        beneficiary.PersonalInfo.HealthState = "Error";
                    }

                    if (details[32] != " " || details[32] != null)
                    {
                        personal.Disalility = details[32];
                    }
                    else { personal.Disalility = " "; }

                    if (details[33] != " " || details[33] != null)
                    {
                        personal.ChronicCondition = details[33];
                    }
                    else { personal.ChronicCondition = " "; }

                    if (details[34] != " " || details[34] != null)
                    {
                        personal.Addictions = details[34];
                    }
                    else { personal.Addictions = " "; }

                    if (details[35] == "true" || details[35] == "True")
                    {
                        personal.HealthInsurance = true;
                    }
                    else { personal.HealthInsurance = false; }

                    if (details[36] == "true" || details[36] == "True")
                    {
                        personal.HealthCard = true;
                    }
                    else { personal.HealthCard = false; }

                    if (details[37] != " " || details[37] != null)
                    {
                        personal.Married = details[37];
                    }
                    else { personal.Married = " "; }

                    if (details[38] != " " || details[38] != null)
                    {
                        personal.SpouseName = details[38];
                    }
                    else { personal.SpouseName = " "; }

                    if (details[40] != " " || details[40] != null)
                    {
                        personal.HousingType = details[40];
                    }
                    else { personal.HousingType = " "; }

                    if (details[39] == "true" || details[39] == "True")
                    {
                        personal.HasHome = true;
                    }
                    else { personal.HasHome = false; }

                    if (details[41] != " " || details[41] != null)
                    {
                        personal.Income = details[41];
                    }
                    else { personal.Income = " "; }

                    if (details[42] != " " || details[42] != null)
                    {
                        personal.Expences = details[42];
                    }
                    else { personal.Expences = " "; }

                    try
                    {
                        if (details[24] == null || details[24] == "")
                        {
                            personal.Birthdate = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[24].Contains("/") == true)
                            {
                                string[] date = details[24].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                            }
                            else
                            {
                                string[] anotherDate = details[24].Split('.');
                                data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                            }
                            personal.Birthdate = data.AddDays(1);
                        }
                    }
                    catch
                    {
                        personal.Birthdate = DateTime.MinValue;
                    }

                    try
                    {
                        if (aux == "1" || aux == "True")
                        {
                            personal.Gender = VolCommon.Gender.Female;
                        }
                        else
                        {
                            personal.Gender = VolCommon.Gender.Male;
                        }
                        beneficiary.Coments = details[23];
                    }
                    catch
                    {
                        beneficiary.Coments = "";
                    }

                    

                    if (details[22] == null || details[22] == "")
                    {
                        beneficiary.LastTimeActiv = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (details[22].Contains("/") == true)
                        {
                            string[] date = details[22].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[22].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }
                        beneficiary.LastTimeActiv = data.AddDays(1);
                    }

                    CI ciInfo = new CI();

                    try
                    {
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
                            ciInfo.ExpirationDateCI = data.AddDays(1);
                        }
                    }
                    catch
                    {
                        ciInfo.ExpirationDateCI = DateTime.MinValue;
                    }
                    
                    try
                    {
                        ciInfo.CIinfo = details[15];

                        if (details[10] != "True" || details[10] != "true")
                        { ciInfo.HasId = true; } 
                    }
                    catch
                    {
                        ciInfo.CIinfo = "";
                    }
                    beneficiary.PersonalInfo = personal;
                    beneficiary.CI = ciInfo;
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
