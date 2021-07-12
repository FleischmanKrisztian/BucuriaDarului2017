using BucuriaDarului.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BucuriaDarului.Gateway.SettingsGateways
{
    public class SingleSettingReturnerGateway
    {

        public static Settings GetSettingItem()
        {
            var dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            IMongoCollection<Settings> settingcollection = dbContext.Database.GetCollection<Settings>("Settings");
            Settings setting = settingcollection.AsQueryable<Settings>().SingleOrDefault();

            return setting;
        }
    }
}
