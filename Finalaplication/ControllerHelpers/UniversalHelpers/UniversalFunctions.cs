using Finalaplication.Common;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;

namespace Finalaplication.ControllerHelpers.UniversalHelpers
{
    public class UniversalFunctions
    {
        public static bool DateExpiryChecker(DateTime date)
        {
            var now = DateTime.Now;
            var firstday = now.AddDays(-1);
            var lastday = now.AddDays(10);
            var answer = false;
            if (date >= firstday && date <= lastday)
            {
                answer = true;
            }
            return answer;
        }

        public static bool IsAboutToExpire(int currentday, int daytocompareto)
        {
            if (currentday <= daytocompareto && currentday + 10 > daytocompareto || currentday > 355 && daytocompareto < 9)
            {
                return true;
            }
            return false;
        }

        public static bool File_is_not_empty(IFormFile file)
        {
            if (file.Length > 0)
                return true;
            else
                return false;
        }

        internal static void CreateFileStream(IFormFile Files, string path)
        {
            using var stream = new FileStream(path, FileMode.Create);
            Files.CopyTo(stream);
        }

        internal static void RemoveTempFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        public static int GetNumberOfItemPerPageFromSettings(ITempDataDictionary tempDataDic)
        {
            try
            {
                string numberOfDocumentsAsString = tempDataDic.Peek(VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE).ToString();
                return Convert.ToInt16(numberOfDocumentsAsString);
            }
            catch
            {
                return VolMongoConstants.DEFAULT_NUMBER_OF_ITEMS_PER_PAGE;
            }
        }

        internal static dynamic GetNumberOfExpiringBeneficiaryContracts(List<Beneficiarycontract> beneficiarycontracts)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            int beneficiaryContractsCounter = 0;
            foreach (var item in beneficiarycontracts)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(item.ExpirationDate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    beneficiaryContractsCounter++;
                }
            }
            return beneficiaryContractsCounter;
        }

        internal static dynamic GetNumberOfExpiringSponsorContracts(List<Sponsor> sponsors)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            int sponsorContractsCounter = 0;
            foreach (var item in sponsors)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(item.Contract.ExpirationDate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    sponsorContractsCounter++;
                }
            }
            return sponsorContractsCounter;
        }

        internal static dynamic GetNumberOfExpiringVolContracts(List<Volcontract> volcontracts)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            int volunteerContractsCounter = 0;
            foreach (var item in volcontracts)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(item.ExpirationDate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    volunteerContractsCounter++;
                }
            }
            return volunteerContractsCounter;
        }

        internal static int GetNumberOfVolunteersWithBirthdays(List<Volunteer> volunteers)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            int birthdayCounter = 0;
            foreach (var item in volunteers)
            {
                int volbdday = UniversalFunctions.GetDayOfYear(item.Birthdate);
                if (UniversalFunctions.IsAboutToExpire(currentday, volbdday))
                {
                    birthdayCounter++;
                }
            }
            return birthdayCounter;
        }

        internal static int GetCurrentPage(int page)
        {
            if (page > 0)
                return page;
            else
            {
                page = 1;
                return page;
            }
        }

        internal static int GetDayOfYear(DateTime date)
        {
            string dateasstring = date.ToString("dd-MM-yyyy");
            string[] dates = dateasstring.Split('-');
            int Day = Convert.ToInt16(dates[0]);
            int Month = Convert.ToInt16(dates[1]);
            Day = (Month - 1) * 30 + Day;
            return Day;
        }

        internal static bool ContainsSpecialChar(string incomingstring)
        {
            bool containsspecialchar = false;
            if (incomingstring.Contains(";"))
            {
                containsspecialchar = true;
            }
            return containsspecialchar;
        }

        internal static byte[] Addimage(IFormFile image)
        {
            byte[] returnedimage = null;
            if (image != null)
            {
                var stream = new MemoryStream();
                image.CopyTo(stream);
                returnedimage = stream.ToArray();
            }
            return returnedimage;
        }
    }
}