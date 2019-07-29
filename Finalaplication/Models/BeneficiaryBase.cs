using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace VolCommon
{
    
    public class BeneficiaryBase
    {
      
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool Active { get; set; }
        public bool Weeklypackage { get; set; }
        public bool Canteen { get; set; }
        public string HomeDeliveryDriver { get; set; }
        public bool HasGDPRAgreement { get; set; }
        public Address Adress { get; set; }
        public string CNP { get; set; }
        public CI CI { get; set; }
        public Marca Marca { get; set; }
        public int NumberOfPortions { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastTimeActiv { get; set; }
        public string Coments { get; set; }
        public Personalinfo PersonalInfo { get; set; }
        public Contract Contract { get; set; }


        
    }
    
        }

       


