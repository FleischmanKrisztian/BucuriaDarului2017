using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;

namespace Finalaplication.Models
{
    public class CsvBeneficiaryMapping : CsvMapping<BeneficiaryFromCsv>
    {
        public CsvBeneficiaryMapping() : base()
        {
            MapProperty(0, x => x.Index);
            MapProperty(1, x => x.Fullname);
            MapProperty(3, x => x.Active);
            MapProperty(7, x => x.HomeDeliveryDriver);
            MapProperty(6, x => x.Weeklypackage);
            MapProperty(9, x => x.CNP);
            MapProperty(17, x => x.NumberOfPortions);
            MapProperty(18, x => x.PhoneNumber);
            MapProperty(19, x => x.BirthPlace);
            MapProperty(20, x => x.Studies);
            MapProperty(21, x => x.Profesion);
            MapProperty(22, x => x.Ocupation);
            MapProperty(23, x => x.SeniorityInWorkField);
            MapProperty(24, x => x.HealthState);
            MapProperty(25, x => x.Disalility);
            MapProperty(26, x => x.ChronicCondition);
            MapProperty(27, x => x.Addictions);
            MapProperty(28, x => x.HealthInsurance);
            MapProperty(29, x => x.HealthCard);
            MapProperty(30, x => x.Married);
            MapProperty(31, x => x.SpouseName);
            MapProperty(32, x => x.HousingType);
            MapProperty(33, x => x.HasHome);
            MapProperty(34, x => x.Income);
            MapProperty(35, x => x.Expences);
            //MapProperty(36, x => x.Birthdate.Year);
            //MapProperty(37, x => x.Birthdate.Month);
            //MapProperty(38, x => x.Birthdate.Day);
            MapProperty(41, x => x.Gender);
            MapProperty(42, x => x.Coments);
           
        }
    }
}
