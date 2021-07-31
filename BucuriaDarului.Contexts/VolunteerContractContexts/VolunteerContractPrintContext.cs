using BucuriaDarului.Core.Gateways.VolunteerContractGateways;
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

        public Response Execute(Stream dataToImport, string idContract, string fileName)
        {
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
                response.FileName = "Contract" + "-" + contract.Fullname.ToString() + ".docx";
           
           var fileContent = ReadFile(dataToImport);

            if (response.IsValid)
                response.FileContent = GetFinalFile(fileContent);
            else
                response.Message = "An eror has occured!Please check if the template is valid and approriate for this type of contract.";


            return response;
        }

        public string GetFinalFile(List<string> originalFile)
        {
            string file = string.Empty;

            foreach (var line in originalFile)
            {
                file += line;
            }

            return file;
        }

        public List<string> ReadFile(Stream data)
        {
            var result = new List<string>();
            var reader = new StreamReader(data, Encoding.UTF8);
            while (reader.Peek() >= 0)
            {
                result.Add(reader.ReadLine());
            }

            return result;
        }
    }

    public class Response
    {
        public string FileContent { get; set; }
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