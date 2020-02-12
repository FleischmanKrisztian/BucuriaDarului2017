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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using VolCommon;

namespace Finalaplication.Controllers
{
    public class BeneficiaryController : Controller
    {
        private MongoDBContext dbcontext;
        private MongoDB.Driver.IMongoCollection<Beneficiary> beneficiarycollection;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;

        public string DuplicatesCallback(string duplicates)
        {
            TempData["duplicates"] = duplicates;
            return duplicates;
        }

        public int Documentsimportedcallback(int documentsimported)
        {
            TempData["docsimported"] = documentsimported;
            return documentsimported;
        }

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
                string duplicates = "";
                int documentsimported = 0;

                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }

                DuplicatesCallback callback1 = new DuplicatesCallback(DuplicatesCallback);
                Documentsimportedcallback callback2 = new Documentsimportedcallback(Documentsimportedcallback);

                ProcessDataBeneficiary processed = new ProcessDataBeneficiary(beneficiarycollection, result, duplicates, documentsimported, callback1, callback2);
                Thread myThread = new Thread(() => processed.GetProcessedB(beneficiarycollection, result, duplicates, documentsimported));

                myThread.Start();

                myThread.Join();

                string docsimported = TempData.Peek("docsimported").ToString();
                duplicates = TempData.Peek("duplicates").ToString();

