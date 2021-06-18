using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts
{
    public class EventsExporterContext
    {
        public EventsExporterResponse Execute(EventsExporterRequest request)
        {
            string idsAndFields = GetIdAndFieldString(request.csv);
            string header = GetHeaderForExcelPrinterEvent(request.Localizer);
            var dict = CreateDictionaries(Constants.EVENTSESSION, Constants.EVENTHEADER, idsAndFields, header);
            return new EventsExporterResponse(dict,200,"");
        }

        private string GetIdAndFieldString(CsvExportParamenters csv)
        {
            string ids_and_options = csv.stringOfIDs + "(((";
            if (csv.All)
                ids_and_options += "0";
            if (csv.NameOfEvent)
                ids_and_options += "1";
            if (csv.PlaceOfEvent)
                ids_and_options += "2";
            if (csv.DateOfEvent)
                ids_and_options += "3";
            if (csv.TypeOfActivities)
                ids_and_options += "4";
            if (csv.TypeOfEvent)
                ids_and_options += "5";
            if (csv.Duration)
                ids_and_options += "6";
            if (csv.AllocatedVolunteers)
                ids_and_options += "7";
            if (csv.AllocatedSponsors)
                ids_and_options += "8";
            return ids_and_options;
        }

        public static string GetHeaderForExcelPrinterEvent(IStringLocalizer _localizer)
        {
            string[] header = new string[8];
            header[0] = _localizer["Nameofevent"];
            header[1] = _localizer["Placeofevent"];
            header[2] = _localizer["Dateofevent"];
            header[3] = _localizer["Typeofactivities"];
            header[4] = _localizer["Typeofevent"];
            header[5] = _localizer["Duration"];
            header[6] = _localizer["Allocatedvolunteers"];
            header[7] = _localizer["Allocatedsponsors"];
            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        private Dictionary<string, string> CreateDictionaries(string key1, string key2, string idsAndFields, string header)
        {
            var result = new Dictionary<string, string>();
            result.Add(key1, idsAndFields);
            result.Add(key2, header);

            return result;
        }
    }

    public class EventsExporterResponse
    {
        public Dictionary<string, string> Dictionary { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public EventsExporterResponse(Dictionary<string,string> dictionary, int statusCode, string message)
        {
            Dictionary = dictionary;
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class EventsExporterRequest
    {
        public EventsExporterRequest(CsvExportParamenters csv_, IStringLocalizer localizer)
        {
            csv = csv_;
            Localizer = localizer;
        }

        public CsvExportParamenters csv { get; set; }
        public IStringLocalizer Localizer { get; set; }
    }

    public class CsvExportParamenters
    {
        public string stringOfIDs { get; set; }
        public bool All { get; set; }
        public bool AllocatedSponsors { get; set; }
        public bool AllocatedVolunteers { get; set; }
        public bool Duration { get; set; }
        public bool TypeOfEvent { get; set; }
        public bool NameOfEvent { get; set; }
        public bool PlaceOfEvent { get; set; }
        public bool DateOfEvent { get; set; }
        public bool TypeOfActivities { get; set; }
    }
}