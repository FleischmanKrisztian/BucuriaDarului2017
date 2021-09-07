using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using ExcelDataReader;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerImportContext
    {
        private readonly IVolunteerImportGateway dataGateway;
        private static int _fileType = 0;
        private readonly IStringLocalizer localizer;
        private static List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

        public VolunteerImportContext(IVolunteerImportGateway dataGateway, IStringLocalizer localizer)
        {
            this.localizer = localizer;
            this.dataGateway = dataGateway;
        }

        public VolunteerImportResponse Execute(Stream dataToImport, string overwrite)
        {
            var lines = ReadMappingTemplate();
            var listOfColumns = GetListOfColumnsTemplate(lines);

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

                var listOfVolunteers = dataGateway.GetVolunteersList();
                var result = ExtractImportRawData(dataToImport, listOfColumns, localizer);
                var volunteersFromCsv = new List<Volunteer>();
                if (_fileType == 0)
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", @localizer["File must be of type Volunteer!"]));
                    response.IsValid = false;
                }
                else
                    volunteersFromCsv = GetVolunteerFromCsv(result, response, localizer, overwrite, listOfVolunteers);

                if (response.IsValid)
                {
                    if (overwrite == "yes")
                    {
                        foreach (var v in volunteersFromCsv)
                        {
                            if (listOfVolunteers.FindAll(x => x.Id == v.Id).Count() != 0 || listOfVolunteers.FindAll(x => x.CNP == v.CNP).Count() != 0)
                                volunteersToUpdate.Add(v);
                            else
                                volunteersToInsert.Add(v);
                        }
                        dataGateway.UpdateVolunteers(volunteersToUpdate);
                        dataGateway.Insert(volunteersToInsert);
                    }
                    else
                        dataGateway.Insert(volunteersFromCsv);
                }
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

        public List<KeyValuePair<string, string>> GetListOfColumnsTemplate(string[] lines)
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

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var reader = ExcelReaderFactory.CreateReader(dataToImport);
            int numberOfColumns = 0;
            var i = 0;
            while (reader.Read())
            {
                if (IsHeader(i))
                {
                    int j = 0;
                    var headerLine = new string[30];
                    while (reader.GetValue(j) != null)
                    {
                        headerLine[j] = reader.GetValue(j).ToString();
                        j++;
                    }

                    numberOfColumns = CountColumns(headerLine);

                    _fileType = IsTheCorrectHeader(headerLine, listOfColumns);

                    var mapping = MapPropertiesFromExcel(headerLine);
                    list = GetColumnsOrder(mapping, listOfColumns);
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
                    var row = GetCsvRow(reader, numberOfColumns);
                    result.Add(row);
                }

                i++;
            }

            return result;
        }

        private static string[] GetCsvRow(IExcelDataReader reader, int numberOfColumns)
        {
            var list = new string[numberOfColumns];
            int j = 0;
            for (j = 0; j < numberOfColumns; j++)
            {
                if (reader.GetValue(j) != null)

                    list[j] = reader.GetValue(j).ToString();
                else
                    list[j] = "";
            }
            var row = list;
            return row;
        }

        private static int IsTheCorrectHeader(string[] headerColumns, List<KeyValuePair<string, string>> listOfColumns)
        {
            var filter = listOfColumns.FirstOrDefault(kvp => kvp.Key == "DesiredWorkplace");

            foreach (var column in headerColumns)
            {
                if (column.Contains(filter.Value))
                    return 1;
            }
            return 0;
        }

        public static int CountColumns(string[] headerLine)
        {
            var numberOfColumns = 0;
            foreach (var c in headerLine)
            {
                if (c != null)
                    numberOfColumns++;
            }

            return numberOfColumns;
        }

        private static bool IsHeader(int i)
        {
            return i == 0;
        }

        public static List<KeyValuePair<int, string>> MapPropertiesFromExcel(string[] header)
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

        public static List<KeyValuePair<int, string>> GetColumnsOrder(List<KeyValuePair<int, string>> columnsMappingList, List<KeyValuePair<string, string>> listOfColumns)
        {
            var list = new List<KeyValuePair<int, string>>();
            int count = 0;
            foreach (var column in listOfColumns)
            {
                var result = columnsMappingList.FirstOrDefault(x => x.Value == column.Value);
                list.Add(new KeyValuePair<int, string>(result.Key, column.Key));
                count++;
            }

            return list;
        }

        private static Volunteer GetDataFromCSVLine(string[] line, Volunteer volunteer)
        {
            foreach (var c in list)
            {
                if (c.Key != 0)
                {
                    if (c.Value == "Name")
                    {
                        if (line[c.Key] != null && line[c.Key] != "")
                            volunteer.Fullname = line[c.Key];
                    }
                    if (c.Value == "CNP")
                        volunteer.CNP = line[c.Key];

                    if (volunteer.Birthdate == null)
                        volunteer.Birthdate = DateTime.MinValue;

                    if (c.Value == "Address")
                        volunteer.Address = line[c.Key];
                    if (c.Value == "CI")
                    {
                        volunteer.CI = new CI() { Info = line[c.Key] };
                        volunteer.CI.HasId = true;
                    }

                    if (c.Value == "PhoneNumber")
                        volunteer.ContactInformation = new ContactInformation()
                        { PhoneNumber = line[c.Key] };

                    if (c.Value == "Mail")
                        volunteer.ContactInformation.MailAddress = line[c.Key];

                    if (c.Value == "DesiredWorkplace")
                        volunteer.DesiredWorkplace = line[c.Key];

                    if (c.Value == "Comments")
                        volunteer.AdditionalInfo = new AdditionalInfo() { Remark = line[c.Key] };

                    if (volunteer.CNP != null && volunteer.CNP != "")
                    {
                        if (volunteer.CNP.StartsWith("1") || volunteer.CNP.StartsWith("3"))
                            volunteer.Gender = Gender.Male;
                        else if (volunteer.CNP.StartsWith("2") || volunteer.CNP.StartsWith("4"))
                            volunteer.Gender = Gender.Female;
                        else
                            volunteer.Gender = Gender.NotSpecified;
                    }
                    try
                    {
                        if (c.Value == "WorkingHours")
                            volunteer.HourCount = Convert.ToInt32(line[c.Key]);
                    }
                    catch
                    {
                        if (c.Value == "WorkingHours")
                        {
                            var additionInfo = new AdditionalInfo();
                            if (volunteer.AdditionalInfo != null)
                            {
                                additionInfo = volunteer.AdditionalInfo;
                                additionInfo.Remark += " ;" + line[c.Key];
                            }
                            else
                                additionInfo.Remark = line[c.Key];

                            volunteer.AdditionalInfo = additionInfo;
                        }
                    }

                    volunteer.InActivity = true;
                }
            }

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
                        volunteer.Id = Guid.NewGuid().ToString();
                        volunteer = GetDataFromCSVLine(line, volunteer);
                    }
                    else
                    {
                        if (listOfVolunteers.FindAll(x => x.Id == line[0]).Count != 0)
                        {
                            var databaseVolunteer = listOfVolunteers.Find(x => x.Id == line[0]);
                            volunteer = GetDataFromCSVLine(line, databaseVolunteer);
                        }
                        else if (listOfVolunteers.FindAll(x => x.CNP == line[6]).Count != 0)
                        {
                            var databaseVolunteer = listOfVolunteers.Find(x => x.CNP == line[6]);
                            volunteer = GetDataFromCSVLine(line, databaseVolunteer);
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