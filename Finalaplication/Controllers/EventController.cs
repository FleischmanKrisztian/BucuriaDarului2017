using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;

using Finalaplication.Models;
using Finalaplication.App_Start;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using MongoDB.Bson.Serialization;
using System.Text;
using System;

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
            eventcollection = dbcontext.database.GetCollection<Event>("events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("volunteers");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("sponsor");
        }

        public ActionResult Export()
        {

            List<Event> events = eventcollection.AsQueryable().ToList();
            string path = "./jsondata/Event.csv";
           
            var allLines = (
                            from Event in events
                            select new object[]
                            { string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            Event.NameOfEvent,
                              Event.DateOfEvent.ToString(),
                              Event.Duration.ToString(),
                              
                              Event.NumberOfVolunteersNeeded.ToString(),
                              Event.PlaceOfEvent,
                              Event.TypeOfActivities,
                              Event.TypeOfEvent,
                              Event.AllocatedVolunteers)


                            }
                             ).ToList();
            var csv1 = new StringBuilder();


            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));

            }
           );
            System.IO.File.WriteAllText(path, "NameOfEvent,DateOfEvent,Duration,NumberOfVolunteersNeeded,PlaceOfEvent,TypeOfActivities,TypeOfEvent,AllocatedVolunteers\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");
        }


        //public async System.Threading.Tasks.Task<ActionResult> ExportAsync()
        //{
        //    string json = eventcollection.ToString();
        //    using (var streamWriter = new StreamWriter("./jsondata/event.json"))
        //    {
        //        await streamWriter.WriteAsync("[");
        //        await eventcollection.Find(new BsonDocument())
        //            .ForEachAsync(async (document) =>
        //            {
        //                using (var stringWriter = new StringWriter())

        //                using (var jsonWriter = new MongoDB.Bson.IO.JsonWriter(stringWriter))
        //                {
        //                    var context = BsonSerializationContext.CreateRoot(jsonWriter);
        //                    eventcollection.DocumentSerializer.Serialize(context, document);
        //                    string line = stringWriter.ToString();
        //                    int lngth = line.Count();
        //                    line = line.Remove(lngth - 48);
        //                    line = line + "},";
        //                    await streamWriter.WriteLineAsync(line);
        //                }
        //            });
        //        await streamWriter.WriteAsync("]");
        //    }
        //    using (var streamWriter = new StreamWriter("./jsondata/volunteer.json"))
        //    {
        //        await vollunteercollection.Find(new BsonDocument())
        //            .ForEachAsync(async (document) =>
        //            {
        //                using (var stringWriter = new StringWriter())
        //                using (var jsonWriter = new MongoDB.Bson.IO.JsonWriter(stringWriter))
        //                {
        //                    var context = BsonSerializationContext.CreateRoot(jsonWriter);
        //                    vollunteercollection.DocumentSerializer.Serialize(context, document);
        //                    var line = stringWriter.ToString();
        //                    await streamWriter.WriteLineAsync(line);
        //                }
        //            });
        //    }

        //    return RedirectToAction("Index");
        //}

        public ActionResult Index(string searching)
        {
            List<Event> events = eventcollection.AsQueryable().ToList();
            if (searching != null)
            {
                return View(events.Where(x => x.NameOfEvent.Contains(searching)).ToList());
            }
            else
            {
                return View(events);
            }
        }

        public ActionResult VolunteerAllocation(string id, string searching)
        {
            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            var names = events.Find(b => b.EventID.ToString() == id);
            names.AllocatedVolunteers = names.AllocatedVolunteers + ".";
            ViewBag.strname = names.AllocatedVolunteers.ToString();
            ViewBag.Eventname = names.NameOfEvent.ToString();
            if (searching != null)
            {
                ViewBag.Evid = id;
                return View(volunteers.Where(x => x.Firstname.Contains(searching)).ToList());
            }
            else
            {
                ViewBag.Evid = id;
                return View(volunteers);
            }
        }

        [HttpPost]
        public ActionResult VolunteerAllocation(string[] vols, string Evid)
        {
            try
            {
                string volname = "";
                for (int i = 0; i < vols.Length; i++)
                {
                    var volunteerId = new ObjectId(vols[i]);
                    var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == volunteerId);

                    volname = volname + volunteer.Firstname + " " + volunteer.Lastname + "  ";
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


        public ActionResult SponsorsAllocation(string id, string searching)
        {
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            var names = events.Find(b => b.EventID.ToString() == id);
            names.AllocatedSponsors = names.AllocatedSponsors + ".";
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

        [HttpPost]
        public ActionResult SponsorAllocation(string[] spons, string Evid)
        {
            try
            {
                string sponsname = "";
                for (int i = 0; i < spons.Length; i++)
                {
                    var sponsorId = new ObjectId(spons[i]);
                    var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == sponsorId);

                    sponsname = sponsname+" " + sponsor.NameOfSponsor ;
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

        //public ActionResult returner(string evid, Volunteer vol)
        //{

        //}


        // GET: Volunteer/Details/5
        public ActionResult Details(string id)
        {
            var eventId = new ObjectId(id);
            var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == eventId);

            return View(eventt);
        }

        // GET: Volunteer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Volunteer/Create
        [HttpPost]
        public ActionResult Create(Event eventt)
        {
            try
            {
                eventt.DateOfEvent = eventt.DateOfEvent.AddHours(5);
                eventcollection.InsertOne(eventt);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Volunteer/Edit/5
        public ActionResult Edit(string id)
        {
            var eventId = new ObjectId(id);
            var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == eventId);
            return View(eventt);
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Event eventt)
        {
            try
            {
                var filter = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Event>.Update
                    .Set("NameOfEvent", eventt.NameOfEvent)
                    .Set("PlaceOfEvent", eventt.PlaceOfEvent)
                    .Set("DateOfEvent", eventt.DateOfEvent.AddHours(5))
                    .Set("NumberOfVolunteersNeeded", eventt.NumberOfVolunteersNeeded)
                    .Set("TypeOfActivities", eventt.TypeOfActivities)
                    .Set("TypeOfEvent", eventt.TypeOfEvent)
                    .Set("Duration", eventt.Duration);


                var result = eventcollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Volunteer/Delete/5
        public ActionResult Delete(string id)
        {
            var eventId = new ObjectId(id);
            var eventt = eventcollection.AsQueryable<Event>().SingleOrDefault(x => x.EventID == eventId);
            return View(eventt);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection)
        {
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
    }
}