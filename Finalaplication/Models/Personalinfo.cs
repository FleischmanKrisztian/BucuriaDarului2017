using System;
using System.ComponentModel.DataAnnotations;

namespace VolCommon
{
    public class Personalinfo
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthdate { get; set; }

        public string PhoneNumber { get; set; }
        public string BirthPlace { get; set; }
        public string Studies { get; set; }
        public string Profesion { get; set; }
        public string Ocupation { get; set; }
        public string SeniorityInWorkField { get; set; }
        public string HealthState { get; set; }
        public string Disalility { get; set; }
        public string ChronicCondition { get; set; }
        public string Addictions { get; set; }
        public bool HealthInsurance { get; set; }
        public bool HealthCard { get; set; }
        public string Married { get; set; }
        public string SpouseName { get; set; }
        public bool HasHome { get; set; }
        public string HousingType { get; set; }
        public string Income { get; set; }
        public string Expences { get; set; }
        public Gender Gender { get; set; }
    }
}