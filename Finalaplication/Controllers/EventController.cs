using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
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
            try
            {
                dbcontext = new MongoDBContext();
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            }
            catch { }
        }

        public ActionResult Index(string searching, int page)
        {
            try
            {
                ViewBag.searching = searching;
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
                ViewBag.counter = events.Count();

                ViewBag.nrofdocs = nrofdocs;
                string stringofids="events";
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
