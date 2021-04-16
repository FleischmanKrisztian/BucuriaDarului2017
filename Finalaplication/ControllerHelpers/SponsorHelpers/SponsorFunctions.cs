using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VolCommon;

namespace Finalaplication.ControllerHelpers.SponsorHelpers
{
    public class SponsorFunctions
    {
        internal static Sponsor GetSponsorFromString(string[] sponsorstring)
        {
            Sponsor newsponsor = new Sponsor();
            Sponsorship s = new Sponsorship();
            Contract c = new Contract();
            ContactInformation ci = new ContactInformation();

            newsponsor._id = Guid.NewGuid().ToString();
            newsponsor.NameOfSponsor = sponsorstring[0];

            try
            {
                s.Date = Convert.ToDateTime(sponsorstring[1]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                s.Date = DateTime.MinValue;
            }

            s.MoneyAmount = sponsorstring[2];
            s.WhatGoods = sponsorstring[3];
            s.GoodsAmount = sponsorstring[4];

            newsponsor.Sponsorship = s;
            if (sponsorstring[5] == "True" || sponsorstring[5] == "true")
            {
                c.HasContract = true;
            }
            else
            {
                c.HasContract = false;
            }
            c.HasContract = Convert.ToBoolean(sponsorstring[5]);
            c.NumberOfRegistration = sponsorstring[6];

            try
            {
                c.RegistrationDate = Convert.ToDateTime(sponsorstring[7]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                c.RegistrationDate = DateTime.MinValue;
            }
            try
            {
                c.ExpirationDate = Convert.ToDateTime(sponsorstring[8]);
            }
            catch
            {
                Console.WriteLine("Invalid Date, defaulting to min value!");
                c.ExpirationDate = DateTime.MinValue;
            }
            newsponsor.Contract = c;

            ci.PhoneNumber = sponsorstring[9];
            ci.MailAdress = sponsorstring[10];
            newsponsor.ContactInformation = ci;
            return newsponsor;
        }

        internal static string GetStringOfIds(List<Sponsor> sponsors)
        {
            string stringofids = "sponsorCSV";
            foreach (Sponsor sponsor in sponsors)
            {
                stringofids = stringofids + "," + sponsor._id;
            }
            return stringofids;
        }

        internal static List<Sponsor> GetSponsorsAfterPaging(List<Sponsor> sponsors, int page, int nrofdocs)
        {
            sponsors = sponsors.AsQueryable().Skip((page - 1) * nrofdocs).ToList();
            sponsors = sponsors.AsQueryable().Take(nrofdocs).ToList();
            return sponsors;
        }

        internal static List<Sponsor> GetSponsorsAfterSearching(List<Sponsor> sponsors, string searching)
        {
            if (searching != null)
            {
                sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return sponsors;
        }

        internal static List<Sponsor> GetSponsorsByIds(List<Sponsor> sponsors, string[] sponsorids)
        {
            List<Sponsor> sponsorlist = new List<Sponsor>();
            for (int i = 0; i < sponsorids.Length; i++)
            {
                Sponsor singlesponsor = sponsors.Where(x => x._id == sponsorids[i]).First();
                sponsorlist.Add(singlesponsor);
            }
            return sponsorlist;
        }

        internal static string GetSponsorNames(List<Sponsor> sponsors)
        {
            string sponsornames = "";
            for (int i = 0; i < sponsors.Count; i++)
            {
                var sponsor = sponsors[i];
                sponsornames = sponsornames + sponsor.NameOfSponsor + " / ";
            }
            return sponsornames;
        }

        internal static string GetIdAndFieldString(string IDS, bool All, bool NameOfSponsor, bool Date, bool MoneyAmount, bool WhatGoods, bool GoodsAmount, bool HasContract, bool ContractDetails, bool PhoneNumber, bool MailAdress)
        {
            string ids_and_options = IDS + "(((";
            if (All == true)
                ids_and_options += "0";
            if (NameOfSponsor == true)
                ids_and_options += "1";
            if (Date == true)
                ids_and_options += "2";
            if (HasContract == true)
                ids_and_options += "3";
            if (ContractDetails == true)
                ids_and_options += "4";
            if (PhoneNumber == true)
                ids_and_options += "5";
            if (MailAdress == true)
                ids_and_options += "6";
            if (MoneyAmount == true)
                ids_and_options += "7";
            if (WhatGoods == true)
                ids_and_options += "8";
            if (GoodsAmount == true)
                ids_and_options += "9";

            return ids_and_options;
        }

        private static bool Dateinputreceived(DateTime date)
        {
            DateTime comparisondate = new DateTime(0003, 1, 1);
            if (date > comparisondate)
                return true;
            else
                return false;
        }

        internal static List<Sponsor> GetSponsorsAfterFilters(List<Sponsor> sponsors, string searching, string ContactInfo, DateTime lowerdate, DateTime upperdate, bool HasContract, string WhatGoods, string MoneyAmount, string GoodsAmounts)
        {
            if (searching != null)
            {
                sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(searching, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (ContactInfo != null)
            {
                List<Sponsor> sp = sponsors;
                foreach (var s in sp)
                {
                    if (s.ContactInformation.PhoneNumber == null || s.ContactInformation.PhoneNumber == "")
                        s.ContactInformation.PhoneNumber = "-";
                    if (s.ContactInformation.MailAdress == null || s.ContactInformation.MailAdress == "")
                        s.ContactInformation.MailAdress = "-";
                }

                sponsors = sp.Where(x => x.ContactInformation.PhoneNumber.Contains(ContactInfo, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAdress.Contains(ContactInfo, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (WhatGoods != null)
            {
                List<Sponsor> sp = sponsors;
                foreach (var s in sp)
                {
                    if (s.Sponsorship.WhatGoods == null || s.Sponsorship.WhatGoods == "")
                        s.Sponsorship.WhatGoods = "-";
                }

                sponsors = sp.Where(x => x.Sponsorship.WhatGoods.Contains(WhatGoods, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (GoodsAmounts != null)
            {
                List<Sponsor> sp = sponsors;
                foreach (var s in sp)
                {
                    if (s.Sponsorship.GoodsAmount == null || s.Sponsorship.GoodsAmount == "")
                        s.Sponsorship.GoodsAmount = "-";
                }

                sponsors = sp.Where(x => x.Sponsorship.GoodsAmount.Contains(GoodsAmounts, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (MoneyAmount != null)
            {
                List<Sponsor> sp = sponsors;
                foreach (var s in sp)
                {
                    if (s.Sponsorship.MoneyAmount == null || s.Sponsorship.MoneyAmount == "")
                        s.Sponsorship.MoneyAmount = "-";
                }

                sponsors = sp.Where(x => x.Sponsorship.MoneyAmount.Contains(MoneyAmount, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (HasContract == true)
            {
                sponsors = sponsors.Where(x => x.Contract.HasContract == true).ToList();
            }

            if (Dateinputreceived(lowerdate))
            {
                sponsors = sponsors.Where(x => x.Sponsorship.Date > lowerdate).ToList();
            }
            if (Dateinputreceived(upperdate))
            {
                sponsors = sponsors.Where(x => x.Sponsorship.Date <= upperdate).ToList();
            }

            return sponsors;
        }

        internal static List<Sponsor> GetExpiringContracts(List<Sponsor> sponsors)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            List<Sponsor> returnlistofspons = new List<Sponsor>();
            foreach (var spons in sponsors)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(spons.Contract.ExpirationDate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    returnlistofspons.Add(spons);
                }
            }
            return returnlistofspons;
        }
    }
}