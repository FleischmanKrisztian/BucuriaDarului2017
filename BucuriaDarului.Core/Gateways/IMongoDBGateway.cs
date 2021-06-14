using MongoDB.Driver;
using System.Collections.Generic;

namespace BucuriaDarului.Core.Gateways
{
    public interface IMongoDBGateway
    {
        void ConnectToDB(string SERVER_NAME, int SERVER_PORT, string DATABASE_NAME);
    }
}