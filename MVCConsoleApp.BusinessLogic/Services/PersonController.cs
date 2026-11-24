using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Data.Services;

namespace MVCConsoleApp.BusinessLogic.Services
{
    public class PersonController
    {
        private readonly PersonServices _service;
        public PersonController()
        {
            _service = new PersonServices();
        }

        public bool Create(string fullName, string? telephone, string? email, int personRoleId)
        {
            Person person = new Person
            {
                FullName = fullName,
                Telephone = telephone,
                Email = email,
                PersonRoleId = personRoleId
            };
            return _service.Create(person);
        }

        public List<Person> GetAll()
        {
            return _service.GetAll();
        }
        public Person? GetById(int id)
        {
            return _service.GetById(id);
        }
        public bool Update(int id, string fullName, string? telephone, string? email, int personRoleId)
        {
            Person person = new Person
            {
                Id = id,
                FullName = fullName,
                Telephone = telephone,
                Email = email,
                PersonRoleId = personRoleId
            };
            return _service.Update(person);
        }
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
