using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Finalaplication.ControllerHelpers.EventHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.DatabaseHandler;
using EventManager = Finalaplication.DatabaseHandler.EventManager;

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
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;
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
            string ids_and_optionssecond = "csvexporterapp:" + key1 + ";" + key2;
            return Redirect(ids_and_optionssecond);
        }

        public ActionResult VolunteerAllocation(string id, int page, string searching)
        {
            try
            {
                int nrofdocs = UniversalFunctions.getNumberOfItemPerPageFromSettings(TempData);
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;

                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();

                ViewBag.counter = volunteers.Count();

                ViewBag.nrofdocs = nrofdocs;
                string stringofids = "vol";
                foreach (Volunteer vol in volunteers)
                {
                    stringofids = stringofids + "," + vol.VolunteerID;
                }
                ViewBag.stringofids = stringofids;
                volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                volunteers = volunteers.AsQueryable().Take(nrofdocs).ToList();

                List<Event> events = eventManager.GetListOfEvents();
                var names = events.Find(b => b.EventID.ToString() == id);
                names.AllocatedVolunteers += " / ";
                ViewBag.strname = names.AllocatedVolunteers.ToString();
                ViewBag.Eventname = names.NameOfEvent.ToString();
                if (searching != null)
                {
                    ViewBag.Evid = id;
                    return View(volunteers.Where(x => x.Firstname.Contains(searching, StringComparison.InvariantCultureIgnoreCase) || x.Lastname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList());
                }
                else
                {
                    ViewBag.Evid = id;
                    return View(volunteers);
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult VolunteerAllocation(string[] vols, string Evid)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    string volname = "";
                    for (int i = 0; i < vols.Length; i++)
                    {
                        var id = vols[i];
                        var volunteer = volunteerManager.GetOneVolunteer(id);

                        volname = volname + volunteer.Firstname + " " + volunteer.Lastname + " / ";
                        var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(Evid));
                        var eventtoupdate = Builders<Event>.Update
                            .Set("AllocatedVolunteers", volname);

                        eventManager.UpdateAnEvent(filter, eventtoupdate);
                    }
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

        public ActionResult SponsorAllocation(string id, int page, string searching)
        {
            try
            {
                int nrofdocs = UniversalFunctions.getNumberOfItemPerPageFromSettings(TempData);
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Sponsor> sponsors = sponsorManager.GetListOfSponsors();

                ViewBag.counter = sponsors.Count();

                ViewBag.nrofdocs = nrofdocs;
                string stringofids = "sponsor";
                foreach (Sponsor sponsor in sponsors)
                {
                    stringofids = stringofids + "," + sponsor.SponsorID;
                }
                ViewBag.stringofids = stringofids;
                sponsors = sponsors.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                sponsors = sponsors.AsQueryable().Take(nrofdocs).ToList();

                List<Event> events = eventManager.GetListOfEvents();
                var names = events.Find(b => b.EventID.ToString() == id);
                names.AllocatedSponsors += " ";
                ViewBag.strname = names.AllocatedSponsors.ToString();
                ViewBag.Eventname = names.NameOfEvent.ToString();
                if (searching != null)
                {
                    ViewBag.Evid = id;
                    return View(sponsors.Where(x => x.NameOfSponsor.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList());
                }
                else
                {
                    ViewBag.Evid = id;
                    return View(sponsors);
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult SponsorAllocation(string[] spons, string Evid)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    string sponsname = "";
                    for (int i = 0; i < spons.Length; i++)
                    {
                        var id = spons[i];
                        var sponsor = sponsorManager.GetOneSponsor(id);

                        sponsname = sponsname + " " + sponsor.NameOfSponsor;
                        var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(Evid));
                        var eventtoupdate = Builders<Event>.Update
                            .Set("AllocatedSponsors", sponsname);

                        eventManager.UpdateAnEvent(filter, eventtoupdate);
                    }
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

        // GET: Volunteer/Details/5
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

        // GET: Volunteer/Create
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
        public ActionResult Edit(string id, Event eventt, string Originalsavedeventstring)
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
                Event Originalsavedvol = JsonConvert.DeserializeObject<Event>(Originalsavedeventstring);
                try
                {
                    Event currentsavedevent = eventManager.GetOneEvent(id);
                    if (JsonConvert.SerializeObject(Originalsavedvol).Equals(JsonConvert.SerializeObject(currentsavedevent)))
                    {
                        ModelState.Remove("NumberOfVolunteersNeeded");
                        ModelState.Remove("DateOfEvent");
                        ModelState.Remove("Duration");
                        if (ModelState.IsValid)
                        {
                            var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));
                            var update = Builders<Event>.Update
                            .Set("NameOfEvent", eventt.NameOfEvent)
                            .Set("PlaceOfEvent", eventt.PlaceOfEvent)
                            .Set("DateOfEvent", eventt.DateOfEvent.AddHours(5))
                            .Set("NumberOfVolunteersNeeded", eventt.NumberOfVolunteersNeeded)
                            .Set("TypeOfActivities", eventt.TypeOfActivities)
                            .Set("TypeOfEvent", eventt.TypeOfEvent)
                            .Set("Duration", eventt.Duration)
                            ;

                            eventManager.UpdateAnEvent(filter, update);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.originalsavedvol = Originalsavedeventstring;
                            ViewBag.id = id;
                            ViewBag.containsspecialchar = containsspecialchar;
                            return View();
                        }
                    }
                    else
                    {
                        return View("Volunteerwarning");
                    }
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

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