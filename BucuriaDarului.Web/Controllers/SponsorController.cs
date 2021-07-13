using BucuriaDarului.Gateway.SponsorGateways;
using BucuriaDarului.Contexts.SponsorContexts;
// using BucuriaDarului.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.ControllerHelpers.SponsorHelpers;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using BucuriaDarului.Web.DatabaseManager;
using BucuriaDarului.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace BucuriaDarului.Web.Controllers
{
    public class SponsorController : Controller
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.DATABASE_NAME_LOCAL);

        private readonly IStringLocalizer<SponsorController> _localizer;

        private EventManager eventManager = new EventManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();
        private AuxiliaryDBManager auxiliaryDBManager = new AuxiliaryDBManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private SponsorManager sponsorManager = new SponsorManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

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
        public ActionResult Import(IFormFile Files, string message)
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
            if (sponsorExportData.IsValid)
                return Redirect("csvexporterapp:sponsorSession;sponsorHeader");
            return RedirectToAction("CsvExporter", new { message = "Please select at least one Property!" });
        }

        // TODO: what is the difference between searching(name) & ContactInfo? in the Sponsor class ContactInformation is a class with 2 strings as fields
        // TODO: ContactInfo is the phone or email????
        public IActionResult Index(string searching, int page, string ContactInfo, DateTime lowerdate, DateTime upperdate, bool HasContract, string WhatGoods, string MoneyAmount, string GoodsAmounts)

        {
            //var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            //var sponsorsMainDisplayIndexContext = new SponsorsMainDisplayIndexContext(new SponsorMainDisplayIndexGateway());
            //var model = sponsorsMainDisplayIndexContext.Execute(new SponsorsMainDisplayIndexRequest(searching, page, nrOfDocs, ContactInfo, lowerDate, upperDate, hasContract, WhatGoods, MoneyAmount, GoodsAmount));
            //return View(model);
            try
            {
                if (searching != null)
                { ViewBag.Filters1 = searching; }
                if (ContactInfo != null)
                { ViewBag.Filters2 = ContactInfo; }
                if (HasContract == true)
                { ViewBag.Filters3 = ""; }
                if (WhatGoods != null)
                { ViewBag.Filters4 = WhatGoods; }
                if (MoneyAmount != null)
                { ViewBag.Filters5 = MoneyAmount; }
                if (GoodsAmounts != null)
                { ViewBag.Filters6 = GoodsAmounts; }
                DateTime date = Convert.ToDateTime("01.01.0001 00:00:00");
                if (lowerdate != date)
                { ViewBag.Filter7 = lowerdate.ToString(); }
                if (upperdate != date)
                { ViewBag.Filter8 = upperdate.ToString(); }
                ViewBag.Contact = ContactInfo;
                ViewBag.searching = searching;
                ViewBag.Upperdate = upperdate;
                ViewBag.Lowerdate = lowerdate;
                ViewBag.HasContract = HasContract;
                ViewBag.WhatGoods = WhatGoods;
                ViewBag.GoodsAmount = GoodsAmounts;
                ViewBag.MoneyAmount = MoneyAmount;

                List<Sponsor> sponsors = sponsorManager.GetListOfSponsors();
                page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.page = page;
                sponsors = SponsorFunctions.GetSponsorsAfterFilters(sponsors, searching, ContactInfo, lowerdate, upperdate, HasContract, WhatGoods, MoneyAmount, GoodsAmounts);
                ViewBag.counter = sponsors.Count();
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = SponsorFunctions.GetStringOfIds(sponsors);
                ViewBag.stringofids = stringofids;
                sponsors = SponsorFunctions.GetSponsorsAfterPaging(sponsors, page, nrofdocs);
                string key = Constants.SESSION_KEY_SPONSOR;
                HttpContext.Session.SetString(key, stringofids);

                return View(sponsors);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
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