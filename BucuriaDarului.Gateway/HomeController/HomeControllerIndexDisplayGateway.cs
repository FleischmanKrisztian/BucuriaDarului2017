using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.HomeControllerGateways;
using BucuriaDarului.Gateway.SettingsGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

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
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var sponsorCollection = dbContext.Database.GetCollection<Sponsor>("Sponsors");
            // TODO: solve this exception
            var sponsors = sponsorCollection.AsQueryable().ToList();
            return sponsors;
        }

        
        public List<Volunteer> GetListOfVolunteers()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerCollection = dbContext.Database.GetCollection<Volunteer>("Volunteers");
            List<Volunteer> volunteers = volunteerCollection.AsQueryable().ToList();
            return volunteers;
        }

        public List<VolunteerContract> GetListVolunteerContracts()
        {
            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var volunteerContractCollection = dbContext.Database.GetCollection<VolunteerContract>("Contracts");
            List<VolunteerContract> contracts = volunteerContractCollection.AsQueryable().ToList();
            return contracts;
        }

        public Settings GetSettingItem()
        {
            return SingleSettingReturnerGateway.GetSettingItem();
        }
    }
}
