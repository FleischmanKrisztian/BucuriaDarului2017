using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.HomeControllerGateways;
using System;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.HomeControllerContexts
{
    public class HomeControllerIndexDisplayContext
    {
        private readonly IHomeControllerIndexDisplayGateway dataGateway;

        public HomeControllerIndexDisplayContext(IHomeControllerIndexDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public Response Execute(int birthdayAlarm, int numberOfDaysBeforExpiration)
        {
            var response = new Response();
            var volunteerContracts = dataGateway.GetListVolunteerContracts();
            var volunteerAdditionalContracts = dataGateway.GetListVolunteerAdditionalContracts();
            var beneficiarycontracts = dataGateway.GetListOfBeneficiaryContracts();
            var volunteers = dataGateway.GetListOfVolunteers();
            var sponsors = dataGateway.GetListOfSponsors();
            response.BirthdayOfVolunteersNumber = GetNumberOfVolunteersWithBirthdays(volunteers, birthdayAlarm);
            response.VolunteerContractExpirationNumber = GetNumberOfExpiringVolContracts(volunteerContracts, volunteerAdditionalContracts, numberOfDaysBeforExpiration);
            response.SponsorContractExpirationNumber = GetNumberOfExpiringSponsorContracts(sponsors, numberOfDaysBeforExpiration);
            response.BeneficiaryContractExpirationNumber = GetNumberOfExpiringBeneficiaryContracts(beneficiarycontracts, numberOfDaysBeforExpiration);

            response.Settings = dataGateway.GetSettingItem();

            return response;
        }

        public int GetNumberOfVolunteersWithBirthdays(List<Volunteer> volunteers, int birthdayAlarm)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int birthdayCounter = 0;
            foreach (var item in volunteers)
            {
                int volunteerbirthday = GetDayOfYear(item.Birthdate);
                if (BirthdayAlarm(currentDay, volunteerbirthday, birthdayAlarm))
                {
                    birthdayCounter++;
                }
            }
            return birthdayCounter;
        }

        public int GetNumberOfExpiringBeneficiaryContracts(List<BeneficiaryContract> beneficiaryContracts, int numberOfDaysBeforExpiration)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int beneficiaryContractsCounter = 0;
            foreach (var item in beneficiaryContracts)
            {
                int dayToCompareTo = GetDayOfYear(item.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo, numberOfDaysBeforExpiration))
                {
                    beneficiaryContractsCounter++;
                }
            }
            return beneficiaryContractsCounter;
        }

        public int GetNumberOfExpiringSponsorContracts(List<Sponsor> sponsors, int numberOfDaysBeforExpiration)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int sponsorContractsCounter = 0;
            foreach (var item in sponsors)
            {
                int dayToCompareTo = GetDayOfYear(item.Contract.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo, numberOfDaysBeforExpiration))
                {
                    sponsorContractsCounter++;
                }
            }
            return sponsorContractsCounter;
        }

        public int GetNumberOfExpiringVolContracts(List<VolunteerContract> volunteerContracts, List<AdditionalContractVolunteer> additionalContracts, int numberOfDaysBeforExpiration)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int volunteerContractsCounter = 0;
            foreach (var item in volunteerContracts)
            {
                int dayToCompareTo = GetDayOfYear(item.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo, numberOfDaysBeforExpiration))
                {
                    volunteerContractsCounter++;
                }
            }

            foreach (var item in additionalContracts)
            {
                int dayToCompareTo = GetDayOfYear(item.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo, numberOfDaysBeforExpiration))
                {
                    volunteerContractsCounter++;
                }
            }
            return volunteerContractsCounter;
        }

        public bool IsAboutToExpire(int currentDay, int dayToCompareTo, int numberOfDaysBeforExpiration)
        {
            if (currentDay <= dayToCompareTo && currentDay + numberOfDaysBeforExpiration > dayToCompareTo || currentDay > 355 && dayToCompareTo < numberOfDaysBeforExpiration - 1)
            {
                return true;
            }
            return false;
        }

        public bool BirthdayAlarm(int currentDay, int dayToCompareTo, int alarmNumberOfDaysBeforExpiration)
        {
            if (currentDay <= dayToCompareTo && currentDay + alarmNumberOfDaysBeforExpiration > dayToCompareTo || currentDay > 355 && dayToCompareTo < alarmNumberOfDaysBeforExpiration - 1)
            {
                return true;
            }
            return false;
        }

        public int GetDayOfYear(DateTime date)
        {
            string dateAsString = date.ToString("dd-MM-yyyy");
            string[] dates = dateAsString.Split('-');
            int Day = Convert.ToInt16(dates[0]);
            int Month = Convert.ToInt16(dates[1]);
            Day = (Month - 1) * 30 + Day;
            return Day;
        }
    }

    public class Response
    {
        public int BeneficiaryContractExpirationNumber { get; set; }
        public int VolunteerContractExpirationNumber { get; set; }
        public int SponsorContractExpirationNumber { get; set; }
        public int BirthdayOfVolunteersNumber { get; set; }

        public Settings Settings { get; set; }
    }
}