using System.Linq;
using SP.ControlPanel.Data.Entities;
using SP.ControlPanel.Data.Entities.NullObjects;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;

namespace SP.ControlPanel.Data.Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private ControlPanelEntities _db;

        public PersonsRepository(ControlPanelEntities db)
        {
            _db = db;
        }

        public void Add(IPerson person)
        {
            Person newPerson = new Person(person);

            _db.Persons.Add(newPerson);
        }

        public void Update(IPerson person)
        {
            Person dbPerson = _db.Persons.FirstOrDefault(x => x.Id == person.Id);

            dbPerson.Email = person.Email;
            dbPerson.LastName = person.LastName;
            dbPerson.Name = person.Name;
            dbPerson.PersonTypeId = person.PersonTypeId;
        }

        public void Delete(IPerson person)
        {
            Person dbPerson = _db.Persons.FirstOrDefault(x => x.Id == person.Id);
            _db.Persons.Remove(dbPerson);
        }

        public IPerson GetByEmail(string email)
        {
            IPerson dbPerson = _db.Persons.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

            dbPerson ??= new NullPerson();

            return dbPerson;
        }
    }
}