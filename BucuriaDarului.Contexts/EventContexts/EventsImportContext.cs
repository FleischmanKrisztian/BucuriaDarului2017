﻿using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BucuriaDarului.Contexts
{
    public class EventsImportContext
    {
        private readonly IEventsImportDataGateway dataGateway;

        public EventsImportContext(IEventsImportDataGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public Response Execute(Stream dataToImport)
        {
            Response response = new Response();
            var result = ExtractImportRawData(dataToImport);
            
            if (result.Count() == 0)
            {
                response.IsValid = false;
                response.Messages.Add(new KeyValuePair<string, string>("error", "File is empty. Please try again!"));
            }
            var eventsFromCsv = GetEventsFromCsv(result);
            if (eventsFromCsv.Count() == 0)
            {
                response.IsValid = false;
                response.Messages.Add(new KeyValuePair<string, string>("error2", "File type is wrong! Please import a valid file for event documents!"));
            }
            dataGateway.Insert(eventsFromCsv);

            return response;
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
            return headerColumns.Length == 11;
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
            var Events = new List<Event>();

            foreach (var line in lines)
            {
                Event ev = new Event();
                try
                {
                    ev._id = line[0];
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
                            string[] date = line[3].Split(" ");
                            string[] FinalDate = date[0].Split("/");
                            data = Convert.ToDateTime(FinalDate[2] + "-" + FinalDate[0] + "-" + FinalDate[1]);
                        }
                        else
                        {
                            string[] anotherDate = line[3].Split('.');
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
                        int number = 0;
                        bool converted = false;
                        converted = Int32.TryParse(line[4], out number);
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
                catch (Exception ex)
                {
                    Console.WriteLine("An error has occured while importing!");
                    Console.WriteLine(ex);
                }
                Events.Add(ev);
            }

            return Events;
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

    public class Response
    {
        public List<KeyValuePair<string, string>> Messages { get; set; }
        public bool IsValid { get; set; }

        public Response()
        {
            IsValid = true;
            Messages = new List<KeyValuePair<string, string>>();
        }
    }
}