using BucuriaDarului.Contexts.EventContexts;
using BucuriaDarului.Gateway.EventGateways;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IStringLocalizer<EventController> _localizer;

        public EventController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, IStringLocalizer<EventController> localizer)
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
            var eventsImportContext = new EventsImportContext(new EventsImportDataGateway());
            var response = new EventImportResponse();
            if (files != null)
                response = eventsImportContext.Execute(files.OpenReadStream());
            else
            {
                response.Message.Add(new KeyValuePair<string, string>("NoFile", "Please choose a file!"));
                response.IsValid = false;
            }
            if (response.IsValid)
                return RedirectToAction("Import", new { message = "The Document has been successfully imported" });
            return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        public ActionResult Index(string searching, int page, string searchingPlace, string searchingActivity, string searchingType, string searchingVolunteers, string searchingSponsor, DateTime lowerDate, DateTime upperDate)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            HttpContext.Session.SetString("queryString", Request.QueryString.ToString());
            var eventsMainDisplayIndexContext = new EventsMainDisplayIndexContext(new EventsMainDisplayIndexGateway());
            var model = eventsMainDisplayIndexContext.Execute(new EventsMainDisplayIndexRequest(searching, page, nrOfDocs, searchingPlace, searchingActivity, searchingType, searchingVolunteers, searchingSponsor, lowerDate, upperDate));
            return View(model);
        }

        [HttpGet]
        public ActionResult CsvExporter(string message)
        {
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult CsvExporter(ExportParameters csvExportProperties)
        {
            var eventsExporterContext = new EventsExporterContext(_localizer);
            var eventsExportData = eventsExporterContext.Execute(new EventsExporterRequest(csvExportProperties));
            DictionaryHelper.d = eventsExportData.Dictionary;
            if (eventsExportData.IsValid && eventsExportData.FileName != "")
                return DownloadCSV(eventsExportData.FileName, "eventSession", "eventHeader");

            return RedirectToAction("CsvExporter", new { message = "Please select at least one Property!" });
        }

        public FileContentResult DownloadCSV(string fileName, string idsKey, string headerKey)
        {
            DictionaryHelper.d.TryGetValue(idsKey, out var ids);
            DictionaryHelper.d.TryGetValue(headerKey, out var header);

            var context = new EventDownloadContext(new EventDownloadGateway());
            var response = context.Execute(ids, header);

            return File(new System.Text.UTF8Encoding().GetBytes(response.ToString()), "text/csv", fileName);
        }

        public ActionResult VolunteerAllocationDisplay(string id, string messages, int page, string searching)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var allocatedVolunteerContext = new EventVolunteerAllocationDisplayContext(new EventVolunteerAllocationDataGateway());
            var model = allocatedVolunteerContext.Execute(new EventsVolunteerAllocationDisplayRequest(id, page, nrOfDocs, searching, messages));
            model.Query = HttpContext.Session.GetString("queryString");

            return View(model);
        }

        [HttpPost]
        public ActionResult VolunteerAllocation(string[] CheckedIds, string[] AllIds, string evId)
        {
            var allocatedVolunteerUpdateContext = new EventVolunteerAllocationUpdateContext(new EventVolunteerAllocationUpdateGateway());
            var response = allocatedVolunteerUpdateContext.Execute(new EventsVolunteerAllocationRequest(CheckedIds, AllIds, evId));

            if (response.IsValid)
                return RedirectToAction("VolunteerAllocationDisplay", new { id = evId, messages = "The event has been successfully updated!", page = 1, searching = "" });
            else
                return RedirectToAction("VolunteerAllocationDisplay", new { id = evId, messages = "Update failed!Please try again!", page = 1, searching = "" });
        }

        public ActionResult SponsorAllocationDisplay(string id, string messages, int page, string searching)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var allocatedSponsorContext = new EventSponsorAllocationDisplayContext(new EventsSponsorAllocationDataGateway());
            var model = allocatedSponsorContext.Execute(new EventsSponsorsAllocationDisplayRequest(id, page, nrOfDocs, searching, messages));
            model.Query = HttpContext.Session.GetString("queryString");

            return View(model);
        }

        [HttpPost]
        public ActionResult SponsorAllocation(string[] CheckedIds, string[] AllIds, string evId)
        {
            var allocatedSponsorContext = new EventSponsorAllocationUpdateContext(new EventSponsorAllocationUpdateGateway());
            var response = allocatedSponsorContext.Execute(new EventsSponsorAllocationRequest(CheckedIds, AllIds, evId));
            if (response.IsValid)
                return RedirectToAction("SponsorAllocationDisplay", new { id = evId, messages = "The event has been successfully updated!", page = 1, searching = "" });
            else
                return RedirectToAction("SponsorAllocationDisplay", new { id = evId, messages = "Update failed!Please try again!", page = 1, searching = "" });
        }

        public ActionResult Details(string id)
        {
            var model = SingleEventReturnerGateway.ReturnEvent(id);
            return View(model);
        }

        public ActionResult Create(string message)
        {
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Create(EventCreateRequest request)
        {
            var eventCreateContext = new EventCreateContext(new EventCreateGateway());
            var eventCreateResponse = eventCreateContext.Execute(request);
            ModelState.Remove("NumberOfVolunteersNeeded");
            ModelState.Remove("DateOfEvent");
            if (!eventCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { message = eventCreateResponse.Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleEventReturnerGateway.ReturnEvent(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EventEditRequest request)
        {
            var eventEditContext = new EventEditContext(new EventEditGateway());
            var eventEditResponse = eventEditContext.Execute(request);
            ModelState.Remove("NumberOfVolunteersNeeded");
            ModelState.Remove("DateOfEvent");
            if (!eventEditResponse.IsValid)
            {
                return RedirectToAction("Edit", new { id = request.Id, message = eventEditResponse.Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var model = SingleEventReturnerGateway.ReturnEvent(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            EventDeleteGateway.DeleteEvent(id);
            return RedirectToAction("Index");
        }
    }
}