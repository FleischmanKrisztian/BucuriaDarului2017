using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

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

        internal static bool DifferentFormat(string header)
        {
            if (header.Count() >= 22 && header.Contains("Gender") == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static Volunteer GetVolunteerFromString(string[] volunteerstring)
        {
            Volunteer volunteer = new Volunteer();
            volunteer.Firstname = volunteerstring[0];
            volunteer.Lastname = volunteerstring[1];
            try
            {
                volunteer.Birthdate = Convert.ToDateTime(volunteerstring[2]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                volunteer.Birthdate = DateTime.MinValue;
            }
            Address address = new Address();

            if (volunteerstring[3] != null || volunteerstring[3] != "")
            {
                address.District = volunteerstring[3];
            }
            else { address.District = "-"; }

            if (volunteerstring[4] != null || volunteerstring[5] != "")
            {
                address.City = volunteerstring[4];
            }
            else
            {
                address.City = "-";
            }

            if (volunteerstring[5] != null || volunteerstring[5] != "")
            {
                address.Street = volunteerstring[5];
            }
            else { address.Street = "-"; }

            if (volunteerstring[6] != null || volunteerstring[6] != "")
            {
                address.Number = volunteerstring[6];
            }
            else
            {
                address.Number = "-";
            }
            try
            {
                if (volunteerstring[7] == "1")
                {
                    volunteer.Gender = VolCommon.Gender.Female;
                }
                else
                {
                    volunteer.Gender = VolCommon.Gender.Male;
                }
            }
            catch
            {
                volunteer.Gender = VolCommon.Gender.Male;
            }
            volunteer.Desired_workplace = volunteerstring[8];
            volunteer.CNP = volunteerstring[9];
            volunteer.Field_of_activity = volunteerstring[10];
            volunteer.Occupation = volunteerstring[11];
            volunteer.CIseria = volunteerstring[12];
            volunteer.CINr = volunteerstring[13];
            try
            {
                volunteer.CIEliberat = Convert.ToDateTime(volunteerstring[14]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                volunteer.CIEliberat = DateTime.MinValue;
            }
            volunteer.CIeliberator = volunteerstring[15];
            volunteer.InActivity = Convert.ToBoolean(volunteerstring[16]);
            if (volunteerstring[17] != null || volunteerstring[17] != "")
            {
                volunteer.HourCount = Convert.ToInt16(volunteerstring[17]);
            }
            else
            {
                volunteer.HourCount = 0;
            }
            ContactInformation contactinformation = new ContactInformation();
            if (volunteerstring[18] != null || volunteerstring[18] != "")
            {
                contactinformation.PhoneNumber = volunteerstring[18];
            }
            else
            {
                contactinformation.PhoneNumber = "-";
            }
            if (volunteerstring[19] != null || volunteerstring[19] != "")
            {
                contactinformation.MailAdress = volunteerstring[19];
            }
            else
            {
                contactinformation.MailAdress = "-";
            }
            volunteer.ContactInformation = contactinformation;
            Additionalinfo additionalInformation = new Additionalinfo();

            if (volunteerstring[20] == "True")
            {
                additionalInformation.HasDrivingLicence = true;
            }
            else
            {
                additionalInformation.HasDrivingLicence = false;
            }

            if (volunteerstring[21] == "True")
            {
                additionalInformation.HasCar = true;
            }
            else
            {
                additionalInformation.HasCar = false;
            }
            additionalInformation.Remark = volunteerstring[22];
            volunteer.Additionalinfo = additionalInformation;
            return volunteer;
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
            var Day = Volunteer.Nowdate();
            List<Volunteer> returnlistofvols = new List<Volunteer>();
            foreach (var vol in volunteers)
            {
                var voldays = Volunteer.Volbd(vol);
                if (Day <= voldays && Day + 10 > voldays || Day > 354 && 365 - (Day + 365 - Day - 2) >= voldays)
                {
                    returnlistofvols.Add(vol);
                }
            }
            return returnlistofvols;
        }
    }
}
