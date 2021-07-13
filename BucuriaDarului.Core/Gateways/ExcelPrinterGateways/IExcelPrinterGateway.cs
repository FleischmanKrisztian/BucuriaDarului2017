using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways.ExcelPrinterGateways
{
    public interface IExcelPrinterGateway
    {
        List<Beneficiary> GetListOfBeneficiary();
        List<Event> GetListOfEvents();
        List<Volunteer> GetListOfVolunteerss();
        List<Sponsor> GetListOfSponsors();
    }
}
