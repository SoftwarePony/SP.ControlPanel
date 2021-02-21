using System.Collections.Generic;
using System.Linq;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;

namespace SP.ControlPanel.Data.Repositories
{
    public class AccountTypesRepository : IAccountTypesRepository
    {
        private ControlPanelEntities _db;

        public AccountTypesRepository(ControlPanelEntities db)
        {
            _db = db;
        }

        public ICollection<IAccountType> GetAll()
        {
            ICollection<IAccountType> allAccountTypes = _db.AccountTypes.ToList<IAccountType>();

            return allAccountTypes;
        }
    }
}