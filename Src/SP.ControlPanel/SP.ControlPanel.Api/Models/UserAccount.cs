using SP.ControlPanel.Business.Interfaces.Enums;
using SP.ControlPanel.Business.Interfaces.Model;

namespace SP.ControlPanel.Api.Models
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
    }
}