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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

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
        private IMongoCollection<Volcontract> volcontractcollection;
        private IMongoCollection<Volcontract> volcontractcollectionoffline;

        public HomeController()
        {
            dbcontextoffline = new MongoDBContextOffline();

            dbcontext = new MongoDBContext();
                       
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
            volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");

            dbcontextoffline = new MongoDBContextOffline();
            settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
            eventcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Event>("Events");
            vollunteercollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Volunteer>("Volunteers");
            beneficiarycollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Sponsor>("Sponsors");
            volcontractcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Volcontract>("Contracts");
        }

        public ActionResult Merge()
        {

            List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
            List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
            List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
            List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
            List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();

            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();

            string onlinevols = JsonConvert.SerializeObject(volunteers);
            string onlineevents = JsonConvert.SerializeObject(events);
            string onlinebenefieciaries = JsonConvert.SerializeObject(beneficiaries);
            string onlinesponsoprs = JsonConvert.SerializeObject(sponsors);
            string onlinevolcontrcarts = JsonConvert.SerializeObject(volcontracts);

            for (int i = 0; i < volunteersoffline.Count(); i++)
            {
                if (!(onlinevols.Contains(volunteersoffline[i].VolunteerID)))
                vollunteercollection.InsertOne(volunteersoffline[i]);
            }

            for (int i = 0; i < eventsoffline.Count(); i++)
            {
                if (!(onlineevents.Contains(eventsoffline[i].EventID)))
                    eventcollection.InsertOne(eventsoffline[i]);
            }

            for (int i = 0; i < beneficiariesoffline.Count(); i++)
            {
                if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
                    beneficiarycollection.InsertOne(beneficiariesoffline[i]);
            }

            for (int i = 0; i < sponsorsoffline.Count(); i++)
            {
                if (!(onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
                    sponsorcollection.InsertOne(sponsorsoffline[i]);
            }

            for (int i = 0; i < volcontractsoffline.Count(); i++)
            {
                if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
                    volcontractcollection.InsertOne(volcontractsoffline[i]);
            }
            return RedirectToAction("Index");
        }
            

        public ActionResult Backup()
        {
            try
            {
                dbcontext = new MongoDBContext();
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                List<Event> events = eventcollection.AsQueryable<Event>().ToList();
                List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
                List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();

                dbcontextoffline.databaseoffline.DropCollection("Volunteers");
                dbcontextoffline.databaseoffline.DropCollection("Events");
                dbcontextoffline.databaseoffline.DropCollection("Beneficiaries");
                dbcontextoffline.databaseoffline.DropCollection("Sponsors");
                dbcontextoffline.databaseoffline.DropCollection("Contracts");



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

                for (int i = 0; i < volcontracts.Count(); i++)
                {
                    volcontractcollectionoffline.InsertOne(volcontracts[i]);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Restore()
        {
            try
            {
                dbcontext = new MongoDBContext();
                List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
                List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
                List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
                List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
                List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();

                dbcontext.database.DropCollection("Volunteers");
                dbcontext.database.DropCollection("Events");
                dbcontext.database.DropCollection("Beneficiaries");
                dbcontext.database.DropCollection("Sponsors");
                dbcontext.database.DropCollection("Contracts");



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

                for (int i = 0; i < volcontractsoffline.Count(); i++)
                {
                    volcontractcollection.InsertOne(volcontractsoffline[i]);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }


        public IActionResult Index()
        {
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            int bd = 0;
            int vc = 0;
            int sc = 0;
            int bc = 0;

            foreach (var item in volunteers)
            {
                var Day = Finalaplication.Models.Volunteer.Nowdate();
                var voldays = Finalaplication.Models.Volunteer.Volbd(item);
                if ((Day <= voldays && Day + 10 > voldays) || (Day > 354 && 365 - (Day + 365 - Day - 2) >= voldays))
                {
                    bd++;
                }
            }
            if (bd != 0)
                ViewBag.nrofbds = bd;
            else
                ViewBag.nrofbds = 0;

            foreach (var item in volcontracts)
            {
                if (item.GetDayExpiration(item.ExpirationDate) == true)
                {
                    vc++;
                }
            }
            ViewBag.nrofvc = vc;

            foreach (var item in sponsors)
            {
                if (item.GetDayExpiration(item.Contract.ExpirationDate) == true)
                {
                    sc++;
                }
            }
            ViewBag.nrofsc = sc;

            foreach (var item in beneficiaries)
            {
                if (item.GetDayExpiration(item.Contract.ExpirationDate) == true)
                {
                    bc++;
                }
            }
            ViewBag.nrofbc = bc;

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
            ViewBag.Lang = lang;

            settingcollection.ReplaceOne(y => y.Env.Contains("i"), set);
            return RedirectToAction("Index");
        }

       
            //[HttpPost]
            //public IActionResult SelectCulture(string culture, string returnUrl)
            //{
            //    Response.Cookies.Append(
            //        CookieRequestCultureProvider.DefaultCookieName,
            //        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            //    );

            //    return LocalRedirect(returnUrl);
            //}
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}