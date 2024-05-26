namespace BY.Common
{
    public class Const
    {
        #region Error Codes
        public static int ERROR_EXCEPTION = -4;
        #endregion

        #region Success Codes
        public static int SUCCESS_CREATE_CODE = 1;
        public static string SUCCESS_CREATE_MSG = "Saving data successful!";

        public static int SUCCESS_UPDATE_CODE = 1;
        public static string SUCCESS_UPDATE_MSG = "Update data successful!";

        public static int SUCCESS_DELETE_CODE = 1;
        public static string SUCCESS_DELETE_MSG = "Delete data successful!";

        public static int SUCCESS_READ_CODE = 1;
        public static string SUCCESS_READ_MSG = "Get data successful!";
        #endregion

        #region Warning Codes
        public static int WARNING_NO_DATA_CODE = 4;
        public static string WARNING_NO_DATA_MSG = "No data found!";
        #endregion

        #region Fail Codes
        public static int FAIL_CREATE_CODE = -1;
        public static string FAIL_CREATE_MSG = "Saving data fail!";

        public static int FAIL_UPDATE_CODE = -1;
        public static string FAIL_UPDATE_MSG = "Update data fail!";

        public static int FAIL_DELETE_CODE = -1;
        public static string FAIL_DELETE_MSG = "Delete data fail!";

        public static int FAIL_READ_CODE = -1;
        public static string FAIL_READ_MSG = "Get data fail!";
        #endregion
    }
}
