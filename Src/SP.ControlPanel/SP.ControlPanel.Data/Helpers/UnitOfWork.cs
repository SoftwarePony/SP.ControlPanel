using SP.ControlPanel.Data.Interfaces.Helpers;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;

namespace SP.ControlPanel.Data.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private ControlPanelEntities _db;

        public UnitOfWork(IAccountTypesRepository accountTypesRepository,
                          IPersonTypesRepository personTypesRepository,
                          IAccountsRepository accountsRepository,
                          IPersonsRepository personsRepository,
                          ControlPanelEntities db)
        {
            AccountTypesRepository = accountTypesRepository;
            PersonTypesRepository = personTypesRepository;
            AccountsRepository = accountsRepository;
            PersonsRepository = personsRepository;

            _db = db;
        }

        public IAccountTypesRepository AccountTypesRepository { get; }

        public IPersonTypesRepository PersonTypesRepository { get; }

        public IAccountsRepository AccountsRepository { get; }

        public IPersonsRepository PersonsRepository { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}