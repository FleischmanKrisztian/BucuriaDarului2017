using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BucuriaDarului.Core
{
    public class Sponsor
    {
        [JsonIgnore]
        public string _id { get; set; }

        [Required]
        public string NameOfSponsor { get; set; }

        public Sponsorship Sponsorship { get; set; }

        public Contract Contract { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}