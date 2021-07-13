using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.HomeControllerGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Contexts.HomeControllerContexts
{
    public class HomeControllerIndexDisplayContext
    {
        private readonly IHomeControllerIndexDisplayGateway dataGateway;

        public HomeControllerIndexDisplayContext(IHomeControllerIndexDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public Response Execute()
        {
            var response = new Response();
            var volunteerContracts = dataGateway.GetListVolunteerContracts();
            var beneficiarycontracts = dataGateway.GetListOfBeneficiariesContracts();
            var volunteers = dataGateway.GetListOfVolunteers();
            var sponsors = dataGateway.GetListOfSponsors();
            response.BirthdayOfVolunteersNumber = GetNumberOfVolunteersWithBirthdays(volunteers);
            response.VolunteerContractExpirationNumber = GetNumberOfExpiringVolContracts(volunteerContracts);
            response.SponsorContractExpirationNumber= GetNumberOfExpiringSponsorContracts(sponsors);
            response.BeneficiaryContractExpirationNumber= GetNumberOfExpiringBeneficiaryContracts(beneficiarycontracts);

            response.Settings= dataGateway.GetSettingItem();

            return response;

        }

        public int GetNumberOfVolunteersWithBirthdays(List<Volunteer> volunteers)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int birthdayCounter = 0;
            foreach (var item in volunteers)
            {
                int volunteerbirthday = GetDayOfYear(item.Birthdate);
                if (IsAboutToExpire(currentDay, volunteerbirthday))
                {
                    birthdayCounter++;
                }
            }
            return birthdayCounter;
        }
        public int GetNumberOfExpiringBeneficiaryContracts(List<BeneficiaryContract> beneficiaryContracts)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int beneficiaryContractsCounter = 0;
            foreach (var item in beneficiaryContracts)
            {
                int dayToCompareTo = GetDayOfYear(item.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo))
                {
                    beneficiaryContractsCounter++;
                }
            }
            return beneficiaryContractsCounter;
        }

        public int GetNumberOfExpiringSponsorContracts(List<Sponsor> sponsors)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int sponsorContractsCounter = 0;
            foreach (var item in sponsors)
            {
                int dayToCompareTo = GetDayOfYear(item.Contract.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo))
                {
                    sponsorContractsCounter++;
                }
            }
            return sponsorContractsCounter;
        }

        public int GetNumberOfExpiringVolContracts(List<VolunteerContract> volunteerContracts)
        {
            int currentDay = GetDayOfYear(DateTime.Today);
            int volunteerContractsCounter = 0;
            foreach (var item in volunteerContracts)
            {
                int dayToCompareTo = GetDayOfYear(item.ExpirationDate);
                if (IsAboutToExpire(currentDay, dayToCompareTo))
                {
                    volunteerContractsCounter++;
                }
            }
            return volunteerContractsCounter;
        }

        public  bool IsAboutToExpire(int currentDay, int dayToCompareTo)
        {
            if (currentDay <= dayToCompareTo && currentDay + 10 > dayToCompareTo || currentDay > 355 && dayToCompareTo < 9)
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
