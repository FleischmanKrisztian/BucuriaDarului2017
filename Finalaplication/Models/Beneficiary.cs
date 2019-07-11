using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.Models
{
    public class Beneficiary
    {
        [BsonId]
        public MongoDB.Bson.ObjectId BeneficiaryID { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Status { get; set; }
            public bool Weeklypackage { get; set; }
            public bool Canteen { get; set; }
            public bool HomeDeliveryDriver { get; set; }
            public bool HasGDPRAgreement { get; set; }
            public Adress Adress { get; set; }
            public int CNP { get; set; }
            public CI CI { get; set; }
            public Marca Marca { get; set; }
            public int ContractPeriode { get; set; }
            public int NumberOfPortions { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastTimeActiv { get; set; }
            public string Coments { get; set; }
            public Personalinfo PersonalInfo { get; set; }
        }

        public class Adress
        {
            public string Street { get; set; }
            public int Number { get; set; }
            public string City { get; set; }
        }

        public class CI
        {
            public bool HasId { get; set; }
            public DateTime ExpirationDate { get; set; }
        }

        public class Marca
        {
            public int IdAplication { get; set; }
            public int IdInvestigation { get; set; }
            public int IdContract { get; set; }
        }

        public class Personalinfo
        {
            public int PhoneNumber { get; set; }
            public string BirthPLace { get; set; }
            public string Studies { get; set; }
            public string Profesion { get; set; }
            public string Ocupation { get; set; }
            public string SeniorityInWorkField { get; set; }
            public string HealthState { get; set; }
            public string Disalility { get; set; }
            public string ChronicCondition { get; set; }
            public string Dependent { get; set; }
            public string HealthInsurance { get; set; }
            public string HealthCard { get; set; }
            public string Married { get; set; }
            public string SpouseName { get; set; }
            public bool HasHome { get; set; }
            public string HousingType { get; set; }
            public string Income { get; set; }
            public string Expences { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public DateTime BirthDate { get; set; }
        }

    }

