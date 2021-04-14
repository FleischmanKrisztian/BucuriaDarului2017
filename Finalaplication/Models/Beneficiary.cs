using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VolCommon;

namespace Finalaplication.Models
{
    public class Beneficiary : BeneficiaryBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BeneficiaryID { get; set; }
    }
}