using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;
using Tryitter.WebApi.Requests;
using Tryitter.WebApi.Responses;
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
        public async Task<IActionResult> Register(StudentRequest studentBody)
        {
            var student = new Student(
                studentBody.Name,
                studentBody.Email,
                studentBody.Module,
                studentBody.Status,
                studentBody.Password);

            if (!student.IsValid)
            {
                var keyErrors = student.Notifications.Aggregate("", (current, studentNotification) => current + (studentNotification.Key + ", "));
                var errorsMessage = student.Notifications.Aggregate("", (current, studentNotification) => current + (studentNotification.Message + ", "));

                return Problem(
                    title: keyErrors,
                    statusCode: 400,
                    detail: errorsMessage);
            }

            await _studentServices.Register(student);

            return Created($"/student/{student.Id}", student.Id);
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetStudent()
        {
            var id = new Guid(User.Identity!.Name!);

            var studentFound = await _studentServices.GetStudent(id);

            var studentResponse = new StudentResponse(
                studentFound.Id,
                studentFound.Name,
                studentFound.Email,
                studentFound.Module,
                studentFound.Status);

            return Ok(studentResponse);
        }

        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Update(StudentRequest studentBody)
        {
            var id = new Guid(User.Identity!.Name!);

            var student = new Student(
                id,
                studentBody.Name,
                studentBody.Email,
                studentBody.Module,
                studentBody.Status,
                studentBody.Password
            );
            
            if (!student.IsValid)
            {
                var keyErrors = student.Notifications.Aggregate("", (current, studentNotification) => current + (studentNotification.Key + ", "));
                var errorsMessage = student.Notifications.Aggregate("", (current, studentNotification) => current + (studentNotification.Message + ", "));

                return Problem(
                    title: keyErrors,
                    statusCode: 400,
                    detail: errorsMessage);
            }

            var studentFound = await _studentServices.UpdateStudent(student);

            var studentResponse = new StudentResponse(
                studentFound.Id,
                studentFound.Name,
                studentFound.Email,
                studentFound.Module,
                studentFound.Status);

            return Ok(studentResponse);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authenticatedUserid = new Guid(User.Identity!.Name!);

            if (authenticatedUserid != id)
            {
                return Problem(statusCode: 403);
            }
            
            await _studentServices.DeleteStudent(id);
            return NoContent();
        }
    }
}