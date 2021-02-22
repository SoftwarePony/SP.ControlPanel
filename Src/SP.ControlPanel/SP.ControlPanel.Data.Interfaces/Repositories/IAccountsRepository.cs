using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SP.ControlPanel.Data.Interfaces.Helpers;

namespace SP.ControlPanel.Data.Interfaces.Repositories
{
    public interface IAccountsRepository
    {
        IAccountDetail GetById(long id);
        IAccountDetail GetByIdentityProviderId(string id);
        IPaginatedResult<IAccountDetail> PaginatedGetAllDetails(int page, int size);
        void Add(IAccount account);
        void Update(IAccount account);
        void Delete(IAccount account);
        void DeleteById(long id);
    }
}