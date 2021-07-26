using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var emptyDatabase = false;
            var volunteers = dataGateway.GetListOfVolunteers();
            if (volunteers.Count == 0)
                emptyDatabase = true;
            volunteers = GetVolunteersAfterFilters(volunteers, request.FilterData);
            var volunteersAfterFiltering = volunteers.Count;
            var stringOfIDs = GetStringOfIds(volunteers);
            volunteers = GetVolunteersAfterPaging(volunteers, request.PagingData);

            return new VolunteerMainDisplayIndexResponse(volunteers, request.FilterData, request.PagingData, emptyDatabase, volunteersAfterFiltering, stringOfIDs);
        }

        public List<Volunteer> GetVolunteersAfterFilters(List<Volunteer> volunteers, FilterData filterData)
        {
            if (filterData.SearchedFullname != null)
            {
                volunteers = volunteers.Where(x => x.Fullname.Contains(filterData.SearchedFullname, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.Active)
            {
                volunteers = volunteers.Where(x => x.InActivity).ToList();
            }
            if (filterData.SearchedWorkplace != null)
            {
                volunteers = volunteers.Where(x => x.DesiredWorkplace.Contains(filterData.SearchedWorkplace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchedOccupation != null)
            {
                volunteers = volunteers.Where(x => x.FieldOfActivity.Contains(filterData.SearchedOccupation, StringComparison.InvariantCultureIgnoreCase) || x.Occupation.Contains(filterData.SearchedOccupation, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchedRemarks != null)
            {
                volunteers = volunteers.Where(x => x.AdditionalInfo.Remark.Contains(filterData.SearchedRemarks, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (filterData.SearchedContact != null)
            {
                volunteers = volunteers.Where(x => x.ContactInformation.PhoneNumber.Contains(filterData.SearchedContact, StringComparison.InvariantCultureIgnoreCase) || x.ContactInformation.MailAddress.Contains(filterData.SearchedContact, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (filterData.SearchedHourCount != 0)
            {
                volunteers = volunteers.Where(x => x.HourCount.Equals(filterData.SearchedHourCount)).ToList();
            }

            if (DateInputReceived(filterData.LowerDate))
            {
                volunteers = volunteers.Where(x => x.Birthdate > filterData.LowerDate).ToList();
            }
            if (DateInputReceived(filterData.UpperDate))
            {
                volunteers = volunteers.Where(x => x.Birthdate <= filterData.UpperDate).ToList();
            }
            if (filterData.Gender != " All")
            {
                if (filterData.Gender == "Male")
                {
                    volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Male)).ToList();
                }
                if (filterData.Gender == "Female")
                { volunteers = volunteers.Where(x => x.Gender.Equals(Gender.Female)).ToList(); }
            }

            if (filterData.HasDrivingLicense)
            {
                volunteers = volunteers.Where(x => x.AdditionalInfo.HasDrivingLicense).ToList();
            }
            if (filterData.HasCar)
            {
                volunteers = volunteers.Where(x => x.AdditionalInfo.HasCar).ToList();
            }

            switch (filterData.SortOrder.SortOrder)
            {
                case "Fullname_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Fullname).ToList();
                    break;

                case "Gender":
                    volunteers = volunteers.OrderBy(s => s.Gender).ToList();
                    break;

                case "Gender_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Gender).ToList();
                    break;

                case "HourCount":
                    volunteers = volunteers.OrderBy(s => s.HourCount).ToList();
                    break;

                case "HourCount_desc":
                    volunteers = volunteers.OrderByDescending(s => s.HourCount).ToList();
                    break;

                case "Active":
                    volunteers = volunteers.OrderBy(s => s.InActivity).ToList();
                    break;

                case "Active_desc":
                    volunteers = volunteers.OrderByDescending(s => s.InActivity).ToList();
                    break;

                case "Date":
                    volunteers = volunteers.OrderBy(s => s.Birthdate).ToList();
                    break;

                case "Date_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Birthdate).ToList();
                    break;

                default:
                    volunteers = volunteers.OrderBy(s => s.Fullname).ToList();
                    break;
            }
            return volunteers;
        }

        private string GetStringOfIds(List<Volunteer> volunteers)
        {
            var stringOfIDs = "volunteerCSV";
            foreach (Volunteer v in volunteers)
            {
                stringOfIDs = stringOfIDs + "," + v.Id;
            }
            return stringOfIDs;
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
        public PagingData PagingData { get; set; }

        public VolunteerMainDisplayIndexRequest(int nrOfDocs, int page, string searchedFullname, string searchedContact, string sortOrder, bool active, bool hasCar, bool hasDrivingLicense, DateTime lowerDate, DateTime upperDate, string gender, string searchedAddress, string searchedWorkplace, string searchedOccupation, string searchedRemarks, int searchedHourCount)
        {
            var filterData = new FilterData();
            var pagingData = new PagingData();
            filterData.SearchedFullname = searchedFullname;
            filterData.SearchedContact = searchedContact;
            filterData.Active = active;
            filterData.HasCar = hasCar;
            filterData.HasDrivingLicense = hasDrivingLicense;
            filterData.LowerDate = lowerDate;
            filterData.UpperDate = upperDate;
            filterData.Gender = gender;
            filterData.SearchedAddress = searchedAddress;
            filterData.SearchedWorkplace = searchedWorkplace;
            filterData.SearchedOccupation = searchedOccupation;
            filterData.SearchedRemarks = searchedRemarks;
            if (searchedHourCount != 0)
                filterData.SearchedHourCount = searchedHourCount;
            filterData.Gender = gender;
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
        public List<Volunteer> Volunteers { get; set; }

        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public int TotalVolunteers { get; set; }

        public bool EmptyDatabase { get; set; }

        public string StringOfIDs { get; set; }

        public VolunteerMainDisplayIndexResponse(List<Volunteer> volunteers, FilterData filterData, PagingData pagingData, bool emptyDatabase, int volunteersAfterFiltering, string stringOfIDs)
        {
            Volunteers = volunteers;
            FilterData = filterData;
            PagingData = pagingData;
            TotalVolunteers = volunteersAfterFiltering;
            EmptyDatabase = emptyDatabase;
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
        public string SearchedFullname { get; set; }
        public string SearchedContact { get; set; }
        public bool Active { get; set; }
        public bool HasCar { get; set; }
        public bool HasDrivingLicense { get; set; }
        public DateTime LowerDate { get; set; }
        public DateTime UpperDate { get; set; }
        public string Gender { get; set; }
        public string SearchedAddress { get; set; }
        public string SearchedWorkplace { get; set; }
        public string SearchedOccupation { get; set; }
        public string SearchedRemarks { get; set; }
        public int SearchedHourCount { get; set; }
        public Sort SortOrder { get; set; }
    }

    public class Sort
    {
        public string SortOrder { get; set; }
        public string DateSortParam { get; set; }
        public string FullnameSort { get; set; }
        public string GenderSort { get; set; }
        public string HourCountSort { get; set; }
        public string ActiveSort { get; set; }

        public Sort(string sortOrder)
        {
            SortOrder = sortOrder;

            DateSortParam = sortOrder == "Date" ? "Date_desc" : "Date";
            FullnameSort = sortOrder == "Fullname" ? "Fullname_desc" : "Fullname";
            HourCountSort = sortOrder == "HourCount" ? "HourCount_desc" : "HourCount";
            GenderSort = sortOrder == "Gender" ? "Gender_desc" : "Gender";
            ActiveSort = sortOrder == "Active" ? "Active_desc" : "Active";
        }
    }
}