using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorContractsExpirationContext
    {
        private readonly IListDisplaySponsorContractsGateway dataGateway;

        public SponsorContractsExpirationContext(IListDisplaySponsorContractsGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public List<Sponsor> Execute(int nrOfDaysBeforExpiration)
        {
            var sponsors = dataGateway.GetListSponsorContracts();
            sponsors = GetExpiringContracts(sponsors, nrOfDaysBeforExpiration);
            return sponsors;
        }

        internal static List<Sponsor> GetExpiringContracts(List<Sponsor> sponsors, int nrOfDaysBeforExpiration)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            List<Sponsor> returnlistofspons = new List<Sponsor>();
            foreach (var spons in sponsors)
            {
                int dayToCompare = GetDayOfYear(spons.Contract.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompare, nrOfDaysBeforExpiration))
                {
                    returnlistofspons.Add(spons);
                }
            }
            return returnlistofspons;
        }

        public static bool IsAboutToExpire(int currentDay, int dayToCompare, int nrOfDaysBeforExpiration)
        {
            if (currentDay <= dayToCompare && currentDay + nrOfDaysBeforExpiration > dayToCompare || currentDay > 355 && dayToCompare < nrOfDaysBeforExpiration - 1)
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