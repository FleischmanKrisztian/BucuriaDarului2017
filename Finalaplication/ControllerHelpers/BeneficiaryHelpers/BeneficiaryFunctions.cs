using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.ControllerHelpers.BeneficiaryHelpers
{
    public class BeneficiaryFunctions
    {

        internal static string GetStringOfIds(List<Beneficiary> beneficiaries)
        {
            string stringofids = "beneficiary";
            foreach (Beneficiary beneficiary in beneficiaries)
            {
                stringofids = stringofids + "," + beneficiary.BeneficiaryID;
            }
            return stringofids;
        }

        internal static List<Beneficiary> GetBeneficiariesAfterPaging(List<Beneficiary> beneficiaries, int page, int nrofdocs)
        {
            beneficiaries = beneficiaries.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            beneficiaries = beneficiaries.AsQueryable().Take(nrofdocs).ToList();
            return beneficiaries;
        }

        internal static List<Beneficiary> GetBeneficiariesAfterSearching(List<Beneficiary> beneficiaries, string searching)
        {
            if (searching != null)
            {
                beneficiaries = beneficiaries.Where(x => x.Fullname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return beneficiaries;
        }

        internal static List<Beneficiary> GetBeneficiariesByIds(List<Beneficiary> beneficiaries, string[] beneficiaryids)
        {
            List<Beneficiary> beneficiarylist = new List<Beneficiary>();
            for (int i = 0; i < beneficiaryids.Length; i++)
            {
                Beneficiary singlebeneficiary = beneficiaries.Where(x => x.BeneficiaryID == beneficiaryids[i]).First();
                beneficiarylist.Add(singlebeneficiary);
            }
            return beneficiarylist;
        }

        internal static string GetBeneficiaryNames(List<Beneficiary> beneficiaries)
        {
            string beneficiariesnames = "";
            for (int i = 0; i < beneficiaries.Count; i++)
            {
                var beneficiary= beneficiaries[i];
                beneficiariesnames = beneficiariesnames + beneficiary.Fullname + " / ";
            }
            return beneficiariesnames;
        }

        
    }
}

