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

        public BeneficiaryImportResponse Execute(Stream dataToImport, string overwrite)
        {
            var response = new BeneficiaryImportResponse();
            if (FileIsNotEmpty(dataToImport))
            {
                response.Message.Add(new KeyValuePair<string, string>("EmptyFile", "File Cannot be Empty!"));
                response.IsValid = false;
            }

            var beenficiariesToUpdate = new List<Beneficiary>();
            var beenficiariesToImport = new List<Beneficiary>();
            var listOfBeneficiaries = dataGateway.GetBenficiariesList();
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
                    beneficiariesFromCsv = GetBeneficiaryFromCsv(result, response, overwrite, listOfBeneficiaries);
                else
                    beneficiariesFromCsv = GetBeneficiaryFromBucuriaDaruluiCSV(result, response, overwrite, listOfBeneficiaries);
                if (response.IsValid)
                {
                    if (overwrite == "yes")
                    {
                        foreach (var b in beneficiariesFromCsv)
                        {
                            if (listOfBeneficiaries.FindAll(x => x.Id == b.Id).Count() != 0 || listOfBeneficiaries.FindAll(x => x.CNP == b.CNP).Count() != 0)
                                beenficiariesToUpdate.Add(b);
                            else
                                beenficiariesToImport.Add(b);
                        }
                        dataGateway.Update(beenficiariesToUpdate);
                        dataGateway.Insert(beenficiariesToImport);
                    }
                    else
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

        public static Beneficiary GetBeneficiaryFromCvsLine(string[] line, Beneficiary beneficiary)
        {
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

            var cI = new CI();

            cI.HasId = Convert.ToBoolean(line[10]);
            cI.Info = line[11];
            if (line[12] != null && line[12] != "")
                cI.ExpirationDate = Convert.ToDateTime(line[12]);
            else
                cI.ExpirationDate = DateTime.MinValue;

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

            if (line[20] != null && line[20] != "")
                personalInfo.Birthdate = Convert.ToDateTime(line[20]);
            else
                personalInfo.Birthdate = DateTime.MinValue;

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

            return beneficiary;
        }

        public static Beneficiary OverwriteBeneficiaryFromCvsLine(string[] line, Beneficiary beneficiary)
        {
            if (line[1] != null && line[1] != "")
                beneficiary.Fullname = line[1];
            if (line[2] != null && line[2] != "")
                beneficiary.Active = Convert.ToBoolean(line[2]);
            else
                beneficiary.Active = true;
            if (line[3] != null && line[3] != "")
                beneficiary.WeeklyPackage = Convert.ToBoolean(line[3]);
            if (line[4] != null && line[4] != "")
                beneficiary.Canteen = Convert.ToBoolean(line[4]);
            if (line[5] != null && line[5] != "")
                beneficiary.HomeDelivery = Convert.ToBoolean(line[5]);
            if (line[6] != null && line[6] != "")
                beneficiary.HomeDeliveryDriver = line[6];
            if (line[7] != null && line[7] != "")
                beneficiary.HasGDPRAgreement = Convert.ToBoolean(line[7]);
            if (line[8] != null && line[8] != "")
                beneficiary.Address = line[8];
            if (line[9] != null && line[9] != "")
                beneficiary.CNP = line[9];

            var cI = new CI();
            if (line[10] != null && line[10] != "")
                cI.HasId = Convert.ToBoolean(line[10]);
            if (line[11] != null && line[11] != "")
                cI.Info = line[11];
            if (line[12] != null && line[12] != "")
                cI.ExpirationDate = Convert.ToDateTime(line[12]);
            else
                cI.ExpirationDate = DateTime.MinValue;

            beneficiary.CI = cI;

            var marca = new Marca();
            if (line[13] != null && line[13] != "")
                marca.MarcaName = line[13];
            if (line[14] != null && line[14] != "")
                marca.IdApplication = line[14];
            if (line[15] != null && line[15] != "")
                marca.IdInvestigation = line[15];
            if (line[16] != null && line[16] != "")
                marca.IdContract = line[16];

            beneficiary.Marca = marca;
            if (line[17] != null && line[17] != "")
                beneficiary.NumberOfPortions = Convert.ToInt16(line[17]);
            if (line[18] != null && line[18] != "")
                beneficiary.LastTimeActive = Convert.ToDateTime(line[18]);
            if (line[19] != null && line[19] != "")
                beneficiary.Comments = line[19];

            var personalInfo = new PersonalInfo();

            if (line[20] != null && line[20] != "")
                personalInfo.Birthdate = Convert.ToDateTime(line[20]);
            else
                personalInfo.Birthdate = DateTime.MinValue;

            if (line[21] != null && line[21] != "")
                personalInfo.PhoneNumber = line[21];

            if (line[22] != null && line[22] != "")
                personalInfo.BirthPlace = line[22];

            if (line[23] != null && line[23] != "")
                personalInfo.Studies = line[23];

            if (line[24] != null && line[24] != "")
                personalInfo.Profession = line[24];

            if (line[25] != null && line[25] != "")
                personalInfo.Occupation = line[25];

            if (line[26] != null && line[26] != "")
                personalInfo.SeniorityInWorkField = line[26];

            if (line[27] != null && line[27] != "")
                personalInfo.HealthState = line[27];

            if (line[28] != null && line[28] != "")
                personalInfo.Disability = line[28];

            if (line[29] != null && line[29] != "")
                personalInfo.ChronicCondition = line[29];

            if (line[30] != null && line[30] != "")
                personalInfo.Addictions = line[30];
            if (line[31] != null && line[31] != "")
                personalInfo.HealthInsurance = Convert.ToBoolean(line[31]);
            if (line[32] != null && line[32] != "")
                personalInfo.HealthCard = Convert.ToBoolean(line[32]);
            if (line[33] != null && line[33] != "")
                personalInfo.Married = line[33];
            if (line[34] != null && line[34] != "")
                personalInfo.SpouseName = line[34];
            if (line[35] != null && line[35] != "")
                personalInfo.HasHome = Convert.ToBoolean(line[35]);
            if (line[36] != null && line[36] != "")
                personalInfo.HousingType = line[36];
            if (line[37] != null && line[37] != "")
                personalInfo.Income = line[37];
            if (line[38] != null && line[38] != "")
                personalInfo.Expenses = line[38];
            if (line[39] != null && line[39] != "")
                personalInfo.Gender = line[39] == "Male" ? Gender.Male : Gender.Female;

            beneficiary.PersonalInfo = personalInfo;

            return beneficiary;
        }

        private static List<Beneficiary> GetBeneficiaryFromCsv(List<string[]> lines, BeneficiaryImportResponse response, string overwrite, List<Beneficiary> listOfBeneficiaries)
        {
            var beneficiaries = new List<Beneficiary>();

            foreach (var line in lines)
            {
                var beneficiary = new Beneficiary();
                try
                {
                    if (overwrite == "no")
                    {
                        if (line[0] != null && line[0] != string.Empty)
                            beneficiary.Id = line[0];
                        else
                            beneficiary.Id = Guid.NewGuid().ToString();
                        beneficiary = GetBeneficiaryFromCvsLine(line, beneficiary);
                    }
                    else
                    {
                        if (listOfBeneficiaries.FindAll(x => x.Id == line[0]).Count != 0)
                        {
                            var databaseBeneficiary = listOfBeneficiaries.Find(x => x.Id == line[0]);
                            beneficiary = OverwriteBeneficiaryFromCvsLine(line, databaseBeneficiary);
                        }
                        else if (listOfBeneficiaries.FindAll(x => x.CNP == line[9]).Count != 0)
                        {
                            var databaseBeneficiary = listOfBeneficiaries.Find(x => x.CNP == line[9]);
                            beneficiary = OverwriteBeneficiaryFromCvsLine(line, databaseBeneficiary);
                        }
                        else
                        {
                            if (line[0] != null && line[0] != string.Empty)
                                beneficiary.Id = line[0];
                            else
                                beneficiary.Id = Guid.NewGuid().ToString();
                            beneficiary = GetBeneficiaryFromCvsLine(line, beneficiary);
                        }
                    }
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

        public static Beneficiary GetBeneficiarFromBucuriaDaruluiCsvLine(string[] line, Beneficiary beneficiary)
        {
            var culture = CultureInfo.CreateSpecificCulture("ro-RO");
            var styles = DateTimeStyles.None;

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
            else
                ci.ExpirationDate = DateTime.MinValue;
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
                personalInfo.Birthdate = DateTime.MinValue;

            beneficiary.Comments = beneficiary.Comments + " Varsta: " + line[39];
            personalInfo.Gender = line[41] == "M" ? Gender.Male : Gender.Female;
            beneficiary.Comments = beneficiary.Comments + " Observatii: " + line[42];

            beneficiary.PersonalInfo = personalInfo;

            return beneficiary;
        }

        //public static List<BeneficiaryContract> BeneficiaryContract(List<string[]> lines, List<Beneficiary> beneficiaries)
        //{
        //    var listOfBeneficiariesContract = new List<BeneficiaryContract>();
        //    var splitedLineForNumberOfContract = new string[2];
        //    var dataSplitedLine = new string[2];
        //    var contract = new BeneficiaryContract();

        //    foreach (var line in lines)
        //    {

        //        if (beneficiaries.FindAll(x => x.CNP == line[9]).Count != 0)
        //        {var beneficiary= beneficiaries.Find(x => x.CNP == line[9]);
        //            if (line[15] != "" && line[15] != null)
        //            {
        //                if (line[15].Contains("/"))
        //                    splitedLineForNumberOfContract = line[15].Split("/");
                       
        //            }
        //            if (line[16] != "" && line[16] != null)
        //            {
        //                if (line[16].Contains("-"))
        //                    dataSplitedLine = line[16].Split("-");
        //            }

        //            if (splitedLineForNumberOfContract.Count() == 2 && dataSplitedLine.Count() == 2)
        //            {
        //                contract.NumberOfRegistration = splitedLineForNumberOfContract[0];
        //                contract.NumberOfPortions = beneficiary.NumberOfPortions;
        //                contract.PhoneNumber = beneficiary.PersonalInfo.PhoneNumber;

                    
        //            }


        //        }
        //    }

        //    return listOfBeneficiariesContract;
        //}

        public static Beneficiary BeneficiaryOverWriteFromBucuriadaruluiCsv(string[] line, Beneficiary beneficiary)
        {
            var culture = CultureInfo.CreateSpecificCulture("ro-RO");
            var styles = DateTimeStyles.None;

            if (line[1] != "" && line[1] != null)
                beneficiary.Fullname = line[1];
            if (line[2] != "" && line[2] != null)
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
            if (line[6] != "" && line[6] != null)
                beneficiary.Comments = beneficiary.Comments + " Sala de mese: " + line[6];
            if (line[7] != "" && line[7] != null)
                beneficiary.HomeDeliveryDriver = line[7];
            if (line[8] != "" && line[8] != null)
                beneficiary.Address = line[8];
            if (line[9] != "" && line[9] != null)
                beneficiary.CNP = line[9];

            var ci = new CI();
            if (line[10] != "" && line[10] != null)
                ci.Info = line[10];

            if (DateTime.TryParse(line[11], culture, styles, out var dateResult))
                ci.ExpirationDate = dateResult;
            else
                ci.ExpirationDate = DateTime.MinValue;
            ci.HasId = true;
            beneficiary.CI = ci;

            var marca = new Marca();
            if (line[12] != "" && line[12] != null)
                marca.MarcaName = line[12];
            if (line[13] != "" && line[13] != null)
                marca.IdApplication = line[13];
            if (line[14] != "" && line[14] != null)
                marca.IdInvestigation = line[14];
            if (line[15] != "" && line[15] != null)
                marca.IdContract = line[15];
            beneficiary.Marca = marca;

            if (Int16.TryParse(line[16], out var result))
                beneficiary.NumberOfPortions = result;
            else
                beneficiary.NumberOfPortions = 0;

            var personalInfo = new PersonalInfo();
            if (line[18] != "" && line[18] != null)
                personalInfo.PhoneNumber = line[18];
            if (line[19] != "" && line[19] != null)
                personalInfo.BirthPlace = line[19];
            if (line[20] != "" && line[20] != null)
                personalInfo.Studies = line[20];
            if (line[21] != "" && line[21] != null)
                personalInfo.Profession = line[21];
            if (line[22] != "" && line[22] != null)
                personalInfo.Occupation = line[22];
            if (line[23] != "" && line[23] != null)
                personalInfo.SeniorityInWorkField = line[23];
            if (line[24] != "" && line[24] != null)
                personalInfo.HealthState = line[24];
            if (line[25] != "" && line[25] != null)
                personalInfo.Disability = line[25];
            if (line[26] != "" && line[26] != null)
                personalInfo.ChronicCondition = line[26];
            if (line[27] != "" && line[27] != null)
                personalInfo.Addictions = line[27];
            if (line[28].ToUpper() == "NU")
                personalInfo.HealthInsurance = false;
            else
                personalInfo.HealthInsurance = true;
            if (line[29].ToUpper() == "NU")
                personalInfo.HealthCard = false;
            else
                personalInfo.HealthCard = true;
            if (line[30] != "" && line[30] != null)
                personalInfo.Married = line[30];
            if (line[31] != "" && line[31] != null)
                personalInfo.SpouseName = line[31];
            if (line[32] != "" && line[32] != null)
                personalInfo.HousingType = line[32];
            if (line[33].ToUpper() == "NU")
                personalInfo.HasHome = true;
            else
                personalInfo.HasHome = false;
            if (line[34] != "" && line[34] != null)
                personalInfo.Income = line[34];
            if (line[35] != "" && line[35] != null)
                personalInfo.Expenses = line[35];

            if (DateTime.TryParse(line[38] + "." + line[37] + "." + line[36], culture, styles, out var dateResult2))
                personalInfo.Birthdate = dateResult2;
            else
                personalInfo.Birthdate = DateTime.MinValue;
            if (line[39] != "" && line[39] != null)
                beneficiary.Comments = beneficiary.Comments + " Varsta: " + line[39];
            personalInfo.Gender = line[41] == "M" ? Gender.Male : Gender.Female;
            beneficiary.Comments = beneficiary.Comments + " Observatii: " + line[42];

            beneficiary.PersonalInfo = personalInfo;

            return beneficiary;
        }

        private List<Beneficiary> GetBeneficiaryFromBucuriaDaruluiCSV(List<string[]> lines, BeneficiaryImportResponse response, string overwrite, List<Beneficiary> listOfBeneficiaries)
        {
            var beneficiaries = new List<Beneficiary>();

            foreach (var line in lines)
            {
                var beneficiary = new Beneficiary();
                try
                {
                    if (overwrite == "no")
                    {
                        beneficiary.Id = Guid.NewGuid().ToString();
                        beneficiary = GetBeneficiarFromBucuriaDaruluiCsvLine(line, beneficiary);
                    }
                    else
                    {
                        if (listOfBeneficiaries.FindAll(x => x.CNP == line[9]).Count != 0)
                        {
                            var databaseBeneficiary = listOfBeneficiaries.Find(x => x.CNP == line[9]);
                            beneficiary = BeneficiaryOverWriteFromBucuriadaruluiCsv(line, databaseBeneficiary);
                        }
                        else
                        {
                            beneficiary.Id = Guid.NewGuid().ToString();
                            beneficiary = GetBeneficiarFromBucuriaDaruluiCsvLine(line, beneficiary);
                        }
                    }
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