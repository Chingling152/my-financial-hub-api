using System;
using System.Net;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using FinancialHub.Domain.Models;
using FinancialHub.IntegrationTests.Base;
using FinancialHub.IntegrationTests.Setup;
using FinancialHub.IntegrationTests.Extensions;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Tests.Builders.Models;
using FinancialHub.Domain.Tests.Builders.Entities;

namespace FinancialHub.IntegrationTests
{
    public class AccountsControllerTests : BaseControllerTests
    {
        private AccountEntityBuilder entityBuilder;
        private AccountModelBuilder modelBuilder;
        private BalanceEntityBuilder balanceBuilder;

        public AccountsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/accounts")
        {

        }

        public override void SetUp()
        {
            this.modelBuilder   = new AccountModelBuilder(); 
            this.entityBuilder  = new AccountEntityBuilder();
            this.balanceBuilder = new BalanceEntityBuilder();
            base.SetUp();
        }

        protected static void AssertEqual(AccountModel expected, AccountModel result)
        {
            Assert.AreEqual(expected.Name,          result.Name);
            Assert.AreEqual(expected.Description,   result.Description);
            Assert.AreEqual(expected.IsActive,      result.IsActive);
        }

        protected async Task AssertGetExists(AccountModel expected)
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<AccountModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            AssertEqual(expected, getResult!.Data.First());
        }

        protected void Populate(int amount = 10)
        {
            var account = this.entityBuilder.Generate();
            var data = this.balanceBuilder.WithAccountId(account.Id).Generate(amount);
            this.fixture.AddData(account);
            this.fixture.AddData(data.ToArray());
        }

        [Test]
        public async Task GetAccountsBalances_ReturnBalances()
        {
            var expectedAmount = this.random.Next(1, 10);
            this.Populate(this.random.Next(1, 10));

            var account = this.entityBuilder.Generate();
            var data = this.balanceBuilder.WithAccountId(account.Id).Generate(expectedAmount);
            this.fixture.AddData(account);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync($"{baseEndpoint}/{account.Id}/balances");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<BalanceModel>>();
            Assert.AreEqual(expectedAmount, result?.Data.Count);
        }

        [Test]
        public async Task GetAll_ReturnAccounts()
        {
            var data = entityBuilder.Generate(10);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<AccountModel>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task Post_ValidAccount_ReturnCreatedAccount()
        {
            var data = this.modelBuilder.Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountModel>>();
            Assert.IsNotNull(result?.Data);
            AssertEqual(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidAccount_CreateAccount()
        {
            var body = this.modelBuilder.Generate();

            await this.client.PostAsync(baseEndpoint, body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_ExistingAccount_ReturnUpdatedAccount()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            AssertEqual(body,result!.Data);
        }

        [Test]
        public async Task Put_ExistingAccount_UpdatesAccount()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();
            await this.client.PutAsync($"{baseEndpoint}/{id}", body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingAccount_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found account with id {id}", result!.Message);
        }

        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesAccountFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var getResponse = await this.client.GetAsync(baseEndpoint);
            var getResult = await getResponse.ReadContentAsync<ListResponse<AccountModel>>();
            Assert.IsEmpty(getResult!.Data);
        }
    }
}