using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorDownloadContext
    {
        private readonly ISponsorDownloadGateway dataGateway;

        public SponsorDownloadContext(ISponsorDownloadGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public string Execute(string dataString, string header)
        {
            var finalHeader = SplitHeader(header);
            string jsonString;
            var properties = GetProperties(dataString);
            var ids = GetIds(dataString);

            if (properties.Contains("0"))
                jsonString = GetSponsorsCsv(ids, finalHeader);
            else
                jsonString = GetSponsors(properties, ids, finalHeader);

            return jsonString;
        }

        public static string[] SplitHeader(string header)
        {
            var splitHeader = header.Split(",");

            return splitHeader;
        }

        public string GetProperties(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var properties = auxiliaryStrings[1];
            return properties;
        }

        public string[] GetIds(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var ids = auxiliaryStrings[0].Split(",");
            return ids;
        }

        public string[] SponsorToArray(Sponsor sponsor)
        {
            var result = new[]
            {
                sponsor.Id,
                sponsor.NameOfSponsor,
                DateTime.Parse(sponsor.Sponsorship.Date.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToShortDateString().ToString(CultureInfo.InvariantCulture),
                sponsor.Sponsorship.MoneyAmount,
                sponsor.Sponsorship.WhatGoods,
                sponsor.Sponsorship.GoodsAmount,
                sponsor.Contract.HasContract.ToString(),
                sponsor.Contract.NumberOfRegistration,
                DateTime.Parse(sponsor.Contract.RegistrationDate.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToShortDateString().ToString(CultureInfo.InvariantCulture),
                DateTime.Parse(sponsor.Contract.ExpirationDate.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToShortDateString().ToString(CultureInfo.InvariantCulture),
                sponsor.ContactInformation.PhoneNumber,
                sponsor.ContactInformation.MailAddress
            };
            return result;
        }

        public string GetSponsorsCsv(string[] ids, string[] finalHeader)
        {
            var listOfSponsors = dataGateway.GetListOfSponsors();
            var finalListOSponsors = new List<Sponsor>();
            foreach (var sponsor in listOfSponsors)
            {
                if (ids.Contains(sponsor.Id))
                {
                    finalListOSponsors.Add(sponsor);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in finalHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }
            sb.Append("\r\n");
            foreach (var item in finalListOSponsors)
            {
                string[] arrSponsor = SponsorToArray(item); ;
                foreach (var data in arrSponsor)
                {
                    sb.Append("\"" + data + "\"" + ",");
                }

                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public string[] GetUsedHeader(string properties, string[] header)
        {
            List<string> headerList = new List<string>();
            if (properties.Contains("1"))
                headerList.Add(header[1]);

            if (properties.Contains("2"))
                headerList.Add(header[2]);

            if (properties.Contains("3"))
                headerList.Add(header[6]);

            if (properties.Contains("4"))
            {
                headerList.Add(header[7]);
                headerList.Add(header[8]);
                headerList.Add(header[9]);
            }

            if (properties.Contains("5"))
                headerList.Add(header[10]);

            if (properties.Contains("6"))
                headerList.Add(header[11]);

            if (properties.Contains("7"))
                headerList.Add(header[3]);

            if (properties.Contains("8"))
                headerList.Add(header[4]);

            if (properties.Contains("9"))
                headerList.Add(header[5]);

            var returnedHeader = headerList.ToArray();
            return returnedHeader;
        }

        public string GetSponsors(string properties, string[] ids, string[] header)
        {
            var usedHeader = GetUsedHeader(properties, header);
            var listOfSponsors = dataGateway.GetListOfSponsors();
            var idListForPrinting = new List<Sponsor>();
            foreach (var sponsor in listOfSponsors)
            {
                if (ids.Contains(sponsor.Id))
                {
                    idListForPrinting.Add(sponsor);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in usedHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }

            sb.Append("\r\n");
            foreach (var item in idListForPrinting)
            {
                string[] arrSponsor = SponsorToArray(item);

                if (properties.Contains("1"))

                    sb.Append("\"" + arrSponsor[1] + "\"" + ",");

                if (properties.Contains("2"))
                    sb.Append("\"" + arrSponsor[2] + "\"" + ",");

                if (properties.Contains("3"))
                    sb.Append("\"" + arrSponsor[6] + "\"" + ",");

                if (properties.Contains("4"))
                    sb.Append("\"" + arrSponsor[7] + "\"" + ",");
                sb.Append("\"" + arrSponsor[8] + "\"" + ",");
                sb.Append("\"" + arrSponsor[9] + "\"" + ",");

                if (properties.Contains("5"))
                    sb.Append("\"" + arrSponsor[10] + "\"" + ",");

                if (properties.Contains("6"))
                    sb.Append("\"" + arrSponsor[11] + "\"" + ",");

                if (properties.Contains("7"))
                    sb.Append("\"" + arrSponsor[3] + "\"" + ",");

                if (properties.Contains("8"))
                    sb.Append("\"" + arrSponsor[4] + "\"" + ",");

                if (properties.Contains("9"))
                    sb.Append("\"" + arrSponsor[5] + "\"" + ",");
            }
            return sb.ToString();
        }
    }
}