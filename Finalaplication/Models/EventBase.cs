using System;
using System.ComponentModel.DataAnnotations;


namespace VolCommon
{
    public class EventBase
    {

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


        //public int  VolunteerAllocateCounter(string AllocatedVolunteers, int NumberOfVolunteersNeeded)
        //{
        //    int VolunteerAllocate = 0;
        //    for (int i = 0; i <= AllocatedVolunteers.Length; i++)
        //    {
        //        VolunteerAllocate= VolunteerAllocate + 1;
        //    }

        //    int rez = NumberOfVolunteersNeeded - VolunteerAllocate;
        //    return rez;
        //}



    }
}
