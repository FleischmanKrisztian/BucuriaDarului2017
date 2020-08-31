using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Finalaplication.Controllers
{
    public class EventController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Event> eventcollection;
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private readonly IStringLocalizer<EventController> _localizer;
       


        public EventController(IHostingEnvironment env, IStringLocalizer<EventController> localizer)
        {
            try
            {
                
                dbcontext = new MongoDBContext();
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            }
            catch { }
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

                if (Files.Length > 0)
                {
                    path = Path.Combine(
                               Directory.GetCurrentDirectory(), "wwwroot",
                               Files.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Files.CopyTo(stream);
                    }
                }
                else
                {
                    return View();
                }
                CSVImportParser cSV = new CSVImportParser(path);
                List<string[]> result = cSV.ExtractDataFromFile(path);

                Thread myNewThread = new Thread(() => ControllerHelper.GetEventsFromCsv(eventcollection, result));
                myNewThread.Start();

                myNewThread.Join();

                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        public ActionResult Index(string searching, int page,string searchingPlace,string searchingActivity,string searchingType,string searchingVolunteers,string searchingSponsor, DateTime lowerdate, DateTime upperdate)
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
                

                List<Event> events = eventcollection.AsQueryable().ToList();
                

                    ViewBag.searching = searching;
                    ViewBag.Activity = searchingActivity;
                    ViewBag.Place = searchingPlace;
                    ViewBag.Type = searchingType;
                    ViewBag.Volunteer = searchingVolunteers;
                    ViewBag.Sponsor = searchingSponsor;
                    ViewBag.Upperdate = upperdate;
                    ViewBag.Lowerdate = lowerdate;
                    ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                    int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                    if (page > 0)
                        ViewBag.Page = page;
                    else
                        ViewBag.Page = 1;
                   
                    if (searching != null)
                    {
                        events = events.Where(x => x.NameOfEvent.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    if (searchingPlace != null)
                    {
                        List<Event> ev = events;
                        foreach (var e in ev)
                        {
                            if (e.PlaceOfEvent == null || e.PlaceOfEvent == "")
                            { e.PlaceOfEvent = "-"; }
                        }
                        events = ev.Where(x => x.PlaceOfEvent.Contains(searchingPlace, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    if (searchingActivity != null)
                    {
                        List<Event> ev = events;
                        foreach (var e in ev)
                        {
                            if (e.TypeOfActivities == null || e.TypeOfActivities == "")
                            { e.TypeOfActivities = "-"; }
                        }
                        events = ev.Where(x => x.TypeOfActivities.Contains(searchingActivity, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    if (searchingType != null)
                    {
                        List<Event> ev = events;
                        foreach (var e in ev)
                        {
                            if (e.TypeOfEvent == null || e.TypeOfEvent == "")
                            { e.TypeOfEvent = "-"; }
                        }
                        events = ev.Where(x => x.TypeOfEvent.Contains(searchingType, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    if (searchingVolunteers != null)
                    {
                        List<Event> ev = events;
                        foreach (var e in ev)
                        {
                            if (e.AllocatedVolunteers == null || e.AllocatedVolunteers == "")
                            { e.AllocatedVolunteers = "-"; }
                        }
                        events = ev.Where(x => x.AllocatedVolunteers.Contains(searchingVolunteers, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    if (searchingSponsor != null)
                    {
                        List<Event> ev = events;
                        foreach (var e in ev)
                        {
                            if (e.AllocatedSponsors == null || e.AllocatedSponsors == "")
                            { e.AllocatedSponsors = "-"; }
                        }
                        events = ev.Where(x => x.AllocatedSponsors.Contains(searchingSponsor, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    DateTime d1 = new DateTime(0003, 1, 1);
                    if (lowerdate > d1)
                    {
                        events = events.Where(x => x.DateOfEvent > lowerdate).ToList();
                    }
                    if (upperdate > d1)
                    {
                        events = events.Where(x => x.DateOfEvent <= upperdate).ToList();
                    }


                    ViewBag.counter = events.Count();

                    ViewBag.nrofdocs = nrofdocs;
                    string stringofids = "events";
                    foreach (Event eve in events)
                    {
                        stringofids = stringofids + "," + eve.EventID;
                    }
                    ViewBag.stringofids = stringofids;
                    events = events.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                    events = events.AsQueryable().Take(nrofdocs).ToList();

                string key = "FirstSessionEvent";
                  HttpContext.Session.SetString(key, stringofids);
                //DictionaryHelper.d.Add(key, new DictionaryHelper(stringofids));
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
            string ids = HttpContext.Session.GetString("FirstSessionEvent");
            HttpContext.Session.Remove("FirstSessionEvent");
           string key = "SecondSessionEvent";
            HttpContext.Session.SetString(key, ids);

            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver( bool All, bool AllocatedSponsors, bool AllocatedVolunteers, bool Duration, bool TypeOfEvent, bool NameOfEvent, bool PlaceOfEvent, bool DateOfEvent, bool TypeOfActivities)
        {
             var IDS = HttpContext.Session.GetString("SecondSessionEvent");
            HttpContext.Session.Remove("SecondSessionEvent");
            string ids_and_options = IDS + "(((";
            if (All == true)
                ids_and_options = ids_and_options + "0";
            if (NameOfEvent == true)
                ids_and_options = ids_and_options + "1";
            if (PlaceOfEvent == true)
                ids_and_options = ids_and_options + "2";
            if (DateOfEvent == true)
                ids_and_options = ids_and_options + "3";
            if (TypeOfActivities == true)
                ids_and_options = ids_and_options + "4";
            if (TypeOfEvent == true)
                ids_and_options = ids_and_options + "5";
            if (Duration == true)
                ids_and_options = ids_and_options + "6";
            if (AllocatedVolunteers == true)
                ids_and_options = ids_and_options + "7";
            if (AllocatedSponsors == true)
                ids_and_options = ids_and_options + "8";

            string key1 = "eventSession";
            ControllerHelper helper = new ControllerHelper();
            string header = helper.GetHeaderForExcelPrinterEvent(_localizer);
            string key2 = "eventHeader";

            if (DictionaryHelper.d.ContainsKey(key1) == true)
            {
                DictionaryHelper.d[key1] = ids_and_options;
            }
            else
            {
                DictionaryHelper.d.Add(key1, ids_and_options);
            }
            if (DictionaryHelper.d.ContainsKey(key2) == true)
            {
                DictionaryHelper.d[key2] = header;
            }
            else
            {
                DictionaryHelper.d.Add(key2, header);
            }
            string ids_and_optionssecond = "csvexporterapp:" + key1 + ";" + key2;

            return Redirect(ids_and_optionssecond);
        }

        public ActionResult VolunteerAllocation(string id, int page,string searching)
        {
            try
            {
                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;

                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();

                ViewBag.counter = volunteers.Count();

                ViewBag.nrofdocs = nrofdocs;
                string stringofids = "vol";
                foreach (Volunteer vol in volunteers)
                {
                    stringofids = stringofids + "," + vol.VolunteerID;
                }
                ViewBag.stringofids = stringofids;
                volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                volunteers= volunteers.AsQueryable().Take(nrofdocs).ToList();

                List<Event> events = eventcollection.AsQueryable<Event>().ToList();
                var names = events.Find(b => b.EventID.ToString() == id);
                names.AllocatedVolunteers = names.AllocatedVolunteers + " / ";
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
                        var volunteerId = new ObjectId(vols[i]);
                        var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == volunteerId.ToString());

                        volname = volname + volunteer.Firstname + " " + volunteer.Lastname + " / ";
                        var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(Evid));
                        var update = Builders<Event>.Update
                            .Set("AllocatedVolunteers", volname);

                        var result = eventcollection.UpdateOne(filter, update);
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

        public ActionResult SponsorAllocation(string id, string searching)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
                List<Event> events = eventcollection.AsQueryable<Event>().ToList();
                var names = events.Find(b => b.EventID.ToString() == id);
                names.AllocatedSponsors = names.AllocatedSponsors + " ";
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
                        var sponsorId = new ObjectId(spons[i]);
                        var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == sponsorId.ToString());

                        sponsname = sponsname + " " + sponsor.NameOfSponsor;
                        var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(Evid));
                        var update = Builders<Event>.Update
                            .Set("AllocatedSponsors", sponsname);

                        var result = eventcollection.UpdateOne(filter, update);
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
                var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
                return View(eventt);
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
                        eventcollection.InsertOne(eventt);
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
                var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
                Event originalsavedevent = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
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
                    Event currentsavedevent = eventcollection.Find(x => x.EventID == id).Single();
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

                            var result = eventcollection.UpdateOne(filter, update);
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
                var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
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
                    eventcollection.DeleteOne(Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id)));
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
