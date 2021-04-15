using Finalaplication.App_Start;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.CommonDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Finalaplication.Controllers
{
    public class DatabaseManagementController : Controller
    {
        
        private EventManager eventManager = new EventManager();
        private SponsorManager sponsorManager = new SponsorManager();
        private VolunteerManager volunteerManager = new VolunteerManager();
        private SettingsManager settingsManager = new SettingsManager();
        private VolContractManager volContractManager = new VolContractManager();
        private BeneficiaryManager beneManager = new BeneficiaryManager();
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager();

        private EventManagerCommon eventManager_ = new EventManagerCommon();
        private SponsorManagerCommon sponsorManager_ = new SponsorManagerCommon();
        private VolunteerManagerCommon volunteerManager_ = new VolunteerManagerCommon();
        private VolunteerContractManagerCommon volContractManager_ = new VolunteerContractManagerCommon();
        private BeneficiaryManagerCommon beneManager_ = new BeneficiaryManagerCommon();
        private BeneficiaryContractManagerCommon beneficiaryContractManager_ = new BeneficiaryContractManagerCommon();

        public DatabaseManagementController()
        {
            //dbContextLocal = new MongoDBContextLocal();
            //dbContextCommon = new MongoDBContextCommon();

            //var eventcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            //var vollunteercollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            //var beneficiarycollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Beneficiary>("Beneficiaries");
            //var sponsorcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Sponsor>("Sponsors");
            //var volcontractcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            //var beneficiarycontractcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            //var eventcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Event>("Events");
            //var vollunteercollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Volunteer>("Volunteers");
            //var beneficiarycollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            //var sponsorcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            //var volcontractcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Volcontract>("Contracts");
            //var beneficiarycontractcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
        }

        public IActionResult Servermanagement()
        {
            try
            {
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

        public ActionResult Push()
        {
            return RedirectToAction("Servermanagement");
        }

        public ActionResult Pull()
        {
            List<Volunteer> volunteerslocal = volunteerManager.GetListOfVolunteers();
            List<Event> eventslocal = eventManager.GetListOfEvents();
            List<Beneficiary> beneficiarieslocal = beneManager.GetListOfBeneficiaries();
            List<Sponsor> sponsorslocal = sponsorManager.GetListOfSponsors();
            List<Volcontract> volcontractslocal = volContractManager.GetListOfVolunteersContracts();
            List<Beneficiarycontract> beneficiarycontractslocal = beneficiaryContractManager.GetListOfBeneficiariesContracts();
            List<Volunteer> volunteers = volunteerManager_.GetListOfVolunteers();
            List<Event> events = eventManager_.GetListOfEvents();
            List<Beneficiary> beneficiaries = beneManager_.GetListOfBeneficiaries();
            List<Sponsor> sponsors = sponsorManager_.GetListOfSponsors();
            List<Volcontract> volcontracts = volContractManager_.GetListOfVolunteersContracts();
            List<Beneficiarycontract> beneficiarycontracts = beneficiaryContractManager_.GetListOfBeneficiariesContracts();

            string localvols = JsonConvert.SerializeObject(volunteerslocal);
            string localevents = JsonConvert.SerializeObject(eventslocal);
            string localbenefieciaries = JsonConvert.SerializeObject(beneficiarieslocal);
            string localsponsors = JsonConvert.SerializeObject(sponsorslocal);
            string localvolcontrcarts = JsonConvert.SerializeObject(volcontractslocal);
            string localbeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontractslocal);

            //for (int i = 0; i < volunteersoffline.Count(); i++)
            //{
            //    if (!(onlinevols.Contains(volunteersoffline[i]._id)))
            //        vollunteercollection.InsertOne(volunteersoffline[i]);
            //}

            //for (int i = 0; i < eventsoffline.Count(); i++)
            //{
            //    if (!(onlineevents.Contains(eventsoffline[i]._id)))
            //        eventcollection.InsertOne(eventsoffline[i]);
            //}

            //for (int i = 0; i < beneficiariesoffline.Count(); i++)
            //{
            //    if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i]._id)))
            //        beneficiarycollection.InsertOne(beneficiariesoffline[i]);
            //}

            //for (int i = 0; i < sponsorsoffline.Count(); i++)
            //{
            //    if (!(onlinesponsoprs.Contains(sponsorsoffline[i]._id)))
            //        sponsorcollection.InsertOne(sponsorsoffline[i]);
            //}

            //for (int i = 0; i < volcontractsoffline.Count(); i++)
            //{
            //    if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i]._id)))
            //        volcontractcollection.InsertOne(volcontractsoffline[i]);
            //}
            //for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
            //{
            //    if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i]._id)))
            //        beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
            //}
            return RedirectToAction("Servermanagement");
        }

        //MERGE CU PRIORIOTATE LA CEA COMUNA.
        //public ActionResult Merge()
        //{
        //    List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
        //    List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
        //    List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
        //    List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
        //    List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();
        //    List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();
        //    List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
        //    List<Event> events = eventcollection.AsQueryable<Event>().ToList();
        //    List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
        //    List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
        //    List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
        //    List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

        //    string onlinevols = JsonConvert.SerializeObject(volunteers);
        //    string onlineevents = JsonConvert.SerializeObject(events);
        //    string onlinebenefieciaries = JsonConvert.SerializeObject(beneficiaries);
        //    string onlinesponsoprs = JsonConvert.SerializeObject(sponsors);
        //    string onlinevolcontrcarts = JsonConvert.SerializeObject(volcontracts);
        //    string onlinebeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontracts);

        //    for (int i = 0; i < volunteersoffline.Count(); i++)
        //    {
        //        if (!(onlinevols.Contains(volunteersoffline[i]._id)))
        //            vollunteercollection.InsertOne(volunteersoffline[i]);
        //    }

        //    for (int i = 0; i < eventsoffline.Count(); i++)
        //    {
        //        if (!(onlineevents.Contains(eventsoffline[i]._id)))
        //            eventcollection.InsertOne(eventsoffline[i]);
        //    }

        //    for (int i = 0; i < beneficiariesoffline.Count(); i++)
        //    {
        //        if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i]._id)))
        //            beneficiarycollection.InsertOne(beneficiariesoffline[i]);
        //    }

        //    for (int i = 0; i < sponsorsoffline.Count(); i++)
        //    {
        //        if (!(onlinesponsoprs.Contains(sponsorsoffline[i]._id)))
        //            sponsorcollection.InsertOne(sponsorsoffline[i]);
        //    }

        //    for (int i = 0; i < volcontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i]._id)))
        //            volcontractcollection.InsertOne(volcontractsoffline[i]);
        //    }
        //    for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i]._id)))
        //            beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
        //    }
        //    return RedirectToAction("Servermanagement");
        //}

        //MERGE CU PRIRITATE LA BAZA DE DATE LOCALA
        //public ActionResult Mergelocal()
        //{
        //    List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
        //    List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
        //    List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
        //    List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
        //    List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();
        //    List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();

        //    List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
        //    List<Event> events = eventcollection.AsQueryable<Event>().ToList();
        //    List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
        //    List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
        //    List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
        //    List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

        //    string onlinevols = JsonConvert.SerializeObject(volunteers);
        //    string onlineevents = JsonConvert.SerializeObject(events);
        //    string onlinebenefieciaries = JsonConvert.SerializeObject(beneficiaries);
        //    string onlinesponsoprs = JsonConvert.SerializeObject(sponsors);
        //    string onlinevolcontrcarts = JsonConvert.SerializeObject(volcontracts);
        //    string onlinebeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontracts);

        //    for (int i = 0; i < volunteersoffline.Count(); i++)
        //    {
        //        if (!(onlinevols.Contains(volunteersoffline[i]._id)))
        //            vollunteercollection.InsertOne(volunteersoffline[i]);
        //        if ((onlinevols.Contains(volunteersoffline[i]._id)))
        //            vollunteercollection.ReplaceOne(z => z._id == volunteersoffline[i]._id, volunteersoffline[i]);
        //    }

        //    for (int i = 0; i < eventsoffline.Count(); i++)
        //    {
        //        if (!(onlineevents.Contains(eventsoffline[i]._id)))
        //            eventcollection.InsertOne(eventsoffline[i]);
        //        if ((onlineevents.Contains(eventsoffline[i]._id)))
        //            eventcollection.ReplaceOne(z => z._id == eventsoffline[i]._id, eventsoffline[i]);
        //    }

        //    for (int i = 0; i < beneficiariesoffline.Count(); i++)
        //    {
        //        if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i]._id)))
        //            beneficiarycollection.InsertOne(beneficiariesoffline[i]);
        //        if ((onlinebenefieciaries.Contains(beneficiariesoffline[i]._id)))
        //            beneficiarycollection.ReplaceOne(z => z._id == beneficiariesoffline[i]._id, beneficiariesoffline[i]);
        //    }

        //    for (int i = 0; i < sponsorsoffline.Count(); i++)
        //    {
        //        if (!(onlinesponsoprs.Contains(sponsorsoffline[i]._id)))
        //            sponsorcollection.InsertOne(sponsorsoffline[i]);
        //        if ((onlinesponsoprs.Contains(sponsorsoffline[i]._id)))
        //            sponsorcollection.ReplaceOne(z => z._id == sponsorsoffline[i]._id, sponsorsoffline[i]);
        //    }

        //    for (int i = 0; i < volcontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i]._id)))
        //            volcontractcollection.InsertOne(volcontractsoffline[i]);
        //        if ((onlinevolcontrcarts.Contains(volcontractsoffline[i]._id)))
        //            volcontractcollection.ReplaceOne(z => z._id == volcontractsoffline[i]._id, volcontractsoffline[i]);
        //    }
        //    for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i]._id)))
        //            beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
        //        if ((onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i]._id)))
        //            beneficiarycontractcollection.ReplaceOne(z => z._id == beneficiarycontractsoffline[i]._id, beneficiarycontractsoffline[i]);
        //    }
        //    return RedirectToAction("Servermanagement");
        //}

        //public ActionResult Backup()
        //{
        //    try
        //    {
        //        List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
        //        List<Event> events = eventcollection.AsQueryable<Event>().ToList();
        //        List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
        //        List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
        //        List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
        //        List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

        //        dbcontextoffline.databaseoffline.DropCollection("Volunteers");
        //        dbcontextoffline.databaseoffline.DropCollection("Events");
        //        dbcontextoffline.databaseoffline.DropCollection("Beneficiaries");
        //        dbcontextoffline.databaseoffline.DropCollection("Sponsors");
        //        dbcontextoffline.databaseoffline.DropCollection("Contracts");
        //        dbcontextoffline.databaseoffline.DropCollection("BeneficiariesContracts");

        //        for (int i = 0; i < volunteers.Count(); i++)
        //        {
        //            vollunteercollectionoffline.InsertOne(volunteers[i]);
        //        }

        //        for (int i = 0; i < events.Count(); i++)
        //        {
        //            eventcollectionoffline.InsertOne(events[i]);
        //        }

        //        for (int i = 0; i < beneficiaries.Count(); i++)
        //        {
        //            beneficiarycollectionoffline.InsertOne(beneficiaries[i]);
        //        }

        //        for (int i = 0; i < sponsors.Count(); i++)
        //        {
        //            sponsorcollectionoffline.InsertOne(sponsors[i]);
        //        }

        //        for (int i = 0; i < volcontracts.Count(); i++)
        //        {
        //            volcontractcollectionoffline.InsertOne(volcontracts[i]);
        //        }
        //        for (int i = 0; i < beneficiarycontracts.Count(); i++)
        //        {
        //            beneficiarycontractcollectionoffline.InsertOne(beneficiarycontracts[i]);
        //        }

        //        return RedirectToAction("Servermanagement");
        //    }
        //    catch
        //    {
        //        return View("Localserver");
        //    }
        //}

        //public ActionResult Restore()
        //{
        //    try
        //    {
        //        List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
        //        List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
        //        List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
        //        List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
        //        List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();
        //        List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();

        //        dbcontext.database.DropCollection("Volunteers");
        //        dbcontext.database.DropCollection("Events");
        //        dbcontext.database.DropCollection("Beneficiaries");
        //        dbcontext.database.DropCollection("Sponsors");
        //        dbcontext.database.DropCollection("Contracts");
        //        dbcontext.database.DropCollection("BeneficiariesContracts");

        //        for (int i = 0; i < volunteersoffline.Count(); i++)
        //        {
        //            vollunteercollection.InsertOne(volunteersoffline[i]);
        //        }

        //        for (int i = 0; i < eventsoffline.Count(); i++)
        //        {
        //            eventcollection.InsertOne(eventsoffline[i]);
        //        }

        //        for (int i = 0; i < beneficiariesoffline.Count(); i++)
        //        {
        //            beneficiarycollection.InsertOne(beneficiariesoffline[i]);
        //        }

        //        for (int i = 0; i < sponsorsoffline.Count(); i++)
        //        {
        //            sponsorcollection.InsertOne(sponsorsoffline[i]);
        //        }

        //        for (int i = 0; i < volcontractsoffline.Count(); i++)
        //        {
        //            volcontractcollection.InsertOne(volcontractsoffline[i]);
        //        }
        //        for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
        //        {
        //            beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
        //        }

        //        return RedirectToAction("Servermanagement");
        //    }
        //    catch
        //    {
        //        return View("Localserver");
        //    }
        //}
    }
}