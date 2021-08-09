using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryContractGateways;
using System.IO;

namespace BucuriaDarului.Contexts.BeneficiaryContractContexts
{
    public class BeneficiaryContractPrintContext
    {
        private readonly IBeneficiaryContractPrintGateway dataGateway;

        public BeneficiaryContractPrintContext(IBeneficiaryContractPrintGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryContractPrintResponse Execute(Stream data, string idContract, string optionValue, string otherOptionValue)
        {
            var response = new BeneficiaryContractPrintResponse();
            if (FileIsNotEmpty(data))
            {
                response.Message = "File Cannot be Empty!";
                response.IsValid = false;
            }
            if (response.IsValid)
            {
                var document = Novacode.DocX.Load(data);

                var contract = dataGateway.GetBeneficiaryContract(idContract);

                response.FileName = GetFileName(contract.Fullname);
                var option = GetOptionValue(optionValue, otherOptionValue);
                document = FillInDocument(document, contract, option);
                document.SaveAs(response.FileName);

                response.DownloadPath = Path.GetFullPath(response.FileName);
            }

            return response;
        }

        private bool FileIsNotEmpty(Stream dataToImport)
        {
            return dataToImport.Length <= 0;
        }

        public string GetOptionValue(string optionValue, string otherOptionValue)
        {
            string option = string.Empty;
            if (optionValue == "first")
                option = "Zilnic, in zilele lucratoare, a unei mese calde / persoana / zi lucratoare, respectiv pranzul, acordata la sediul cantinei.";
            if (optionValue == "second")
                option = "Zilnic, in zilele lucratoare, a unei mese calde/persoana/zi lucratoare, repsectiv pranzul, acordata la domiciliul beneficiarului.";
            if (optionValue == "third")
                option = "Saptamanal, a unui pachet cu alimente necesare pentru pregatirea unei mese calde/persoana/zi lucratoare, respectiv pranzul, acordat la domiciliul beneficiarului.";
            if (optionValue == "fourth")
                option = "Saptamanal, a unui pachet cu alimente necesare pentru pregatirea unei mese calde/persoana/zi lucratoare, respectiv pranzul, acordat la sediul cantinei.";
            if (otherOptionValue != null && otherOptionValue != " ")
                option = otherOptionValue;

            return option;
        }

        public Novacode.DocX FillInDocument(Novacode.DocX document, BeneficiaryContract contract, string optionValue)
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
            if (optionValue != null)
            { 
                document.ReplaceText("<option>", optionValue); 
            }
            document.ReplaceText("<NumberOfPortions>", contract.NumberOfPortions.ToString());
            document.ReplaceText("<RegistrationDate> ", contract.RegistrationDate.ToShortDateString());
            document.ReplaceText("<ExpirationDate>", contract.ExpirationDate.ToShortDateString());

            return document;
        }

        public string GetFileName(string Fullname)
        {
            string resultName = string.Empty;
            resultName = "Contract" + "-" + Fullname.Replace(' ', '_') + ".docx";

            return resultName;
        }
    }

    public class BeneficiaryContractPrintResponse
    {
        public string DownloadPath { get; set; }
        public bool IsValid { get; set; }
        public string FileName { get; set; }

        public string Message { get; set; }

        public BeneficiaryContractPrintResponse()
        {
            IsValid = true;
            Message = "Contract exported successfully!";
        }
    }
}