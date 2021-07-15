using BucuriaDarului.Contexts.HomeControllerContexts;
using BucuriaDarului.Gateway.HomeController;
using BucuriaDarului.Web.Common;
using BucuriaDarului.Web.DatabaseManager;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BucuriaDarului.Web.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            var context = new HomeControllerIndexDisplayContext(new HomeControllerIndexDisplayGateway());
            var response = context.Execute();
            TempData[Constants.CONNECTION_LANGUAGE] = response.Settings.Lang;
            TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = response.Settings.Quantity;

            ViewBag.NumberOfBirtdays = response.BirthdayOfVolunteersNumber;
            ViewBag.NumberOfVolunteerContracts = response.VolunteerContractExpirationNumber;
            ViewBag.NumberOfSponsorContracts = response.SponsorContractExpirationNumber;
            ViewBag.NumberOfBeneficiaryContracts = response.BeneficiaryContractExpirationNumber;

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

        public ActionResult ImportUpdate(string docsImported)
        {
            ViewBag.documentsimported = docsImported;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}