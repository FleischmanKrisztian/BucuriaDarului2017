using System;

namespace BucuriaDarului.Core
{
    public class Contract
    {
        public bool HasContract { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}