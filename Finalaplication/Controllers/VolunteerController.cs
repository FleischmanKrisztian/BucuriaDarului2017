using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Finalaplication.Models;
using Finalaplication.App_Start;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System;
using System.Threading.Tasks;
using ReflectionIT.Mvc.Paging;

namespace Finalaplication.Controllers
{
    public class VolunteerController : Controller
    {
        private MongoDBContext dbcontext;
        private readonly IMongoCollection<Event> eventcollection;
        private IMongoCollection<Volunteer> vollunteercollection;

        public VolunteerController()
        {
            dbcontext = new MongoDBContext();
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
        }

        public ActionResult ExportVolunteers()
        {
            List<Volunteer> volunteers = vollunteercollection.AsQueryable().ToList();
            string path = "./jsondata/Volunteers.csv";



            var allLines = (from Volunteer in volunteers
                            select new object[]
                            {
                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20};",
                            Volunteer.Firstname,
                            Volunteer.Lastname,
                            Volunteer.Birthdate.ToString(),
                            Volunteer.Gender.ToString(),
                            Volunteer.Occupation,
                            Volunteer.Field_of_activity,
                            Volunteer.Desired_workplace,
                            Volunteer.InActivity.ToString(),
                            Volunteer.HourCount.ToString(),
                            Volunteer.Additionalinfo.HasCar.ToString(),
                            Volunteer.Additionalinfo.HasDrivingLicence.ToString(),
                            Volunteer.Additionalinfo.Remark,
                            Volunteer.Address.Country,
                            Volunteer.Address.City,
                            Volunteer.Address.Number,
                            Volunteer.Address.Street,
                            Volunteer.Contract.NumberOfRegistration.ToString(),
                            Volunteer.Contract.HasContract.ToString(),
                            Volunteer.Contract.RegistrationDate.ToString(),
                            Volunteer.Contract.ExpirationDate.ToString(),
                            Volunteer.ContactInformation.MailAdress,
                            Volunteer.ContactInformation.PhoneNumber)


                            }
                             ).ToList();

            var csv1 = new StringBuilder();


            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));

            }
           );
            System.IO.File.WriteAllText(path, "Firstname,Lastname,Birthdate,Gender,Occupation,Filed_of_activity,Desired_workplace,InActivity,HourCount,HasCar,HasDrivingLicence,Remark,Country,City,Number,Street,NumberOfRegistration,HasContract,RegistrationDate,ExpirationDate,MailAddres,PhoneNumber\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");

        }

        public ActionResult Index(string sortOrder, string searching, bool Active, bool HasCar, bool HasContract, DateTime lowerdate, DateTime upperdate, int page)
        {
            ViewBag.searching = searching;
            ViewBag.active = Active;
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Upperdate = upperdate;
            ViewBag.Lowerdate = lowerdate;
            ViewBag.hascar = HasCar;
            ViewBag.hascontract = HasContract;

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
            if (HasContract == true)
            {
                volunteers = volunteers.Where(x => x.Contract.HasContract == true).ToList();
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
            volunteers = volunteers.AsQueryable().Skip((page - 1) * 5).ToList();
            volunteers = volunteers.AsQueryable().Take(5).ToList();
            return View(volunteers);
        }


        public ActionResult Birthday()
        {
            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            return View(volunteers);
        }

        public ActionResult ContractExp()
        {
            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            return View(volunteers);
        }


        // GET: Volunteer/Details/5
        public ActionResult Details(string id)
        {
            var volunteerId = new ObjectId(id);
            var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == volunteerId);

            return View(volunteer);
        }

        // GET: Volunteer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Volunteer/Create
        [HttpPost]
        public ActionResult Create(Volunteer volunteer)
        {
            try
            {
                ModelState.Remove("Birthdate");
                ModelState.Remove("HourCount");
                ModelState.Remove("Contract.RegistrationDate");
                ModelState.Remove("Contract.ExpirationDate");
                if (ModelState.IsValid)
                {
                    volunteer.Birthdate = volunteer.Birthdate.AddHours(5);
                    volunteer.Contract.RegistrationDate = volunteer.Contract.RegistrationDate.AddHours(5);
                    volunteer.Contract.ExpirationDate = volunteer.Contract.ExpirationDate.AddHours(5);
                    vollunteercollection.InsertOne(volunteer);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Volunteer/Edit/5
        public ActionResult Edit(string id)
        {
            var volunteerId = new ObjectId(id);
            var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == volunteerId);
            return View(volunteer);
        }

        public ActionResult Volunteerwarning()
        {
            return View();
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Volunteer volunteer)
        {
            try
            {
                ModelState.Remove("Birthdate");
                ModelState.Remove("HourCount");
                ModelState.Remove("Contract.RegistrationDate");
                ModelState.Remove("Contract.ExpirationDate");
                if (ModelState.IsValid)
                {
                    var filter = Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id));
                    var update = Builders<Volunteer>.Update
                        .Set("Firstname", volunteer.Firstname)
                        .Set("Lastname", volunteer.Lastname)
                        .Set("Birthdate", volunteer.Birthdate.AddHours(5))
                        .Set("Address.Country", volunteer.Address.Country)
                        .Set("Address.City", volunteer.Address.City)
                        .Set("Address.Street", volunteer.Address.Street)
                        .Set("Address.Number", volunteer.Address.Number)
                        .Set("Gender", volunteer.Gender)
                        .Set("Desired_workplace", volunteer.Desired_workplace)
                        .Set("Field_of_activity", volunteer.Field_of_activity)
                        .Set("Occupation", volunteer.Occupation)
                        .Set("InActivity", volunteer.InActivity)
                        .Set("HourCount", volunteer.HourCount)
                        .Set("Contract.HasContract", volunteer.Contract.HasContract)
                        .Set("Contract.NumberOfRegistration", volunteer.Contract.NumberOfRegistration)
                        .Set("Contract.RegistrationDate", volunteer.Contract.RegistrationDate.AddHours(5))
                        .Set("Contract.ExpirationDate", volunteer.Contract.ExpirationDate.AddHours(5))
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
            catch
            {
                return View();
            }
        }

        // GET: Volunteer/Delete/5
        public ActionResult Delete(string id)
        {
            var volunteerId = new ObjectId(id);
            var volunteer = vollunteercollection.AsQueryable<Volunteer>().SingleOrDefault(x => x.VolunteerID == volunteerId);
            return View(volunteer);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection collection, Volunteer volunteer, bool Inactive)
        {
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
    }
}