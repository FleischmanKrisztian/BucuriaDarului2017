using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorsImportContext
    {
        private readonly ISponsorsImportDataGateway dataGateway;

        public SponsorsImportContext(ISponsorsImportDataGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SponsorImportResponse Execute(Stream dataToImport)
        {
            var response = new SponsorImportResponse();
            if (FileIsNotEmpty(dataToImport))
            {
                response.Message.Add(new KeyValuePair<string, string>("EmptyFile", "File Cannot be Empty!"));
                response.IsValid = false;
            }

            if (response.IsValid)
            {
                var result = ExtractImportRawData(dataToImport);
                if (result[0].Contains("File must be of type Sponsor!"))
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Sponsor!"));
                    response.IsValid = false;
                }
                var sponsorsFromCsv = GetSponsorsFromCsv(result, response);
                if (response.IsValid)
                {
                    dataGateway.Insert(sponsorsFromCsv);
                }
            }

            return response;
        }

        private bool FileIsNotEmpty(Stream dataToImport)
        {
            return dataToImport.Length <= 0;
        }

        private static List<string[]> ExtractImportRawData(Stream dataToImport)
        {
            var result = new List<string[]>();
            var reader = new StreamReader(dataToImport, Encoding.GetEncoding("iso-8859-1"));

            var csvSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var i = 0;
            while (reader.Peek() >= 0)
            {
                if (IsHeader(i))
                {
                    var headerLine = reader.ReadLine();

                    csvSeparator = CsvUtils.DetectSeparator(headerLine);

                    var headerColumns = GetHeaderColumns(headerLine, csvSeparator);

                    if (!IsTheCorrectHeader(headerColumns))
                    {
                        var returnList = new List<string[]>();
                        var strarray = new string[1];
                        strarray[0] = "File must be of type Sponsor!";
                        returnList.Add(strarray);

                        return returnList;
                    }
                }
                else
                {
                    var row = GetCsvRow(reader, csvSeparator);

                    result.Add(row);
                }

                i++;
            }

            return result;
        }

        private static string[] GetCsvRow(StreamReader reader, string csvSeparator)
        {
            var line = reader.ReadLine();
            var splits = new Regex("((?<=\")[^\"]*(?=\"(" + csvSeparator + "|$)+)|(?<=" + csvSeparator + "|^)[^" + csvSeparator + "\"]*(?=" + csvSeparator + "|$))").Matches(line);

            var row = splits.Cast<Match>().Select(match => match.Value).ToArray();
            return row;
        }

        private static bool IsTheCorrectHeader(string[] headerColumns)
        {
            var correct = headerColumns[1].Contains("Name of sponsor", StringComparison.InvariantCultureIgnoreCase) && headerColumns[2].Contains("Date", StringComparison.InvariantCultureIgnoreCase);
            if (correct)
                return correct;
            correct = headerColumns[1].Contains("Numele sponsorului", StringComparison.InvariantCultureIgnoreCase) && headerColumns[2].Contains("Data", StringComparison.InvariantCultureIgnoreCase);
            return correct;
        }

        private static string[] GetHeaderColumns(string headerLine, string csvSeparator)
        {
            var headerColumns = headerLine?.Split(csvSeparator);
            return headerColumns;
        }

        private static bool IsHeader(int i)
        {
            return i == 0;
        }

        private static List<Sponsor> GetSponsorsFromCsv(List<string[]> lines, SponsorImportResponse response)
        {
            var sponsors = new List<Sponsor>();

            foreach (var line in lines)
            {
                var newSponsor = new Sponsor();
                var sponsorship = new Sponsorship();
                var contract = new Contract();
                var contactInformation = new ContactInformation();
                try
                {
                    newSponsor.Id = line[0];
                    newSponsor.NameOfSponsor = line[1];

                    sponsorship.Date = Convert.ToDateTime(line[2]);
                    sponsorship.MoneyAmount = line[3];
                    sponsorship.WhatGoods = line[4];
                    sponsorship.GoodsAmount = line[5];

                    contract.HasContract = Convert.ToBoolean(line[6]);
                    contract.NumberOfRegistration = line[7];
                    contract.RegistrationDate = Convert.ToDateTime(line[8]);
                    contract.ExpirationDate = Convert.ToDateTime(line[9]);

                    contactInformation.PhoneNumber = line[10];
                    contactInformation.MailAddress = line[11];

                    newSponsor.Sponsorship = sponsorship;
                    newSponsor.ContactInformation = contactInformation;
                    newSponsor.Contract = contract;
                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("IncorrectFile", "There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file.")));
                    response.IsValid = false;
                }

                sponsors.Add(newSponsor);
            }
            return sponsors;
        }
    }

    public class SponsorImportResponse
    {
        public bool IsValid { get; set; }

        public List<KeyValuePair<string, string>> Message { get; set; }

        public SponsorImportResponse()
        {
            IsValid = true;
            Message = new List<KeyValuePair<string, string>>();
        }
    }

    public static class CsvUtils
    {
        private static readonly string CsvSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        private static readonly string[] SeparatorChars = { ";", "|", "\t", "," };

        public static string DetectSeparator(string line)
        {
            foreach (var separatorChar in SeparatorChars)
            {
                if (line.Contains(separatorChar, StringComparison.InvariantCulture))
                    return separatorChar;
            }

            return CsvUtils.CsvSeparator;
        }
    }
}