using System;
using System.Text.Json.Serialization;

namespace BucuriaDarului.Core
{
    public enum Gender
    {
        Male, Female
    }

    public class Volunteer {
      
        public string Id { get; set; }

        public string Fullname { get; set; }

        public DateTime Birthdate { get; set; }

        public Address Address { get; set; }

        public Gender Gender { get; set; }

        public string Desired_workplace { get; set; }

        public string CNP { get; set; }

        public string Field_of_activity { get; set; }

        public string Occupation { get; set; }

        public string CIseria { get; set; }

        public string CINr { get; set; }

        public DateTime CIEliberat { get; set; }

        public string CIeliberator { get; set; }

        public bool InActivity { get; set; }

        public int HourCount { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public AdditionalInfo Additionalinfo { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
    }

}
