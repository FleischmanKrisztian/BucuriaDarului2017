using BucuriaDarului.Contexts.SponsorContexts;
using BucuriaDarului.Gateway.SponsorGateways;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using BucuriaDarului.Core;

namespace BucuriaDarului.Web.Controllers
{
    public class SponsorController : Controller
    {
        private readonly IStringLocalizer<SponsorController> _localizer;

        public SponsorController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, IStringLocalizer<SponsorController> localizer)
        {
            _localizer = localizer;
        }

        public ActionResult Import(string message)
        {
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile files, string overwrite)
        {
            var sponsorsImportContext = new SponsorsImportContext(new SponsorsImportDataGateway());
            var response = new SponsorImportResponse();
            if (files != null)
                response = sponsorsImportContext.Execute(files.OpenReadStream(),overwrite);
            else
            {
                response.Message.Add(new KeyValuePair<string, string>("NoFile", @_localizer["Please choose a file!"]));
                response.IsValid = false;
            }
            if (response.IsValid)
                return RedirectToAction("Import", new { message = @_localizer["The Document has been successfully imported"] });
            return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        [HttpGet]
        public ActionResult CSVExporter(string dictionaryKey, string message)
        {
            string StringOfIds = HttpContext.Session.GetString(dictionaryKey);
            ViewBag.Ids = StringOfIds;
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult CsvExporter(ExportParameters csvExportProperties)
        {
            var sponsorExporterContext = new SponsorExporterContext(_localizer);
            var sponsorExportData = sponsorExporterContext.Execute(new SponsorExporterRequest(csvExportProperties));
            DictionaryHelper.d = sponsorExportData.Dictionary;
            if (sponsorExportData.IsValid && sponsorExportData.FileName != "")
                return DownloadCSV(sponsorExportData.FileName, Constants.SPONSOR_SESSION, Constants.SPONSOR_HEADER);
            return RedirectToAction("CsvExporter", new { dictionaryKey = Constants.SPONSOR_SESSION, message =_localizer["Please select at least one Property!"] });
        }

        public FileContentResult DownloadCSV(string fileName, string idsKey, string headerKey)
        {
            DictionaryHelper.d.TryGetValue(idsKey, out var ids);
            DictionaryHelper.d.TryGetValue(headerKey, out var header);
            var context = new SponsorDownloadContext(new SponsorDownloadGateway());
            var response = context.Execute(ids, header);

            return File(new System.Text.UTF8Encoding().GetBytes(response.ToString()), "text/csv", fileName);
        }

        public IActionResult Index(string sponsorName, int page, string contactInfo, DateTime lowerDate, DateTime upperDate, bool hasContract, string whatGoods, string moneyAmount, string goodsAmounts)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var sponsorsMainDisplayIndexContext = new SponsorsMainDisplayIndexContext(new SponsorMainDisplayIndexGateway());
            var model = sponsorsMainDisplayIndexContext.Execute(new SponsorsMainDisplayIndexRequest(sponsorName, page, nrOfDocs, contactInfo, lowerDate, upperDate, hasContract, whatGoods, moneyAmount, goodsAmounts));
            HttpContext.Session.SetString(model.DictionaryKey, model.StringOfIDs);
            return View(model);
        }

        public ActionResult ContractExp()
        {
            var nrOfDays = UniversalFunctions.GetNumberOfDaysBeforeExpiration(TempData);
            var contractExpirationContext = new SponsorContractsExpirationContext(new SponsorContractExpirationGateway());
            var contracts = contractExpirationContext.Execute(nrOfDays);
            return View(contracts);
        }

        public ActionResult Details(string id)
        {
            var model = SingleSponsorReturnerGateway.ReturnSponsor(id);
            return View(model);
        }

        public ActionResult Create(string message)
        {
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Create(SponsorCreateRequest request)
        {
            var sponsorCreateContext = new SponsorCreateContext(new SponsorCreateGateway());
            var sponsorCreateResponse = sponsorCreateContext.Execute(request);

            if (!sponsorCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { message = @_localizer[sponsorCreateResponse.Message] });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleSponsorReturnerGateway.ReturnSponsor(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SponsorEditRequest request)
        {
            var sponsorEditContext = new SponsorEditContext(new SponsorEditGateway());
            var sponsorEditResponse = sponsorEditContext.Execute(request);

            if (!sponsorEditResponse.IsValid)
            {
                return RedirectToAction("Edit", new { id = request.Id, message = @_localizer[sponsorEditResponse.Message] });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var model = SingleSponsorReturnerGateway.ReturnSponsor(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            SponsorDeleteGateway.DeleteSponsor(id);
            return RedirectToAction("Index");
        }
    }
}