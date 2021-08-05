using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.SponsorGateways;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            var emptyDatabase = false;
            var sponsors = dataGateway.GetListOfSponsors();
            if (sponsors.Count == 0)
                emptyDatabase = true;

            sponsors = GetSponsorsAfterFilters(sponsors, request.FilterData);

            var sponsorsAfterFiltering = sponsors.Count();

            var stringOfIDs = GetStringOfIds(sponsors);

            sponsors = GetSponsorsAfterPaging(sponsors, request.PagingData);

            return new SponsorsMainDisplayIndexResponse(sponsors, request.FilterData, request.PagingData, emptyDatabase, sponsorsAfterFiltering, stringOfIDs,Constants.SPONSORSESSION);
        }

        private string GetStringOfIds(List<Sponsor> sponsors)
        {
            var stringOfIDs = "sponsorCSV";
            foreach (Sponsor s in sponsors)
            {
                stringOfIDs = stringOfIDs + "," + s.Id;
            }
            return stringOfIDs;
        }

        private List<Sponsor> GetSponsorsAfterFilters(List<Sponsor> sponsors, FilterData filterData)
        {
            sponsors = sponsors.Where(x => x.NameOfSponsor.Contains(filterData.NameOfSponsor, StringComparison.InvariantCultureIgnoreCase)).ToList();
            sponsors = sponsors.Where(x => x.ContactInformation.PhoneNumber.Contains(filterData.ContactInfo, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAddress.Contains(filterData.ContactInfo, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.HasContract == true)
                sponsors = sponsors.Where(x => x.Contract.HasContract == true).ToList();
            sponsors = sponsors.Where(x => x.Sponsorship.WhatGoods.Contains(filterData.WhatGoods, StringComparison.InvariantCultureIgnoreCase)).ToList();
            sponsors = sponsors.Where(x => x.Sponsorship.MoneyAmount.Contains(filterData.MoneyAmount, StringComparison.InvariantCultureIgnoreCase)).ToList();
            sponsors = sponsors.Where(x => x.Sponsorship.GoodsAmount.Contains(filterData.GoodsAmount, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (DateInputReceived(filterData.LowerDate))
            {
                sponsors = sponsors.Where(x => x.Contract.RegistrationDate > filterData.LowerDate).ToList();
            }
            if (DateInputReceived(filterData.UpperDate))
            {
                sponsors = sponsors.Where(x => x.Contract.RegistrationDate <= filterData.UpperDate).ToList();
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

        public PagingData PagingData { get; set; }

        public SponsorsMainDisplayIndexRequest(string sponsorName, int page, int nrOfDocs, string contactInfo, DateTime lowerDate, DateTime upperDate, bool hasContract, string whatGoods, string moneyAmount, string goodsAmount)
        {
            var filterData = new FilterData();
            var pagingData = new PagingData();

            filterData.NameOfSponsor = sponsorName ?? "";
            filterData.ContactInfo = contactInfo ?? "";
            filterData.LowerDate = lowerDate;
            filterData.UpperDate = upperDate;
            filterData.HasContract = hasContract;
            filterData.WhatGoods = whatGoods ?? "";
            filterData.MoneyAmount = moneyAmount ?? "";
            filterData.GoodsAmount = goodsAmount ?? "";

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

        public bool EmptyDatabase { get; set; }

        public int TotalSponsors { get; set; }

        public string StringOfIDs { get; set; }

        public string DictionaryKey { get; set; }

        public SponsorsMainDisplayIndexResponse(List<Sponsor> sponsors, FilterData filterData, PagingData pagingData, bool emptyDatabase, int sponsorsAfterFiltering, string stringOfIDs, string dictionaryKey)
        {
            Sponsors = sponsors;
            FilterData = filterData;
            PagingData = pagingData;
            TotalSponsors = sponsorsAfterFiltering;
            EmptyDatabase = emptyDatabase;
            StringOfIDs = stringOfIDs;
            DictionaryKey =dictionaryKey;
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
        public string ContactInfo { get; set; }
        public string WhatGoods { get; set; }
        public string MoneyAmount { get; set; }
        public string GoodsAmount { get; set; }
        public bool HasContract { get; set; }
        public DateTime LowerDate { get; set; }
        public DateTime UpperDate { get; set; }
    }
}