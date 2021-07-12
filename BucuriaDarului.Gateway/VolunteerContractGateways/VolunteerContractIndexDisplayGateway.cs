using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.VolContractGateways
{
    public class VolunteerContractIndexDisplayGateway : IVolunteerContractMainDisplayGateway
    {
        private MongoDBGateway dbContext = new MongoDBGateway();

        public List<VolunteerContract> GetListVolunteerContracts()
        {

            List<VolunteerContract> contracts = ListVolunteerContractGateway.GetListVolunteerContracts();
            return contracts;
        }

        public Volunteer GetVolunteer(string idOfVolunteer)
        {
            return SingleVolunteerReturnerGateway.ReturnVolunteer(idOfVolunteer);
        }
    }
}