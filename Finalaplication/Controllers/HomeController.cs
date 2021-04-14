using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.DatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Finalaplication.Controllers
{
    public class HomeController : Controller
    {
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

                ViewBag.nrofbds = UniversalFunctions.GetNumberOfVolunteersWithBirthdays(volunteers);
                ViewBag.nrofvc = UniversalFunctions.GetNumberOfExpiringVolContracts(volcontracts);
                ViewBag.nrofsc = UniversalFunctions.GetNumberOfExpiringSponsorContracts(sponsors);
                ViewBag.nrofbc = UniversalFunctions.GetNumberOfExpiringBeneficiaryContracts(beneficiarycontracts);

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