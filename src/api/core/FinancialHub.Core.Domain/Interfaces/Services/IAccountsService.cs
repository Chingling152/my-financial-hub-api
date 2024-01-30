﻿using FinancialHub.Common.Results;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Models;

namespace FinancialHub.Core.Domain.Interfaces.Services
{
    public interface IAccountsService 
    {
        Task<ServiceResult<ICollection<AccountModel>>> GetAllByUserAsync(string userId);

        Task<ServiceResult<AccountDto>> GetByIdAsync(Guid id);
        [Obsolete("use CreateAsync using a DTO")]
        Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account);

        Task<ServiceResult<AccountDto>> CreateAsync(CreateAccountDto accountDto);

        Task<ServiceResult<AccountModel>> UpdateAsync(Guid id,AccountModel account);

        Task<ServiceResult<int>> DeleteAsync(Guid id);
    }
}
