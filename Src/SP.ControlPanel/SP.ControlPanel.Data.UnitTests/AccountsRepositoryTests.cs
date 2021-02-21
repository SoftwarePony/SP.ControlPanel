using System;
using System.Linq;
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

            _db = new ControlPanelEntities(options);

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
        public void ShouldDeleteAccount()
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
            Account accountDb = _db.Accounts.FirstOrDefault();

            Assert.IsNull(accountDb);
        }

        [Test]
        public void ShouldGetAccountById()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1
            };
            _db.Accounts.Add(account);
            _db.SaveChanges();
            long expectedId = account.Id;

            //Act
            IAccount accountDb = _uow.AccountsRepository.GetById(expectedId);

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedId, accountDb.Id);
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
            IAccount accountDb = _uow.AccountsRepository.GetById(expectedId);

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(0, accountDb.Id);
        }

        [Test]
        public void ShouldGetAccountByIdentityProviderId()
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
            IAccount accountDb = _uow.AccountsRepository.GetByIdentityProviderId(account.IdentityProviderId);

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedId, accountDb.Id);
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
            IAccount accountDb = _uow.AccountsRepository.GetByIdentityProviderId("111-111-111");

            //Assert
            Assert.NotNull(accountDb);
            Assert.AreEqual(expectedId, accountDb.Id);
        }

        [Test]
        public void ShouldGetPaginatedAccountsFirstPage()
        {
            //Arrange
            Account account1 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000"
            };
            _db.Accounts.Add(account1);
            Account account2 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "111-111-111"
            };
            _db.Accounts.Add(account2);
            Account account3 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "222-222-222"
            };
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccount> result = _uow.AccountsRepository.PaginatedGetAll(1, 2);

            //Assert
            Assert.IsNotEmpty(result.Items);
            Assert.AreEqual(2, result.Items.Count());
            Assert.AreEqual(3, result.TotalItems);
        }

        [Test]
        public void ShouldGetPaginatedAccountsSecondPage()
        {
            //Arrange
            Account account1 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000"
            };
            _db.Accounts.Add(account1);
            Account account2 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "111-111-111"
            };
            _db.Accounts.Add(account2);
            Account account3 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "222-222-222"
            };
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccount> result = _uow.AccountsRepository.PaginatedGetAll(2, 2);

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
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "000-000-000"
            };
            _db.Accounts.Add(account1);
            Account account2 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "111-111-111"
            };
            _db.Accounts.Add(account2);
            Account account3 = new Account()
            {
                PersonId = 1,
                AccountTypeId = 1,
                IdentityProviderId = "222-222-222"
            };
            _db.Accounts.Add(account3);
            _db.SaveChanges();

            //Act
            IPaginatedResult<IAccount> result = _uow.AccountsRepository.PaginatedGetAll(2, 3);

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
        public void ShouldDeleteAccountWithId()
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
            Account accountDb = _db.Accounts.FirstOrDefault();

            Assert.IsNull(accountDb);
        }
    }
}