namespace Finalaplication.Common
{
    public class VolMongoConstants
    {
        public const string SERVER_NAME_MAIN = "volmongo_server";
        public const string SERVER_PORT_MAIN = "volmongo_port";
        public const string DATABASE_NAME_MAIN = "volmongo_databasename";

        public const string SERVER_NAME_SECONDARY = "volmongo_server2";
        public const string SERVER_PORT_SECONDARY = "volmongo_port2";
        public const string DATABASE_NAME_SECONDARY = "volmongo_databasename2";

        public const string CONNECTION_MODE_ONLINE = "online";
        public const string CONNECTION_MODE_OFFLINE = "offline";

        public const string NUMBER_OF_ITEMS_PER_PAGE = "numberofdocuments";
        public const string CONNECTION_ENVIRONMENT = "environment";

        public const int DEFAULT_NUMBER_OF_ITEMS_PER_PAGE = 15;

        public const string CONNECTION_LANGUAGE = "Language";

        public static string SESSION_KEY_EVENT = "FirstSessionEvent";
        public static string SECONDARY_SESSION_KEY_EVENT = "SecondSessionEvent";
        public static string EVENTSESSION = "eventSession";
        public static string EVENTHEADER = "eventHeader";

        public static string SESSION_KEY_VOLUNTEER = "FirstSessionVolunteer";
        public static string SECONDARY_SESSION_KEY_VOLUNTEER = "SecondSessionVolunteer";
        public static string VOLUNTEERSESSION = "volunteerSession";
        public static string VOLUNTEERHEADER = "volunteerHeader";


        public static string SESSION_KEY_SPONSOR = "FirstSessionSponsor";
        public static string SECONDARY_SESSION_KEY_SPONSOR = "SecondSessionSponsor";
        public static string SPONSORSESSION = "sponsorSession";
        public static string SPONSORHEADER = "sponsorHeader";
    }
}