using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.ControllerHelpers.VolunteerHelpers
{
    public class VolunteerFunctions
    {
        internal static string GetStringOfIds(List<Volunteer> volunteers)
        {
            string stringofids = "vol";
            foreach (Volunteer vol in volunteers)
            {
                stringofids = stringofids + "," + vol.VolunteerID;
            }
            return stringofids;
        }

        internal static List<Volunteer> GetVolunteersAfterPaging(List<Volunteer> volunteers, int page, int nrofdocs)
        {
            volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            volunteers = volunteers.AsQueryable().Take(nrofdocs).ToList();
            return volunteers;
        }

        internal static List<Volunteer> GetVolunteersAfterSearching(List<Volunteer> volunteers, string searching)
        {
            if (searching != null)
            {
               volunteers = volunteers.Where(x => x.Firstname.Contains(searching, StringComparison.InvariantCultureIgnoreCase) || x.Lastname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return volunteers;
        }

        internal static List<Volunteer> GetVolunteersByIds(List<Volunteer> volunteers, string[] vols)
        {
            List<Volunteer> volunteerlist = new List<Volunteer>();
            for (int i = 0; i < vols.Length; i++)
            {
                Volunteer singlevolunteer = volunteers.Where(x => x.VolunteerID == vols[i]).First();
                volunteerlist.Add(singlevolunteer);
            }
            return volunteerlist;
        }

        internal static string GetVolunteerNames(List<Volunteer> volunteers)
        {
            string volnames = "";
            for (int i = 0; i < volunteers.Count; i++)
            {
                var volunteer = volunteers[i];
                volnames = volnames + volunteer.Firstname + " " + volunteer.Lastname + " / ";
            }
            return volnames;
        }

        //internal static Volunteer GetVolunteerFromString(string[] volunteerstring)
        //{
        //    Volunteer volunteer = new Volunteer();
        //    volunteer.
        //    return volunteer;
        //}
    }
}
