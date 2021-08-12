using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventsImportContext
    {
        private readonly IEventsImportDataGateway dataGateway;

        public EventsImportContext(IEventsImportDataGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public EventImportResponse Execute(Stream dataToImport)
        {
            var response = new EventImportResponse();
            if (FileIsNotEmpty(dataToImport))
            {
                response.Message.Add(new KeyValuePair<string, string>("EmptyFile", "File Cannot be Empty!"));
                response.IsValid = false;
            }

            if (response.IsValid)
            {
                var result = ExtractImportRawData(dataToImport);
                if (result[0].Contains("File must be of type Event!"))
                {
                    response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Event!"));
                    response.IsValid = false;
                }
                var eventsFromCsv = GetEventsFromCsv(result, response);
                if (response.IsValid)
                {
                    dataGateway.Insert(eventsFromCsv);
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
                        strarray[0] = "File must be of type Event!";
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
            var correct = headerColumns[1].Contains("Name of event", StringComparison.InvariantCultureIgnoreCase) && headerColumns[2].Contains("Place of event", StringComparison.InvariantCultureIgnoreCase);
            if (correct)
                return correct;
            correct = headerColumns[1].Contains("Numele evenimentului", StringComparison.InvariantCultureIgnoreCase) && headerColumns[2].Contains("Locul evenimentului", StringComparison.InvariantCultureIgnoreCase);
            return correct;
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

        private static List<Event> GetEventsFromCsv(List<string[]> lines, EventImportResponse response)
        {
            var events = new List<Event>();

            foreach (var line in lines)
            {
                var ev = new Event();
                try
                {
                    //TODO : The Date Crashes From the Romanian Import
                    if (line[0] != null && line[0] != string.Empty)
                        ev.Id = line[0];
                    else
                        ev.Id = Guid.NewGuid().ToString();
                    ev.NameOfEvent = line[1];
                    ev.PlaceOfEvent = line[2];
                    ev.DateOfEvent = Convert.ToDateTime(line[3]);
                    ev.TypeOfActivities = line[4];
                    ev.TypeOfEvent = line[5];
                    ev.Duration = line[6];
                    ev.NumberOfVolunteersNeeded= Convert.ToInt32(line[7]);
                    ev.AllocatedVolunteers = line[8];
                    ev.NumberAllocatedVolunteers = Convert.ToInt32(line[9]);
                    ev.AllocatedSponsors = line[10];
                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("Error", "There was an error while adding the file! Make Sure the Document has all of its Fields and is not only a partial CSV file.")));
                    response.IsValid = false;
                    break;
                }
                events.Add(ev);
            }

            return events;
        }
    }

    public class EventImportResponse
    {
        public bool IsValid { get; set; }

        public List<KeyValuePair<string, string>> Message { get; set; }

        public EventImportResponse()
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