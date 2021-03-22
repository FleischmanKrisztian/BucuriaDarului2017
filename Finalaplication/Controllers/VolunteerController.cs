using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolunteerHelpers;
using Finalaplication.DatabaseHandler;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IStringLocalizer<VolunteerController> _localizer;
        VolunteerManager volunteerManager = new VolunteerManager();
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Volcontract> volcontractcollection;
        private MongoDBContext dbcontext;
        public VolunteerController(IStringLocalizer<VolunteerController> localizer)
        {
            dbcontext = new MongoDBContext();
            volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            _localizer = localizer;
        }

        public ActionResult FileUpload()
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(IFormFile Files)
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            try
            {
                List<Volunteer> Volunteers = volunteerManager.GetListOfVolunteers();
                string path = " ";
                int docsimported = 0;
                if (UniversalFunctions.File_is_not_empty(Files))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Files.FileName);
                    UniversalFunctions.CreateFileStream(Files, path);
                }
                else
                {
                    return View();
                }
                List<string[]> volunteersasstring = CSVImportParser.GetListFromCSV(path);
                //TODO: make application be able to import different CSV format
                for (int i = 0; i < volunteersasstring.Count; i++)
                {
                    Volunteer volunteer = VolunteerFunctions.GetVolunteerFromString(volunteersasstring[i]);
                    if (VolunteerFunctions.DoesNotExist(Volunteers, volunteer))
                    {
                        docsimported++;
                        volunteerManager.AddVolunteerToDB(volunteer);
                    }
                }
                UniversalFunctions.RemoveTempFile(path);
                return RedirectToAction("ImportUpdate", "Beneficiary", new { docsimported });
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        public ActionResult Index(string lang, bool HasDrivingLicence, string searchingLastname, string searchedContact, string sortOrder, string searching, bool Active, bool HasCar, DateTime lowerdate, DateTime upperdate, DateTime activesince, DateTime activetill, int page, string gender, string searchedAddress, string searchedworkplace, string searchedOccupation, string searchedRemarks, int searchedHourCount)

        {
            try
            { 
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();

                if (HasDrivingLicence == true)
                { ViewBag.Filter1 = ""; }
                if (searchingLastname != null)
                { ViewBag.Filters2 = searchingLastname; }
                if (searchedContact != null)
                { ViewBag.Filter3 = searchedContact; }
                if (searching != null)
                { ViewBag.Filter4 = searching; }
                if (gender != null)
                { ViewBag.Filter5 = gender; }
                if (searchedAddress != null)
                { ViewBag.Filter6 = searchedAddress; }
                if (searchedworkplace != null)
                { ViewBag.Filter7 = searchedworkplace; }
                if (searchedRemarks != null)
                { ViewBag.Filter8 = searchedRemarks; }
                if (searchedOccupation != null)
                { ViewBag.Filter9 = searchedOccupation; }
                if (searchedHourCount != 0)
                { ViewBag.Filter10 = searchedHourCount.ToString(); }
                if (Active != false)
                { ViewBag.Filter11 = ""; }
                if (HasCar != false)
                { ViewBag.Filter12 = ""; }
                DateTime date = Convert.ToDateTime("01.01.0001 00:00:00");
                if (lowerdate != date)
                { ViewBag.Filter13 = lowerdate.ToString(); }
                if (upperdate != date)
                { ViewBag.Filter14 = upperdate.ToString(); }
                if (activesince != date)
                { ViewBag.Filter15 = activesince.ToString(); }
                if (activetill != date)
                { ViewBag.Filter16 = activetill.ToString(); }
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                ViewBag.page = UniversalFunctions.GetCurrentPage(page);
                ViewBag.lang = lang;
                ViewBag.searchingLastname = searchingLastname;
                ViewBag.searching = searching;
                ViewBag.active = Active;
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

                
                if (activetill < activesince && activetill > DateTime.Now.AddYears(-2000))
                {
                    ViewBag.wrongorder = true;
                    RedirectToPage("Index");
                }
                DateTime d1 = new DateTime(0003, 1, 1);
                if (searching != null)
                {
                     volunteers = volunteers.Where(x => x.Firstname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                if (searchingLastname != null)
                {
                     volunteers = volunteers.Where(x => x.Lastname.Contains(searchingLastname, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                if (Active == true)
                {
                    volunteers = volunteers.Where(x => x.InActivity == true).ToList();
                }
                if (searchedworkplace != null)
                {
                    volunteers = volunteers.Where(x => x.Desired_workplace.Contains(searchedworkplace, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                if (searchedOccupation != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach (var v in vol)
                    {
                        if (v.Field_of_activity == null || v.Field_of_activity == "")
                        { v.Field_of_activity = "-"; }
                        if (v.Occupation == null || v.Occupation == "")
                        { v.Occupation = "-"; }
                    }
                    try
                    {
                        volunteers = vol.Where(x => x.Field_of_activity.Contains(searchedOccupation, StringComparison.InvariantCultureIgnoreCase) || x.Occupation.Contains(searchedOccupation, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    catch { }
                }
                if (searchedRemarks != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach (var v in vol)
                    {
                        if (v.Additionalinfo.Remark == null || v.Additionalinfo.Remark == "")
                            v.Additionalinfo.Remark = "";
                    }
                    try
                    {
                        volunteers = vol.Where(x => x.Additionalinfo.Remark.Contains(searchedRemarks, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    catch { }
                }

                if (searchedContact != null)
                {
                    List<Volunteer> vol = volunteers;
                    foreach (var v in vol)
                    {
                        if (v.ContactInformation.PhoneNumber == null || v.ContactInformation.PhoneNumber == "")
                            v.ContactInformation.PhoneNumber = "-";
                        if (v.ContactInformation.MailAdress == null || v.ContactInformation.MailAdress == "")
                            v.ContactInformation.MailAdress = "-";
                    }
                    try
                    {
                        volunteers = vol.Where(x => x.ContactInformation.PhoneNumber.Contains(searchedContact, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAdress.Contains(searchedContact, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                if (activesince > d1 && activetill <= d1)
                {
                    string ids_to_remove = "";
                    foreach (Volunteer vol in volunteers)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = UniversalFunctions.Datereturner(vol.Activedates);
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
                if (activesince < d1 && activetill > d1)
                {
                    string ids_to_remove = "";
                    foreach (Volunteer vol in volunteers)
                    {
                        (DateTime[] startdates, DateTime[] enddates, int i) = UniversalFunctions.Datereturner(vol.Activedates);
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
                        (DateTime[] startdates, DateTime[] enddates, int i) = UniversalFunctions.Datereturner(vol.Activedates);
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
                if (gender != " All")
                {
                    if (gender == "Male")
                    {
                        volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Male)).ToList();
                    }
                    if (gender == "Female")
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

                string key = "FirstSessionVolunteer";
                HttpContext.Session.SetString(key, stringofids);

                return View(volunteers);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult CSVSaver()
        {
            string ids = HttpContext.Session.GetString(VolMongoConstants.SESSION_KEY_VOLUNTEER);
            HttpContext.Session.Remove(VolMongoConstants.SESSION_KEY_VOLUNTEER);
            string key = VolMongoConstants.SECONDARY_SESSION_KEY_VOLUNTEER;
            HttpContext.Session.SetString(key, ids);
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver(bool All, bool Name, bool Birthdate, bool Address, bool Gender, bool Desired_Workplace, bool CNP, bool Field_of_Activity, bool Occupation, bool CI_Info, bool Activity, bool Hour_Count, bool Contact_Information, bool Additional_info)
        {
            string IDS = HttpContext.Session.GetString(VolMongoConstants.SECONDARY_SESSION_KEY_VOLUNTEER);
            HttpContext.Session.Remove(VolMongoConstants.SECONDARY_SESSION_KEY_VOLUNTEER);
            string ids_and_fields = VolunteerFunctions.GetIdAndFieldString(IDS, All, Name, Birthdate, Address, Gender, Desired_Workplace, CNP, Field_of_Activity, Occupation, CI_Info, Activity, Hour_Count, Contact_Information, Additional_info);
            string key1 = VolMongoConstants.VOLUNTEERSESSION;
            string header = ControllerHelper.GetHeaderForExcelPrinterVolunteer(_localizer);
            string key2 = VolMongoConstants.VOLUNTEERHEADER;
            ControllerHelper.CreateDictionaries(key1, key2, ids_and_fields, header);
            string csvexporterlink = "csvexporterapp:" + key1 + ";" + key2;
            return Redirect(csvexporterlink);
        }

        public ActionResult Birthday()
        {
            try
            {
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                volunteers = VolunteerFunctions.GetVolunteersWithBirthdays(volunteers);
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

        public ActionResult Details(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var volunteer = volunteerManager.GetOneVolunteer(id);
                return View(volunteer);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

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

        [HttpPost]
        public ActionResult Create(Volunteer volunteer, IFormFile image)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                string volasstring = JsonConvert.SerializeObject(volunteer);
                bool containsspecialchar = UniversalFunctions.ContainsSpecialChar(volasstring);
                if (containsspecialchar)
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                }
                ModelState.Remove("Birthdate");
                ModelState.Remove("HourCount");
                ModelState.Remove("CIEliberat");
                if (ModelState.IsValid)
                {
                    volunteer.Birthdate = volunteer.Birthdate.AddHours(5);
                    volunteer.Image = VolunteerFunctions.addimage(image);

                    if (volunteer.InActivity == true)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("ro");
                        volunteer.Activedates = volunteer.Activedates + "," + DateTime.Today.AddHours(5).ToShortDateString() + "-currently";
                        volunteer.Activedates = volunteer.Activedates.Replace(" ", "");
                        volunteer.Activedates = volunteer.Activedates.Replace(".", "/");
                    }
                    volunteerManager.AddVolunteerToDB(volunteer);
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

        public ActionResult Edit(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var volunteer = volunteerManager.GetOneVolunteer(id);
                ViewBag.originalsavedvol = JsonConvert.SerializeObject(volunteer);
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
                    Volunteer currentsavedvol = volunteerManager.GetOneVolunteer(id);
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
                var volunteer = volunteerManager.GetOneVolunteer(id);
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
                        volunteerManager.DeleteAVolunteer(id);
                        volcontractcollection.DeleteMany(zzz => zzz.OwnerID == id);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        bool wasactive = false;

                        if (volunteer.InActivity == true)
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