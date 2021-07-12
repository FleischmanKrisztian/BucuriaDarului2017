using System;
using System.Collections.Generic;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorContractsExpirationContext
    {
        private readonly IListDisplaySponsorContractsGateway dataGateway;

        public SponsorContractsExpirationContext(IListDisplaySponsorContractsGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public List<Sponsor> Execute()
        {
            var sponsors = dataGateway.GetListSponsorContracts();
            sponsors = GetExpiringContracts(sponsors);
            return sponsors;

        }

        internal static List<Sponsor> GetExpiringContracts(List<Sponsor> sponsors)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            List<Sponsor> returnlistofspons = new List<Sponsor>();
            foreach (var spons in sponsors)
            {
                int dayToCompare = GetDayOfYear(spons.Contract.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompare))
                {
                    returnlistofspons.Add(spons);
                }
            }
            return returnlistofspons;
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
