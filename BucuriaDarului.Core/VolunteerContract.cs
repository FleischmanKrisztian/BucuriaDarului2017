using System;

namespace BucuriaDarului.Core
{
    internal class VolunteerContract
    {
        public string ID { get; set; }

        public string OwnerID { get; set; }

        public string Fullname { get; set; }

        public string CNP { get; set; }

        public string Address { get; set; }

        public int Hourcount { get; set; }

        public string PhoneNumber { get; set; }

        public CI CI { get; set; }

        public DateTime Birthdate { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}