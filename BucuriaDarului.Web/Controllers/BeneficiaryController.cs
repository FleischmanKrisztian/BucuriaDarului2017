using BucuriaDarului.Contexts.BeneficiaryContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using BucuriaDarului.Web.Common;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.BeneficiaryHelpers;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.LocalDatabaseManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BucuriaDarului.Web.Controllers
{
    public class BeneficiaryController : Controller
    {
        private readonly IStringLocalizer<BeneficiaryController> _localizer;
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.DATABASE_NAME_LOCAL);

        private BeneficiaryManager beneficiaryManager = new BeneficiaryManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        public BeneficiaryController(IStringLocalizer<BeneficiaryController> localizer)
        {
            _localizer = localizer;
        }

        public ActionResult Import(string message)
        {
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile Files)
        {
            var beneficiaryImportContext = new BeneficiaryImportContext(new BeneficiaryImportGateway());
            var response = beneficiaryImportContext.Execute(Files.OpenReadStream());
            if (response.IsValid)
                return RedirectToAction("Import", new { message = "The Document has successfully been imported" });
            else
                return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        public ActionResult Contracts(string id)
        {
            return RedirectToAction("Index", "Beneficiarycontract", new { idofbeneficiary = id });
        }

        public ActionResult Index(string sortOrder, string searching, bool Active, string searchingBirthPlace, bool HasContract, bool Homeless, DateTime lowerdate, DateTime upperdate, DateTime activesince, DateTime activetill, int page, bool Weeklypackage, bool Canteen, bool HomeDelivery, string searchingDriver, bool HasGDPRAgreement, string searchingAddress, bool HasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpences, string gender)
        {
            try
            {
                if (searching != null)
                { ViewBag.Filters1 = searching; }
                if (Active == true)
                { ViewBag.Filters2 = ""; }
                if (searchingBirthPlace != null)
                { ViewBag.Filters3 = searchingBirthPlace; }
                if (HasContract == true)
                { ViewBag.Filters4 = ""; }
                if (Homeless == true)
                { ViewBag.Filters5 = ""; }
                if (Weeklypackage == true)
                { ViewBag.Filters6 = ""; }
                if (Canteen == true)
                { ViewBag.Filters7 = ""; }
                if (HomeDelivery == true)
                { ViewBag.Filters8 = ""; }
                if (searchingDriver != null)
                { ViewBag.Filter9 = searchingDriver; }
                if (HasGDPRAgreement == true)
                { ViewBag.Filters10 = ""; }
                if (searchingAddress != null)
                { ViewBag.Filters11 = searchingAddress; }
                if (HasID == true)
                { ViewBag.Filters12 = ""; }
                if (searchingNumberOfPortions != 0)
                { ViewBag.Filters13 = searchingNumberOfPortions.ToString(); }
                if (searchingComments != null)
                { ViewBag.Filters14 = searchingComments; }
                if (searchingStudies != null)
                { ViewBag.Filters15 = searchingStudies; }
                if (searchingPO != null)
                { ViewBag.Filters16 = searchingPO; }
                if (searchingSeniority != null)
                { ViewBag.Filters17 = searchingSeniority; }
                if (searchingHealthState != null)
                { ViewBag.Filters18 = searchingHealthState; }
                if (searchingAddictions != null)
                { ViewBag.Filters19 = searchingAddictions; }
                if (searchingMarried != null)
                { ViewBag.Filters20 = searchingMarried; }
                if (searchingHealthInsurance == true)
                { ViewBag.Filters21 = ""; }
                if (searchingHealthCard == true)
                { ViewBag.Filters22 = ""; }
                if (searchingHasHome == true)
                { ViewBag.Filters23 = ""; }
                if (searchingHousingType != null)
                { ViewBag.Filters24 = searchingHousingType; }
                if (searchingIncome != null)
                { ViewBag.Filters25 = searchingIncome; }
                if (searchingExpences != null)
                { ViewBag.Filters26 = searchingExpences; }
                if (gender != null)
                { ViewBag.Filters27 = gender; }
                DateTime date = Convert.ToDateTime("01.01.0001 00:00:00");
                if (lowerdate != date)
                { ViewBag.Filter28 = lowerdate.ToString(); }
                if (upperdate != date)
                { ViewBag.Filter29 = upperdate.ToString(); }
                if (activesince != date)
                { ViewBag.Filter30 = activesince.ToString(); }
                if (activetill != date)
                { ViewBag.Filter31 = activetill.ToString(); }

                ViewBag.SortOrder = sortOrder;
                ViewBag.searching = searching;
                ViewBag.active = Active;
                ViewBag.hascontract = HasContract;
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
                List<Beneficiary> beneficiaries = beneficiaryManager.GetListOfBeneficiaries();
                page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.page = page;
                beneficiaries = BeneficiaryFunctions.GetBeneficiariesAfterFilters(beneficiaries, sortOrder, searching, Active, searchingBirthPlace, HasContract, Homeless, lowerdate, upperdate, activesince, activetill, page, Weeklypackage, Canteen, HomeDelivery, searchingDriver, HasGDPRAgreement, searchingAddress, HasID, searchingNumberOfPortions, searchingComments, searchingStudies, searchingPO, searchingSeniority, searchingHealthState, searchingAddictions, searchingMarried, searchingHealthInsurance, searchingHealthCard, searchingHasHome, searchingHousingType, searchingIncome, searchingExpences, gender);
                ViewBag.counter = beneficiaries.Count();
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = BeneficiaryFunctions.GetStringOfIds(beneficiaries);
                ViewBag.stringofids = stringofids;
                beneficiaries = BeneficiaryFunctions.GetBeneficiariesAfterPaging(beneficiaries, page, nrofdocs);
                string key = Constants.SESSION_KEY_BENEFICIARY;
                HttpContext.Session.SetString(key, stringofids);

                return View(beneficiaries);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult CsvExporter()
        {
            string ids = HttpContext.Session.GetString(Constants.SESSION_KEY_BENEFICIARY);
            HttpContext.Session.Remove(Constants.SESSION_KEY_BENEFICIARY);
            string key = Constants.SECONDARY_SESSION_KEY_BENEFICIARY;
            HttpContext.Session.SetString(key, ids);

            return View();
        }

        [HttpPost]
        public ActionResult CsvExporter(bool All, bool PhoneNumber, bool SpouseName, bool Gender, bool Expences, bool Income, bool HousingType, bool HasHome, bool Married, bool HealthCard, bool HealthInsurance, bool Addictions, bool ChronicCondition, bool Disalility, bool HealthState, bool Profesion, bool SeniorityInWorkField, bool Ocupation, bool BirthPlace, bool Studies, bool CI_Info, bool IdContract, bool IdInvestigation, bool IdAplication, bool marca, bool CNP, bool Fullname, bool Active, bool Canteen, bool HomeDelivery, bool HomeDeliveryDriver, bool HasGDPRAgreement, bool Adress, bool NumberOfPortions, bool LastTimeActiv, bool WeeklyPackage)
        {
            string IDS = HttpContext.Session.GetString(Constants.SECONDARY_SESSION_KEY_BENEFICIARY);
            HttpContext.Session.Remove(Constants.SECONDARY_SESSION_KEY_BENEFICIARY);
            string ids_and_fields = BeneficiaryFunctions.GetIdAndFieldString(IDS, PhoneNumber, SpouseName, Gender, Expences, Income, HousingType, HasHome, Married, HealthCard, HealthInsurance, Addictions, ChronicCondition, Disalility, HealthState, Profesion, SeniorityInWorkField, Ocupation, BirthPlace, Studies, CI_Info, IdContract, IdInvestigation, IdAplication, marca, All, CNP, Fullname, Active, Canteen, HomeDelivery, HomeDeliveryDriver, HasGDPRAgreement, Adress, NumberOfPortions, LastTimeActiv, WeeklyPackage);
            string key1 = Constants.BENEFICIARYSESSION;
            string header = ControllerHelper.GetHeaderForExcelPrinterBeneficiary(_localizer);
            string key2 = Constants.BENEFICIARYHEADER;
            ControllerHelper.CreateDictionaries(key1, key2, ids_and_fields, header);
            string csvexporterlink = "csvexporterapp:" + key1 + ";" + key2;
            return Redirect(csvexporterlink);
        }

        public ActionResult Details(string id)
        {
            try
            {
                var model = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(id);
                return View(model);
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
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Create(BeneficiaryCreateRequest request, IFormFile image)
        {
            var beneficiaryCreateContext = new BeneficiaryCreateContext(new BeneficiaryCreateGateway());
            var fileBytes = new byte[0];
            if (image != null)
            {
                if (image.Length > 0)
                {
                    using var ms = new MemoryStream();
                    image.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            var beneficiaryCreateResponse = beneficiaryCreateContext.Execute(request, fileBytes);
            ModelState.Remove("Contract.RegistrationDate");
            ModelState.Remove("Contract.ExpirationDate");
            ModelState.Remove("Marca.IdApplication");
            ModelState.Remove("Marca.IdContract");
            ModelState.Remove("Marca.IdInvestigation");
            ModelState.Remove("NumberOfPortions");
            ModelState.Remove("LastTimeActive");
            ModelState.Remove("PersonalInfo.Birthdate");
            ModelState.Remove("CI.ExpirationDate");
            if (beneficiaryCreateResponse.ContainsSpecialChar)
            {
                ViewBag.ContainsSpecialChar = true;
                return View();
            }
            else if (!beneficiaryCreateResponse.IsValid)
            {
                ModelState.AddModelError("Fullname", "Name Of Beneficiary must not be empty");
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string id)
        {
            var model = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BeneficiaryEditRequest request, IFormFile image)
        {
            var fileBytes = new byte[0];
            if (image != null)
            {
                if (image.Length > 0)
                {
                    using var ms = new MemoryStream();
                    image.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }
            var beneficiaryEditContext = new BeneficiaryEditContext(new BeneficiaryEditGateway());
            var beneficiaryEditResponse = beneficiaryEditContext.Execute(request, fileBytes);
            ModelState.Remove("Contract.RegistrationDate");
            ModelState.Remove("Contract.ExpirationDate");
            ModelState.Remove("Marca.IdApplication");
            ModelState.Remove("Marca.IdContract");
            ModelState.Remove("Marca.IdInvestigation");
            ModelState.Remove("NumberOfPortions");
            ModelState.Remove("LastTimeActive");
            ModelState.Remove("PersonalInfo.Birthdate");
            ModelState.Remove("CI.ExpirationDate");
            if (beneficiaryEditResponse.ContainsSpecialChar)
            {
                ViewBag.ContainsSpecialChar = true;
                return View(beneficiaryEditResponse.Beneficiary);
            }
            else if (!beneficiaryEditResponse.IsValid)
            {
                ModelState.AddModelError("Fullname", "Name Of Beneficiary must not be empty");
                return View(beneficiaryEditResponse.Beneficiary);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string id)
        {
            var model = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            BeneficiaryDeleteGateway.DeleteBeneficiary(id);
            return RedirectToAction("Index");
        }
    }
}