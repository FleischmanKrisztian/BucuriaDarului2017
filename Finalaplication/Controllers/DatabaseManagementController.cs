using Finalaplication.App_Start;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Finalaplication.Controllers
{
    public class DatabaseManagementController : Controller
    {
        private MongoDBContextCommon dbContextCommon;
        private MongoDBContextLocal dbContextLocal;

        private IMongoCollection<Event> eventcollectioncommon;
        private IMongoCollection<Event> eventcollectionlocal;
        private IMongoCollection<Volunteer> vollunteercollectioncommon;
        private IMongoCollection<Volunteer> vollunteercollectionlocal;
        private IMongoCollection<Beneficiary> beneficiarycollectioncommon;
        private IMongoCollection<Beneficiary> beneficiarycollectionlocal;
        private IMongoCollection<Sponsor> sponsorcollectioncommon;
        private IMongoCollection<Sponsor> sponsorcollectionlocal;
        private IMongoCollection<Volcontract> volcontractcollectioncommon;
        private IMongoCollection<Volcontract> volcontractcollectionlocal;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollectioncommon;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollectionlocal;

        public DatabaseManagementController()
        {
            dbContextLocal = new MongoDBContextLocal();
            dbContextCommon = new MongoDBContextCommon();

            eventcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Event>("Events");
            vollunteercollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Volunteer>("Volunteers");
            beneficiarycollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Sponsor>("Sponsors");
            volcontractcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Volcontract>("Contracts");
            beneficiarycontractcollectioncommon = dbContextCommon.DatabaseCommon.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            eventcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Event>("Events");
            vollunteercollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Volunteer>("Volunteers");
            beneficiarycollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Beneficiary>("Beneficiaries");
            sponsorcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Sponsor>("Sponsors");
            volcontractcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Volcontract>("Contracts");
            beneficiarycontractcollectionlocal = dbContextLocal.DatabaseLocal.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
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
            //List<Volunteer> volunteersoffline = vollunteercollectionoffline.AsQueryable<Volunteer>().ToList();
            //List<Event> eventsoffline = eventcollectionoffline.AsQueryable<Event>().ToList();
            //List<Beneficiary> beneficiariesoffline = beneficiarycollectionoffline.AsQueryable<Beneficiary>().ToList();
            //List<Sponsor> sponsorsoffline = sponsorcollectionoffline.AsQueryable<Sponsor>().ToList();
            //List<Volcontract> volcontractsoffline = volcontractcollectionoffline.AsQueryable<Volcontract>().ToList();
            //List<Beneficiarycontract> beneficiarycontractsoffline = beneficiarycontractcollectionoffline.AsQueryable<Beneficiarycontract>().ToList();
            //List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
            //List<Event> events = eventcollection.AsQueryable<Event>().ToList();
            //List<Beneficiary> beneficiaries = beneficiarycollection.AsQueryable<Beneficiary>().ToList();
            //List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();
            //List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
            //List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();

            //string onlinevols = JsonConvert.SerializeObject(volunteers);
            //string onlineevents = JsonConvert.SerializeObject(events);
            //string onlinebenefieciaries = JsonConvert.SerializeObject(beneficiaries);
            //string onlinesponsoprs = JsonConvert.SerializeObject(sponsors);
            //string onlinevolcontrcarts = JsonConvert.SerializeObject(volcontracts);
            //string onlinebeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontracts);

            //for (int i = 0; i < volunteersoffline.Count(); i++)
            //{
            //    if (!(onlinevols.Contains(volunteersoffline[i].VolunteerID)))
            //        vollunteercollection.InsertOne(volunteersoffline[i]);
            //}

            //for (int i = 0; i < eventsoffline.Count(); i++)
            //{
            //    if (!(onlineevents.Contains(eventsoffline[i].EventID)))
            //        eventcollection.InsertOne(eventsoffline[i]);
            //}

            //for (int i = 0; i < beneficiariesoffline.Count(); i++)
            //{
            //    if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
            //        beneficiarycollection.InsertOne(beneficiariesoffline[i]);
            //}

            //for (int i = 0; i < sponsorsoffline.Count(); i++)
            //{
            //    if (!(onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
            //        sponsorcollection.InsertOne(sponsorsoffline[i]);
            //}

            //for (int i = 0; i < volcontractsoffline.Count(); i++)
            //{
            //    if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
            //        volcontractcollection.InsertOne(volcontractsoffline[i]);
            //}
            //for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
            //{
            //    if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
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
        //        if (!(onlinevols.Contains(volunteersoffline[i].VolunteerID)))
        //            vollunteercollection.InsertOne(volunteersoffline[i]);
        //    }

        //    for (int i = 0; i < eventsoffline.Count(); i++)
        //    {
        //        if (!(onlineevents.Contains(eventsoffline[i].EventID)))
        //            eventcollection.InsertOne(eventsoffline[i]);
        //    }

        //    for (int i = 0; i < beneficiariesoffline.Count(); i++)
        //    {
        //        if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
        //            beneficiarycollection.InsertOne(beneficiariesoffline[i]);
        //    }

        //    for (int i = 0; i < sponsorsoffline.Count(); i++)
        //    {
        //        if (!(onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
        //            sponsorcollection.InsertOne(sponsorsoffline[i]);
        //    }

        //    for (int i = 0; i < volcontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
        //            volcontractcollection.InsertOne(volcontractsoffline[i]);
        //    }
        //    for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
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
        //        if (!(onlinevols.Contains(volunteersoffline[i].VolunteerID)))
        //            vollunteercollection.InsertOne(volunteersoffline[i]);
        //        if ((onlinevols.Contains(volunteersoffline[i].VolunteerID)))
        //            vollunteercollection.ReplaceOne(z => z.VolunteerID == volunteersoffline[i].VolunteerID, volunteersoffline[i]);
        //    }

        //    for (int i = 0; i < eventsoffline.Count(); i++)
        //    {
        //        if (!(onlineevents.Contains(eventsoffline[i].EventID)))
        //            eventcollection.InsertOne(eventsoffline[i]);
        //        if ((onlineevents.Contains(eventsoffline[i].EventID)))
        //            eventcollection.ReplaceOne(z => z.EventID == eventsoffline[i].EventID, eventsoffline[i]);
        //    }

        //    for (int i = 0; i < beneficiariesoffline.Count(); i++)
        //    {
        //        if (!(onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
        //            beneficiarycollection.InsertOne(beneficiariesoffline[i]);
        //        if ((onlinebenefieciaries.Contains(beneficiariesoffline[i].BeneficiaryID)))
        //            beneficiarycollection.ReplaceOne(z => z.BeneficiaryID == beneficiariesoffline[i].BeneficiaryID, beneficiariesoffline[i]);
        //    }

        //    for (int i = 0; i < sponsorsoffline.Count(); i++)
        //    {
        //        if (!(onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
        //            sponsorcollection.InsertOne(sponsorsoffline[i]);
        //        if ((onlinesponsoprs.Contains(sponsorsoffline[i].SponsorID)))
        //            sponsorcollection.ReplaceOne(z => z.SponsorID == sponsorsoffline[i].SponsorID, sponsorsoffline[i]);
        //    }

        //    for (int i = 0; i < volcontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
        //            volcontractcollection.InsertOne(volcontractsoffline[i]);
        //        if ((onlinevolcontrcarts.Contains(volcontractsoffline[i].ContractID)))
        //            volcontractcollection.ReplaceOne(z => z.ContractID == volcontractsoffline[i].ContractID, volcontractsoffline[i]);
        //    }
        //    for (int i = 0; i < beneficiarycontractsoffline.Count(); i++)
        //    {
        //        if (!(onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
        //            beneficiarycontractcollection.InsertOne(beneficiarycontractsoffline[i]);
        //        if ((onlinebeneficiarycontrcarts.Contains(beneficiarycontractsoffline[i].ContractID)))
        //            beneficiarycontractcollection.ReplaceOne(z => z.ContractID == beneficiarycontractsoffline[i].ContractID, beneficiarycontractsoffline[i]);
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