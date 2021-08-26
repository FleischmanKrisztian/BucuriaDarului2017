using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractIndexDisplayContext
    {
        private readonly IBeneficiaryContractMainDisplayGateway dataGateway;

        public BeneficiaryContractIndexDisplayContext(IBeneficiaryContractMainDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryContractsMainDisplayIndexResponse Execute(BeneficiaryContractsMainDisplayIndexRequest request)
        {
            var contracts = dataGateway.GetListBeneficiaryContracts();
            var beneficiary = dataGateway.GetBeneficiary(request.BeneficiaryId);
            contracts = contracts.Where(z => z.OwnerID.ToString() == request.BeneficiaryId).ToList();

            return new BeneficiaryContractsMainDisplayIndexResponse(contracts, beneficiary.Fullname, beneficiary.Id);
        }
    }

    public class BeneficiaryContractsMainDisplayIndexRequest
    {
        public string BeneficiaryId { get; set; }

        public int NrOfDocumentsPerPage { get; set; }

        public BeneficiaryContractsMainDisplayIndexRequest(string beneficiaryId, int nrOfDocs)
        {
            BeneficiaryId = beneficiaryId;
            NrOfDocumentsPerPage = nrOfDocs;
        }
    }

    public class BeneficiaryContractsMainDisplayIndexResponse
    {
        public List<BeneficiaryContract> Contracts { get; set; }
        public string NameOfBeneficiary { get; set; }
        public string BeneficiaryId { get; set; }
        public string Query { get; set; }

        public BeneficiaryContractsMainDisplayIndexResponse(List<BeneficiaryContract> contracts, string nameOfBeneficiary, string beneficiaryId)
        {
            Contracts = contracts;
            NameOfBeneficiary = nameOfBeneficiary;
            BeneficiaryId = beneficiaryId;
        }
    }
}