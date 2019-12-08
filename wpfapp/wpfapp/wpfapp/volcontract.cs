using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfapp
{
    class volcontract
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string CNP { get; set; }

        public string Address { get; set; }

        public string Nrtel { get; set; }

        public DateTime Birthdate { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string CIseria { get; set; }

        public string CINr { get; set; }

        public DateTime CIEliberat { get; set; }

        public string CIeliberator { get; set; }

        public bool InActivity { get; set; }

        public int HourCount { get; set; }
    }
}
