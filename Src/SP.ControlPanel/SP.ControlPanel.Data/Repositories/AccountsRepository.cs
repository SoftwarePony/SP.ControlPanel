using System.Collections.Generic;
using System.Linq;
using SP.ControlPanel.Data.Entities;
using SP.ControlPanel.Data.Entities.NullObjects;
using SP.ControlPanel.Data.Helpers;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Helpers;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;

namespace SP.ControlPanel.Data.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private ControlPanelEntities _db;

        public AccountsRepository(ControlPanelEntities db)
        {
            _db = db;
        }

        public IAccountDetail GetById(long id)
        {
            IAccountDetail accountDb = _db.AccountDetails.FirstOrDefault(x => x.AccountId == id);

            accountDb ??= new NullAccountDetail();

            return accountDb;
        }

        public IAccountDetail GetByIdentityProviderId(string id)
        {
            IAccountDetail accountDb = _db.AccountDetails.FirstOrDefault(x => x.IdentityProviderId == id);

            accountDb ??= new NullAccountDetail();

            return accountDb;
        }

        public IPaginatedResult<IAccountDetail> PaginatedGetAllDetails(int page, int size)
        {
            IEnumerable<IAccountDetail> accountsPage = _db.AccountDetails.Skip((page - 1) * size).Take(size).ToList();
            long totalItems = _db.AccountDetails.LongCount();

            return new PaginatedResult<IAccountDetail>(accountsPage, totalItems, page, size);
        }

        public void Add(IAccount account)
        {
            Account accountToAdd = new Account(account);

            _db.Accounts.Add(accountToAdd);
        }

        public void Update(IAccount account)
        {
            Account accountDb = _db.Accounts.FirstOrDefault(x => x.Id == account.Id);

            accountDb.IdentityProviderId = account.IdentityProviderId;
            accountDb.AccountTypeId = accountDb.AccountTypeId;
        }

        public void Delete(IAccount account)
        {
            Account accountToDelete = _db.Accounts.FirstOrDefault(x => x.Id == account.Id);
            
            accountToDelete.IsActive = false;
        }

        public void DeleteById(long id)
        {
            Account accountToDelete = _db.Accounts.FirstOrDefault(x => x.Id == id);

            accountToDelete.IsActive = false;
        }
    }
}