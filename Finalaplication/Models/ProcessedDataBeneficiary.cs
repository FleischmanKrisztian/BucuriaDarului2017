using MongoDB.Driver;
using System;
using System.Collections.Generic;
using VolCommon;

namespace Finalaplication.Models
{
    public delegate string DuplicatesCallback(string duplicates);

    public delegate int Documentsimportedcallback(int documentsimported);

    public class ProcessedDataBeneficiary
    {
        private IMongoCollection<Beneficiary> beneficiarycollection;
        private List<string[]> result;
        private string duplicates;
        private int documentsimported;
        private DuplicatesCallback callback1;
        private Documentsimportedcallback callback2;

        public ProcessedDataBeneficiary(IMongoCollection<Beneficiary> beneficiarycollection,
        List<string[]> result,
        string duplicates,
        int documentsimported,
        DuplicatesCallback _callback1,
        Documentsimportedcallback _callback2
        )
        {
            this.beneficiarycollection = beneficiarycollection;
            this.result = result;
            this.duplicates = duplicates;
            this.documentsimported = documentsimported;
            this.callback1 = _callback1;
            this.callback2 = _callback2;
        }

        public void GetProcessedBeneficiaries(IMongoCollection<Beneficiary> beneficiarycollection, List<string[]> result, string duplicates, int documentsimported)
        {
            foreach (var details in result)
            {
                if (beneficiarycollection.CountDocuments(z => z.CNP == details[8]) >= 1 && details[8] != "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.CNP == details[9]) >= 1 && details[9] != "")
                {
                    duplicates = duplicates + details[1] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.Fullname == details[0]) >= 1 && details[8] == "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.Fullname == details[1]) >= 1 && details[9] == "")
                {
                    duplicates = duplicates + details[1] + ", ";
                }
                else
                {
                    if (details[2] == "True" || details[2] == "False")
                    {
                        documentsimported++;
                        var aux = details[42];
                        for (int i = details.Length - 1; i > 0; i--)
                        {
                            details[i] = details[i - 1];
                        }
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

                        if (details[2] == "true" || details[2] == "True")
                        {
                            beneficiary.Active = true;
                        }
                        else
                        {
                            beneficiary.Active = false;
                        }

                        if (details[4] == "true" || details[4] == "True")
                        {
                            beneficiary.Canteen = true;
                        }
                        else
                        {
                            beneficiary.Canteen = false;
                        }

                        if (details[5] == "true" || details[5] == "True")
                        {
                            beneficiary.HomeDelivery = true;
                        }
                        else
                        {
                            beneficiary.HomeDelivery = false;
                        }

                        if (details[3] == "true" || details[3] == "True")
                        {
                            beneficiary.Weeklypackage = true;
                        }
                        else
                        {
                            beneficiary.Weeklypackage = false;
                        }

                        if (details[6] != null || details[6] != "")
                        {
                            beneficiary.HomeDeliveryDriver = details[6];
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
                            if (details[21] != null || details[21] != " ")
                            {
                                if (int.TryParse(details[21], out int portions))
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
                            if (details[25] != null) { MarcaDetails.IdContract = details[15]; }
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
                            personal.HealthState = details[31];
                        }
                        catch
                        {
                            beneficiary.PersonalInfo.Profesion = "Error";
                            beneficiary.PersonalInfo.Ocupation = "Error";
                            beneficiary.PersonalInfo.SeniorityInWorkField = "Error";
                            beneficiary.PersonalInfo.HealthState = "Error";
                        }

                        if (details[32] != " " || details[32] != null)
                        {
                            personal.Disalility = details[32];
                        }
                        else { personal.Disalility = " "; }

                        if (details[33] != " " || details[33] != null)
                        {
                            personal.ChronicCondition = details[33];
                        }
                        else { personal.ChronicCondition = " "; }

                        if (details[34] != " " || details[34] != null)
                        {
                            personal.Addictions = details[34];
                        }
                        else { personal.Addictions = " "; }

                        if (details[35] == "true" || details[35] == "True")
                        {
                            personal.HealthInsurance = true;
                        }
                        else { personal.HealthInsurance = false; }

                        if (details[36] == "true" || details[36] == "True")
                        {
                            personal.HealthCard = true;
                        }
                        else { personal.HealthCard = false; }

                        if (details[37] != " " || details[37] != null)
                        {
                            personal.Married = details[37];
                        }
                        else { personal.Married = " "; }

                        if (details[38] != " " || details[38] != null)
                        {
                            personal.SpouseName = details[38];
                        }
                        else { personal.SpouseName = " "; }

                        if (details[40] != " " || details[40] != null)
                        {
                            personal.HousingType = details[40];
                        }
                        else { personal.HousingType = " "; }

                        if (details[39] == "true" || details[39] == "True")
                        {
                            personal.HasHome = true;
                        }
                        else { personal.HasHome = false; }

                        if (details[41] != " " || details[41] != null)
                        {
                            personal.Income = details[41];
                        }
                        else { personal.Income = " "; }

                        if (details[42] != " " || details[42] != null)
                        {
                            personal.Expences = details[42];
                        }
                        else { personal.Expences = " "; }

                        try
                        {
                            if (details[24] == null || details[24] == "")
                            {
                                DateTime myDate = DateTime.ParseExact(details[9].Substring(1, 2) + "-" + details[9].Substring(3, 2) + "-" + details[9].Substring(5, 2), "yy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                personal.Birthdate = myDate.AddHours(5);
                            }
                            else
                            {
                                DateTime myDate;
                                if (details[24].Contains("/") == true)
                                {
                                    string[] date = details[24].Split(" ");
                                    string[] FinalDate = date[0].Split("/");
                                    myDate = DateTime.ParseExact(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1], "yy-MM-dd",
                                    System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    string[] anotherDate = details[24].Split('.');
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
                            if (aux == "1" || aux == "True")
                            {
                                personal.Gender = VolCommon.Gender.Female;
                            }
                            else
                            {
                                personal.Gender = VolCommon.Gender.Male;
                            }
                            beneficiary.Comments = details[23];
                        }
                        catch
                        {
                            beneficiary.Comments = "";
                        }

                        if (details[22] == null || details[22] == "")
                        {
                            beneficiary.LastTimeActiv = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime data;
                            if (details[22].Contains("/") == true)
                            {
                                string[] date = details[22].Split(" ");
                                string[] FinalDate = date[0].Split("/");
                                data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                            }
                            else
                            {
                                string[] anotherDate = details[22].Split('.');
                                data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                            }
                            beneficiary.LastTimeActiv = data.AddDays(1);
                        }

                        CI ciInfo = new CI();

                        try
                        {
                            if (details[16] == null || details[16] == "")
                            {
                                ciInfo.ExpirationDateCI = DateTime.MinValue;
                            }
                            else
                            {
                                DateTime data;
                                if (details[16].Contains("/") == true)
                                {
                                    string[] date = details[16].Split(" ");
                                    string[] FinalDate = date[0].Split("/");
                                    data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                }
                                else
                                {
                                    string[] anotherDate = details[16].Split('.');
                                    data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
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
                            ciInfo.CIinfo = details[15];

                            if (details[10] != "True" || details[10] != "true")
                            { ciInfo.HasId = false; }
                        }
                        catch
                        {
                            ciInfo.CIinfo = "";
                        }
                        beneficiary.PersonalInfo = personal;
                        beneficiary.CI = ciInfo;

                        beneficiarycollection.InsertOne(beneficiary);
                    }
                }
            }
            callback1?.Invoke(duplicates);
            callback2?.Invoke(documentsimported);
        }

        public void GetProcessedBeneficiariesFromApp(IMongoCollection<Beneficiary> beneficiarycollection, List<string[]> result, string duplicates, int documentsimported)
        {
            foreach (var details in result)
            {
                if (beneficiarycollection.CountDocuments(z => z.CNP == details[8]) >= 1 && details[8] != "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.CNP == details[9]) >= 1 && details[9] != "")
                {
                    duplicates = duplicates + details[1] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.Fullname == details[0]) >= 1 && details[8] == "")
                {
                    duplicates = duplicates + details[0] + ", ";
                }
                else if (beneficiarycollection.CountDocuments(z => z.Fullname == details[1]) >= 1 && details[9] == "")
                {
                    duplicates = duplicates + details[1] + ", ";
                }
                else
                {
                    {
                        documentsimported++;

                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary.HasGDPRAgreement = false;

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

                        if (details[3] == "true" || details[3] == "True" || details[3] == "Activ" || details[3] == "activ")
                        {
                            beneficiary.Active = true;
                        }
                        else
                        {
                            beneficiary.Active = false;
                        }

                        if (details[4] == "da" || details[4] == "DA" || details[4] == "Da")
                        {
                            beneficiary.HomeDelivery = true;
                        }
                        else
                        {
                            beneficiary.HomeDelivery = false;
                        }
                        if (beneficiary.HomeDelivery == true)
                        { beneficiary.Canteen = false; }
                        if (details[5] == "da" || details[5] == "DA" || details[5] == "Da")
                        {
                            beneficiary.Weeklypackage = true;
                        }
                        else
                        {
                            beneficiary.Weeklypackage = false;
                        }

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

                        if (details[28] == "true" || details[28] == "True" || details[28] == "da" || details[28] == "Da" || details[28] == "DA")
                        {
                            personal.HealthInsurance = true;
                        }
                        else { personal.HealthInsurance = false; }

                        if (details[29] == "true" || details[29] == "True" || details[29] == "da" || details[29] == "Da" || details[29] == "DA")
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

                        if (details[33] == "true" || details[33] == "True" || details[33] == "da" || details[33] == "Da" || details[33] == "DA")
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
                            string date = details[36] + "-" + details[37] + "-" + details[38];
                            DateTime data = Convert.ToDateTime(date);
                            personal.Birthdate = data.AddDays(1);
                        }
                        catch
                        {
                            try
                            {
                                DateTime myDate = DateTime.ParseExact(details[9].Substring(1, 2) + "-" + details[9].Substring(3, 2) + "-" + details[9].Substring(5, 2), "yy-MM-dd",
                                System.Globalization.CultureInfo.InvariantCulture);
                                personal.Birthdate = myDate.AddHours(5);
                            }
                            catch
                            {
                                personal.Birthdate = DateTime.MinValue;
                            }
                        }

                        try
                        {
                            if (details[42] == "F" || details[42] == "f")
                            {
                                personal.Gender = VolCommon.Gender.Female;
                            }
                            else
                            {
                                personal.Gender = VolCommon.Gender.Male;
                            }
                            beneficiary.Comments = details[42];
                        }
                        catch
                        {
                            beneficiary.Comments = "";
                        }

                        beneficiary.LastTimeActiv = DateTime.MinValue;

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
                                    string[] date = details[11].Split(" ");
                                    string[] FinalDate = date[0].Split("/");
                                    data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                                }
                                else
                                {
                                    string[] anotherDate = details[11].Split('.');
                                    data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
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
                            { ciInfo.HasId = true; }
                        }
                        catch
                        {
                            ciInfo.CIinfo = "";
                        }
                        beneficiary.PersonalInfo = personal;
                        beneficiary.CI = ciInfo;

                        beneficiarycollection.InsertOne(beneficiary);
                    }
                }
            }

            callback1?.Invoke(duplicates);
            callback2?.Invoke(documentsimported);
        }
    }
}