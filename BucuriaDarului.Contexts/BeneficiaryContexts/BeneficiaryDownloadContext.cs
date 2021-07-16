using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using System;
using System.Collections.Generic;
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
                jsonString = GetBneficiariesCsv(ids, finalHeader);
            //else
            //jsonString = GetBeneficiary(properties, ids, finalHeader);

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

        public string[] BeneficiaryToArray(Beneficiary b)
        {
            string[] result = new string[39];
            result[0] = b.Id;
            result[1] = b.Fullname;
            result[2] = b.Active.ToString();
            result[3] = b.WeeklyPackage.ToString();
            result[4] = b.Canteen.ToString();
            result[5] = b.HomeDelivery.ToString();
            result[6] = b.HomeDeliveryDriver;
            result[7] = b.HasGDPRAgreement.ToString();
            result[8] = b.Address; result[9] = b.CNP;
            result[10] = b.CI.HasId.ToString();
            result[11] = b.CI.Info;
            result[12] = b.CI.ExpirationDate.ToLongDateString();
            result[13] = b.Marca.MarcaName;
            result[14] = b.Marca.IdApplication;
            result[15] = b.Marca.IdInvestigation;
            result[16] = b.Marca.IdContract;
            result[17] = b.NumberOfPortions.ToString();
            result[18] = b.LastTimeActive.ToLongDateString();
            result[19] = b.Comments;
            result[20] = b.PersonalInfo.BirthPlace;
            result[21] = b.PersonalInfo.PhoneNumber;
            result[22] = b.PersonalInfo.Studies;
            result[24] = b.PersonalInfo.Profession;
            result[24] = b.PersonalInfo.Occupation;
            result[25] = b.PersonalInfo.SeniorityInWorkField;
            result[26] = b.PersonalInfo.HealthState;
            result[27] = b.PersonalInfo.Disability;
            result[28] = b.PersonalInfo.ChronicCondition;
            result[29] = b.PersonalInfo.Addictions;
            result[30] = b.PersonalInfo.HealthInsurance.ToString();
            result[31] = b.PersonalInfo.HealthCard.ToString();
            result[32] = b.PersonalInfo.Married;
            result[33] = b.PersonalInfo.SpouseName;
            result[34] = b.PersonalInfo.HasHome.ToString();
            result[35] = b.PersonalInfo.HousingType;
            result[36] = b.PersonalInfo.Income;
            result[37] = b.PersonalInfo.Expenses;
            result[38] = b.PersonalInfo.Gender.ToString();

            return result;
        }

        public string GetBneficiariesCsv(string[] ids, string[] finalHeader)
        {
            var listOfBeneficiaries = dataGateway.GetListOfBeneficiaries();
            var finalListOfBeneficiaries = new List<Beneficiary>();
            foreach (var b in listOfBeneficiaries)
            {
                if (ids.Contains(b.Id))
                {
                    finalListOfBeneficiaries.Add(b);
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
                string[] arrbeneficiary = BeneficiaryToArray(item);
                foreach (var data in arrbeneficiary)
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

                headerList.Add(header[1]);

            if (properties.Contains("2"))

                headerList.Add(header[2]);

            if (properties.Contains("3"))

                headerList.Add(header[3]);

            if (properties.Contains("4"))
                headerList.Add(header[4]);

            if (properties.Contains("5"))
                headerList.Add(header[5]);

            if (properties.Contains("6"))

                headerList.Add(header[6]);

            if (properties.Contains("7"))

                headerList.Add(header[7]);

            if (properties.Contains("8"))
                headerList.Add(header[8]);

            var returnedHeader = headerList.ToArray();
            return returnedHeader;
        }

        //public string GetBeneficiaries(string properties, string[] ids, string[] header)
        //{
        //    var usedHeader = GetUsedHeader(properties, header);
        //    var listOfBeneficiary = dataGateway.GetListOfBeneficiaries();
        //    var idListForPrinting = new List<Beneficiary>();
        //    foreach (var b in listOfBeneficiary)
        //    {
        //        if (ids.Contains(b.Id))
        //        {
        //            idListForPrinting.Add(b);
        //        }
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    foreach (var data in usedHeader)
        //    {
        //        sb.Append("\"" + data + "\"" + ",");
        //    }

        //    sb.Append("\r\n");
        //    foreach (var item in idListForPrinting)
        //    {
        //        string[] arrBeneficiary = BeneficiaryToArray(item);

        //        if (properties.Contains("1"))

        //            sb.Append("\"" + arrBeneficiary[1] + "\"" + ",");

        //        if (properties.Contains("2"))

        //            sb.Append("\"" + arrBeneficiary[2] + "\"" + ",");

        //        if (properties.Contains("3"))

        //            sb.Append("\"" + arrBeneficiary[3] + "\"" + ",");

        //        if (properties.Contains("4"))
        //            sb.Append("\"" + arrBeneficiary[4] + "\"" + ",");

        //        if (properties.Contains("5"))
        //            sb.Append("\"" + arrBeneficiary[5] + "\"" + ",");

        //        if (properties.Contains("6"))

        //            sb.Append("\"" + arrBeneficiary[6] + "\"" + ",");

        //        if (properties.Contains("7"))

        //            sb.Append("\"" + arrBeneficiary[7] + "\"" + ",");

        //        if (properties.Contains("8"))
        //            sb.Append("\"" + arrBeneficiary[8] + "\"" + ",");

        //        sb.Append("\r\n");
        //    }
        //    return sb.ToString();
        //}
    }
}