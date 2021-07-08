using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Finalaplication.Models
{
    public class Event
    {
        public string Id { get; set; }

        [Required]
        public string NameOfEvent { get; set; }

        [DefaultValue("")]
        public string PlaceOfEvent { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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