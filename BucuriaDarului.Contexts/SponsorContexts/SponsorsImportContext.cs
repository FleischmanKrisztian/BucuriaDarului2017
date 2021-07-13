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
                    response.Message.Add(
                        new KeyValuePair<string, string>("IncorrectFile", "File must be of type Sponsor!"));
                    response.IsValid = false;
                }
                else
                {
                    var sponsorsFromCsv = GetSponsorsFromCsv(result, response);
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
            return headerColumns[1].Contains("Sponsor", StringComparison.InvariantCultureIgnoreCase) || headerColumns[1].Contains("Sponsor", StringComparison.InvariantCultureIgnoreCase);
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
                Sponsorship s = new Sponsorship();
                Contract c = new Contract();
                ContactInformation ci = new ContactInformation();
                try
                {
                    newSponsor.Id = Guid.NewGuid().ToString(); // TODO: check if the Id is already in the database
                    newSponsor.NameOfSponsor = line[0];

                    if (line[1] == null || line[1] == "" || line[1] == "0")
                    {
                        s.Date = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (line[1].Contains("/") == true)
                        {
                            var date = line[1].Split(" ");
                            var finalDate = date[0].Split("/");
                            data = Convert.ToDateTime(finalDate[2] + "-" + finalDate[0] + "-" + finalDate[1]);
                        }
                        else
                        {
                            var anotherDate = line[1].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }
                    }
                    s.MoneyAmount = line[2];
                    s.WhatGoods = line[3];
                    s.GoodsAmount = line[4];

                    newSponsor.Sponsorship = s;
                    if (line[5] == "True" || line[5] == "true")
                    {
                        c.HasContract = true;
                    }
                    else
                    {
                        c.HasContract = false;
                    }
                    c.HasContract = Convert.ToBoolean(line[5]);
                    c.NumberOfRegistration = line[6];
                    
                    if (line[7] == null || line[7] == "" || line[7] == "0")
                    {
                        c.RegistrationDate = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (line[7].Contains("/") == true)
                        {
                            var date = line[7].Split(" ");
                            var finalDate = date[0].Split("/");
                            data = Convert.ToDateTime(finalDate[2] + "-" + finalDate[0] + "-" + finalDate[1]);
                        }
                        else
                        {
                            var anotherDate = line[7].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }
                    }

                    if (line[8] == null || line[8] == "" || line[8] == "0")
                    {
                        c.ExpirationDate = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (line[8].Contains("/") == true)
                        {
                            var date = line[8].Split(" ");
                            var finalDate = date[0].Split("/");
                            data = Convert.ToDateTime(finalDate[2] + "-" + finalDate[0] + "-" + finalDate[1]);
                        }
                        else
                        {
                            var anotherDate = line[8].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }
                    }
                    newSponsor.Contract = c;

                    ci.PhoneNumber = line[9];
                    ci.MailAddress = line[10];
                    newSponsor.ContactInformation = ci;
                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("IncorrectFile", "File must be of Event type!")));
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
