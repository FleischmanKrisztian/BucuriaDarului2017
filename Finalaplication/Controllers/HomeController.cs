using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Finalaplication.Models;
using MongoDB.Driver;
using Finalaplication.App_Start;
using System.Threading;
using System.Globalization;

namespace Finalaplication.Controllers
{
    public class HomeController : Controller
    {
        private MongoDBContext dbcontext;
        private MongoDBContextoffline dbcontextoffline;
        private IMongoCollection<Event> eventcollection;
        private readonly IMongoCollection<Volunteer> vollunteercollection;
        private readonly IMongoCollection<Beneficiary> beneficiarycollection;
        private readonly IMongoCollection<Sponsor> sponsorcollection;
        private IMongoCollection<Event> eventcollectionoffline;
        private IMongoCollection<Volunteer> vollunteercollectionoffline;
        private IMongoCollection<Beneficiary> beneficiarycollectionoffline;
        private IMongoCollection<Sponsor> sponsorcollectionoffline;

        public HomeController()
        {
            dbcontext = new MongoDBContext();
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");

            dbcontextoffline = new MongoDBContextoffline();
            eventcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Event>("Events");
            vollunteercollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Volunteer>("Volunteers");
            beneficiarycollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Sponsor>("Sponsors");
        }
        public ActionResult Backup()
        {
            try
            {
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                List<Event> events = eventcollection.AsQueryable<Event>().ToList();
                List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();

                dbcontextoffline.databaseoffline.DropCollection("Volunteers");
                dbcontextoffline.databaseoffline.DropCollection("Events");
                dbcontextoffline.databaseoffline.DropCollection("Beneficiaries");
                dbcontextoffline.databaseoffline.DropCollection("Sponsors");



                for (int i = 0; i < volunteers.Count(); i++)
                {
                    vollunteercollectionoffline.InsertOne(volunteers[i]);
                }

                for (int i = 0; i < events.Count(); i++)
                {
                    eventcollectionoffline.InsertOne(events[i]);
                }

                for (int i = 0; i < beneficiaries.Count(); i++)
                {
                    beneficiarycollectionoffline.InsertOne(beneficiaries[i]);
                }

                for (int i = 0; i < sponsors.Count(); i++)
                {
                    sponsorcollectionoffline.InsertOne(sponsors[i]);
                }

                return View("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        public ActionResult Restore()
        {
            try
            {
                List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
                List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
                List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
                List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();

                dbcontext.database.DropCollection("Volunteers");
                dbcontext.database.DropCollection("Events");
                dbcontext.database.DropCollection("Beneficiaries");
                dbcontext.database.DropCollection("Sponsors");



                for (int i = 0; i < volunteersoffline.Count(); i++)
                {
                    vollunteercollection.InsertOne(volunteersoffline[i]);
                }

                for (int i = 0; i < eventsoffline.Count(); i++)
                {
                    eventcollection.InsertOne(eventsoffline[i]);
                }

                for (int i = 0; i < beneficiariesoffline.Count(); i++)
                {
                    beneficiarycollection.InsertOne(beneficiariesoffline[i]);
                }

                for (int i = 0; i < sponsorsoffline.Count(); i++)
                {
                    sponsorcollection.InsertOne(sponsorsoffline[i]);
                }

                return View("Index");
            }
            catch
            {
                return View("Index");
            }
        }


        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult About()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}