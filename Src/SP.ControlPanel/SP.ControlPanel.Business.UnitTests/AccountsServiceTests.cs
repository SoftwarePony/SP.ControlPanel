using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SP.ControlPanel.Business.Interfaces.Enums;
using SP.ControlPanel.Business.Interfaces.Exceptions;
using SP.ControlPanel.Business.Interfaces.Model;
using SP.ControlPanel.Business.Interfaces.Services;
using SP.ControlPanel.Business.Model;
using SP.ControlPanel.Business.Services;
using SP.ControlPanel.Data.Entities;
using SP.ControlPanel.Data.Entities.NullObjects;
using SP.ControlPanel.Data.Helpers;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Helpers;
using SP.ControlPanel.Data.Interfaces.Repositories;

namespace SP.ControlPanel.Business.UnitTests
{
    public class AccountsServiceTests
    {
        private IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ShouldThrowExceptionIfNewAccountEmailAlreadyExists()
        {
            //Arrange
            IUserAccount account = new UserAccount()
            {
                Name = "Jessica",
                AccountType = AccountTypes.RootAccount,
                PersonType = PersonTypes.Company,
                Email = "softwarepony@pro-code.tech",
                IdentityProviderId = "111-111-111"
            };
            Mock<IPersonsRepository> mockedPersonsRepository = new Mock<IPersonsRepository>();
            mockedPersonsRepository.Setup(x => x.GetByEmail(It.IsAny<string>()))
                                    .Returns(new Person()
                                    {
                                        Email = account.Email,
                                        Id = 1,
                                        LastName = account.LastName,
                                        Name = account.Name,
                                        PersonTypeId = (int)PersonTypes.Company
                                    });
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.PersonsRepository).Returns(mockedPersonsRepository.Object);
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            Assert.Throws<EmailAlreadyInUseException>(() =>
            {
                //Act
                accountsService.Create(account);
            });
        }

