using SP.ControlPanel.Business.Interfaces.Enums;

namespace SP.ControlPanel.Business.Interfaces.Model
{
    public interface IUserAccount
    {
        long AccountId { get; set; }
        string IdentityProviderId { get; set; }
        string Name { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        long PersonId { get; set; }
        long? AccountOwnerId { get; set; }
        bool IsActive { get; set; }
        AccountTypes AccountType { get; set; }
        PersonTypes PersonType { get; set; }
    }
}