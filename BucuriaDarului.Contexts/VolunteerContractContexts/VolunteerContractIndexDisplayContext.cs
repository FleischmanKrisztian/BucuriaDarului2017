using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractIndexDisplayContext
    {
        private readonly IVolunteerContractMainDisplayGateway dataGateway;

        public VolunteerContractIndexDisplayContext(IVolunteerContractMainDisplayGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerContractsMainDisplayIndexResponse Execute(VolunteerContractsMainDisplayIndexRequest request)
        {
            var contracts = dataGateway.GetListVolunteerContracts();
            var volunteer = dataGateway.GetVolunteer(request.VolunteerId);
            contracts = contracts.Where(z => z.OwnerID.ToString() == request.VolunteerId).ToList();

            return new VolunteerContractsMainDisplayIndexResponse(contracts, volunteer.Fullname, volunteer.Id);
        }
    }

    public class VolunteerContractsMainDisplayIndexRequest
    {
        public string VolunteerId { get; set; }

        public int NrOfDocumentsPerPage { get; set; }

        public VolunteerContractsMainDisplayIndexRequest(string volunteerId, int nrOfDocs)
        {
            VolunteerId = volunteerId;
            NrOfDocumentsPerPage = nrOfDocs;
        }
    }

    public class VolunteerContractsMainDisplayIndexResponse
    {
        public List<VolunteerContract> Contracts { get; set; }
        public string NameOfVolunteer { get; set; }
        public string Query { get; set; }
        public string VolunteerId { get; set; }

        public VolunteerContractsMainDisplayIndexResponse(List<VolunteerContract> contracts, string nameOfVolunteer, string volunteerId)
        {
            Contracts = contracts;
            NameOfVolunteer = nameOfVolunteer;
            VolunteerId = volunteerId;
        }
    }
}