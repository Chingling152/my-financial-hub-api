﻿using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Tests.Common.Builders.Entities;

namespace FinancialHub.Auth.Services.Tests.Services
{
    public partial class UserServiceTests
    {
        private IUserService service;
        private Mock<IUserProvider> mockProvider;
        private UserModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            this.builder = new UserModelBuilder();

            this.mockProvider = new Mock<IUserProvider>();
            this.service = new UserService(this.mockProvider.Object);
        }

        private static void AssertEqual(UserModel expected, UserModel actual)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(actual.LastName, Is.EqualTo(expected.LastName));
                Assert.That(actual.Email, Is.EqualTo(expected.Email));
                Assert.That(actual.BirthDate, Is.EqualTo(expected.BirthDate));
            });
        }
    }
}
