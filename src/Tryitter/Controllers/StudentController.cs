using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;
using Tryitter.WebApi.ViewModels;

namespace Tryitter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentServices _studentServices;

        public StudentController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] StudentLoginRequest request)
        {
            return Ok(await _studentServices.Login(request.Email, request.Password));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]Student student)
        {
            await _studentServices.Register(student);
            
            return Created($"/student/{student.Id}", student);
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetStudent()
        {
            var id = new Guid(User.Identity!.Name!);
            return Ok(await _studentServices.GetStudent(id));
        }

        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Update(Student student)
        {
            var studentUpdated = await _studentServices.UpdateStudent(student);
            return Created($"/student/{studentUpdated.Id}", studentUpdated);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _studentServices.DeleteStudent(id);
            return NoContent();
        }
    }
}