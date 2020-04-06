using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Finalaplication.Controllers
{
    public class SponsorController : Controller
    {
        private MongoDBContext dbcontext;
        private readonly IMongoCollection<Event> eventcollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private readonly IStringLocalizer<SponsorController> _localizer;

        public SponsorController(IStringLocalizer<SponsorController> localizer)
        {
            try
            {
                dbcontext = new MongoDBContext();
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                _localizer = localizer;
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
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                Thread myNewThread = new Thread(() => ControllerHelper.GetSponsorsFromCsv(sponsorcollection, result));
                myNewThread.Start();
                myNewThread.Join();

                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("IncorrectFile", "Home");
            }
        }

        [HttpGet]
        public ActionResult CSVSaver()
        {
            string ids = HttpContext.Session.GetString("FirstSessionSponsor");
            HttpContext.Session.Remove("FirstSessionSponsor");
            ids = "csvexporterapp:" + ids;

            string key2 = "SecondSessionSponsor";
             HttpContext.Session.SetString(key2, ids);
          
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            return View();
        }

        [HttpPost]
        public ActionResult CSVSaver( bool All, bool NameOfSponsor, bool Date, bool MoneyAmount, bool WhatGoods, bool GoodsAmount, bool HasContract, bool ContractDetails, bool PhoneNumber, bool MailAdress)
        {
            var IDS = HttpContext.Session.GetString("SecondSessionSponsor");
           
            string ids_and_options = IDS + "(((";
            if (All == true)
                ids_and_options = ids_and_options + "0";
            if (NameOfSponsor == true)
                ids_and_options = ids_and_options + "1";
            if (Date == true)
                ids_and_options = ids_and_options + "2";
            if (HasContract == true)
                ids_and_options = ids_and_options + "3";
            if (ContractDetails == true)
                ids_and_options = ids_and_options + "4";
            if (PhoneNumber == true)
                ids_and_options = ids_and_options + "5";
            if (MailAdress == true)
                ids_and_options = ids_and_options + "6";
            if (MoneyAmount == true)
                ids_and_options = ids_and_options + "7";
            if (WhatGoods == true)
                ids_and_options = ids_and_options + "8";
            if (GoodsAmount == true)
                ids_and_options = ids_and_options + "9";

            string key1 = "sponsorSession";
            ControllerHelper helper = new ControllerHelper();
            string header = helper.GetHeaderForExcelPrinterSponsor(_localizer);
            string key2 = "sponsorHeader";
            //DictionaryHelper.d.Add(key1, ids_and_options);
            //DictionaryHelper.d.Add(key2,header);
            string ids_and_optionssecond = "csvexporterapp:" + ";" + key1 + ";" + key2;

            return Redirect(ids_and_optionssecond);

            
        }

        public IActionResult Index(string searching, int page, string ContactInfo, DateTime lowerdate, DateTime upperdate, bool HasContract, string WhatGoods, string MoneyAmount, string GoodsAmounts)

        {
            try
            {
                if (searching != null)
                { ViewBag.Filters1 = searching; }
                if (ContactInfo != null)
                { ViewBag.Filters2 = ContactInfo; }

                if (HasContract == true)
                { ViewBag.Filters3 = ""; }
                if (WhatGoods != null)
                { ViewBag.Filters4 = WhatGoods; }
                if (MoneyAmount != null)
                { ViewBag.Filters5 = MoneyAmount; }
                if (GoodsAmounts != null)
                { ViewBag.Filters6 = GoodsAmounts; }
                DateTime date = Convert.ToDateTime("01.01.0001 00:00:00");
                if (lowerdate != date)
                { ViewBag.Filter7 = lowerdate.ToString(); }

                if (upperdate != date)
                { ViewBag.Filter8 = upperdate.ToString(); }
                List<Sponsor> sponsors = sponsorcollection.AsQueryable().ToList();

                ViewBag.Contact = ContactInfo;
                ViewBag.searching = searching;
                ViewBag.Upperdate = upperdate;
                ViewBag.Lowerdate = lowerdate;
                ViewBag.HasContract = HasContract;
                ViewBag.WhatGoods = WhatGoods;
                ViewBag.GoodsAmount = GoodsAmounts;
                ViewBag.MoneyAmount = MoneyAmount;
                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;

                DateTime d1 = new DateTime(0003, 1, 1);
                if (searching != null)
                {
                    sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                if (ContactInfo != null)
                {
                    List<Sponsor> sp = sponsors;
                    foreach (var s in sp)
                    {
                        if (s.ContactInformation.PhoneNumber == null || s.ContactInformation.PhoneNumber == "")
                            s.ContactInformation.PhoneNumber = "-";
                        if (s.ContactInformation.MailAdress == null || s.ContactInformation.MailAdress == "")
                            s.ContactInformation.MailAdress = "-";
                    }
                    try
                    {
                        sponsors = sp.Where(x => x.ContactInformation.PhoneNumber.Contains(ContactInfo, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAdress.Contains(ContactInfo, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    catch { }
                }
                if (lowerdate > d1)
                {
                    sponsors = sponsors.Where(x => x.Sponsorship.Date > lowerdate).ToList();
                }
                if (upperdate > d1)
                {
                    sponsors = sponsors.Where(x => x.Sponsorship.Date <= upperdate).ToList();
                }
                if (HasContract == true)
                {
                    sponsors = sponsors.Where(x => x.Contract.HasContract == true).ToList();
                }
                if (WhatGoods != null)
                {
                    List<Sponsor> sp = sponsors;
                    foreach (var s in sp)
                    {
                        if (s.Sponsorship.WhatGoods == null || s.Sponsorship.WhatGoods == "")
                            s.Sponsorship.WhatGoods = "-";
                    }
                    try
                    {
                        sponsors = sp.Where(x => x.Sponsorship.WhatGoods.Contains(WhatGoods, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    catch { }
                }
                if (GoodsAmounts != null)
                {
                    List<Sponsor> sp = sponsors;
                    foreach (var s in sp)
                    {
                        if (s.Sponsorship.GoodsAmount == null || s.Sponsorship.GoodsAmount == "")
                            s.Sponsorship.GoodsAmount = "-";
                    }
                    try
                    {
                        sponsors = sp.Where(x => x.Sponsorship.GoodsAmount.Contains(GoodsAmounts, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    catch { }
                }
                if (MoneyAmount != null)
                {
                    List<Sponsor> sp = sponsors;
                    foreach (var s in sp)
                    {
                        if (s.Sponsorship.MoneyAmount == null || s.Sponsorship.MoneyAmount == "")
                            s.Sponsorship.MoneyAmount = "-";
                    }
                    try
                    {
                        sponsors = sp.Where(x => x.Sponsorship.MoneyAmount.Contains(MoneyAmount, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                    catch { }
                }
                ViewBag.counter = sponsors.Count();
                ViewBag.nrofdocs = nrofdocs;
                string stringofids = "sponsors";
                foreach (Sponsor ben in sponsors)
                {
                    stringofids = stringofids + "," + ben.SponsorID;
                }
                ViewBag.stringofids = stringofids;
                sponsors = sponsors.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
                sponsors = sponsors.AsQueryable().Take(nrofdocs).ToList();

                string key = "FirstSessionSponsor";
                //DictionaryHelper.d.Add(key, new DictionaryHelper(stringofids));
                HttpContext.Session.SetString(key, stringofids);
                return View(sponsors);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult ContractExp()
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
                return View(sponsors);
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
                var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
                return View(sponsor);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

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
        public ActionResult Create(Sponsor sponsor)
        {
            try
            {
                string volasstring = JsonConvert.SerializeObject(sponsor);
                bool containsspecialchar = false;
                if (volasstring.Contains(";"))
                {
                    ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                    containsspecialchar = true;
                }
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                try
                {
                    ModelState.Remove("Contract.RegistrationDate");
                    ModelState.Remove("Contract.ExpirationDate");
                    ModelState.Remove("Sponsorship.Date");
                    if (ModelState.IsValid)
                    {
                        sponsor.Contract.RegistrationDate = sponsor.Contract.RegistrationDate.AddHours(5);
                        sponsor.Contract.ExpirationDate = sponsor.Contract.ExpirationDate.AddHours(5);
                        sponsorcollection.InsertOne(sponsor);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.containsspecialchar = containsspecialchar;
                        return View();
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

        public ActionResult Edit(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
                Sponsor originalsavedvol = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
                ViewBag.originalsavedvol = JsonConvert.SerializeObject(originalsavedvol);
                ViewBag.id = id;
                return View(sponsor);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(string id, Sponsor sponsor, string Originalsavedvolstring)
        {
            try
            {
                Sponsor Originalsavedvol = JsonConvert.DeserializeObject<Sponsor>(Originalsavedvolstring);
                try
                {
                    string volasstring = JsonConvert.SerializeObject(sponsor);
                    bool containsspecialchar = false;
                    if (volasstring.Contains(";"))
                    {
                        ModelState.AddModelError("Cannot contain semi-colons", "Cannot contain semi-colons");
                        containsspecialchar = true;
                    }
                    var volunteerId = new ObjectId(id);
                    Sponsor currentsavedvol = sponsorcollection.Find(x => x.SponsorID == id).Single();
                    if (JsonConvert.SerializeObject(Originalsavedvol).Equals(JsonConvert.SerializeObject(currentsavedvol)))
                    {
                        ModelState.Remove("Contract.RegistrationDate");
                        ModelState.Remove("Contract.ExpirationDate");
                        ModelState.Remove("Sponsorship.Date");
                        if (ModelState.IsValid)
                        {
                            var filter = Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id));
                            var update = Builders<Sponsor>.Update
                                .Set("NameOfSponsor", sponsor.NameOfSponsor)
                                .Set("ContactInformation.PhoneNumber", sponsor.ContactInformation.PhoneNumber)
                                .Set("ContactInformation.MailAdress", sponsor.ContactInformation.MailAdress)
                                .Set("Contract.HasContract", sponsor.Contract.HasContract)
                                .Set("Contract.NumberOfRegistration", sponsor.Contract.NumberOfRegistration)
                                .Set("Contract.RegistrationDate", sponsor.Contract.RegistrationDate.AddHours(5))
                                .Set("Contract.ExpirationDate", sponsor.Contract.ExpirationDate.AddHours(5))
                                .Set("Sponsorship.Date", sponsor.Sponsorship.Date.AddHours(5))
                                .Set("Sponsorship.MoneyAmount", sponsor.Sponsorship.MoneyAmount)
                                .Set("Sponsorship.GoodsAmount", sponsor.Sponsorship.GoodsAmount)
                                .Set("Sponsorship.WhatGoods", sponsor.Sponsorship.WhatGoods);
                            var result = sponsorcollection.UpdateOne(filter, update);
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
                return View(sponsor);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Sponsor sponsor)
        {
            try
            {
                try
                {
                    sponsorcollection.DeleteOne(Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id)));
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