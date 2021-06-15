//using BucuriaDarului.Core;
using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finalaplication.ControllerHelpers.EventHelpers
{
    public class EventFunctions
    {
        internal static Event GetEventFromString(string[] eventstring)
        {
            Event newevent = new Event();
            newevent._id = Guid.NewGuid().ToString();
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

        internal static List<Event> GetEventsAfterPaging(List<Event> events, int page, int nrofdocs)
        {
            events = events.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            events = events.AsQueryable().Take(nrofdocs).ToList();
            return events;
        }

        internal static string GetIdAndFieldString(string IDS, bool All, bool AllocatedSponsors, bool AllocatedVolunteers, bool Duration, bool TypeOfEvent, bool NameOfEvent, bool PlaceOfEvent, bool DateOfEvent, bool TypeOfActivities)
        {
            string ids_and_options = IDS + "(((";
            if (All)
                ids_and_options += "0";
            if (NameOfEvent)
                ids_and_options += "1";
            if (PlaceOfEvent)
                ids_and_options += "2";
            if (DateOfEvent)
                ids_and_options += "3";
            if (TypeOfActivities)
                ids_and_options += "4";
            if (TypeOfEvent)
                ids_and_options += "5";
            if (Duration)
                ids_and_options += "6";
            if (AllocatedVolunteers)
                ids_and_options += "7";
            if (AllocatedSponsors)
                ids_and_options += "8";
            return ids_and_options;
        }

        internal static string GetAllocatedVolunteersString(List<Event> events, string id)
        {
            Event returnedevent = events.Find(b => b._id.ToString() == id);
            returnedevent.AllocatedVolunteers += " / ";
            return returnedevent.AllocatedVolunteers;
        }

        internal static string GetNameOfEvent(List<Event> events, string id)
        {
            Event returnedevent = events.Find(b => b._id.ToString() == id);
            return returnedevent.NameOfEvent;
        }

        internal static dynamic GetAllocatedSponsorsString(List<Event> events, string id)
        {
            Event returnedevent = events.Find(b => b._id.ToString() == id);
            returnedevent.AllocatedSponsors += " / ";
            return returnedevent.AllocatedSponsors;
        }

        internal static int VolunteersAllocatedCounter(string AllocatedVolunteers)
        {
            if (AllocatedVolunteers != null)
            {
                string[] split = AllocatedVolunteers.Split(" / ");
                return split.Count() - 1;
            }
            return 0;
        }
    }
}