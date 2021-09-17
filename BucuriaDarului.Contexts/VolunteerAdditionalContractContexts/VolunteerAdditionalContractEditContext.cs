using BucuriaDarului.Contexts.VolunteerContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BucuriaDarului.Contexts.VolunteerAdditionalContractContexts
{
    public class VolunteerAdditionalContractEditContext
    {
        private readonly IVolunteerAdditionalContractEditGateway dataGateway;
        public VolunteerAdditionalContractEditResponse response = new VolunteerAdditionalContractEditResponse("", true);

        public VolunteerAdditionalContractEditContext(IVolunteerAdditionalContractEditGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerAdditionalContractEditResponse Execute(VolunteerEditRequest request)
        {
            var listOfContracts = dataGateway.GetListOfAdditionalContracts(request.Id);
            if (listOfContracts.Count > 0)
            {
                CreateAuxiliaryFiles(listOfContracts);
                UpdateContracts(listOfContracts, request);
            }
            return response;
        }

        private void CreateAuxiliaryFiles(List<AdditionalContractVolunteer> listOfContracts)
        {
            foreach (var contract in listOfContracts)
            {
                var beforeEditingVolunteerAdditionalContractString = JsonConvert.SerializeObject(contract);
                dataGateway.AddVAdditionalContractToModifiedList(beforeEditingVolunteerAdditionalContractString);
            }
        }

        private void UpdateContracts(List<AdditionalContractVolunteer> listOfContracts, VolunteerEditRequest request)
        {
            try
            {
                foreach (var contract in listOfContracts)
                {
                    var beforeEditingVolunteerAdditionalContractString = JsonConvert.SerializeObject(contract);
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
                response.Message = "There was an Error Updating the Volunteers Additional Contracts. Please Try again!";
                response.IsValid = false;
            }
        }
    }

    public class VolunteerAdditionalContractEditResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }

        public VolunteerAdditionalContractEditResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}