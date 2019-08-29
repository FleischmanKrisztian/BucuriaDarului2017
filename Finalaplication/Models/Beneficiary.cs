using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolCommon;

namespace Finalaplication.Models
{
    public class Beneficiary:BeneficiaryBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BeneficiaryID { get; set; }


        public static int Benefxp(Beneficiary beneficiary)
        {
            int benefexp;
            {
                benefexp = (beneficiary.Contract.ExpirationDate.Month - 1) * 30 + beneficiary.Contract.ExpirationDate.Day;
            }
            return benefexp;

        }
        public bool GetDayExpiration(DateTime date)
        {
            var now = DateTime.Now;
            var firstday = now.AddDays(-1);
            var lastday = now.AddDays(10);
            var answer = false;
            if (date >= firstday && date <= lastday)
            {
                answer = true;
            }
            return answer;
        }
    }
}
