using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
using DocumentFormat.OpenXml.Packaging;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BucuriaDarului.Contexts.VolunteerContractContexts
{
    public class VolunteerContractPrintContext
    {
        private readonly IVolunteerContractPrintGateway dataGateway;

        public VolunteerContractPrintContext(IVolunteerContractPrintGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public Response Execute(Stream data, string idContract, string fileName)
        {
            
            
            var response = new Response();
            if (FileIsNotEmpty(data))
            {
                response.Message = "File Cannot be Empty!";
                response.IsValid = false;
            }
            if (response.IsValid)
            {
                var document = Novacode.DocX.Load(data);

                var contract = dataGateway.GetVolunteerContract(idContract);

                response.FileName = GetFileName(fileName, contract.Fullname);

                document = FillInDocument(document, contract);
                document.SaveAs(response.FileName);

                response.DownloadPath = Path.GetFullPath(response.FileName);

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
        public string GetFileName(string fileName,string Fullname)
        {
            string resultName = string.Empty;
            if (fileName != null && fileName != "")
            {
                if (fileName.Contains(".docx"))
                    resultName = fileName;
                else
                    resultName = fileName + ".docx";
            }
            else
            {
                if (Fullname.Contains(" "))
                    resultName = "Contract" + "-" + Fullname.Replace(' ', '_') + ".docx";
            }

            return resultName;
        }
 
        
    }

    public class Response
    {
        public string  DownloadPath { get; set; }
        public bool IsValid { get; set; }
        public string FileName { get; set; }

        public string Message { get; set; }

        public Response()
        {
            IsValid = true;
            Message = "Contract exported succesfully!";
        }
    }
}