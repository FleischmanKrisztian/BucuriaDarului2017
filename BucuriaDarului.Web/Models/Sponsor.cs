using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VolCommon;

namespace Finalaplication.Models
{
    public class Sponsor
    {
        [JsonIgnore]
        public string Id { get; set; }

        [Required]
        public string NameOfSponsor { get; set; }

        public Sponsorship Sponsorship { get; set; }

        public Contract Contract { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}