using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using ExcelDataReader;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerImportContext
    {
        private readonly IVolunteerImportGateway dataGateway;
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

                volunteersFromCsv = GetVolunteerFromCsv(result, response, localizer, overwrite, listOfVolunteers);

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
            int counter = 0;
            string line;
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
            int j = 0;
            while (reader.Read())
            {
                if (IsHeader(i))
                {
                    var headerLine = new string[30];

                    for (int k = 0; k < reader.FieldCount; k++)
                    {
                        if (reader.GetValue(k) != null)
                        {
                            headerLine[j] = reader.GetValue(k).ToString();
                            j++;
                        }
                    }

                    numberOfColumns = CountColumns(headerLine);

                    var mapping = MapPropertiesFromExcel(headerLine);
                    list = GetColumnsOrder(mapping, listOfColumns);
                }
                else
                {
                    var row = GetRowFromExcel(reader, numberOfColumns);
                    result.Add(row);
                }

                i++;
            }

            return result;
        }

        private static string[] GetRowFromExcel(IExcelDataReader reader, int numberOfColumns)
        {
            var list = new string[reader.FieldCount];
            int j = 0;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetValue(i) != null)
                {
                    list[i] = reader.GetValue(i).ToString();
                   
                }
                else
                {
                    list[i] = "";
                    
                }
            }
            return list;
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
            list.Add(new KeyValuePair<int, string>(0, "empty"));
            int counter = 1;
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

        private static Volunteer GetDataFromCSVLine(string[] line)
        {
            var culture = CultureInfo.CreateSpecificCulture("ro-RO");
            var styles = DateTimeStyles.None;
            var volunteer = new Volunteer();
            var contactInformation = new ContactInformation();
            var additionalInfo = new AdditionalInfo();
            var cI = new CI();
            foreach (var c in list)
            {
                if (c.Key != 0)
                {
                    if (c.Value == "Ids" && volunteer.Id == null)
                    {
                        volunteer.Id = line[c.Key - 1];
                    }
                    else if (c.Value == "Name")
                    {
                        volunteer.Fullname = line[c.Key - 1];
                    }
                    else if (c.Value == "CNP")
                        volunteer.CNP = line[c.Key - 1];
                    else if (c.Value == "Address")
                        volunteer.Address = line[c.Key - 1];
                    else if (c.Value == "CI")
                    {
                        cI.Info = line[c.Key - 1];
                        cI.HasId = true;
                    }
                    else if (c.Value == "PhoneNumber")
                        contactInformation.PhoneNumber = line[c.Key - 1];
                    else if (c.Value == "Mail")
                        contactInformation.MailAddress = line[c.Key - 1];
                    else if (c.Value == "DesiredWorkplace")
                        volunteer.DesiredWorkplace = line[c.Key - 1];
                    else if (c.Value == "Comments")
                        additionalInfo.Remark = line[c.Key - 1];
                    else if (c.Value == "DriversLicense")
                    {
                        if (line[c.Key - 1] == "TRUE" || line[c.Key - 1] == "FALSE")
                            volunteer.AdditionalInfo.HasDrivingLicense = Convert.ToBoolean(line[c.Key]);
                        else if (line[c.Key - 1] == "no" || line[c.Key - 1] == "nu")
                            volunteer.AdditionalInfo.HasDrivingLicense = false;
                        else if (line[c.Key - 1] == "yes" || line[c.Key - 1] == "da")
                            volunteer.AdditionalInfo.HasDrivingLicense = true;
                    }
                    else if (c.Value == "HasCar")
                    {
                        if (line[c.Key - 1] == "TRUE" || line[c.Key - 1] == "FALSE")
                            volunteer.AdditionalInfo.HasCar = Convert.ToBoolean(line[c.Key]);
                        else if (line[c.Key - 1] == "no" || line[c.Key - 1] == "nu")
                            volunteer.AdditionalInfo.HasCar = false;
                        else if (line[c.Key - 1] == "yes" || line[c.Key - 1] == "da")
                            volunteer.AdditionalInfo.HasCar = true;
                    }
                    else if (c.Value == "FieldOfActivity")
                        volunteer.FieldOfActivity = line[c.Key];
                    else if (c.Value == "Gender")
                    {
                        if (line[c.Key - 1].ToLower() == "f" || line[c.Key - 1] == "feminin" || line[c.Key - 1] == "female")
                            volunteer.Gender = Gender.Female;
                        else if (line[c.Key - 1].ToLower() == "m" || line[c.Key - 1] == "masculin" || line[c.Key - 1] == "male")
                            volunteer.Gender = Gender.Male;
                        else
                            volunteer.Gender = Gender.NotSpecified;
                    }
                    else if (c.Value == "Occupation")
                        volunteer.Occupation = line[c.Key - 1];
                    else if (c.Value == "HasCI")
                    {
                        if (cI.Info != null && cI.Info != string.Empty)
                            cI.HasId = true;
                        else if (line[c.Key - 1] == "TRUE" || line[c.Key - 1] == "FALSE")
                            cI.HasId = Convert.ToBoolean(line[c.Key - 1]);
                        else if (line[c.Key - 1] == "no" || line[c.Key - 1] == "nu")
                            cI.HasId = false;
                        else if (line[c.Key - 1] == "yes" || line[c.Key - 1] == "da")
                            cI.HasId = true;
                        else
                            cI.HasId = false;
                    }
                    else if (c.Value == "CIExpirationDate")
                    {
                        var dateLine = line[c.Key - 1];
                        DateTime date;
                        try
                        {
                            date = Convert.ToDateTime(dateLine);
                        }
                        catch
                        {
                            string[] splitLine = new string[3];
                            if (line[c.Key - 1].Contains("."))
                                splitLine = dateLine.Split(".");
                            if (line[c.Key - 1].Contains("/"))
                                splitLine = dateLine.Split("/");
                            if (line[c.Key - 1].Contains("-"))
                                splitLine = dateLine.Split("-");
                            if (DateTime.TryParse(splitLine[0] + "." + splitLine[1] + "." + splitLine[2], culture, styles, out var dateResult2))
                                date = dateResult2;
                            else
                                date = DateTime.MinValue;
                        }
                        cI.ExpirationDate = date.AddHours(3);
                    }
                    else if (c.Value == "Birthdate")
                    {
                        var birthdateLine = line[c.Key - 1];
                        DateTime date;
                        try
                        {
                            date = Convert.ToDateTime(birthdateLine);
                        }
                        catch
                        {
                            string[] splitLine = new string[3];
                            if (line[c.Key - 1].Contains("."))
                                splitLine = birthdateLine.Split(".");
                            if (line[c.Key - 1].Contains("/"))
                                splitLine = birthdateLine.Split("/");
                            if (line[c.Key - 1].Contains("-"))
                                splitLine = birthdateLine.Split("-");
                            if (DateTime.TryParse(splitLine[0] + "." + splitLine[1] + "." + splitLine[2], culture, styles, out var dateResult2))
                                date = dateResult2;
                            else
                                date = DateTime.MinValue;
                        }
                        volunteer.Birthdate = date.AddHours(3);
                    }
                    else if (c.Value == "WorkingHours")
                    {
                        try
                        {
                            volunteer.HourCount = Convert.ToInt32(line[c.Key - 1]);
                        }
                        catch
                        {
                            if (c.Value == "WorkingHours")
                            {
                                if (volunteer.AdditionalInfo != null)
                                {
                                    additionalInfo.Remark += " ;" + line[c.Key - 1];
                                }
                                else
                                    additionalInfo.Remark = line[c.Key - 1];
                            }
                        }
                    }
                }
            }
            volunteer.InActivity = true;
            if (volunteer.Id == null)
                volunteer.Id = Guid.NewGuid().ToString();
            volunteer.CI = cI;
            volunteer.AdditionalInfo = additionalInfo;
            volunteer.ContactInformation = contactInformation;

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
                        volunteer = GetDataFromCSVLine(line);
                        volunteer.Id = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        var filterCnp = list.FirstOrDefault(kvp => kvp.Value == "CNP");
                        var filterId = list.FirstOrDefault(kvp => kvp.Value == "Ids");
                        //TODO: TEST HERE IF we need both verifications
                        if (listOfVolunteers.FindAll(x => x.CNP == line[filterCnp.Key - 1]).Count != 0 || listOfVolunteers.FindAll(x => x.Id == line[filterId.Key - 1]).Count != 0)
                        {
                            var databaseVolunteer = listOfVolunteers.FirstOrDefault(x => x.CNP == line[filterCnp.Key - 1] || x.Id == line[filterId.Key - 1]);
                            volunteer = GetDataFromCSVLine(line);
                            volunteer.Id = databaseVolunteer.Id;
                        }
                        else
                        {
                            volunteer = GetDataFromCSVLine(line);
                        }
                    }
                }
                catch
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", @localizer["There was an unknown error while importing the File."]));
                    response.IsValid = false;
                }

                if (volunteer.Fullname != null && volunteer.Fullname != string.Empty)
                    volunteers.Add(volunteer);
                else
                {
                    response.IsValid = false;
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile",
                        @localizer["There was an error while importing the file! Make Sure the Document has a column for Names and contains no empty values."]));
                }
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