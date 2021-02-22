using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
    public class AccountTypesRepositoryTests
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
            mock.Setup(o => o.AccountTypesRepository).Returns(new AccountTypesRepository(_db));

            _uow = mock.Object;
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public void ShouldReturnAllAccountTypes()
        {
            //Arrange
            string testName = "TestAccountType";
            _db.AccountTypes.Add(new AccountType() { Name = testName });
            _db.SaveChanges();

            //Act
            ICollection<IAccountType> accountTypes = _uow.AccountTypesRepository.GetAll();

            //Assert
            Assert.AreEqual(1, accountTypes.Count);
            Assert.AreEqual(testName, accountTypes.ElementAt(0).Name);
        }
    }
}