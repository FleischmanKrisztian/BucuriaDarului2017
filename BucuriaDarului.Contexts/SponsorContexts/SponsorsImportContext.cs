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

        public SponsorImportResponse Execute(Stream dataToImport, string overwrite)
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
                var listOfSponsors = dataGateway.GetSponsors();
                var sponsorsFromCsv = GetSponsorsFromCsv(result, response, overwrite, listOfSponsors);
                var sponsorsToUpdate = new List<Sponsor>();
                var sponsorsToInsert = new List<Sponsor>();

                if (response.IsValid)
                {
                    if (overwrite == "yes")
                    {
                        foreach (var e in sponsorsFromCsv)
                        {
                            if (listOfSponsors.FindAll(x => x.Id == e.Id).Count() != 0)
                                sponsorsToUpdate.Add(e);
                            else
                                sponsorsToInsert.Add(e);
                        }
                        dataGateway.Update(sponsorsToUpdate);
                        dataGateway.Insert(sponsorsToInsert);
                    }
                    else
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

        public static Sponsor GetSponsorFromCsvLine(string[] line, Sponsor newSponsor)
        {
            var sponsorship = new Sponsorship();
            var contract = new Contract();
            var contactInformation = new ContactInformation();
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

            return newSponsor;
        }

        public static Sponsor GetSponsorFromBucuriaDaruluiCsvLine(string[] line, Sponsor newSponsor)
        {
            var sponsorship = new Sponsorship();
            var contract = new Contract();
            var contactInformation = new ContactInformation();
            if (line[1] != "" && line[1] != null)
                newSponsor.NameOfSponsor = line[1];
            if (line[2] != "" && line[2] != null)
                sponsorship.Date = Convert.ToDateTime(line[2]);
            if (line[3] != "" && line[3] != null)
                sponsorship.MoneyAmount = line[3];
            if (line[4] != "" && line[4] != null)
                sponsorship.WhatGoods = line[4];
            if (line[5] != "" && line[5] != null)
                sponsorship.GoodsAmount = line[5];
            if (line[6] != "" && line[6] != null)
                contract.HasContract = Convert.ToBoolean(line[6]);
            if (line[7] != "" && line[7] != null)
                contract.NumberOfRegistration = line[7];
            if (line[8] != "" && line[8] != null)
                contract.RegistrationDate = Convert.ToDateTime(line[8]);
            if (line[9] != "" && line[9] != null)
                contract.ExpirationDate = Convert.ToDateTime(line[9]);
            if (line[10] != "" && line[10] != null)
                contactInformation.PhoneNumber = line[10];
            if (line[11] != "" && line[11] != null)
                contactInformation.MailAddress = line[11];

            newSponsor.Sponsorship = sponsorship;
            newSponsor.ContactInformation = contactInformation;
            newSponsor.Contract = contract;

            return newSponsor;
        }

        private static List<Sponsor> GetSponsorsFromCsv(List<string[]> lines, SponsorImportResponse response, string overwrite, List<Sponsor> listOfSponsors)
        {
            var sponsors = new List<Sponsor>();

            foreach (var line in lines)
            {
                var newSponsor = new Sponsor();

                try
                {
                    if (overwrite == "no")
                    {
                        if (line[0] != null && line[0] != string.Empty)
                            newSponsor.Id = line[0];
                        else
                            newSponsor.Id = Guid.NewGuid().ToString();
                        newSponsor = GetSponsorFromCsvLine(line, newSponsor);
                    }
                    else
                    {
                        if (listOfSponsors.FindAll(x => x.Id == line[0]).Count != 0)
                        {
                            var databaseSponsor = listOfSponsors.Find(x => x.Id == line[0]);
                            newSponsor = GetSponsorFromBucuriaDaruluiCsvLine(line, databaseSponsor);
                        }
                        else
                        {
                            if (line[0] != null && line[0] != string.Empty)
                                newSponsor.Id = line[0];
                            else
                                newSponsor.Id = Guid.NewGuid().ToString();
                            newSponsor = GetSponsorFromCsvLine(line, newSponsor);
                        }
                    }
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