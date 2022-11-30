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

        public StudentController(IStudentServices studentSerices)
        {
            _studentServices = studentSerices;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] StudentLoginRequest request)
        {
            return Ok(await _studentServices.Login(request.Email, request.Password));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]Student ToCreate)
        {
            var isCreated = await _studentServices.Register(ToCreate);

            if (!isCreated)
            {
                return BadRequest("Email ja cadastrado!");
            }

            return Created("", ToCreate);
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetStudent()
        {
            if (User is null || User.Identity is null || User.Identity.Name is null)
            {
                return BadRequest();
            }

            var Id = new Guid(User.Identity.Name);
            return Ok(await _studentServices.GetStudent(Id));
        }

        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Update(Student ToUpdate)
        {
            var isUpdated = await _studentServices.UpdateStudent(ToUpdate);
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
