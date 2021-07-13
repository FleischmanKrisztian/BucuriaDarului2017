﻿using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractsExpirationContext
    {
        private readonly IListDisplayVolunteerContractsGateway dataGateway;

        public VolunteerContractsExpirationContext(IListDisplayVolunteerContractsGateway dataGateway)
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
            var currentDay = GetDayOfYear(DateTime.Today);
            var returnListOfVolunteerContracts = new List<VolunteerContract>();
            foreach (var contract in contracts)
            {
                var dayToCompare = GetDayOfYear(contract.ExpirationDate);
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
            var dateAsString = date.ToString("dd-MM-yyyy");
            var dates = dateAsString.Split('-');
            int day = Convert.ToInt16(dates[0]);
            int month = Convert.ToInt16(dates[1]);
            day = (month - 1) * 30 + day;
            return day;
        }
    }
}