using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Data.Services;

namespace MVCConsoleApp.BusinessLogic.Services
{
    public class PersonRoleController
    {
        private readonly PersonRoleServices _service;
        public PersonRoleController()
        {
            _service = new PersonRoleServices();
        }

        /// <summary>
        /// hàm tạo new role
        /// </summary>
        /// <param name="name">tên role</param>
        /// <param name="description">mô tả role</param>
        /// <returns></returns>
        public bool Create(string name, string description)
        {
            PersonRole personRole = new PersonRole
            {
                Name = name,
                Description = description
            };
            return _service.Create(personRole);
        }

        public List<PersonRole> GetAll()
        {
            return _service.GetAll();
        }
        public PersonRole? GetById(int id)
        {
            return _service.GetById(id);
        }
        public bool Update(int id, string name, string description)
        {
            PersonRole personRole = new PersonRole
            {
                Id = id,
                Name = name,
                Description = description
            };
            return _service.Update(personRole);
        }
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
