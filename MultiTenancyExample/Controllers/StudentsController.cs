using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenancyExample.Dtos;
using MultiTenancyExample.Entities;
using MultiTenancyExample.Services;

namespace MultiTenancyExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var students = await _studentService.GetAllAsync(); 
            return Ok(students);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var student = await _studentService.GetByIdAsync(id);

            
            return student is null ? NotFound() : Ok(student);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(StudentDto studentDto)
        {
            Student student = new()
            {
                Name = studentDto.Name,
                Age = studentDto.Age,
                Grade = studentDto.Grade,
                
            };
            
            var createdStudent = await _studentService.CreatedAsync(student);

            return Ok(student);
        }

    }
}
