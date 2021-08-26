using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractsExpirationContext
    {
        private readonly IListDisplayBeneficiaryContractsGateway dataGateway;

        public BeneficiaryContractsExpirationContext(IListDisplayBeneficiaryContractsGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public List<BeneficiaryContract> Execute(int nrOfDaysBeforExpiration)
        {
            var contracts = dataGateway.GetListBeneficiaryContracts();
            contracts = GetExpiringContracts(contracts, nrOfDaysBeforExpiration);
            return contracts;
        }

        internal static List<BeneficiaryContract> GetExpiringContracts(List<BeneficiaryContract> contracts, int nrOfDaysBeforExpiration)
        {
            var currentDay = GetDayOfYear(DateTime.Today);
            var returnListOfBeneficiaryContracts = new List<BeneficiaryContract>();
            foreach (var contract in contracts)
            {
                var dayToCompare = GetDayOfYear(contract.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompare, nrOfDaysBeforExpiration))
                {
                    returnListOfBeneficiaryContracts.Add(contract);
                }
            }
            return returnListOfBeneficiaryContracts;
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
            var dateAsString = date.ToString("dd-MM-yyyy");
            var dates = dateAsString.Split('-');
            int day = Convert.ToInt16(dates[0]);
            int month = Convert.ToInt16(dates[1]);
            day = (month - 1) * 30 + day;
            return day;
        }
    }
}