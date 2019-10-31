using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;

namespace Finalaplication.Models
{
    public class CsvEventMapping : CsvMapping<Event>
    {
        public CsvEventMapping() : base()
        {   MapProperty(1, x => x.NameOfEvent);
            MapProperty(2, x => x.PlaceOfEvent);
            MapProperty(3, x => x.DateOfEvent);
            MapProperty(4, x => x.NumberOfVolunteersNeeded);
            MapProperty(5, x => x.TypeOfActivities);
            MapProperty(6, x => x.TypeOfEvent);
            MapProperty(7, x => x.Duration);
        }

       
    }
}
