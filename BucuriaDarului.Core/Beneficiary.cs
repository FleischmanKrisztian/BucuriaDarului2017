using System;

namespace BucuriaDarului.Core
{
    public class Beneficiary
    {
        public string Id { get; set; }

        public string Fullname { get; set; }

        public bool Active { get; set; }

        public bool WeeklyPackage { get; set; }

        public bool Canteen { get; set; }

        public bool HomeDelivery { get; set; }

        public string HomeDeliveryDriver { get; set; }

        public bool HasGDPRAgreement { get; set; }

        public string Address { get; set; }

        public string CNP { get; set; }

        public CI CI { get; set; }

        public Marca Marca { get; set; }

        public int NumberOfPortions { get; set; }

        public DateTime LastTimeActive { get; set; }

        public string Comments { get; set; }

        public PersonalInfo PersonalInfo { get; set; }

        public byte[] Image { get; set; }
    }
}