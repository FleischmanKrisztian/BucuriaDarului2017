﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finalaplication.App_Start;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Finalaplication.Controllers
{
    public class SponsorController : Controller
    {
        private MongoDBContext dbcontext;
        private MongoDBContextOffline dbcontextoffline;
        private readonly IMongoCollection<Event> eventcollection;
        private IMongoCollection<Settings> settingcollection;
        private IMongoCollection<Sponsor> sponsorcollection;


        public SponsorController()
        {
            dbcontextoffline = new MongoDBContextOffline();
            settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();
            dbcontext = new MongoDBContext(set);
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            
        }

        public ActionResult ExportSponsors()
        {
            List<Sponsor> sponsors = sponsorcollection.AsQueryable().ToList();
            string path = "./jsondata/Sponsors.csv";



            var allLines = (from Sponsor in sponsors
                            select new object[]
                            {
                                 string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10};",
                                 Sponsor.NameOfSponsor,
                                 Sponsor.ContactInformation.PhoneNumber,
                                 Sponsor.ContactInformation.MailAdress,
                                 Sponsor.Contract.HasContract.ToString(),
                                 Sponsor.Contract.NumberOfRegistration.ToString(),
                                 Sponsor.Contract.RegistrationDate.ToString(),
                                 Sponsor.Contract.ExpirationDate.ToString(),
                                 Sponsor.Sponsorship.Date.ToString(),
                                 Sponsor.Sponsorship.MoneyAmount.ToString(),
                                 Sponsor.Sponsorship.WhatGoods,
                                 Sponsor.Sponsorship.GoodsAmount)
                            }
                             ).ToList();

            var csv1 = new StringBuilder();
            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));

            }
           );
            System.IO.File.WriteAllText(path, "NameOfSponsor,PhoneNumber,MailAdress,HasContract,NumberOfRegistration,RegistrationDate,ExpirationDate,DateOfSponsorships,MoneyAmount,WhatGoods,GoodsAmount\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");
        }


        public IActionResult Index(string searching, int page)
        {
            ViewBag.Page = page;
            List<Sponsor> sponsors = sponsorcollection.AsQueryable().ToList();
            if (searching != null)
            {
                sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching)).ToList();
            }
            ViewBag.counter = sponsors.Count();
            sponsors = sponsors.AsQueryable().Skip((page - 1) * 5).ToList();
            sponsors = sponsors.AsQueryable().Take(5).ToList();
            return View(sponsors);
        }

        public ActionResult ContractExp()
        {
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            return View(sponsors);
        }
        public ActionResult Details(string id)
        {
            var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);

            return View(sponsor);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sponsor sponsor)
        {
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
        public ActionResult Edit(string id)
        {
            var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
            Sponsor originalsavedvol = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
            ViewBag.originalsavedvol = JsonConvert.SerializeObject(originalsavedvol);
            return View(sponsor);
        }

        [HttpPost]
        public ActionResult Edit(string id, Sponsor sponsor, string Originalsavedvolstring)
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

        // GET: Volunteer/Delete/5
        public ActionResult Delete(string id)
        {
            var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == id);
            return View(sponsor);
        }

        // POST: Volunteer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Sponsor sponsor)
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
    }
}