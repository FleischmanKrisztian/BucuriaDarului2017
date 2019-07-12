using System;
using System.ComponentModel.DataAnnotations;

public class CI
        {
            public bool HasId { get; set; }
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CIExpirationDate { get; set; }
        }



