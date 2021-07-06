using BucuriaDarului.Web.Controllers;
using Finalaplication.Common;
using Finalaplication.Controllers;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace BucuriaDarului.Web.Common
{
    public class ControllerHelper
    {
        public static string[] SplitHeader(string header)
        {
            var splitHeader = header.Split(",");

            return splitHeader;
        }

        public static string GetAnswer(string finalHeader, bool toBeCompared)
        {
            var result = string.Empty;
            if (finalHeader.ToString() == "Activ" || finalHeader.Contains("are") == true || finalHeader.Contains("Are") == true || finalHeader.Contains("Cantină") == true || finalHeader.Contains("Pachet") == true || finalHeader.Contains("Fără") == true || finalHeader.Contains("Livrare") == true || finalHeader.Contains("Mașină") == true)
            {
                if (toBeCompared == true)
                { result = "Da"; }
                else
                { result = "Nu"; }
            }
            else
            {
                if (toBeCompared == true)
                { result = "Yes"; }
                else
                { result = "No"; }
            }
            return result;
        }

        public static string GetHeaderForExcelPrinterVolunteer(IStringLocalizer<VolunteerController> _localizer)
        {
            string[] header = new string[25];
            header[0] = _localizer["Fullname"];
            header[1] = _localizer["Birthdate"];
            header[2] = _localizer["Address"];
            header[3] = _localizer["Gender"];
            header[4] = _localizer["Desiredarea"];
            header[5] = _localizer["CNP"];
            header[6] = _localizer["Fieldofactivity"];
            header[7] = _localizer["Ocupation"];
            header[8] = _localizer["CI_Info"];
            header[9] = _localizer["Active"];
            header[10] = _localizer["HourCount"];
            header[11] = _localizer["ContactInfo"];
            header[12] = _localizer["Additional_info"];
            header[14] = _localizer["District"];
            header[15] = _localizer["City"];
            header[16] = _localizer["Street"];
            header[17] = _localizer["Number"];
            header[18] = _localizer["CISeria"];
            header[19] = _localizer["CINr"];
            header[20] = _localizer["CIEliberat"];
            header[21] = _localizer["CIeliberator"];
            header[20] = _localizer["Phonenumber"];
            header[21] = _localizer["Emailaddress"];
            header[22] = _localizer["Hasdrivinglicense"];
            header[23] = _localizer["Hascar"];
            header[24] = _localizer["Remarks"];

            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        public static string GetHeaderForExcelPrinterSponsor(IStringLocalizer<SponsorController> _localizer)
        {
            string[] header = new string[10];
            header[0] = _localizer["Nameofsponsor"];
            header[1] = _localizer["Date"];
            header[2] = _localizer["Moneyamount"];
            header[3] = _localizer["Whatgoods"];
            header[4] = _localizer["Goodsamount"];
            header[5] = _localizer["HasContract"];
            header[6] = _localizer["ContractDetails"];
            header[7] = _localizer["Phonenumber"];
            header[8] = _localizer["Emailaddress"];
            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        public static string GetHeaderForExcelPrinterBeneficiary(IStringLocalizer<BeneficiaryController> _localizer)
        {
            string[] header = new string[35];
            header[0] = _localizer["Fullname"];
            header[1] = _localizer["Active"];
            header[2] = _localizer["Canteen"];
            header[3] = _localizer["HomeDelivery"];
            header[4] = _localizer["Homedeliverydriver"];
            header[5] = _localizer["HasGDPRagreement"];
            header[6] = _localizer["AddressInformation"];
            header[7] = _localizer["CNP"];
            header[8] = _localizer["CISeria"];
            header[9] = _localizer["CIEliberat"];
            header[10] = _localizer["Marca"];
            header[11] = _localizer["IDinvestigation"];
            header[12] = _localizer["IDapplication"];
            header[13] = _localizer["NumberOfPortions"];
            header[14] = _localizer["Lastimeactiv"];
            header[15] = _localizer["Phonenumber"];
            header[16] = _localizer["Birthplace"];
            header[17] = _localizer["Studies"];
            header[18] = _localizer["Profession"];
            header[19] = _localizer["Occupation"];
            header[20] = _localizer["Seniorityinworkfield"];
            header[21] = _localizer["Healthstate"];
            header[22] = _localizer["Disability"];
            header[23] = _localizer["Chroniccondition"];
            header[24] = _localizer["Addictions"];
            header[25] = _localizer["Hashealthinsurance"];
            header[26] = _localizer["Hashealthcard"];
            header[27] = _localizer["Married"];
            header[28] = _localizer["Spousename"];
            header[29] = _localizer["Homeless"];
            header[30] = _localizer["Housingtype"];
            header[31] = _localizer["Income"];
            header[32] = _localizer["Expenses"];
            header[33] = _localizer["Gender"];
            header[34] = _localizer["Weeklypackage"];

            string result = string.Empty;
            for (int i = 0; i < header.Count(); i++)
            {
                if (i == 0)
                { result = header[i]; }
                else
                { result = result + "," + header[i]; }
            }
            return result;
        }

        internal static void CreateDictionaries(string key1, string key2, string ids_and_fields, string header)
        {
            if (DictionaryHelper.d.ContainsKey(key1))
            {
                DictionaryHelper.d[key1] = ids_and_fields;
            }
            else
            {
                DictionaryHelper.d.Add(key1, ids_and_fields);
            }
            if (DictionaryHelper.d.ContainsKey(key2) == true)
            {
                DictionaryHelper.d[key2] = header;
            }
            else
            {
                DictionaryHelper.d.Add(key2, header);
            }
        }
    }
}