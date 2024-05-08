﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace FinancialHub.Core.Domain.Tests.Setup
{
    public abstract class FinancialHubFixture
    {
        protected readonly IConfiguration configuration;
        protected readonly IServiceCollection services;
        protected IServiceProvider serviceProvider;

        public IServiceCollection Services => this.services;
        public IConfiguration Configuration => this.configuration;

        protected FinancialHubFixture()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services = new ServiceCollection();
        }

        public T? GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }
    }
}
