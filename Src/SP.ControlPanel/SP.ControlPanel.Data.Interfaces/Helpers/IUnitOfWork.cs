﻿using SP.ControlPanel.Data.Interfaces.Repositories;

namespace SP.ControlPanel.Data.Interfaces.Helpers
{
    public interface IUnitOfWork
    {
        IAccountTypesRepository AccountTypesRepository { get; }
        IPersonTypesRepository PersonTypesRepository { get; }
        IAccountsRepository AccountsRepository { get; }
        IPersonsRepository PersonsRepository { get; }

        void Save();
    }
}