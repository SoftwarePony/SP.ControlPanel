﻿namespace SP.ControlPanel.Data.Interfaces
{
    public interface IPerson
    {
        long Id { get; set; }
        string Name { get; set; }
        string LastName { get; set; }
        int PersonTypeId { get; set; }
    }
}