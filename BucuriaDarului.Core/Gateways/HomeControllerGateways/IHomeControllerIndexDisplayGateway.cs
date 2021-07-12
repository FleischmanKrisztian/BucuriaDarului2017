using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways.HomeControllerGateways
{
    public interface IHomeControllerIndexDisplayGateway
    {
        List<VolunteerContract> GetListVolunteerContracts();

        List<BeneficiaryContract> GetListOfBeneficiariesContracts();

        List<Volunteer> GetListOfVolunteers();

        List<Sponsor> GetListOfSponsors();

        Settings GetSettingItem();
    }
}