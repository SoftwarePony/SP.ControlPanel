using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Data.Entities.NullObjects
{
    public class NullAccount : IAccount
    {
        public long Id { get; set; }
        public string IdentityProviderId { get; set; }
        public int AccountTypeId { get; set; }
        public long? AccountOwnerId { get; set; }
        public long PersonId { get; set; }
    }
}