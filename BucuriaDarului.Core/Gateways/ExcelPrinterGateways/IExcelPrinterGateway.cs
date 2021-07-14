using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.ExcelPrinterGateways
{
    public interface IExcelPrinterGateway
    {
        List<Beneficiary> GetListOfBeneficiaries();
        List<Event> GetListOfEvents();
        List<Volunteer> GetListOfVolunteers();
        List<Sponsor> GetListOfSponsors();
    }
}
