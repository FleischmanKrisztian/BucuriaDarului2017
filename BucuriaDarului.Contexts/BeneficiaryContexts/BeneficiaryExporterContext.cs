using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
    public class BeneficiaryExporterContext
    {
        private readonly IStringLocalizer localizer;

        public BeneficiaryExporterContext(IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        //public BeneficiaryExporterResponse Execute(BeneficiaryExporterRequest request)
        //{
        //    //var idsAndFields = GetIdAndFieldString(request.ExportParameters);

        //    var header = GetHeaderForExcelPrinterEvent(localizer);
        //    var response = new BeneficiaryExporterResponse(CreateDictionaries(Constants.BENEFICIARYSESSION, Constants.BENEFICIARYHEADER, idsAndFields, header));
        //    response.IsValid = CheckForProperties(idsAndFields);
        //    return response;
        //}

        private bool CheckForProperties(string idsAndFields)
        {
            var splitstring = idsAndFields.Split("(((");
            if (splitstring[1] == "")
                return false;

            return true;
        }

        //private string GetIdAndFieldString(ExportParameters csv)
        //{
        //    string ids_and_options = csv.StringOfIDs + "(((";
        //    if (All == true)
        //        ids_and_options = ids_and_options + "0";
        //    if (Fullname == true)
        //        ids_and_options = ids_and_options + "1";
        //    if (Active == true)
        //        ids_and_options = ids_and_options + "2";
        //    if (Canteen == true)
        //        ids_and_options = ids_and_options + "3";
        //    if (HomeDelivery == true)
        //        ids_and_options = ids_and_options + "4";
        //    if (HomeDeliveryDriver == true)
        //        ids_and_options = ids_and_options + "5";
        //    if (HasGDPRAgreement == true)
        //        ids_and_options = ids_and_options + "6";
        //    if (Adress == true)
        //        ids_and_options = ids_and_options + "7";
        //    if (CNP == true)
        //        ids_and_options = ids_and_options + "8";
        //    if (CI_Info == true)
        //        ids_and_options = ids_and_options + "9";
        //    if (marca == true)
        //        ids_and_options = ids_and_options + "A";
        //    if (IdInvestigation == true)
        //        ids_and_options = ids_and_options + "B";
        //    if (IdAplication == true)
        //        ids_and_options = ids_and_options + "C";
        //    if (NumberOfPortions == true)
        //        ids_and_options = ids_and_options + "D";
        //    if (LastTimeActiv == true)
        //        ids_and_options = ids_and_options + "E";
        //    if (PhoneNumber == true)
        //        ids_and_options = ids_and_options + "F";
        //    if (BirthPlace == true)
        //        ids_and_options += "G";
        //    if (Studies == true)
        //        ids_and_options = ids_and_options + "H";
        //    if (Profesion == true)
        //        ids_and_options = ids_and_options + "I";
        //    if (Ocupation == true)
        //        ids_and_options = ids_and_options + "J";
        //    if (SeniorityInWorkField == true)
        //        ids_and_options = ids_and_options + "K";
        //    if (HealthState == true)
        //        ids_and_options = ids_and_options + "L";
        //    if (Disalility == true)
        //        ids_and_options = ids_and_options + "M";
        //    if (ChronicCondition == true)
        //        ids_and_options = ids_and_options + "N";
        //    if (Addictions == true)
        //        ids_and_options = ids_and_options + "O";
        //    if (HealthInsurance == true)
        //        ids_and_options = ids_and_options + "Z";
        //    if (HealthCard == true)
        //        ids_and_options = ids_and_options + "P";
        //    if (Married == true)
        //        ids_and_options = ids_and_options + "Q";
        //    if (SpouseName == true)
        //        ids_and_options = ids_and_options + "R";
        //    if (HasHome == true)
        //        ids_and_options = ids_and_options + "S";
        //    if (HousingType == true)
        //        ids_and_options = ids_and_options + "T";
        //    if (Income == true)
        //        ids_and_options = ids_and_options + "U";
        //    if (Expences == true)
        //        ids_and_options = ids_and_options + "V";
        //    if (Gender == true)
        //        ids_and_options = ids_and_options + "W";
        //    if (WeeklyPackage == true)
        //        ids_and_options = ids_and_options + "Z";
        //    return ids_and_options;
        //}

        private string GetHeaderForExcelPrinterEvent(IStringLocalizer _localizer)
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

        private Dictionary<string, string> CreateDictionaries(string key1, string key2, string idsAndFields, string header)
        {
            var result = new Dictionary<string, string>
            {
                { key1, idsAndFields },
                { key2, header }
            };

            return result;
        }
    }

    public class BeneficiaryExporterResponse
    {
        public Dictionary<string, string> Dictionary { get; set; }

        public bool IsValid { get; set; }

        public BeneficiaryExporterResponse(Dictionary<string, string> dictionary)
        {
            Dictionary = dictionary;
            IsValid = true;
        }
    }

    public class BeneficiaryExporterRequest
    {
        public BeneficiaryExporterRequest(ExportParameters exportProperties)
        {
            ExportParameters = exportProperties;
        }

        public ExportParameters ExportParameters { get; set; }
    }

    public class ExportParameters
    {
        public string StringOfIDs { get; set; }
        public bool All { get; set; }
        //  INTORDUS VARIABILELE DOIN VIEW
    }
}