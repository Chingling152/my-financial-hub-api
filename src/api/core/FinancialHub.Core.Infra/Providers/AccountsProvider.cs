﻿namespace FinancialHub.Core.Infra.Providers
{
    public class AccountsProvider : IAccountsProvider
    {
        private readonly IMapper mapper;
        private readonly IAccountsRepository repository;

        public AccountsProvider(IMapper mapper, IAccountsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<AccountModel> CreateAsync(AccountModel account)
        {
            var accountEntity = mapper.Map<AccountEntity>(account);

            var createdAccount = await this.repository.CreateAsync(accountEntity);

            return mapper.Map<AccountModel>(createdAccount);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await repository.DeleteAsync(id);
        }

        public async Task<ICollection<AccountModel>> GetAllAsync()
        {
            var accounts = await this.repository.GetAllAsync();

            return mapper.Map<ICollection<AccountModel>>(accounts);
        }

        public async Task<AccountModel?> GetByIdAsync(Guid id)
        {
            var account = await this.repository.GetByIdAsync(id);

            if (account == null)
                return null;

            return mapper.Map<AccountModel>(account);
        }

        public async Task<AccountModel> UpdateAsync(Guid id, AccountModel account)
        {
            var accountEntity = mapper.Map<AccountEntity>(account);
            accountEntity.Id = id;

            var updatedAccount = await this.repository.UpdateAsync(accountEntity);
            return mapper.Map<AccountModel>(updatedAccount);
        }
    }
}
