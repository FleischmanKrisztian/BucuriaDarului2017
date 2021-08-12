using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorExporterContext
    {
        private readonly IStringLocalizer localizer;

        public SponsorExporterContext(IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public SponsorExporterResponse Execute(SponsorExporterRequest request)
        {
            var idsAndFields = GetIdAndFieldString(request.ExportParameters);
            var header = GetHeaderForExcelPrinterSponsor();
            var response = new SponsorExporterResponse(CreateDictionaries(Constants.SPONSOR_SESSION, Constants.SPONSOR_HEADER, idsAndFields, header));
            response.IsValid = CheckForProperties(idsAndFields);
            if (!string.IsNullOrEmpty(request.ExportParameters.FileName))
            {
                if (request.ExportParameters.FileName.Contains(".csv"))
                    response.FileName = request.ExportParameters.FileName;
                else
                    response.FileName = request.ExportParameters.FileName + ".csv";
            }
            else
                response.FileName = "SponsorsReport" + DateTime.Now.ToString() + ".csv";
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
            if (csv.NameOfSponsor && csv.Date && csv.HasContract && csv.ContractDetails && csv.PhoneNumber && csv.MailAddress && csv.MoneyAmount &&
               csv.WhatGoods && csv.GoodsAmount)
                idsAndOptions += "0";
            if (csv.NameOfSponsor)
                idsAndOptions += "1";
            if (csv.Date)
                idsAndOptions += "2";
            if (csv.HasContract)
                idsAndOptions += "3";
            if (csv.ContractDetails)
                idsAndOptions += "4";
            if (csv.PhoneNumber)
                idsAndOptions += "5";
            if (csv.MailAddress)
                idsAndOptions += "6";
            if (csv.MoneyAmount)
                idsAndOptions += "7";
            if (csv.WhatGoods)
                idsAndOptions += "8";
            if (csv.GoodsAmount)
                idsAndOptions += "9";

            return idsAndOptions;
        }

        private string GetHeaderForExcelPrinterSponsor()
        {
            var header = new string[12];
            header[0] = localizer["Ids"];
            header[1] = localizer["NameOfSponsor"];
            header[2] = localizer["Date"];
            header[3] = localizer["MoneyAmount"];
            header[4] = localizer["WhatGoods"];
            header[5] = localizer["GoodsAmount"];
            header[6] = localizer["HasContract"];
            header[7] = localizer["ContractNumberOfRegistration"];
            header[8] = localizer["RegistrationDate"];
            header[9] = localizer["ExpirationDate"];
            header[10] = localizer["PhoneNumber"];
            header[11] = localizer["EmailAddress"];
            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
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

    public class SponsorExporterResponse
    {
        public Dictionary<string, string> Dictionary { get; set; }

        public bool IsValid { get; set; }
        public string FileName { get; set; }

        public SponsorExporterResponse(Dictionary<string, string> dictionary)
        {
            Dictionary = dictionary;
            IsValid = true;
        }
    }

    public class SponsorExporterRequest
    {
        public SponsorExporterRequest(ExportParameters exportProperties)
        {
            ExportParameters = exportProperties;
        }

        public ExportParameters ExportParameters { get; set; }
    }

    public class ExportParameters
    {
        public string StringOfIDs { get; set; }
        public bool All { get; set; }
        public bool NameOfSponsor { get; set; }
        public bool Date { get; set; }
        public bool MoneyAmount { get; set; }
        public bool WhatGoods { get; set; }
        public bool GoodsAmount { get; set; }
        public bool HasContract { get; set; }
        public bool ContractDetails { get; set; }
        public bool PhoneNumber { get; set; }
        public bool MailAddress { get; set; }
        public string FileName { get; set; }
    }
}