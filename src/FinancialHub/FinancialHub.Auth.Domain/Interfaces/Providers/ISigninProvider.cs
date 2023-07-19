﻿using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Providers
{
    public interface ISigninProvider
    {
        Task<UserModel?> GetAccountAsync(SigninModel signin);
    }
}
