﻿using Microsoft.Extensions.Localization;
using System;
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
            if (request.ExportParameters.FileName != "" && request.ExportParameters.FileName != null)
            {
                if (request.ExportParameters.FileName.Contains(".csv"))
                    response.FileName = request.ExportParameters.FileName;
                else
                    response.FileName = request.ExportParameters.FileName + ".csv";
            }
            else
                response.FileName = "BeneficiariesReport" + DateTime.Now.ToString() + ".csv";
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
            if (csv.WeeklyPackage)
                idsAndOptions += "X";
            return idsAndOptions;
        }

        private string GetHeaderForExcelPrinterBeneficiary()
        {
            var header = new string[39];
            header[0] = localizer["Id"];
            header[1] = localizer["Fullname"];
            header[2] = localizer["Active"];
            header[3] = localizer["WeeklyPackage"];
            header[4] = localizer["Canteen"];
            header[5] = localizer["HomeDelivery"];
            header[6] = localizer["HomeDeliveryDriver"];
            header[7] = localizer["HasGDPRAgreement"];
            header[8] = localizer["AddressInformation"];
            header[9] = localizer["CNP"];
            header[10] = localizer["HasId"];
            header[11] = localizer["CiInfo"];
            header[12] = localizer["CiExpirationDate"];
            header[13] = localizer["MarcaName"];
            header[14] = localizer["IdApplication"];
            header[15] = localizer["IdInvestigation"];
            header[16] = localizer["Marca_IdContract"];
            header[17] = localizer["NumberOfPortions"];
            header[18] = localizer["LastTimeActive"];
            header[19] = localizer["Comments"];
            header[20] = localizer["Birthplace"];
            header[21] = localizer["PhoneNumber"];
            header[22] = localizer["Studies"];
            header[23] = localizer["Profession"];
            header[24] = localizer["Occupation"];
            header[25] = localizer["SeniorityInWorkField"];
            header[26] = localizer["HealthState"];
            header[27] = localizer["Disability"];
            header[28] = localizer["ChronicCondition"];
            header[29] = localizer["Addictions"];
            header[30] = localizer["HasHealthInsurance"];
            header[31] = localizer["HasHealthCard"];
            header[32] = localizer["Married"];
            header[33] = localizer["SpouseName"];
            header[34] = localizer["HasHome"];
            header[35] = localizer["HousingType"];
            header[36] = localizer["Income"];
            header[37] = localizer["Expenses"];
            header[38] = localizer["Gender"];

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
        public string FileName { get; set; }

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
        public string FileName { get; set; }
    }
}