﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using NUnit.Framework;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Entities;
using FinancialHub.Services.Mappers;
using FinancialHub.Services.Services;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Entities;
using FinancialHub.Domain.Tests.Builders.Models;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class BalancesServiceTests
    {
        protected Random random;
        protected BalanceEntityBuilder balanceEntityBuilder;
        protected BalanceModelBuilder balanceModelBuilder;

        private IBalancesService service;

        private IMapper mapper;
        private Mock<IMapperWrapper> mapperWrapper;
        private Mock<IBalancesRepository> repository;
        private Mock<IAccountsRepository> accountsRepository;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAutoMapperProfile());
            }
            ).CreateMapper();

            this.mapperWrapper = new Mock<IMapperWrapper>();
        }

        [SetUp]
        public void Setup()
        {
            this.MockMapper();

            this.repository         = new Mock<IBalancesRepository>();
            this.accountsRepository = new Mock<IAccountsRepository>();
            this.service            = new BalancesService(
                mapperWrapper.Object, 
                repository.Object, 
                accountsRepository.Object
            );

            this.random = new Random();

            this.balanceEntityBuilder = new BalanceEntityBuilder();
            this.balanceModelBuilder = new BalanceModelBuilder();
        }

        private void SetUpMapper()
        {
            this.mapperWrapper
                .Setup(x => x.Map<BalanceModel>(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>((ent) => this.mapper.Map<BalanceModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<BalanceEntity>(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>((model) => this.mapper.Map<BalanceEntity>(model))
                .Verifiable();
        }

        public ICollection<BalanceEntity> GenerateBalances()
        {
            return this.balanceEntityBuilder.Generate(random.Next(5, 10));
        }
    }
}
