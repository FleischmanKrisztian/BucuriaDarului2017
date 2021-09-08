using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerAdditionalContractContexts
{
    public class VolunteerAdditionalContractIndexDisplayContext
    {
        private readonly IVolunteerAdditionalContractMainDisplayGateway dataGateway;

        public VolunteerAdditionalContractIndexDisplayContext(IVolunteerAdditionalContractMainDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerAdditionalContractsMainDisplayIndexResponse Execute(VolunteerAdditionalContractsMainDisplayIndexRequest request)
        {
            var additionalContracts = dataGateway.GetListAdditionalContracts();
            var contract = dataGateway.GetContract(request.ContractId);
            additionalContracts = additionalContracts.Where(z => z.ContractID.ToString() == request.ContractId).ToList();
            
            return new VolunteerAdditionalContractsMainDisplayIndexResponse(additionalContracts, contract.Fullname,contract.OwnerID, request.ContractId);
        }
    }

    public class VolunteerAdditionalContractsMainDisplayIndexRequest
    {
        public string ContractId { get; set; }

        public int NrOfDocumentsPerPage { get; set; }

        public VolunteerAdditionalContractsMainDisplayIndexRequest(string contractId, int nrOfDocs)
        {
            ContractId = contractId;
            NrOfDocumentsPerPage = nrOfDocs;
        }
    }

    public class VolunteerAdditionalContractsMainDisplayIndexResponse
    {
        public List<AdditionalContractVolunteer> Contracts { get; set; }
        public string NameOfVolunteer { get; set; }
        public string Query { get; set; }
        public string VolunteerId { get; set; }
        public string ContractId { get; set; }

        public VolunteerAdditionalContractsMainDisplayIndexResponse(List<AdditionalContractVolunteer> contracts, string nameOfVolunteer, string volunteerId,string contractId)
        {
            Contracts = contracts;
            NameOfVolunteer = nameOfVolunteer;
            VolunteerId = volunteerId;
            ContractId = contractId;
        }
    }
}