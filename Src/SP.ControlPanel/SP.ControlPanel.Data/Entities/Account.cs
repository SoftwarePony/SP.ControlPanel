using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Data.Entities
{
    public class Account : IAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string IdentityProviderId { get; set; }
        
        [Required]
        [ForeignKey(nameof(AccountType))]
        public int AccountTypeId { get; set; }
        
        [ForeignKey(nameof(AccountOwner))]
        public long? AccountOwnerId { get; set; }
        
        [Required]
        [ForeignKey(nameof(Person))]
        public long PersonId { get; set; }

        public Person Person { get; set; }
        public Account AccountOwner { get; set; }
        public AccountType AccountType { get; set; }

        public Account(IAccount account)
        {
            Id = account.Id;
            IdentityProviderId = account.IdentityProviderId;
            AccountTypeId = account.AccountTypeId;
            AccountOwnerId = account.AccountOwnerId;
            PersonId = account.PersonId;
        }

        public Account()
        {
            
        }
    }
}