using BucuriaDarului.Contexts.VolunteerContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractEditContext
    {
        private readonly IVolunteerContractEditGateway dataGateway;
        public VolunteerContractEditResponse response = new VolunteerContractEditResponse("", true);

        public VolunteerContractEditContext(IVolunteerContractEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerContractEditResponse Execute(VolunteerEditRequest request)
        {
            var listOfContracts = dataGateway.GetListOfVolunteersContracts(request.Id);
            if (listOfContracts.Count > 0)
            {
                CreateAuxiliaryFiles(listOfContracts);
                UpdateContracts(listOfContracts, request);
            }
            return response;
        }

        private void CreateAuxiliaryFiles(List<VolunteerContract> listOfContracts)
        {
            foreach (var contract in listOfContracts)
            {
                var beforeEditingVolunteerContractString = JsonConvert.SerializeObject(contract);
                dataGateway.AddVolunteerContractToModifiedList(beforeEditingVolunteerContractString);
            }
        }

        private void UpdateContracts(List<VolunteerContract> listOfContracts, VolunteerEditRequest request)
        {
            try
            {
                foreach (var contract in listOfContracts)
                {
                    var beforeEditingVolunteerContractString = JsonConvert.SerializeObject(contract);
                    contract.Address = request.Address;
                    contract.Birthdate = request.Birthdate;
                    contract.CI = request.CI;
                    contract.CNP = request.CNP;
                    contract.Fullname = request.Fullname;
                    contract.HourCount = request.HourCount;
                    contract.PhoneNumber = request.ContactInformation.PhoneNumber;

                    dataGateway.Update(contract);
                }
            }
            catch
            {
                response.Message = "There was an Error Updating the Volunteers Contracts. Please Try again!";
                response.IsValid = false;
            }
        }
    }

    public class VolunteerContractEditResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public VolunteerContractEditResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}