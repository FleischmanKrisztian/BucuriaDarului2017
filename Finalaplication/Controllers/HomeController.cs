using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.ControllerHelpers.VolunteerHelpers;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class HomeController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<Volunteer> vollunteercollection;
        private IMongoCollection<Sponsor> sponsorcollection;
        private IMongoCollection<Volcontract> volcontractcollection;
        private IMongoCollection<Beneficiarycontract> beneficiarycontractcollection;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            dbcontext = new MongoDBContext();

            try
            {
                vollunteercollection = dbcontext.database.GetCollection<Volunteer>("Volunteers");
                sponsorcollection = dbcontext.database.GetCollection<Sponsor>("Sponsors");
                volcontractcollection = dbcontext.database.GetCollection<Volcontract>("Contracts");
                beneficiarycontractcollection = dbcontext.database.GetCollection<Beneficiarycontract>("BeneficiariesContracts");
            }
            catch (Exception)
            {
            }
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            try
            {
                if (dbcontext.nointernet == true)
                {
                    TempData[VolMongoConstants.CONNECTION_ENVIRONMENT] = VolMongoConstants.CONNECTION_MODE_OFFLINE;
                }
                if (dbcontext.english == true)
                {
                    TempData[VolMongoConstants.CONNECTION_LANGUAGE] = "en";
                }
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = dbcontext.numberofdocsperpage;
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
                List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();

                int bd = 0;
                int vc = 0;
                int sc = 0;
                int bc = 0;

                foreach (var item in volunteers)
                {
                    var Day = UniversalFunctions.GetCurrentDate();
                    var voldays = VolunteerFunctions.GetVolBirthdate(item);
                    if ((Day <= voldays && Day + 10 > voldays) || (Day > 354 && 365 - (Day + 365 - Day - 2) >= voldays))
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
                    if (UniversalFunctions.GetDayExpiration(item.ExpirationDate))
                    {
                        vc++;
                    }
                }
                ViewBag.nrofvc = vc;

                foreach (var item in sponsors)
                {
                    if (UniversalFunctions.GetDayExpiration(item.Contract.ExpirationDate))
                    {
                        sc++;
                    }
                }
                ViewBag.nrofsc = sc;

                foreach (var item in beneficiarycontracts)
                {
                    if (UniversalFunctions.DateExpiryChecker(item.ExpirationDate))
                    {
                        bc++;
                    }
                }
                ViewBag.nrofbc = bc;

                return View();
            }
            catch
            {
                return RedirectToAction("Changeenvironment", "Settings");
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
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }


        public ActionResult ImportUpdate(string docsimported)
        {
            ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
            ViewBag.documentsimported = docsimported;
            return View();
        }

        public IActionResult About()
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
    }
}