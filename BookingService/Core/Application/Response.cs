namespace Application.Responses
{
    public enum ErrorCode
    {
        #region Guest
        NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        INVALID_DOCUMENT_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_EMAIL = 5,
        GUEST_NOT_FOUND = 6,
        #endregion

        #region Booking
        NOT_FOUND_BOOKING = 201,
        COULD_NOT_STORE_BOOKING = 202,
        MISSING_BK_PLACEAT_INFORMATION = 203,
        MISSING_BK_START_INFORMATION = 204,
        MISSING_BK_END_INFORMATION = 205,
        MISSING_BK_ROOM_INFORMATION = 206,
        MISSING_BK_GUEST_INFORMATION = 207,
        #endregion
    }

    public abstract class Response
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
}
