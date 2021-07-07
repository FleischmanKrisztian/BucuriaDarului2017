using Finalaplication.ControllerHelpers.UniversalHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;

namespace Finalaplication.ControllerHelpers.VolunteerHelpers
{
    public class VolunteerFunctions
    {
        internal static string GetStringOfIds(List<Volunteer> volunteers)
        {
            string stringofids = "volunteerCSV";
            foreach (Volunteer vol in volunteers)
            {
                stringofids = stringofids + "," + vol.Id;
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
                volunteers = volunteers.Where(x => x.Fullname.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return volunteers;
        }

        internal static List<Volunteer> GetVolunteersByIds(List<Volunteer> volunteers, string[] vols)
        {
            List<Volunteer> volunteerlist = new List<Volunteer>();
            for (int i = 0; i < vols.Length; i++)
            {
                Volunteer singlevolunteer = volunteers.Where(x => x.Id == vols[i]).First();
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
                volnames = volnames + volunteer.Fullname + " / ";
            }
            return volnames;
        }

        internal static bool DoesNotExist(List<Volunteer> volunteers, Volunteer vol)
        {
            if (vol.CNP != null || vol.CNP != "")
            {
                int numberofoccurences = volunteers.Where(p => p.CNP == vol.CNP).Count();
                if (numberofoccurences >= 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

       

        internal static List<Volunteer> GetVolunteerAfterSorting(List<Volunteer> volunteers, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Fullname).ToList();
                    break;

                case "Gender":
                    volunteers = volunteers.OrderBy(s => s.Gender).ToList();
                    break;

                case "Gender_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Gender).ToList();
                    break;

                case "Hourcount":
                    volunteers = volunteers.OrderBy(s => s.HourCount).ToList();
                    break;

                case "Hourcount_desc":
                    volunteers = volunteers.OrderByDescending(s => s.HourCount).ToList();
                    break;

                case "Active":
                    volunteers = volunteers.OrderBy(s => s.InActivity).ToList();
                    break;

                case "Active_desc":
                    volunteers = volunteers.OrderByDescending(s => s.InActivity).ToList();
                    break;

                case "Date":
                    volunteers = volunteers.OrderBy(s => s.Birthdate).ToList();
                    break;

                case "date_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Birthdate).ToList();
                    break;

                default:
                    volunteers = volunteers.OrderBy(s => s.Fullname).ToList();
                    break;
            }
            return volunteers;
        }

        internal static List<Volunteer> GetVolunteerAfterPaging(List<Volunteer> volunteers, int page, int nrofdocs)
        {
            volunteers = volunteers.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            volunteers = volunteers.AsQueryable().Take(nrofdocs).ToList();
            return volunteers;
        }

        internal static List<Volunteer> GetVolunteersAfterFilters(List<Volunteer> volunteers, string searchingFullname, string searchedContact, bool active, bool hasCar, bool hasDrivingLicence, DateTime lowerdate, DateTime upperdate, string gender, string searchedAddress, string searchedworkplace, string searchedOccupation, string searchedRemarks, int searchedHourCount)
        {
            DateTime d1 = new DateTime(0003, 1, 1);
            if (searchingFullname != null)
            {
                volunteers = volunteers.Where(x => x.Fullname.Contains(searchingFullname, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (active == true)
            {
                volunteers = volunteers.Where(x => x.InActivity == true).ToList();
            }
            if (searchedworkplace != null)
            {
                volunteers = volunteers.Where(x => x.DesiredWorkplace.Contains(searchedworkplace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchedOccupation != null)
            {
                List<Volunteer> vol = volunteers;
                foreach (var v in vol)
                {
                    if (v.FieldOfActivity == null || v.FieldOfActivity == "")
                    { v.FieldOfActivity = "-"; }
                    if (v.Occupation == null || v.Occupation == "")
                    { v.Occupation = "-"; }
                }
                try
                {
                    volunteers = vol.Where(x => x.FieldOfActivity.Contains(searchedOccupation, StringComparison.InvariantCultureIgnoreCase) || x.Occupation.Contains(searchedOccupation, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                catch { }
            }
            if (searchedRemarks != null)
            {
                List<Volunteer> vol = volunteers;
                foreach (var v in vol)
                {
                    if (string.IsNullOrEmpty(v.AdditionalInfo.Remark))
                        v.AdditionalInfo.Remark = "";
                }
                try
                {
                    volunteers = vol.Where(x => x.AdditionalInfo.Remark.Contains(searchedRemarks, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                catch { }
            }

            if (searchedContact != null)
            {
                List<Volunteer> vol = volunteers;
                foreach (var v in vol)
                {
                    if (v.ContactInformation.PhoneNumber == null || v.ContactInformation.PhoneNumber == "")
                        v.ContactInformation.PhoneNumber = "-";
                    if (v.ContactInformation.EmailAddress == null || v.ContactInformation.EmailAddress == "")
                        v.ContactInformation.EmailAddress = "-";
                }
                try
                {
                    volunteers = vol.Where(x => x.ContactInformation.PhoneNumber.Contains(searchedContact, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.EmailAddress.Contains(searchedContact, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                catch { }
            }
            if (searchedHourCount != 0)
            {
                volunteers = volunteers.Where(x => x.HourCount.Equals(searchedHourCount)).ToList();
            }

            if (lowerdate > d1)
            {
                volunteers = volunteers.Where(x => x.Birthdate > lowerdate).ToList();
            }
            if (upperdate > d1)
            {
                volunteers = volunteers.Where(x => x.Birthdate <= upperdate).ToList();
            }
            if (gender != " All")
            {
                if (gender == "Male")
                {
                    volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Male)).ToList();
                }
                if (gender == "Female")
                { volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Female)).ToList(); }
            }

            if (hasDrivingLicence)
            {
                volunteers = volunteers.Where(x => x.AdditionalInfo.HasDrivingLicense == true).ToList();
            }
            if (hasCar)
            {
                volunteers = volunteers.Where(x => x.AdditionalInfo.HasCar == true).ToList();
            }
            return volunteers;
        }

        internal static string GetIdAndFieldString(string IDS, bool all, bool name, bool birthdate, bool address, bool gender, bool DesiredWorkplace, bool cnp, bool FieldOfActivity, bool occupation, bool cI_Info, bool activity, bool hour_Count, bool contact_Information, bool additional_info)
        {
            string ids_and_options = IDS + "(((";
            if (all)
                ids_and_options += "0";
            if (name)
                ids_and_options += "1";
            if (birthdate)
                ids_and_options += "2";
            if (address)
                ids_and_options += "3";
            if (gender)
                ids_and_options += "4";
            if (DesiredWorkplace)
                ids_and_options += "5";
            if (cnp)
                ids_and_options += "6";
            if (FieldOfActivity)
                ids_and_options += "7";
            if (occupation)
                ids_and_options += "8";
            if (cI_Info)
                ids_and_options += "9";
            if (activity)
                ids_and_options += "A";
            if (hour_Count)
                ids_and_options += "B";
            if (contact_Information)
                ids_and_options += "C";
            if (additional_info)
                ids_and_options += "D";
            return ids_and_options;
        }

        internal static List<Volunteer> GetVolunteersWithBirthdays(List<Volunteer> volunteers)
        {
            int currentday = UniversalHelpers.UniversalFunctions.GetDayOfYear(DateTime.Today);
            List<Volunteer> returnlistofvols = new List<Volunteer>();
            foreach (var vol in volunteers)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(vol.Birthdate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    returnlistofvols.Add(vol);
                }
            }
            return returnlistofvols;
        }
    }
}