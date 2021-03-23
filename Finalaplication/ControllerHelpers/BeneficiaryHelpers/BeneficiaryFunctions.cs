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

        internal static string GetIdAndFieldString(string IDS,bool PhoneNumber, bool SpouseName, bool Gender, bool Expences, bool Income, bool HousingType, bool HasHome, bool Married, bool HealthCard, bool HealthInsurance, bool Addictions, bool ChronicCondition, bool Disalility, bool HealthState, bool Profesion, bool SeniorityInWorkField, bool Ocupation, bool BirthPlace, bool Studies, bool CI_Info, bool IdContract, bool IdInvestigation, bool IdAplication, bool marca, bool All, bool CNP, bool Fullname, bool Active, bool Canteen, bool HomeDelivery, bool HomeDeliveryDriver, bool HasGDPRAgreement, bool Adress, bool NumberOfPortions, bool LastTimeActiv, bool WeeklyPackage)
        { 
            string ids_and_options = IDS + "(((";
            if (All == true)
                ids_and_options = ids_and_options + "0";
            if (Fullname == true)
                ids_and_options = ids_and_options + "1";
            if (Active == true)
                ids_and_options = ids_and_options + "2";
            if (Canteen == true)
                ids_and_options = ids_and_options + "3";
            if (HomeDelivery == true)
                ids_and_options = ids_and_options + "4";
            if (HomeDeliveryDriver == true)
                ids_and_options = ids_and_options + "5";
            if (HasGDPRAgreement == true)
                ids_and_options = ids_and_options + "6";
            if (Adress == true)
                ids_and_options = ids_and_options + "7";
            if (CNP == true)
                ids_and_options = ids_and_options + "8";
            if (CI_Info == true)
                ids_and_options = ids_and_options + "9";
            if (marca == true)
                ids_and_options = ids_and_options + "A";
            if (IdInvestigation == true)
                ids_and_options = ids_and_options + "B";
            if (IdAplication == true)
                ids_and_options = ids_and_options + "C";
            if (NumberOfPortions == true)
                ids_and_options = ids_and_options + "D";
            if (LastTimeActiv == true)
                ids_and_options = ids_and_options + "E";
            if (PhoneNumber == true)
                ids_and_options = ids_and_options + "F";
            if (BirthPlace == true)
                ids_and_options = ids_and_options + "G";
            if (Studies == true)
                ids_and_options = ids_and_options + "H";
            if (Profesion == true)
                ids_and_options = ids_and_options + "I";
            if (Ocupation == true)
                ids_and_options = ids_and_options + "J";
            if (SeniorityInWorkField == true)
                ids_and_options = ids_and_options + "K";
            if (HealthState == true)
                ids_and_options = ids_and_options + "L";
            if (Disalility == true)
                ids_and_options = ids_and_options + "M";
            if (ChronicCondition == true)
                ids_and_options = ids_and_options + "N";
            if (Addictions == true)
                ids_and_options = ids_and_options + "O";
            if (HealthInsurance == true)
                ids_and_options = ids_and_options + "Z";
            if (HealthCard == true)
                ids_and_options = ids_and_options + "P";
            if (Married == true)
                ids_and_options = ids_and_options + "Q";
            if (SpouseName == true)
                ids_and_options = ids_and_options + "R";
            if (HasHome == true)
                ids_and_options = ids_and_options + "S";
            if (HousingType == true)
                ids_and_options = ids_and_options + "T";
            if (Income == true)
                ids_and_options = ids_and_options + "U";
            if (Expences == true)
                ids_and_options = ids_and_options + "V";
            if (Gender == true)
                ids_and_options = ids_and_options + "W";
            if (WeeklyPackage == true)
                ids_and_options = ids_and_options + "Z";
            return ids_and_options;
        }
    }
}

