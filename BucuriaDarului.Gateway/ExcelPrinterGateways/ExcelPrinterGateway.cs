using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.ExcelPrinterGateways;
using BucuriaDarului.Gateway.BeneficiaryGateways;
using BucuriaDarului.Gateway.EventGateways;
using BucuriaDarului.Gateway.SponsorGateways;
using BucuriaDarului.Gateway.VolunteerGateways;
using System.Collections.Generic;

namespace BucuriaDarului.Gateway.ExcelPrinterGateways
{
    public class ExcelPrinterGateway : IExcelPrinterGateway
    {
        public List<Beneficiary> GetListOfBeneficiaries()
        {
            return ListBeneficiariesGateway.GetListOfBeneficiaries();
        }

        public List<Event> GetListOfEvents()
        {
            return ListEventsGateway.GetListOfEvents();
        }

        public List<Sponsor> GetListOfSponsors()
        {
            return ListSponsorsGateway.GetListOfSponsors();
        }

        public List<Volunteer> GetListOfVolunteers()
        {
            return ListVolunteersGateway.GetListOfVolunteers();
        }
    }
}