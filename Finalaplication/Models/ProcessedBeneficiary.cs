using Finalaplication.DatabaseManager;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Models
{
    public class ProcessedBeneficiary
    {
        private BeneficiaryManager beneficiaryManager = new BeneficiaryManager();
        private BeneficiaryContractManager beneficiaryContractManager = new BeneficiaryContractManager();
        private List<string[]> result;
        private string duplicates;
        private int documentsimported;

        public ProcessedBeneficiary(List<string[]> result,
        string duplicates,
        int documentsimported
        )
        {
            this.result = result;
            this.duplicates = duplicates;
            this.documentsimported = documentsimported;
        }

        public string ReturnRegistrationNumber(string number)
        {
            string NumberOfRegistration;
            if (number.Contains("/") == true)
            {
                string[] registration = number.Split("/");
                NumberOfRegistration = registration[0];
            }
            else
            { NumberOfRegistration = number; }

            return NumberOfRegistration;
        }

        public async Task ImportBeneficiaryContractsFromCsv()
        {
            List<Beneficiary> beneficiarycollection = beneficiaryManager.GetListOfBeneficiaries();
            List<Beneficiarycontract> beneficiarycontractcollection = beneficiaryContractManager.GetListOfBeneficiariesContracts();

            foreach (var details in result)
            {
                if (details[9] != null || details[9] != "")
                {
                    if (details[15] != "" && details[16] != "")
                    {
                        var results = beneficiarycollection.Where(x => x.CNP == details[9]); ;

                        string numberOfRegistration = ReturnRegistrationNumber(details[15]);
                        if (beneficiarycontractcollection.Count(z => z.NumberOfRegistration == numberOfRegistration) != 0)
                        { }
                        else
                        {
                            foreach (var b in results)
                            {
                                Beneficiarycontract beneficiarycontract = new Beneficiarycontract
                                {
                                    Fullname = b.Fullname,
                                    Address = b.Adress,
                                    OwnerID = b.BeneficiaryID,
                                    Birthdate = b.PersonalInfo.Birthdate,
                                    CIinfo = b.CI.CIinfo,
                                    CNP = b.CNP,
                                    IdApplication = b.Marca.IdAplication,
                                    IdInvestigation = b.Marca.IdInvestigation,
                                    Nrtel = b.PersonalInfo.PhoneNumber,
                                    NumberOfPortion = b.NumberOfPortions.ToString(),
                                    NumberOfRegistration = ReturnRegistrationNumber(details[15]),

                                    RegistrationDate = DateTime.MinValue,
                                    ExpirationDate = DateTime.MinValue
                                };
                                try
                                {
                                    string[] splitDates = details[16].Split('-');
                                    string[] forRegistrtionDate = splitDates[0].Split('.');
                                    string forRegistrationDate = forRegistrtionDate[2] + "-" + forRegistrtionDate[1] + "-" + forRegistrtionDate[0];
                                    DateTime data = DateTime.ParseExact(forRegistrationDate, "yy-MM-dd",
                                    System.Globalization.CultureInfo.InvariantCulture);
                                    beneficiarycontract.RegistrationDate = data.AddDays(1);

                                    string[] forexpirationDate = splitDates[1].Split('.');
                                    string forExpirationDate = forRegistrtionDate[2] + "-" + forRegistrtionDate[1] + "-" + forRegistrtionDate[0];
                                    DateTime data_ = DateTime.ParseExact(forRegistrationDate, "yy-MM-dd",
                                    System.Globalization.CultureInfo.InvariantCulture);
                                    beneficiarycontract.ExpirationDate = data_.AddDays(1);
                                }
                                catch
                                {
                                    string[] splitDates = details[16].Split('-');
                                    string[] forRegistrtionDate = splitDates[0].Split('.');
                                    string forRegistrationDate = forRegistrtionDate[2] + "-" + forRegistrtionDate[1] + "-" + forRegistrtionDate[0];
                                    DateTime data = Convert.ToDateTime(forRegistrationDate);
                                    beneficiarycontract.RegistrationDate = data.AddDays(1);

                                    string[] forexpirationDate = splitDates[1].Split('.');
                                    string forExpirationDate = forRegistrtionDate[2] + "-" + forRegistrtionDate[1] + "-" + forRegistrtionDate[0];
                                    DateTime data_ = Convert.ToDateTime(forRegistrationDate);
                                    beneficiarycontract.ExpirationDate = data_.AddDays(1);
                                }

                                beneficiaryContractManager.AddBeneficiaryContractToDB(beneficiarycontract);
                            }
                        }
                    }
                }
            }
        }

        public async Task<Tuple<string, string>> GetProcessedBeneficiaries()
        {
            List<Beneficiary> beneficiarycollection = beneficiaryManager.GetListOfBeneficiaries();
            List<Beneficiarycontract> beneficiarycontractcollection = beneficiaryContractManager.GetListOfBeneficiariesContracts();

            foreach (var details in result)
            {
                if (beneficiarycollection.Count(z => z.CNP == details[8]) >= 1 && details[8] != "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.Count(z => z.CNP == details[9]) >= 1 && details[9] != "")
                {
                    duplicates = duplicates + details[1] + ", ";
                }
                else if (beneficiarycollection.Count(z => z.Fullname == details[0]) >= 1 && details[8] == "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.Count(z => z.Fullname == details[1]) >= 1 && details[9] == "")
                {
                    duplicates = duplicates + details[1] + ", ";
                }
                else
                {
                    documentsimported++;

                    Beneficiary beneficiary = new Beneficiary();

                    try
                    {
                        beneficiary.Fullname = details[1];
                    }
                    catch
                    {
                        beneficiary.Fullname = "Incorrect Name";
                    }
                    try
                    {
                        beneficiary.Adress = details[8];
                    }
                    catch
                    {
                        beneficiary.Adress = "Incorrect address";
                    }

                    if (details[3] == "Activ" || details[3] == "activ" || details[3] == "Da" || details[3] == "DA" || details[3] == "da")
                    {
                        beneficiary.Active = true;
                    }
                    else
                    {
                        beneficiary.Active = false;
                    }

                    if (details[4] == "da" || details[4] == "DA" || details[4] == "DA")
                    {
                        beneficiary.HomeDelivery = true;
                    }
                    else
                    {
                        beneficiary.HomeDelivery = false;
                    }

                    if (details[5] == "da" || details[5] == "DA" || details[5] == "DA")
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

                    if (details[7] != null || details[7] != "")
                    {
                        beneficiary.HomeDeliveryDriver = details[7];
                    }
                    else
                    {
                        beneficiary.HomeDeliveryDriver = " ";
                    }

                    if (details[9] != null || details[9] != "")
                    {
                        beneficiary.CNP = details[9];
                    }

                    try
                    {
                        if (details[17] != null || details[17] != " ")
                        {
                            if (int.TryParse(details[17], out int portions))
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
                        Marca MarcaDetails = new Marca();
                        if (details[12] != null) { MarcaDetails.marca = details[12]; }
                        if (details[13] != null) { MarcaDetails.IdAplication = details[13]; }
                        if (details[14] != null) { MarcaDetails.IdInvestigation = details[14]; }
                        if (details[15] != null) { MarcaDetails.IdContract = details[15]; }
                        beneficiary.Marca = MarcaDetails;
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
                        if (details[18] != null || details[18] != "")
                        {
                            personal.PhoneNumber = details[18];
                        }

                        if (details[19] != null || details[19] != "")
                        {
                            personal.BirthPlace = details[19];
                        }

                        if (details[20] != null)
                        {
                            personal.Studies = details[20];
                        }

                        personal.Profesion = details[21];
                        personal.Ocupation = details[22];
                        personal.SeniorityInWorkField = details[23];
                        personal.HealthState = details[24];
                    }
                    catch
                    {
                        beneficiary.PersonalInfo.Profesion = "Error";
                        beneficiary.PersonalInfo.Ocupation = "Error";
                        beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                        beneficiary.PersonalInfo.HealthState = "Error";
                    }

                    if (details[25] != " " || details[25] != null)
                    {
                        personal.Disalility = details[25];
                    }
                    else { personal.Disalility = " "; }

                    if (details[26] != " " || details[26] != null)
                    {
                        personal.ChronicCondition = details[26];
                    }
                    else { personal.ChronicCondition = " "; }

                    if (details[27] != " " || details[27] != null)
                    {
                        personal.Addictions = details[27];
                    }
                    else { personal.Addictions = " "; }

                    if (details[28] == "Da" || details[28] == "DA" || details[28] == "DA")
                    {
                        personal.HealthInsurance = true;
                    }
                    else { personal.HealthInsurance = false; }

                    if (details[29] == "Da" || details[29] == "DA" || details[29] == "DA")
                    {
                        personal.HealthCard = true;
                    }
                    else { personal.HealthCard = false; }

                    if (details[30] != " " || details[30] != null)
                    {
                        personal.Married = details[30];
                    }
                    else { personal.Married = " "; }

                    if (details[31] != " " || details[31] != null)
                    {
                        personal.SpouseName = details[31];
                    }
                    else { personal.SpouseName = " "; }

                    if (details[32] != " " || details[32] != null)
                    {
                        personal.HousingType = details[32];
                    }
                    else { personal.HousingType = " "; }

                    if (details[33] == "da" || details[33] == "DA" || details[33] == "Da")
                    {
                        personal.HasHome = true;
                    }
                    else { personal.HasHome = false; }

                    if (details[34] != " " || details[34] != null)
                    {
                        personal.Income = details[34];
                    }
                    else { personal.Income = " "; }

                    if (details[35] != " " || details[35] != null)
                    {
                        personal.Expences = details[35];
                    }
                    else { personal.Expences = " "; }
                    try
                    {
                        try
                        {
                            DateTime myDate = DateTime.ParseExact(details[9].Substring(1, 2) + "-" + details[9].Substring(3, 2) + "-" + details[9].Substring(5, 2), "yy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture);
                            personal.Birthdate = myDate.AddHours(5);
                        }
                        catch
                        {
                            string date = details[36] + "-" + details[37] + "-" + details[38];
                            DateTime data = DateTime.ParseExact(date, "yy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture);
                            personal.Birthdate = data.AddDays(1);
                        }
                    }
                    catch { personal.Birthdate = DateTime.MinValue; }

                    if (details[41] == "1" || details[41] == "True")
                    {
                        personal.Gender = VolCommon.Gender.Female;
                    }
                    else
                    {
                        personal.Gender = VolCommon.Gender.Male;
                    }
                    if (details[42] != null)
                    {
                        beneficiary.Comments = details[42];
                    }
                    else
                    {
                        beneficiary.Comments = "";
                    }

                    CI ciInfo = new CI();

                    try
                    {
                        if (details[11] == null || details[11] == "")
                        {
                            ciInfo.ExpirationDateCI = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[11].Contains("/") == true)
                            {
                                string[] date = details[16].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = DateTime.ParseExact(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1], "yy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture); ;
                            }
                            else
                            {
                                string[] anotherDate = details[11].Split('.');
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
                        ciInfo.CIinfo = details[10];

                        if (details[10] != "" || details[10] != null)
                        { ciInfo.HasId = false; }
                    }
                    catch
                    {
                        ciInfo.CIinfo = "";
                    }
                    beneficiary.PersonalInfo = personal;
                    beneficiary.CI = ciInfo;

                    beneficiaryManager.AddBeneficiaryToDB(beneficiary);
                }
            }
            string key1 = "BeneficiaryImportDuplicate";
            return new Tuple<string, string>(documentsimported.ToString(), key1);
        }
    }
}