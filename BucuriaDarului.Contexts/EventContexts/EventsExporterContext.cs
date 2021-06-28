using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventsExporterContext
    {
        private readonly IStringLocalizer localizer;

        public EventsExporterContext(IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public EventsExporterResponse Execute(EventsExporterRequest request)
        {
            string idsAndFields = GetIdAndFieldString(request.ExportParameters);
            string header = GetHeaderForExcelPrinterEvent();
            return new EventsExporterResponse(CreateDictionaries(Constants.EVENTSESSION, Constants.EVENTHEADER, idsAndFields, header),"");
        }

        private string GetIdAndFieldString(ExportParameters csv)
        {
            string idsAndOptions = csv.StringOfIDs + "(((";
            if (csv.All)
                idsAndOptions += "0";
            if (csv.NameOfEvent)
                idsAndOptions += "1";
            if (csv.PlaceOfEvent)
                idsAndOptions += "2";
            if (csv.DateOfEvent)
                idsAndOptions += "3";
            if (csv.TypeOfActivities)
                idsAndOptions += "4";
            if (csv.TypeOfEvent)
                idsAndOptions += "5";
            if (csv.Duration)
                idsAndOptions += "6";
            if (csv.AllocatedVolunteers)
                idsAndOptions += "7";
            if (csv.AllocatedSponsors)
                idsAndOptions += "8";
            return idsAndOptions;
        }

        private string GetHeaderForExcelPrinterEvent()
        {
            string[] header = new string[8];
            header[0] = localizer["Nameofevent"];
            header[1] = localizer["Placeofevent"];
            header[2] = localizer["Dateofevent"];
            header[3] = localizer["Typeofactivities"];
            header[4] = localizer["Typeofevent"];
            header[5] = localizer["Duration"];
            header[6] = localizer["Allocatedvolunteers"];
            header[7] = localizer["Allocatedsponsors"];
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
            var result = new Dictionary<string, string>
            {
                { key1, idsAndFields },
                { key2, header }
            };

            return result;
        }
    }

    public class EventsExporterResponse
    {
        public Dictionary<string, string> Dictionary { get; set; }
        public string Message { get; set; }

        public EventsExporterResponse(Dictionary<string,string> dictionary, string message)
        {
            Dictionary = dictionary;
            Message = message;
        }
    }

    public class EventsExporterRequest
    {
        public EventsExporterRequest(ExportParameters exportProperties)
        {
            ExportParameters = exportProperties;
        }

        public ExportParameters ExportParameters { get; set; }
    }

    public class ExportParameters
    {
        public string StringOfIDs { get; set; }
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