using SP.ControlPanel.Data.Interfaces;

namespace SP.ControlPanel.Data.Entities.NullObjects
{
    public class NullPerson : IPerson
    {
        public long Id { get; set; }
        public string Name
        {
            get => "Person doesn't exist";
            set { }
        }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PersonTypeId { get; set; }
    }
}