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
                
                if (dbcontext.english == true)
                {
                    TempData[VolMongoConstants.CONNECTION_LANGUAGE] = "en";
                }
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = dbcontext.numberofdocsperpage;
                List<Volcontract> volcontracts = volcontractcollection.AsQueryable<Volcontract>().ToList();
                List<Beneficiarycontract> beneficiarycontracts = beneficiarycontractcollection.AsQueryable<Beneficiarycontract>().ToList();
                List<Volunteer> volunteers = vollunteercollection.AsQueryable<Volunteer>().ToList();
                List<Sponsor> sponsors = sponsorcollection.AsQueryable<Sponsor>().ToList();

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