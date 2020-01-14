using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfapp
{
    class beneficiarycontract
    {
        public string ContractID { get; set; }
        public string OwnerID { get; set; }
        public string Fullname { get; set; }
        public string CNP { get; set; }
        public string Address { get; set; }
        public string Nrtel { get; set; }
        public string CIinfo { get; set; }
        public DateTime Birthdate { get; set; }
        public string NumberOfRegistration { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string NumberOfPortion { get; set; }
        public string IdInvestigation { get; set; }
        public string IdApplication { get; set; }
        public string ContractOption { get; set; }


    }
}
