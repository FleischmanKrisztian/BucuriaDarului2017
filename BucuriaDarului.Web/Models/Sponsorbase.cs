using System.ComponentModel.DataAnnotations;

namespace BucuriaDarului.Web.Models
{
    public class SponsorBase
    {
        [Required]
        public string NameOfSponsor { get; set; }

        public Sponsorship Sponsorship { get; set; }
        public Contract Contract { get; set; }
        public ContactInformation ContactInformation { get; set; }
    }
}