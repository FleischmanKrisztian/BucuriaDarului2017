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
        private  IMongoCollection<Event> eventcollectionoffline;
        private  IMongoCollection<Volunteer> vollunteercollectionoffline;
        private  IMongoCollection<Beneficiary> beneficiarycollectionoffline;
        private  IMongoCollection<Sponsor> sponsorcollectionoffline;

        public HomeController()
        {
            dbcontext = new MongoDBContext();
            eventcollection = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");

            dbcontextoffline = new MongoDBContextoffline();
            eventcollectionoffline = dbcontext.database.GetCollection<Event>("Events");
            vollunteercollectionoffline = dbcontext.database.GetCollection<Volunteer>("Volunteers");
            beneficiarycollectionoffline = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollectionoffline = dbcontext.database.GetCollection<Sponsor>("Sponsors");
        }
        public ActionResult Transport()
        {
            try
            {
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                List<Event> events = eventcollection.AsQueryable<Event>().ToList();
                List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();

                Event eve = new Event()
                {
                    NameOfEvent = "HALLo"
                };
                eventcollectionoffline.InsertOne(eve);

                //eventcollectionoffline.InsertOne(events[0]);
                //eventcollectionoffline.InsertOne(events[1]);
                //eventcollectionoffline.InsertOne(events[2]);

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
