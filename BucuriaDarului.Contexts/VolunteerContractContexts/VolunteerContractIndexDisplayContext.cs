using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerContractContext
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
            List<VolunteerContract> contracts = dataGateway.GetListVolunteerContracts();
            Volunteer volunteer = dataGateway.GetVolunteer(request.VolunteerId);
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

        public string VolunteerId { get; set; }

        public VolunteerContractsMainDisplayIndexResponse(List<VolunteerContract> contracts, string nameOfVolunteer, string volunteerId)
        {
            Contracts = contracts;
            NameOfVolunteer = nameOfVolunteer;
            VolunteerId = volunteerId;
        }
    }
}