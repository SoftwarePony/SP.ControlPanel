using SP.ControlPanel.Business.Interfaces.Enums;
using SP.ControlPanel.Business.Interfaces.Model;
using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Business.Model
{
    public class UserAccount : IUserAccount
    {
        public long AccountId { get; set; }
        public string IdentityProviderId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long PersonId { get; set; }
        public long? AccountOwnerId { get; set; }
        public bool IsActive { get; set; }
        public AccountTypes AccountType { get; set; }
        public PersonTypes PersonType { get; set; }

        public UserAccount()
        {
            
        }

        public UserAccount(IAccountDetail account)
        {
            AccountId = account.AccountId;
            IdentityProviderId = account.IdentityProviderId;
            Name = account.Name;
            LastName = account.LastName;
            Email = account.Email;
            PersonId = account.PersonId;
            AccountOwnerId = account.AccountOwnerId;
            IsActive = account.IsActive;
            AccountType = (AccountTypes)account.AccountTypeId;
            PersonType = (PersonTypes) account.PersonTypeId;
        }
    }
}