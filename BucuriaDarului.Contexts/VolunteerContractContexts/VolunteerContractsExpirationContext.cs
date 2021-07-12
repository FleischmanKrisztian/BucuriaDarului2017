using System;
using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractsExpirationContext
    {
        private readonly IListDisplayVolunterContractsGateway dataGateway;

        public VolunteerContractsExpirationContext(IListDisplayVolunterContractsGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public List<VolunteerContract> Execute()
        {
            var contracts = dataGateway.GetListVolunteerContracts();
            contracts = GetExpiringContracts(contracts);
            return contracts;

        }

        internal static List<VolunteerContract> GetExpiringContracts(List<VolunteerContract> contracts)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            List<VolunteerContract> returnListOfVolunteerContracts = new List<VolunteerContract>();
            foreach (var contract in contracts)
            {
                int dayToCompare = GetDayOfYear(contract.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompare))
                {
                    returnListOfVolunteerContracts.Add(contract);
                }
            }
            return returnListOfVolunteerContracts;
        }

        public static bool IsAboutToExpire(int currentDay, int dayToCompare)
        {
            if (currentDay <= dayToCompare && currentDay + 10 > dayToCompare || currentDay > 355 && dayToCompare < 9)
            {
                return true;
            }
            return false;
        }

        internal static int GetDayOfYear(DateTime date)
        {
            string dateAsString = date.ToString("dd-MM-yyyy");
            string[] dates = dateAsString.Split('-');
            int Day = Convert.ToInt16(dates[0]);
            int Month = Convert.ToInt16(dates[1]);
            Day = (Month - 1) * 30 + Day;
            return Day;
        }
    }
}
