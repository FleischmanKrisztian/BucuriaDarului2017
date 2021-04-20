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
            string stringofids = "beneficiaryCSV";
            foreach (Beneficiary beneficiary in beneficiaries)
            {
                stringofids = stringofids + "," + beneficiary._id;
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
                Beneficiary singlebeneficiary = beneficiaries.Where(x => x._id == beneficiaryids[i]).First();
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

        internal static Beneficiary GetBeneficiaryFromOtherString(string[] beneficiarystring)
        {
            Beneficiary beneficiary = new Beneficiary();
            beneficiary._id = Guid.NewGuid().ToString();
            try
            {
                beneficiary.Fullname = beneficiarystring[1];
            }
            catch
            {
                beneficiary.Fullname = "Incorrect Name";
            }
            try
            {
                beneficiary.Adress = beneficiarystring[8];
            }
            catch
            {
                beneficiary.Adress = "Incorrect address";
            }

            if (beneficiarystring[3] == "Activ" || beneficiarystring[3] == "activ" || beneficiarystring[3] == "Da" || beneficiarystring[3] == "DA" || beneficiarystring[3] == "da")
            {
                beneficiary.Active = true;
            }
            else
            {
                beneficiary.Active = false;
            }

            if (beneficiarystring[4] == "da" || beneficiarystring[4] == "DA" || beneficiarystring[4] == "DA")
            {
                beneficiary.HomeDelivery = true;
            }
            else
            {
                beneficiary.HomeDelivery = false;
            }

            if (beneficiarystring[5] == "da" || beneficiarystring[5] == "DA" || beneficiarystring[5] == "DA")
            {
                beneficiary.Weeklypackage = true;
            }
            else
            {
                beneficiary.Weeklypackage = false;
            }
            if (beneficiary.Weeklypackage == true)
            { beneficiary.Canteen = false; }
            else
            { beneficiary.Canteen = true; }

            if (beneficiarystring[7] != null || beneficiarystring[7] != "")
            {
                beneficiary.HomeDeliveryDriver = beneficiarystring[7];
            }
            else
            {
                beneficiary.HomeDeliveryDriver = " ";
            }

            if (beneficiarystring[9] != null || beneficiarystring[9] != "")
            {
                beneficiary.CNP = beneficiarystring[9];
            }

            try
            {
                if (beneficiarystring[17] != null || beneficiarystring[17] != " ")
                {
                    if (int.TryParse(beneficiarystring[17], out int portions))
                    {
                        beneficiary.NumberOfPortions = portions;
                    }
                }
            }
            catch
            {
                beneficiary.NumberOfPortions = 0;
            }

            try
            {
                Marca Marcabeneficiarystring = new Marca();
                if (beneficiarystring[12] != null) { Marcabeneficiarystring.marca = beneficiarystring[12]; }
                if (beneficiarystring[13] != null) { Marcabeneficiarystring.IdAplication = beneficiarystring[13]; }
                if (beneficiarystring[14] != null) { Marcabeneficiarystring.IdInvestigation = beneficiarystring[14]; }
                if (beneficiarystring[15] != null) { Marcabeneficiarystring.IdContract = beneficiarystring[15]; }
                beneficiary.Marca = Marcabeneficiarystring;
            }
            catch
            {
                beneficiary.Marca.marca = "error";
                beneficiary.Marca.IdAplication = "error";
                beneficiary.Marca.IdInvestigation = "error";
                beneficiary.Marca.IdContract = "error";
            }

            Personalinfo personal = new Personalinfo();
            try
            {
                if (beneficiarystring[18] != null || beneficiarystring[18] != "")
                {
                    personal.PhoneNumber = beneficiarystring[18];
                }

                if (beneficiarystring[19] != null || beneficiarystring[19] != "")
                {
                    personal.BirthPlace = beneficiarystring[19];
                }

                if (beneficiarystring[20] != null)
                {
                    personal.Studies = beneficiarystring[20];
                }

                personal.Profesion = beneficiarystring[21];
                personal.Ocupation = beneficiarystring[22];
                personal.SeniorityInWorkField = beneficiarystring[23];
                personal.HealthState = beneficiarystring[24];
            }
            catch
            {
                beneficiary.PersonalInfo.Profesion = "Error";
                beneficiary.PersonalInfo.Ocupation = "Error";
                beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                beneficiary.PersonalInfo.HealthState = "Error";
            }

            if (beneficiarystring[25] != " " || beneficiarystring[25] != null)
            {
                personal.Disalility = beneficiarystring[25];
            }
            else { personal.Disalility = " "; }

            if (beneficiarystring[26] != " " || beneficiarystring[26] != null)
            {
                personal.ChronicCondition = beneficiarystring[26];
            }
            else { personal.ChronicCondition = " "; }

            if (beneficiarystring[27] != " " || beneficiarystring[27] != null)
            {
                personal.Addictions = beneficiarystring[27];
            }
            else { personal.Addictions = " "; }

            if (beneficiarystring[28] == "Da" || beneficiarystring[28] == "DA" || beneficiarystring[28] == "DA")
            {
                personal.HealthInsurance = true;
            }
            else { personal.HealthInsurance = false; }

            if (beneficiarystring[29] == "Da" || beneficiarystring[29] == "DA" || beneficiarystring[29] == "DA")
            {
                personal.HealthCard = true;
            }
            else { personal.HealthCard = false; }

            if (beneficiarystring[30] != " " || beneficiarystring[30] != null)
            {
                personal.Married = beneficiarystring[30];
            }
            else { personal.Married = " "; }

            if (beneficiarystring[31] != " " || beneficiarystring[31] != null)
            {
                personal.SpouseName = beneficiarystring[31];
            }
            else { personal.SpouseName = " "; }

            if (beneficiarystring[32] != " " || beneficiarystring[32] != null)
            {
                personal.HousingType = beneficiarystring[32];
            }
            else { personal.HousingType = " "; }

            if (beneficiarystring[33] == "da" || beneficiarystring[33] == "DA" || beneficiarystring[33] == "Da")
            {
                personal.HasHome = true;
            }
            else { personal.HasHome = false; }

            if (beneficiarystring[34] != " " || beneficiarystring[34] != null)
            {
                personal.Income = beneficiarystring[34];
            }
            else { personal.Income = " "; }

            if (beneficiarystring[35] != " " || beneficiarystring[35] != null)
            {
                personal.Expences = beneficiarystring[35];
            }
            else { personal.Expences = " "; }
            try
            {
                try
                {
                    DateTime myDate = DateTime.ParseExact(beneficiarystring[9].Substring(1, 2) + "-" + beneficiarystring[9].Substring(3, 2) + "-" + beneficiarystring[9].Substring(5, 2), "yy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
                    personal.Birthdate = myDate.AddHours(5);
                }
                catch
                {
                    string date = beneficiarystring[36] + "-" + beneficiarystring[37] + "-" + beneficiarystring[38];
                    DateTime data = DateTime.ParseExact(date, "yy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
                    personal.Birthdate = data.AddDays(1);
                }
            }
            catch { personal.Birthdate = DateTime.MinValue; }

            if (beneficiarystring[41] == "1" || beneficiarystring[41] == "True")
            {
                personal.Gender = Gender.Female;
            }
            else
            {
                personal.Gender = Gender.Male;
            }
            if (beneficiarystring[42] != null)
            {
                beneficiary.Comments = beneficiarystring[42];
            }
            else
            {
                beneficiary.Comments = "";
            }

            CI ciInfo = new CI();

            try
            {
                if (beneficiarystring[11] == null || beneficiarystring[11] == "")
                {
                    ciInfo.ExpirationDateCI = DateTime.MinValue;
                }
                else
                {
                    DateTime data;
                    if (beneficiarystring[11].Contains("/") == true)
                    {
                        string[] date = beneficiarystring[16].Split(" ");
                        string[] FinalDate = date[0].Split("/");
                        data = DateTime.ParseExact(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1], "yy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture); ;
                    }
                    else
                    {
                        string[] anotherDate = beneficiarystring[11].Split('.');
                        data = DateTime.ParseExact(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0], "yy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture); ;
                    }
                    ciInfo.ExpirationDateCI = data.AddDays(1);
                }
            }
            catch
            {
                ciInfo.ExpirationDateCI = DateTime.MinValue;
            }

            try
            {
                ciInfo.CIinfo = beneficiarystring[10];

                if (beneficiarystring[10] != "" || beneficiarystring[10] != null)
                { ciInfo.HasId = false; }
            }
            catch
            {
                ciInfo.CIinfo = "";
            }
            beneficiary.PersonalInfo = personal;
            beneficiary.CI = ciInfo;
            return beneficiary;
        }

        public static Beneficiary GetBeneficiaryFromString(string[] beneficiarystring)
        {
            Beneficiary beneficiary = new Beneficiary();
            beneficiary._id = Guid.NewGuid().ToString();
            {
                beneficiary.HasGDPRAgreement = false;
            };
            try
            {
                beneficiary.Fullname = beneficiarystring[0];
            }
            catch
            {
                beneficiary.Fullname = "Incorrect Name";
            }
            try
            {
                beneficiary.Adress = beneficiarystring[7];
            }
            catch
            {
                beneficiary.Adress = "Incorrect address";
            }

            if (beneficiarystring[1] == "true" || beneficiarystring[1] == "True" || beneficiarystring[1] == "Activ" || beneficiarystring[1] == "activ")
            {
                beneficiary.Active = true;
            }
            else
            {
                beneficiary.Active = false;
            }

            if (beneficiarystring[4] == "da" || beneficiarystring[4] == "DA" || beneficiarystring[4] == "Da" || beneficiarystring[4] == "TRUE" || beneficiarystring[4] == "True")
            {
                beneficiary.HomeDelivery = true;
            }
            else
            {
                beneficiary.HomeDelivery = false;
            }
            if (beneficiarystring[3] == "da" || beneficiarystring[3] == "DA" || beneficiarystring[3] == "Da" || beneficiarystring[3] == "TRUE" || beneficiarystring[3] == "True")
            {
                beneficiary.Canteen = true;
            }
            else
            {
                beneficiary.Canteen = false;
            }

            if (beneficiarystring[2] == "da" || beneficiarystring[2] == "DA" || beneficiarystring[2] == "Da")
            {
                beneficiary.Weeklypackage = true;
            }
            else
            {
                beneficiary.Weeklypackage = false;
            }

            if (beneficiarystring[5] != null || beneficiarystring[5] != "")
            {
                beneficiary.HomeDeliveryDriver = beneficiarystring[5];
            }
            else
            {
                beneficiary.HomeDeliveryDriver = " ";
            }

            if (beneficiarystring[8] != null || beneficiarystring[8] != "")
            {
                beneficiary.CNP = beneficiarystring[8];
            }

            try
            {
                if (beneficiarystring[20] != null || beneficiarystring[20] != " ")
                {
                    if (int.TryParse(beneficiarystring[20], out int portions))
                    {
                        beneficiary.NumberOfPortions = portions;
                    }
                }
            }
            catch
            {
                beneficiary.NumberOfPortions = 0;
            }

            try
            {
                Marca Marcabeneficiarystring = new Marca();
                if (beneficiarystring[16] != null) { Marcabeneficiarystring.marca = beneficiarystring[16]; }
                if (beneficiarystring[17] != null) { Marcabeneficiarystring.IdAplication = beneficiarystring[17]; }
                if (beneficiarystring[18] != null) { Marcabeneficiarystring.IdInvestigation = beneficiarystring[18]; }
                if (beneficiarystring[19] != null) { Marcabeneficiarystring.IdContract = beneficiarystring[19]; }
                beneficiary.Marca = Marcabeneficiarystring;
            }
            catch
            {
                beneficiary.Marca.marca = "error";
                beneficiary.Marca.IdAplication = "error";
                beneficiary.Marca.IdInvestigation = "error";
                beneficiary.Marca.IdContract = "error";
            }

            Personalinfo personal = new Personalinfo();
            try
            {
                if (beneficiarystring[24] != null || beneficiarystring[24] != "")
                {
                    personal.PhoneNumber = beneficiarystring[24];
                }

                if (beneficiarystring[25] != null || beneficiarystring[25] != "")
                {
                    personal.BirthPlace = beneficiarystring[25];
                }

                if (beneficiarystring[26] != null)
                {
                    personal.Studies = beneficiarystring[26];
                }

                personal.Profesion = beneficiarystring[27];
                personal.Ocupation = beneficiarystring[28];
                personal.SeniorityInWorkField = beneficiarystring[29];
                personal.HealthState = beneficiarystring[30];
            }
            catch
            {
                beneficiary.PersonalInfo.Profesion = "Error";
                beneficiary.PersonalInfo.Ocupation = "Error";
                beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                beneficiary.PersonalInfo.HealthState = "Error";
            }

            if (beneficiarystring[31] != " " || beneficiarystring[31] != null)
            {
                personal.Disalility = beneficiarystring[31];
            }
            else { personal.Disalility = " "; }

            if (beneficiarystring[32] != " " || beneficiarystring[32] != null)
            {
                personal.ChronicCondition = beneficiarystring[32];
            }
            else { personal.ChronicCondition = " "; }

            if (beneficiarystring[33] != " " || beneficiarystring[33] != null)
            {
                personal.Addictions = beneficiarystring[33];
            }
            else { personal.Addictions = " "; }

            if (beneficiarystring[34] == "true" || beneficiarystring[34] == "True" || beneficiarystring[34] == "da" || beneficiarystring[34] == "Da" || beneficiarystring[28] == "DA")
            {
                personal.HealthInsurance = true;
            }
            else { personal.HealthInsurance = false; }

            if (beneficiarystring[35] == "true" || beneficiarystring[35] == "True" || beneficiarystring[35] == "da" || beneficiarystring[35] == "Da" || beneficiarystring[35] == "DA")
            {
                personal.HealthCard = true;
            }
            else { personal.HealthCard = false; }

            if (beneficiarystring[36] != " " || beneficiarystring[36] != null)
            {
                personal.Married = beneficiarystring[36];
            }
            else { personal.Married = " "; }

            if (beneficiarystring[37] != " " || beneficiarystring[37] != null)
            {
                personal.SpouseName = beneficiarystring[37];
            }
            else { personal.SpouseName = " "; }

            if (beneficiarystring[39] != " " || beneficiarystring[39] != null)
            {
                personal.HousingType = beneficiarystring[39];
            }
            else { personal.HousingType = " "; }

            if (beneficiarystring[38] == "true" || beneficiarystring[38] == "True" || beneficiarystring[38] == "da" || beneficiarystring[38] == "Da" || beneficiarystring[38] == "DA")
            {
                personal.HasHome = true;
            }
            else { personal.HasHome = false; }

            if (beneficiarystring[40] != " " || beneficiarystring[40] != null)
            {
                personal.Income = beneficiarystring[40];
            }
            else { personal.Income = " "; }

            if (beneficiarystring[41] != " " || beneficiarystring[41] != null)
            {
                personal.Expences = beneficiarystring[41];
            }
            else { personal.Expences = " "; }

            try
            {
                DateTime myDate;
                if (beneficiarystring[23] == null || beneficiarystring[23] == "")
                {
                    myDate = DateTime.ParseExact(beneficiarystring[8].Substring(1, 2) + "-" + beneficiarystring[8].Substring(3, 2) + "-" + beneficiarystring[8].Substring(5, 2), "yy-MM-dd",
                          System.Globalization.CultureInfo.InvariantCulture);
                    personal.Birthdate = myDate.AddHours(5);
                }
                else
                {
                    if (beneficiarystring[23].Contains("/") == true)
                    {
                        string[] date = beneficiarystring[23].Split(" ");
                        string[] FinalDate = date[0].Split("/");
                        myDate = DateTime.ParseExact(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]
                            , "yy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string[] anotherDate = beneficiarystring[23].Split('.');
                        myDate = DateTime.ParseExact(anotherDate[2] + "-" + anotherDate[0] + "-" + anotherDate[1], "yy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                    }
                    personal.Birthdate = myDate.AddDays(1);
                }
            }
            catch
            {
                personal.Birthdate = DateTime.MinValue;
            }

            try
            {
                if (beneficiarystring[42] == "F" || beneficiarystring[42] == "f")
                {
                    personal.Gender = Gender.Female;
                }
                else
                {
                    personal.Gender = Gender.Male;
                }

                beneficiary.Comments = beneficiarystring[22];
            }
            catch
            {
                beneficiary.Comments = "";
            }

            if (beneficiarystring[22] == null || beneficiarystring[22] == "")
            {
                beneficiary.LastTimeActiv = DateTime.MinValue;
            }
            else
            {
                DateTime data;
                if (beneficiarystring[21].Contains("/") == true)
                {
                    string[] date = beneficiarystring[21].Split(" ");
                    string[] FinalDate = date[0].Split("/");
                    data = DateTime.ParseExact(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1], "yy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture); ;
                }
                else
                {
                    string[] anotherDate = beneficiarystring[21].Split('.');
                    data = DateTime.ParseExact(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0], "yy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture); ;
                }
                beneficiary.LastTimeActiv = data.AddDays(1);
            }

            CI ciInfo = new CI();

            try
            {
                if (beneficiarystring[15] == null || beneficiarystring[15] == "")
                {
                    ciInfo.ExpirationDateCI = DateTime.MinValue;
                }
                else
                {
                    DateTime data;
                    if (beneficiarystring[15].Contains("/") == true)
                    {
                        string[] date = beneficiarystring[15].Split(" ");
                        string[] FinalDate = date[0].Split("/");
                        data = DateTime.ParseExact(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1], "yy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string[] anotherDate = beneficiarystring[15].Split('.');
                        data = DateTime.ParseExact(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0], "yy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture); ;
                    }
                    ciInfo.ExpirationDateCI = data.AddDays(1);
                }
            }
            catch
            {
                ciInfo.ExpirationDateCI = DateTime.MinValue;
            }

            try
            {
                ciInfo.CIinfo = beneficiarystring[14];

                if (beneficiarystring[14] != "" || beneficiarystring[14] != null)
                { ciInfo.HasId = true; }
            }
            catch
            {
                ciInfo.CIinfo = "";
            }
            beneficiary.PersonalInfo = personal;
            beneficiary.CI = ciInfo;
            return beneficiary;
        }

        internal static bool DoesNotExist(List<Beneficiary> beneficiaries, Beneficiary beneficiary)
        {
            if (beneficiary.CNP != null || beneficiary.CNP != "")
            {
                int numberofoccurences = beneficiaries.Where(p => p.CNP == beneficiary.CNP).Count();
                if (numberofoccurences >= 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
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
                ids_and_options += "G";
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