﻿namespace FinancialHub.Core.Common.Responses.Errors
{
    public abstract class BaseErrorResponse
    {
        public int Code { get; protected set; }
        public string Message { get; protected set; }

        public BaseErrorResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
