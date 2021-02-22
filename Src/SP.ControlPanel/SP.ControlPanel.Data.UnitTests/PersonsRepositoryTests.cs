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
    public class PersonsRepositoryTests
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
            mock.Setup(o => o.PersonsRepository).Returns(new PersonsRepository(_db));
            mock.Setup(o => o.Save()).Callback(() => _db.SaveChanges());
            _uow = mock.Object;

            //Setup common db entries
            PersonType personType = new PersonType() { Name = "Individual" };
            _db.PersonTypes.Add(personType);
            _db.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public void ShouldAddPerson()
        {
            //Arrange
            IPerson newPerson = new Person()
            {
                Name = "Jessica",
                LastName = "L",
                Email = "jessica@pro-code.tech",
                PersonTypeId = 1
            };

            //Act
            _uow.PersonsRepository.Add(newPerson);
            _uow.Save();

            //Assert
            Person dbPerson = _db.Persons.FirstOrDefault();

            Assert.NotNull(dbPerson);
            Assert.AreEqual(1, dbPerson.Id);
        }

        [Test]
        public void ShouldUpdatePerson()
        {
            //Arrange
            Person person = new Person()
            {
                Name = "Jessica",
                LastName = "L",
                Email = "jessica@pro-code.tech",
                PersonTypeId = 1
            };
            _db.Persons.Add(person);
            _db.SaveChanges();

            //Act
            IPerson updatedPerson = new Person()
            {
                Name = "Jessica",
                LastName = "L",
                Email = "jessicaE@pro-code.tech",
                PersonTypeId = 1,
                Id = 1
            };
            _uow.PersonsRepository.Update(updatedPerson);
            _uow.Save();

            //Assert
            Person dbPerson = _db.Persons.FirstOrDefault();

            Assert.NotNull(dbPerson);
            Assert.AreEqual(updatedPerson.Email, dbPerson.Email);
        }

        [Test]
        public void ShouldGetPersonByEmail()
        {
            //Arrange
            Person person = new Person()
            {
                Name = "Jessica",
                LastName = "L",
                Email = "jessica@pro-code.tech",
                PersonTypeId = 1
            };
            _db.Persons.Add(person);
            _db.SaveChanges();

            //Act
            IPerson personWithEmail = _uow.PersonsRepository.GetByEmail(person.Email);

            //Assert
            Assert.NotNull(personWithEmail);
        }

        [Test]
        public void ShouldDeletePerson()
        {
            //Arrange
            Person person = new Person()
            {
                Name = "Jessica",
                LastName = "L",
                Email = "jessica@pro-code.tech",
                PersonTypeId = 1
            };
            _db.Persons.Add(person);
            _db.SaveChanges();

            //Act
            IPerson personToDelete = new Person()
            {
                Id = 1,
                Name = "Jessica",
                LastName = "L",
                Email = "jessica@pro-code.tech",
                PersonTypeId = 1
            };
            _uow.PersonsRepository.Delete(personToDelete);
            _uow.Save();

            //Assert
            Person dbPerson = _db.Persons.FirstOrDefault();
            Assert.IsNull(dbPerson);
        }
    }
}