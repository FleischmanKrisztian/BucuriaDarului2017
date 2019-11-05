using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class VolunteerController : Controller
    {
        private MongoDBContext dbcontext;
        private readonly IMongoCollection<Event> eventcollection;
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Volcontract> volcontractcollection;

        public VolunteerController()
        {
            try
            {
                dbcontext = new MongoDBContext();
                volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            }
            catch { }
        }

        public ActionResult Exportwithfiltersvolunteer(string searching, bool Active, bool HasCar, DateTime lowerdate, DateTime upperdate)
        {
            try
            {
                List<Volunteer> volunteers = vollunteercollection.AsQueryable().ToList();
                DateTime d1 = new DateTime(0003, 1, 1);
                if (upperdate > d1)
                {
                    volunteers = volunteers.Where(x => x.Birthdate <= upperdate).ToList();
                }
                if (searching != null)
                {
                    volunteers = volunteers.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList();
                }
                if (Active == true)
                {
                    volunteers = volunteers.Where(x => x.InActivity == true).ToList();
                }
                if (lowerdate > d1)
                {
                    volunteers = volunteers.Where(x => x.Birthdate > lowerdate).ToList();
                }
                if (HasCar == true)
                {
                    volunteers = volunteers.Where(x => x.Additionalinfo.HasCar == true).ToList();
                }

                ControllerHelper.ExportvolunteersAsDefaultCsv(volunteers);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ExportFailed", "Home");
            }
        }

        public ActionResult ExportVolunteers()
        {
            try
            {
                List<Volunteer> volunteers = vollunteercollection.AsQueryable().ToList();
                ControllerHelper.ExportvolunteersAsDefaultCsv(volunteers);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Index(string lang, string sortOrder, string searching, bool Active, bool HasCar, DateTime lowerdate, DateTime upperdate, int page)
        {
            try
            {
                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ViewBag.lang = lang;
                ViewBag.searching = searching;
                ViewBag.active = Active;
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;
                ViewBag.SortOrder = sortOrder;
                ViewBag.Upperdate = upperdate;
                ViewBag.Lowerdate = lowerdate;
                ViewBag.hascar = HasCar;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.LastnameSort = sortOrder == "Lastname" ? "Lastname_desc" : "Lastname";
                ViewBag.HourCountSort = sortOrder == "Hourcount" ? "Hourcount_desc" : "Hourcount";
                ViewBag.Gendersort = sortOrder == "Gender" ? "Gender_desc" : "Gender";
                ViewBag.Activesort = sortOrder == "Active" ? "Active_desc" : "Active";

                List<Volunteer> volunteers = vollunteercollection.AsQueryable().ToList();
                DateTime d1 = new DateTime(0003, 1, 1);
                if (upperdate > d1)
                {
                    volunteers = volunteers.Where(x => x.Birthdate <= upperdate).ToList();
                }
                if (searching != null)
                {
                    volunteers = volunteers.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList();
                }
                if (Active == true)
                {
                    volunteers = volunteers.Where(x => x.InActivity == true).ToList();
                }
                if (lowerdate > d1)
                {
                    volunteers = volunteers.Where(x => x.Birthdate > lowerdate).ToList();
                }
                if (HasCar == true)
                {
                    volunteers = volunteers.Where(x => x.Additionalinfo.HasCar == true).ToList();
                }
                switch (sortOrder)
                {
                    case "Gender":
                        volunteers = volunteers.OrderBy(s => s.Gender).ToList();
                        break;

                    case "Gender_desc":
                        volunteers = volunteers.OrderByDescending(s => s.Gender).ToList();
                        break;

                    case "Lastname":
                        volunteers = volunteers.OrderBy(s => s.Lastname).ToList();
                        break;

                    case "Lastname_desc":
                        volunteers = volunteers.OrderByDescending(s => s.Lastname).ToList();
                        break;

                    case "Hourcount":
                        volunteers = volunteers.OrderBy(s => s.HourCount).ToList();
                        break;

                    case "Hourcount_desc":
                        volunteers = volunteers.OrderByDescending(s => s.HourCount).ToList();
                        break;

                    case "Active":
                        volunteers = volunteers.OrderBy(s => s.InActivity).ToList();
                        break;

                    case "Active_desc":
                        volunteers = volunteers.OrderByDescending(s => s.InActivity).ToList();
                        break;

                    case "name_desc":
                        volunteers = volunteers.OrderByDescending(s => s.Firstname).ToList();
                        break;

                    case "Date":
                        volunteers = volunteers.OrderBy(s => s.Birthdate).ToList();
                        break;

                    case "date_desc":
                        volunteers = volunteers.OrderByDescending(s => s.Birthdate).ToList();
                        break;

                    default:
                        volunteers = volunteers.OrderBy(s => s.Firstname).ToList();
                        break;
                }
                ViewBag.counter = volunteers.Count();
                ViewBag.nrofdocs = nrofdocs;
                volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                volunteers = volunteers.AsQueryable().Take(nrofdocs).ToList();
                return View(volunteers);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Birthday()
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                return View(volunteers);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Contracts(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                return RedirectToAction("Index", "Volcontract", new { idofvol = id });
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
                var volunteerId = new ObjectId(id);
                var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == id);
                return View(volunteer);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // GET: Volunteer/Create
        [HttpGet]
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
        public ActionResult Create(Volunteer volunteer, List<IFormFile> Image)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ModelState.Remove("Birthdate");
                ModelState.Remove("HourCount");
                ModelState.Remove("CIEliberat");
                if (ModelState.IsValid)
                {
                    volunteer.Birthdate = volunteer.Birthdate.AddHours(5);

                    foreach (var item in Image)
                    {
                        if (item.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                item.CopyTo(stream);
                                volunteer.Image = stream.ToArray();
                            }
                        }
                    }
                    vollunteercollection.InsertOne(volunteer);

                    return RedirectToAction("Index");
                }
                return View(volunteer);
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
                var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == id);
                Volunteer originalsavedvol = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == id);
                ViewBag.originalsavedvol = JsonConvert.SerializeObject(originalsavedvol);
                ViewBag.id = id;
                return View(volunteer);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(string id, Volunteer volunteer, string Originalsavedvolstring, IList<IFormFile> image)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);

                Volunteer Originalsavedvol = JsonConvert.DeserializeObject<Volunteer>(Originalsavedvolstring);
                try
                {
                    Volunteer currentsavedvol = vollunteercollection.Find(x => x.VolunteerID == id).Single();
                    if (JsonConvert.SerializeObject(Originalsavedvol).Equals(JsonConvert.SerializeObject(currentsavedvol)))
                    {
                        ModelState.Remove("Birthdate");
                        ModelState.Remove("HourCount");
                        if (ModelState.IsValid)
                        {
                            var filter = Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id));

                            foreach (var item in image)
                            {
                                if (item.Length > 0)
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                        item.CopyTo(stream);
                                        volunteer.Image = stream.ToArray();
                                    }
                                }
                            }

                            var update = Builders<Volunteer>.Update
                                .Set("Image", volunteer.Image)
                                .Set("Firstname", volunteer.Firstname)
                                .Set("Lastname", volunteer.Lastname)
                                .Set("Birthdate", volunteer.Birthdate.AddHours(5))
                                .Set("Address.District", volunteer.Address.District)
                                .Set("Address.City", volunteer.Address.City)
                                .Set("Address.Street", volunteer.Address.Street)
                                .Set("Address.Number", volunteer.Address.Number)
                                .Set("Gender", volunteer.Gender)
                                .Set("CNP", volunteer.CNP)
                                .Set("Desired_workplace", volunteer.Desired_workplace)
                                .Set("Field_of_activity", volunteer.Field_of_activity)
                                .Set("Occupation", volunteer.Occupation)
                                .Set("InActivity", volunteer.InActivity)
                                .Set("HourCount", volunteer.HourCount)
                                .Set("CIseria", volunteer.CIseria)
                                .Set("CINr", volunteer.CINr)
                                .Set("CIEliberat", volunteer.CIEliberat)
                                .Set("CIeliberator", volunteer.CIeliberator)
                                .Set("ContactInformation.PhoneNumber", volunteer.ContactInformation.PhoneNumber)
                                .Set("ContactInformation.MailAdress", volunteer.ContactInformation.MailAdress)
                                .Set("Additionalinfo.HasCar", volunteer.Additionalinfo.HasCar)
                                .Set("Additionalinfo.Remark", volunteer.Additionalinfo.Remark)
                                .Set("Additionalinfo.HasDrivingLicence", volunteer.Additionalinfo.HasDrivingLicence);
                            var result = vollunteercollection.UpdateOne(filter, update);
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
                    return RedirectToAction("Error");
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
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            var volunteerId = new ObjectId(id);
            var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == id);
            return View(volunteer);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection, Volunteer volunteer, bool Inactive)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    if (Inactive == false)
                    {
                        vollunteercollection.DeleteOne(Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id)));

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var filter = Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id));
                        var update = Builders<Volunteer>.Update
                            .Set("InActivity", volunteer.InActivity);
                        var result = vollunteercollection.UpdateOne(filter, update);
                        return RedirectToAction("Index");
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
    }
}
