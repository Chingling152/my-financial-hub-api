﻿using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Filters;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Queries;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Mappers;

namespace FinancialHub.Services.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IMapperWrapper mapper;
        private readonly ITransactionsRepository repository;

        public TransactionsService(IMapperWrapper mapper, ITransactionsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<TransactionModel> CreateAsync(TransactionModel category)
        {
            var entity = mapper.Map<TransactionEntity>(category);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<TransactionModel>> GetAllByUserAsync(string userId, TransactionFilter filter)
        {
            var query = mapper.Map<TransactionQuery>(filter);
            //query.UserId = userId;

            var entities = await this.repository.GetAsync(query.Query());

            return mapper.Map<ICollection<TransactionModel>>(entities);
        }

        public async Task<TransactionModel> UpdateAsync(Guid id, TransactionModel category)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NullReferenceException($"Not found category with id {id}");
            }
            entity.Id = id;

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<TransactionModel>(entity);
        }
    }
}
