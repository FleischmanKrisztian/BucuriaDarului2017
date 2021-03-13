using Finalaplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.ControllerHelpers.SponsorHelpers
{
    public class SponsorFunctions
    {
        internal static string GetStringOfIds(List<Sponsor> sponsors)
        {
            string stringofids = "sponsor";
            foreach (Sponsor sponsor in sponsors)
            {
                stringofids = stringofids + "," + sponsor.SponsorID;
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
                Sponsor singlesponsor = sponsors.Where(x => x.SponsorID == sponsorids[i]).First();
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
    }
}
