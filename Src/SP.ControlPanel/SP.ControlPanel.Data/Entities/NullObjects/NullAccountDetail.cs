using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Data.Entities.NullObjects
{
    public class NullAccountDetail : IAccountDetail
    {
        public long AccountId { get; set; }
        public string IdentityProviderId { get; set; }
        public int AccountTypeId { get; set; }
        public long? AccountOwnerId { get; set; }
        public long PersonId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PersonTypeId { get; set; }
        public string AccountTypeDescription { get; set; }
        public string PersonTypeDescription { get; set; }
        public bool IsActive { get; set; }
    }
}