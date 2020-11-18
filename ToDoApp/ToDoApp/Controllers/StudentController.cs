using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        DataContext dataContext;
        public StudentController(IUnitOfWork _unitOfWork,DataContext data)
        {
            unitOfWork = _unitOfWork;
            dataContext = data;
        }
        [HttpGet("get-all")]
        public async Task<IEnumerable<Student>> GetAll()
        {
            return await unitOfWork.studentRepo.GetAll();
        }
    }
}