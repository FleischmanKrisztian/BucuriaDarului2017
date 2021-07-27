using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.EventGateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucuriaDarului.Contexts.EventContexts
{
    public class EventDownloadContext
    {
        private readonly IEventDownloadGateway dataGateway;

        public object CsvSerializer { get; private set; }

        public EventDownloadContext(IEventDownloadGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public string Execute(string dataString, string header)
        {
            var finalHeader = SplitHeader(header);
            string jsonString = string.Empty;
            var properties = GetProperties(dataString);
            var ids = GetIds(dataString);

            if (properties.Contains("0"))
                jsonString = GetEventsCsv(ids, finalHeader);
            else
                jsonString = GetEvents(properties, ids, finalHeader);

            return jsonString;
        }

        public static string[] SplitHeader(string header)
        {
            var splitHeader = header.Split(",");

            return splitHeader;
        }

        public string GetProperties(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var properties = auxiliaryStrings[1];
            return properties;
        }

        public string[] GetIds(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var ids = auxiliaryStrings[0].Split(",");
            return ids;
        }

        public string[] EventToArray(Event @event)
        {
            string[] result = new string[] { @event.Id, @event.NameOfEvent, @event.PlaceOfEvent, DateTime.Parse(@event.DateOfEvent.ToString(), System.Globalization.CultureInfo.InvariantCulture).ToString(), @event.TypeOfActivities, @event.TypeOfEvent,
                  @event.Duration ,@event.NumberOfVolunteersNeeded.ToString() , @event.AllocatedVolunteers ,@event.NumberAllocatedVolunteers.ToString(), @event.AllocatedSponsors };
            return result;
        }

        public string GetEventsCsv(string[] ids, string[] finalHeader)
        {
            var listOfEvents = dataGateway.GetListOfEvents();
            var finalListOEvents = new List<Event>();
            foreach (var @event in listOfEvents)
            {
                if (ids.Contains(@event.Id))
                {
                    finalListOEvents.Add(@event);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in finalHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }
            sb.Append("\r\n");
            foreach (var item in finalListOEvents)
            {
                string[] arrEvent = EventToArray(item); ;
                foreach (var data in arrEvent)
                {
                    sb.Append("\"" + data + "\"" + ",");
                }

                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public string[] GetUsedHeader(string properties, string[] header)
        {
            List<string> headerList = new List<string>();
            if (properties.Contains("1"))

                headerList.Add(header[1]);

            if (properties.Contains("2"))

                headerList.Add(header[2]);

            if (properties.Contains("3"))

                headerList.Add(header[3]);

            if (properties.Contains("4"))
                headerList.Add(header[4]);

            if (properties.Contains("5"))
                headerList.Add(header[5]);

            if (properties.Contains("6"))

                headerList.Add(header[6]);

            if (properties.Contains("7"))

                headerList.Add(header[7]);

            if (properties.Contains("8"))
                headerList.Add(header[8]);

            var returnedHeader = headerList.ToArray();
            return returnedHeader;
        }

        public string GetEvents(string properties, string[] ids, string[] header)
        {
            var usedHeader = GetUsedHeader(properties, header);
            var listOfEvents = dataGateway.GetListOfEvents();
            var idListForPrinting = new List<Event>();
            foreach (var @event in listOfEvents)
            {
                if (ids.Contains(@event.Id))
                {
                    idListForPrinting.Add(@event);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in usedHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }

            sb.Append("\r\n");
            foreach (var item in idListForPrinting)
            {
                string[] arrEvent = EventToArray(item);

                if (properties.Contains("1"))

                    sb.Append("\"" + arrEvent[1] + "\"" + ",");

                if (properties.Contains("2"))

                    sb.Append("\"" + arrEvent[2] + "\"" + ",");

                if (properties.Contains("3"))

                    sb.Append("\"" + arrEvent[3] + "\"" + ",");

                if (properties.Contains("4"))
                    sb.Append("\"" + arrEvent[4] + "\"" + ",");

                if (properties.Contains("5"))
                    sb.Append("\"" + arrEvent[5] + "\"" + ",");

                if (properties.Contains("6"))

                    sb.Append("\"" + arrEvent[6] + "\"" + ",");

                if (properties.Contains("7"))

                    sb.Append("\"" + arrEvent[7] + "\"" + ",");

                if (properties.Contains("8"))
                    sb.Append("\"" + arrEvent[8] + "\"" + ",");

                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}