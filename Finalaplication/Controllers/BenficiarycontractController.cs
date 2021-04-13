using Finalaplication.Common;
using Finalaplication.ControllerHelpers.BeneficiaryContractHelpers;
using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.DatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.Controllers
{
    public class BeneficiarycontractController : Controller
    {
        private BeneficiaryManager beneficiaryManager = new BeneficiaryManager();
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager();
        private readonly IStringLocalizer<BeneficiarycontractController> _localizer;

        public BeneficiarycontractController(IStringLocalizer<BeneficiarycontractController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string idofbeneficiary)
        {
            try
            {
                int nrofdocs = UniversalFunctions.GetNumberOfItemPerPageFromSettings(TempData);
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
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
                var contractid = new ObjectId(id);
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