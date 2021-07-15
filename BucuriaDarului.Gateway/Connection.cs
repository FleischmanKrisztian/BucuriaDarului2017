using System;

namespace BucuriaDarului.Gateway
{
    internal static class Connection
    {
        internal static string SERVER_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_server");
        internal static int SERVER_PORT_LOCAL = int.Parse(Environment.GetEnvironmentVariable("volmongo_port"));
        internal static string DATABASE_NAME_LOCAL = Environment.GetEnvironmentVariable("volmongo_databasename");
    }

    internal static class SecondaryConnection
    {
        internal static string SERVER_NAME_COMMON = Environment.GetEnvironmentVariable("volmongo_server2");
        internal static int SERVER_PORT_COMMON = int.Parse(Environment.GetEnvironmentVariable("volmongo_port2"));
        internal static string DATABASE_NAME_COMMON = Environment.GetEnvironmentVariable("volmongo_databasename2");
    }
}