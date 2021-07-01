namespace BucuriaDarului.Core.Gateways.EventGateways
{
    public interface IEventCreateGateway
    {
        void Insert(Event @event);
    }
}