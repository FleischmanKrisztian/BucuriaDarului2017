using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VolCommon;

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
                var beneficiary = beneficiaries[i];
                beneficiariesnames = beneficiariesnames + beneficiary.Fullname + " / ";
            }
            return beneficiariesnames;
        }

        internal static string GetIdAndFieldString(string IDS, bool PhoneNumber, bool SpouseName, bool Gender, bool Expences, bool Income, bool HousingType, bool HasHome, bool Married, bool HealthCard, bool HealthInsurance, bool Addictions, bool ChronicCondition, bool Disalility, bool HealthState, bool Profesion, bool SeniorityInWorkField, bool Ocupation, bool BirthPlace, bool Studies, bool CI_Info, bool IdContract, bool IdInvestigation, bool IdAplication, bool marca, bool All, bool CNP, bool Fullname, bool Active, bool Canteen, bool HomeDelivery, bool HomeDeliveryDriver, bool HasGDPRAgreement, bool Adress, bool NumberOfPortions, bool LastTimeActiv, bool WeeklyPackage)
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

        private static bool Dateinputreceived(DateTime date)
        {
            DateTime comparisondate = new DateTime(0003, 1, 1);
            if (date > comparisondate)
                return true;
            else
                return false;
        }

        internal static List<Beneficiary> GetBeneficiariesAfterFilters(List<Beneficiary> beneficiaries, string sortOrder, string searching, bool Active, string searchingBirthPlace, bool HasContract, bool Homeless, DateTime lowerdate, DateTime upperdate, DateTime activesince, DateTime activetill, int page, bool Weeklypackage, bool Canteen, bool HomeDelivery, string searchingDriver, bool HasGDPRAgreement, string searchingAddress, bool HasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpences, string gender)
        {
            DateTime d1 = new DateTime(0003, 1, 1);
            if (Dateinputreceived(upperdate))
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Birthdate <= upperdate).ToList();
            }

            if (searching != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.Fullname == null || b.Fullname == "")
                    {
                        b.Fullname = "-";
                    }
                }
                try { beneficiaries = beneficiaries.Where(x => x.Fullname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList(); } catch { }
            }

            if (Homeless == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HasHome == false).ToList();
            }

            if (Dateinputreceived(lowerdate))
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Birthdate > lowerdate).ToList();
            }

            if (Active == true)
            {
                beneficiaries = beneficiaries.Where(x => x.Active == true).ToList();
            }

            if (Weeklypackage == true)
            {
                beneficiaries = beneficiaries.Where(x => x.Weeklypackage == true).ToList();
            }

            if (Canteen == true)
            {
                beneficiaries = beneficiaries.Where(x => x.Canteen == true).ToList();
            }

            if (HomeDelivery == true)
            {
                beneficiaries = beneficiaries.Where(x => x.HomeDelivery == true).ToList();
            }

            if (HasGDPRAgreement == true)
            {
                beneficiaries = beneficiaries.Where(x => x.HasGDPRAgreement == true).ToList();
            }

            if (HasID == true)
            {
                beneficiaries = beneficiaries.Where(x => x.CI.HasId == true).ToList();
            }

            if (searchingHealthInsurance == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthInsurance == true).ToList();
            }

            if (searchingHealthCard == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthCard == true).ToList();
            }

            if (searchingDriver != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.HomeDeliveryDriver == null || b.HomeDeliveryDriver == "")
                        b.HomeDeliveryDriver = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.HomeDeliveryDriver.Contains(searchingDriver, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingAddress != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.Adress == null || b.Adress == "")
                        b.Adress = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.Adress.Contains(searchingAddress, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingPO != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.Ocupation == null || b.PersonalInfo.Ocupation == "")
                        b.PersonalInfo.Ocupation = "-";
                    if (b.PersonalInfo.Profesion == null || b.PersonalInfo.Profesion == "")
                        b.PersonalInfo.Profesion = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Ocupation.Contains(searchingPO, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.Profesion.Contains(searchingPO, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingNumberOfPortions != 0)
            {
                beneficiaries = beneficiaries.Where(x => x.NumberOfPortions.Equals(searchingNumberOfPortions)).ToList();
            }

            if (searchingComments != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.Comments == null || b.Comments == "")
                        b.Comments = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.Comments.Contains(searchingComments, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingBirthPlace != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.BirthPlace == null || b.PersonalInfo.BirthPlace == "")
                        b.PersonalInfo.BirthPlace = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(searchingBirthPlace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingStudies != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.Studies == null || b.PersonalInfo.Studies == "")
                        b.PersonalInfo.Studies = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Studies.Contains(searchingStudies, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingSeniority != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.SeniorityInWorkField == null || b.PersonalInfo.SeniorityInWorkField == "")
                        b.PersonalInfo.SeniorityInWorkField = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.SeniorityInWorkField.Contains(searchingSeniority, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingHealthState != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.HealthState == null || b.PersonalInfo.HealthState == "")
                        b.PersonalInfo.HealthState = "-";
                    if (b.PersonalInfo.Disalility == null || b.PersonalInfo.Disalility == "")
                        b.PersonalInfo.Disalility = "-";
                    if (b.PersonalInfo.ChronicCondition == null || b.PersonalInfo.ChronicCondition == "")
                        b.PersonalInfo.ChronicCondition = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthState.Contains(searchingHealthState, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.Disalility.Contains(searchingHealthState, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.ChronicCondition.Contains(searchingHealthState, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingAddictions != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.Addictions == null || b.PersonalInfo.Addictions == "")
                        b.PersonalInfo.Addictions = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Addictions.Contains(searchingAddictions, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingMarried != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.Married == null || b.PersonalInfo.Married == "")
                        b.PersonalInfo.Married = "-";
                    if (b.PersonalInfo.SpouseName == null || b.PersonalInfo.SpouseName == "")
                        b.PersonalInfo.SpouseName = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Married.Contains(searchingMarried, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.SpouseName.Contains(searchingMarried, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingIncome != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.Income == null || b.PersonalInfo.Income == "")
                        b.PersonalInfo.Income = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Income.Contains(searchingIncome, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchingHousingType != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.HousingType == null || b.PersonalInfo.HousingType == "")
                        b.PersonalInfo.HousingType = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Income.Contains(searchingHousingType, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (gender != " All")
            {
                if (gender == "Male")
                {
                    beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Gender.Equals(Gender.Male)).ToList();
                }
                if (gender == "Female")
                { beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Gender.Equals(Gender.Female)).ToList(); }
            }

            if (searchingExpences != null)
            {
                foreach (var b in beneficiaries)
                {
                    if (b.PersonalInfo.Expences == null || b.PersonalInfo.Expences == "")
                        b.PersonalInfo.Expences = "-";
                }
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Expences.Contains(searchingExpences, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            switch (sortOrder)
            {
                case "Gender":
                    beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Gender).ToList();
                    break;

                case "Gender_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Gender).ToList();
                    break;

                case "Fullname":
                    beneficiaries = beneficiaries.OrderBy(s => s.Fullname).ToList();
                    break;

                case "Active":
                    beneficiaries = beneficiaries.OrderBy(s => s.Active).ToList();
                    break;

                case "Active_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Active).ToList();
                    break;

                case "name_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Fullname).ToList();
                    break;

                case "Date":
                    beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Birthdate).ToList();
                    break;

                case "date_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Birthdate).ToList();
                    break;

                default:
                    beneficiaries = beneficiaries.OrderBy(s => s.Fullname).ToList();
                    break;
            }
            return beneficiaries;
        }
    }
}