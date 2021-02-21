using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SP.ControlPanel.Data.Entities;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;
using SP.ControlPanel.Data.Repositories;

namespace SP.ControlPanel.Data.UnitTests
{
    public class PersonTypesRepositoryTests
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
            mock.Setup(o => o.PersonTypesRepository).Returns(new PersonTypesRepository(_db));

            _uow = mock.Object;
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public void ShouldReturnAllPersonTypes()
        {
            //Arrange
            string testName = "TestPersonType";
            _db.PersonTypes.Add(new PersonType() { Name = testName });
            _db.SaveChanges();

            //Act
            ICollection<IPersonType> personTypes = _uow.PersonTypesRepository.GetAll();

            //Assert
            Assert.AreEqual(1, personTypes.Count);
            Assert.AreEqual(testName, personTypes.ElementAt(0).Name);
        }
    }
}