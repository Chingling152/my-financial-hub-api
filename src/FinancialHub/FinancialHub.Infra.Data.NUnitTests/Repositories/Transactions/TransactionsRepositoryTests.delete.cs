﻿using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class TransactionsRepositoryTests
    {
        [Test]
        public async Task DeleteAsync_ValidItemWithNestChild_DoesNotDeleteNestChild()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Balance);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);
            this.context.ChangeTracker.Clear();

            var result = await this.repository.DeleteAsync(entity.Id.Value);

            Assert.AreEqual(1,result);

            Assert.IsEmpty(this.context.Transactions.Local);
            Assert.AreEqual(1, this.context.Accounts.Local.Count);
            Assert.AreEqual(1, this.context.Categories.Local.Count);
        }
    }
}
