﻿using Moq;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Models;
using System;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class AccountsControllerTests
    {
        private Random random;

        private AccountModelBuilder accountModelBuilder;

        private AccountsController controller;
        private Mock<IAccountsService> mockService;
        private Mock<IAccountBalanceService> mockAccountService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();

            this.mockService = new Mock<IAccountsService>();
            this.mockAccountService = new Mock<IAccountBalanceService>();
            this.controller = new AccountsController(mockAccountService.Object ,this.mockService.Object);
        }
    }
}
