using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
   public  class BeneficiaryContractPrintContext
    {
        private readonly IBeneficiaryContractPrintGateway dataGateway;

        public BeneficiaryContractPrintContext(IBeneficiaryContractPrintGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public Response Execute(Stream data, string idContract, string fileName,string optionValue)
        {

            var document = Novacode.DocX.Load(data);
            var response = new Response();
            var contract = dataGateway.GetBeneficiaryContract(idContract);

            response.FileName = GetFileName(fileName, contract.Fullname);

            document = FillInDocument(document, contract, optionValue);
            document.SaveAs(response.FileName);

            response.DownloadPath = Path.GetFullPath(response.FileName);


            return response;
        }

        public Novacode.DocX FillInDocument(Novacode.DocX document, BeneficiaryContract contract,string optionValue)
        {
            document.ReplaceText("<nrreg>", contract.NumberOfRegistration);
            document.ReplaceText("<todaydate>", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<Fullname>", contract.Fullname);
            if (contract.CNP != null)
                document.ReplaceText("<CNP>", contract.CNP);
            if (contract.CIinfo != null)
                document.ReplaceText("<CiInfo>", contract.CIinfo);
            if (contract.Address != null)
                document.ReplaceText("<Address>", contract.Address);
            if (contract.PhoneNumber != null)
                document.ReplaceText("<tel>", contract.PhoneNumber);
            if (contract.IdApplication != null)
                document.ReplaceText("<IdAplication>", contract.IdApplication);
            if (contract.IdInvestigation != null)
                document.ReplaceText("<IdInvestigation>", contract.IdInvestigation);
            document.ReplaceText("<option>", optionValue);
            document.ReplaceText("<NumberOfPortions>", contract.NumberOfPortions.ToString());
            document.ReplaceText("<RegistrationDate> ", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<ExpirationDate>", contract.ExpirationDate.ToShortDateString());

            return document;
        }
        public string GetFileName(string fileName, string Fullname)
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
        public string DownloadPath { get; set; }
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