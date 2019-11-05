using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Models
{
    public class BeneficiaryFromCsv
    {
        public string Index { get; set; }
        public string Fullname { get; set; }

        public string Active { get; set; }
        public string Weeklypackage { get; set; }
        public string Canteen { get; set; }
        public string HomeDeliveryDriver { get; set; }
        public string HasGDPRAgreement { get; set; }
        public string Adress { get; set; }
        public string CNP { get; set; }
        public string CI { get; set; }
        public string IdAplication { get; set; }
        public string IdInvestigation { get; set; }
        public string IdContract { get; set; }
        public string NumberOfPortions { get; set; }

        public string LastTimeActiv { get; set; }

        public string Coments { get; set; }
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
        public string HealthInsurance { get; set; }
        public string HealthCard { get; set; }
        public string Married { get; set; }
        public string SpouseName { get; set; }
        public string HasHome { get; set; }
        public string HousingType { get; set; }
        public string Income { get; set; }
        public string Expences { get; set; }
        public string Gender { get; set; }

      
    }
}
