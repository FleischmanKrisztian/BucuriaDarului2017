using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerImportContext
    {
        private readonly IVolunteerImportGateway dataGateway;

        public VolunteerImportContext(IVolunteerImportGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerImportResponse Execute(Stream dataToImport)
        {
            var response = new VolunteerImportResponse();
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
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Volunteer!"));
                    response.IsValid = false;
                }
                else
                {
                    var beneficiariesFromCsv = GetVolunteerFromCsv(result, response);
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

                    if (!IsTheCorrectHeader(headerColumns))
                    {
                        var returnList = new List<string[]>();
                        var strArray = new string[1];
                        strArray[0] = "File must be of type Volunteer!";
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
            return headerColumns[1].Contains("Fullname", StringComparison.InvariantCultureIgnoreCase) && headerColumns[2].Contains("Birthdate", StringComparison.InvariantCultureIgnoreCase);
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
                    //CODE HERE
                    volunteer.Id = line[0];
                    volunteer.Fullname = line[1];
                    volunteer.Birthdate = Convert.ToDateTime(line[2]);
                    volunteer.Address = line[3];
                    volunteer.Gender = Convert.ToInt16(line[4]) == 0 ? Gender.Male : Gender.Female;
                    volunteer.DesiredWorkplace = line[5];
                    volunteer.CNP = line[6];
                    volunteer.FieldOfActivity = line[7];
                    volunteer.Occupation = line[8];

                    //MAYBE WRONG HERE
                    var cI = new CI
                    {
                        HasId = Convert.ToBoolean(line[9]),
                        Info = line[10],
                        ExpirationDate = Convert.ToDateTime(line[11])
                    };

                    volunteer.CI = cI;
                    //volunteer.CIseria = line[7];
                    //volunteer.CNP = line[6];
                    //volunteer.FieldOfActivity = line[7];
                    //volunteer.CNP = line[6];
                    //volunteer.FieldOfActivity = line[7];



                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("IncorrectFile", "File must be of Volunteer type!")));
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