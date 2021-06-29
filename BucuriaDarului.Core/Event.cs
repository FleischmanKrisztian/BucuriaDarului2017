using System;

namespace BucuriaDarului.Core
{
    public class Event
    {
        public string Id { get; set; }

        public string NameOfEvent { get; set; }

        public string PlaceOfEvent { get; set; }

        public DateTime DateOfEvent { get; set; }

        public int NumberOfVolunteersNeeded { get; set; }

        public string TypeOfActivities { get; set; }

        public string TypeOfEvent { get; set; }

        public string Duration { get; set; }

        public string AllocatedVolunteers { get; set; }

        public int NumberAllocatedVolunteers { get; set; }

        public string AllocatedSponsors { get; set; }
    }
}