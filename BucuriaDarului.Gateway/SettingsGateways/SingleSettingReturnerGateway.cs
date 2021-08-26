﻿using BucuriaDarului.Core;
using MongoDB.Driver;
using System;

namespace BucuriaDarului.Gateway.SettingsGateways
{
    public class SingleSettingReturnerGateway
    {
        public static Settings GetSettingItem()
        {
            var dbContext = new MongoDBGateway();

            dbContext.ConnectToDB(Connection.SERVER_NAME_LOCAL, Connection.SERVER_PORT_LOCAL, Connection.DATABASE_NAME_LOCAL);
            var settingCollection = dbContext.Database.GetCollection<Settings>("Settings");
            var setting = settingCollection.AsQueryable().SingleOrDefault();
            if (setting == null)
            {
                var newSetting = new Settings
                {
                    Id = Guid.NewGuid().ToString(),
                    Lang = "ro",
                    Quantity = 15,
                    NumberOfDaysBeforBirthday = 10,
                    NumberOfDaysBeforeExpiration = 30
                };
                settingCollection.InsertOne(newSetting);
                return newSetting;
            }
            return setting;
        }
    }
}