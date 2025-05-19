using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Services;
using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public StudentController(AppDbContext _appDbContext) => appDbContext = _appDbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Students>>> GetStudent() =>
        await appDbContext.Students.ToListAsync();


        [HttpPost]
        public async Task<ActionResult<Students>> AddStudent(Students students)
        {
            appDbContext.Students.Add(students);
            await appDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = students.Id }, students);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var students = await appDbContext.Students.FindAsync(id);
            if(students == null) return NotFound();
            appDbContext.Students.Remove(students);
            await appDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            var student = await appDbContext.Students.FindAsync(id);
            return student == null ? NotFound() : student;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Students student)
        {
            if (id != student.Id) return BadRequest();
            appDbContext.Entry(student).State = EntityState.Modified;
            await appDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
