using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolCommon
{
    public class BeneficiaryBase
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public bool Active { get; set; }

        public bool Canteen { get; set; }

        public string HomeDeliveryDriver { get; set; }

        public bool HasGDPR { get; set; }

        public Address Address { get; set; }

        public CI CI { get; set; }

        public Marca Marca { get; set; }

        public Contract Contract { get; set; }

        public PersonalInfo PersonalInfo { get; set; }






    }
}
