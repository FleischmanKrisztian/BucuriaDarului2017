using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
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


        public VolunteerImportContext(IStringLocalizer localizer,IVolunteerImportGateway dataGateway)
        {
            this.localizer=localizer;
            this.dataGateway = dataGateway;
        }

        public VolunteerImportResponse Execute(Stream dataToImport)
        {
            var response = new VolunteerImportResponse();
            if (FileIsNotEmpty(dataToImport))
            {
                response.Message.Add(new KeyValuePair<string, string>("EmptyFile",@localizer["File Cannot be Empty!"]));
                response.IsValid = false;
            }

            if (response.IsValid)
            {
                var result = ExtractImportRawData(dataToImport);
                List<Volunteer> volunteersFromCsv = new List<Volunteer>();
                if (_fileType == 0)
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", @localizer["File must be of type Volunteer!"]));
                    response.IsValid = false;
                }
                else if (_fileType == 1)
                    volunteersFromCsv = GetVolunteerFromCsv(result, response);
                else
                    volunteersFromCsv = GetVolunteerFromBucuriaDaruluiCSV(result, response);
                if (response.IsValid)
                {
                    dataGateway.Insert(volunteersFromCsv);
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
            var splits = new Regex("((?<=\")[^\"]*(?=\"(" + csvSeparator + "|$)+)|(?<=" + csvSeparator + "|^)[^" +
                                   csvSeparator + "\"]*(?=" + csvSeparator + "|$))").Matches(line);
            var row = splits.Cast<Match>().Select(match => match.Value).ToArray();
            return row;
        }

        private static int IsTheCorrectHeader(string[] headerColumns)
        {
            var correct = headerColumns[1].Contains("Fullname", StringComparison.InvariantCultureIgnoreCase) &&
                          headerColumns[2].Contains("Birthdate", StringComparison.InvariantCultureIgnoreCase);
            if (correct)
                return 1;

            correct = headerColumns[1].Contains("prenume", StringComparison.InvariantCultureIgnoreCase) &&
                      headerColumns[2].Contains("data", StringComparison.InvariantCultureIgnoreCase);
            if (correct)
                return 1;

            return 0;
        }

        private static int IsTheCorrectHeaderForTheirCsv(string[] headerColumns)
        {
            var differentCSV =
                headerColumns[0].Contains("nume si prenume", StringComparison.InvariantCultureIgnoreCase) &&
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

        private static List<Volunteer> GetVolunteerFromCsv(List<string[]> lines, VolunteerImportResponse response)
        {
            var volunteers = new List<Volunteer>();

            foreach (var line in lines)
            {
                var volunteer = new Volunteer();
                try
                {
                    volunteer.Id = line[0];
                    volunteer.Fullname = line[1];
                    volunteer.Birthdate = Convert.ToDateTime(line[2]);
                    volunteer.Address = line[3];
                    volunteer.Gender = line[4] == "Male" ? Gender.Male : Gender.Female;
                    volunteer.DesiredWorkplace = line[5];
                    volunteer.CNP = line[6];
                    volunteer.FieldOfActivity = line[7];
                    volunteer.Occupation = line[8];

                    var cI = new CI
                    {
                        HasId = Convert.ToBoolean(line[9]),
                        Info = line[10],
                        ExpirationDate = Convert.ToDateTime(line[11])
                    };

                    volunteer.CI = cI;
                    volunteer.InActivity = Convert.ToBoolean(line[12]);
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
                }
                catch
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile",
                        "There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file."));
                    response.IsValid = false;
                }

                volunteers.Add(volunteer);
            }

            return volunteers;
        }

        private List<Volunteer> GetVolunteerFromBucuriaDaruluiCSV(List<string[]> lines,
            VolunteerImportResponse response)
        {
            var volunteers = new List<Volunteer>();

            foreach (var line in lines)
            {
                var volunteer = new Volunteer();
                try
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
                        ExpirationDate = DateTime.Today
                    };

                    volunteer.Id = Guid.NewGuid().ToString();

                    volunteer.Fullname = line[0];
                    volunteer.CNP = line[1];
                    volunteer.Address = line[2];
                    volunteer.Occupation = line[8];
                    volunteer.DesiredWorkplace = line[9];
                    volunteer.Birthdate = DateTime.Today;
                    volunteer.FieldOfActivity = "";
                    volunteer.Gender = Gender.Female;
                    volunteer.HourCount = 0;
                    volunteer.InActivity = false;
                    volunteer.CI = ci;
                    volunteer.ContactInformation = contactInformation;
                    volunteer.AdditionalInfo = additionalInformation;
                }
                catch
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file."));
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