using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.DatabaseHandler;
using Finalaplication.DatabaseLocalManager;
using Finalaplication.DatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Finalaplication.Controllers
{
    public class HomeController : Controller
    {
        private MongoDBContextLocal dBContextLocal = new MongoDBContextLocal();

        private EventManager eventManager = new EventManager();
        private SponsorManager sponsorManager = new SponsorManager();
        private VolunteerManager volunteerManager = new VolunteerManager();
        private SettingsManager settingsManager = new SettingsManager();
        private VolContractManager volContractManager = new VolContractManager();
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager();

        public IActionResult Index()
        {
            try
            {
                List<Volcontract> volcontracts = volContractManager.GetListOfVolunteersContracts();
                List<Beneficiarycontract> beneficiarycontracts = beneficiaryContractManager.GetListOfBeneficiariesContracts();
                List<Volunteer> volunteers = volunteerManager.GetListOfVolunteers();
                List<Sponsor> sponsors = sponsorManager.GetListOfSponsors();
                Settings appsettings = settingsManager.GetSettingsItem();
                TempData[VolMongoConstants.CONNECTION_LANGUAGE] = appsettings.Lang;
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = appsettings.Quantity;

                int bd = 0;
                int vc = 0;
                int sc = 0;
                int bc = 0;

                int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
                foreach (var item in volunteers)
                {
                    int volbdday = UniversalFunctions.GetDayOfYear(item.Birthdate);
                    if (UniversalFunctions.IsAboutToExpire(currentday, volbdday))
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
                    int daytocompare = UniversalFunctions.GetDayOfYear(item.ExpirationDate);
                    if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                    {
                        vc++;
                    }
                }
                ViewBag.nrofvc = vc;

                foreach (var item in sponsors)
                {
                    int daytocompare = UniversalFunctions.GetDayOfYear(item.Contract.ExpirationDate);
                    if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                    {
                        sc++;
                    }
                }
                ViewBag.nrofsc = sc;

                foreach (var item in beneficiarycontracts)
                {
                    int daytocompare = UniversalFunctions.GetDayOfYear(item.ExpirationDate);
                    if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                    {
                        bc++;
                    }
                }
                ViewBag.nrofbc = bc;

                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
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
            try
            {
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult ImportUpdate(string docsimported)
        {
            ViewBag.documentsimported = docsimported;
            return View();
        }

        public IActionResult About()
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
    }
}