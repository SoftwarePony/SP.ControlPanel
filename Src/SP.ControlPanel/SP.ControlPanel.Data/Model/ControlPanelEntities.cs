using Microsoft.EntityFrameworkCore;
using SP.ControlPanel.Data.Entities;

namespace SP.ControlPanel.Data.Model
{
    public class ControlPanelEntities : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonType> PersonTypes { get; set; }

        public ControlPanelEntities(DbContextOptions options) : base(options)
        {
            
        }
    }
}