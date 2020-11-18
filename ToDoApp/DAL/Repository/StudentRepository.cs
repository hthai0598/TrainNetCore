using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Model;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {

        public StudentRepository(DataContext context):base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetListActive()
        {
            return await DbSet.ToListAsync();
        }
    }
}
