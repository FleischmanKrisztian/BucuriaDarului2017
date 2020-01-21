using Elm.Core.Parsers;
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
using VolCommon;

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
                string duplicates = "";
                int documentsimported = 0;

                foreach (var details in result)
                {
                    try
                    {
                        if (vollunteercollection.CountDocuments(z => z.CNP == details[9]) >= 1 && details[9] != "")
                        {
                            duplicates = duplicates + details[0] + " " + details[1] + ", ";
                        }
                        else if (vollunteercollection.CountDocuments(z => z.Firstname == details[0]) >= 1 && details[9] == "" && vollunteercollection.CountDocuments(z => z.Lastname == details[1]) >=1)
                        {
                            duplicates = duplicates + details[0] + " " + details[1] + ", ";
                        }
                        else
                        {
                            documentsimported++;
                            //Mistake;we added a new aux in order to save the value from the last cell;
                            var aux = details[22];
                            for (int i = details.Length - 1; i > 0; i--)
                            {
                                details[i] = details[i - 1];
                            }
                            Volunteer volunteer = new Volunteer();
                            try
                            {
                                volunteer.Firstname = details[1];
                                volunteer.Lastname = details[2];
                            }
                            catch
                            {
                                volunteer.Firstname = "Error";
                                volunteer.Lastname = "Error";
                            }

                            try
                            {
                                if (details[3] != null || details[3] != "")
                                {
                                    string[] date;
                                    date = details[3].Split(" ");

                                    string[] FinalDate = date[0].Split("/");
                                    DateTime data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);

                                    volunteer.Birthdate = data.AddDays(1);
                                }
                                else
                                { volunteer.Birthdate = DateTime.MinValue; }
                            }
                            catch
                            {
                                volunteer.Birthdate = DateTime.MinValue;
                            }

                            Address a = new Address();

                            if (details[4] != null || details[4] != "")
                            {
                                a.District = details[4];
                            }

                            if (details[5] != null || details[5] != "")
                            {
                                a.City = details[5];
                            }

                            if (details[6] != null || details[6] != "")
                            {
                                a.Street = details[6];
                            }

                            if (details[7] != null || details[7] != "")
                            {
                                a.Number = details[7];
                            }

                            try
                            {
                                if (details[8] == "True" || details[8] == "1")
                                {
                                    volunteer.Gender = VolCommon.Gender.Female;
                                }
                                else
                                {
                                    volunteer.Gender = VolCommon.Gender.Male;
                                }
                            }
                            catch
                            {
                                volunteer.Gender = Gender.Male;
                            }

                            if (details[9] != null || details[9] != "")
                            {
                                volunteer.Desired_workplace = details[9];
                            }

                            if (details[10] != null || details[10] != "")
                            {
                                volunteer.CNP = details[10];
                            }

                            if (details[11] != null || details[11] != "")
                            {
                                volunteer.Field_of_activity = details[11];
                            }

                            if (details[12] != null || details[12] != "")
                            {
                                volunteer.Occupation = details[12];
                            }

                            if (details[13] != null || details[13] != "")
                            {
                                volunteer.CIseria = details[13];
                            }

                            if (details[14] != null || details[14] != "")
                            {
                                volunteer.CINr = details[13];
                            }
                            try
                            {
                                if (details[15] != null || details[15] != "")
                                {
                                    string[] date;
                                    date = details[15].Split(" ");

                                    string[] FinalDate = date[0].Split("/");
                                    DateTime data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                    volunteer.CIEliberat = data.AddDays(1);
                                }
                                else
                                { volunteer.CIEliberat = DateTime.MinValue; }
                            }
                            catch
                            {
                                volunteer.CIEliberat = DateTime.MinValue;
                            }

                            if (details[16] != null || details[16] != "")
                            {
                                volunteer.CIeliberator = details[16];
                            }
                            if (details[17] == "True")
                            {
                                volunteer.InActivity = true;
                            }
                            else
                            {
                                volunteer.InActivity = false;
                            }

                            if (details[18] != null || details[18] != "0" || details[18] != "")
                            {
                                volunteer.HourCount = Convert.ToInt16(details[18]);
                            }
                            else
                            {
                                volunteer.HourCount = 0;
                            }
                            ContactInformation c = new ContactInformation();
                            if (details[19] != null || details[19] != "")
                            {
                                c.PhoneNumber = details[19];
                            }
                            if (details[20] != null || details[20] != "")
                            {
                                c.MailAdress = details[20];
                            }
                            volunteer.ContactInformation = c;
                            Additionalinfo ai = new Additionalinfo();

                            if (details[21] == "True")
                            {
                                ai.HasDrivingLicence = true;
                            }
                            else
                            {
                                ai.HasDrivingLicence = false;
                            }

                            if (details[22] == "True")
                            {
                                ai.HasCar = true;
                            }
                            else
                            {
                                ai.HasCar = false;
                            }

                            ai.Remark = aux;

                            volunteer.Address = a;
                            volunteer.Additionalinfo = ai;
                            vollunteercollection.InsertOne(volunteer);
                        }
                    }catch
                    {
                        if (vollunteercollection.CountDocuments(z => z.CNP == details[1]) >= 1 && details[1] != "")
                        {
                            duplicates = duplicates + details[0] +  ", ";
                        }
                        else if (vollunteercollection.CountDocuments(z => z.Firstname == details[0]) >= 1 && details[1] == "")
                        {
                            duplicates = duplicates + details[0] + ", ";
                        }
                        else
                        {
                            documentsimported++;
                            
                            Volunteer volunteer = new Volunteer();
                            try
                            {if(details[0].Contains(" ")==true)
                                   
                                   {string[] splitName= details[0].Split();
                                    volunteer.Lastname= splitName[0] ;
                                    if (splitName.Count() == 2)
                                    {
                                        volunteer.Firstname = splitName[1];
                                    }else
                                    { volunteer.Firstname = splitName[1] + " " + splitName[2]; }
                                }
                               
                            }
                            catch
                            {
                                volunteer.Firstname = "Error";
                                volunteer.Lastname = "Error";
                            }

                           
                           
                                volunteer.Birthdate = DateTime.MinValue;
                            


                            Address a = new Address();


                            if (details[2] != null || details[2] != "")
                            {
                              a.District=  details[2];
                                  
                            }

                            

                                volunteer.Gender = Gender.Male;
                            

                            if (details[9] != null || details[9] != "")
                            {
                                volunteer.Desired_workplace = details[9];
                            }

                            if (details[1] != null || details[1] != "")
                            {
                                volunteer.CNP = details[1];
                            }




                            if (details[3] != null)
                            { if (details[3] != "")
                                {
                                    string[] splited = details[3].Split(" ");
                                    volunteer.CIseria = splited[0];
                                    volunteer.CINr = splited[1];
                                }
                                else
                                {
                                    volunteer.CIseria = "Error";
                                    volunteer.CINr = "Error";
                                }

                            }

                            
                                volunteer.CIEliberat = DateTime.MinValue;
                            

                           

                            
                                volunteer.HourCount = 0;
                            
                           ContactInformation c = new ContactInformation();
                            if (details[4] != null || details[4] != "")
                            {
                                c.PhoneNumber = details[4];
                            }
                            
                            volunteer.ContactInformation = c;
                            Additionalinfo ai = new Additionalinfo();

                            
                                ai.HasDrivingLicence = false;

                            ai.Remark = details[8];
                           
                                ai.HasCar = false;
                            

                            

                            volunteer.Address = a;
                            volunteer.Additionalinfo = ai;
                            vollunteercollection.InsertOne(volunteer);
                        }
                    }

                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                string docsimported = documentsimported.ToString();
                return RedirectToAction("ImportUpdate","Beneficiary", new { duplicates, docsimported });
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        public ActionResult Index(string lang, string sortOrder, string searching, bool Active, bool HasCar, DateTime lowerdate, DateTime upperdate, DateTime activesince, DateTime activetill, int page)
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
                ViewBag.Activesince = activesince;
                ViewBag.Activetill = activetill;
                ViewBag.hascar = HasCar;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.LastnameSort = sortOrder == "Lastname" ? "Lastname_desc" : "Lastname";
                ViewBag.HourCountSort = sortOrder == "Hourcount" ? "Hourcount_desc" : "Hourcount";
                ViewBag.Gendersort = sortOrder == "Gender" ? "Gender_desc" : "Gender";
                ViewBag.Activesort = sortOrder == "Active" ? "Active_desc" : "Active";

                List<Volunteer> volunteers = vollunteercollection.AsQueryable().ToList();
                DateTime d1 = new DateTime(0003, 1, 1);
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
                if (upperdate > d1)
                {
                    volunteers = volunteers.Where(x => x.Birthdate <= upperdate).ToList();
                }
                //IN CASE THERE IS NO END DATE
                    if (activesince > d1 && activetill <= d1 )
                    {
                    }
                    //IN CASE THERE IS NO START DATE
                    if (activesince < d1 && activetill > d1)
                    {
                    }
                    //IN CASE THERE ARE BOTH
                    if (activesince > d1 && activetill > d1)
                    {
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
                string stringofids = "volunteers";
                foreach (Volunteer ben in volunteers)
                {
                    stringofids = stringofids + "," + ben.VolunteerID;
                }
                ViewBag.stringofids = stringofids;
                volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                volunteers = volunteers.AsQueryable().Take(nrofdocs).ToList();
                return View(volunteers);
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
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(string IDS, bool All, bool Name, bool Birthdate,bool Address, bool Gender, bool Desired_Workplace, bool CNP, bool Field_of_Activity, bool Occupation, bool CI_Info, bool Activity, bool Hour_Count, bool Contact_Information, bool Additional_info)
        {
            string ids_and_options = IDS + "(((";
            if (All == true)
                ids_and_options = ids_and_options + "0";
            if (Name == true)
                ids_and_options = ids_and_options + "1";
            if (Birthdate == true)
                ids_and_options = ids_and_options + "2";
            if (Address == true)
                ids_and_options = ids_and_options + "3";
            if (Gender == true)
                ids_and_options = ids_and_options + "4";
            if (Desired_Workplace == true)
                ids_and_options = ids_and_options + "5";
            if (CNP == true)
                ids_and_options = ids_and_options + "6";
            if (Field_of_Activity == true)
                ids_and_options = ids_and_options + "7";
            if (Occupation == true)
                ids_and_options = ids_and_options + "8";
            if (CI_Info == true)
                ids_and_options = ids_and_options + "9";
            if (Activity == true)
                ids_and_options = ids_and_options + "A";
            if (Hour_Count == true)
                ids_and_options = ids_and_options + "B";
            if (Contact_Information == true)
                ids_and_options = ids_and_options + "C";
            if (Additional_info == true)
                ids_and_options = ids_and_options + "D";
            ids_and_options = "csvexporterapp:" + ids_and_options;

            return Redirect(ids_and_options);


            //return RedirectToAction("Index");
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
                string volasstring = JsonConvert.SerializeObject(volunteer);
                bool containsspecialchar = false;
                if (volasstring.Contains(";"))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                    containsspecialchar = true;
                }
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
                    if(volunteer.InActivity==true)
                    {
                        volunteer.Activedates = volunteer.Activedates + ", " + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
                    }
                    vollunteercollection.InsertOne(volunteer);

                    return RedirectToAction("Index");
                }
                ViewBag.containsspecialchar = containsspecialchar;
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
                string volasstring = JsonConvert.SerializeObject(volunteer);
                bool containsspecialchar = false;
                if (volasstring.Contains(";"))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                    containsspecialchar = true;
                }
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
                            bool wasactive = false;

                            if (Originalsavedvol.InActivity == true)
                            {
                                wasactive = true;   
                            }
                            if (volunteer.InActivity == false && wasactive == true)
                            {
                                volunteer.Activedates = volunteer.Activedates.Replace("currently", DateTime.Now.AddHours(5).ToShortDateString());
                            }
                            if (volunteer.InActivity == true && wasactive == false)
                            {
                                volunteer.Activedates = volunteer.Activedates + ", " + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
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
                                .Set("Activedates",volunteer.Activedates)
                                .Set("Additionalinfo.HasDrivingLicence", volunteer.Additionalinfo.HasDrivingLicence);

                            var result = vollunteercollection.UpdateOne(filter, update);
                            return RedirectToAction("Index");
                        }

                        else
                        {
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
                        volcontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (volunteer.InActivity == false)
                        {
                            volunteer.Activedates = volunteer.Activedates.Replace("currently", DateTime.Now.AddHours(5).ToShortDateString());
                        }
                        var filter = Builders<Volunteer>.Filter.Eq("_id", ObjectId.Parse(id));
                        var update = Builders<Volunteer>.Update
                            .Set("InActivity", volunteer.InActivity)
                            .Set("Activedates", volunteer.Activedates);
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
