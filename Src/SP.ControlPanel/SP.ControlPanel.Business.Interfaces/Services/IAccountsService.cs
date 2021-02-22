using SP.ControlPanel.Business.Interfaces.Helpers;
using SP.ControlPanel.Business.Interfaces.Model;

namespace SP.ControlPanel.Business.Interfaces.Services
{
    public interface IAccountsService
    {
        IUserAccount Create(IUserAccount account);
        IUserAccount Update(IUserAccount account);
        IUserAccount Delete(IUserAccount account);
        IUserAccount GetByGlobalId(string id);
        IUserAccount GetById(long id);
        IPaginatedResult<IUserAccount> PaginatedList(int page, int size);
    }
}