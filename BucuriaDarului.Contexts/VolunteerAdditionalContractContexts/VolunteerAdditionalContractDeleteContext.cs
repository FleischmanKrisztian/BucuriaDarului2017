using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Contexts.VolunteerAdditionalContractContexts
{
    public class VolunteerAdditionalContractDeleteContext
    {
        private readonly IVolunteerAdditionalContractDeleteGateway dataGateway;

        public VolunteerAdditionalContractDeleteContext(IVolunteerAdditionalContractDeleteGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerAdditionalContractDeleteResponse Execute(VolunteerAdditionalContractDeleteRequest request)
        {
            var response = new VolunteerAdditionalContractDeleteResponse();
            response.VolunteerId = request.OwnerID;
            response.ContractID = request.ContractID;
            dataGateway.Delete(request.Id);
            var contracts = dataGateway.GetListAdditionalContracts();
            if (contracts.Find(x => x.Id == request.Id) != null)
                response.IsValid = false;
            return response;
        }
    }
    public class VolunteerAdditionalContractDeleteRequest
    {
        public string Id { get; set; }
        public string ContractID { get; set; }
        public string OwnerID { get; set; }

        public string Fullname { get; set; }

        public string CNP { get; set; }

        public string Address { get; set; }

        public string HourCount { get; set; }

        public string PhoneNumber { get; set; }

        public CI CI { get; set; }

        public DateTime Birthdate { get; set; }

        public string ContractNumberOfRegistration { get; set; }
        public string AdditionalContractNumberOfRegistration { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }

    public class VolunteerAdditionalContractDeleteResponse
    {
        public string VolunteerId { get; set; }
        public string ContractID { get; set; }
        public bool IsValid { get; set; }

        public VolunteerAdditionalContractDeleteResponse()
        {
            IsValid = true;
        }
    }
}