using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class DatabaseManagementController : Controller
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
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollectionoffline;

        public DatabaseManagementController()
        {
            dbcontextoffline = new MongoDBContextOffline();

            dbcontext = new MongoDBContext();

            try
            {
                eventcollection = dbcontext.database.GetCollection<Event>("Events");
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                beneficiarycollection = dbcontext.database.GetCollection<Beneficiary>("Beneficiaries");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
                beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
                settingcollection = dbcontextoffline.databaseoffline.GetCollection<Settings>("Settings");
                eventcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Event>("Events");
                vollunteercollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Volunteer>("Volunteers");
                beneficiarycollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Beneficiary>("Beneficiaries");
                sponsorcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Sponsor>("Sponsors");
                volcontractcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Volcontract>("Contracts");
                beneficiarycontractcollectionoffline = dbcontextoffline.databaseoffline.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
                Settings set = settingcollection.AsQueryable().FirstOrDefault();
            }
            catch (Exception)
            {
            }
        }

        public IActionResult Servermanagement()
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

        public ActionResult Backupview()
        {
            return View();
        }

        public ActionResult Restoreview()
        {
            return View();
        }

        public ActionResult Mergeview()
        {
            return View();
        }

        public ActionResult Mergeview2()
        {
            return View();
        }

        public ActionResult Merge()
        {
            List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
            List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
            List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
            List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
            List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();
            List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();
            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
            List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

            string onlinevols = JsonConvert.SerializeObject(volunteers);
            string onlineevents = JsonConvert.SerializeObject(events);
            string onlinebenefieciaries = JsonConvert.SerializeObject(beneficiaries);
            string onlinesponsoprs = JsonConvert.SerializeObject(sponsors);
            string onlinevolcontrcarts = JsonConvert.SerializeObject(volcontracts);
            string onlinebeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontracts);

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
            for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
            {
                if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
                    beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
            }
            return RedirectToAction("Servermanagement");
        }

        public ActionResult Mergelocal()
        {
            List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
            List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
            List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
            List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
            List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();
            List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();

            List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
            List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

            string onlinevols = JsonConvert.SerializeObject(volunteers);
            string onlineevents = JsonConvert.SerializeObject(events);
            string onlinebenefieciaries = JsonConvert.SerializeObject(beneficiaries);
            string onlinesponsoprs = JsonConvert.SerializeObject(sponsors);
            string onlinevolcontrcarts = JsonConvert.SerializeObject(volcontracts);
            string onlinebeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontracts);

            for (int i = 0; i < volunteersoffline.Count(); i++)
            {
                if (!(onlinevols.Contains(volunteersoffline[i].VolunteerID)))
                    vollunteercollection.InsertOne(volunteersoffline[i]);
                if ((onlinevols.Contains(volunteersoffline[i].VolunteerID)))
                    vollunteercollection.ReplaceOne(z => z.VolunteerID == volunteersoffline[i].VolunteerID, volunteersoffline[i]);
            }

            for (int i = 0; i < eventsoffline.Count(); i++)
            {
                if (!(onlineevents.Contains(eventsoffline[i].EventID)))
                    eventcollection.InsertOne(eventsoffline[i]);
                if ((onlineevents.Contains(eventsoffline[i].EventID)))
                    eventcollection.ReplaceOne(z => z.EventID == eventsoffline[i].EventID, eventsoffline[i]);
            }

            for (int i = 0; i < beneficiariesoffline.Count(); i++)
            {
                if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
                    beneficiarycollection.InsertOne(beneficiariesoffline[i]);
                if ((onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
                    beneficiarycollection.ReplaceOne(z => z.BeneficiaryID == beneficiariesoffline[i].BeneficiaryID, beneficiariesoffline[i]);
            }

            for (int i = 0; i < sponsorsoffline.Count(); i++)
            {
                if (!(onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
                    sponsorcollection.InsertOne(sponsorsoffline[i]);
                if ((onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
                    sponsorcollection.ReplaceOne(z => z.SponsorID == sponsorsoffline[i].SponsorID, sponsorsoffline[i]);
            }

            for (int i = 0; i < volcontractsoffline.Count(); i++)
            {
                if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
                    volcontractcollection.InsertOne(volcontractsoffline[i]);
                if ((onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
                    volcontractcollection.ReplaceOne(z => z.ContractID == volcontractsoffline[i].ContractID, volcontractsoffline[i]);
            }
            for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
            {
                if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
                    beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
                if ((onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
                    beneficiarycontractcollection.ReplaceOne(z => z.ContractID == beneficiarycontractsoffline[i].ContractID, beneficiarycontractsoffline[i]);
            }
            return RedirectToAction("Servermanagement");
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
                List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

                dbcontextoffline.databaseoffline.DropCollection("Volunteers");
                dbcontextoffline.databaseoffline.DropCollection("Events");
                dbcontextoffline.databaseoffline.DropCollection("Beneficiaries");
                dbcontextoffline.databaseoffline.DropCollection("Sponsors");
                dbcontextoffline.databaseoffline.DropCollection("Contracts");
                dbcontextoffline.databaseoffline.DropCollection("BeneficiariesContracts");

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
                for (int i = 0; i < beneficiarycontracts.Count(); i++)
                {
                    beneficiarycontractcollectionoffline.InsertOne(beneficiarycontracts[i]);
                }

                return RedirectToAction("Servermanagement");
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
                List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();

                dbcontext.database.DropCollection("Volunteers");
                dbcontext.database.DropCollection("Events");
                dbcontext.database.DropCollection("Beneficiaries");
                dbcontext.database.DropCollection("Sponsors");
                dbcontext.database.DropCollection("Contracts");
                dbcontext.database.DropCollection("BeneficiariesContracts");

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
                for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
                {
                    beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
                }

                return RedirectToAction("Servermanagement");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}