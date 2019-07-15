using System;
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

namespace Finalaplication.Controllers
{
    public class SponsorController : Controller
    {
        private MongoDBContext dbcontext;
        private readonly IMongoCollection<Event> eventcollection;
        private IMongoCollection<Sponsor> sponsorcollection;


        public SponsorController()
        {
            dbcontext = new MongoDBContext();
            eventcollection = dbcontext.database.GetCollection<Event>("events");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("sponsors");
            
        }

        public ActionResult ExportSponsors()
        {
            List<Sponsor> sponsors = sponsorcollection.AsQueryable().ToList();
            string path = "./jsondata/Sponsors.csv";



            var allLines = (from Sponsor in sponsors
                            select new object[]
                            {
                                 string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11};",
                                 Sponsor.NameOfSponsor,
                                 Sponsor.Contact.PhoneNumber.ToString(),
                                  Sponsor.Contact.MailAdress.ToString()),
                                 Sponsor.Contract.HasContract.ToString(),
                                 Sponsor.Contract.NumberOfRegistration.ToString(),
                                 Sponsor.Contract.RegistrationDate.ToString(),
                                 Sponsor.Contract.ExpirationDate.ToString(),
                                 Sponsor.Sponsorships.Date.ToString(),
                                 Sponsor.Sponsorships.TypeOfSupport,
                                 Sponsor.Sponsorships.MoneyAmount.ToString(),
                                 Sponsor.Sponsorships.GoodsAmount.ToString(),
                                  Sponsor.Sponsorships.WhatGoods

                            }
                             ).ToList();

            var csv1 = new StringBuilder();


            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));

            }
           );
            System.IO.File.WriteAllText(path, "NameOfSponsor,PhoneNumber,MailAdress,HasContract,NumberOfRegistration,RegistrationDate,ExpirationDate,DateOfSponsorships,TypeOfSuport,MoneyAmount,GoodsAmount,WhatGoods\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
            return RedirectToAction("Index");

        }

        public IActionResult Index(string searching)
        {
            List<Sponsor> sponsors = sponsorcollection.AsQueryable().ToList();
            if (searching != null)
            {
                return View(sponsors.Where(x => x.NameOfSponsor.Contains(searching)).ToList());
            }
            else
            {
                return View(sponsors);
            }
        }

        public ActionResult ContractExp()
        {
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            return View(sponsors);
        }
        public ActionResult Details(string id)
        {
            var sponsorId = new ObjectId(id);
            var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == sponsorId);

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
                
                sponsor.Contract.RegistrationDate = sponsor.Contract.RegistrationDate.AddHours(5);
                sponsor.Contract.ExpirationDate = sponsor.Contract.ExpirationDate.AddHours(5);
                sponsorcollection.InsertOne(sponsor);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(string id)
        {
            var sponsorId = new ObjectId(id);
            var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == sponsorId);
            return View(sponsor);
        }

        [HttpPost]
        public ActionResult Edit(string id, Sponsor sponsor)
        {
            try
            {
                var filter = Builders<Sponsor>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Sponsor>.Update
                    .Set("NameOfSponsor", sponsor.NameOfSponsor)
                    .Set("PhoneNumber",sponsor.Contact.PhoneNumber)
                    .Set("MailAdress",sponsor.Contact.MailAdress)
                    .Set("HasContract", sponsor.Contract.HasContract)
                    .Set("NumberOfRegistration", sponsor.Contract.NumberOfRegistration)
                    .Set("RegistrationDate", sponsor.Contract.RegistrationDate)
                    .Set("ExpirationDate", sponsor.Contract.ExpirationDate)
                    .Set("SponsorshipDate", sponsor.Sponsorships.Date)
                    .Set("TypeOfSuport", sponsor.Sponsorships.TypeOfSupport)
                    .Set("MoneyAmount", sponsor.Sponsorships.MoneyAmount)
                    .Set("GoodsAmount", sponsor.Sponsorships.GoodsAmount)
                     .Set("WhatGoods", sponsor.Sponsorships.WhatGoods);
                var result = sponsorcollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Volunteer/Delete/5
        public ActionResult Delete(string id)
        {
            var sponsorId = new ObjectId(id);
            var sponsor = sponsorcollection.AsQueryable<Sponsor>().SingleOrDefault(x => x.SponsorID == sponsorId);
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