using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VolCommon;

namespace Finalaplication.ControllerHelpers.VolunteerHelpers
{
    public class VolunteerFunctions
    {
        internal static string GetStringOfIds(List<Volunteer> volunteers)
        {
            string stringofids = "volunteerCSV";
            foreach (Volunteer vol in volunteers)
            {
                stringofids = stringofids + "," + vol._id;
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
                Volunteer singlevolunteer = volunteers.Where(x => x._id == vols[i]).First();
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

        internal static Volunteer GetVolunteerFromOtherString(string[] volunteerstring)
        {
            Volunteer volunteer = new Volunteer();
            volunteer._id = Guid.NewGuid().ToString();
            volunteer.Fullname = volunteerstring[0];

            if (volunteerstring[1] != null || volunteerstring[1] != "")
            {
                try
                {
                    DateTime myDate = DateTime.ParseExact(volunteerstring[1].Substring(1, 2) + "-" + volunteerstring[1].Substring(3, 2) + "-" + volunteerstring[1].Substring(5, 2), "yy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
                    volunteer.Birthdate = myDate.AddHours(5);
                }
                catch
                {
                    volunteer.Birthdate = DateTime.MinValue;
                }
            }

            Address a = new Address();

            if (volunteerstring[2] != null || volunteerstring[2] != "")
            {
                a.District = volunteerstring[2];
            }
            else
            {
                a.District = "-";
            }
            a.City = "-";
            a.Street = "-";
            a.Number = "-";

            volunteer.Gender = Gender.Male;

            if (volunteerstring[9] != null || volunteerstring[9] != "")
            {
                volunteer.Desired_workplace = volunteerstring[9];
            }
            else
            {
                volunteer.Desired_workplace = "-";
            }
            if (volunteerstring[1] != null || volunteerstring[1] != "")
            {
                volunteer.CNP = volunteerstring[1];
            }
            else
            {
                volunteer.CNP = "-";
            }

            if (volunteerstring[3] != null)
            {
                if (volunteerstring[3] != "")
                {
                    string[] splited = volunteerstring[3].Split(" ");
                    volunteer.CIseria = splited[0];
                    volunteer.CINr = splited[1];
                }
                else
                {
                    volunteer.CIseria = "Error";
                    volunteer.CINr = "Error";
                }
            }

            volunteer.CIEliberat = DateTime.MinValue;

            volunteer.HourCount = 0;

            ContactInformation c = new ContactInformation();
            if (volunteerstring[4] != null || volunteerstring[4] != "")
            {
                c.PhoneNumber = volunteerstring[4];
            }
            else
            {
                c.PhoneNumber = "-";
            }

            volunteer.ContactInformation = c;
            Additionalinfo ai = new Additionalinfo
            {
                HasDrivingLicence = false
            };
            if (volunteerstring[8] != null)
            {
                ai.Remark = volunteerstring[8];
            }
            else { ai.Remark = "-"; }
            ai.HasCar = false;

            volunteer.Occupation = "-";

            volunteer.Address = a;
            volunteer.Additionalinfo = ai;
            return volunteer;
        }

        internal static Volunteer GetVolunteerFromString(string[] volunteerstring)
        {
            Volunteer volunteer = new Volunteer();
            volunteer._id = Guid.NewGuid().ToString();
            volunteer.Fullname = volunteerstring[0];
            try
            {
                volunteer.Birthdate = Convert.ToDateTime(volunteerstring[1]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                volunteer.Birthdate = DateTime.MinValue;
            }
            Address address = new Address();

            if (volunteerstring[2] != null || volunteerstring[2] != "")
            {
                address.District = volunteerstring[2];
            }
            else { address.District = "-"; }

            if (volunteerstring[3] != null || volunteerstring[3] != "")
            {
                address.City = volunteerstring[3];
            }
            else
            {
                address.City = "-";
            }

            if (volunteerstring[4] != null || volunteerstring[4] != "")
            {
                address.Street = volunteerstring[4];
            }
            else { address.Street = "-"; }

            if (volunteerstring[5] != null || volunteerstring[5] != "")
            {
                address.Number = volunteerstring[5];
            }
            else
            {
                address.Number = "-";
            }
            volunteer.Address = address;
            try
            {
                if (volunteerstring[6] == "1")
                {
                    volunteer.Gender = Gender.Female;
                }
                else
                {
                    volunteer.Gender = Gender.Male;
                }
            }
            catch
            {
                volunteer.Gender = Gender.Male;
            }
            volunteer.Desired_workplace = volunteerstring[7];
            volunteer.CNP = volunteerstring[8];
            volunteer.Field_of_activity = volunteerstring[9];
            volunteer.Occupation = volunteerstring[10];
            volunteer.CIseria = volunteerstring[11];
            volunteer.CINr = volunteerstring[12];
            try
            {
                volunteer.CIEliberat = Convert.ToDateTime(volunteerstring[13]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                volunteer.CIEliberat = DateTime.MinValue;
            }
            volunteer.CIeliberator = volunteerstring[14];
            volunteer.InActivity = Convert.ToBoolean(volunteerstring[15]);
            if (volunteerstring[16] != null || volunteerstring[16] != "")
            {
                volunteer.HourCount = Convert.ToInt16(volunteerstring[16]);
            }
            else
            {
                volunteer.HourCount = 0;
            }
            ContactInformation contactinformation = new ContactInformation();
            if (volunteerstring[17] != null || volunteerstring[17] != "")
            {
                contactinformation.PhoneNumber = volunteerstring[17];
            }
            else
            {
                contactinformation.PhoneNumber = "-";
            }
            if (volunteerstring[18] != null || volunteerstring[18] != "")
            {
                contactinformation.MailAdress = volunteerstring[18];
            }
            else
            {
                contactinformation.MailAdress = "-";
            }
            volunteer.ContactInformation = contactinformation;
            Additionalinfo additionalInformation = new Additionalinfo();

            if (volunteerstring[19] == "True")
            {
                additionalInformation.HasDrivingLicence = true;
            }
            else
            {
                additionalInformation.HasDrivingLicence = false;
            }

            if (volunteerstring[20] == "True")
            {
                additionalInformation.HasCar = true;
            }
            else
            {
                additionalInformation.HasCar = false;
            }
            additionalInformation.Remark = volunteerstring[21];
            volunteer.Additionalinfo = additionalInformation;
            return volunteer;
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
                volunteers = volunteers.Where(x => x.Desired_workplace.Contains(searchedworkplace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (searchedOccupation != null)
            {
                List<Volunteer> vol = volunteers;
                foreach (var v in vol)
                {
                    if (v.Field_of_activity == null || v.Field_of_activity == "")
                    { v.Field_of_activity = "-"; }
                    if (v.Occupation == null || v.Occupation == "")
                    { v.Occupation = "-"; }
                }
                try
                {
                    volunteers = vol.Where(x => x.Field_of_activity.Contains(searchedOccupation, StringComparison.InvariantCultureIgnoreCase) || x.Occupation.Contains(searchedOccupation, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                catch { }
            }
            if (searchedRemarks != null)
            {
                List<Volunteer> vol = volunteers;
                foreach (var v in vol)
                {
                    if (v.Additionalinfo.Remark == null || v.Additionalinfo.Remark == "")
                        v.Additionalinfo.Remark = "";
                }
                try
                {
                    volunteers = vol.Where(x => x.Additionalinfo.Remark.Contains(searchedRemarks, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                    if (v.ContactInformation.MailAdress == null || v.ContactInformation.MailAdress == "")
                        v.ContactInformation.MailAdress = "-";
                }
                try
                {
                    volunteers = vol.Where(x => x.ContactInformation.PhoneNumber.Contains(searchedContact, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAdress.Contains(searchedContact, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                volunteers = volunteers.Where(x => x.Additionalinfo.HasDrivingLicence == true).ToList();
            }
            if (hasCar)
            {
                volunteers = volunteers.Where(x => x.Additionalinfo.HasCar == true).ToList();
            }
            return volunteers;
        }

        internal static string GetIdAndFieldString(string IDS, bool all, bool name, bool birthdate, bool address, bool gender, bool desired_Workplace, bool cnp, bool field_of_Activity, bool occupation, bool cI_Info, bool activity, bool hour_Count, bool contact_Information, bool additional_info)
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
            if (desired_Workplace)
                ids_and_options += "5";
            if (cnp)
                ids_and_options += "6";
            if (field_of_Activity)
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