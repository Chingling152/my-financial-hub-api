﻿using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class TransactionsRepositoryTests
    {
        protected override void AssertCreated(TransactionEntity createdItem)
        {
            base.AssertCreated(createdItem);

            Assert.IsNotNull(createdItem.BalanceId);
            Assert.IsNotNull(createdItem.Category);
            Assert.AreEqual(createdItem.BalanceId, createdItem.Balance.Id);
            Assert.AreEqual(createdItem.CategoryId, createdItem.Category.Id);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with valid Category and Balance", Category = "Create")]
        public override async Task CreateAsync_ValidItem_AddsOneRow(TransactionEntity item = null)
        {
            var entity = this.GenerateObject();

            entity.Category = await this.InsertData(entity.Category);
            entity.Balance = await this.InsertData(entity.Balance);

            await base.CreateAsync_ValidItem_AddsOneRow(entity);
        }

        [Test]
        [TestCase(TestName = "Create new Item with id", Category = "Create")]
        public async override Task CreateAsync_ValidItemWithId_AddsOneRowWithTheDifferentId(TransactionEntity item = null)
        {
            var entity = this.GenerateObject();

            var generatedBalance = this.GenerateBalance(entity.BalanceId);
            var generatedCategory = this.GenerateCategory(entity.CategoryId);

            await this.InsertData(generatedBalance);
            await this.InsertData(generatedCategory);

            await base.CreateAsync_ValidItemWithId_AddsOneRowWithTheDifferentId(entity);
        }

        [Test]
        [TestCase(TestName = "Create item with existing id", Category = "Create")]
        public override async Task CreateAsync_ValidItemWithExistingId_AddsOneRowWithTheDifferentId(TransactionEntity item = null)
        {
            var id = Guid.NewGuid();
            item = this.GenerateObject(id);

            //INSERTS BALANCE AND CATEGORY
            var generatedBalance = this.GenerateBalance(item.BalanceId);
            var generatedCategory = this.GenerateCategory(item.CategoryId);

            await this.InsertData(generatedBalance);
            await this.InsertData(generatedCategory);

            item.Category = null;
            item.Balance = null;
            item = await this.InsertData(item);

            var newItem = this.GenerateObject(id);
            newItem.CategoryId  = generatedCategory.Id.GetValueOrDefault();
            newItem.BalanceId   = generatedBalance.Id.GetValueOrDefault();

            var result = await this.repository.CreateAsync(newItem);

            Assert.AreNotEqual(item.Id,newItem.Id);
            this.AssertCreated(result);
            Assert.AreEqual(2, context.Set<TransactionEntity>().Count());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction without Updates/Creates Balance or Category", Category = "Create")]
        public async Task CreateAsync_ValidItemWithNestChild_DoesNotUpdateNestChild()
        {
            #warning this test is too complex

            /***** ARRANGE *****/
            var entity = this.GenerateObject();

            //INSERTS ACCOUNT AND CATEGORY
            var oldBalance = this.GenerateBalance(entity.BalanceId);
            var oldCategory = this.GenerateCategory(entity.CategoryId);

            await this.InsertData(oldBalance);
            await this.InsertData(oldCategory);

            //CHANGES TRANSACTION'S CATEGORY AND BALANCE
            entity.Balance.Name = Guid.NewGuid().ToString();
            entity.Category.Name = Guid.NewGuid().ToString();

            /***** ACT *****/

            var result = await this.repository.CreateAsync(entity);

            /***** ASSERT *****/

            this.AssertCreated(result);

            //SHOULD NOT CREATE ACCOUNTS OR CATEGORIES
            Assert.AreEqual(1,this.context.Balances.Count());
            Assert.AreEqual(1,this.context.Categories.Count());

            var account = this.context.Balances.FirstOrDefault(x => x.Id == entity.BalanceId);
            var category = this.context.Categories.FirstOrDefault(x => x.Id == entity.CategoryId);
            
            //SHOULD NOT UPDATE DATABASE
            Assert.AreEqual(oldBalance, account);
            Assert.AreEqual(oldCategory, category);

            //SHOULD NOT RETURN THE WRONG ITEM
            Assert.AreEqual(oldBalance, result.Balance);
            Assert.AreEqual(oldCategory, result.Category);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with invalid Balance", Category = "Create")]
        public async Task CreateAsync_InvalidBalanceId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(this.GenerateBalance());

            entity.Category = await this.InsertData(entity.Category);
            entity.CategoryId = entity.Category.Id.GetValueOrDefault();

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions);
            Assert.AreEqual(1, this.context.Balances.Count());
            Assert.AreEqual(1, this.context.Categories.Count());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with invalid Category", Category = "Create")]
        public async Task CreateAsync_InvalidCategoryId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Balance);
            await this.InsertData(this.GenerateCategory());

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions);
            Assert.AreEqual(1, this.context.Balances.Count());
            Assert.AreEqual(1, this.context.Categories.Count());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with no Balance", Category = "Create")]
        public async Task CreateAsync_NoBalance_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Category);

            entity.Balance = null;

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions.ToList());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with no Category", Category = "Create")]
        public async Task CreateAsync_NoCategory_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Balance);

            entity.Category = null;

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions.ToList());
        }
    }
}
