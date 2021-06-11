using Finalaplication.ControllerHelpers.BeneficiaryContractHelpers;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.LocalDatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class BeneficiarycontractController : Controller
    {
        private static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_NAME_LOCAL);
        private static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Common.VolMongoConstants.SERVER_PORT_LOCAL));
        private static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Common.VolMongoConstants.DATABASE_NAME_LOCAL);

        private BeneficiaryManager beneficiaryManager = new BeneficiaryManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager(SERVER_NAME_LOCAL, SERVER_PORT_LOCAL, DATABASE_NAME_LOCAL);

        [HttpGet]
        public IActionResult Index(string idofbeneficiary)
        {
            try
            {
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                List<Beneficiarycontract> benficiarycontracts = beneficiaryContractManager.GetListOfBeneficiariesContracts();
                Beneficiary benenficiary = beneficiaryManager.GetOneBeneficiary(idofbeneficiary);
                benficiarycontracts = benficiarycontracts.Where(z => z.OwnerID.ToString() == idofbeneficiary).ToList();
                ViewBag.nameofbeneficiary = benenficiary.Fullname;
                ViewBag.idofbeneficiary = idofbeneficiary;
                return View(benficiarycontracts);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult ContractExp()
        {
            try
            {
                List<Beneficiarycontract> benefcontracts = beneficiaryContractManager.GetListOfBeneficiariesContracts();
                benefcontracts = BeneficiarycontractFunctions.GetExpiringContracts(benefcontracts);
                return View(benefcontracts);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            try
            {
                ViewBag.idofbeneficiary = id;
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Create(Beneficiarycontract benenficiarycontract, string idofbeneficiary)
        {
            try
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Beneficiary beneficiary = beneficiaryManager.GetOneBeneficiary(idofbeneficiary);
                        benenficiarycontract._id = Guid.NewGuid().ToString();
                        benenficiarycontract.ExpirationDate = benenficiarycontract.ExpirationDate.AddDays(1);
                        benenficiarycontract.RegistrationDate = benenficiarycontract.RegistrationDate.AddDays(1);
                        benenficiarycontract.Birthdate = beneficiary.PersonalInfo.Birthdate;
                        benenficiarycontract.Fullname = beneficiary.Fullname;
                        benenficiarycontract.CNP = beneficiary.CNP;
                        benenficiarycontract.CIinfo = beneficiary.CI.CIinfo;
                        benenficiarycontract.Nrtel = beneficiary.PersonalInfo.PhoneNumber;
                        benenficiarycontract.NumberOfPortion = beneficiary.NumberOfPortions.ToString();
                        benenficiarycontract.IdApplication = beneficiary.Marca.IdAplication;
                        benenficiarycontract.IdInvestigation = beneficiary.Marca.IdInvestigation;
                        benenficiarycontract.Address = beneficiary.Adress;
                        benenficiarycontract.OwnerID = idofbeneficiary;
                        beneficiaryContractManager.AddBeneficiaryContractToDB(benenficiarycontract);
                        return RedirectToAction("Index", new { idofbeneficiary });
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes! ");
                }
                return View(benenficiarycontract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult Print(string id)
        {
            try
            {
                var contract = beneficiaryContractManager.GetBeneficiaryContract(id);

                return View(contract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            try
            {
                var contract = beneficiaryContractManager.GetBeneficiaryContract(id);
                return View(contract);
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Delete(string id, string idofbeneficiary)
        {
            try
            {
                beneficiaryContractManager.DeleteBeneficiaryContract(id);
                return RedirectToAction("Index", new { idofbeneficiary });
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}