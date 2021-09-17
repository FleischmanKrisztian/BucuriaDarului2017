using BucuriaDarului.Contexts.VolunteerContexts;
using BucuriaDarului.Contexts.VolunteerContractContexts;
using BucuriaDarului.Contexts.VolunteerAdditionalContractContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
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
            ViewBag.query = HttpContext.Session.GetString("queryString");
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile files, string overwrite)
        {
            var volunteerImportContext = new VolunteerImportContext(new VolunteerImportGateway(), _localizer);
            var response = new VolunteerImportResponse();
            if (files != null)
                response = volunteerImportContext.Execute(files.OpenReadStream(), overwrite);
            else
            {
                response.Message.Add(new KeyValuePair<string, string>("NoFile", @_localizer["Please choose a file!"]));
                response.IsValid = false;
            }
            if (response.IsValid)
                return RedirectToAction("Import", new { message = @_localizer["The Document has been successfully imported"] });
            return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        public ActionResult Index(string searchedFullname, string searchedContact, string sortOrder, bool active, bool hasCar, bool hasDrivingLicense, DateTime lowerDate, DateTime upperDate, int page, string gender, string searchedAddress, string searchedWorkplace, string searchedOccupation, string searchedRemarks, string searchedHourCount)
        {
            HttpContext.Session.SetString("queryString", Request.QueryString.ToString());
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var volunteerMainDisplayIndexContext = new VolunteerMainDisplayIndexContext(new VolunteerMainDisplayIndexGateway());
            var model = volunteerMainDisplayIndexContext.Execute(new VolunteerMainDisplayIndexRequest(nrOfDocs, page, searchedFullname, searchedContact, sortOrder, active, hasCar, hasDrivingLicense, lowerDate, upperDate, gender, searchedAddress, searchedWorkplace, searchedOccupation, searchedRemarks, searchedHourCount));
            HttpContext.Session.SetString(model.DictionaryKey, model.StringOfIDs);
            return View(model);
        }

        [HttpGet]
        public ActionResult ExcelExporter(string dictionaryKey, string message)
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
            var stringOfIds = HttpContext.Session.GetString(dictionaryKey);
            ViewBag.Ids = stringOfIds;
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult ExcelExporter(ExportParameters excelExportProperties)
        {
            var volunteerExporterContext = new VolunteerExporterContext(_localizer);
            var volunteerExportData = volunteerExporterContext.Execute(new VolunteerExporterRequest(excelExportProperties));
            if (volunteerExportData.IsValid)
                return DownloadFile(volunteerExportData.Stream, volunteerExportData.FileName+".xls");
                //TODO: Return Success Message to Screen
            //TODO: Verification if there are any selected checkboxes
            return RedirectToAction("ExcelExporter", new { dictionaryKey = Constants.VOLUNTEER_SESSION, message = @_localizer["Please select at least one Property!"] });
        }

        public FileContentResult DownloadFile(MemoryStream data, string fileName)
        {
            return File(data.ToArray(), "application/vnd.ms-excel", fileName);
        }

        public ActionResult Birthday()
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
            var nrOfDays = UniversalFunctions.GetNumberOfDaysBeforeBirthday(TempData);
            var volunteerContext = new VolunteerBirthdayDisplayContext(new BithdayListVolunteersGateway());
            var volunteers = volunteerContext.Execute(nrOfDays);
            return View(volunteers);
        }

        public ActionResult Contracts(string id)
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
            return RedirectToAction("Index", "VolunteerContract", new { idOfVolunteer = id });
        }

        public ActionResult Details(string id)
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
            var model = SingleVolunteerReturnerGateway.ReturnVolunteer(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string message)
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Create(VolunteerCreateRequest request, IFormFile image)
        {
            var volunteerCreateContext = new VolunteerCreateContext(new VolunteerCreateGateway());
            var fileBytes = Array.Empty<byte>();

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
            if (!volunteerCreateResponse.IsValid)
            {
                return RedirectToAction("Create", new { message = @_localizer[volunteerCreateResponse.Message] });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id, string message)
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
            ViewBag.message = message;
            var model = SingleVolunteerReturnerGateway.ReturnVolunteer(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(VolunteerEditRequest request, IFormFile image)
        {
            var fileBytes = Array.Empty<byte>();
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

            var volunteerContractEditContext = new VolunteerContractEditContext(new VolunteerContractEditGateway());
            var volunteerContractEditResponse = volunteerContractEditContext.Execute(request);

            var volunteerAdditionalEditContext = new VolunteerAdditionalContractEditContext(new VolunteerAdditionalContractEditGateway());
            var volunteerAdditionalEditResponse = volunteerAdditionalEditContext.Execute(request);

            if (!volunteerContractEditResponse.IsValid)
            {
                volunteerEditResponse.Message = volunteerContractEditResponse.Message;
                volunteerEditResponse.IsValid = false;
            }

            if (!volunteerAdditionalEditResponse.IsValid)
            {
                volunteerEditResponse.Message = volunteerAdditionalEditResponse.Message;
                volunteerEditResponse.IsValid = false;
            }

            if (!volunteerEditResponse.IsValid)
            {
                return RedirectToAction("Edit", new { id = request.Id, message = @_localizer[volunteerEditResponse.Message] });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            ViewBag.query = HttpContext.Session.GetString("queryString");
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