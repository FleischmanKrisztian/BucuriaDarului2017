﻿using BucuriaDarului.Contexts.BeneficiaryContexts;
using BucuriaDarului.Gateway;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using BucuriaDarului.Web.Common;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.BeneficiaryHelpers;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;

namespace BucuriaDarului.Web.Controllers
{
    public class BeneficiaryController : Controller
    {
        private readonly IStringLocalizer<BeneficiaryController> _localizer;

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

        public ActionResult Index(string sortOrder, string searching, bool active, string searchingBirthPlace, bool hasContract, bool homeless, DateTime lowerDate, DateTime upperDate, DateTime activeSince, DateTime activeTill, int page, bool weeklyPackage, bool canteen, bool homeDelivery, string searchingDriver, bool hasGDPRAgreement, string searchingAddress, bool hasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpences, string gender)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var beneficiariesMainDisplayIndexContext = new BeneficiariesMainDisplayIndexContext(new BeneficiariesMainDisplayIndexGateway());
            var model = beneficiariesMainDisplayIndexContext.Execute(new BucuriaDarului.Contexts.BeneficiaryContexts.BeneficiariesMainDisplayIndexRequest(searching, page, nrOfDocs, sortOrder, active, searchingBirthPlace, hasContract, homeless, lowerDate, upperDate, activeSince, activeTill, weeklyPackage, canteen, homeDelivery, searchingDriver, hasGDPRAgreement, searchingAddress, hasID, searchingNumberOfPortions, searchingComments, searchingStudies, searchingPO, searchingSeniority, searchingHealthState, searchingAddictions, searchingMarried, searchingHealthInsurance, searchingHealthCard, searchingHasHome, searchingHousingType, searchingIncome, searchingExpences, gender));
            return View(model);
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