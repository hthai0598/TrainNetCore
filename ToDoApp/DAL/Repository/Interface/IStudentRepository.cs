using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Repository.Interface
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetListActive();
    }
}
