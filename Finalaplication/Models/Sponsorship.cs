using System;
using System.ComponentModel.DataAnnotations;

namespace Finalaplication.Models
{
    public class Sponsorship
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }
        public TypeOfSupport TypeOfSupport { get; set; }
        public int MoneyAmount { get; set; }
        public string WhatGoods { get; set; }
        public string GoodsAmount { get; set; }

    }
}

       

    

