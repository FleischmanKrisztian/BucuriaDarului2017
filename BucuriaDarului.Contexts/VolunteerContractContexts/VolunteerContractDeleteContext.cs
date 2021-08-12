using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractDeleteContext
    {
        private readonly IVolunteerContractDeleteGateway dataGateway;
        public VolunteerContractDeleteContext(IVolunteerContractDeleteGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerContractDeleteResponse Execute(VolunteerContractDeleteRequest request)
        {
            var response = new VolunteerContractDeleteResponse();
            response.VolunteerId = request.OwnerID;
            dataGateway.Delete(request.ContractId);
            var contracts = dataGateway.GetListVolunteerContracts();
            if (contracts.Find(x => x.Id == request.ContractId) != null)
                response.IsValid = false;
            return response;
        }
    }

    public class VolunteerContractDeleteRequest
    {
        public string ContractId { get; set; }
        public string OwnerID { get; set; }

        public string Fullname { get; set; }

        public string CNP { get; set; }

        public string Address { get; set; }

        public int HourCount { get; set; }

        public string PhoneNumber { get; set; }

        public CI CI { get; set; }

        public DateTime Birthdate { get; set; }

        public string NumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }

    public class VolunteerContractDeleteResponse
    {
        public string VolunteerId { get; set; }
        public bool IsValid { get; set; }

        public VolunteerContractDeleteResponse()
        {
            IsValid = true;
        }

    }
}