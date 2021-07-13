using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Web.ControllerHelpers.UniversalHelpers;
using BucuriaDarului.Web.Models;

namespace BucuriaDarului.Web.ControllerHelpers.SponsorHelpers
{
    public class SponsorFunctions
    {
        internal static string GetStringOfIds(List<Sponsor> sponsors)
        {
            string stringofids = "sponsorCSV";
            foreach (Sponsor sponsor in sponsors)
            {
                stringofids = stringofids + "," + sponsor.Id;
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
                Sponsor singlesponsor = sponsors.Where(x => x.Id == sponsorids[i]).First();
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
                    if (s.ContactInformation.MailAddress == null || s.ContactInformation.MailAddress == "")
                        s.ContactInformation.MailAddress = "-";
                }

                sponsors = sp.Where(x => x.ContactInformation.PhoneNumber.Contains(ContactInfo, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAddress.Contains(ContactInfo, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
    }
}