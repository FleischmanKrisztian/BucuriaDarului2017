using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
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

            if (!IsTheCorrectHeader(GetHeaderColumns(dataToImport)))
            {
                response.Message.Add(new KeyValuePair<string, string>("IncorrectFile", "File must be of type Event!"));
                response.IsValid = false;
            }

            if (response.IsValid)
            {
                var result = ExtractImportRawData(dataToImport);
                var eventsFromCsv = GetEventsFromCsv(result, response);
                dataGateway.Insert(eventsFromCsv);
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
            return headerColumns[0].Contains("Event", StringComparison.InvariantCultureIgnoreCase) || headerColumns[0].Contains("Eveniment", StringComparison.InvariantCultureIgnoreCase);
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
                    ev.Id = line[0];
                    ev.NameOfEvent = line[1];
                    ev.PlaceOfEvent = line[2];

                    if (line[3] == null || line[3] == "" || line[3] == "0")
                    {
                        ev.DateOfEvent = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (line[3].Contains("/") == true)
                        {
                            var date = line[3].Split(" ");
                            var finalDate = date[0].Split("/");
                            data = Convert.ToDateTime(finalDate[2] + "-" + finalDate[0] + "-" + finalDate[1]);
                        }
                        else
                        {
                            var anotherDate = line[3].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }
                        ev.DateOfEvent = data.AddDays(1);
                    }

                    if (line[4] == "" || line[4] == null)
                    {
                        ev.NumberOfVolunteersNeeded = 0;
                    }
                    else
                    {
                        var converted = false;
                        converted = Int32.TryParse(line[4], out var number);
                        if (converted)
                        {
                            ev.NumberOfVolunteersNeeded = number;
                        }
                        else
                        {
                            ev.NumberOfVolunteersNeeded = 0;
                        }
                    }
                    ev.TypeOfActivities = line[5];
                    ev.TypeOfEvent = line[6];
                    ev.Duration = line[7];
                    ev.AllocatedVolunteers = line[8];
                    ev.AllocatedSponsors = line[10];
                }
                catch
                {
                    response.Message.Add((new KeyValuePair<string, string>("IncorrectFile", "File must be of Event type!")));
                    response.IsValid = false;
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