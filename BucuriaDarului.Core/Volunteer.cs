using System;

namespace BucuriaDarului.Core
{
    public enum Gender
    {
        NotSpecified,Male, Female
    }

    public class Volunteer
    {
        public string Id { get; set; }

        public string Fullname { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }

        public Gender Gender { get; set; }

        public string DesiredWorkplace { get; set; }

        public string CNP { get; set; }

        public string FieldOfActivity { get; set; }

        public string Occupation { get; set; }

        public CI CI { get; set; }

        public bool InActivity { get; set; }

        public int HourCount { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public AdditionalInfo AdditionalInfo { get; set; }

        public byte[] Image { get; set; }
    }
}