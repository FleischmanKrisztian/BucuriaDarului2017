using Finalaplication.CommonDatabaseManager;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class DatabaseManagementController : Controller
    {
        private EventManager eventManager = new EventManager();
        private ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();
        private SponsorManager sponsorManager = new SponsorManager();
        private VolunteerManager volunteerManager = new VolunteerManager();
        private VolContractManager volContractManager = new VolContractManager();
        private BeneficiaryManager beneManager = new BeneficiaryManager();
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager();

        private EventManagerCommon commonEventManager = new EventManagerCommon();
        private SponsorManagerCommon commonSponsorManager = new SponsorManagerCommon();
        private VolunteerManagerCommon commonvolunteerManager = new VolunteerManagerCommon();
        private VolunteerContractManagerCommon commonVolContractManager = new VolunteerContractManagerCommon();
        private BeneficiaryManagerCommon commonBeneficiaryManager = new BeneficiaryManagerCommon();
        private BeneficiaryContractManagerCommon commonBenefContractManager = new BeneficiaryContractManagerCommon();

        public DatabaseManagementController()
        {
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

        public ActionResult Push()
        {
            return View();
        }

        public ActionResult Fetch()
        {
            return View();
        }

        public ActionResult PushData()
        {
            List<Volunteer> volunteerslocal = volunteerManager.GetListOfVolunteers();
            List<Event> eventslocal = eventManager.GetListOfEvents();
            List<Beneficiary> beneficiarieslocal = beneManager.GetListOfBeneficiaries();
            List<Sponsor> sponsorslocal = sponsorManager.GetListOfSponsors();
            List<Volcontract> volcontractslocal = volContractManager.GetListOfVolunteersContracts();
            List<Beneficiarycontract> beneficiarycontractslocal = beneficiaryContractManager.GetListOfBeneficiariesContracts();
            List<Volunteer> volunteers = commonvolunteerManager.GetListOfVolunteers();
            List<Event> events = commonEventManager.GetListOfEvents();
            List<Beneficiary> beneficiaries = commonBeneficiaryManager.GetListOfBeneficiaries();
            List<Sponsor> sponsors = commonSponsorManager.GetListOfSponsors();
            List<Volcontract> volcontracts = commonVolContractManager.GetListOfVolunteersContracts();
            List<Beneficiarycontract> beneficiarycontracts = commonBenefContractManager.GetListOfBeneficiariesContracts();

            List<ModifiedIDs> modifiedidlist = modifiedDocumentManager.GetListOfModifications();
            string modifiedids = JsonConvert.SerializeObject(modifiedidlist);
            List<DeletedIDS> deletedlist = modifiedDocumentManager.GetListOfDeletions();
            string deletedids = JsonConvert.SerializeObject(deletedlist);

            string commonvols = JsonConvert.SerializeObject(volunteers);
            string commonevents = JsonConvert.SerializeObject(events);
            string commonbenefieciaries = JsonConvert.SerializeObject(beneficiaries);
            string commonsponsors = JsonConvert.SerializeObject(sponsors);
            string commonvolcontracts = JsonConvert.SerializeObject(volcontracts);
            string commonbeneficiarycontracts = JsonConvert.SerializeObject(beneficiarycontracts);

            for (int i = 0; i < volunteerslocal.Count(); i++)
            {
                if (!commonvols.Contains(volunteerslocal[i]._id) && modifiedids.Contains(volunteerslocal[i]._id))
                    commonvolunteerManager.AddVolunteerToDB(volunteerslocal[i]);
                else if (modifiedids.Contains(volunteerslocal[i]._id))
                    commonvolunteerManager.UpdateAVolunteer(volunteerslocal[i], volunteerslocal[i]._id);
            }
            for (int i = 0; i < volunteers.Count(); i++)
            {
                if (deletedids.Contains(volunteers[i]._id))
                    commonvolunteerManager.DeleteAVolunteer(volunteers[i]._id);
            }

            for (int i = 0; i < eventslocal.Count(); i++)
            {
                if (!commonevents.Contains(eventslocal[i]._id) && modifiedids.Contains(eventslocal[i]._id))
                    commonEventManager.AddEventToDB(eventslocal[i]);
                else if (modifiedids.Contains(eventslocal[i]._id))
                    commonEventManager.UpdateAnEvent(eventslocal[i], eventslocal[i]._id);
            }
            for (int i = 0; i < events.Count(); i++)
            {
                if (deletedids.Contains(events[i]._id))
                    commonEventManager.DeleteAnEvent(events[i]._id);
            }

            for (int i = 0; i < beneficiarieslocal.Count(); i++)
            {
                if (!commonbenefieciaries.Contains(beneficiarieslocal[i]._id) && modifiedids.Contains(beneficiarieslocal[i]._id))
                    commonBeneficiaryManager.AddBeneficiaryToDB(beneficiarieslocal[i]);
                else if (modifiedids.Contains(beneficiarieslocal[i]._id))
                    commonBeneficiaryManager.UpdateABeneficiary(beneficiarieslocal[i], beneficiarieslocal[i]._id);
            }
            for (int i = 0; i < beneficiaries.Count(); i++)
            {
                if (deletedids.Contains(beneficiaries[i]._id))
                    commonBeneficiaryManager.DeleteBeneficiary(beneficiaries[i]._id);
            }

            for (int i = 0; i < sponsorslocal.Count(); i++)
            {
                if (!commonsponsors.Contains(sponsorslocal[i]._id) && modifiedids.Contains(sponsorslocal[i]._id))
                    commonSponsorManager.AddSponsorToDB(sponsorslocal[i]);
                else if (modifiedids.Contains(sponsorslocal[i]._id))
                    commonSponsorManager.UpdateSponsor(sponsorslocal[i], sponsorslocal[i]._id);
            }
            for (int i = 0; i < sponsors.Count(); i++)
            {
                if (deletedids.Contains(sponsors[i]._id))
                    commonSponsorManager.DeleteSponsor(sponsors[i]._id);
            }

            for (int i = 0; i < volcontractslocal.Count(); i++)
            {
                if (!commonvolcontracts.Contains(volcontractslocal[i]._id) && modifiedids.Contains(volcontractslocal[i]._id))
                    commonVolContractManager.AddVolunteerContractToDB(volcontractslocal[i]);
                else if (modifiedids.Contains(volcontractslocal[i]._id))
                    commonVolContractManager.UpdateVolunteerContract(volcontractslocal[i], volcontractslocal[i]._id);
            }
            for (int i = 0; i < volcontracts.Count(); i++)
            {
                if (deletedids.Contains(volcontracts[i]._id))
                    commonVolContractManager.DeleteAVolContract(volcontracts[i]._id);
            }
            for (int i = 0; i < beneficiarycontractslocal.Count(); i++)
            {
                if (!commonbeneficiarycontracts.Contains(beneficiarycontractslocal[i]._id) && modifiedids.Contains(beneficiarycontractslocal[i]._id))
                    commonBenefContractManager.AddBeneficiaryContractToDB(beneficiarycontractslocal[i]);
                else if (modifiedids.Contains(beneficiarycontractslocal[i]._id))
                    commonBenefContractManager.UpdateBeneficiaryContract(beneficiarycontractslocal[i], beneficiarycontractslocal[i]._id);
            }
            for (int i = 0; i < beneficiarycontracts.Count(); i++)
            {
                if (deletedids.Contains(beneficiarycontracts[i]._id))
                    commonBenefContractManager.DeleteBeneficiaryContract(beneficiarycontracts[i]._id);
            }
            modifiedDocumentManager.DeleteAuxiliaryDatabases();

            return RedirectToAction("Servermanagement");
        }

        public ActionResult FetchData()
        {
            List<Volunteer> volunteerslocal = volunteerManager.GetListOfVolunteers();
            List<Event> eventslocal = eventManager.GetListOfEvents();
            List<Beneficiary> beneficiarieslocal = beneManager.GetListOfBeneficiaries();
            List<Sponsor> sponsorslocal = sponsorManager.GetListOfSponsors();
            List<Volcontract> volcontractslocal = volContractManager.GetListOfVolunteersContracts();
            List<Beneficiarycontract> beneficiarycontractslocal = beneficiaryContractManager.GetListOfBeneficiariesContracts();

            List<Volunteer> volunteers = commonvolunteerManager.GetListOfVolunteers();
            List<Event> events = commonEventManager.GetListOfEvents();
            List<Beneficiary> beneficiaries = commonBeneficiaryManager.GetListOfBeneficiaries();
            List<Sponsor> sponsors = commonSponsorManager.GetListOfSponsors();
            List<Volcontract> volcontracts = commonVolContractManager.GetListOfVolunteersContracts();
            List<Beneficiarycontract> beneficiarycontracts = commonBenefContractManager.GetListOfBeneficiariesContracts();

            List<ModifiedIDs> modifiedidlist = modifiedDocumentManager.GetListOfModifications();
            string modifiedids = JsonConvert.SerializeObject(modifiedidlist);

            string localvols = JsonConvert.SerializeObject(volunteerslocal);
            string localevents = JsonConvert.SerializeObject(eventslocal);
            string localbenefieciaries = JsonConvert.SerializeObject(beneficiarieslocal);
            string localsponsors = JsonConvert.SerializeObject(sponsorslocal);
            string localvolcontrcarts = JsonConvert.SerializeObject(volcontractslocal);
            string localbeneficiarycontrcarts = JsonConvert.SerializeObject(beneficiarycontractslocal);

            for (int i = 0; i < volunteers.Count(); i++)
            {
                if (!(localvols.Contains(volunteers[i]._id)))
                    volunteerManager.AddVolunteerToDB(volunteers[i]);
                else if (!modifiedids.Contains(volunteers[i]._id))
                    volunteerManager.UpdateAVolunteer(volunteers[i], volunteers[i]._id);
            }

            for (int i = 0; i < events.Count(); i++)
            {
                if (!(localevents.Contains(events[i]._id)))
                    eventManager.AddEventToDB(events[i]);
                else if (!modifiedids.Contains(events[i]._id))
                    eventManager.UpdateAnEvent(events[i], events[i]._id);
            }

            for (int i = 0; i < beneficiaries.Count(); i++)
            {
                if (!(localbenefieciaries.Contains(beneficiaries[i]._id)))
                    beneManager.AddBeneficiaryToDB(beneficiaries[i]);
                else if (!modifiedids.Contains(beneficiaries[i]._id))
                    beneManager.UpdateABeneficiary(beneficiaries[i], beneficiaries[i]._id);
            }

            for (int i = 0; i < sponsors.Count(); i++)
            {
                if (!(localsponsors.Contains(sponsors[i]._id)))
                    sponsorManager.AddSponsorToDB(sponsors[i]);
                else if (!modifiedids.Contains(sponsors[i]._id))
                    sponsorManager.UpdateSponsor(sponsors[i], sponsors[i]._id);
            }

            for (int i = 0; i < volcontracts.Count(); i++)
            {
                if (!(localvolcontrcarts.Contains(volcontracts[i]._id)))
                    volContractManager.AddVolunteerContractToDB(volcontracts[i]);
                else if (!modifiedids.Contains(volcontracts[i]._id))
                    volContractManager.UpdateVolunteerContract(volcontracts[i], volcontracts[i]._id);
            }
            for (int i = 0; i < beneficiarycontracts.Count(); i++)
            {
                if (!(localbeneficiarycontrcarts.Contains(beneficiarycontracts[i]._id)))
                    beneficiaryContractManager.AddBeneficiaryContractToDB(beneficiarycontracts[i]);
                else if (!modifiedids.Contains(beneficiarycontracts[i]._id))
                    beneficiaryContractManager.UpdateBeneficiaryContract(beneficiarycontracts[i], beneficiarycontracts[i]._id);
            }
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