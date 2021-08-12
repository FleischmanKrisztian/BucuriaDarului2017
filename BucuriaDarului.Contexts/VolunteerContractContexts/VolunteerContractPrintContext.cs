using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using Microsoft.Extensions.Localization;
using Novacode;
using System.IO;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractPrintContext
    {
        private readonly IVolunteerContractPrintGateway dataGateway;
        private readonly IStringLocalizer localizer;

        public VolunteerContractPrintContext(IVolunteerContractPrintGateway dataGateway, IStringLocalizer localizer)
        {
            this.dataGateway = dataGateway;
            this.localizer = localizer;
        }

        public VolunteerContractPrintResponse Execute(Stream data, string idContract, string fileName)
        {
            var response = new VolunteerContractPrintResponse();
            if (FileIsNotEmpty(data))
            {
                response.Message =@localizer["File Cannot be Empty!"];
                response.IsValid = false;
            }
            if (response.IsValid)
            {
                var document = Novacode.DocX.Load(data);

                var contract = dataGateway.GetVolunteerContract(idContract);

                response.FileName = GetFileName(fileName, contract.Fullname);

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

        public DocX FillInDocument(DocX document, VolunteerContract contract)
        {
            if (contract.Address != null)
                document.ReplaceText("<Address>", contract.Address);
            document.ReplaceText("<nrreg>", contract.NumberOfRegistration);
            document.ReplaceText("<todaydate>", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<Fullname>", contract.Fullname);
            if (contract.CNP != null)
                document.ReplaceText("<CNP>", contract.CNP);
            if (contract.CI.Info != null)
                document.ReplaceText("<CiInfo>", contract.CI.Info);
            if (contract.PhoneNumber != null)
                document.ReplaceText("<tel>", contract.PhoneNumber);
            document.ReplaceText("<startdate>", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<finishdate>", contract.ExpirationDate.ToShortDateString());
            document.ReplaceText("<hourcount>", contract.HourCount.ToString());
            return document;
        }

        public string GetFileName(string fileName, string Fullname)
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
               
               resultName = "Contract" + "-" + Fullname.Replace(' ', '_') + ".docx";
            }

            return resultName;
        }
    }

    public class VolunteerContractPrintResponse
    {
        public string DownloadPath { get; set; }
        public bool IsValid { get; set; }
        public string FileName { get; set; }
        public MemoryStream Stream { get; set; }
        public string Message { get; set; }

        public VolunteerContractPrintResponse()
        {
            IsValid = true;
        }
    }
}