using UnityEngine;
using System.Collections;
namespace google.service.game
{

    public class GameConst
    {

        public static readonly int COLLECTION_PUBLIC =0;
        public static readonly int COLLECTION_SOCIAL = 1;

        public static readonly int TIME_SPAN_DAILY = 0;
        public static readonly int TIME_SPAN_WEEKLY = 1;
        public static readonly int TIME_SPAN_ALL_TIME = 2;


        public static readonly int SELECT_UPCOMING = 1;
        public static readonly int SELECT_OPEN = 2;
        public static readonly int SELECT_ACCEPTED = 3;
        public static readonly int SELECT_COMPLETED = 4;
        public static readonly int SELECT_COMPLETED_UNCLAIMED = 101;
        public static readonly int SELECT_EXPIRED = 5;
        public static readonly int SELECT_ENDING_SOON = 102;
        public static readonly int SELECT_FAILED = 6;
        public static readonly int SELECT_RECENTLY_FAILED = 103;
        public static readonly int[] SELECT_ALL_QUESTS = { 1, 2, 3, 4, 101, 5, 102, 6, 103 };

        public static readonly int SORT_ORDER_RECENTLY_UPDATED_FIRST = 0;
        public static readonly int SORT_ORDER_ENDING_SOON_FIRST = 1;


         public static readonly int RESOLUTION_POLICY_MANUAL = -1;
          public static readonly int RESOLUTION_POLICY_LONGEST_PLAYTIME = 1;
          public static readonly int RESOLUTION_POLICY_LAST_KNOWN_GOOD = 2;
          public static readonly int RESOLUTION_POLICY_MOST_RECENTLY_MODIFIED = 3;
          public static readonly int RESOLUTION_POLICY_HIGHEST_PROGRESS = 4;


        public static readonly int STATUS_NOT_INVITED_YET = 0;
        public static readonly int STATUS_INVITED = 1;
        public static readonly int STATUS_JOINED = 2;
        public static readonly int STATUS_DECLINED = 3;
        public static readonly int STATUS_LEFT = 4;
        public static readonly int STATUS_FINISHED = 5;
        public static readonly int STATUS_UNRESPONSIVE = 6;


        public static readonly int RESULT_RECONNECT_REQUIRED = 10001;
        public static readonly int RESULT_SIGN_IN_FAILED = 10002;
        public static readonly int RESULT_LICENSE_FAILED = 10003;
        public static readonly int RESULT_APP_MISCONFIGURED = 10004;
        public static readonly int RESULT_LEFT_ROOM = 10005;
        public static readonly int RESULT_NETWORK_FAILURE = 10006;
        public static readonly int RESULT_SEND_REQUEST_FAILED = 10007;
        public static readonly int RESULT_INVALID_ROOM = 10008;

        public static readonly int SORT_ORDER_MOST_RECENT_FIRST = 0;
        public static readonly int SORT_ORDER_SOCIAL_AGGREGATION = 1;
        
         /** Standard activity result: operation canceled. */
    public static readonly int RESULT_CANCELED    = 0;
    /** Standard activity result: operation succeeded. */
    public static readonly int RESULT_OK           = -1;

    }
}
