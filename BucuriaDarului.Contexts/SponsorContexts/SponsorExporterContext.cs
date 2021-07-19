using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var response = new SponsorExporterResponse(CreateDictionaries(Constants.SPONSORSESSION, Constants.SPONSORHEADER, idsAndFields, header));
            response.IsValid = CheckForProperties(idsAndFields);
            if (request.ExportParameters.FileName != "" && request.ExportParameters.FileName != null)
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
            if (csv.All)
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
            var header = new string[10];
            header[0] = localizer["NameOfSponsor"];
            header[1] = localizer["Date"];
            header[2] = localizer["MoneyAmount"];
            header[3] = localizer["WhatGoods"];
            header[4] = localizer["GoodsAmount"];
            header[5] = localizer["HasContract"];
            header[6] = localizer["ContractDetails"];
            header[7] = localizer["PhoneNumber"];
            header[8] = localizer["EmailAddress"];
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