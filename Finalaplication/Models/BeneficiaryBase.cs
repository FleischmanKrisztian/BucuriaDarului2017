using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace VolCommon
{
    public class BeneficiaryBase
    {
        [Required]
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

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastTimeActiv { get; set; }

        public string Coments { get; set; }
        public Personalinfo PersonalInfo { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
    }
}
