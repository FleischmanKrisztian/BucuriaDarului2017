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

        internal static List <Event> GetEventsAfterFilters(List<Event> events, string searching, string searchingPlace, string searchingActivity, string searchingType, string searchingVolunteers, string searchingSponsor, DateTime lowerdate, DateTime upperdate)
        {
            if (searching != null)
            {
                events = events.Where(x => x.NameOfEvent.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchingPlace != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.PlaceOfEvent == null || e.PlaceOfEvent == "")
                    { e.PlaceOfEvent = "-"; }
                }
                events = ev.Where(x => x.PlaceOfEvent.Contains(searchingPlace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchingActivity != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.TypeOfActivities == null || e.TypeOfActivities == "")
                    { e.TypeOfActivities = "-"; }
                }
                events = ev.Where(x => x.TypeOfActivities.Contains(searchingActivity, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchingType != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.TypeOfEvent == null || e.TypeOfEvent == "")
                    { e.TypeOfEvent = "-"; }
                }
                events = ev.Where(x => x.TypeOfEvent.Contains(searchingType, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchingVolunteers != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.AllocatedVolunteers == null || e.AllocatedVolunteers == "")
                    { e.AllocatedVolunteers = "-"; }
                }
                events = ev.Where(x => x.AllocatedVolunteers.Contains(searchingVolunteers, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchingSponsor != null)
            {
                List<Event> ev = events;
                foreach (var e in ev)
                {
                    if (e.AllocatedSponsors == null || e.AllocatedSponsors == "")
                    { e.AllocatedSponsors = "-"; }
                }
                events = ev.Where(x => x.AllocatedSponsors.Contains(searchingSponsor, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (Dateinputreceived(lowerdate))
            {
                events = events.Where(x => x.DateOfEvent > lowerdate).ToList();
            }
            if (Dateinputreceived(upperdate))
            {
                events = events.Where(x => x.DateOfEvent <= upperdate).ToList();
            }
            return events;
        }

        private static bool Dateinputreceived(DateTime date)
        {
            DateTime comparisondate = new DateTime(0003, 1, 1);
            if (date > comparisondate)
                return true;
            else
                return false;
        }

        internal static List<Event> GetEventsAfterPaging(List<Event> events, int page, int nrofdocs)
        {
            events = events.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            events = events.AsQueryable().Take(nrofdocs).ToList();
            return events;
        }

        internal static string GetStringOfIds(List<Event> events)
        {
            string stringofids = "events";
            foreach (Event eve in events)
            {
                stringofids = stringofids + "," + eve.EventID;
            }
            return stringofids;
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
            Event returnedevent = events.Find(b => b.EventID.ToString() == id);
            returnedevent.AllocatedVolunteers += " / ";
            return returnedevent.AllocatedVolunteers;
        }

        internal static string GetNameOfEvent(List<Event> events, string id)
        {
            Event returnedevent = events.Find(b => b.EventID.ToString() == id);
            return returnedevent.NameOfEvent;
        }

        internal static dynamic GetAllocatedSponsorsString(List<Event> events, string id)
        {
            Event returnedevent = events.Find(b => b.EventID.ToString() == id);
            returnedevent.AllocatedSponsors += " / ";
            return returnedevent.AllocatedSponsors;
        }

        internal static int VolunteersAllocatedCounter(string AllocatedVolunteers)
        {
            if (AllocatedVolunteers != null)
            {
                string[] split = AllocatedVolunteers.Split(" / ");
                return split.Count()-1;
            }
            return 0;
        }
    }
}
