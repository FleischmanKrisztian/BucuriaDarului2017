using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.HomeControllerGateways
{
    public interface IHomeControllerIndexDisplayGateway
    {
        List<VolunteerContract> GetListVolunteerContracts();

        List<BeneficiaryContract> GetListOfBeneficiaryContracts();

        List<Volunteer> GetListOfVolunteers();

        List<Sponsor> GetListOfSponsors();

        Settings GetSettingItem();
    }
}