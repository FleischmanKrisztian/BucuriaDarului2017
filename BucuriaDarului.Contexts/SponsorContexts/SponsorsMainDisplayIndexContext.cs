using System;
using System.Collections.Generic;
using System.Linq;
using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways;
using BucuriaDarului.Core.Gateways.SponsorGateways;

namespace BucuriaDarului.Contexts.SponsorContexts
{
    public class SponsorsMainDisplayIndexContext
    {
        private readonly ISponsorsMainDisplayIndexGateway dataGateway;

        public SponsorsMainDisplayIndexContext(ISponsorsMainDisplayIndexGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public SponsorsMainDisplayIndexResponse Execute(SponsorsMainDisplayIndexRequest request)
        {
            var sponsors = dataGateway.GetListOfSponsors();
            sponsors = GetSponsorsAfterFilters(sponsors, request.FilterData);
            var beneficiariesAfterFiltering = sponsors.Count();
            var stringOfIDs = GetStringOfIds(sponsors);
            sponsors = GetSponsorsAfterPaging(sponsors, request.PagingData);

            return new SponsorsMainDisplayIndexResponse(sponsors, request.FilterData, request.PagingData, beneficiariesAfterFiltering, stringOfIDs);
        }

        private string GetStringOfIds(List<Sponsor> sponsors)
        {
            var stringOfIDs = "sponsorCSV";
            foreach (Sponsor s in sponsors)
            {
                stringOfIDs = stringOfIDs + "," + s._id;
            }
            return stringOfIDs;
        }

        private List<Sponsor> GetSponsorsAfterFilters(List<Sponsor> sponsors, FilterData filterData)
        {
            if (filterData.NameOfSponsor != null)
                sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(filterData.NameOfSponsor, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.GoodsAmount != null)
                sponsors = sponsors.Where(x => x.Sponsorship.GoodsAmount.Contains(filterData.GoodsAmount, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.WhatGoods != null)
                sponsors = sponsors.Where(x => x.Sponsorship.WhatGoods.Contains(filterData.WhatGoods, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.MoneyAmount != null)
                sponsors = sponsors.Where(x => x.Sponsorship.MoneyAmount.Contains(filterData.MoneyAmount, StringComparison.InvariantCultureIgnoreCase)).ToList();
            // TODO: check this
            //if (filterData.ContactInfo != null)
                //sponsors = sponsors.Where(x => x.ContactInformation.Contains(filterData.ContactInfo, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.HasContract != false)
                sponsors = sponsors.Where(x => x.Contract.HasContract == true).ToList();
            if (DateInputReceived(filterData.LowerDate))
                sponsors = sponsors.Where(x => x.Contract.RegistrationDate <= filterData.LowerDate).ToList();
            if (DateInputReceived(filterData.UpperDate))
                sponsors = sponsors.Where(x => x.Contract.RegistrationDate <= filterData.UpperDate).ToList();


            switch (filterData.SortOrder.SortOrder)
            {
                // TODO: add cases for sorting
                default:
                    sponsors = sponsors.OrderBy(s => s.NameOfSponsor).ToList();
                    break;
            }
            return sponsors;
        }

        private List<Sponsor> GetSponsorsAfterPaging(List<Sponsor> sponsors, PagingData pagingData)
        {
            sponsors = sponsors.AsQueryable().Skip((pagingData.CurrentPage - 1) * pagingData.NrOfDocumentsPerPage).ToList();
            sponsors = sponsors.AsQueryable().Take(pagingData.NrOfDocumentsPerPage).ToList();
            return sponsors;
        }

        private static bool DateInputReceived(DateTime date)
        {
            var comparisonDate = new DateTime(0003, 1, 1);
            return date > comparisonDate;
        }
    }

    public class SponsorsMainDisplayIndexRequest
    {
        public FilterData FilterData { get; set; }
        public Sort SortOrder { get; set; }
        public PagingData PagingData { get; set; }

        public SponsorsMainDisplayIndexRequest(string searching, int page, int nrOfDocs, string ContactInfo, DateTime lowerDate, DateTime upperDate, bool hasContract, string WhatGoods, string MoneyAmount, string GoodsAmount)
        {
            var filterData = new FilterData();
            var pagingData = new PagingData();
            // TODO: still don't know wht this searching means
            // filterData.ContactInfo = searching ?? null;
            filterData.LowerDate = lowerDate;
            filterData.UpperDate = upperDate;
            filterData.HasContract = hasContract;
            filterData.WhatGoods = WhatGoods ?? null;
            filterData.MoneyAmount = MoneyAmount ?? null;
            filterData.GoodsAmount = GoodsAmount ?? null;
            // TODO: do I also need sorting??
            // filterData.SortOrder = new Sort(sortOrder);

            pagingData.CurrentPage = GetCurrentPage(page);
            pagingData.NrOfDocumentsPerPage = nrOfDocs;

            FilterData = filterData;
            PagingData = pagingData;
        }

        private int GetCurrentPage(int page)
        {
            if (page > 0)
                return page;
            else
            {
                page = 1;
                return page;
            }
        }
    }

    public class SponsorsMainDisplayIndexResponse
    {
        public List<Sponsor> Sponsors { get; set; }

        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public int TotalSponsors { get; set; }

        public string StringOfIDs { get; set; }

        public SponsorsMainDisplayIndexResponse(List<Sponsor> sponsors, FilterData filterData, PagingData pagingData, int sponsorsAfterFiltering, string stringOfIDs)
        {
            Sponsors = sponsors;
            FilterData = filterData;
            PagingData = pagingData;

            TotalSponsors = sponsorsAfterFiltering;
            StringOfIDs = stringOfIDs;
        }
    }

    public class PagingData
    {
        public int CurrentPage { get; set; }

        public int NrOfDocumentsPerPage { get; set; }
    }

    public class FilterData
    {
        public string NameOfSponsor { get; set; }
        public ContactInformation ContactInfo { get; set; }
        public string WhatGoods { get; set; }
        public string MoneyAmount { get; set; }
        public string GoodsAmount { get; set; }
        public bool HasContract { get; set; }
        public DateTime LowerDate { get; set; }
        public DateTime UpperDate { get; set; }

        public Sort SortOrder { get; set; }
    }

    public class Sort
    {
        public string SortOrder { get; set; }
        public string NameSortParm { get; set; }
        public string DateSortParm { get; set; }

        public Sort(string sortOrder)
        {
            SortOrder = sortOrder;
            NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
        }
    }
}
