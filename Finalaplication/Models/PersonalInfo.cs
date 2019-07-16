using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VolCommon
{
    public class PersonalInfo
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthdate { get; set; }

        public string PhoneNumber { get; set; }

        public string BirthPlace { get; set; }

        public string Studies { get; set; }

        public string Profession { get; set; }

        public string Occupation { get; set; }

        public string Healthstate { get; set; }

        public string Disability { get; set; }

        public string ChronicCondition { get; set; }

        public bool Addict { get; set; }

        public bool HealthInsurance { get; set; }

        public bool HealthCard { get; set; }

        public bool Married { get; set; }

        public string SpouseName { get; set; }

        public bool Homeless { get; set; }

        public string Income { get; set; }

        public string Expenses { get; set; }

        public Gender Gender { get; set; }
    }
}
