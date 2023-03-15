﻿using Moq;
using System;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Models;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {
        private Random random;
        private CategoryModelBuilder categoryModelBuilder;

        private CategoriesController controller;
        private Mock<ICategoriesService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.categoryModelBuilder = new CategoryModelBuilder();

            this.mockService = new Mock<ICategoriesService>();
            this.controller = new CategoriesController(this.mockService.Object);
        }
    }
}