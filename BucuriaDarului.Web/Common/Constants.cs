namespace Finalaplication.Common
{
    public class Constants
    {
        public const string SERVER_NAME_LOCAL = "volmongo_server";
        public const string SERVER_PORT_LOCAL = "volmongo_port";
        public const string DATABASE_NAME_LOCAL = "volmongo_databasename";

        public const string SERVER_NAME_COMMON = "volmongo_server2";
        public const string SERVER_PORT_COMMON = "volmongo_port2";
        public const string DATABASE_NAME_COMMON = "volmongo_databasename2";

        public const string CONNECTION_MODE_COMMON = "common";
        public const string CONNECTION_MODE_LOCAL = "local";

        public const string NUMBER_OF_ITEMS_PER_PAGE = "numberofdocuments";
        public const int DEFAULT_NUMBER_OF_ITEMS_PER_PAGE = 15;

        public const string CONNECTION_LANGUAGE = "Language";

        public static string SESSION_KEY_EVENT = "FirstSessionEvent";
        public static string SECONDARY_SESSION_KEY_EVENT = "SecondSessionEvent";
        public static string EVENTSESSION = "eventSession";
        public static string EVENTHEADER = "eventHeader";

        public static string SESSION_KEY_BENEFICIARY = "FirstSessionBeneficiary";
        public static string SECONDARY_SESSION_KEY_BENEFICIARY = "SecondSessionBeneficiary";
        public static string BENEFICIARYSESSION = "beneficiarySession";
        public static string BENEFICIARYHEADER = "beneficiaryHeader";

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