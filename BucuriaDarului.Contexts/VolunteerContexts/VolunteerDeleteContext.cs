using BucuriaDarului.Core;
using BucuriaDarului.Core.Gateways.VolunteerGateways;
using System.Collections.Generic;
using System.Linq;

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
                var events = dataGateway.GetEvents();
                var listOfEventsToModify = new List<Event>();

                if (events != null && events.Count != 0)
                {
                    foreach (var e in events)
                    {
                        if (e.AllocatedVolunteersId != null)
                            if (e.AllocatedVolunteersId.Contains(id))
                                listOfEventsToModify.Add(e);
                    }
                }
                if (listOfEventsToModify.Count() != 0)
                {
                    foreach (var e in listOfEventsToModify)
                    {
                        var eventToUpdate = new Event();
                        eventToUpdate = e;
                        eventToUpdate.AllocatedVolunteersId = eventToUpdate.AllocatedVolunteersId.Replace(" + " + id, "");
                        eventToUpdate.AllocatedVolunteersId = eventToUpdate.AllocatedVolunteersId.Replace(id, "");
                        var volunteer = dataGateway.GetVolunteer(id);
                        eventToUpdate.AllocatedVolunteers = eventToUpdate.AllocatedVolunteers.Replace(" + " + volunteer.Fullname, "");
                        eventToUpdate.AllocatedVolunteers = eventToUpdate.AllocatedVolunteers.Replace(volunteer.Fullname, "");
                        eventToUpdate.NumberAllocatedVolunteers = AllocatedVolunteerCounter(eventToUpdate.AllocatedVolunteers);

                        dataGateway.UpdateEvent(e.Id, eventToUpdate);
                    }
                }
                dataGateway.Delete(id);
                dataGateway.DeleteVolunteerContracts(id);
            }
            else
            {
                Volunteer volunteer = dataGateway.GetVolunteer(id);
                volunteer.InActivity = false;
                var volunteerToUpdate = volunteer;
                dataGateway.UpdateVolunteer(id, volunteerToUpdate);
            }
        }

        private int AllocatedVolunteerCounter(string AllocatedVolunteersName)
        {
            if (AllocatedVolunteersName != null)
            {
                var split = AllocatedVolunteersName.Split(" + ");
                return split.Count() - 1;
            }
            return 0;
        }
    }
}