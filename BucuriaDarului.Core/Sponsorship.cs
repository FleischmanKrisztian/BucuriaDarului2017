using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BucuriaDarului.Core
{
    public class Sponsorship
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        public string MoneyAmount { get; set; }

        public string WhatGoods { get; set; }

        public string GoodsAmount { get; set; }
    }
}
