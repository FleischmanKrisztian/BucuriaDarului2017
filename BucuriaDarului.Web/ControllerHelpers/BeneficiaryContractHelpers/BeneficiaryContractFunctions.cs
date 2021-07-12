using System;
using System.Collections.Generic;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using BucuriaDarului.Web.Models;

namespace BucuriaDarului.Web.ControllerHelpers.BeneficiaryContractHelpers
{
    public class BeneficiarycontractFunctions
    {
        internal static List<Beneficiarycontract> GetExpiringContracts(List<Beneficiarycontract> benefcontracts)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            List<Beneficiarycontract> returnlistofbencontracts = new List<Beneficiarycontract>();
            foreach (var benefcontract in benefcontracts)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(benefcontract.ExpirationDate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    returnlistofbencontracts.Add(benefcontract);
                }
            }
            return returnlistofbencontracts;
        }
    }
}