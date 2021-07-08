using BucuriaDarului.Contexts.VolunteerContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.VolunteerGateways;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolunteerHelpers;
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
    public class VolunteerController : Controller
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Finalaplication.Common.Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Finalaplication.Common.Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Finalaplication.Common.Constants.DATABASE_NAME_LOCAL);

        private readonly IStringLocalizer<VolunteerController> _localizer;
        private VolunteerManager volunteerManager = new VolunteerManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private VolContractManager volcontractManager = new VolContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

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
        public ActionResult Import(IFormFile Files)
        {
            var volunteerImportContext = new VolunteerImportContext(new VolunteerImportGateway());
            var response = volunteerImportContext.Execute(Files.OpenReadStream());
            if (response.IsValid)
                return RedirectToAction("Import", new { message = "The Document has successfully been imported" });
            else
                return RedirectToAction("Import", new { message = response.Message[0].Value });
        }

        public ActionResult Index(string searchedFullname, string searchedContact, string sortOrder, bool Active, bool HasCar, bool HasDrivingLicence, DateTime lowerdate, DateTime upperdate, int page, string gender, string searchedAddress, string searchedworkplace, string searchedOccupation, string searchedRemarks, int searchedHourCount)
        {
            var nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
            var volunteerMainDisplayIndexContext = new VolunteerMainDisplayIndexContext(new VolunteerMainDisplayIndexGateway());
            var model = volunteerMainDisplayIndexContext.Execute(new VolunteerMainDisplayIndexRequest(searchedFullname, page, nrOfDocs, sortOrder, searchedContact, Active, HasCar, HasDrivingLicence, lowerdate, upperdate, gender, searchedAddress, searchedworkplace, searchedOccupation, searchedRemarks, searchedHourCount));
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
            if (volunteerExportData.IsValid)
                return Redirect("csvexporterapp:volunteerSession;volunteerHeader");
            return RedirectToAction("CsvExporter", new { message = "Please select at least one Property!" });
        }

        public ActionResult Birthday()
        {
            var volunteerContext = new VolunteerBirthdayDisplayContext(new ListVolunteersGateway());
            var volunteers = volunteerContext.Execute();
            return View(volunteers);
        }

        public ActionResult Contracts(string id)
        {
            return RedirectToAction("Index", "Volcontract", new { idofvol = id });
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
            ModelState.Remove("CIEliberat");

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
            ModelState.Remove("CIEliberat");
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
        public ActionResult Delete(bool Inactive, string id)
        {
            var deleteVolunteerContext = new VolunteerDeleteContext(new VolunteerDeleteGateway());
            deleteVolunteerContext.Execute(Inactive, id);
            return RedirectToAction("Index");
        }
    }
}