using BucuriaDarului.Contexts.VolunteerContexts;
using BucuriaDarului.Gateway.VolunteerGateways;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;

namespace BucuriaDarului.Web.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IStringLocalizer<VolunteerController> _localizer;

        public VolunteerController(IStringLocalizer<VolunteerController> localizer)
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
            var volunteerImportContext = new VolunteerImportContext(new VolunteerImportGateway());
            var response = volunteerImportContext.Execute(files.OpenReadStream());
            if (response.IsValid)
                return RedirectToAction("Import", new { message = "The Document has successfully been imported" });
            else
                return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        public ActionResult Index(string searchedFullname, string searchedContact, string sortOrder, bool active, bool hasCar, bool hasDrivingLicense, DateTime lowerDate, DateTime upperDate, int page, string gender, string searchedAddress, string searchedWorkplace, string searchedOccupation, string searchedRemarks, int searchedHourCount)
        {
            HttpContext.Session.SetString("queryString", Request.QueryString.ToString());
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var volunteerMainDisplayIndexContext = new VolunteerMainDisplayIndexContext(new VolunteerMainDisplayIndexGateway());
            var model = volunteerMainDisplayIndexContext.Execute(new VolunteerMainDisplayIndexRequest(nrOfDocs, page, searchedFullname, searchedContact, sortOrder, active, hasCar, hasDrivingLicense, lowerDate, upperDate, gender, searchedAddress, searchedWorkplace, searchedOccupation, searchedRemarks, searchedHourCount));
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
            var volunteerExporterContext = new VolunteerExporterContext(_localizer);
            var volunteerExportData = volunteerExporterContext.Execute(new VolunteerExporterRequest(csvExportProperties));
            DictionaryHelper.d = volunteerExportData.Dictionary;
            if (volunteerExportData.IsValid && volunteerExportData.FileName != "")
                return DownloadCSV(volunteerExportData.FileName, "volunteerSession", "volunteerHeader");
            return RedirectToAction("CsvExporter", new { message = "Please select at least one Property!" });
        }

        public FileContentResult DownloadCSV(string fileName, string idsKey, string headerKey)
        {
            DictionaryHelper.d.TryGetValue(idsKey, out var ids);
            DictionaryHelper.d.TryGetValue(headerKey, out var header);
            var context = new VolunteerDownloadContext(new VolunteerDownloadGateway());
            var response = context.Execute(ids, header);

            return File(new System.Text.UTF8Encoding().GetBytes(response.ToString()), "text/csv", fileName);
        }

        public ActionResult Birthday()
        {
            var volunteerContext = new VolunteerBirthdayDisplayContext(new BithdayListVolunteersGateway());
            var volunteers = volunteerContext.Execute();
            return View(volunteers);
        }

        public ActionResult Contracts(string id)
        {
            return RedirectToAction("Index", "VolunteerContract", new { idOfVolunteer = id });
        }

        public ActionResult Details(string id)
        {
            var model = SingleVolunteerReturnerGateway.ReturnVolunteer(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string message)
        {
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Create(VolunteerCreateRequest request, IFormFile image)
        {
            var volunteerCreateContext = new VolunteerCreateContext(new VolunteerCreateGateway());
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

            var volunteerCreateResponse = volunteerCreateContext.Execute(request, fileBytes);

            ModelState.Remove("Birthdate");
            ModelState.Remove("HourCount");
            ModelState.Remove("ExpirationDate");

            if (!volunteerCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { message = volunteerCreateResponse.Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id, string message)
        {
            ViewBag.message = message;
            var model = SingleVolunteerReturnerGateway.ReturnVolunteer(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(VolunteerEditRequest request, IFormFile image)
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
            var volunteerEditContext = new VolunteerEditContext(new VolunteerEditGateway());
            var volunteerEditResponse = volunteerEditContext.Execute(request, fileBytes);
            ModelState.Remove("Birthdate");
            ModelState.Remove("HourCount");
            ModelState.Remove("ExpirationDate");
            if (!volunteerEditResponse.IsValid)
            {
                return RedirectToAction("Edit", new { id = request.Id, message = volunteerEditResponse.Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var model = SingleVolunteerReturnerGateway.ReturnVolunteer(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(bool inactive, string id)
        {
            var deleteVolunteerContext = new VolunteerDeleteContext(new VolunteerDeleteGateway());
            deleteVolunteerContext.Execute(inactive, id);
            return RedirectToAction("Index");
        }
    }
}