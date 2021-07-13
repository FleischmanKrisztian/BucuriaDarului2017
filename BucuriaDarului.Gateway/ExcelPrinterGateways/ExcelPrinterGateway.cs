using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.ExcelPrinterGateways;
using BucuriaDarului.Gateway.SponsorGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.ExcelPrinterGateways
{
    public class ExcelPrinterGateway : IExcelPrinterGateway
    {
        public List<Beneficiary> GetListOfBeneficiary()
        {
            return;
        }

        public List<Event> GetListOfEvents()
        {
           return;
        }

        public List<Sponsor> GetListOfSponsors()
        {
           return ListSponsorsGateway.GetListOfSponsors();
        }

        public List<Volunteer> GetListOfVolunteerss()
        {
            return;
        }
    }
}
