using System;
using Microsoft.EntityFrameworkCore;
using SP.ControlPanel.Data.Entities;

namespace SP.ControlPanel.Data.Model
{
    public class ControlPanelEntities : DbContext
    {
        private readonly Action<ControlPanelEntities, ModelBuilder> _customizeModel;

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonType> PersonTypes { get; set; }
        public DbSet<AccountDetail> AccountDetails { get; set; }

        public ControlPanelEntities(DbContextOptions options, Action<ControlPanelEntities, ModelBuilder> customizeModel = null) : base(options)
        {
            _customizeModel = customizeModel;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountDetail>().HasNoKey().ToView("VwAccountsDetails");

            if (_customizeModel != null)
            {
                _customizeModel(this, modelBuilder);
            }
        }
    }
}