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

        public EventImportResponse Execute(Stream dataToImport, string overwrite)
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
                var listOfEvents = dataGateway.GetEvents();
                var eventsFromCsv = GetEventsFromCsv(result, response,overwrite,listOfEvents);
                var eventsToUpdate = new List<Event>();
                var eventsToInsert = new List<Event>();
                
                if (response.IsValid )
                {
                    if (overwrite == "yes")
                    {
                        foreach (var e in eventsFromCsv)
                        {
                            if (listOfEvents.FindAll(x => x.Id == e.Id).Count() != 0)
                                eventsToUpdate.Add(e);
                            else
                                eventsToInsert.Add(e);
                        }
                        dataGateway.Update(eventsToUpdate);
                        dataGateway.Insert(eventsToInsert);
                    }
                    else
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

        private static Event GetDataFromLine(string[] line, Event ev)
        {
            ev.NameOfEvent = line[1];
            ev.PlaceOfEvent = line[2];
            ev.DateOfEvent = Convert.ToDateTime(line[3]);
            ev.TypeOfActivities = line[4];
            ev.TypeOfEvent = line[5];
            ev.Duration = line[6];
            ev.NumberOfVolunteersNeeded = Convert.ToInt32(line[7]);
            ev.AllocatedVolunteers = line[8];
            ev.NumberAllocatedVolunteers = Convert.ToInt32(line[9]);
            ev.AllocatedSponsors = line[10];

            return ev;
        }
        private static Event EventOverWrite(string[] line, Event databaseEvent)
        {
            if (line[1] != "" && line[1] != null)
            databaseEvent.NameOfEvent = line[1];
            if (line[2] != "" && line[2] != null)
                databaseEvent.PlaceOfEvent = line[2];
            if (line[3] != "" && line[3] != null)
                databaseEvent.DateOfEvent = Convert.ToDateTime(line[3]);
            if (line[4] != "" && line[4] != null)
                databaseEvent.TypeOfActivities = line[4];
            if (line[5] != "" && line[5] != null)
                databaseEvent.TypeOfEvent = line[5];
            if (line[6] != "" && line[6] != null)
                databaseEvent.Duration = line[6];
            if (line[7] != "" && line[7] != null)
                databaseEvent.NumberOfVolunteersNeeded = Convert.ToInt32(line[7]);
            if (line[8] != "" && line[8] != null)
                databaseEvent.AllocatedVolunteers = line[8];
            if (line[9] != "" && line[9] != null)
                databaseEvent.NumberAllocatedVolunteers = Convert.ToInt32(line[9]);
            if (line[10] != "" && line[10] != null)
                databaseEvent.AllocatedSponsors = line[10];

            return databaseEvent;
        }

        private static List<Event> GetEventsFromCsv(List<string[]> lines, EventImportResponse response,string owerwrite,List<Event> allEventList)
        {
            var events = new List<Event>();

            foreach (var line in lines)
            {
                var ev = new Event();
                
                try
                {

                    if (owerwrite == "no")
                    {
                        if (line[0] != null && line[0] != string.Empty)
                            ev.Id = line[0];
                        else
                            ev.Id = Guid.NewGuid().ToString();
                        ev = GetDataFromLine(line, ev);
                    }
                    else
                    {
                        if (allEventList.FindAll(x => x.Id == line[0]).Count != 0)
                        {
                            var databaseEvent = allEventList.Find(x => x.Id == line[0]);
                            ev= GetDataFromLine(line, databaseEvent);
                        }
                        else
                        {
                            if (line[0] != null && line[0] != string.Empty)
                                ev.Id = line[0];
                            else
                                ev.Id = Guid.NewGuid().ToString();
                            ev = GetDataFromLine(line, ev);
                        }

                    }

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