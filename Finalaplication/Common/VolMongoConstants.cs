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

        public static string SESSION_KEY = "FirstSessionEvent";
        public static string SECONDARY_SESSION_KEY = "SecondSessionEvent";
        public static string EVENTSESSION = "eventSession";
        public static string EVENTHEADER = "eventHeader";

        public static string SESSION_KEY_BENEFICIARY = "FirstSessionBeneficiary";
        public static string SECONDARY_SESSION_KEY_BENEFICIARY = "SecondSessionBeneficiary";
        public static string BENEFICIARYSESSION = "beneficiarySession";
        public static string BENEFICIARYHEADER = "beneficiaryHeader";
    }
}