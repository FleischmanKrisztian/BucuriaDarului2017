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

        public void Execute(bool inactive, string id, Volunteer volunteerToUpdate)
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
                volunteerToUpdate=volunteer;
                dataGateway.UpdateVolunteer(id, volunteerToUpdate);
               
            }
        }
    }

    public class DeleteRequest
    {
        public Volunteer Volunteer { get; set; }

        public string Id { get; set; }
        public bool Inactive { get; set; }

        public DeleteRequest(Volunteer volunteer,  bool inactive)
        {
            Volunteer = volunteer;
            Id = volunteer.Id;
            Inactive = true;
        }
    }
}
