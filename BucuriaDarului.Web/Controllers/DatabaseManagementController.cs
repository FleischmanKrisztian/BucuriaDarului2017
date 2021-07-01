using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using Event = Finalaplication.Models.Event;
using ModifiedIDs = Finalaplication.Models.ModifiedIDs;
using Sponsor = Finalaplication.Models.Sponsor;
using Volunteer = Finalaplication.Models.Volunteer;

namespace Finalaplication.Controllers
{
    public class DatabaseManagementController : Controller
    {

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

        public ActionResult SynchronizationResults(int numberOfModifictaions, int numberOfDeletions, string outOfSyncDocuments = "")
        {
            try
            {
                ViewBag.outOfSyncDocuments = outOfSyncDocuments;
                ViewBag.numberOfModifications = numberOfModifictaions.ToString();
                ViewBag.numberOfDeletions = numberOfDeletions.ToString();
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Synchronize()
        {
            return View();
        }

        public ActionResult BackupManagerApp()
        {
            return Redirect("BackupManagerApp:asd");
        }

        public ActionResult SynchronizeData()
        {
            string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL);
            int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL));
            string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL);

            string SERVER_NAME_COMMON = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_COMMON);
            int SERVER_PORT_COMMON = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_COMMON));
            string DATABASE_NAME_COMMON = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_COMMON);

            ModifiedDocumentManager modifiedDocumentManager = new ModifiedDocumentManager();
            AuxiliaryDBManager AuxiliaryDBManager = new AuxiliaryDBManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

            EventManager eventManager = new EventManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            SponsorManager sponsorManager = new SponsorManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            VolunteerManager volunteerManager = new VolunteerManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            VolContractManager volContractManager = new VolContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            BeneficiaryManager beneManager = new BeneficiaryManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
            BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

            EventManager commonEventManager = new EventManager(SERVER_NAME_COMMON, SERVER_PORT_COMMON, DATABASE_NAME_COMMON);
            SponsorManager commonSponsorManager = new SponsorManager(SERVER_NAME_COMMON, SERVER_PORT_COMMON, DATABASE_NAME_COMMON);
            VolunteerManager commonvolunteerManager = new VolunteerManager(SERVER_NAME_COMMON, SERVER_PORT_COMMON, DATABASE_NAME_COMMON);
            VolContractManager commonVolContractManager = new VolContractManager(SERVER_NAME_COMMON, SERVER_PORT_COMMON, DATABASE_NAME_COMMON);
            BeneficiaryManager commonBeneficiaryManager = new BeneficiaryManager(SERVER_NAME_COMMON, SERVER_PORT_COMMON, DATABASE_NAME_COMMON);
            BeneficiaryContractManager commonBenefContractManager = new BeneficiaryContractManager(SERVER_NAME_COMMON, SERVER_PORT_COMMON, DATABASE_NAME_COMMON);

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
            List<DeletedIDS> deletedlist = modifiedDocumentManager.GetListOfDeletions();

            int numberOfModifictaions = modifiedidlist.Count();
            int numberOfDeletions = deletedlist.Count();

            string modifiedids = JsonConvert.SerializeObject(modifiedidlist);
            string deletedids = JsonConvert.SerializeObject(deletedlist);

            string commonvols = JsonConvert.SerializeObject(volunteers);
            string commonevents = JsonConvert.SerializeObject(events);
            string commonbenefieciaries = JsonConvert.SerializeObject(beneficiaries);
            string commonsponsors = JsonConvert.SerializeObject(sponsors);
            string commonvolcontracts = JsonConvert.SerializeObject(volcontracts);
            string commonbeneficiarycontracts = JsonConvert.SerializeObject(beneficiarycontracts);

            string outOfSyncDocuments = "";

            for (int i = 0; i < volunteerslocal.Count(); i++)
            {
                // If the common db does not contain the volunteer and it has been created since the last fetch/push it gets added.
                if (!commonvols.Contains(volunteerslocal[i]._id) && modifiedids.Contains(volunteerslocal[i]._id))
                    commonvolunteerManager.AddVolunteerToDB(volunteerslocal[i]);
                // if the common db contains the volunteer, but it has been edited since last sync it gets updated
                else if (modifiedids.Contains(volunteerslocal[i]._id))
                {
                    string auxiliaryDocument = AuxiliaryDBManager.GetDocumentByID(volunteerslocal[i]._id);
                    string currentDocument = JsonConvert.SerializeObject(commonvolunteerManager.GetOneVolunteer(volunteerslocal[i]._id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    //Checking whether or not the Document has been modified since we have last synced.
                    if (auxiliaryDocument != currentDocument)
                    {
                        outOfSyncDocuments += volunteerslocal[i].Fullname + ", ";
                    }
                    commonvolunteerManager.UpdateAVolunteer(volunteerslocal[i], volunteerslocal[i]._id);
                }
            }
            for (int i = 0; i < volunteers.Count(); i++)
            {
                // if the document has been deleted it will get deleted from the common db aswell.
                // the document will not be re-added unless someone has modified the document with this ID.
                if (deletedids.Contains(volunteers[i]._id))
                    commonvolunteerManager.DeleteAVolunteer(volunteers[i]._id);
            }

            for (int i = 0; i < eventslocal.Count(); i++)
            {
                if (!commonevents.Contains(eventslocal[i]._id) && modifiedids.Contains(eventslocal[i]._id))
                    commonEventManager.AddEventToDB(eventslocal[i]);
                else if (modifiedids.Contains(eventslocal[i]._id))
                {
                    string auxiliaryDocument = AuxiliaryDBManager.GetDocumentByID(eventslocal[i]._id);
                    string currentDocument = JsonConvert.SerializeObject(commonEventManager.GetOneEvent(eventslocal[i]._id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        outOfSyncDocuments += eventslocal[i].NameOfEvent + ", ";
                    }
                    commonEventManager.UpdateAnEvent(eventslocal[i], eventslocal[i]._id);
                }
            }
            for (int i = 0; i < events.Count(); i++)
            {
                if (deletedids.Contains(events[i]._id))
                    commonEventManager.DeleteAnEvent(events[i]._id);
            }

            for (int i = 0; i < beneficiarieslocal.Count(); i++)
            {
                if (!commonbenefieciaries.Contains(beneficiarieslocal[i].Id) && modifiedids.Contains(beneficiarieslocal[i].Id))
                    commonBeneficiaryManager.AddBeneficiaryToDB(beneficiarieslocal[i]);
                else if (modifiedids.Contains(beneficiarieslocal[i].Id))
                {
                    string auxiliaryDocument = AuxiliaryDBManager.GetDocumentByID(beneficiarieslocal[i].Id);
                    string currentDocument = JsonConvert.SerializeObject(commonBeneficiaryManager.GetOneBeneficiary(beneficiarieslocal[i].Id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        outOfSyncDocuments += beneficiarieslocal[i].Fullname + ", ";
                    }
                    commonBeneficiaryManager.UpdateABeneficiary(beneficiarieslocal[i], beneficiarieslocal[i].Id);
                }
            }
            for (int i = 0; i < beneficiaries.Count(); i++)
            {
                if (deletedids.Contains(beneficiaries[i].Id))
                    commonBeneficiaryManager.DeleteBeneficiary(beneficiaries[i].Id);
            }

            for (int i = 0; i < sponsorslocal.Count(); i++)
            {
                if (!commonsponsors.Contains(sponsorslocal[i]._id) && modifiedids.Contains(sponsorslocal[i]._id))
                    commonSponsorManager.AddSponsorToDB(sponsorslocal[i]);
                else if (modifiedids.Contains(sponsorslocal[i]._id))
                {
                    string auxiliaryDocument = AuxiliaryDBManager.GetDocumentByID(sponsorslocal[i]._id);
                    string currentDocument = JsonConvert.SerializeObject(commonSponsorManager.GetOneSponsor(sponsorslocal[i]._id));
                    auxiliaryDocument = auxiliaryDocument.Replace(" ", "");
                    currentDocument = currentDocument.Replace(" ", "");
                    if (auxiliaryDocument != currentDocument)
                    {
                        outOfSyncDocuments += sponsorslocal[i].NameOfSponsor + ", ";
                    }
                    commonSponsorManager.UpdateSponsor(sponsorslocal[i], sponsorslocal[i]._id);
                }
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

            volunteers = commonvolunteerManager.GetListOfVolunteers();
            events = commonEventManager.GetListOfEvents();
            beneficiaries = commonBeneficiaryManager.GetListOfBeneficiaries();
            sponsors = commonSponsorManager.GetListOfSponsors();
            volcontracts = commonVolContractManager.GetListOfVolunteersContracts();
            beneficiarycontracts = commonBenefContractManager.GetListOfBeneficiariesContracts();

            string volstring = JsonConvert.SerializeObject(volunteers);
            string eventstring = JsonConvert.SerializeObject(events);
            string benefieciarystring = JsonConvert.SerializeObject(beneficiaries);
            string sponsorstring = JsonConvert.SerializeObject(sponsors);
            string volcontractstring = JsonConvert.SerializeObject(volcontracts);
            string benefcontractstring = JsonConvert.SerializeObject(beneficiarycontracts);

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
            for (int i = 0; i < volunteerslocal.Count(); i++)
            {
                if (!volstring.Contains(volunteerslocal[i]._id))
                    volunteerManager.DeleteAVolunteer(volunteerslocal[i]._id);
            }

            for (int i = 0; i < events.Count(); i++)
            {
                if (!(localevents.Contains(events[i]._id)))
                    eventManager.AddEventToDB(events[i]);
                else if (!modifiedids.Contains(events[i]._id))
                    eventManager.UpdateAnEvent(events[i], events[i]._id);
            }
            for (int i = 0; i < eventslocal.Count(); i++)
            {
                if (!eventstring.Contains(eventslocal[i]._id))
                    eventManager.DeleteAnEvent(eventslocal[i]._id);
            }

            for (int i = 0; i < beneficiaries.Count(); i++)
            {
                if (!(localbenefieciaries.Contains(beneficiaries[i].Id)))
                    beneManager.AddBeneficiaryToDB(beneficiaries[i]);
                else if (!modifiedids.Contains(beneficiaries[i].Id))
                    beneManager.UpdateABeneficiary(beneficiaries[i], beneficiaries[i].Id);
            }
            for (int i = 0; i < beneficiarieslocal.Count(); i++)
            {
                if (!benefieciarystring.Contains(beneficiarieslocal[i].Id))
                    beneManager.DeleteBeneficiary(beneficiarieslocal[i].Id);
            }

            for (int i = 0; i < sponsors.Count(); i++)
            {
                if (!(localsponsors.Contains(sponsors[i]._id)))
                    sponsorManager.AddSponsorToDB(sponsors[i]);
                else if (!modifiedids.Contains(sponsors[i]._id))
                    sponsorManager.UpdateSponsor(sponsors[i], sponsors[i]._id);
            }
            for (int i = 0; i < sponsorslocal.Count(); i++)
            {
                if (!sponsorstring.Contains(sponsorslocal[i]._id))
                    sponsorManager.DeleteSponsor(sponsorslocal[i]._id);
            }

            for (int i = 0; i < volcontracts.Count(); i++)
            {
                if (!(localvolcontrcarts.Contains(volcontracts[i]._id)))
                    volContractManager.AddVolunteerContractToDB(volcontracts[i]);
                else if (!modifiedids.Contains(volcontracts[i]._id))
                    volContractManager.UpdateVolunteerContract(volcontracts[i], volcontracts[i]._id);
            }
            for (int i = 0; i < volcontractslocal.Count(); i++)
            {
                if (!volcontractstring.Contains(volcontractslocal[i]._id))
                    volContractManager.DeleteAVolContract(volcontractslocal[i]._id);
            }

            for (int i = 0; i < beneficiarycontracts.Count(); i++)
            {
                if (!(localbeneficiarycontrcarts.Contains(beneficiarycontracts[i]._id)))
                    beneficiaryContractManager.AddBeneficiaryContractToDB(beneficiarycontracts[i]);
                else if (!modifiedids.Contains(beneficiarycontracts[i]._id))
                    beneficiaryContractManager.UpdateBeneficiaryContract(beneficiarycontracts[i], beneficiarycontracts[i]._id);
            }
            for (int i = 0; i < beneficiarycontractslocal.Count(); i++)
            {
                if (!benefcontractstring.Contains(beneficiarycontractslocal[i]._id))
                    beneficiaryContractManager.DeleteBeneficiaryContract(beneficiarycontractslocal[i]._id);
            }

            modifiedDocumentManager.DeleteAuxiliaryDatabases();
            AuxiliaryDBManager.DropAuxiliaryDatabase();
            return RedirectToAction("SynchronizationResults", "DatabaseManagement", new { numberOfModifictaions, numberOfDeletions, outOfSyncDocuments });
        }
    }
}