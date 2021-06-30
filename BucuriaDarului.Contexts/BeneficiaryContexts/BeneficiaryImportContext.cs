using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
    public class BeneficiaryImportContext
    {
        private readonly IBeneficiaryImportGateway dataGateway;

        public BeneficiaryImportContext(IBeneficiaryImportGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryImportResponse Execute(Stream dataToImport)
        {
            var response = new BeneficiaryImportResponse();
            if (FileIsNotEmpty(dataToImport))
            {
                response.Message.Add(new KeyValuePair<string, string>("EmptyFile", "File Cannot be Empty!"));
                response.IsValid = false;
            }

            if (!IsTheCorrectHeader(GetHeaderColumns(dataToImport)))
            {
                response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Beneficiary!"));
                response.IsValid = false;
            }

            if (response.IsValid)
            {
                var result = ExtractImportRawData(dataToImport);
                var beneficiariesFromCsv = GetBeneficiaryFromCsv(result, response);
                dataGateway.Insert(beneficiariesFromCsv);
            }

            return response;
        }

        private string[] GetHeaderColumns(Stream dataToImport)
        {
            using var reader = new StreamReader(dataToImport, Encoding.GetEncoding("iso-8859-1"));
            var headerLine = reader.ReadLine();

            var csvSeparator = CsvUtils.DetectSeparator(headerLine);

            var headerColumns = GetHeaderColumns(headerLine, csvSeparator);

            return headerColumns;
        }

        private bool FileIsNotEmpty(Stream dataToimport)
        {
            if (dataToimport.Length > 0)
                return false;
            return true;
        }

        private static List<string[]> ExtractImportRawData(Stream dataToImport)
        {
            List<string[]> result = new List<string[]>();
            using var reader = new StreamReader(dataToImport, Encoding.GetEncoding("iso-8859-1"));

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
                        return new List<string[]>();
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
            return headerColumns[0].Contains("Fullname", StringComparison.InvariantCultureIgnoreCase) && headerColumns[1].Contains("Active", StringComparison.InvariantCultureIgnoreCase);
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

        private static List<Beneficiary> GetBeneficiaryFromCsv(List<string[]> lines, BeneficiaryImportResponse response)
        {
            var beneficiaries = new List<Beneficiary>();

            foreach (var line in lines)
            {
                var beneficiary = new Beneficiary();
                try
                {
                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("IncorrectFile", "File must be of Beneficiary type!")));
                    response.IsValid = false;
                }
                beneficiaries.Add(beneficiary);
            }

            return beneficiaries;
        }
    }

    public class BeneficiaryImportResponse
    {
        public bool IsValid { get; set; }

        public List<KeyValuePair<string, string>> Message { get; set; }

        public BeneficiaryImportResponse()
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