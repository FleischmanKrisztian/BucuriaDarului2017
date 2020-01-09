using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
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

        public HomeController()
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
        }

        public IActionResult Index()
        {
            try
            {
                if(dbcontext.nointernet==true)
                {
                    TempData[VolMongoConstants.CONNECTION_ENVIRONMENT] = VolMongoConstants.CONNECTION_MODE_OFFLINE;
                }
                if(dbcontext.english==true)
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
                    var Day = Finalaplication.Models.Volunteer.Nowdate();
                    var voldays = Finalaplication.Models.Volunteer.Volbd(item);
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
                    if (item.GetDayExpiration(item.ExpirationDate) == true)
                    {
                        vc++;
                    }
                }
                ViewBag.nrofvc = vc;

                foreach (var item in sponsors)
                {
                    if (item.GetDayExpiration(item.Contract.ExpirationDate) == true)
                    {
                        sc++;
                    }
                }
                ViewBag.nrofsc = sc;

                foreach (var item in beneficiarycontracts)
                {
                    if (item.GetDayExpiration(item.ExpirationDate) == true)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
