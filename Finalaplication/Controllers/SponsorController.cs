using Elm.Core.Parsers;
using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VolCommon;

namespace Finalaplication.Controllers
{
    public class SponsorController : Controller
    {
        private MongoDBContext dbcontext;
        private readonly IMongoCollection<Event> eventcollection;
        private IMongoCollection<Sponsor> sponsorcollection;

        public SponsorController()
        {
            try
            {
                dbcontext = new MongoDBContext();
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            }
            catch { }
        }
        public ActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(IFormFile Files)
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

            foreach (var details in result)
            {
                Sponsor sponsor=new Sponsor();
                sponsor.NameOfSponsor = details[1];
                Sponsorship s = new Sponsorship();

                if (details[2] == null || details[2] == "")
                {
                    s.Date = DateTime.MinValue;
                }
                else
                {
                    DateTime data;
                    if (details[2].Contains("/") == true)
                    {
                        string[] date = details[2].Split(" ");
                        string[] FinalDate = date[0].Split("/");
                        data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                    }
                    else
                    {
                        string[] anotherDate = details[2].Split('.');
                        data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                    }

                    s.Date = data;
                }
                    s.MoneyAmount = details[3].Replace("/", ","); ;
                    s.WhatGoods = details[4].Replace("/", ","); ;
                    s.GoodsAmount = details[5].Replace("/", ","); ;
                    sponsor.Sponsorship = s;

                    Contract c = new Contract();
                    if (details[6] == "True" || details[6] == "true")
                    {
                        c.HasContract = true;
                    }
                    else
                    {
                        c.HasContract = false;
                    }

                c.NumberOfRegistration = details[7];


                     if(details[8] == null || details[8] == "")
                    {
                        c.RegistrationDate = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime dataS;
                        if (details[8].Contains("/") == true)
                        {
                            string[] date = details[8].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            dataS = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[8].Split('.');
                            dataS = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }

                        c.RegistrationDate = dataS;
                    } 

                    if(details[9] == null || details[9] == "")
                    {
                        c.ExpirationDate = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime dataS;
                        if (details[9].Contains("/") == true)
                        {
                            string[] date = details[9].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            dataS = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = details[9].Split('.');
                            dataS = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }

                        c.ExpirationDate = dataS;
                    }
                    sponsor.Contract = c;

                    ContactInformation ci = new ContactInformation();
                    ci.PhoneNumber = details[10].Replace("/",",");
                    ci.MailAdress= details[11].Replace("/", ","); ;
                sponsor.ContactInformation = ci;
                sponsorcollection.InsertOne(sponsor);



                }
                FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            return RedirectToAction("Index");
        }


        public IActionResult Index(string searching, int page)
        {
            try
            {
                ViewBag.searching = searching;
                int nrofdocs = ControllerHelper.getNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                if (page > 0)
                    ViewBag.Page = page;
                else
                    ViewBag.Page = 1;
                List<Sponsor> sponsors = sponsorcollection.AsQueryable().ToList();
                if (searching != null)
                {
                    sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching)).ToList();
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

        public ActionResult Edit(string id)
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
                Sponsor originalsavedvol = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
                ViewBag.originalsavedvol = JsonConvert.SerializeObject(originalsavedvol);
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
