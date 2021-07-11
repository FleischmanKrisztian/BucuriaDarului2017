using BucuriaDarului.Core;
using BucuriaDarului.Gateway.VolContractGateways;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.DatabaseManager;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Sponsor = Finalaplication.Models.Sponsor;

namespace Finalaplication.Controllers
{
    public class HomeController : Controller
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.Constants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.Constants.DATABASE_NAME_LOCAL);

        private SponsorManager sponsorManager = new SponsorManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private VolunteerManager volunteerManager = new VolunteerManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private SettingsManager settingsManager = new SettingsManager();
       
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        public IActionResult Index()
        {
                List<VolunteerContract> volunteerContracts = ListVolunteerContractGateway.GetListVolunteerContracts();
                List<Beneficiarycontract> beneficiarycontracts = beneficiaryContractManager.GetListOfBeneficiariesContracts();
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                List<Sponsor> sponsors = sponsorManager.GetListOfSponsors();
                Settings appsettings = settingsManager.GetSettingsItem();
                TempData[Constants.CONNECTION_LANGUAGE] = appsettings.Lang;
                TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = appsettings.Quantity;

                ViewBag.nrofbds = UniversalFunctions.GetNumberOfVolunteersWithBirthdays(volunteers);
                ViewBag.nrofvc = UniversalFunctions.GetNumberOfExpiringVolContracts(volunteerContracts);
                ViewBag.nrofsc = UniversalFunctions.GetNumberOfExpiringSponsorContracts(sponsors);
                ViewBag.nrofbc = UniversalFunctions.GetNumberOfExpiringBeneficiaryContracts(beneficiarycontracts);

                return View();
           
        }

        public IActionResult Localserver()
        {
            return View();
        }

        public IActionResult IncorrectFile()
        {
            return View();
        }

        public IActionResult ExportFailed()
        {
            return View();
        }

        public IActionResult Contact()
        {
           
                return View();
            
        }

        public ActionResult ImportUpdate(string docsimported)
        {
            ViewBag.documentsimported = docsimported;
            return View();
        }

        public IActionResult About()
        {
           
                return View();
            
        }
    }
}