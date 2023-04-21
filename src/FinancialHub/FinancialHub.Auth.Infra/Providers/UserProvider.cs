﻿using AutoMapper;
using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Infra.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UserProvider(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserModel> CreateAsync(UserModel user)
        {
            var entity = this.mapper.Map<UserEntity>(user);

            var createdEntity = await this.repository.CreateAsync(entity);

            return this.mapper.Map<UserModel>(createdEntity);
        }

        public async Task<UserModel> GetAsync(Guid id)
        {
            var user = await this.repository.GetAsync(id);
            return this.mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            var entity = this.mapper.Map<UserEntity>(user);

            var updatedEntity = await this.repository.UpdateAsync(entity);

            return this.mapper.Map<UserModel>(updatedEntity);
        }
    }
}
