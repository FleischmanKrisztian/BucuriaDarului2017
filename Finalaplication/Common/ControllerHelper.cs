using Finalaplication.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finalaplication.Common
{
    public class ControllerHelper
    {
        public static void setViewBagEnvironment(ITempDataDictionary tempDataDic, dynamic viewBag)
        {
            string message = tempDataDic.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT).ToString();
            viewBag.env = message;
        }

        public static int getNumberOfItemPerPageFromSettings(ITempDataDictionary tempDataDic)
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

        public static string VolunteersToCSVFormat(List<Volunteer> volunteers)
        {
            var allLines = (from Volunteer in volunteers
                            select new object[]
                            {
                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18};",
                            Volunteer.Firstname,
                            Volunteer.Lastname,
                            Volunteer.Birthdate.ToString(),
                            Volunteer.Gender.ToString(),
                            Volunteer.CNP,
                            Volunteer.Occupation,
                            Volunteer.Field_of_activity,
                            Volunteer.Desired_workplace,
                            Volunteer.InActivity.ToString(),
                            Volunteer.HourCount.ToString(),
                            Volunteer.Additionalinfo.HasCar.ToString(),
                            Volunteer.Additionalinfo.HasDrivingLicence.ToString(),
                            Volunteer.Additionalinfo.Remark,
                            Volunteer.Address.District,
                            Volunteer.Address.City,
                            Volunteer.Address.Number,
                            Volunteer.Address.Street,
                            Volunteer.ContactInformation.MailAdress,
                            Volunteer.ContactInformation.PhoneNumber)
                            }
                             ).ToList();

            var csv1 = new StringBuilder();

            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));
            });
            return csv1.ToString();
        }

        public static void ExportvolunteersAsCsv(List<Volunteer> volunteers, string outputFileName)
        {
            string path = outputFileName; // "./Excelfiles/Volunteers.csv";

            // Add the header
            System.IO.File.WriteAllText(path, "Firstname,Lastname,Birthdate,Gender,CNP,Occupation,Filed_of_activity,Desired_workplace,InActivity,HourCount,HasCar,HasDrivingLicence,Remark,District,City,Number,Street,MailAddres,PhoneNumber\n");
            System.IO.File.AppendAllText(path, VolunteersToCSVFormat(volunteers));
        }

        public static void ExportvolunteersAsDefaultCsv(List<Volunteer> volunteers)
        {
            ExportvolunteersAsCsv(volunteers, "./Excelfiles/Volunteers.csv");
        }

        public static void ExportBeneficiaries(List<Beneficiary> beneficiaries)
        {
            string path = "./Excelfiles/Beneficiaries.csv";

            var allLines = (from Beneficiary in beneficiaries
                            select new object[]
                            {
                             string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42};",
                            Beneficiary.Firstname,
                            Beneficiary.Lastname,
                            Beneficiary.Active,
                            Beneficiary.Weeklypackage.ToString(),
                            Beneficiary.Canteen.ToString(),
                            Beneficiary.HomeDeliveryDriver,
                            Beneficiary.HasGDPRAgreement.ToString(),
                            Beneficiary.Adress.District,
                            Beneficiary.Adress.City,
                            Beneficiary.Adress.Street,
                            Beneficiary.Adress.Number,
                            Beneficiary.CNP,
                            Beneficiary.CI.HasId.ToString(),
                            Beneficiary.CI.CIseria,
                            Beneficiary.CI.CINr,
                            Beneficiary.CI.CIEliberat.ToString(),
                            Beneficiary.CI.CIeliberator,
                            Beneficiary.Marca.IdAplication.ToString(),
                            Beneficiary.Marca.IdContract.ToString(),
                            Beneficiary.Marca.IdInvestigation.ToString(),
                            Beneficiary.NumberOfPortions.ToString(),
                            Beneficiary.LastTimeActiv.ToString(),
                            Beneficiary.Coments,
                            Beneficiary.PersonalInfo.Birthdate.ToString(),
                            Beneficiary.PersonalInfo.PhoneNumber,
                            Beneficiary.PersonalInfo.BirthPlace,
                            Beneficiary.PersonalInfo.Studies,
                            Beneficiary.PersonalInfo.Profesion,
                            Beneficiary.PersonalInfo.Ocupation,
                            Beneficiary.PersonalInfo.SeniorityInWorkField,
                            Beneficiary.PersonalInfo.HealthState,
                            Beneficiary.PersonalInfo.Disalility,
                            Beneficiary.PersonalInfo.ChronicCondition,
                            Beneficiary.PersonalInfo.Addictions,
                            Beneficiary.PersonalInfo.HealthInsurance.ToString(),
                            Beneficiary.PersonalInfo.HealthCard.ToString(),
                            Beneficiary.PersonalInfo.Married.ToString(),
                            Beneficiary.PersonalInfo.SpouseName,
                            Beneficiary.PersonalInfo.HasHome.ToString(),
                            Beneficiary.PersonalInfo.HousingType,
                            Beneficiary.PersonalInfo.Income,
                            Beneficiary.PersonalInfo.Expences,
                            Beneficiary.PersonalInfo.Gender)
                            }
                             ).ToList();

            var csv1 = new StringBuilder();

            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));
            }
               );
            System.IO.File.WriteAllText(path, "Firstname,Lastname,Active,Weekly package,Canteen,Home Delivery Driver,HAS GDPR,District,City,Street,Number,CNP,Has ID,IDSerie,IDNr,IDEliberat,IdEliberator,IDAplication,IDInvestigation,IDContract,Number Of Portions,Last Time Active,Comments,Birthdate,Phone Number,Birth place,Studies,Profession,Occupation,Seniority In Workfield,Health State,Disability,Chronic Condition,Addictions,Health Insurance,Health Card,Married,Spouse Name,Has Home,Housing Type,Income,Expenses,Gender,Has Contract,Number Of Registration,Registration Date,Expiration Date\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
        }

        public static void ExportEvents(List<Event> events)
        {
            string path = "./Excelfiles/Events.csv";
            var allLines = (
                            from Event in events
                            select new object[]
                            {
            string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                            Event.NameOfEvent,
                              Event.DateOfEvent.ToString(),
                              Event.Duration.ToString(),
                              Event.NumberOfVolunteersNeeded.ToString(),
                              Event.PlaceOfEvent,
                              Event.TypeOfActivities,
                              Event.TypeOfEvent,
                              Event.AllocatedVolunteers,
                              Event.AllocatedSponsors
                              )
                            }
                             ).ToList();
            var csv1 = new StringBuilder();

            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));
            }
           );
            System.IO.File.WriteAllText(path, "NameOfEvent,DateOfEvent,Duration,NumberOfVolunteersNeeded,PlaceOfEvent,TypeOfActivities,TypeOfEvent,AllocatedVolunteers,AllocatedSponsors\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
        }

        public static void ExportSponsors(List<Sponsor> sponsors)
        {
            string path = "./Excelfiles/Sponsors.csv";

            var allLines = (from Sponsor in sponsors
                            select new object[]
                            {
                                 string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10};",
                                 Sponsor.NameOfSponsor,
                                 Sponsor.ContactInformation.PhoneNumber,
                                 Sponsor.ContactInformation.MailAdress,
                                 Sponsor.Contract.HasContract.ToString(),
                                 Sponsor.Contract.NumberOfRegistration.ToString(),
                                 Sponsor.Contract.RegistrationDate.ToString(),
                                 Sponsor.Contract.ExpirationDate.ToString(),
                                 Sponsor.Sponsorship.Date.ToString(),
                                 Sponsor.Sponsorship.MoneyAmount.ToString(),
                                 Sponsor.Sponsorship.WhatGoods,
                                 Sponsor.Sponsorship.GoodsAmount)
                            }
                             ).ToList();

            var csv1 = new StringBuilder();
            allLines.ForEach(line =>
            {
                csv1 = csv1.AppendLine(string.Join(";", line));
            }
            );
            System.IO.File.WriteAllText(path, "NameOfSponsor,PhoneNumber,MailAdress,HasContract,NumberOfRegistration,RegistrationDate,ExpirationDate,DateOfSponsorships,MoneyAmount,WhatGoods,GoodsAmount\n");
            System.IO.File.AppendAllText(path, csv1.ToString());
        }
    }
}
