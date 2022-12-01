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
        public async Task<IActionResult> Register([FromBody]Student toCreate)
        {
            var isCreated = await _studentServices.Register(toCreate);

            if (!isCreated)
            {
                return BadRequest("Email ja cadastrado!");
            }

            return Created("", toCreate);
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetStudent()
        {
            if (User.Identity?.Name is null)
            {
                return BadRequest();
            }

            var id = new Guid(User.Identity.Name);
            return Ok(await _studentServices.GetStudent(id));
        }

        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Update(Student toUpdate)
        {
            var isUpdated = await _studentServices.UpdateStudent(toUpdate);
            return Created("", isUpdated);
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