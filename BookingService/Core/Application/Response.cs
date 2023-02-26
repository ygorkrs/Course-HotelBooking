namespace Application.Responses
{
    public enum ErrorCode
    {
        #region Guest = 0 - 100
        NOT_FOUND_GUEST = 1,
        COULD_NOT_STORE_GUEST = 2,
        INVALID_DOCUMENT_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_EMAIL = 5,
        #endregion

        #region Room = 101 - 200
        NOT_FOUND_ROOM = 101,
        COULD_NOT_STORE_ROOM = 102,
        MISSING_ROOM_REQUIRED_INFORMATION = 103,
        #endregion

        #region Booking = 201 - 500
        NOT_FOUND_BOOKING = 201,
        COULD_NOT_STORE_BOOKING = 202,
        MISSING_BK_PLACEAT_INFORMATION = 203,
        MISSING_BK_START_INFORMATION = 204,
        MISSING_BK_END_INFORMATION = 205,
        MISSING_BK_ROOM_INFORMATION = 206,
        MISSING_BK_GUEST_INFORMATION = 207,
        BK_ROOM_CANNOT_BE_BOOKED = 208,
        #endregion

        #region Payment related = 501 - 1500
        PAYMENT_GENERAL_ERROR = 501,
        PAYMENT_INVALID_PAYMENT_INTENTION = 502,
        #endregion
    }

    public abstract class Response
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
}
