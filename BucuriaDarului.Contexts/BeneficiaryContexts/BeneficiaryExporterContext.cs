using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
    public class BeneficiaryExporterContext
    {
        private readonly IStringLocalizer localizer;

        public BeneficiaryExporterContext(IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public BeneficiaryExporterResponse Execute(BeneficiaryExporterRequest request)
        {
            var idsAndFields = GetIdAndFieldString(request.ExportParameters);
            var header = GetHeaderForExcelPrinterBeneficiary();
            var response = new BeneficiaryExporterResponse(CreateDictionaries(Constants.BENEFICIARYSESSION, Constants.BENEFICIARYHEADER, idsAndFields, header));
            response.IsValid = CheckForProperties(idsAndFields);
            return response;
        }

        private bool CheckForProperties(string idsAndFields)
        {
            var splitStrings = idsAndFields.Split("(((");
            if (splitStrings[1] == "")
                return false;

            return true;
        }

        private string GetIdAndFieldString(ExportParameters csv)
        {
            var idsAndOptions = csv.StringOfIDs + "(((";
            if (csv.All)
                idsAndOptions += "0";
            if (csv.Fullname)
                idsAndOptions += "1";
            if (csv.Active)
                idsAndOptions += "2";
            if (csv.Canteen)
                idsAndOptions += "3";
            if (csv.HomeDelivery)
                idsAndOptions += "4";
            if (csv.HomeDeliveryDriver)
                idsAndOptions += "5";
            if (csv.HasGDPRAgreement)
                idsAndOptions += "6";
            if (csv.Address)
                idsAndOptions += "7";
            if (csv.CNP)
                idsAndOptions += "8";
            if (csv.CIInfo)
                idsAndOptions += "9";
            if (csv.Marca)
                idsAndOptions += "A";
            if (csv.IdInvestigation)
                idsAndOptions += "B";
            if (csv.IdApplication)
                idsAndOptions += "C";
            if (csv.NumberOfPortions)
                idsAndOptions += "D";
            if (csv.LastTimeActive)
                idsAndOptions += "E";
            if (csv.PhoneNumber)
                idsAndOptions += "F";
            if (csv.BirthPlace)
                idsAndOptions += "G";
            if (csv.Studies)
                idsAndOptions += "H";
            if (csv.Profession)
                idsAndOptions += "I";
            if (csv.Occupation)
                idsAndOptions += "J";
            if (csv.SeniorityInWorkField)
                idsAndOptions += "K";
            if (csv.HealthState)
                idsAndOptions += "L";
            if (csv.Disability)
                idsAndOptions += "M";
            if (csv.ChronicCondition)
                idsAndOptions += "N";
            if (csv.Addictions)
                idsAndOptions += "O";
            if (csv.HealthInsurance)
                idsAndOptions += "Z";
            if (csv.HealthCard)
                idsAndOptions += "P";
            if (csv.Married)
                idsAndOptions += "Q";
            if (csv.SpouseName)
                idsAndOptions += "R";
            if (csv.HasHome)
                idsAndOptions += "S";
            if (csv.HousingType)
                idsAndOptions += "T";
            if (csv.Income)
                idsAndOptions += "U";
            if (csv.Expenses)
                idsAndOptions += "V";
            if (csv.Gender)
                idsAndOptions += "W";
            return idsAndOptions;
        }

        private string GetHeaderForExcelPrinterBeneficiary()
        {
            var header = new string[35];
            header[0] = localizer["Fullname"];
            header[1] = localizer["Active"];
            header[2] = localizer["Canteen"];
            header[3] = localizer["HomeDelivery"];
            header[4] = localizer["HomeDeliveryDriver"];
            header[5] = localizer["HasGDPRagreement"];
            header[6] = localizer["AddressInformation"];
            header[7] = localizer["CNP"];
            header[8] = localizer["CISeria"];
            header[9] = localizer["CIEliberat"];
            header[10] = localizer["Marca"];
            header[11] = localizer["IDinvestigation"];
            header[12] = localizer["IDapplication"];
            header[13] = localizer["NumberOfPortions"];
            header[14] = localizer["LastTimeActive"];
            header[15] = localizer["PhoneNumber"];
            header[16] = localizer["Birthplace"];
            header[17] = localizer["Studies"];
            header[18] = localizer["Profession"];
            header[19] = localizer["Occupation"];
            header[20] = localizer["SeniorityInWorkField"];
            header[21] = localizer["HealthState"];
            header[22] = localizer["Disability"];
            header[23] = localizer["ChronicCondition"];
            header[24] = localizer["Addictions"];
            header[25] = localizer["HasHealthInsurance"];
            header[26] = localizer["HasHealthCard"];
            header[27] = localizer["Married"];
            header[28] = localizer["SpouseName"];
            header[29] = localizer["Homeless"];
            header[30] = localizer["HousingType"];
            header[31] = localizer["Income"];
            header[32] = localizer["Expenses"];
            header[33] = localizer["Gender"];
            header[34] = localizer["WeeklyPackage"];

            var result = string.Empty;
            for (var i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        private Dictionary<string, string> CreateDictionaries(string key1, string key2, string idsAndFields, string header)
        {
            var result = new Dictionary<string, string>
            {
                { key1, idsAndFields },
                { key2, header }
            };

            return result;
        }
    }

    public class BeneficiaryExporterResponse
    {
        public Dictionary<string, string> Dictionary { get; set; }

        public bool IsValid { get; set; }

        public BeneficiaryExporterResponse(Dictionary<string, string> dictionary)
        {
            Dictionary = dictionary;
            IsValid = true;
        }
    }

    public class BeneficiaryExporterRequest
    {
        public BeneficiaryExporterRequest(ExportParameters exportProperties)
        {
            ExportParameters = exportProperties;
        }

        public ExportParameters ExportParameters { get; set; }
    }

    public class ExportParameters
    {
        public string StringOfIDs { get; set; }
        public bool All { get; set; }
        public bool Fullname { get; set; }
        public bool Active { get; set; }
        public bool Canteen { get; set; }
        public bool HomeDelivery { get; set; }
        public bool HomeDeliveryDriver { get; set; }
        public bool HasGDPRAgreement { get; set; }
        public bool Address { get; set; }
        public bool CNP { get; set; }
        public bool CIInfo { get; set; }
        public bool Marca { get; set; }
        public bool IdInvestigation { get; set; }
        public bool IdApplication { get; set; }
        public bool NumberOfPortions { get; set; }
        public bool LastTimeActive { get; set; }
        public bool PhoneNumber { get; set; }
        public bool BirthPlace { get; set; }
        public bool Studies { get; set; }
        public bool Profession { get; set; }
        public bool Occupation { get; set; }
        public bool SeniorityInWorkField { get; set; }
        public bool HealthState { get; set; }
        public bool Disability { get; set; }
        public bool ChronicCondition { get; set; }
        public bool Addictions { get; set; }
        public bool HealthInsurance { get; set; }
        public bool HealthCard { get; set; }
        public bool Married { get; set; }
        public bool SpouseName { get; set; }
        public bool HasHome { get; set; }
        public bool HousingType { get; set; }
        public bool Income { get; set; }
        public bool Expenses { get; set; }
        public bool Gender { get; set; }
        public bool WeeklyPackage { get; set; }
    }
}