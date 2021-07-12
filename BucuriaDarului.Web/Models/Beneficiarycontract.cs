using System;
using System.ComponentModel.DataAnnotations;

namespace BucuriaDarului.Web.Models
{
    public class Beneficiarycontract
    {
        public string Id { get; set; }

        public string OwnerID { get; set; }

        public string Fullname { get; set; }

        public string NumberOfPortion { get; set; }

        public string CNP { get; set; }

        public string Address { get; set; }

        public string Nrtel { get; set; }

        public string CIinfo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthdate { get; set; }

        public string NumberOfRegistration { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RegistrationDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDate { get; set; }

        public string IdInvestigation { get; set; }

        public string IdApplication { get; set; }
    }
}