using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Data.Entities
{
    public class Person : IPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required]
        [ForeignKey(nameof(PersonType))]
        public int PersonTypeId { get; set; }
        
        public PersonType PersonType { get; set; }

        public Person(IPerson person)
        {
            Id = person.Id;
            Name = person.Name;
            LastName = person.LastName;
            Email = person.Email;
            PersonTypeId = person.PersonTypeId;
        }

        public Person()
        {
            
        }
    }
}