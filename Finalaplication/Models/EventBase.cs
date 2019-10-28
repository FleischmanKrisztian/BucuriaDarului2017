using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace VolCommon
{
    public class EventBase
    {
        [Required]
        public string NameOfEvent { get; set; }

        public string PlaceOfEvent { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfEvent { get; set; }

        public int NumberOfVolunteersNeeded { get; set; }

        public string TypeOfActivities { get; set; }

        public string TypeOfEvent { get; set; }

        public int Duration { get; set; }

        public string AllocatedVolunteers { get; set; }

        public string AllocatedSponsors { get; set; }

        public IFormFile Files { get; set; }

        public string VolunteerAllocateCounter(string AllocatedVolunteers, int NumberOfVolunteersNeeded)
        {
            if (AllocatedVolunteers != null)
            {
                string[] split = AllocatedVolunteers.Split(" / ");
                int nr = 0;
                foreach (string item in split)
                {
                    nr++;
                }
                nr--;
                string rezstring = nr.ToString();

                return rezstring;
            }
            return null;
        }
    }
}
