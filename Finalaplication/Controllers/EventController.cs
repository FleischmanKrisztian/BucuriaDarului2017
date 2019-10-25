using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Finalaplication.Controllers
{
    public class EventController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Event> eventcollection;
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Sponsor> sponsorcollection;

        public EventController()
        {
            dbcontext = new MongoDBContext();
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
        }

        public ActionResult Export()
        {
            List<Event> events = eventcollection.AsQueryable().ToList();
            string path = "./jsondata/Events.csv";
            var allLines = (
                            from Event in events
                            select new object[]
                            {
            string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                            Event.NameOfEvent,
                              Event.DateOfEvent.ToString(),
                              Event.Duration.ToString(),
                              Event.NumberOfVolunteersNeeded.ToString(),
                              Event.PlaceOfEvent,
                              Event.TypeOfActivities,
                              Event.TypeOfEvent,
                              Event.AllocatedVolunteers,
                              Event.AllocatedSponsors
                              )
                            }
                             ).ToList();
            var csv1 = new StringBuilder();

            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));
            }
           );
            System.IO.File.WriteAllText(path, "NameOfEvent,DateOfEvent,Duration,NumberOfVolunteersNeeded,PlaceOfEvent,TypeOfActivities,TypeOfEvent,AllocatedVolunteers,AllocatedSponsors\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");
        }

        public ActionResult Index(string searching, int page)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            if (page > 0)
                ViewBag.Page = page;
            else
                ViewBag.Page = 1;
            List<Event> events = eventcollection.AsQueryable().ToList();
            if (searching != null)
            {
                events = events.Where(x => x.NameOfEvent.Contains(searching)).ToList();
            }
            ViewBag.counter = events.Count();

            ViewBag.nrofdocs = nrofdocs;
            events = events.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            events = events.AsQueryable().Take(nrofdocs).ToList();
            try
            {
                ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(events);
        }

        public ActionResult VolunteerAllocation(string id, string searching)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            var names = events.Find(b => b.EventID.ToString() == id);
            names.AllocatedVolunteers = names.AllocatedVolunteers + " / ";
            ViewBag.strname = names.AllocatedVolunteers.ToString();
            ViewBag.Eventname = names.NameOfEvent.ToString();
            if (searching != null)
            {
                ViewBag.Evid = id;
                try
                {
                    ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
                }
                catch
                {
                    return RedirectToAction("Localserver");
                }
                return View(volunteers.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList());
            }
            else
            {
                try
                {
                    ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
                }
                catch
                {
                    return RedirectToAction("Localserver");
                }
                ViewBag.Evid = id;
                return View(volunteers);
            }
        }

        

        [HttpPost]
        public ActionResult VolunteerAllocation(string[] vols, string Evid)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
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
                try
                {
                    ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
                }
                catch
                {
                    return RedirectToAction("Localserver");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SponsorAllocation(string id, string searching)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
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
                try
                {
                    ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
                }
                catch
                {
                    return RedirectToAction("Localserver");
                }
                ViewBag.Evid = id;
                return View(sponsors);
            }
        }

        [HttpPost]
        public ActionResult SponsorAllocation(string[] spons, string Evid)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
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
                try
                {
                    ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
                }
                catch
                {
                    return RedirectToAction("Localserver");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Volunteer/Details/5
        public ActionResult Details(string id)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
            try
            {
                ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(eventt);
        }

        // GET: Volunteer/Create
        public ActionResult Create()
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            try
            {
                ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View();
        }

        // POST: Volunteer/Create
        [HttpPost]
        public ActionResult Create(Event eventt)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
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
                else return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Volunteer/Edit/5
        public ActionResult Edit(string id)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
            Event originalsavedevent = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
            ViewBag.originalsavedevent = JsonConvert.SerializeObject(originalsavedevent);
            try
            {
                ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(eventt);
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Event eventt, string Originalsavedeventstring)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
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
                    else return View();
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

        // GET: Volunteer/Delete/5
        public ActionResult Delete(string id)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == id);
            try
            {
                ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View(eventt);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            int nrofdocs = 0;
            String Am = TempData.Peek("numberofdocuments").ToString();
            String environment = TempData.Peek("environment").ToString();
            nrofdocs = Convert.ToInt16(Am);
            try
            {
                eventcollection.DeleteOne(Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id)));

                try
                {
                    ControllerHelper.setViewBagEnvironment(TempData, ViewBag);
                }
                catch
                {
                    return RedirectToAction("Localserver");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
