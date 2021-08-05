using BucuriaDarului.Contexts.BeneficiaryContexts;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
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
        public ActionResult Import(IFormFile files)
        {
            var beneficiaryImportContext = new BeneficiaryImportContext(new BeneficiaryImportGateway());
            var response = new BeneficiaryImportResponse();
            if (files != null)
                response = beneficiaryImportContext.Execute(files.OpenReadStream());
            else
            {
                response.Message.Add(new KeyValuePair<string, string>("NoFile", "Please choose a file!"));
                response.IsValid = false;
            }
            if (response.IsValid)
                return RedirectToAction("Import", new { message = "The Document has been successfully imported" });
            return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        public ActionResult Contracts(string id)
        {
            return RedirectToAction("Index", "Beneficiarycontract", new { idofbeneficiary = id });
        }

        public ActionResult Index(string sortOrder, string searching, bool active, string searchingBirthPlace, bool hasContract, bool homeless, DateTime lowerDate, DateTime upperDate, DateTime activeSince, DateTime activeTill, int page, bool weeklyPackage, bool canteen, bool homeDelivery, string searchingDriver, bool hasGDPRAgreement, string searchingAddress, bool hasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpenses, string gender)
        {
            HttpContext.Session.SetString("queryString", Request.QueryString.ToString());
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var beneficiariesMainDisplayIndexContext = new BeneficiariesMainDisplayIndexContext(new BeneficiariesMainDisplayIndexGateway());
            var model = beneficiariesMainDisplayIndexContext.Execute(new BeneficiariesMainDisplayIndexRequest(searching, page, nrOfDocs, sortOrder, active, searchingBirthPlace, hasContract, homeless, lowerDate, upperDate, activeSince, activeTill, weeklyPackage, canteen, homeDelivery, searchingDriver, hasGDPRAgreement, searchingAddress, hasID, searchingNumberOfPortions, searchingComments, searchingStudies, searchingPO, searchingSeniority, searchingHealthState, searchingAddictions, searchingMarried, searchingHealthInsurance, searchingHealthCard, searchingHasHome, searchingHousingType, searchingIncome, searchingExpenses, gender));
            HttpContext.Session.SetString(model.DictionaryKey, model.StringOfIDs);
            return View(model);
        }

        [HttpGet]
        public ActionResult CsvExporter(string dictionaryKey, string message)
        {
            string StringOfIds = HttpContext.Session.GetString(dictionaryKey);
            ViewBag.Ids = StringOfIds;
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult CsvExporter(ExportParameters csvExportProperties)
        {
            var beneficiaryExporterContext = new BeneficiaryExporterContext(_localizer);
            var beneficiaryExportData = beneficiaryExporterContext.Execute(new BeneficiaryExporterRequest(csvExportProperties));
            DictionaryHelper.d = beneficiaryExportData.Dictionary;
            if (beneficiaryExportData.IsValid && beneficiaryExportData.FileName != "")
                return DownloadCSV(beneficiaryExportData.FileName, "beneficiarySession", "beneficiaryHeader");
            return RedirectToAction("CsvExporter", new { message = "Please select at least one Property!" });
        }

        public FileContentResult DownloadCSV(string fileName, string idsKey, string headerKey)
        {
            string ids = string.Empty;
            string header = string.Empty;
            DictionaryHelper.d.TryGetValue(idsKey, out ids);
            DictionaryHelper.d.TryGetValue(headerKey, out header);

            var context = new BeneficiaryDownloadContext(new BeneficiaryDownloadGateway());
            var respons = context.Execute(ids, header);

            return File(new System.Text.UTF8Encoding().GetBytes(respons.ToString()), "text/csv", fileName);
        }

        public ActionResult Details(string id)
        {
            var model = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(id);
            return View(model);
        }

        public ActionResult Create(string message)
        {
            ViewBag.message = message;
            return View();
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
            if (!beneficiaryCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { message = beneficiaryCreateResponse.Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id, string message)
        {
            ViewBag.message = message;
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

            if (!beneficiaryEditResponse.IsValid)
            {
                return RedirectToAction("Edit", new { id = request.Id, message = beneficiaryEditResponse.Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var model = SingleBeneficiaryReturnerGateway.ReturnBeneficiary(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(bool inactive, string id)
        {
            var deleteBeneficiaryContext = new BeneficiaryDeleteContext(new BeneficiaryDeleteGateway());
            deleteBeneficiaryContext.Execute(inactive, id);
            return RedirectToAction("Index");
        }
    }
}