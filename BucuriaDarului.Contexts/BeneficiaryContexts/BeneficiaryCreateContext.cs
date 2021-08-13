using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.BeneficiaryGateways;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;

namespace BucuriaDarului.Contexts.BeneficiaryContexts
{
    public class BeneficiaryCreateContext
    {
        private readonly IBeneficiaryCreateGateway dataGateway;
        private BeneficiaryCreateResponse response = new BeneficiaryCreateResponse("", true);
        public BeneficiaryCreateContext(IBeneficiaryCreateGateway dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public BeneficiaryCreateResponse Execute(BeneficiaryCreateRequest request, byte[] image)
        {
            
            var noNullRequest = ChangeNullValues(request);
            
            if (ContainsSpecialChar(noNullRequest))
            {
                response.IsValid = false;
                response.Message = "The Object Cannot contain Semi-Colons!";
            }

            var beneficiary = ValidateRequest(noNullRequest,image);

            if (response.IsValid)
            {
                dataGateway.Insert(beneficiary);
            }
            return response;
        }

        private Beneficiary ValidateRequest(BeneficiaryCreateRequest request, byte[] image)
        {
            if (request.Fullname == "")
            {
                response.Message += "The Beneficiary must have a name!";
                response.IsValid = false;
            }
            request.CI.ExpirationDate = request.CI.ExpirationDate.AddHours(5);
            request.PersonalInfo.Birthdate = request.PersonalInfo.Birthdate.AddHours(5);
            var validatedBeneficiary = new Beneficiary
            {
                Id = Guid.NewGuid().ToString(),
                Fullname = request.Fullname,
                Active = request.Active,
                WeeklyPackage = request.WeeklyPackage,
                Canteen = request.Canteen,
                HomeDelivery = request.HomeDelivery,
                HomeDeliveryDriver = request.HomeDeliveryDriver,
                HasGDPRAgreement = request.HasGDPRAgreement,
                Address = request.Address,
                CNP = request.CNP,
                CI = request.CI,
                Marca = request.Marca,
                NumberOfPortions = request.NumberOfPortions,
                LastTimeActive = request.LastTimeActive.AddHours(5),
                Comments = request.Comments,
                PersonalInfo = request.PersonalInfo,
            };

            if (image.Length>2)
            {
                validatedBeneficiary.Image = image;
            }

            return validatedBeneficiary;
        }

        private static bool ContainsSpecialChar(object beneficiary)
        {
            var beneficiaryString = JsonConvert.SerializeObject(beneficiary);
            var containsSpecialChar = beneficiaryString.Contains(";");
            return containsSpecialChar;
        }

        private static BeneficiaryCreateRequest ChangeNullValues(BeneficiaryCreateRequest request)
        {
            foreach (var property in request.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                var value = property.GetValue(request, null);
                if (propertyType == typeof(string) && value == null)
                {
                    property.SetValue(request, string.Empty);
                }
            }
           
            foreach (var property in request.PersonalInfo.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType != typeof(DateTime))
                {
                    var value = property.GetValue(request.PersonalInfo, null);
                    if (propertyType == typeof(string) && value == null)
                    {
                        property.SetValue(request.PersonalInfo, string.Empty);
                    }
                }   
            }

            return request;
        }

        
    }

    public class BeneficiaryCreateResponse
    {
        public string Message { get; set; }

        public bool IsValid { get; set; }


        public BeneficiaryCreateResponse(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }

    public class BeneficiaryCreateRequest
    {
        public string Fullname { get; set; }

        public bool Active { get; set; }

        public bool WeeklyPackage { get; set; }

        public bool Canteen { get; set; }

        public bool HomeDelivery { get; set; }

        public string HomeDeliveryDriver { get; set; }

        public bool HasGDPRAgreement { get; set; }

        public string Address { get; set; }

        public string CNP { get; set; }

        public CI CI { get; set; }

        public Marca Marca { get; set; }

        public int NumberOfPortions { get; set; }

        public DateTime LastTimeActive { get; set; }

        public string Comments { get; set; }

        public PersonalInfo PersonalInfo { get; set; }

        public byte[] Image { get; set; }

       
    }

    
}