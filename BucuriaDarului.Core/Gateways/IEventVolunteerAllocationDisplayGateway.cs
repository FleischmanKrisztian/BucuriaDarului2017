﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Core.Gateways
{
    public interface IEventVolunteerAllocationDisplayGateway
    {
        List<Volunteer> GetListOfVolunteers();
    }
}
