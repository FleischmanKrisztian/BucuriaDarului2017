using System;
using System.ComponentModel.DataAnnotations;

namespace VolCommon
{
    public class CI
    {
        public bool HasID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ICExpiration { get; set; }
    }
}
