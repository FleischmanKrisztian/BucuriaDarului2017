using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
    public class BeneficiaryDownloadContext
    {
        private readonly IBeneficiaryDownloadGateway dataGateway;

        public BeneficiaryDownloadContext(IBeneficiaryDownloadGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public string Execute(string dataString, string header)
        {
            var finalHeader = SplitHeader(header);
            string jsonString = string.Empty;
            var properties = GetProperties(dataString);
            var ids = GetIds(dataString);

            if (properties.Contains("0"))
                jsonString = GetBeneficiariesCsv(ids, finalHeader);
            else
                jsonString = GetBeneficiaries(properties, ids, finalHeader);

            return jsonString;
        }

        public string[] SplitHeader(string header)
        {
            var splitHeader = header.Split(",");

            return splitHeader;
        }

        public string GetProperties(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var properties = auxiliaryStrings[1];
            return properties;
        }

        public string[] GetIds(string ids_)
        {
            var auxiliaryStrings = ids_.Split("(((");
            var ids = auxiliaryStrings[0].Split(",");
            return ids;
        }

        public string[] BeneficiaryToArray(Beneficiary beneficiary)
        {
            string[] result = new string[40];
            result[0] = beneficiary.Id;
            result[1] = beneficiary.Fullname;
            result[2] = beneficiary.Active.ToString();
            result[3] = beneficiary.WeeklyPackage.ToString();
            result[4] = beneficiary.Canteen.ToString();
            result[5] = beneficiary.HomeDelivery.ToString();
            result[6] = beneficiary.HomeDeliveryDriver;
            result[7] = beneficiary.HasGDPRAgreement.ToString();
            result[8] = beneficiary.Address;
            result[9] = beneficiary.CNP;
            result[10] = beneficiary.CI.HasId.ToString();
            result[11] = beneficiary.CI.Info;
            result[12] = DateTime.Parse(beneficiary.CI.ExpirationDate.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToShortDateString().ToString(CultureInfo.InvariantCulture);
            result[13] = beneficiary.Marca.MarcaName;
            result[14] = beneficiary.Marca.IdApplication;
            result[15] = beneficiary.Marca.IdInvestigation;
            result[16] = beneficiary.Marca.IdContract;
            result[17] = beneficiary.NumberOfPortions.ToString();
            result[18] = DateTime.Parse(beneficiary.LastTimeActive.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToShortDateString().ToString(CultureInfo.InvariantCulture);
            result[19] = beneficiary.Comments;
            result[20] = DateTime.Parse(beneficiary.PersonalInfo.Birthdate.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).ToShortDateString().ToString(CultureInfo.InvariantCulture);
            result[21] = beneficiary.PersonalInfo.PhoneNumber;
            result[22] = beneficiary.PersonalInfo.BirthPlace;
            result[23] = beneficiary.PersonalInfo.Studies;
            result[24] = beneficiary.PersonalInfo.Profession;
            result[25] = beneficiary.PersonalInfo.Occupation;
            result[26] = beneficiary.PersonalInfo.SeniorityInWorkField;
            result[27] = beneficiary.PersonalInfo.HealthState;
            result[28] = beneficiary.PersonalInfo.Disability;
            result[29] = beneficiary.PersonalInfo.ChronicCondition;
            result[30] = beneficiary.PersonalInfo.Addictions;
            result[31] = beneficiary.PersonalInfo.HealthInsurance.ToString();
            result[32] = beneficiary.PersonalInfo.HealthCard.ToString();
            result[33] = beneficiary.PersonalInfo.Married;
            result[34] = beneficiary.PersonalInfo.SpouseName;
            result[35] = beneficiary.PersonalInfo.HasHome.ToString();
            result[36] = beneficiary.PersonalInfo.HousingType;
            result[37] = beneficiary.PersonalInfo.Income;
            result[38] = beneficiary.PersonalInfo.Expenses;
            result[39] = beneficiary.PersonalInfo.Gender.ToString();

            return result;
        }

        public string GetBeneficiariesCsv(string[] ids, string[] finalHeader)
        {
            var listOfBeneficiaries = dataGateway.GetListOfBeneficiaries();
            var finalListOfBeneficiaries = new List<Beneficiary>();
            foreach (var beneficiary in listOfBeneficiaries)
            {
                if (ids.Contains(beneficiary.Id))
                {
                    finalListOfBeneficiaries.Add(beneficiary);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in finalHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }
            sb.Append("\r\n");

            foreach (var item in finalListOfBeneficiaries)
            {
                var beneficiaryArray = BeneficiaryToArray(item);
                foreach (var data in beneficiaryArray)
                {
                    if (data == "-")
                        sb.Append("\"" + "" + "\"" + ",");
                    else
                        sb.Append("\"" + data + "\"" + ",");
                }

                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public string[] GetUsedHeader(string properties, string[] header)
        {
            List<string> headerList = new List<string>();
            if (properties.Contains("1"))
            {
                headerList.Add(header[1]);
            }
            if (properties.Contains("2"))
            {
                headerList.Add(header[2]);
            }
            if (properties.Contains("3"))
            {
                headerList.Add(header[4]);
            }
            if (properties.Contains("Z"))
            {
                headerList.Add(header[3]);
            }
            if (properties.Contains("4"))
            {
                headerList.Add(header[5]);
            }
            if (properties.Contains("5"))
            {
                headerList.Add(header[6]);
            }
            if (properties.Contains("6"))
            {
                headerList.Add(header[7]);
            }
            if (properties.Contains("7"))
            {
                headerList.Add(header[9]);
            }
            if (properties.Contains("8"))
            {
                headerList.Add(header[9]);
            }
            if (properties.Contains("9"))
            {
                headerList.Add(header[11]);
            }
            if (properties.Contains("A"))
            {
                headerList.Add(header[13]);
            }
            if (properties.Contains("B"))
            {
                headerList.Add(header[15]);
            }
            if (properties.Contains("C"))
            {
                headerList.Add(header[14]);
            }
            if (properties.Contains("D"))
            {
                headerList.Add(header[17]);
            }
            if (properties.Contains("E"))
            {
                headerList.Add(header[18]);
            }
            if (properties.Contains("Y"))
            {
                headerList.Add(header[20]);
            }
            if (properties.Contains("F"))
            {
                headerList.Add(header[21]);
            }
            if (properties.Contains("G"))
            {
                headerList.Add(header[22]);
            }
            if (properties.Contains("H"))
            {
                headerList.Add(header[23]);
            }
            if (properties.Contains("I"))
            {
                headerList.Add(header[24]);
            }
            if (properties.Contains("J"))
            {
                headerList.Add(header[25]);
            }
            if (properties.Contains("K"))
            {
                headerList.Add(header[26]);
            }
            if (properties.Contains("L"))
            {
                headerList.Add(header[27]);
            }
            if (properties.Contains("M"))
            {
                headerList.Add(header[28]);
            }
            if (properties.Contains("N"))
            {
                headerList.Add(header[29]);
            }
            if (properties.Contains("O"))
            {
                headerList.Add(header[30]);
            }
            if (properties.Contains("Z"))
            {
                headerList.Add(header[31]);
            }
            if (properties.Contains("P"))
            {
                headerList.Add(header[32]);
            }
            if (properties.Contains("Q"))
            {
                headerList.Add(header[33]);
            }
            if (properties.Contains("R"))
            {
                headerList.Add(header[34]);
            }
            if (properties.Contains("S"))
            {
                headerList.Add(header[35]);
            }
            if (properties.Contains("T"))
            {
                headerList.Add(header[36]);
            }
            if (properties.Contains("U"))
            {
                headerList.Add(header[37]);
            }
            if (properties.Contains("V"))
            {
                headerList.Add(header[38]);
            }
            if (properties.Contains("W"))
            {
                headerList.Add(header[39]);
            }
            

            var returnedHeader = headerList.ToArray();
            return returnedHeader;
        }

        public string GetBeneficiaries(string properties, string[] ids, string[] header)
        {
            var usedHeader = GetUsedHeader(properties, header);
            var listOfBeneficiary = dataGateway.GetListOfBeneficiaries();
            var idListForPrinting = new List<Beneficiary>();
            foreach (var beneficiary in listOfBeneficiary)
            {
                if (ids.Contains(beneficiary.Id))
                {
                    idListForPrinting.Add(beneficiary);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var data in usedHeader)
            {
                sb.Append("\"" + data + "\"" + ",");
            }

            sb.Append("\r\n");
            foreach (var item in idListForPrinting)
            {
                if (properties.Contains("1"))
                {
                    sb.Append("\"" + item.Fullname + "\"" + ",");
                }
                if (properties.Contains("2"))
                {
                    sb.Append("\"" + item.Active + "\"" + ",");
                }

                if (properties.Contains("3"))
                {
                    sb.Append("\"" + item.Canteen + "\"" + ",");
                }
                if (properties.Contains("Z"))
                {
                    sb.Append("\"" + item.WeeklyPackage + "\"" + ",");
                }
                if (properties.Contains("4"))
                {
                    sb.Append("\"" + item.HomeDelivery + "\"" + ",");
                }
                if (properties.Contains("5"))
                {
                    sb.Append("\"" + item.HomeDeliveryDriver + "\"" + ",");
                }
                if (properties.Contains("6"))
                {
                    sb.Append("\"" + item.HasGDPRAgreement + "\"" + ",");
                }
                if (properties.Contains("7"))
                {
                    sb.Append("\"" + item.Address + "\"" + ",");
                }
                if (properties.Contains("8"))
                {
                    sb.Append("\"" + item.CNP + "\"" + ",");
                }
                if (properties.Contains("9"))
                {
                    sb.Append("\"" + item.CI.Info + "\"" + ",");
                }
                if (properties.Contains("A"))
                {
                    sb.Append("\"" + item.Marca.MarcaName + "\"" + ",");
                }
                if (properties.Contains("B"))
                {
                    sb.Append("\"" + item.Marca.IdInvestigation + "\"" + ",");
                }
                if (properties.Contains("C"))
                {
                    sb.Append("\"" + item.Marca.IdApplication + "\"" + ",");
                }
                if (properties.Contains("D"))
                {
                    sb.Append("\"" + item.NumberOfPortions + "\"" + ",");
                }
                if (properties.Contains("E"))
                {
                    sb.Append("\"" + DateTime.Parse(item.LastTimeActive.ToString(), System.Globalization.CultureInfo.InvariantCulture).ToString() + "\"" + ",");
                }
                if (properties.Contains("Y"))
                {
                    sb.Append("\"" + DateTime.Parse(item.PersonalInfo.Birthdate.ToString(), System.Globalization.CultureInfo.InvariantCulture).ToString() + "\"" + ",");
                }
                if (properties.Contains("F"))
                {
                    sb.Append("\"" + item.PersonalInfo.PhoneNumber + "\"" + ",");
                }
                if (properties.Contains("G"))
                {
                    sb.Append("\"" + item.PersonalInfo.BirthPlace + "\"" + ",");
                }
                if (properties.Contains("H"))
                {
                    sb.Append("\"" + item.PersonalInfo.Studies + "\"" + ",");
                }
                if (properties.Contains("I"))
                {
                    sb.Append("\"" + item.PersonalInfo.Profession + "\"" + ",");
                }
                if (properties.Contains("J"))
                {
                    sb.Append("\"" + item.PersonalInfo.Occupation + "\"" + ",");
                }
                if (properties.Contains("K"))
                {
                    sb.Append("\"" + item.PersonalInfo.SeniorityInWorkField + "\"" + ",");
                }
                if (properties.Contains("L"))
                {
                    sb.Append("\"" + item.PersonalInfo.HealthState + "\"" + ",");
                }
                if (properties.Contains("M"))
                {
                    sb.Append("\"" + item.PersonalInfo.Disability + "\"" + ",");
                }
                if (properties.Contains("N"))
                {
                    sb.Append("\"" + item.PersonalInfo.ChronicCondition + "\"" + ",");
                }
                if (properties.Contains("O"))
                {
                    sb.Append("\"" + item.PersonalInfo.Addictions + "\"" + ",");
                }
                if (properties.Contains("Z"))
                {
                    sb.Append("\"" + item.PersonalInfo.HealthInsurance + "\"" + ",");
                }
                if (properties.Contains("P"))
                {
                    sb.Append("\"" + item.PersonalInfo.HealthCard + "\"" + ",");
                }
                if (properties.Contains("Q"))
                {
                    sb.Append("\"" + item.PersonalInfo.Married + "\"" + ",");
                }
                if (properties.Contains("R"))
                {
                    sb.Append("\"" + item.PersonalInfo.SpouseName + "\"" + ",");
                }
                if (properties.Contains("S"))
                {
                    sb.Append("\"" + item.PersonalInfo.HasHome + "\"" + ",");
                }
                if (properties.Contains("T"))
                {
                    sb.Append("\"" + item.PersonalInfo.HousingType + "\"" + ",");
                }
                if (properties.Contains("U"))
                {
                    sb.Append("\"" + item.PersonalInfo.Income + "\"" + ",");
                }
                if (properties.Contains("V"))
                {
                    sb.Append("\"" + item.PersonalInfo.Expenses + "\"" + ",");
                }
                if (properties.Contains("W"))
                {
                    sb.Append("\"" + item.PersonalInfo.Gender + "\"" + ",");
                }

                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}