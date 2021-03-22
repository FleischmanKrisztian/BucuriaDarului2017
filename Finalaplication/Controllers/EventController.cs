using Elm.Core.Parsers;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.EventHelpers;
using Finalaplication.ControllerHelpers.SponsorHelpers;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolunteerHelpers;
using Finalaplication.DatabaseHandler;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventManager = Finalaplication.DatabaseHandler.EventManager;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Finalaplication.Controllers
{
    public class EventController : Controller
    {
        private readonly IStringLocalizer<EventController> _localizer;
        private EventManager eventManager = new EventManager();
        private VolunteerManager volunteerManager = new VolunteerManager();
        private SponsorManager sponsorManager = new SponsorManager();

        public EventController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, IStringLocalizer<EventController> localizer)
        {
            _localizer = localizer;
        }

        public ActionResult FileUpload()
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(IFormFile Files)
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            try
            {
                string path = " ";
                if (UniversalFunctions.File_is_not_empty(Files))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Files.FileName);
                    UniversalFunctions.CreateFileStream(Files, path);
                }
                else
                {
                    return View();
                }
                List<string[]> Events = CSVImportParser.GetListFromCSV(path);
                for (int i = 0; i < Events.Count; i++)
                {
                    Event ev = EventFunctions.GetEventFromString(Events[i]);
                    eventManager.AddEventToDB(ev);
                }
                UniversalFunctions.RemoveTempFile(path);
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
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                List<Event> events = eventManager.GetListOfEvents();
                events = EventFunctions.GetEventsAfterFilters(events, searching, searchingPlace, searchingActivity, searchingType, searchingVolunteers, searchingSponsor, lowerdate, upperdate);
                ViewBag.counter = events.Count();
                string stringofids = EventFunctions.GetStringOfIds(events);
                events = EventFunctions.GetEventsAfterPaging(events, page, nrofdocs);
                string key = VolMongoConstants.SESSION_KEY_EVENT;
                HttpContext.Session.SetString(key, stringofids);

                if (searching != null)
                { ViewBag.Filter1 = searching; }
                if (searchingPlace != null)
                { ViewBag.Filter2 = searchingPlace; }
                if (searchingActivity != null)
                { ViewBag.Filter3 = searchingActivity; }
                if (searchingType != null)
                { ViewBag.Filter4 = searchingType; }
                if (searchingVolunteers != null)
                { ViewBag.Filter5 = searchingVolunteers; }
                if (searchingSponsor != null)
                { ViewBag.Filter6 = searchingSponsor; }
                DateTime date = Convert.ToDateTime("01.01.0001 00:00:00");
                if (lowerdate != date)
                { ViewBag.Filter7 = lowerdate.ToString(); }
                if (upperdate != date)
                { ViewBag.Filter8 = upperdate.ToString(); }
                ViewBag.searching = searching;
                ViewBag.Activity = searchingActivity;
                ViewBag.Place = searchingPlace;
                ViewBag.Type = searchingType;
                ViewBag.Volunteer = searchingVolunteers;
                ViewBag.Sponsor = searchingSponsor;
                ViewBag.Upperdate = upperdate;
                ViewBag.Lowerdate = lowerdate;
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ViewBag.page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.nrofdocs = nrofdocs;
                ViewBag.stringofids = stringofids;

                return View(events);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult CSVSaver()
        {
            string ids = HttpContext.Session.GetString(VolMongoConstants.SESSION_KEY_EVENT);
            HttpContext.Session.Remove(VolMongoConstants.SESSION_KEY_EVENT);
            string key = VolMongoConstants.SECONDARY_SESSION_KEY_EVENT;
            HttpContext.Session.SetString(key, ids);
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(bool All, bool AllocatedSponsors, bool AllocatedVolunteers, bool Duration, bool TypeOfEvent, bool NameOfEvent, bool PlaceOfEvent, bool DateOfEvent, bool TypeOfActivities)
        {
            string IDS = HttpContext.Session.GetString(VolMongoConstants.SECONDARY_SESSION_KEY_EVENT);
            HttpContext.Session.Remove(VolMongoConstants.SECONDARY_SESSION_KEY_EVENT);
            string ids_and_fields = EventFunctions.GetIdAndFieldString(IDS, All, AllocatedSponsors, AllocatedVolunteers, Duration, TypeOfEvent, NameOfEvent, PlaceOfEvent, DateOfEvent, TypeOfActivities);
            string key1 = VolMongoConstants.EVENTSESSION;
            string header = ControllerHelper.GetHeaderForExcelPrinterEvent(_localizer);
            string key2 = VolMongoConstants.EVENTHEADER;
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                volunteers = VolunteerFunctions.GetVolunteersByIds(volunteers, volunteerids);
                Event eventtoallocateto = eventManager.GetOneEvent(Evid);
                string nameofvolunteers = VolunteerFunctions.GetVolunteerNames(volunteers);
                eventtoallocateto.AllocatedVolunteers = nameofvolunteers;
                eventManager.UpdateAnEvent(eventtoallocateto, Evid);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SponsorAllocation(string id, int page, string searching)
        {
            try
            {
                page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.Page = page;
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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