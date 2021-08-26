using BucuriaDarului.Core;
using System;

namespace BucuriaDarului.Gateway
{
    internal static class Connection
    {
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.SERVER_NAME_LOCAL);
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable(Constants.SERVER_PORT_LOCAL));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable(Constants.DATABASE_NAME_LOCAL);
    }

    internal static class SecondaryConnection
    {
        internal static string SERVER_NAME_COMMON = Environment.GetEnvironmentVariable(Constants.SERVER_NAME_COMMON);
        internal static int SERVER_PORT_COMMON = int.Parse(Environment.GetEnvironmentVariable(Constants.SERVER_PORT_COMMON));
        internal static string DATABASE_NAME_COMMON = Environment.GetEnvironmentVariable(Constants.DATABASE_NAME_COMMON);
    }
}