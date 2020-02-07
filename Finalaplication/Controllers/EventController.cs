using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public EventController(IHostingEnvironment env)
        {
            try
            {
                dbcontext = new MongoDBContext();
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            }
            catch { }
        }

        public ActionResult FileUpload()
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(IFormFile Files)
        {
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
                List<Event> events = eventcollection.AsQueryable().ToList();
                if (searching != null)
                {
                    events = events.Where(x => x.NameOfEvent.Contains(searching)).ToList();
                }
                if (searchingPlace != null)
                {
                    List<Event> ev = events;
                    foreach (var e in ev)
                    {
                        if (e.PlaceOfEvent == null || e.PlaceOfEvent == "")
                        { e.PlaceOfEvent = "-"; }
                    }
                    events = ev.Where(x => x.PlaceOfEvent.Contains(searchingPlace)).ToList();
                }
                if (searchingActivity!=null)
                {
                    List<Event> ev = events;
                    foreach(var e in ev)
                    {
                        if(e.TypeOfActivities==null || e.TypeOfActivities =="")
                        { e.TypeOfActivities = "-"; }
                    }
                    events = ev.Where(x => x.TypeOfActivities.Contains(searchingActivity)).ToList();
                }
                if (searchingType != null)
                {
                    List<Event> ev = events;
                    foreach (var e in ev)
                    {
                        if (e.TypeOfEvent == null || e.TypeOfEvent == "")
                        { e.TypeOfEvent = "-"; }
                    }
                    events = ev.Where(x => x.TypeOfEvent.Contains(searchingType)).ToList();
                }
                if (searchingVolunteers != null)
                {
                    List<Event> ev = events;
                    foreach (var e in ev)
                    {
                        if (e.AllocatedVolunteers == null || e.AllocatedVolunteers == "")
                        { e.AllocatedVolunteers = "-"; }
                    }
                    events = ev.Where(x => x.AllocatedVolunteers.Contains(searchingVolunteers)).ToList();
                }
                if (searchingSponsor != null)
                {
                    List<Event> ev = events;
                    foreach (var e in ev)
                    {
                        if (e.AllocatedSponsors == null || e.AllocatedSponsors == "")
                        { e.AllocatedSponsors = "-"; }
                    }
                    events = ev.Where(x => x.AllocatedSponsors.Contains(searchingSponsor)).ToList();
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
                return View(events);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult CSVSaver(string ids)
        {
            ViewBag.IDS = ids;
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(string IDS, bool All, bool AllocatedSponsors, bool AllocatedVolunteers, bool Duration, bool TypeOfEvent, bool NameOfEvent, bool PlaceOfEvent, bool DateOfEvent, bool TypeOfActivities)
        {
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

            ids_and_options = "csvexporterapp:" + ids_and_options;

            return Redirect(ids_and_options);
        }

        public ActionResult VolunteerAllocation(string id, string searching)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                List<Event> events = eventcollection.AsQueryable<Event>().ToList();
                var names = events.Find(b => b.EventID.ToString() == id);
                names.AllocatedVolunteers = names.AllocatedVolunteers + " / ";
                ViewBag.strname = names.AllocatedVolunteers.ToString();
                ViewBag.Eventname = names.NameOfEvent.ToString();
                if (searching != null)
                {
                    ViewBag.Evid = id;
                    return View(volunteers.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList());
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
                    return View(sponsors.Where(x => x.NameOfSponsor.Contains(searching)).ToList());
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
