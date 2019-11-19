﻿using CsvHelper;
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
using System.Net.Http.Headers;
using System.Text;
using TinyCsvParser;
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

        [HttpPost]
        public ActionResult FileUpload(IFormFile Files)
        {

            string path = " ";
            //var filename = "baza date Siemens.xlsx";

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

            // string filePath = Path.GetFileName(Files.FileName);
            // var filePath = ContentDispositionHeaderValue.Parse(Files.ContentDisposition).FileName.Replace("\"", string.Empty);


            CSVImportParser cSV = new CSVImportParser(path);
            List<string[]> result = cSV.ExtractDataFromFile(path);


           

            foreach (var details in result)
            {
                Beneficiary beneficiary = new Beneficiary();
                if (details[1] != null)
                {
                    
                        beneficiary.Fullname = details[1];
                }
                if (details[8] != null)
                { beneficiary.Adress = details[8];
                }


                    if (details[3] == "activ" || details[3] == "da " || details[3] == "DA ")
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


                    if (details[13] != null || details[13] != " ")
                    {
                        beneficiary.CNP = details[13];
                    }

                    beneficiary.NumberOfPortions = 0;
                    if (details[23] != null || details[23] != " ")
                    {
                        if (int.TryParse(details[23], out int portions))
                        {
                            beneficiary.NumberOfPortions = portions;
                        }
                    }

                Personalinfo personal = new Personalinfo();

                    if (details[24] != null || details[24] != " ")
                    {
                       personal.PhoneNumber = details[19];
                    }

                    if (details[20] != null || details[20] != " ")
                    {
                    personal.BirthPlace = details[2];
                    }

                    if (details[21] != null)
                    {
                    personal.Studies = details[21];
                    }


                personal.Profesion = details[22];
                personal.Ocupation = details[23];
                personal.SeniorityInWorkField = details[24];
                personal.HealthState = details[25];
                    if (details[26] != " " || details[26] != null)
                    {
                    personal.Disalility = details[26];
                    }
                    else { personal.Disalility = " "; }

                    if (details[27] != " " || details[27] != null)
                    {
                    personal.ChronicCondition = details[27];
                    }
                    else { personal.ChronicCondition = " "; }

                    if (details[28] != " " || details[28] != null)
                    {
                    personal.Addictions = details[29];

                    }
                    else { personal.Addictions = " "; }

                    if (details[29] == "da" || details[29] == "Da")
                    {
                    personal.HealthInsurance = true;
                    }
                    else { personal.HealthInsurance = false; }

                    if (details[30] == "da" || details[30] == "Da")
                    {
                    personal.HealthCard = true;
                    }
                    else { personal.HealthCard = false; }



                    if (details[31] != " " || details[31] != null)
                    {
                    personal.Married = details[31];
                    }
                    else { personal.Married = " "; }

                    if (details[32] != " " || details[32] != null)
                    {
                    personal.SpouseName = details[32];
                    }
                    else { personal.SpouseName = " "; }

                    if (details[33] != " " || details[33] != null)
                    {
                    personal.HousingType = details[33];
                    }
                    else { personal.HousingType = " "; }

                    if (details[34] == "da" || details[34] == "Da")
                    {
                    personal.HasHome = true;
                    }
                    else { personal.HasHome = false; }

                    if (details[35] != " " || details[35] != null)
                    {
                    personal.Income = details[35];
                    }
                    else { personal.Income = " "; }

                    if (details[36] != " " || details[36] != null)
                    {
                    personal.Expences = details[36];
                    }
                    else { personal.Expences = " "; }

                    //if (details[37] != null || details[37] != " " || details[38] != null || details[38] != " " || details[39] != null || details[39] != " ")
                    //{
                    //personal.Birthdate = Convert.ToDateTime(details[37] + "-" + details[38] + "- " + details[39]);
                    //}

                    if (details[40] == "F" || details[40] == "f" || details[40] == "feminin" || details[40] == "Feminin")
                    {
                    personal.Gender = VolCommon.Gender.Female;
                    }
                    else
                    {
                    personal.Gender = VolCommon.Gender.Male;
                    }
                beneficiary.PersonalInfo = personal;

                    beneficiarycollection.InsertOne(beneficiary);
                   
               
            }
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            return RedirectToAction("Index");
        }
      
            


    

    public ActionResult ExportBeneficiaries()
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable().ToList();
                string path = "./Excelfiles/Beneficiaries.csv";

                var allLines = (from Beneficiary in beneficiaries
                                select new object[]
                                {
                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38};",
                            Beneficiary.Fullname,
                           
                            Beneficiary.Active,
                            Beneficiary.Weeklypackage.ToString(),
                            Beneficiary.Canteen.ToString(),
                            Beneficiary.HomeDeliveryDriver,
                            Beneficiary.HasGDPRAgreement.ToString(),
                            Beneficiary.Adress,
                           Beneficiary.CNP,
                            Beneficiary.CI.HasId.ToString(),
                            Beneficiary.CI.CIseria,
                            Beneficiary.CI.CINr,
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
            catch
            {
                return RedirectToAction("Localserver", "Home");
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
                    beneficiaries = beneficiaries.Where(x => x.Fullname.Contains(searching) ).ToList();
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
                               .Set("Adress.District", beneficiary.Adress)
                              
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
