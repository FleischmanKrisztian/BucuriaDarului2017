using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventVolunteerAllocationDisplayGateway
    {
        Event GetEvent(string eventId);
        List<Volunteer> GetListOfVolunteers();
       

    }
}
