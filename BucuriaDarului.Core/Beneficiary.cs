using System;

namespace BucuriaDarului.Core
{
    public class Beneficiary
    {
        public string Id { get; set; }

        public string Fullname { get; set; }

        public bool Active { get; set; }

        public bool Weeklypackage { get; set; }

        public bool Canteen { get; set; }

        public bool HomeDelivery { get; set; }

        public string HomeDeliveryDriver { get; set; }

        public bool HasGDPRAgreement { get; set; }

        public string Adress { get; set; }

        public string CNP { get; set; }

        public CI CI { get; set; }

        public Marca Marca { get; set; }

        public int NumberOfPortions { get; set; }

        public DateTime LastTimeActiv { get; set; }

        public string Comments { get; set; }

        public Personalinfo PersonalInfo { get; set; }

        public byte[] Image { get; set; }
    }
}