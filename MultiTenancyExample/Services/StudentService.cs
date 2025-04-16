using Microsoft.EntityFrameworkCore;
using MultiTenancyExample.Data;
using MultiTenancyExample.Entities;

namespace MultiTenancyExample.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> CreatedAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<IReadOnlyList<Student>> GetAllAsync() => await _context.Students.ToListAsync();

        public async Task<Student?> GetByIdAsync(int id) => await _context.Students.FindAsync(id);
    }
}
