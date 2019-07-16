using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using VolCommon;


namespace Finalaplication.Models
{

    public class Beneficiary : BeneficiaryBase
    {
        [BsonId]
        public ObjectId BeneficiaryID { get; set; }


        public static int Volbd(Beneficiary vol)
        {
            int voldays;
            {
                voldays = (vol.PersonalInfo.Birthdate.Month - 1) * 30 + vol.PersonalInfo.Birthdate.Day;
            }
            return voldays;
        }
    }
}