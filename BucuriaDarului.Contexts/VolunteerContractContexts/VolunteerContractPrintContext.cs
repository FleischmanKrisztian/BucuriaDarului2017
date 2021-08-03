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
            
            var doc = Novacode.DocX.Load(data);
            var response = new Response();
            var contract = dataGateway.GetVolunteerContract(idContract);

            if (fileName != null && fileName != "")
            {
                if (fileName.Contains(".docx"))
                    response.FileName = fileName;
                else
                    response.FileName = fileName + ".docx";
            }
            else
            {
                if (contract.Fullname.Contains(" "))
                    response.FileName = "Contract" + "-" + contract.Fullname.Replace(' ', '_') + ".docx";
            }

            if (contract.Address != null)
                doc.ReplaceText("<Address>", contract.Address);
            doc.ReplaceText("<nrreg>", contract.NumberOfRegistration);
            doc.ReplaceText("<todaydate>", contract.RegistrationDate.ToShortDateString());
            doc.ReplaceText("<Fullname>", contract.Fullname);
            if (contract.CNP != null)
                doc.ReplaceText("<CNP>", contract.CNP);
            if (contract.CI.Info != null)
                doc.ReplaceText("<CiInfo>", contract.CI.Info);
            if (contract.PhoneNumber != null)
                doc.ReplaceText("<tel>", contract.PhoneNumber);
            doc.ReplaceText("<startdate>", contract.RegistrationDate.ToShortDateString());
            doc.ReplaceText("<finishdate>", contract.ExpirationDate.ToShortDateString());
            doc.ReplaceText("<hourcount>", contract.HourCount.ToString());

            doc.SaveAs(response.FileName);

           response.DownloadPath= Path.GetFullPath(response.FileName);


            return response;
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