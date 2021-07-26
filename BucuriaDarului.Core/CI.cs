using System;
using System.ComponentModel.DataAnnotations;

namespace BucuriaDarului.Core
{
    public class CI
    {
        public bool HasId { get; set; }

        public string Info { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}