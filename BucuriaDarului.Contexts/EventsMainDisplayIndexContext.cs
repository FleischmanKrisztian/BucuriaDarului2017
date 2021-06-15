using System.Collections.Generic;
using BucuriaDarului.Core;
using MongoDB.Driver;

namespace BucuriaDarului.Contexts
{
    public class EventsMainDisplayIndexContext
    {
        public EventsMainDisplayIndexResponse Execute(EventsMainDisplayIndexRequest request)
        {
            return new EventsMainDisplayIndexResponse();
        }
    }

    public class EventsMainDisplayIndexRequest
    {
        public FilterData FilterData { get; set; }
    }

    public class EventsMainDisplayIndexResponse
    {
        public List<Event> Events { get; set; }

        public FilterData FilterData { get; set; }
    }

    public class FilterData
    {
        //searching
        //searchingPlace
        //searchingType
    }


}