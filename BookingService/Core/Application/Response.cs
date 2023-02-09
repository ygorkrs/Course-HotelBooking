﻿namespace Application.Responses
{
    public enum ErrorCode
    {
        NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        INVALID_DOCUMENT_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_EMAIL = 5,
        GUEST_NOT_FOUND = 6,
    }

    public abstract class Response
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }
}
