using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core
{
    public class Settings
    {
        public string Id { get; set; }

        public string Lang { get; set; }

        public int Quantity { get; set; }

        public int NumberOfDaysBeforBirthday { get; set; }

        public int NumberOfDaysBeforeExpiration { get; set; }
    }
}
