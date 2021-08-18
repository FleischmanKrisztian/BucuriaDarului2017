using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
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
        private static int _fileType = 0;

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
                var beneficiariesFromCsv = new List<Beneficiary>();
                if (_fileType == 0)
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Beneficiary!"));
                    response.IsValid = false;
                }
                else if (_fileType == 1)
                    beneficiariesFromCsv = GetBeneficiaryFromCsv(result, response);
                else
                    beneficiariesFromCsv = GetBeneficiaryFromBucuriaDaruluiCSV(result, response);
                if (response.IsValid)
                {
                    dataGateway.Insert(beneficiariesFromCsv);
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

                    _fileType = IsTheCorrectHeader(headerColumns) + IsTheCorrectHeaderForTheirCsv(headerColumns);

                    if (_fileType == 0)
                    {
                        var returnList = new List<string[]>();
                        var strArray = new string[1];
                        strArray[0] = "The File Does Not have the correct header!";
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

        private static int IsTheCorrectHeader(string[] headerColumns)
        {
            var correct = headerColumns[1].Contains("Fullname", StringComparison.InvariantCultureIgnoreCase) &&
                          headerColumns[2].Contains("Active", StringComparison.InvariantCultureIgnoreCase);
            if (correct)
                return 1;

            correct = headerColumns[1].Contains("prenume", StringComparison.InvariantCultureIgnoreCase) &&
                      headerColumns[2].Contains("Activ", StringComparison.InvariantCultureIgnoreCase);
            if (correct)
                return 1;

            return 0;
        }

        private static int IsTheCorrectHeaderForTheirCsv(string[] headerColumns)
        {
            var differentCSV =
                headerColumns[0].Contains("Nr. Crt", StringComparison.InvariantCultureIgnoreCase) &&
                headerColumns[1].Contains("prenume", StringComparison.InvariantCultureIgnoreCase);
            if (differentCSV)
                return 2;

            return 0;
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
                    if (line[0] != null && line[0] != string.Empty)
                        beneficiary.Id = line[0];
                    else
                        beneficiary.Id = Guid.NewGuid().ToString();
                    beneficiary.Fullname = line[1];
                    if (line[2] != null && line[2] != "")
                        beneficiary.Active = Convert.ToBoolean(line[2]);
                    else
                        beneficiary.Active = true;
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

                    var personalInfo = new PersonalInfo();

                    personalInfo.Birthdate = Convert.ToDateTime(line[20]);
                    personalInfo.PhoneNumber = line[21];
                    personalInfo.BirthPlace = line[22];
                    personalInfo.Studies = line[23];
                    personalInfo.Profession = line[24];
                    personalInfo.Occupation = line[25];
                    personalInfo.SeniorityInWorkField = line[26];
                    personalInfo.HealthState = line[27];
                    personalInfo.Disability = line[28];
                    personalInfo.ChronicCondition = line[29];
                    personalInfo.Addictions = line[30];
                    personalInfo.HealthInsurance = Convert.ToBoolean(line[31]);
                    personalInfo.HealthCard = Convert.ToBoolean(line[32]);
                    personalInfo.Married = line[33];
                    personalInfo.SpouseName = line[34];
                    personalInfo.HasHome = Convert.ToBoolean(line[35]);
                    personalInfo.HousingType = line[36];
                    personalInfo.Income = line[37];
                    personalInfo.Expenses = line[38];
                    personalInfo.Gender = line[39] == "Male" ? Gender.Male : Gender.Female;

                    beneficiary.PersonalInfo = personalInfo;
                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("IncorrectFile", "There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file.")));
                    response.IsValid = false;
                }
                beneficiaries.Add(beneficiary);
            }

            return beneficiaries;
        }

        private List<Beneficiary> GetBeneficiaryFromBucuriaDaruluiCSV(List<string[]> lines, BeneficiaryImportResponse response)
        {
            var beneficiaries = new List<Beneficiary>();
            var culture = CultureInfo.CreateSpecificCulture("ro-RO");
            var styles = DateTimeStyles.None;

            foreach (var line in lines)
            {
                var beneficiary = new Beneficiary();
                try
                {
                    beneficiary.Id = Guid.NewGuid().ToString();
                    beneficiary.Fullname = line[1];
                    beneficiary.Comments = "Cap Familie: " + line[2];
                    if (line[3].ToLower() == "activ")
                        beneficiary.Active = true;
                    else
                        beneficiary.Active = false;
                    if (line[4].ToUpper() == "NU")
                        beneficiary.Canteen = true;
                    else
                        beneficiary.Canteen = false;
                    if (line[5].ToUpper() == "NU")
                        beneficiary.WeeklyPackage = false;
                    else
                        beneficiary.Canteen = true;
                    beneficiary.Comments = beneficiary.Comments + " Sala de mese: " + line[6];
                    beneficiary.HomeDeliveryDriver = line[7];
                    beneficiary.Address = line[8];
                    beneficiary.CNP = line[9];

                    var ci = new CI();
                    ci.Info = line[10];

                    if (DateTime.TryParse(line[11], culture, styles, out var dateResult))
                        ci.ExpirationDate = dateResult;
                    //else
                    //    ci.ExpirationDate = DateTime.Today;
                    ci.HasId = true;
                    beneficiary.CI = ci;

                    var marca = new Marca();
                    marca.MarcaName = line[12];
                    marca.IdApplication = line[13];
                    marca.IdInvestigation = line[14];
                    marca.IdContract = line[15];
                    beneficiary.Marca = marca;

                    if (Int16.TryParse(line[16], out var result))
                        beneficiary.NumberOfPortions = result;
                    else
                        beneficiary.NumberOfPortions = 0;

                    var personalInfo = new PersonalInfo();

                    personalInfo.PhoneNumber = line[18];
                    personalInfo.BirthPlace = line[19];
                    personalInfo.Studies = line[20];
                    personalInfo.Profession = line[21];
                    personalInfo.Occupation = line[22];
                    personalInfo.SeniorityInWorkField = line[23];
                    personalInfo.HealthState = line[24];
                    personalInfo.Disability = line[25];
                    personalInfo.ChronicCondition = line[26];
                    personalInfo.Addictions = line[27];
                    if (line[28].ToUpper() == "NU")
                        personalInfo.HealthInsurance = false;
                    else
                        personalInfo.HealthInsurance = true;
                    if (line[29].ToUpper() == "NU")
                        personalInfo.HealthCard = false;
                    else
                        personalInfo.HealthCard = true;
                    personalInfo.Married = line[30];
                    personalInfo.SpouseName = line[31];

                    personalInfo.HousingType = line[32];
                    if (line[33].ToUpper() == "NU")
                        personalInfo.HasHome = true;
                    else
                        personalInfo.HasHome = false;
                    personalInfo.Income = line[34];
                    personalInfo.Expenses = line[35];

                    if (DateTime.TryParse(line[38] + "." + line[37] + "." + line[36], culture, styles, out var dateResult2))
                        personalInfo.Birthdate = dateResult2;
                    else
                        personalInfo.Birthdate = DateTime.Today;

                    beneficiary.Comments = beneficiary.Comments + " Varsta: " + line[39];
                    personalInfo.Gender = line[41] == "M" ? Gender.Male : Gender.Female;
                    beneficiary.Comments = beneficiary.Comments + " Observatii: " + line[42];

                    beneficiary.PersonalInfo = personalInfo;
                }
                catch
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file."));
                    response.IsValid = false;
                    break;
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