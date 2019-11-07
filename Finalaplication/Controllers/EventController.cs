using CsvHelper;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Finalaplication.Controllers
{
    public class EventController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Event> eventcollection;
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private IHostingEnvironment hostingEnv;

        public EventController(IHostingEnvironment env)
        {
            try
            {
                dbcontext = new MongoDBContext();
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                this.hostingEnv = env;
            }
            catch { }
        }

        public ActionResult FileUpload(IFormFile Files)
        {

            if (Files.Length > 0)
            {
                var filename = "evenimenteTest.csv";
                //string path = Path.Combine(IHostingEnvironment.WebRootPath("~/UploadedFiles"), fileName);
                //string fileContent = "";
               var filePath = Path.GetFullPath(filename);
                //using (System.IO.StreamReader Reader = new System.IO.StreamReader(filePath))
                //{
                //    fileContent = Reader.ReadToEnd();

                //}

                //CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                //CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
                //CsvEventMapping csvMapper = new CsvEventMapping();
                //CsvParser<Event> csvParser = new CsvParser<Event>(csvParserOptions, csvMapper);
                //var result = csvParser
                //             .ReadFromFile(filePath, Encoding.UTF8)
                //             .ToList();
                // var result = csvParser
                //.ReadFromString(csvReaderOptions, fileContent)
                //.ToList();
                Event eventt = new Event();

                //foreach (var details in result)
                //{
                //    eventt.NameOfEvent = details.Result.NameOfEvent;
                //    eventt.PlaceOfEvent = details.Result.PlaceOfEvent;
                //    eventt.NumberOfVolunteersNeeded = details.Result.NumberOfVolunteersNeeded;
                //    eventt.TypeOfActivities = details.Result.TypeOfActivities;
                //    eventt.TypeOfEvent = details.Result.TypeOfEvent;
                //    eventt.Date = details.Result.Date;
                //   // eventt.Duration = details.Result.Duration;
                //    eventcollection.InsertOne(eventt);
                //}
            }
            //List<Event> result;
            //string jsonString;
            //var filePath = Path.GetTempPath();
            //using (var reader = new StreamReader(filePath))
            //        using (var csv = new CsvReader(reader))
            //       {

            //    csv.Configuration.HasHeaderRecord = true;
            //    csv.Read();
            //    result = csv.GetRecords<Event>().ToList();
            //}
            ////Csv data as Json string if needed
            //jsonString = JsonConvert.SerializeObject(result);
            //foreach (Event details in result)
            //{
            //    Event eventt = new Event();
            //    eventt.NameOfEvent = details.NameOfEvent;
            //    eventt.PlaceOfEvent = details.PlaceOfEvent;
            //    eventt.NumberOfVolunteersNeeded = details.NumberOfVolunteersNeeded;
            //    eventt.TypeOfActivities = details.TypeOfActivities;
            //    eventt.TypeOfEvent = details.TypeOfEvent;
            //    eventt.DateOfEvent = details.DateOfEvent;
            //    eventt.Duration = details.Duration;
            //    eventcollection.InsertOne(eventt);

            //}

            return RedirectToAction("Index");

        }


        public ActionResult Export()
        {
            try
            {
                List<Event> events = eventcollection.AsQueryable().ToList();
                string path = "./Excelfiles/Events.csv";
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
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Index(string searching, int page)
        {
            try
            {
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
