using System;
using System.ComponentModel.DataAnnotations;

namespace BucuriaDarului.Core
{
    public class CI
    {
        public bool HasId { get; set; }

        public string Info { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDate { get; set; }
    }
}