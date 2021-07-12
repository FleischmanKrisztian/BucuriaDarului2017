﻿namespace BucuriaDarului.Core.Gateways.VolunteerGateways
{
    public interface IVolunteerDeleteGateways
    {
        public void UpdateVolunteer(string volunteerId, Volunteer volunteer);

        public Volunteer GetVolunteer(string id);
        public  void Delete(string volunteeerId);
    }
}