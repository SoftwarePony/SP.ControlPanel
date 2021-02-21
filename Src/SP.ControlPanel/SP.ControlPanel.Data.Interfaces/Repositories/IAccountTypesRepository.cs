using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SP.ControlPanel.Data.Interfaces.Repositories
{
    public interface IAccountTypesRepository
    {
        ICollection<IAccountType> GetAll();
    }
}