using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerBirthdayDisplayContext
    {
        private readonly IListDisplayVolunteersGateway dataGateway;

        public VolunteerBirthdayDisplayContext(IListDisplayVolunteersGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public List<Volunteer> Execute(int alarmNumberOfDaysBeforExpiration)
        {
            List<Volunteer> volunteers = dataGateway.GetListOfVolunteers();
            volunteers = GetVolunteersWithBirthdays(volunteers, alarmNumberOfDaysBeforExpiration);
            return volunteers;
        }

        internal static List<Volunteer> GetVolunteersWithBirthdays(List<Volunteer> volunteers,int alarmNumberOfDaysBeforExpiration)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            List<Volunteer> returnListOfVolunteers = new List<Volunteer>();
            foreach (var vol in volunteers)
            {
                int dayToCompare = GetDayOfYear(vol.Birthdate);
                if (IsAboutToExpire(currentDay, dayToCompare, alarmNumberOfDaysBeforExpiration))
                {
                    returnListOfVolunteers.Add(vol);
                }
            }
            return returnListOfVolunteers;
        }

        internal static int GetDayOfYear(DateTime date)
        {
            string dateasString = date.ToString("dd-MM-yyyy");
            string[] dates = dateasString.Split('-');
            int Day = Convert.ToInt16(dates[0]);
            int Month = Convert.ToInt16(dates[1]);
            Day = (Month - 1) * 30 + Day;
            return Day;
        }

        public static bool IsAboutToExpire(int currentDay, int dayToCompareto,int alarmNumberOfDaysBeforExpiration)
        {
            if (currentDay <= dayToCompareto && currentDay + alarmNumberOfDaysBeforExpiration > dayToCompareto || currentDay > 355 && dayToCompareto < alarmNumberOfDaysBeforExpiration-1)
            {
                return true;
            }
            return false;
        }

    }
}
