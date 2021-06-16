using Elm.Core.Parsers;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.EventHelpers;
using Finalaplication.ControllerHelpers.SponsorHelpers;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolunteerHelpers;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BucuriaDarului.Contexts;
using BucuriaDarului.Gateway;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Finalaplication.Controllers
{
    public class EventController : Controller
    {
        private readonly IStringLocalizer<EventController> _localizer;
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL);

        private EventManager eventManager = new EventManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();
        private AuxiliaryDBManager auxiliaryDBManager = new AuxiliaryDBManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private VolunteerManager volunteerManager = new VolunteerManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private SponsorManager sponsorManager = new SponsorManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        public EventController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, IStringLocalizer<EventController> localizer)
        {
            _localizer = localizer;
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile Files)
        {
            try
            {
                var _eventsImportContext = new EventsImportContext(new EventsImportDataGateway());
                Stream csvFile = Files.OpenReadStream();
                _eventsImportContext.Execute(csvFile);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        public ActionResult Index(string searching, int page, string searchingPlace, string searchingActivity, string searchingType, string searchingVolunteers, string searchingSponsor, DateTime lowerdate, DateTime upperdate)
        {
            try
            {
                //TODO: FIX THIS:
                int nrOfDocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                var eventsMainDisplayIndexContext = new EventsMainDisplayIndexContext(new EventsMainDisplayIndexGateway());
                var QueriedData = eventsMainDisplayIndexContext.Execute(new EventsMainDisplayIndexRequest(searching, page, nrOfDocs, searchingPlace, searchingActivity, searchingType, searchingVolunteers, searchingSponsor, lowerdate, upperdate));
                return View(QueriedData);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult CSVSaver(string stringOfIDs)
        {
            string ids = stringOfIDs;
            string key = Constants.SECONDARY_SESSION_KEY_EVENT;
            HttpContext.Session.SetString(key, ids);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(bool All, bool AllocatedSponsors, bool AllocatedVolunteers, bool Duration, bool TypeOfEvent, bool NameOfEvent, bool PlaceOfEvent, bool DateOfEvent, bool TypeOfActivities)
        {
            string IDS = HttpContext.Session.GetString(Constants.SECONDARY_SESSION_KEY_EVENT);
            HttpContext.Session.Remove(Constants.SECONDARY_SESSION_KEY_EVENT);
            string ids_and_fields = EventFunctions.GetIdAndFieldString(IDS, All, AllocatedSponsors, AllocatedVolunteers, Duration, TypeOfEvent, NameOfEvent, PlaceOfEvent, DateOfEvent, TypeOfActivities);
            string key1 = Constants.EVENTSESSION;
            string header = ControllerHelper.GetHeaderForExcelPrinterEvent(_localizer);
            string key2 = Constants.EVENTHEADER;
            ControllerHelper.CreateDictionaries(key1, key2, ids_and_fields, header);
            string csvexporterlink = "csvexporterapp:" + key1 + ";" + key2;
            return Redirect(csvexporterlink);
        }

        public ActionResult VolunteerAllocation(string id, int page, string searching)
        {
            try
            {
                page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.Page = page;
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                volunteers = VolunteerFunctions.GetVolunteerAfterSorting(volunteers, "Active_desc");
                ViewBag.counter = volunteers.Count();
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = VolunteerFunctions.GetStringOfIds(volunteers);
                ViewBag.stringofids = stringofids;
                volunteers = VolunteerFunctions.GetVolunteersAfterPaging(volunteers, page, nrofdocs);
                List<Event> events = eventManager.GetListOfEvents();
                ViewBag.strname = EventFunctions.GetAllocatedVolunteersString(events, id);
                ViewBag.Eventname = EventFunctions.GetNameOfEvent(events, id);
                ViewBag.Evid = id;
                volunteers = VolunteerFunctions.GetVolunteersAfterSearching(volunteers, searching);
                return View(volunteers);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult VolunteerAllocation(string[] volunteerids, string Evid)
        {
            try
            {
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                volunteers = VolunteerFunctions.GetVolunteersByIds(volunteers, volunteerids);
                Event eventtoallocateto = eventManager.GetOneEvent(Evid);
                string nameofvolunteers = VolunteerFunctions.GetVolunteerNames(volunteers);
                eventtoallocateto.AllocatedVolunteers = nameofvolunteers;
                eventtoallocateto.NumberAllocatedVolunteers = EventFunctions.VolunteersAllocatedCounter(nameofvolunteers);
                eventManager.UpdateAnEvent(eventtoallocateto, Evid);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult SponsorAllocation(string id, int page, string searching)
        {
            try
            {
                page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.Page = page;
                List<Sponsor> sponsors = sponsorManager.GetListOfSponsors();
                ViewBag.counter = sponsors.Count();
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = SponsorFunctions.GetStringOfIds(sponsors);
                ViewBag.stringofids = stringofids;
                sponsors = SponsorFunctions.GetSponsorsAfterPaging(sponsors, page, nrofdocs);
                List<Event> events = eventManager.GetListOfEvents();
                ViewBag.strname = EventFunctions.GetAllocatedSponsorsString(events, id);
                ViewBag.Eventname = EventFunctions.GetNameOfEvent(events, id);
                ViewBag.Evid = id;
                sponsors = SponsorFunctions.GetSponsorsAfterSearching(sponsors, searching);
                return View(sponsors);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult SponsorAllocation(string[] sponsorids, string Evid)
        {
            try
            {
                List<Sponsor> sponsors = sponsorManager.GetListOfSponsors();
                sponsors = SponsorFunctions.GetSponsorsByIds(sponsors, sponsorids);
                Event eventtoallocateto = eventManager.GetOneEvent(Evid);
                string nameofsponsors = SponsorFunctions.GetSponsorNames(sponsors);
                eventtoallocateto.AllocatedSponsors = nameofsponsors;
                eventManager.UpdateAnEvent(eventtoallocateto, Evid);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(string id)
        {
            try
            {
                Event detailedevent = eventManager.GetOneEvent(id);
                return View(detailedevent);
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
        public ActionResult Create(Event incomingevent)
        {
            try
            {
                string eventasstring = JsonConvert.SerializeObject(incomingevent);
                if (UniversalFunctions.ContainsSpecialChar(eventasstring))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                }
                ModelState.Remove("NumberOfVolunteersNeeded");
                ModelState.Remove("DateOfEvent");
                ModelState.Remove("Duration");
                if (ModelState.IsValid)
                {
                    incomingevent._id = Guid.NewGuid().ToString();
                    incomingevent.DateOfEvent = incomingevent.DateOfEvent.AddHours(5);
                    eventManager.AddEventToDB(incomingevent);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.containsspecialchar = UniversalFunctions.ContainsSpecialChar(eventasstring);
                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Edit(string id)
        {
            try
            {
                Event ourevent = eventManager.GetOneEvent(id);
                ViewBag.id = id;
                return View(ourevent);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(string id, Event incomingevent)
        {
            try
            {
                string eventasstring = JsonConvert.SerializeObject(incomingevent);
                if (UniversalFunctions.ContainsSpecialChar(eventasstring))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                }
                ModelState.Remove("NumberOfVolunteersNeeded");
                ModelState.Remove("DateOfEvent");
                ModelState.Remove("Duration");
                if (ModelState.IsValid)
                {
                    incomingevent.DateOfEvent = incomingevent.DateOfEvent.AddHours(5);
                    List<ModifiedIDs> modifiedidlist = modifiedDocumentManager.GetListOfModifications();
                    string modifiedids = JsonConvert.SerializeObject(modifiedidlist);
                    if (!modifiedids.Contains(id))
                    {
                        Event currentevent = eventManager.GetOneEvent(id);
                        string currenteventasstring = JsonConvert.SerializeObject(currentevent);
                        auxiliaryDBManager.AddDocumenttoDB(currenteventasstring);
                    }
                    eventManager.UpdateAnEvent(incomingevent, id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.id = id;
                    ViewBag.containsspecialchar = UniversalFunctions.ContainsSpecialChar(eventasstring);
                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Delete(string id)
        {
            try
            {
                Event eventtoshow = eventManager.GetOneEvent(id);
                return View(eventtoshow);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                eventManager.DeleteAnEvent(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}