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

        public void Execute(Stream dataToImport, string idContract)
        {
            var contract = dataGateway.GetVolunteerContract(idContract);

            var fileContent = ReadFile(dataToImport);
        }

        public List<string> ReadFile(Stream data)
        {

            var result = new List<string>();
            var reader = new StreamReader(data, Encoding.UTF8);
            while (reader.Peek() >= 0)
            {
               result.Add( reader.ReadLine());
            }

                return result;
        }
    }
}