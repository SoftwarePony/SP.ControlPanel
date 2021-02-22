namespace SP.ControlPanel.Data.Interfaces.Repositories
{
    public interface IPersonsRepository
    {
        void Add(IPerson person);
        void Update(IPerson person);
        void Delete(IPerson person);
        IPerson GetByEmail(string email);
    }
}