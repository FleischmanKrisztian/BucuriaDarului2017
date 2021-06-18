using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts
{
    public class EventsExporterContext
    {
        public EventsExporterResponse Execute(EventsExporterRequest request)
        {
            string idsAndFields = GetIdAndFieldString(request.StringOfIDs,request.Properties);
            string header = GetHeaderForExcelPrinterEvent(request.Localizer);
            var dict = CreateDictionaries(Constants.EVENTSESSION, Constants.EVENTHEADER, idsAndFields, header);
            return new EventsExporterResponse(dict,200,"");
        }

        private string GetIdAndFieldString(string stringOfIDs, Properties properties)
        {
            string ids_and_options = stringOfIDs + "(((";
            if (properties.All)
                ids_and_options += "0";
            if (properties.NameOfEvent)
                ids_and_options += "1";
            if (properties.PlaceOfEvent)
                ids_and_options += "2";
            if (properties.DateOfEvent)
                ids_and_options += "3";
            if (properties.TypeOfActivities)
                ids_and_options += "4";
            if (properties.TypeOfEvent)
                ids_and_options += "5";
            if (properties.Duration)
                ids_and_options += "6";
            if (properties.AllocatedVolunteer)
                ids_and_options += "7";
            if (properties.AllocatedSponsors)
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
        public EventsExporterRequest(string stringOfIDs, Properties properties, IStringLocalizer localizer)
        {
            StringOfIDs = stringOfIDs;
            Properties = properties;
            Localizer = localizer;
        }

        public string StringOfIDs { get; set; }
        public Properties Properties { get; set; }
        public IStringLocalizer Localizer { get; set; }
    }


    public class Properties
    {
        public string StringOfIDs;
        public bool All;
        public bool AllocatedSponsors;
        public bool AllocatedVolunteer;
        public bool Duration;
        public bool TypeOfEvent;
        public bool NameOfEvent;
        public bool PlaceOfEvent;
        public bool DateOfEvent;
        public bool TypeOfActivities;
    }
}