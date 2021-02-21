using System.Collections.Generic;

namespace SP.ControlPanel.Data.Interfaces.Repositories
{
    public interface IPersonTypesRepository
    {
        ICollection<IPersonType> GetAll();
    }
}