using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Contexts.VolunteerContexts
{
    public class VolunteerDeleteContext
    {
        private readonly IVolunteerDeleteGateways dataGateway;

        public VolunteerDeleteContext(IVolunteerDeleteGateways dataGateway)
        {
            this.dataGateway = dataGateway;
        }

        public void Execute(bool inactive, string id)
        {


            if (inactive == false)
            {
                dataGateway.Delete(id);
                //volcontractManager.DeleteAVolunteersContracts(id);
               
            }
            else
            {
                Volunteer volunteer = dataGateway.GetVolunteer(id);
                volunteer.InActivity = false;
                var volunteerToUpdate=volunteer;
                dataGateway.UpdateVolunteer(id, volunteerToUpdate);
               
            }
        }
    }

    
}
