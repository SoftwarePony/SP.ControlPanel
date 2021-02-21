using System.Collections.Generic;
using System.Linq;
using SP.ControlPanel.Data.Entities;
using SP.ControlPanel.Data.Interfaces;
using SP.ControlPanel.Data.Interfaces.Repositories;
using SP.ControlPanel.Data.Model;

namespace SP.ControlPanel.Data.Repositories
{
    public class PersonTypesRepository : IPersonTypesRepository
    {
        private ControlPanelEntities _db;

        public PersonTypesRepository(ControlPanelEntities db)
        {
            _db = db;
        }

        public ICollection<IPersonType> GetAll()
        {
            ICollection<IPersonType> allRecords = _db.PersonTypes.ToList<IPersonType>();
             
            return allRecords;
        }
    }
}