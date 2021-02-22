namespace SP.ControlPanel.Data.Interfaces
{
    public interface IAccount
    {
        long Id { get; set; }
        string IdentityProviderId { get; set; }
        int AccountTypeId { get; set; }
        long? AccountOwnerId { get; set; }
        long PersonId { get; set; }
        bool IsActive { get; set; }
    }
}