namespace BucuriaDarului.Core.Gateways
{
    public interface IEventCreateGateway
    {
        void Insert(Event @event);
    }
}