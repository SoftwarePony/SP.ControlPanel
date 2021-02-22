using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Business.DTO
{
    public class PersonDTO : IPerson
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PersonTypeId { get; set; }
    }
}