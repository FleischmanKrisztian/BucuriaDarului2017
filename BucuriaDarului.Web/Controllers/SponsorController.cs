using BucuriaDarului.Contexts.SponsorContexts;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using BucuriaDarului.Gateway.SponsorGateways;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System;

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
        public ActionResult Import(IFormFile Files)
        {
            var sponsorsImportContext = new SponsorsImportContext(new SponsorsImportDataGateway());
            var response = sponsorsImportContext.Execute(Files.OpenReadStream());
            if (response.IsValid)
                return RedirectToAction("Import", new { message = "The Document has successfully been imported" });
            else
                return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        [HttpGet]
        public ActionResult CSVExporter(string message)
        {
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
                return DownloadCSV(sponsorExportData.FileName, "sponsorSession", "sponsorHeader");
            return RedirectToAction("CsvExporter", new { message = "Please select at least one Property!" });
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
            return View(model);
        }

        public ActionResult ContractExp()
        {
            var contractExpirationContext = new SponsorContractsExpirationContext(new SponsorContractExpirationGateway());
            var contracts = contractExpirationContext.Execute();
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
            ModelState.Remove("Contract.RegistrationDate");
            ModelState.Remove("Contract.ExpirationDate");
            ModelState.Remove("Sponsorship.Date");

            if (!sponsorCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { message = sponsorCreateResponse.Message });
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
            ModelState.Remove("Contract.RegistrationDate");
            ModelState.Remove("Contract.ExpirationDate");
            ModelState.Remove("Sponsorship.Date");

            if (!sponsorEditResponse.IsValid)
            {
                return RedirectToAction("Edit", new { id = request.Id, message = sponsorEditResponse.Message });
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