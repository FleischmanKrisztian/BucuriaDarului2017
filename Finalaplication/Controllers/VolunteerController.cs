using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Finalaplication.Models;
using Finalaplication.App_Start;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;

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



        public ActionResult Index(string searching)
        {
            List<Volunteer> volunteers = vollunteercollection.AsQueryable().ToList();
            if (searching != null)
            {
                return View(volunteers.Where(x => x.Firstname.Contains(searching) || x.Lastname.Contains(searching)).ToList());
            }
            else
            {
                return View(volunteers);
            }
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
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                vollunteercollection.DeleteOne(Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}