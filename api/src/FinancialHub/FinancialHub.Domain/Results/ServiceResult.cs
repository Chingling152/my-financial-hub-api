﻿using FinancialHub.Domain.Results.Errors;

namespace FinancialHub.Domain.Results
{
    public class ServiceResult<T>
    {
        public bool HasError => this.Error != null;
        public ServiceError Error { get; protected set; }//TODO: change to a collection
        public T Data { get; protected set; }

        public ServiceResult(T data = default,ServiceError error = null)
        {
            this.Data = data;
            this.Error = error;
        }

        public static implicit operator ServiceResult<T>(T result)
        {
            return new ServiceResult<T>(result);
        }

        public static implicit operator ServiceResult<T>(ServiceError error)
        {
            return new ServiceResult<T>(error: error);
        }
    }
}
