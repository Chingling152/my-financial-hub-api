﻿namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        [Test]
        public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
        {
            var item = await this.fixture.InsertData(builder.Generate());

            var newUser = builder.WithId(item.Id.GetValueOrDefault()).Generate();
            var updated = await repository.UpdateAsync(newUser);

            EntityAssert.Equal(newUser, updated);
        }

        [Test]
        public async Task UpdateAsync_ExistingUser_UpdatesUser() 
        {
            var oldUser = await this.fixture.InsertData(builder.Generate());

            var newUser = builder.WithId(oldUser.Id.GetValueOrDefault()).Generate();
            await repository.UpdateAsync(newUser);

            var data = fixture.Context.Users.FirstOrDefault(x => x.Id == oldUser.Id.GetValueOrDefault());
            EntityAssert.Equal(newUser, data!);
        }

        [Test]
        public void UpdateAsync_NotExistingUser_DoesNotUpdatesUser()
        {
            var newUser = builder.Generate();

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await this.repository.UpdateAsync(newUser));

            var databaseUser = fixture.Context.Users.FirstOrDefault(u => u.Id == newUser.Id);
            Assert.That(databaseUser, Is.Null);
        }

        [Test]
        public void UpdateAsync_NotExistingUser_ThrowsDbUpdateConcurrencyException()
        {
            var newUser = builder.Generate();
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await this.repository.UpdateAsync(newUser));
        }
    }
}
