using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.ControllerHelpers.EventHelpers
{
    public class EventFunctions
    {
        internal static Event GetEventFromString(string[] eventstring)
        {
            Event newevent = new Event();
            newevent.NameOfEvent = eventstring[0];
            newevent.PlaceOfEvent = eventstring[1];
            try
            {
            newevent.DateOfEvent = Convert.ToDateTime(eventstring[2]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                newevent.DateOfEvent = DateTime.MinValue;
            }
            try
            {
            newevent.NumberOfVolunteersNeeded = Convert.ToInt32(eventstring[3]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to 0");
                newevent.NumberOfVolunteersNeeded = 0;
            }
            newevent.TypeOfActivities = eventstring[4];
            newevent.TypeOfEvent = eventstring[5];
            newevent.Duration = eventstring[6];
            newevent.AllocatedVolunteers = eventstring[7];
            newevent.AllocatedSponsors = eventstring[8];
            return newevent;
        }
    }
}
