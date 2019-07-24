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
        public ObjectId BeneficiaryID { get; set; }


        public static int Benefxp(Beneficiary beneficiary)
        {
            int benefexp;
            {
                benefexp = (beneficiary.Contract.ExpirationDate.Month - 1) * 30 + beneficiary.Contract.ExpirationDate.Day;
            }
            return benefexp;
        }
    }
}
