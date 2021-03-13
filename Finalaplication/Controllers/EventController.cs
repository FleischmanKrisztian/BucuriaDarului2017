using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using Finalaplication.ControllerHelpers.EventHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.DatabaseHandler;
using EventManager = Finalaplication.DatabaseHandler.EventManager;
using Finalaplication.ControllerHelpers.VolunteerHelpers;
using Finalaplication.ControllerHelpers.SponsorHelpers;

namespace Finalaplication.Controllers
{
    public class EventController : Controller
    {
        private readonly IStringLocalizer<EventController> _localizer;
        EventManager eventManager = new EventManager();
        VolunteerManager volunteerManager = new VolunteerManager();
        SponsorManager sponsorManager = new SponsorManager();

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
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",Files.FileName);
                    UniversalFunctions.CreateFileStream(Files, path);
                }
                else
                {
                    return View();
                }
                List<string[]> Events = CSVImportParser.GetListFromCSV(path);
                for (int i = 0; i<Events.Count; i++)
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
                page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.page = page;
                List<Event> events = eventManager.GetListOfEvents();
                events = EventFunctions.GetEventsAfterFilters(events, searching, searchingPlace, searchingActivity, searchingType, searchingVolunteers, searchingSponsor, lowerdate, upperdate);
                ViewBag.counter = events.Count();
                int nrofdocs = UniversalFunctions.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = EventFunctions.GetStringOfIds(events);
                ViewBag.stringofids = stringofids;
                events = EventFunctions.GetEventsAfterPaging(events, page, nrofdocs);
                string key = VolMongoConstants.SESSION_KEY;
                HttpContext.Session.SetString(key, stringofids);

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
            string ids = HttpContext.Session.GetString(VolMongoConstants.SESSION_KEY);
            HttpContext.Session.Remove(VolMongoConstants.SESSION_KEY);
            string key = VolMongoConstants.SECONDARY_SESSION_KEY;
            HttpContext.Session.SetString(key, ids);
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(bool All, bool AllocatedSponsors, bool AllocatedVolunteers, bool Duration, bool TypeOfEvent, bool NameOfEvent, bool PlaceOfEvent, bool DateOfEvent, bool TypeOfActivities)
        {
            string IDS = HttpContext.Session.GetString(VolMongoConstants.SECONDARY_SESSION_KEY);
            HttpContext.Session.Remove(VolMongoConstants.SECONDARY_SESSION_KEY);
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
                int nrofdocs = UniversalFunctions.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = VolunteerFunctions.GetStringOfIds(volunteers);
                ViewBag.stringofids = stringofids;
                volunteers = VolunteerFunctions.GetVolunteersAfterPaging(volunteers,page,nrofdocs);
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
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            try
            {
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                volunteers = VolunteerFunctions.GetVolunteersByIds(volunteers, volunteerids);
                Event eventtoallocateto = eventManager.GetOneEvent(Evid);
                string nameofvolunteers = VolunteerFunctions.GetVolunteerNames(volunteers);
                eventtoallocateto.AllocatedVolunteers = nameofvolunteers;
                eventManager.UpdateAnEvent(eventtoallocateto);
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
                int nrofdocs = UniversalFunctions.getNumberOfItemPerPageFromSettings(TempData);
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
                eventManager.UpdateAnEvent(eventtoallocateto);
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

        // POST: Volunteer/Create
        [HttpPost]
        public ActionResult Create(Event eventt)
        {
            try
            {
                string volasstring = JsonConvert.SerializeObject(eventt);
                bool containsspecialchar = false;
                if (volasstring.Contains(";"))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                    containsspecialchar = true;
                }
                try
                {
                    ModelState.Remove("NumberOfVolunteersNeeded");
                    ModelState.Remove("DateOfEvent");
                    ModelState.Remove("Duration");
                    if (ModelState.IsValid)
                    {
                        eventt.DateOfEvent = eventt.DateOfEvent.AddHours(5);
                        eventManager.AddEventToDB(eventt);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.containsspecialchar = containsspecialchar;
                        return View();
                    }
                }
                catch
                {
                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // GET: Volunteer/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                Event eventt = eventManager.GetOneEvent(id);
                Event originalsavedevent = eventt;
                ViewBag.originalsavedevent = JsonConvert.SerializeObject(originalsavedevent);
                ViewBag.id = id;
                return View(eventt);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        //public ActionResult Edit(string id, Event eventt, string Originalsavedeventstring)
        //{
        //    try
        //    {
        //        string volasstring = JsonConvert.SerializeObject(eventt);
        //        bool containsspecialchar = false;
        //        if (volasstring.Contains(";"))
        //        {
        //            ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
        //            containsspecialchar = true;
        //        }
        //        Event Originalsavedvol = JsonConvert.DeserializeObject<Event>(Originalsavedeventstring);
        //        try
        //        {
        //            Event currentsavedevent = eventManager.GetOneEvent(id);
        //            if (JsonConvert.SerializeObject(Originalsavedvol).Equals(JsonConvert.SerializeObject(currentsavedevent)))
        //            {
        //                ModelState.Remove("NumberOfVolunteersNeeded");
        //                ModelState.Remove("DateOfEvent");
        //                ModelState.Remove("Duration");
        //                if (ModelState.IsValid)
        //                {
        //                    var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));
        //                    var update = Builders<Event>.Update
        //                    .Set("NameOfEvent", eventt.NameOfEvent)
        //                    .Set("PlaceOfEvent", eventt.PlaceOfEvent)
        //                    .Set("DateOfEvent", eventt.DateOfEvent.AddHours(5))
        //                    .Set("NumberOfVolunteersNeeded", eventt.NumberOfVolunteersNeeded)
        //                    .Set("TypeOfActivities", eventt.TypeOfActivities)
        //                    .Set("TypeOfEvent", eventt.TypeOfEvent)
        //                    .Set("Duration", eventt.Duration)
        //                    ;

        //                    eventManager.UpdateAnEvent(filter, update);
        //                    return RedirectToAction("Index");
        //                }
        //                else
        //                {
        //                    ViewBag.originalsavedvol = Originalsavedeventstring;
        //                    ViewBag.id = id;
        //                    ViewBag.containsspecialchar = containsspecialchar;
        //                    return View();
        //                }
        //            }
        //            else
        //            {
        //                return View("Volunteerwarning");
        //            }
        //        }
        //        catch
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Localserver", "Home");
        //    }
        //}

        // GET: Volunteer/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                var eventt = eventManager.GetOneEvent(id);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                return View(eventt);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    eventManager.DeleteAnEvent(id);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}