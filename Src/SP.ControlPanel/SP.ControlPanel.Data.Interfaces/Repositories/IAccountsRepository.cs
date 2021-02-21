using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SP.ControlPanel.Data.Interfaces.Helpers;

namespace SP.ControlPanel.Data.Interfaces.Repositories
{
    public interface IAccountsRepository
    {
        IAccount GetById(long id);
        IAccount GetByIdentityProviderId(string id);
        IPaginatedResult<IAccount> PaginatedGetAll(int page, int size);
        void Add(IAccount account);
        void Update(IAccount account);
        void Delete(IAccount account);
        void DeleteById(long id);
    }
}