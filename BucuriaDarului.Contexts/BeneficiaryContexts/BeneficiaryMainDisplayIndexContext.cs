using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
    public class BeneficiaryMainDisplayIndexContext
    {
        private readonly IBeneficiaryMainDisplayIndexGateway dataGateway;

        public BeneficiaryMainDisplayIndexContext(IBeneficiaryMainDisplayIndexGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiariesMainDisplayIndexResponse Execute(BeneficiariesMainDisplayIndexRequest request)
        {
            var beneficiaries = dataGateway.GetListOfBeneficiaries();
            beneficiaries = GetBeneficiariesAfterFilters(beneficiaries, request.FilterData);
            var beneficiariesAfterFiltering = beneficiaries.Count();
            var stringOfIDs = GetStringOfIds(beneficiaries);
            beneficiaries = GetBeneficiariesAfterPaging(beneficiaries, request.PagingData);

            return new BeneficiariesMainDisplayIndexResponse(beneficiaries, request.FilterData, request.PagingData, beneficiariesAfterFiltering, stringOfIDs);
        }

        private string GetStringOfIds(List<Beneficiary> beneficiaries)
        {
            var stringOfIDs = "beneficiaryCSV";
            foreach (Beneficiary b in beneficiaries)
            {
                stringOfIDs = stringOfIDs + "," + b.Id;
            }
            return stringOfIDs;
        }

        private List<Beneficiary> GetBeneficiariesAfterFilters(List<Beneficiary> beneficiaries, FilterData filterData)
        {
            if (filterData.BeneficiaryName != null)
                beneficiaries = beneficiaries.Where(x => x.Fullname.Contains(filterData.BeneficiaryName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingDriver != null)
                beneficiaries = beneficiaries.Where(x => x.HomeDeliveryDriver.Contains(filterData.SearchingDriver, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingAddress != null)
                beneficiaries = beneficiaries.Where(x => x.Address.Contains(filterData.SearchingAddress, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingPO != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Occupation.Contains(filterData.SearchingPO, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.Profession.Contains(filterData.SearchingPO, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingComments != null)
                beneficiaries = beneficiaries.Where(x => x.Comments.Contains(filterData.SearchingComments, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingBirthPlace != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingBirthPlace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingStudies != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingStudies, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingSeniority != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingSeniority, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingHealthState != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthState.Contains(filterData.SearchingHealthState, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.Disability.Contains(filterData.SearchingHealthState, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.ChronicCondition.Contains(filterData.SearchingHealthState, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingAddictions != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingAddictions, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingMarried != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Married.Contains(filterData.SearchingMarried, StringComparison.InvariantCultureIgnoreCase) || x.PersonalInfo.SpouseName.Contains(filterData.SearchingMarried, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingIncome != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingIncome.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingHousingType != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingHousingType, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingExpenses != null)
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.BirthPlace.Contains(filterData.SearchingExpenses.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (filterData.SearchingNumberOfPortions != 0)
            {
                beneficiaries = beneficiaries.Where(x => x.NumberOfPortions.Equals(filterData.SearchingNumberOfPortions)).ToList();
            }

            if (filterData.Homeless == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HasHome == false).ToList();
            }

            if (filterData.Active == true)
            {
                beneficiaries = beneficiaries.Where(x => x.Active == true).ToList();
            }

            if (filterData.WeeklyPackage == true)
            {
                beneficiaries = beneficiaries.Where(x => x.WeeklyPackage == true).ToList();
            }

            if (filterData.Canteen == true)
            {
                beneficiaries = beneficiaries.Where(x => x.Canteen == true).ToList();
            }

            if (filterData.HomeDelivery == true)
            {
                beneficiaries = beneficiaries.Where(x => x.HomeDelivery == true).ToList();
            }

            if (filterData.HasGDPRAgreement == true)
            {
                beneficiaries = beneficiaries.Where(x => x.HasGDPRAgreement == true).ToList();
            }

            if (filterData.HasID == true)
            {
                beneficiaries = beneficiaries.Where(x => x.CI.HasId == true).ToList();
            }

            if (filterData.SearchingHealthInsurance == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthInsurance == true).ToList();
            }

            if (filterData.SearchingHealthCard == true)
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.HealthCard == true).ToList();
            }

            if (filterData.Gender != " All")
            {
                if (filterData.Gender == "Male")
                {
                    beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Gender.Equals(Gender.Male)).ToList();
                }
                if (filterData.Gender == "Female")
                { beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Gender.Equals(Gender.Female)).ToList(); }
            }

            if (DateInputReceived(filterData.LowerDate))
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Birthdate > filterData.LowerDate).ToList();
            }
            if (DateInputReceived(filterData.UpperDate))
            {
                beneficiaries = beneficiaries.Where(x => x.PersonalInfo.Birthdate <= filterData.UpperDate).ToList();
            }

            if (DateInputReceived(filterData.ActiveTill))
            {
                beneficiaries = beneficiaries.Where(x => x.LastTimeActive > filterData.ActiveTill).ToList();
            }

            switch (filterData.SortOrder.SortOrder)
            {
                case "Gender":
                    beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Gender).ToList();
                    break;

                case "Gender_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Gender).ToList();
                    break;

                case "Fullname":
                    beneficiaries = beneficiaries.OrderBy(s => s.Fullname).ToList();
                    break;

                case "Active":
                    beneficiaries = beneficiaries.OrderBy(s => s.Active).ToList();
                    break;

                case "Active_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Active).ToList();
                    break;

                case "name_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.Fullname).ToList();
                    break;

                case "Date":
                    beneficiaries = beneficiaries.OrderBy(s => s.PersonalInfo.Birthdate).ToList();
                    break;

                case "date_desc":
                    beneficiaries = beneficiaries.OrderByDescending(s => s.PersonalInfo.Birthdate).ToList();
                    break;

                default:
                    beneficiaries = beneficiaries.OrderBy(s => s.Fullname).ToList();
                    break;
            }
            return beneficiaries;
        }

        private List<Beneficiary> GetBeneficiariesAfterPaging(List<Beneficiary> beneficiaries, PagingData pagingData)
        {
            beneficiaries = beneficiaries.AsQueryable().Skip((pagingData.CurrentPage - 1) * pagingData.NrOfDocumentsPerPage).ToList();
            beneficiaries = beneficiaries.AsQueryable().Take(pagingData.NrOfDocumentsPerPage).ToList();
            return beneficiaries;
        }

        private static bool DateInputReceived(DateTime date)
        {
            var comparisonDate = new DateTime(0003, 1, 1);
            return date > comparisonDate;
        }
    }

    public class BeneficiariesMainDisplayIndexRequest
    {
        public FilterData FilterData { get; set; }
        public Sort SortOrder { get; set; }
        public PagingData PagingData { get; set; }

        public BeneficiariesMainDisplayIndexRequest(string searching, int page, int nrOfDocs, string sortOrder, bool active, string searchingBirthPlace, bool hasContract, bool homeless, DateTime lowerDate, DateTime upperDate, DateTime activeSince, DateTime activeTill, bool weeklyPackage, bool canteen, bool homeDelivery, string searchingDriver, bool hasGDPRAgreement, string searchingAddress, bool hasID, int searchingNumberOfPortions, string searchingComments, string searchingStudies, string searchingPO, string searchingSeniority, string searchingHealthState, string searchingAddictions, string searchingMarried, bool searchingHealthInsurance, bool searchingHealthCard, bool searchingHasHome, string searchingHousingType, string searchingIncome, string searchingExpenses, string gender)
        {
            var filterData = new FilterData();
            var pagingData = new PagingData();
            filterData.BeneficiaryName = searching ?? null;
            filterData.Active = active;
            filterData.LowerDate = lowerDate;
            filterData.UpperDate = upperDate;
            filterData.SearchingBirthPlace = searchingBirthPlace ?? null;
            filterData.HasContract = hasContract;
            filterData.Homeless = homeless;
            filterData.ActiveSince = activeSince;
            filterData.ActiveTill = activeTill;
            filterData.WeeklyPackage = weeklyPackage;
            filterData.Canteen = canteen;
            filterData.HomeDelivery = homeDelivery;
            filterData.SearchingDriver = searchingDriver ?? null;
            filterData.HasGDPRAgreement = hasGDPRAgreement;
            filterData.SearchingAddress = searchingAddress ?? null;
            filterData.HasID = hasID;
            if (searchingNumberOfPortions != 0)
                filterData.SearchingNumberOfPortions = searchingNumberOfPortions;
            filterData.SearchingComments = searchingComments ?? null;
            filterData.SearchingStudies = searchingStudies ?? null;
            filterData.SearchingPO = searchingPO ?? null;
            filterData.SearchingSeniority = searchingSeniority ?? null;
            filterData.SearchingHealthState = searchingHealthState ?? null;
            filterData.SearchingAddictions = searchingAddictions ?? null;
            filterData.SearchingMarried = searchingMarried ?? null;
            filterData.SearchingHealthInsurance = searchingHealthInsurance;
            filterData.SearchingHealthCard = searchingHealthCard;
            filterData.SearchingHasHome = searchingHasHome;
            filterData.SearchingHousingType = searchingHousingType ?? null;
            filterData.SearchingIncome = searchingIncome ?? null;
            filterData.SearchingExpenses = searchingExpenses ??null;
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

    public class BeneficiariesMainDisplayIndexResponse
    {
        public List<Beneficiary> Beneficiaries { get; set; }

        public FilterData FilterData { get; set; }

        public PagingData PagingData { get; set; }

        public int TotalBeneficiaries { get; set; }

        public string StringOfIDs { get; set; }

        public BeneficiariesMainDisplayIndexResponse(List<Beneficiary> beneficiaries, FilterData filterData, PagingData pagingData, int beneficiariesAfterFiltering, string stringOfIDs)
        {
            Beneficiaries = beneficiaries;
            FilterData = filterData;
            PagingData = pagingData;

            TotalBeneficiaries = beneficiariesAfterFiltering;
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
        public string BeneficiaryName { get; set; }

        public bool Active { get; set; }
        public string SearchingBirthPlace { get; set; }
        public bool HasContract { get; set; }
        public bool Homeless { get; set; }
        public DateTime LowerDate { get; set; }
        public DateTime UpperDate { get; set; }
        public DateTime ActiveSince { get; set; }
        public DateTime ActiveTill { get; set; }
        public bool WeeklyPackage { get; set; }
        public bool Canteen { get; set; }
        public bool HomeDelivery { get; set; }
        public string SearchingDriver { get; set; }
        public bool HasGDPRAgreement { get; set; }
        public string SearchingAddress { get; set; }
        public bool HasID { get; set; }
        public int SearchingNumberOfPortions { get; set; }
        public string SearchingComments { get; set; }
        public string SearchingStudies { get; set; }
        public string SearchingPO { get; set; }
        public string SearchingSeniority { get; set; }
        public string SearchingHealthState { get; set; }
        public string SearchingAddictions { get; set; }
        public string SearchingMarried { get; set; }
        public bool SearchingHealthInsurance { get; set; }
        public bool SearchingHealthCard { get; set; }
        public bool SearchingHasHome { get; set; }
        public string SearchingHousingType { get; set; }
        public string SearchingIncome { get; set; }
        public string SearchingExpenses { get; set; }
        public string Gender { get; set; }

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