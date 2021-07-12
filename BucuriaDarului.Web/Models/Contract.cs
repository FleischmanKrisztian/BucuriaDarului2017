using System;
using System.ComponentModel.DataAnnotations;

namespace BucuriaDarului.Web.Models
{
    public class Contract
    {
        public bool HasContract { get; set; }

        public string NumberOfRegistration { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RegistrationDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDate { get; set; }
    }
}