using System;
using System.ComponentModel.DataAnnotations;

namespace VolCommon
{
    public class CI
    {
        public bool HasId { get; set; }

        public string CIseria { get; set; }

        public string CINr { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CIEliberat { get; set; }

        public string CIeliberator { get; set; }
        public string CIinfo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDateCI { get; set; }

        //public static string GetBetween(string strSource, string strStart, string strEnd)
        //{
        //    int Start, End;
        //    if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        //    {
        //        Start = strSource.IndexOf(strStart, 0) + strStart.Length;
        //        End = strSource.IndexOf(strEnd, Start);
        //        return strSource.Substring(Start, End - Start);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
    }
}