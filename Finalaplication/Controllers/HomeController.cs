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
        private MongoDBContextOffline dbcontextoffline;
        private IMongoCollection<Event> eventcollection;
        private IMongoCollection<Settings> settingcollection;
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Beneficiary> beneficiarycollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private IMongoCollection<Event> eventcollectionoffline;
        private IMongoCollection<Volunteer> vollunteercollectionoffline;
        private IMongoCollection<Beneficiary> beneficiarycollectionoffline;
        private IMongoCollection<Sponsor> sponsorcollectionoffline;

        public HomeController()
        {
            dbcontextoffline = new MongoDBContextOffline();
            try
            {
                settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
                Settings set = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));
                dbcontext = new MongoDBContext(set);
            }
            catch
            {
                Settings set = new Settings();
                set.Env = "offline";
                set.Lang = "English";
                set.Quantity = 15;
                dbcontext = new MongoDBContext(set);
            }
           
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");

            dbcontextoffline = new MongoDBContextOffline();
            settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
            eventcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Event>("Events");
            vollunteercollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Volunteer>("Volunteers");
            beneficiarycollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Sponsor>("Sponsors");
        }
        public ActionResult Backup()
        {
            try
            {
                Settings set = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));
                set.Env = "online";
                dbcontext = new MongoDBContext(set);
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
                Settings set = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));
                set.Env = "online";
                dbcontext = new MongoDBContext(set);
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
            try
            {
                Settings set = settingcollection.AsQueryable().FirstOrDefault(x => x.Env.Contains("i"));
                if (set.Env == "offline")
                    ViewBag.env = "offline";
                else
                    ViewBag.env = "online";
            }
            catch
            {
                return RedirectToAction("Localserver");
            }
            return View();
        }

        public IActionResult Localserver()
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

        public IActionResult Settings()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Settings(string lang, string env, int quantity)
        {
            Settings set = settingcollection.AsQueryable<Settings>().SingleOrDefault();
            set.Env = env;
            set.Quantity = quantity;
            set.Lang = lang;
            settingcollection.DeleteMany(x => x.Quantity >= 1);
            settingcollection.InsertOne(set);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}