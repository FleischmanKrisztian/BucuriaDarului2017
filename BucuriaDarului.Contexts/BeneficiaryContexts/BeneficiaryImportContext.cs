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

            if (response.IsValid)
            {
                var result = ExtractImportRawData(dataToImport);
                var stringArray = result[0];
                if (stringArray[0].Contains("File must be of type"))
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Beneficiary!"));
                    response.IsValid = false;
                }
                else
                {
                    var beneficiariesFromCsv = GetBeneficiaryFromCsv(result, response);
                    dataGateway.Insert(beneficiariesFromCsv);
                }
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
                        var strArray = new string[1];
                        strArray[0] = "File must be of type Beneficiary!";
                        returnList.Add(strArray);

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
            return headerColumns[1].Contains("Fullname", StringComparison.InvariantCultureIgnoreCase) && headerColumns[2].Contains("Active", StringComparison.InvariantCultureIgnoreCase);
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
                    beneficiary.Id = line[0];
                    beneficiary.Fullname = line[1];
                    beneficiary.Active = Convert.ToBoolean(line[2]);
                    beneficiary.WeeklyPackage = Convert.ToBoolean(line[3]);
                    beneficiary.Canteen = Convert.ToBoolean(line[4]);
                    beneficiary.HomeDelivery = Convert.ToBoolean(line[5]);
                    beneficiary.HomeDeliveryDriver = line[6];
                    beneficiary.HasGDPRAgreement = Convert.ToBoolean(line[7]);
                    beneficiary.Address = line[8];
                    beneficiary.CNP = line[9];

                    var cI = new CI
                    {
                        HasId = Convert.ToBoolean(line[10]),
                        Info = line[11],
                        ExpirationDate = Convert.ToDateTime(line[12])
                    };

                    beneficiary.CI = cI;

                    var marca = new Marca
                    {
                        MarcaName = line[13],
                        IdApplication = line[14],
                        IdInvestigation = line[15],
                        IdContract = line[16]
                    };

                    beneficiary.Marca = marca;

                    beneficiary.NumberOfPortions = Convert.ToInt16(line[17]);
                    beneficiary.LastTimeActive = Convert.ToDateTime(line[18]);
                    beneficiary.Comments = line[19];

                    var personalInfo = new PersonalInfo
                    {
                        Birthdate = Convert.ToDateTime(line[20]),
                        PhoneNumber = line[21],
                        BirthPlace = line[22],
                        Studies = line[23],
                        Profession = line[24],
                        Occupation = line[25],
                        SeniorityInWorkField = line[26],
                        HealthState = line[27],
                        Disability = line[28],
                        ChronicCondition = line[29],
                        Addictions = line[30],
                        HealthInsurance = Convert.ToBoolean(line[31]),
                        HealthCard = Convert.ToBoolean(line[32]),
                        Married = line[33],
                        SpouseName = line[34],
                        HasHome = Convert.ToBoolean(line[35]),
                        HousingType = line[36],
                        Income = line[37],
                        Expenses = line[38]
                    };

                    var gender = Convert.ToInt16(line[39]) == 0 ? Gender.Male : Gender.Female;
                    personalInfo.Gender = gender;

                    beneficiary.PersonalInfo = personalInfo;
                    //beneficiary.Image = Convert.ToByte(line[40]); FOR IMAGE
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