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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
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

                int number = result.Count();
                TempData["numberOfFiles"]= number;
             

                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }

                

                

                Thread myNewThread = new Thread(() => ControllerHelper.GetVolunteersFromCsv(vollunteercollection
                 , result, duplicates
                  , documentsimported));
                myNewThread.Start();

                

                string docsimported = documentsimported.ToString();

               
                return RedirectToAction("ImportUpdate", "Beneficiary", new { duplicates, docsimported });
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

         public ActionResult Index(string lang,bool HasDrivingLicence,string searchingLastname, string searchedContact, string sortOrder, string searching, bool Active, bool HasCar, DateTime lowerdate, DateTime upperdate, DateTime activesince, DateTime activetill, int page,string gender,string searchedAddress,string searchedworkplace,string searchedOccupation,string searchedRemarks,int searchedHourCount)

        {
            try
            {
                if (activetill < activesince && activetill > DateTime.Now.AddYears(-2000))
                {
                    ViewBag.wrongorder = true;
                    RedirectToPage("Index");
                }
                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ViewBag.lang = lang;
                ViewBag.searchingLastname = searchingLastname;
                ViewBag.searching = searching;
                ViewBag.active = Active;
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;
                ViewBag.ContactInfo = searchedContact;
                ViewBag.SortOrder = sortOrder;
                ViewBag.Address = searchedAddress;
                ViewBag.Occupation = searchedOccupation;
                ViewBag.Remarks = searchedRemarks;
                ViewBag.HourCount = searchedHourCount;
                ViewBag.Upperdate = upperdate;
                ViewBag.Lowerdate = lowerdate;
                ViewBag.Activesince = activesince;
                ViewBag.Activetill = activetill;
                ViewBag.Gender = gender;
                ViewBag.hascar = HasCar;
                ViewBag.DesiredWorkplace = searchedworkplace;
                ViewBag.hasDriverLicence = HasDrivingLicence;
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
                    try
                    {
                        volunteers = volunteers.Where(x => x.Firstname.Contains(searching) ).ToList();
                    }
                    catch { }
                    }
                if (searchingLastname != null)
                {
                    try
                    {
                        volunteers = volunteers.Where(x => x.Lastname.Contains(searchingLastname)).ToList();
                    }
                    catch { }
                }
                if (Active == true)
                {
                    volunteers = volunteers.Where(x => x.InActivity == true).ToList();
                }
                if (searchedAddress != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach(var v in vol)
                    {
                        if (v.Address.District == null || v.Address.District == "")
                            v.Address.District = "-";
                        if (v.Address.City == null || v.Address.City == "")
                            v.Address.City = "-";
                        if (v.Address.Street == null || v.Address.Street == "")
                            v.Address.Street = "-";
                        if (v.Address.Number == null || v.Address.Number == "")
                            v.Address.Number = "-";
                    }

                        try
                        {
                        volunteers = vol.Where(x => x.Address.District.Contains(searchedAddress) || x.Address.City.Contains(searchedAddress)|| x.Address.Street.Contains(searchedAddress) || x.Address.Number.Contains(searchedAddress)).ToList();
                            
                        }
                        catch { }
                    
                }

                if (searchedworkplace != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach(var v in vol)
                    { if (v.Desired_workplace == null || v.Desired_workplace == "")
                        {
                            v.Desired_workplace = "-";
                        }
                    }
                    try
                    {
                        volunteers = vol.Where(x => x.Desired_workplace.Contains(searchedworkplace)).ToList();
                    }
                    catch { }
                    }
                if (searchedOccupation != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach(var v in vol)
                    {
                        if(v.Field_of_activity==null || v.Field_of_activity=="")
                        { v.Field_of_activity = "-"; }
                        if(v.Occupation==null ||v.Occupation=="")
                        { v.Occupation = "-"; }
                    }
                    try
                    {
                        volunteers = vol.Where(x => x.Field_of_activity.Contains(searchedOccupation) || x.Occupation.Contains(searchedOccupation)).ToList();
                    }
                    catch { }
                    }
                if (searchedRemarks != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach(var v in vol)
                    { if (v.Additionalinfo.Remark == null || v.Additionalinfo.Remark == "")
                            v.Additionalinfo.Remark = "";
                    }
                    try {

                        volunteers = vol.Where(x => x.Additionalinfo.Remark.Contains(searchedRemarks)).ToList();

                    }catch{ }
                }

                ////Not Working
                if(searchedContact!=null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach(var v in vol)
                    {
                        if (v.ContactInformation.PhoneNumber == null || v.ContactInformation.PhoneNumber == "")
                            v.ContactInformation.PhoneNumber = "-";
                        if (v.ContactInformation.MailAdress == null || v.ContactInformation.MailAdress == "")
                            v.ContactInformation.MailAdress = "-";
                    }
                    try
                    {
                        volunteers = vol.Where(x => x.ContactInformation.PhoneNumber.Contains(searchedContact) || x.ContactInformation.MailAdress.Contains(searchedContact)).ToList();
                    }
                    catch { }
                }
                if (searchedHourCount != 0)
                {
                    
                    volunteers = volunteers.Where(x => x.HourCount.Equals(searchedHourCount)).ToList();
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
                if (activesince > d1 && activetill <= d1)
                {
                    string ids_to_remove = "";
                    foreach (Volunteer vol in volunteers)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = ControllerHelper.Datereturner(vol.Activedates);
                        bool passed = false;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (startdates[j] > activesince || enddates[j] > activesince)
                            {
                                passed = true;
                                break;
                            }
                        }
                        if (!passed)
                        {
                            ids_to_remove = ids_to_remove + "," + vol.VolunteerID;
                        }
                    }
                    List<string> ids = ids_to_remove.Split(',').ToList();
                    foreach (string id in ids)
                    {
                        Volunteer voltodelete = volunteers.FirstOrDefault(x => x.VolunteerID.ToString() == id);
                        volunteers.Remove(voltodelete);
                    }
                }
                //IN CASE THERE IS NO START DATE
                if (activesince < d1 && activetill > d1)
                {
                    string ids_to_remove = "";
                    foreach (Volunteer vol in volunteers)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = ControllerHelper.Datereturner(vol.Activedates);
                        bool passed = false;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (startdates[j] < activetill || enddates[j] < activetill)
                            {
                                passed = true;
                                break;
                            }
                        }
                        if (!passed)
                        {
                            ids_to_remove = ids_to_remove + "," + vol.VolunteerID;
                        }
                    }
                    List<string> ids = ids_to_remove.Split(',').ToList();
                    foreach (string id in ids)
                    {
                        Volunteer voltodelete = volunteers.FirstOrDefault(x => x.VolunteerID.ToString() == id);
                        volunteers.Remove(voltodelete);
                    }
                }
                //IN CASE THERE ARE BOTH
                if (activesince > d1 && activetill > d1)
                {
                    string ids_to_remove = "";

                    foreach (Volunteer vol in volunteers)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = ControllerHelper.Datereturner(vol.Activedates);
                        bool passed = false;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (startdates[j] > activesince && startdates[j] < activetill)
                            {
                                passed = true;
                                break;
                            }
                            else if (enddates[j] > activesince && enddates[j] < activetill)
                            {
                                passed = true;
                                break;
                            }
                            else if (startdates[j] < activesince && enddates[j] > activetill)
                            {
                                passed = true;
                                break;
                            }
                        }
                        if (!passed)
                        {
                            ids_to_remove = ids_to_remove + "," + vol.VolunteerID;
                        }
                    }
                    List<string> ids = ids_to_remove.Split(',').ToList();
                    foreach (string id in ids)
                    {
                        Volunteer voltodelete = volunteers.FirstOrDefault(x => x.VolunteerID.ToString() == id);
                        volunteers.Remove(voltodelete);
                    }
                }
                if (gender !=" All")
                {
                    if (gender =="Male")
                    {
                        volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Male)).ToList();
                    }
                    if(gender=="Female")
                    { volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Female)).ToList(); }
                }
               
                if (HasDrivingLicence == true)
                {
                    volunteers = volunteers.Where(x => x.Additionalinfo.HasDrivingLicence == true).ToList();
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
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(string IDS, bool All, bool Name, bool Birthdate, bool Address, bool Gender, bool Desired_Workplace, bool CNP, bool Field_of_Activity, bool Occupation, bool CI_Info, bool Activity, bool Hour_Count, bool Contact_Information, bool Additional_info)
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
                    if (volunteer.InActivity == true)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                        volunteer.Activedates = volunteer.Activedates + "," + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
                        volunteer.Activedates = volunteer.Activedates.Replace(" ", "");
                        volunteer.Activedates = volunteer.Activedates.Replace(".", "/");
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
                                Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                                volunteer.Activedates = volunteer.Activedates.Replace("currently", DateTime.Now.AddHours(5).ToShortDateString());
                                volunteer.Activedates = volunteer.Activedates.Replace(" ", "");
                                volunteer.Activedates = volunteer.Activedates.Replace(".", "/");
                            }
                            if (volunteer.InActivity == true && wasactive == false)
                            {
                                Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                                volunteer.Activedates = volunteer.Activedates + ", " + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
                                volunteer.Activedates = volunteer.Activedates.Replace(" ", "");
                                volunteer.Activedates = volunteer.Activedates.Replace(".", "/");
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
                                .Set("Activedates", volunteer.Activedates)
                                .Set("Additionalinfo.HasDrivingLicence", volunteer.Additionalinfo.HasDrivingLicence);

                            var result = vollunteercollection.UpdateOne(filter, update);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.originalsavedvol = Originalsavedvolstring;
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

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Volunteer volunteer, bool Inactive)
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
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                            volunteer.Activedates = volunteer.Activedates.Replace("currently", DateTime.Now.AddHours(5).ToShortDateString());
                            volunteer.Activedates = volunteer.Activedates.Replace(" ", "");
                            volunteer.Activedates = volunteer.Activedates.Replace(".", "/");
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
