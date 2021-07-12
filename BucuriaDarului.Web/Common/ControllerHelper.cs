using BucuriaDarului.Web.Controllers;
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

       

        public static string GetHeaderForExcelPrinterSponsor(IStringLocalizer<SponsorController> _localizer)
        {
            string[] header = new string[10];
            header[0] = _localizer["NameOfSponsor"];
            header[1] = _localizer["Date"];
            header[2] = _localizer["MoneyAmount"];
            header[3] = _localizer["WhatGoods"];
            header[4] = _localizer["GoodsAmount"];
            header[5] = _localizer["HasContract"];
            header[6] = _localizer["ContractDetails"];
            header[7] = _localizer["PhoneNumber"];
            header[8] = _localizer["EmailAddress"];
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