        [Test]
        public void ShouldThrowExceptionIfNewAccountDataIsInvalid()
        {
            //Arrange
            IUserAccount account = new UserAccount()
            {
                Name = null,
                AccountType = AccountTypes.RootAccount,
                PersonType = PersonTypes.Company,
                Email = "softwarepony@pro-code.tech",
                IdentityProviderId = "111-111-111"
            };
            Mock<IPersonsRepository> mockedPersonsRepository = new Mock<IPersonsRepository>();
            mockedPersonsRepository.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .Returns(new Person()
                {
                    Email = account.Email,
                    Id = 1,
                    LastName = account.LastName,
                    Name = account.Name,
                    PersonTypeId = (int)PersonTypes.Company
                });
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.PersonsRepository).Returns(mockedPersonsRepository.Object);
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            Assert.Throws<ValidationFailedException>(() =>
            {
                //Act
                accountsService.Create(account);
            });
        }

        [Test]
        public void ShouldCreateNewAccount()
        {
            //Arrange
            int expectedAccountId = 1;
            IUserAccount account = new UserAccount()
            {
                Name = "Jessica",
                AccountType = AccountTypes.RootAccount,
                PersonType = PersonTypes.Company,
                Email = "softwarepony@pro-code.tech",
                IdentityProviderId = "111-111-111"
            };

            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.Setup(x => x.Add(It.IsAny<IAccount>()));

            Mock<IPersonsRepository> mockedPersonsRepository = new Mock<IPersonsRepository>();
            mockedPersonsRepository.Setup(x => x.Add(It.IsAny<IPerson>()));
            mockedAccountsRepository.Setup(x => x.GetByIdentityProviderId(It.IsAny<string>())).Returns(new AccountDetail(){ AccountId = expectedAccountId });
            mockedPersonsRepository.SetupSequence(x => x.GetByEmail(It.IsAny<string>()))
                                    .Returns(new NullPerson())
                                    .Returns(new Person()
                                    {
                                        Email = account.Email,
                                        Id = 1,
                                        LastName = account.LastName,
                                        Name = account.Name,
                                        PersonTypeId = (int) PersonTypes.Company
                                    });


            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);
            mockedUOW.Setup(x => x.PersonsRepository).Returns(mockedPersonsRepository.Object);
            mockedUOW.Setup(x => x.Save());

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            //Act
            IUserAccount createdAccount = accountsService.Create(account);

            //Assert
            mockedAccountsRepository.VerifyAll();
            mockedPersonsRepository.VerifyAll();
            mockedUOW.VerifyAll();
            Assert.AreEqual(expectedAccountId, createdAccount.AccountId);
        }

        [Test]
        public void ShouldUpdateAccount()
        {
            //Arrange
            Account account = new Account()
            {
                PersonId = 1,
                IsActive = true,
                AccountTypeId = (int)AccountTypes.RootAccount,
                Id = 1,
                IdentityProviderId = "111-111-111"
            };
            Person person = new Person()
            {
                Email = "softwarepony@pro-code.tech",
                Name = "Jessica",
                PersonTypeId = (int)PersonTypes.Company,
                Id = 1
            };
            IUserAccount accountToUpdate = new UserAccount()
            {
                Name = "Jessica",
                AccountType = AccountTypes.RootAccount,
                PersonType = PersonTypes.Company,
                Email = "softwareponyE@pro-code.tech",
                IdentityProviderId = "111-111-111"
            };

            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.SetupSequence(x => x.GetById(It.IsAny<long>()))
                                    .Returns(new AccountDetail() { Email = person.Email })
                                    .Returns(new AccountDetail() { Email = accountToUpdate.Email });

            Mock<IPersonsRepository> mockedPersonsRepository = new Mock<IPersonsRepository>();
            mockedPersonsRepository.Setup(x => x.Update(It.IsAny<IPerson>()));
            mockedPersonsRepository.SetupSequence(x => x.GetByEmail(It.IsAny<string>()))
                                    .Returns(new NullPerson())
                                    .Returns(person);


            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);
            mockedUOW.Setup(x => x.PersonsRepository).Returns(mockedPersonsRepository.Object);
            mockedUOW.Setup(x => x.Save());

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            //Act
            IUserAccount updatedAccount = accountsService.Update(accountToUpdate);

            //Assert
            mockedAccountsRepository.VerifyAll();
            mockedPersonsRepository.VerifyAll();
            mockedUOW.VerifyAll();
            Assert.AreEqual(accountToUpdate.Email, updatedAccount.Email);
        }

        [Test]
        public void ShouldThrowExceptionIfUpdatedAccountEmailAlreadyExists()
        {
            //Arrange
            IUserAccount account = new UserAccount()
            {
                Name = "Jessica",
                AccountType = AccountTypes.RootAccount,
                PersonType = PersonTypes.Company,
                Email = "softwareponyE@pro-code.tech",
                IdentityProviderId = "111-111-111"
            };
            Mock<IPersonsRepository> mockedPersonsRepository = new Mock<IPersonsRepository>();
            mockedPersonsRepository.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .Returns(new Person()
                {
                    Email = account.Email,
                    Id = 1,
                    LastName = account.LastName,
                    Name = account.Name,
                    PersonTypeId = (int)PersonTypes.Company
                });
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.SetupSequence(x => x.GetById(It.IsAny<long>()))
                .Returns(new AccountDetail() {Email = "softwarepony@pro-code.tech"});
            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.PersonsRepository).Returns(mockedPersonsRepository.Object);
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            Assert.Throws<EmailAlreadyInUseException>(() =>
            {
                //Act
                accountsService.Update(account);
            });
        }

        [Test]
        public void ShouldThrowExceptionIfUpdatedAccountDataIsInvalid()
        {
            //Arrange
            IUserAccount account = new UserAccount()
            {
                Name = null,
                AccountType = AccountTypes.RootAccount,
                PersonType = PersonTypes.Company,
                Email = "softwareponyE@pro-code.tech",
                IdentityProviderId = "111-111-111"
            };
            Mock<IPersonsRepository> mockedPersonsRepository = new Mock<IPersonsRepository>();
            mockedPersonsRepository.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .Returns(new Person()
                {
                    Email = account.Email,
                    Id = 1,
                    LastName = account.LastName,
                    Name = account.Name,
                    PersonTypeId = (int)PersonTypes.Company
                });
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.SetupSequence(x => x.GetById(It.IsAny<long>()))
                .Returns(new AccountDetail() { Email = "softwarepony@pro-code.tech" });
            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.PersonsRepository).Returns(mockedPersonsRepository.Object);
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            Assert.Throws<ValidationFailedException>(() =>
            {
                //Act
                accountsService.Update(account);
            });
        }

        [Test]
        public void ShouldDisableAccount()
        {
            //Arrange
            IUserAccount account = new UserAccount()
            {
                AccountId = 1,
                IsActive = true
            };
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.Setup(x => x.DeleteById(It.IsAny<long>()));
            mockedAccountsRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new AccountDetail());
            
            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            //Act
            accountsService.Delete(account);

            //Assert
            mockedAccountsRepository.VerifyAll();
            mockedUOW.VerifyAll();
        }

        [Test]
        public void ShouldGetAccountById()
        {
            //Arrange
            long expectedId = 1;
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new AccountDetail(){ AccountId = expectedId });

            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            //Act
            IUserAccount foundAccount = accountsService.GetById(expectedId);

            //Assert
            mockedAccountsRepository.VerifyAll();
            mockedUOW.VerifyAll();
            Assert.AreEqual(expectedId, foundAccount.AccountId);
        }

        [Test]
        public void ShouldGetAccountByIdentityProviderId()
        {
            //Arrange
            string expectedIdentityProviderId = "111-111-111";
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.Setup(x => x.GetByIdentityProviderId(It.IsAny<string>())).Returns(new AccountDetail() { IdentityProviderId = expectedIdentityProviderId });

            Mock<IUnitOfWork> mockedUOW = new Mock<IUnitOfWork>();
            mockedUOW.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            _uow = mockedUOW.Object;
            IAccountsService accountsService = new AccountsService(_uow);

            //Act
            IUserAccount foundAccount = accountsService.GetByGlobalId(expectedIdentityProviderId);

            //Assert
            mockedAccountsRepository.VerifyAll();
            mockedUOW.VerifyAll();
            Assert.AreEqual(expectedIdentityProviderId, foundAccount.IdentityProviderId);
        }

        [Test]
        public void ShouldGetPaginatedAccountsList()
        {
            //Arrange
            Mock<IAccountsRepository> mockedAccountsRepository = new Mock<IAccountsRepository>();
            mockedAccountsRepository.Setup(x => x.PaginatedGetAllDetails(It.IsAny<int>(), It.IsAny<int>()))
                                    .Returns(new Data.Helpers.PaginatedResult<IAccountDetail>(new List<IAccountDetail>(){ new AccountDetail(){ AccountId = 1 } }, 1, 1, 1));

            Mock<IUnitOfWork> mockedUnitOfWork = new Mock<IUnitOfWork>();
            mockedUnitOfWork.Setup(x => x.AccountsRepository).Returns(mockedAccountsRepository.Object);

            IAccountsService accountsService = new AccountsService(mockedUnitOfWork.Object);

            //Act
            Business.Interfaces.Helpers.IPaginatedResult<IUserAccount> accountDetails = accountsService.PaginatedList(1, 1);

            //Assert
            mockedAccountsRepository.VerifyAll();
            mockedUnitOfWork.VerifyAll();
            Assert.IsNotEmpty(accountDetails.Items);
        }
    }
}