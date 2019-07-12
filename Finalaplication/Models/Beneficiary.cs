using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Models
{
    public class Beneficiary
    {
        [BsonId]
        public ObjectId BeneficiaryID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Status { get; set; }
        public bool Weeklypackage { get; set; }
        public bool Canteen { get; set; }
        public string HomeDeliveryDriver { get; set; }
        public bool HasGDPRAgreement { get; set; }
        public Address Adress { get; set; }
        public int CNP { get; set; }
        public CI CI { get; set; }
        public Marca Marca { get; set; }
        public int NumberOfPortions { get; set; }
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastTimeActiv { get; set; }
        public string Coments { get; set; }
        public Personalinfo PersonalInfo { get; set; }
        public Contract Contract { get; set; }


        public static int Benefxp(Beneficiary beneficiary)
        {
            int benefexp;
            {
                benefexp = (beneficiary.Contract.ExpirationDate.Month - 1) * 30 + beneficiary.Contract.ExpirationDate.Day;
            }
            return benefexp;
        }
    }
    
        }

       