                return RedirectToAction("ImportUpdate", new { duplicates, docsimported });
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        public ActionResult ImportUpdate(string duplicates, string docsimported)
        {
            ViewBag.duplicates = duplicates;
            ViewBag.documentsimported = docsimported;
            return View();
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

        public ActionResult Index(string sortOrder, string searching, bool Active, string searchingBirthPlace, bool HasContract, bool Homeless, DateTime lowerdate, DateTime upperdate, DateTime activesince, DateTime activetill, int page, bool Weeklypackage, bool Canteen, bool HomeDelivery, string searchingDriver, bool HasGDPRAgreement, string searchingAddress, bool HasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpences, string gender)
        {
            try
            {
                if (activetill < activesince && activetill > DateTime.Now.AddYears(-2000))
                {
                    ViewBag.wrongorder = true;
                    RedirectToPage("Index");
                }
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
                ViewBag.Weeklypackage = Weeklypackage;
                ViewBag.Canteen = Canteen;
                ViewBag.Activesince = activesince;
                ViewBag.Activetill = activetill;
                ViewBag.HomeDelivery = HomeDelivery;
                ViewBag.searchingDriver = searchingDriver;
                ViewBag.HasGDPRAgreement = HasGDPRAgreement;
                ViewBag.searchingAddress = searchingAddress;
                ViewBag.HasID = HasID;
                ViewBag.searchingNumberOfPortions = searchingNumberOfPortions;
                ViewBag.searchingComments = searchingComments;
                ViewBag.searchingBirthPlace = searchingBirthPlace;
                ViewBag.searchingStudies = searchingStudies;
                ViewBag.searchingPO = searchingPO;
                ViewBag.searchingSeniority = searchingSeniority;
                ViewBag.searchingHealthState = searchingHealthState;
                ViewBag.searchingAddictions = searchingAddictions;
                ViewBag.searchingMarried = searchingMarried;
                ViewBag.searchingHealthInsurance = searchingHealthInsurance;
                ViewBag.searchingHealthCard = searchingHealthCard;
                ViewBag.searchingHasHome = searchingHasHome;
                ViewBag.searchingIncome = searchingIncome;
                ViewBag.searchingExpences = searchingExpences;
                ViewBag.gender = gender;

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
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.Fullname == null || b.Fullname == "")
                        { b.Fullname = "-"; }
                    }
                    try { beneficiaries = bene.Where(x => x.Fullname.Contains(searching)).ToList(); } catch { }
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
                if (activesince > d1 && activetill <= d1)
                {
                    string ids_to_remove = "";
                    foreach (Beneficiary vol in beneficiaries)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = ControllerHelper.Datereturner(vol.Activedates);
                        bool passed = false;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (startdates[j] > activesince || enddates[j] > activesince)
                            {
                                passed = true;
                                break;
                            }
                        }
                        if (!passed)
                        {
                            ids_to_remove = ids_to_remove + "," + ControllerHelper.Datereturner(vol.Activedates);
                        }
                    }
                    List<string> ids = ids_to_remove.Split(',').ToList();
                    foreach (string id in ids)
                    {
                        Beneficiary voltodelete = beneficiaries.FirstOrDefault(x => x.BeneficiaryID.ToString() == id);
                        beneficiaries.Remove(voltodelete);
                    }
                }
                //IN CASE THERE IS NO START DATE
                if (activesince < d1 && activetill > d1)
                {
                    string ids_to_remove = "";
                    foreach (Beneficiary vol in beneficiaries)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = ControllerHelper.Datereturner(vol.Activedates);
                        bool passed = false;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (startdates[j] < activetill || enddates[j] < activetill)
                            {
                                passed = true;
                                break;
                            }
                        }
                        if (!passed)
                        {
                            ids_to_remove = ids_to_remove + "," + vol.BeneficiaryID;
                        }
                    }
                    List<string> ids = ids_to_remove.Split(',').ToList();
                    foreach (string id in ids)
                    {
                        Beneficiary voltodelete = beneficiaries.FirstOrDefault(x => x.BeneficiaryID.ToString() == id);
                        beneficiaries.Remove(voltodelete);
                    }
                }
                //IN CASE THERE ARE BOTH
                if (activesince > d1 && activetill > d1)
                {
                    string ids_to_remove = "";

                    foreach (Beneficiary vol in beneficiaries)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = ControllerHelper.Datereturner(vol.Activedates);
                        bool passed = false;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (startdates[j] > activesince && startdates[j] < activetill)
                            {
                                passed = true;
                                break;
                            }
                            else if (enddates[j] > activesince && enddates[j] < activetill)
                            {
                                passed = true;
                                break;
                            }
                            else if (startdates[j] < activesince && enddates[j] > activetill)
                            {
                                passed = true;
                                break;
                            }
                        }
                        if (!passed)
                        {
                            ids_to_remove = ids_to_remove + "," + vol.BeneficiaryID;
                        }
                    }
                    List<string> ids = ids_to_remove.Split(',').ToList();
                    foreach (string id in ids)
                    {
                        Beneficiary voltodelete = beneficiaries.FirstOrDefault(x => x.BeneficiaryID.ToString() == id);
                        beneficiaries.Remove(voltodelete);
                    }
                }
                if (Weeklypackage == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.Weeklypackage == true).ToList();
                }
                if (Canteen == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.Canteen == true).ToList();
                }
                if (HomeDelivery == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.HomeDelivery == true).ToList();
                }

                if (HasGDPRAgreement == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.HasGDPRAgreement == true).ToList();
                }
                if (HasID == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.CI.HasId == true).ToList();
                }
                if (searchingHealthInsurance == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthInsurance == true).ToList();
                }
                if (searchingHealthCard == true)
                {
                    beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthCard == true).ToList();
                }

                if (searchingDriver != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.HomeDeliveryDriver == null || b.HomeDeliveryDriver == "")
                            b.HomeDeliveryDriver = "-";
                    }
                    beneficiaries = bene.Where(x => x.HomeDeliveryDriver.Contains(searchingDriver)).ToList();
                }

                if (searchingAddress != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.Adress == null || b.Adress == "")
                            b.Adress = "-";
                    }
                    beneficiaries = bene.Where(x => x.Adress.Contains(searchingAddress)).ToList();
                }

                if (searchingPO != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.Ocupation == null || b.PersonalInfo.Ocupation == "")
                            b.PersonalInfo.Ocupation = "-";
                        if (b.PersonalInfo.Profesion == null || b.PersonalInfo.Profesion == "")
                            b.PersonalInfo.Profesion = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Ocupation.Contains(searchingPO) || x.PersonalInfo.Profesion.Contains(searchingPO)).ToList();
                }

                if (searchingNumberOfPortions != 0)
                {
                    beneficiaries = beneficiaries.Where(x => x.NumberOfPortions.Equals(searchingNumberOfPortions)).ToList();
                }

                if (searchingComments != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.Comments == null || b.Comments == "")
                            b.Comments = "-";
                    }
                    beneficiaries = bene.Where(x => x.Comments.Contains(searchingComments)).ToList();
                }

                if (searchingBirthPlace != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.BirthPlace == null || b.PersonalInfo.BirthPlace == "")
                            b.PersonalInfo.BirthPlace = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.BirthPlace.Contains(searchingBirthPlace)).ToList();
                }

                if (searchingStudies != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.Studies == null || b.PersonalInfo.Studies == "")
                            b.PersonalInfo.Studies = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Studies.Contains(searchingStudies)).ToList();
                }

                if (searchingSeniority != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.SeniorityInWorkField == null || b.PersonalInfo.SeniorityInWorkField == "")
                            b.PersonalInfo.SeniorityInWorkField = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.SeniorityInWorkField.Contains(searchingSeniority)).ToList();
                }

                if (searchingHealthState != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.HealthState == null || b.PersonalInfo.HealthState == "")
                            b.PersonalInfo.HealthState = "-";
                        if (b.PersonalInfo.Disalility == null || b.PersonalInfo.Disalility == "")
                            b.PersonalInfo.Disalility = "-";
                        if (b.PersonalInfo.ChronicCondition == null || b.PersonalInfo.ChronicCondition == "")
                            b.PersonalInfo.ChronicCondition = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.HealthState.Contains(searchingHealthState) || x.PersonalInfo.Disalility.Contains(searchingHealthState) || x.PersonalInfo.ChronicCondition.Contains(searchingHealthState)).ToList();
                }

                if (searchingAddictions != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.Addictions == null || b.PersonalInfo.Addictions == "")
                            b.PersonalInfo.Addictions = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Addictions.Contains(searchingAddictions)).ToList();
                }

                if (searchingMarried != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.Married == null || b.PersonalInfo.Married == "")
                            b.PersonalInfo.Married = "-";
                        if (b.PersonalInfo.SpouseName == null || b.PersonalInfo.SpouseName == "")
                            b.PersonalInfo.SpouseName = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Married.Contains(searchingMarried) || x.PersonalInfo.SpouseName.Contains(searchingMarried)).ToList();
                }
                if (searchingIncome != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.Income == null || b.PersonalInfo.Income == "")
                            b.PersonalInfo.Income = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Income.Contains(searchingIncome)).ToList();
                }
                if (searchingHousingType != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.HousingType == null || b.PersonalInfo.HousingType == "")
                            b.PersonalInfo.HousingType = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Income.Contains(searchingHousingType)).ToList();
                }
                if (gender != " All")
                {
                    if (gender == "Male")
                    {
                        beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Gender.Equals(Gender.Male)).ToList();
                    }
                    if (gender == "Female")
                    { beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Gender.Equals(Gender.Female)).ToList(); }
                }
                if (searchingExpences != null)
                {
                    List<Beneficiary> bene = beneficiaries;
                    foreach (var b in bene)
                    {
                        if (b.PersonalInfo.Expences == null || b.PersonalInfo.Expences == "")
                            b.PersonalInfo.Expences = "-";
                    }
                    beneficiaries = bene.Where(x => x.PersonalInfo.Expences.Contains(searchingExpences)).ToList();
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

        [HttpGet]
        public ActionResult CSVSaver(string ids)
        {
            ViewBag.IDS = ids;
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(string IDS, bool PhoneNumber, bool SpouseName, bool Gender, bool Expences, bool Income, bool HousingType, bool HasHome, bool Married, bool HealthCard, bool HealthInsurance, bool Addictions, bool ChronicCondition, bool Disalility, bool HealthState, bool Profesion, bool SeniorityInWorkField, bool Ocupation, bool BirthPlace, bool Studies, bool CI_Info, bool IdContract, bool IdInvestigation, bool IdAplication, bool marca, bool All, bool CNP, bool Fullname, bool Active, bool Canteen, bool HomeDelivery, bool HomeDeliveryDriver, bool HasGDPRAgreement, bool Adress, bool NumberOfPortions, bool LastTimeActiv)
        {
            string ids_and_options = IDS + "(((";
            if (All == true)
                ids_and_options = ids_and_options + "0";
            if (Fullname == true)
                ids_and_options = ids_and_options + "1";
            if (Active == true)
                ids_and_options = ids_and_options + "2";
            if (Canteen == true)
                ids_and_options = ids_and_options + "3";
            if (HomeDelivery == true)
                ids_and_options = ids_and_options + "4";
            if (HomeDeliveryDriver == true)
                ids_and_options = ids_and_options + "5";
            if (HasGDPRAgreement == true)
                ids_and_options = ids_and_options + "6";
            if (Adress == true)
                ids_and_options = ids_and_options + "7";
            if (CNP == true)
                ids_and_options = ids_and_options + "8";
            if (CI_Info == true)
                ids_and_options = ids_and_options + "9";
            if (marca == true)
                ids_and_options = ids_and_options + "A";
            if (IdInvestigation == true)
                ids_and_options = ids_and_options + "B";
            if (IdAplication == true)
                ids_and_options = ids_and_options + "C";
            if (NumberOfPortions == true)
                ids_and_options = ids_and_options + "D";
            if (LastTimeActiv == true)
                ids_and_options = ids_and_options + "E";
            if (PhoneNumber == true)
                ids_and_options = ids_and_options + "F";
            if (BirthPlace == true)
                ids_and_options = ids_and_options + "G";
            if (Studies == true)
                ids_and_options = ids_and_options + "H";
            if (Profesion == true)
                ids_and_options = ids_and_options + "I";
            if (Ocupation == true)
                ids_and_options = ids_and_options + "J";
            if (SeniorityInWorkField == true)
                ids_and_options = ids_and_options + "K";
            if (HealthState == true)
                ids_and_options = ids_and_options + "L";
            if (Disalility == true)
                ids_and_options = ids_and_options + "M";
            if (ChronicCondition == true)
                ids_and_options = ids_and_options + "N";
            if (Addictions == true)
                ids_and_options = ids_and_options + "O";
            if (HealthInsurance == true)
                ids_and_options = ids_and_options + "Z";
            if (HealthCard == true)
                ids_and_options = ids_and_options + "P";
            if (Married == true)
                ids_and_options = ids_and_options + "Q";
            if (SpouseName == true)
                ids_and_options = ids_and_options + "R";
            if (HasHome == true)
                ids_and_options = ids_and_options + "S";
            if (HousingType == true)
                ids_and_options = ids_and_options + "T";
            if (Income == true)
                ids_and_options = ids_and_options + "U";
            if (Expences == true)
                ids_and_options = ids_and_options + "V";
            if (Gender == true)
                ids_and_options = ids_and_options + "W";

            ids_and_options = "csvexporterapp:" + ids_and_options;

            return Redirect(ids_and_options);

            //return RedirectToAction("Index");
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
        public ActionResult Create(Beneficiary beneficiary, List<IFormFile> Image)
        {
            try
            {
                string volasstring = JsonConvert.SerializeObject(beneficiary);
                bool containsspecialchar = false;
                if (volasstring.Contains(";"))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                    containsspecialchar = true;
                }
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ModelState.Remove("CI.ExpirationDateCI");
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
                    foreach (var item in Image)
                    {
                        if (item.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                item.CopyTo(stream);
                                beneficiary.Image = stream.ToArray();
                            }
                        }
                    }
                    if (beneficiary.Active == true)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                        beneficiary.Activedates = beneficiary.Activedates + "," + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
                        beneficiary.Activedates = beneficiary.Activedates.Replace(" ", "");
                        beneficiary.Activedates = beneficiary.Activedates.Replace(".", "/");
                    }
                    beneficiarycollection.InsertOne(beneficiary);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.containsspecialchar = containsspecialchar;
                    return View();
                }
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
                ViewBag.id = id;
                return View(beneficiary);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Beneficiary/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Beneficiary beneficiary, string Originalsavedvolstring, IList<IFormFile> image)
        {
            try
            {
                string volasstring = JsonConvert.SerializeObject(beneficiary);
                bool containsspecialchar = false;
                if (volasstring.Contains(";"))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                    containsspecialchar = true;
                }
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
                        ModelState.Remove("CI.ExpirationDateCI");
                        ModelState.Remove("Marca.IdContract");
                        ModelState.Remove("Marca.IdInvestigation");
                        ModelState.Remove("NumberOfPortions");
                        ModelState.Remove("LastTimeActiv");
                        ModelState.Remove("Personalinfo.Birthdate");
                        ModelState.Remove("CI.ICExpirationDate");
                        foreach (var item in image)
                        {
                            if (item.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    item.CopyTo(stream);
                                    beneficiary.Image = stream.ToArray();
                                }
                            }
                        }
                        bool wasactive = false;

                        if (Originalsavedvol.Active == true)
                        {
                            wasactive = true;
                        }
                        if (beneficiary.Active == false && wasactive == true)
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                            beneficiary.Activedates = beneficiary.Activedates.Replace("currently", DateTime.Now.AddHours(5).ToShortDateString());
                            beneficiary.Activedates = beneficiary.Activedates.Replace(" ", "");
                            beneficiary.Activedates = beneficiary.Activedates.Replace(".", "/");
                        }
                        if (beneficiary.Active == true && wasactive == false)
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                            beneficiary.Activedates = beneficiary.Activedates + ", " + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
                            beneficiary.Activedates = beneficiary.Activedates.Replace(" ", "");
                            beneficiary.Activedates = beneficiary.Activedates.Replace(".", "/");
                        }

                        if (ModelState.IsValid)
                        {
                            var filter = Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id));
                            var update = Builders<Beneficiary>.Update
                               .Set("Fullname", beneficiary.Fullname)
                               .Set("Image", beneficiary.Image)
                               .Set("Weeklypackage", beneficiary.Weeklypackage)
                               .Set("Active", beneficiary.Active)
                               .Set("Canteen", beneficiary.Canteen)
                               .Set("HomeDeliveryDriver", beneficiary.HomeDeliveryDriver)
                               .Set("HasGDPRAgreement", beneficiary.HasGDPRAgreement)
                               .Set("CNP", beneficiary.CNP)
                               .Set("NumberOfPortions", beneficiary.NumberOfPortions)
                               .Set("Comments", beneficiary.Comments)
                               .Set("Adress", beneficiary.Adress)
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
                               .Set("Activedates", beneficiary.Activedates)
                               .Set("PersonalInfo.Studies", beneficiary.PersonalInfo.Studies);

                            var result = beneficiarycollection.UpdateOne(filter, update);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.originalsavedvol = Originalsavedvolstring;
                            ViewBag.id = id;
                            ViewBag.containsspecialchar = containsspecialchar;
                            return View();
                        }
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
                        if (beneficiary.Active == false)
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                            beneficiary.Activedates = beneficiary.Activedates.Replace("currently", DateTime.Now.AddHours(5).ToShortDateString());
                            beneficiary.Activedates = beneficiary.Activedates.Replace(" ", "");
                            beneficiary.Activedates = beneficiary.Activedates.Replace(".", "/");
                        }
                        var filter = Builders<Beneficiary>.Filter.Eq("_id", ObjectId.Parse(id));
                        var update = Builders<Beneficiary>.Update
                            .Set("Active", beneficiary.Active)
                            .Set("Activedates", beneficiary.Activedates);
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