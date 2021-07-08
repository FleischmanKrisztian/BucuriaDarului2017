using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerMainDisplayIndexContext
    {
        private readonly IVolunteerMainDisplayIndexGateway dataGateway;

        public VolunteerMainDisplayIndexContext(IVolunteerMainDisplayIndexGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public VolunteerMainDisplayIndexResponse Execute(VolunteerMainDisplayIndexRequest request)
        {
            var volunteers = dataGateway.GetListOfVolunteers();
            volunteers = GetVolunteerAfterFilters(volunteers, request.FilterData);
            var volunteersAfterFiltering = volunteers.Count();
            var stringOfIDs = GetStringOfIds(volunteers);
            volunteers = GetVolunteersAfterPaging(volunteers, request.PagingData);

            return new VolunteerMainDisplayIndexResponse(volunteers, request.FilterData, request.PagingData, volunteersAfterFiltering, stringOfIDs);
        }

        private string GetStringOfIds(List<Volunteer> volunteers)
        {
            var stringOfIDs = "volunteerCSV";
            foreach (Volunteer b in volunteers)
            {
                stringOfIDs = stringOfIDs + "," + b.Id;
            }
            return stringOfIDs;
        }

        private List<Volunteer> GetVolunteerAfterFilters(List<Volunteer> volunteers, FilterData filterData)
        {
            //CODE HERE
            
            return volunteers;
        }

        private List<Volunteer> GetVolunteersAfterPaging(List<Volunteer> volunteers, PagingData pagingData)
        {
            volunteers = volunteers.AsQueryable().Skip((pagingData.CurrentPage - 1) * pagingData.NrOfDocumentsPerPage).ToList();
            volunteers = volunteers.AsQueryable().Take(pagingData.NrOfDocumentsPerPage).ToList();
            return volunteers;
        }

        private static bool DateInputReceived(DateTime date)
        {
            var comparisonDate = new DateTime(0003, 1, 1);
            return date > comparisonDate;
        }
    }

    public class VolunteerMainDisplayIndexRequest
    {
        public FilterData FilterData { get; set; }
        public Sort SortOrder { get; set; }
        public PagingData PagingData { get; set; }

        public VolunteerMainDisplayIndexRequest(string searching, int page, int nrOfDocs, string sortOrder, bool active, string searchingBirthPlace, bool hasContract, bool homeless, DateTime lowerDate, DateTime upperDate, DateTime activeSince, DateTime activeTill, bool weeklyPackage, bool canteen, bool homeDelivery, string searchingDriver, bool hasGDPRAgreement, string searchingAddress, bool hasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpenses, string gender)
        {
            var filterData = new FilterData();
            var pagingData = new PagingData();

            //CODE HERE
            filterData.SortOrder = new Sort(sortOrder);

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

    public class VolunteerMainDisplayIndexResponse
    {
        public List<Volunteer> Volunteer { get; set; }

        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public int TotalVolunteers { get; set; }

        public string StringOfIDs { get; set; }

        public VolunteerMainDisplayIndexResponse(List<Volunteer> volunteers, FilterData filterData, PagingData pagingData, int volunteersAfterFiltering, string stringOfIDs)
        {
            Volunteer = volunteers;
            FilterData = filterData;
            PagingData = pagingData;

            TotalVolunteers = volunteersAfterFiltering;
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

        public Sort SortOrder { get; set; }
    }

    public class Sort
    {
        public string SortOrder { get; set; }
        public string NameSortParm { get; set; }
        public string DateSortParm { get; set; }
        public string FullnameSort { get; set; }
        public string Gendersort { get; set; }
        public string Activesort { get; set; }

        public Sort(string sortOrder)
        {
            SortOrder = sortOrder;
            NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            FullnameSort = sortOrder == "Fullname" ? "Fullname_desc" : "Fullname";
            Gendersort = sortOrder == "Gender" ? "Gender_desc" : "Gender";
            Activesort = sortOrder == "Active" ? "Active_desc" : "Active";
        }
    }
}