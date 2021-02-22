using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using SP.ControlPanel.Data.Entities;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Helpers;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;
using SP.ControlPanel.Data.Repositories;

namespace SP.ControlPanel.Data.UnitTests
{
    [TestFixture]
    public class AccountsRepositoryTests
    {
        private ControlPanelEntities _db;
        private IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            DbContextOptions options = new DbContextOptionsBuilder<ControlPanelEntities>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _db = new ControlPanelEntities(options, (context, modelBuilder) =>
            {
                modelBuilder.Entity<AccountDetail>()
                    .HasNoKey()
                    .ToView("VwAccountsDetails")
                    .ToInMemoryQuery(() => context.Accounts
                        .Join(context.Persons, x => x.PersonId, x => x.Id, (a, p) => new {Account = a, Person = p})
                        .Where(x => x.Account.IsActive)
                        .Select(x => new AccountDetail()
                        {
                            Name = x.Person.Name,
                            AccountTypeId = x.Account.AccountTypeId,
                            PersonTypeId = x.Person.PersonTypeId,
                            IdentityProviderId = x.Account.IdentityProviderId,
                            PersonId = x.Person.Id,
                            Email = x.Person.Email,
                            LastName = x.Person.LastName,
                            AccountOwnerId = x.Account.AccountOwnerId,
                            AccountId = x.Account.Id,
                            AccountTypeDescription = "Root",
                            PersonTypeDescription = "Individual"
                        }));
            });

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(o => o.AccountsRepository).Returns(new AccountsRepository(_db));
            mock.Setup(o => o.Save()).Callback(() => _db.SaveChanges());
            _uow = mock.Object;

            //Setup common db entries
            PersonType personType = new PersonType() { Name = "Individual" };
            _db.PersonTypes.Add(personType);
            _db.SaveChanges();

            Person person = new Person() { Name = "Test Person", PersonTypeId = personType.Id };
            _db.Persons.Add(person);
            _db.SaveChanges();

            AccountType accountType = new AccountType() {Name = "Root Account"};
            _db.AccountTypes.Add(accountType);
            _db.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public void ShouldAddNewAccount()
        {
            //Arrange

            //Act
            IAccount account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1
            };
            _uow.AccountsRepository.Add(account);
            _uow.Save();

            //Assert
            Account addedAccount = _db.Accounts.Single();

            Assert.NotNull(addedAccount);
            Assert.AreEqual(1, addedAccount.PersonId);
        }

        [Test]
        public void ShouldDisableAccount()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();

            //Act
            _uow.AccountsRepository.Delete(account);
            _uow.Save();

            //Assert
            Account accountDb = _db.Accounts.FirstOrDefault(x => x.IsActive == true);

            Assert.IsNull(accountDb);
        }

        [Test]
        public void ShouldGetAccountById()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IsActive = true
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = account.Id;

            //Act
            IAccountDetail accountDb = _uow.AccountsRepository.GetById(expectedId);

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedId, accountDb.AccountId);
        }

        [Test]
        public void ShouldReturnNullAccountIfIdDoesntExist()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = account.Id + 1;

            //Act
            IAccountDetail accountDb = _uow.AccountsRepository.GetById(expectedId);

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(0, accountDb.AccountId);
        }

        [Test]
        public void ShouldGetAccountByIdentityProviderId()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = account.Id;

            //Act
            IAccountDetail accountDb = _uow.AccountsRepository.GetByIdentityProviderId(account.IdentityProviderId);

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedId, accountDb.AccountId);
        }

        [Test]
        public void ShouldReturnNullAccountIfIdentityProviderIdDoesntExist()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000"
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = 0;

            //Act
            IAccountDetail accountDb = _uow.AccountsRepository.GetByIdentityProviderId("111-111-111");

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedId, accountDb.AccountId);
        }

        [Test]
        public void ShouldGetPaginatedAccountsFirstPage()
        {
            //Arrange
            Account account1 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            Account account2 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "111-111-111",
                IsActive = true
            };
            Account account3 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "222-222-222",
                IsActive = true
            };
            _db.Accounts.Add(account1);
            _db.Accounts.Add(account2);
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccountDetail> result = _uow.AccountsRepository.PaginatedGetAllDetails(1, 2);

            //Assert
            Assert.IsNotEmpty(result.Items);
            Assert.AreEqual(2, result.Items.Count());
            Assert.AreEqual(3, result.TotalItems);
        }

        [Test]
        public void ShouldGetOnlyActiveAccounts()
        {
            //Arrange
            Account account1 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            Account account2 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "111-111-111",
                IsActive = false
            };
            Account account3 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "222-222-222",
                IsActive = false
            };
            _db.Accounts.Add(account1);
            _db.Accounts.Add(account2);
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccountDetail> result = _uow.AccountsRepository.PaginatedGetAllDetails(1, 2);

            //Assert
            Assert.IsNotEmpty(result.Items);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(1, result.TotalItems);
        }

        [Test]
        public void ShouldDisableAccountsInsteadOfDeletingThem()
        {
            //Arrange
            Account account = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();

            //Act
            IAccount accountToDelete = new Account()
            {
                Id = 1,
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            _uow.AccountsRepository.Delete(accountToDelete);
            _uow.Save();

            //Assert
            Account disabledAccount = _db.Accounts.FirstOrDefault();
            Assert.NotNull(disabledAccount);
            Assert.AreEqual(false, disabledAccount.IsActive);
        }

        [Test]
        public void ShouldGetPaginatedAccountsSecondPage()
        {
            //Arrange
            Account account1 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            Account account2 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "111-111-111",
                IsActive = true
            };
            Account account3 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "222-222-222",
                IsActive = true
            };
            _db.Accounts.Add(account1);
            _db.Accounts.Add(account2);
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccountDetail> result = _uow.AccountsRepository.PaginatedGetAllDetails(2, 2);

            //Assert
            Assert.IsNotEmpty(result.Items);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(3, result.TotalItems);
        }

        [Test]
        public void ShouldGetEmptyPaginatedAccountsPageIfBiggerThanCollection()
        {
            //Arrange
            Account account1 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "000-000-000",
                IsActive = true
            };
            Account account2 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "111-111-111",
                IsActive = true
            };
            Account account3 = new Account()
            {
                AccountTypeId = 1,
                PersonId = 1,
                IdentityProviderId = "222-222-222",
                IsActive = true
            };
            _db.Accounts.Add(account1);
            _db.Accounts.Add(account2);
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccountDetail> result = _uow.AccountsRepository.PaginatedGetAllDetails(2, 3);

            //Assert
            Assert.IsEmpty(result.Items);
            Assert.AreEqual(0, result.Items.Count());
            Assert.AreEqual(3, result.TotalItems);
        }

        [Test]
        public void ShouldUpdateAccount()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000"
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = account.Id;
            string expectedString = "111-111-111";

            //Act
            _uow.AccountsRepository.Update(new Account(){Id = 1, PersonId = 1, AccountTypeId = 1, IdentityProviderId = expectedString});
            _uow.Save();

            //Assert
            Account accountDb = _db.Accounts.FirstOrDefault(x => x.Id == expectedId);
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedString, accountDb.IdentityProviderId);
        }

        [Test]
        public void ShouldDisableAccountWithId()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000"
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = account.Id;

            //Act
            _uow.AccountsRepository.DeleteById(expectedId);
            _uow.Save();

            //Assert
            Account accountDb = _db.Accounts.FirstOrDefault(x => x.IsActive == true);

            Assert.IsNull(accountDb);
        }
    }
}