using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;

namespace BucuriaDarului.Contexts
{
    public class EventsImportContext
    {
        private readonly IEventsImportDataGateway dataGateway;

        public EventsImportContext(IEventsImportDataGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public void Execute(Stream dataToImport)
        {
            var result = ExtractImportRawData(dataToImport);

            var eventsFromCsv = GetEventsFromCsv(result);

            dataGateway.Insert(eventsFromCsv);
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
            return headerColumns != null && headerColumns.Length != 10;
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

        private static List<Event> GetEventsFromCsv(List<string[]> lines)
        {
            var result1 = new List<Event>();

            foreach (var line in lines)
            {
                Event ev = new Event();

                try
                {
                    ev.NameOfEvent = line[0];
                }
                catch
                {
                    ev.NameOfEvent = "Invalid name";
                }

                try
                {
                    ev.PlaceOfEvent = line[1];
                }
                catch
                {
                    ev.PlaceOfEvent = "Invalid Place";
                }

                try
                {
                    if (line[2] == null || line[2] == "" || line[2] == "0")
                    {
                        ev.DateOfEvent = DateTime.MinValue;
                    }
                    else
                    {
                        DateTime data;
                        if (line[2].Contains("/") == true)
                        {
                            string[] date = line[2].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = line[2].Split('.');
                            data = Convert.ToDateTime(anotherDate[2] + "-" + anotherDate[1] + "-" + anotherDate[0]);
                        }

                        ev.DateOfEvent = data.AddDays(1);
                    }
                }
                catch
                {
                    ev.DateOfEvent = DateTime.MinValue;
                }

                if (line[3] == "" || line[3] == null)
                {
                    ev.NumberOfVolunteersNeeded = 0;
                }
                else
                {
                    int number = 0;
                    bool converted = false;
                    converted = Int32.TryParse(line[3], out number);
                    if (converted == true)
                    {
                        ev.NumberOfVolunteersNeeded = number;
                    }
                    else
                    {
                        ev.NumberOfVolunteersNeeded = 0;
                    }
                }

                try
                {
                    ev.TypeOfActivities = line[4];
                    ev.TypeOfEvent = line[5];
                    ev.Duration = line[6];
                    ev.AllocatedVolunteers = line[7];
                    ev.AllocatedSponsors = line[8];
                }
                catch
                {
                    ev.TypeOfActivities = "An error has occured";
                    ev.TypeOfEvent = "An error has occured";
                    ev.Duration = "0";
                    ev.AllocatedVolunteers = "An error has occured";
                    ev.AllocatedSponsors = "An error has occured";
                }

                result1.Add(ev);
            }

            return result1;
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