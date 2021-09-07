using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using ExcelDataReader;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerImportContext
    {
        private readonly IVolunteerImportGateway dataGateway;
        private static int _fileType = 0;
        private readonly IStringLocalizer localizer;

        public VolunteerImportContext(IVolunteerImportGateway dataGateway, IStringLocalizer localizer)
        {
            this.localizer = localizer;
            this.dataGateway = dataGateway;
        }

        public VolunteerImportResponse Execute(Stream dataToImport, string overwrite)
        {
            var lines = ReadMappingTemplate();
            var listOfColumns = GetListOfColumns(lines);

            var response = new VolunteerImportResponse();
            if (FileIsNotEmpty(dataToImport))
            {
                response.Message.Add(new KeyValuePair<string, string>("EmptyFile", @localizer["File Cannot be Empty!"]));
                response.IsValid = false;
            }

            if (response.IsValid)
            {
                var volunteersToUpdate = new List<Volunteer>();
                var volunteersToInsert = new List<Volunteer>();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                var reader = ExcelReaderFactory.CreateReader((dataToImport));

                var listVolunteer = new List<Volunteer>();
                while (reader.Read()) //Each row of the file
                {
                    //var list = new int[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

                    listVolunteer.Add(new Volunteer
                    {
                        Fullname = reader.GetValue(1).ToString(),
                        CNP = reader.GetValue(2).ToString(),
                        Address = reader.GetValue(3).ToString(),
                       // CI = new CI { Info = reader.GetValue(4).ToString() },
                       // HourCount = Convert.ToInt32(reader.GetValue(8)),
                       // ContactInformation=new ContactInformation { PhoneNumber= reader.GetValue(9).ToString() ,
                       //     MailAddress= reader.GetValue(10).ToString() },
                       // FieldOfActivity =  reader.GetValue(11).ToString(),
                       //AdditionalInfo=new AdditionalInfo { Remark= reader.GetValue(12).ToString() }
                    });
                };

                    //var listOfVolunteers = dataGateway.GetVolunteersList();
                    //var result = ExtractImportRawData(dataToImport, listOfColumns, localizer);
                    //var volunteersFromCsv = new List<Volunteer>();
                    //if (_fileType == 0)
                    //{
                    //    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", @localizer["File must be of type Volunteer!"]));
                    //    response.IsValid = false;
                    //}
                    //else
                    //    volunteersFromCsv = GetVolunteerFromCsv(result, response, localizer, overwrite, listOfVolunteers);

                    //if (response.IsValid)
                    //{
                    //    if (overwrite == "yes")
                    //    {
                    //        foreach (var v in volunteersFromCsv)
                    //        {
                    //            if (listOfVolunteers.FindAll(x => x.Id == v.Id).Count() != 0 || listOfVolunteers.FindAll(x => x.CNP == v.CNP).Count() != 0)
                    //                volunteersToUpdate.Add(v);
                    //            else
                    //                volunteersToInsert.Add(v);
                    //        }
                    //        dataGateway.UpdateVolunteers(volunteersToUpdate);
                    //        dataGateway.Insert(volunteersToInsert);
                    //    }
                    //    else
                    //        dataGateway.Insert(volunteersFromCsv);
                    //}
                }

                return response;
            }

            private bool FileIsNotEmpty(Stream dataToImport)
            {
                return dataToImport.Length <= 0;
            }

            public string[] ReadMappingTemplate()
            {
                var lines = new string[30];
                var path = Environment.GetEnvironmentVariable(Constants.PATH_TO_VOLUNTEER_EXCEL_MAPPING_FILE);
                int counter = 0; string line;
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    lines[counter] = line;
                    counter++;
                }
                file.Close();

                return lines;
            }

            public List<KeyValuePair<string, string>> GetListOfColumns(string[] lines)
            {
                var listOfKeyValuePairs = new List<KeyValuePair<string, string>>();
                foreach (var line in lines)
                {
                    if (line != "" && line != null)
                    {
                        var splitLine = line.Split("=");
                        listOfKeyValuePairs.Add(new KeyValuePair<string, string>(splitLine[0], splitLine[1]));
                    }
                }

                return listOfKeyValuePairs;
            }

            private static List<string[]> ExtractImportRawData(Stream dataToImport, List<KeyValuePair<string, string>> listOfColumns, IStringLocalizer localizer)
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
                        _fileType = IsTheCorrectHeader(headerColumns, listOfColumns) + IsTheCorrectHeaderForTheirCsv(headerColumns);

                        var mapping = MapProperties(headerColumns);
                        var list = GetColumnsOrder(mapping, listOfColumns);
                        if (_fileType == 0)
                        {
                            var returnList = new List<string[]>();
                            var strArray = new string[1];
                            strArray[0] = @localizer["The File Does Not have the correct header!"];
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
                var splits = new Regex("((?<=\")[^\"]*(?=\"(" + csvSeparator + "|$)+)|(?<=" + csvSeparator + "|^)[^" +
                                       csvSeparator + "\"]*(?=" + csvSeparator + "|$))").Matches(line);
                var row = splits.Cast<Match>().Select(match => match.Value).ToArray();
                return row;
            }

            private static int IsTheCorrectHeader(string[] headerColumns, List<KeyValuePair<string, string>> listOfColumns)
            {
                var filter = listOfColumns.FirstOrDefault(kvp => kvp.Key == "Activity");

                foreach (var column in headerColumns)
                {
                    if (column.Contains(filter.Value))
                        return 1;
                }
                return 0;
            }

            private static int IsTheCorrectHeaderForTheirCsv(string[] headerColumns)
            {
                var differentCSV =
                    headerColumns[0].Contains("prenume", StringComparison.InvariantCultureIgnoreCase) &&
                    headerColumns[1].Contains("CNP", StringComparison.InvariantCultureIgnoreCase);
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

            public static List<KeyValuePair<int, string>> MapProperties(string[] header)
            {
                var columnsMappingList = new List<KeyValuePair<int, string>>();
                int counter = 0;
                foreach (var column in header)
                {
                    columnsMappingList.Add(new KeyValuePair<int, string>(counter, column));
                    counter++;
                }

                return columnsMappingList;
            }

            public static int[] GetColumnsOrder(List<KeyValuePair<int, string>> columnsMappingList, List<KeyValuePair<string, string>> listOfColumns)
            {
                var list = new int[30];
                int count = 0;
                foreach (var column in listOfColumns)
                {
                    var result = columnsMappingList.FirstOrDefault(x => x.Value == column.Value);
                    list[count] = result.Key;
                    count++;
                }

                return list;
            }
            private static Volunteer GetDataFromCSVLine(string[] line, Volunteer volunteer)
            {
                volunteer.Fullname = line[1];
                if (line[2] != null && line[2] != "")
                    volunteer.Birthdate = Convert.ToDateTime(line[2]);
                else
                    volunteer.Birthdate = DateTime.MinValue;

                volunteer.Address = line[3];
                if (line[4] == null && line[4] == "")
                    volunteer.Gender = Gender.NotSpecified;
                else if (line[4] == "M" || line[4] == "Male" || line[4] == "Masculin")
                    volunteer.Gender = Gender.Male;
                else if (line[4] == "F" || line[4] == "Female" || line[4] == "Feminin")
                    volunteer.Gender = Gender.Female;
                volunteer.DesiredWorkplace = line[5];
                volunteer.CNP = line[6];
                volunteer.FieldOfActivity = line[7];
                volunteer.Occupation = line[8];

                var cI = new CI();

                cI.HasId = Convert.ToBoolean(line[9]);
                cI.Info = line[10];
                if (line[11] != null && line[11] != "")
                    cI.ExpirationDate = Convert.ToDateTime(line[11]);
                else
                    cI.ExpirationDate = DateTime.MinValue;

                volunteer.CI = cI;
                if (line[12] != null && line[12] != "")
                    volunteer.InActivity = Convert.ToBoolean(line[12]);
                else
                    volunteer.InActivity = true;
                volunteer.HourCount = Convert.ToInt16(line[13]);

                var contactInformation = new ContactInformation
                {
                    PhoneNumber = line[14],
                    MailAddress = line[15]
                };
                volunteer.ContactInformation = contactInformation;

                var additionalInformation = new AdditionalInfo
                {
                    HasDrivingLicense = Convert.ToBoolean(line[16]),
                    HasCar = Convert.ToBoolean(line[17]),
                    Remark = line[18]
                };

                volunteer.AdditionalInfo = additionalInformation;

                return volunteer;
            }

            private static Volunteer VolunteerOverWriteFromCsv(string[] line, Volunteer volunteer)
            {
                if (line[1] != "" && line[1] != null)
                    volunteer.Fullname = line[1];
                if (line[2] != null && line[2] != "")
                    volunteer.Birthdate = Convert.ToDateTime(line[2]);
                else
                    volunteer.Birthdate = DateTime.MinValue;
                if (line[3] != "" && line[3] != null)
                    volunteer.Address = line[3];

                if (line[4] == null && line[4] == "")
                    volunteer.Gender = Gender.NotSpecified;
                else if (line[4] == "M" || line[4] == "Male" || line[4] == "Masculin")
                    volunteer.Gender = Gender.Male;
                else if (line[4] == "F" || line[4] == "Female" || line[4] == "Feminin")
                    volunteer.Gender = Gender.Female;

                if (line[5] != "" && line[5] != null)
                    volunteer.DesiredWorkplace = line[5];
                if (line[6] != "" && line[6] != null)
                    volunteer.CNP = line[6];
                if (line[7] != "" && line[7] != null)
                    volunteer.FieldOfActivity = line[7];
                if (line[8] != "" && line[8] != null)
                    volunteer.Occupation = line[8];

                var cI = new CI();
                if (line[9] != "" && line[9] != null)
                    cI.HasId = Convert.ToBoolean(line[9]);
                if (line[10] != "" && line[10] != null)
                    cI.Info = line[10];
                if (line[11] != null && line[11] != "")
                    cI.ExpirationDate = Convert.ToDateTime(line[11]);
                else
                    cI.ExpirationDate = DateTime.MinValue;

                volunteer.CI = cI;
                if (line[12] != null && line[12] != "")
                    volunteer.InActivity = Convert.ToBoolean(line[12]);
                else
                    volunteer.InActivity = true;
                if (line[13] != "" && line[13] != null)
                    volunteer.HourCount = Convert.ToInt16(line[13]);

                var contactInformation = new ContactInformation();
                if (line[14] != "" && line[14] != null)
                    contactInformation.PhoneNumber = line[14];
                if (line[15] != "" && line[15] != null)
                    contactInformation.MailAddress = line[15];

                volunteer.ContactInformation = contactInformation;

                var additionalInformation = new AdditionalInfo();
                if (line[16] != "" && line[16] != null)
                    additionalInformation.HasDrivingLicense = Convert.ToBoolean(line[16]);
                if (line[17] != "" && line[17] != null)
                    additionalInformation.HasCar = Convert.ToBoolean(line[17]);
                if (line[18] != "" && line[18] != null)
                    additionalInformation.Remark = line[18];

                volunteer.AdditionalInfo = additionalInformation;

                return volunteer;
            }

            private static List<Volunteer> GetVolunteerFromCsv(List<string[]> lines, VolunteerImportResponse response, IStringLocalizer localizer, string overwrite, List<Volunteer> listOfVolunteers)
            {
                var volunteers = new List<Volunteer>();

                foreach (var line in lines)
                {
                    var volunteer = new Volunteer();

                    try
                    {
                        if (overwrite == "no")
                        {
                            if (line[0] != null && line[0] != string.Empty)
                                volunteer.Id = line[0];
                            else
                                volunteer.Id = Guid.NewGuid().ToString();
                            volunteer = GetDataFromCSVLine(line, volunteer);
                        }
                        else
                        {
                            if (listOfVolunteers.FindAll(x => x.Id == line[0]).Count != 0)
                            {
                                var databaseVolunteer = listOfVolunteers.Find(x => x.Id == line[0]);
                                volunteer = VolunteerOverWriteFromCsv(line, databaseVolunteer);
                            }
                            else if (listOfVolunteers.FindAll(x => x.CNP == line[6]).Count != 0)
                            {
                                var databaseVolunteer = listOfVolunteers.Find(x => x.CNP == line[6]);
                                volunteer = VolunteerOverWriteFromCsv(line, databaseVolunteer);
                            }
                            else
                            {
                                if (line[0] != null && line[0] != string.Empty)
                                    volunteer.Id = line[0];
                                else
                                    volunteer.Id = Guid.NewGuid().ToString();
                                volunteer = GetDataFromCSVLine(line, volunteer);
                            }
                        }
                    }
                    catch
                    {
                        response.Message.Add(new KeyValuePair<string, string>("IncorrectFile",
                            @localizer["There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file."]));
                        response.IsValid = false;
                    }

                    volunteers.Add(volunteer);
                }

                return volunteers;
            }

            private static Volunteer GetDataFromBucuriaDaruluiCSVLine(string[] line, Volunteer volunteer)
            {
                var additionalInformation = new AdditionalInfo
                {
                    HasCar = false,
                    HasDrivingLicense = false,
                    Remark = ""
                };

                var contactInformation = new ContactInformation
                {
                    PhoneNumber = line[4],
                    MailAddress = ""
                };

                var ci = new CI
                {
                    HasId = true,
                    Info = line[3],
                    ExpirationDate = DateTime.MinValue
                };

                volunteer.Id = Guid.NewGuid().ToString();

                volunteer.Fullname = line[0];
                volunteer.CNP = line[1];
                volunteer.Address = line[2];
                volunteer.Occupation = line[8];
                volunteer.DesiredWorkplace = line[9];
                volunteer.Birthdate = DateTime.MinValue;
                volunteer.FieldOfActivity = "";
                if (volunteer.CNP != null && volunteer.CNP != "")
                {
                    if (volunteer.CNP.StartsWith("1") || volunteer.CNP.StartsWith("3"))
                        volunteer.Gender = Gender.Male;
                    else if (volunteer.CNP.StartsWith("2") || volunteer.CNP.StartsWith("4"))
                        volunteer.Gender = Gender.Female;
                }
                else
                    volunteer.Gender = Gender.NotSpecified;
                volunteer.HourCount = 0;
                volunteer.InActivity = true;
                volunteer.CI = ci;
                volunteer.ContactInformation = contactInformation;
                volunteer.AdditionalInfo = additionalInformation;

                return volunteer;
            }

            private static Volunteer VolunteerOverWriteFromBucuriadaruluiCsv(string[] line, Volunteer volunteer)
            {
                var additionalInformation = new AdditionalInfo();

                additionalInformation.HasCar = false;
                additionalInformation.HasDrivingLicense = false;
                additionalInformation.Remark = "";

                var contactInformation = new ContactInformation();
                if (line[4] != "" & line[4] != null)
                    contactInformation.PhoneNumber = line[4];
                contactInformation.MailAddress = "";

                var ci = new CI();
                ci.HasId = true;
                if (line[3] != "" & line[3] != null)
                    ci.Info = line[3];
                ci.ExpirationDate = DateTime.MinValue;
                if (line[0] != "" & line[0] != null)
                    volunteer.Fullname = line[0];
                if (line[1] != "" & line[1] != null)
                    volunteer.CNP = line[1];
                if (line[2] != "" & line[2] != null)
                    volunteer.Address = line[2];
                if (line[8] != "" & line[8] != null)
                    volunteer.Occupation = line[8];
                if (line[9] != "" & line[9] != null)
                    volunteer.DesiredWorkplace = line[9];
                volunteer.Birthdate = DateTime.MinValue;
                volunteer.FieldOfActivity = "";
                if (volunteer.CNP != null && volunteer.CNP != "")
                {
                    if (volunteer.CNP.StartsWith("1") || volunteer.CNP.StartsWith("3"))
                        volunteer.Gender = Gender.Male;
                    else if (volunteer.CNP.StartsWith("2") || volunteer.CNP.StartsWith("4"))
                        volunteer.Gender = Gender.Female;
                }
                else
                    volunteer.Gender = Gender.NotSpecified;
                volunteer.HourCount = 0;
                volunteer.InActivity = true;
                volunteer.CI = ci;
                volunteer.ContactInformation = contactInformation;
                volunteer.AdditionalInfo = additionalInformation;

                return volunteer;
            }

            private List<Volunteer> GetVolunteerFromBucuriaDaruluiCSV(List<string[]> lines,
                VolunteerImportResponse response, IStringLocalizer localizer, string overwrite, List<Volunteer> listOfVolunteers)
            {
                var volunteers = new List<Volunteer>();

                foreach (var line in lines)
                {
                    var volunteer = new Volunteer();
                    try
                    {
                        if (overwrite == "no")
                        {
                            if (line[0] != null && line[0] != string.Empty)
                                volunteer.Id = line[0];
                            else
                                volunteer.Id = Guid.NewGuid().ToString();
                            volunteer = GetDataFromBucuriaDaruluiCSVLine(line, volunteer);
                        }
                        else
                        {
                            if (listOfVolunteers.FindAll(x => x.CNP == line[1]).Count != 0)
                            {
                                var databaseVolunteer = listOfVolunteers.Find(x => x.CNP == line[1]);
                                volunteer = VolunteerOverWriteFromBucuriadaruluiCsv(line, databaseVolunteer);
                            }
                            else
                            {
                                volunteer = GetDataFromBucuriaDaruluiCSVLine(line, volunteer);
                            }
                        }
                    }
                    catch
                    {
                        response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", @localizer["There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file."]));
                        response.IsValid = false;
                    }

                    volunteers.Add(volunteer);
                }

                return volunteers;
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

    public class VolunteerImportResponse
    {
        public bool IsValid { get; set; }

        public List<KeyValuePair<string, string>> Message { get; set; }

        public VolunteerImportResponse()
        {
            IsValid = true;
            Message = new List<KeyValuePair<string, string>>();
        }
    }
}