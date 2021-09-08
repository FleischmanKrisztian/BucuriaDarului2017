using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.HomeControllerGateways;
using BucuriaDarului.Gateway.SettingsGateways;
using BucuriaDarului.Gateway.SponsorGateways;
using BucuriaDarului.Gateway.VolunteerContractGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.HomeController
{
    public class HomeControllerIndexDisplayGateway : IHomeControllerIndexDisplayGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<BeneficiaryContract> GetListOfBeneficiaryContracts()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var benecontractcollection = dbContext.Database.GetCollection<BeneficiaryContract>("BeneficiaryContracts");
            var contracts = benecontractcollection.AsQueryable().ToList();
            return contracts;
        }

        public List<Sponsor> GetListOfSponsors()
        {
            return ListSponsorsGateway.GetListOfSponsors();
        }

        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }

        public List<AdditionalContractVolunteer> GetListVolunteerAdditionalContracts()
        {
            return ListVolunteerAdditionalContractGateway.GetListAdditionalContracts();
        }

        public List<VolunteerContract> GetListVolunteerContracts()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("VolunteerContracts");
            List<VolunteerContract> contracts = volunteerContractCollection.AsQueryable().ToList();
            return contracts;
        }

        public Settings GetSettingItem()
        {
            return SingleSettingReturnerGateway.GetSettingItem();
        }
    }
}