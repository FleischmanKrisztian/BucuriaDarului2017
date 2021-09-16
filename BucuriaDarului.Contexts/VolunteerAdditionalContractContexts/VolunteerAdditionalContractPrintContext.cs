using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using Novacode;
using System.IO;

namespace BucuriaDarului.Contexts.VolunteerAdditionalContractContexts
{
    public class VolunteerAdditionalContractPrintContext
    {
        private readonly IVolunteerAdditionalContractPrintGateway dataGateway;

        public VolunteerAdditionalContractPrintContext(IVolunteerAdditionalContractPrintGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerAdditionalContractPrintResponse Execute(Stream data, string idAdditionalContract, string fileName)
        {
            var response = new VolunteerAdditionalContractPrintResponse();
            if (FileIsNotEmpty(data))
            {
                response.Message = "File Cannot be Empty!";
                response.IsValid = false;
            }
            if (response.IsValid)
            {
                var document = Novacode.DocX.Load(data);

                var contract = dataGateway.GetAdditionalContract(idAdditionalContract);

                response.FileName = GetFileName(fileName, contract.Fullname, contract.ContractNumberOfRegistration);

                document = FillInDocument(document, contract);
                MemoryStream stream = new MemoryStream();
                document.SaveAs(stream);

                response.Stream = stream;
            }
            return response;
        }

        private bool FileIsNotEmpty(Stream dataToImport)
        {
            return dataToImport.Length <= 0;
        }

        public DocX FillInDocument(DocX document, AdditionalContractVolunteer contract)
        {
            if (contract.Address != null)
                document.ReplaceText("<Address>", contract.Address);
            document.ReplaceText("<nrAdditional>", contract.AdditionalContractNumberOfRegistration);
            document.ReplaceText("<nrreg>", contract.ContractNumberOfRegistration);
            document.ReplaceText("<nrreg>", contract.ContractNumberOfRegistration);
            document.ReplaceText("<todaydate>", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<Fullname>", contract.Fullname);
            if (contract.CNP != null)
                document.ReplaceText("<CNP>", contract.CNP);
            if (contract.CI.Info != null)
                document.ReplaceText("<CiInfo>", contract.CI.Info);
            if (contract.PhoneNumber != null)
                document.ReplaceText("<tel>", contract.PhoneNumber);
            document.ReplaceText("<creationdate>", contract.CreationDate.ToShortDateString());
            document.ReplaceText("<startdate>", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<finishdate>", contract.ExpirationDate.ToShortDateString());
            document.ReplaceText("<hourcount>", contract.HourCount.ToString());
            return document;
        }

        public string GetFileName(string fileName, string Fullname, string contractNumberOfRegistration)
        {
            var resultName = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.Contains(".docx"))
                    resultName = fileName;
                else
                    resultName = fileName + ".docx";
            }
            else
            {
                resultName = "ContractAditional" + "-" + Fullname.Replace(' ', '_') + "-" + contractNumberOfRegistration + ".docx";
            }

            return resultName;
        }
    }

    public class VolunteerAdditionalContractPrintResponse
    {
        public string DownloadPath { get; set; }
        public bool IsValid { get; set; }
        public string FileName { get; set; }
        public MemoryStream Stream { get; set; }
        public string Message { get; set; }

        public VolunteerAdditionalContractPrintResponse()
        {
            IsValid = true;
        }
    }
}