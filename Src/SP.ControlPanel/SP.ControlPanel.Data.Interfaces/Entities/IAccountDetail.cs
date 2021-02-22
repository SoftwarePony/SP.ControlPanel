namespace SP.ControlPanel.Data.Interfaces
{
    public interface IAccountDetail
    {
        long AccountId { get; set; }
        string IdentityProviderId { get; set; }
        int AccountTypeId { get; set; }
        long? AccountOwnerId { get; set; }
        long PersonId { get; set; }
        string Name { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        int PersonTypeId { get; set; }
        string AccountTypeDescription { get; set; }
        string PersonTypeDescription { get; set; }
        bool IsActive { get; set; }
    }
}