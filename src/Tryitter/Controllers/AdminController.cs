using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;
using Tryitter.WebApi.Requests;
using Tryitter.WebApi.Responses;

namespace Tryitter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController: ControllerBase
    {
        private readonly IPostServices _postServices;
        private readonly IStudentServices _studentServices;
        public AdminController(IPostServices postServices, IStudentServices studentServices)
        {
            _postServices = postServices;
            _studentServices = studentServices;
        }

        [HttpGet("students")]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentServices.GetAllStudents();
            var studentsResponse = new List<object>();
            foreach(var student in students)
            {
                studentsResponse.Add(new
                {
                    student.Id,
                    student.Name,
                    student.Email,
                    student.Module,
                    student.Status,
                    student.Role,
                });
            }
            return Ok(studentsResponse);
        }

        [HttpGet("{id}")]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetById(string id)
        {   
            var student = await _studentServices.GetStudent(new Guid(id));

            var studentResponse = new StudentResponse(
                student.Id,
                student.Name,
                student.Email,
                student.Module,
                student.Status,
                student.Role);

            return Ok(studentResponse);
        }

        [HttpPut("{id}")]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> UpdateById([FromBody] StudentRequest studentRequest, string id)
        {

            var studentUpdated = new Student(
                new Guid(id),
                studentRequest.Name,
                studentRequest.Email,
                studentRequest.Module,
                studentRequest.Status,
                studentRequest.Password,
                studentRequest.Role!
                );

            await _studentServices.UpdateStudent(studentUpdated);

            var studentResponse = new StudentResponse(
                studentUpdated.Id,
                studentUpdated.Name,
                studentUpdated.Email,
                studentUpdated.Module,
                studentUpdated.Status,
                studentUpdated.Role);

            return Ok(studentResponse);
        }

        [HttpDelete("{id}")]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var student = await _studentServices.GetStudent(new Guid(id));

            await _studentServices.DeleteStudent(new Guid(id));

            return NoContent();
        }

        [HttpGet("{id}/posts")]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> GetPostsById(string id)
        {
            var posts = await _postServices.GetAllPost(new Guid(id));

            return Ok(posts);
        }

        [HttpDelete("{studentId}/posts/{postId}")]
        [Authorize("Admin")]
        public async Task<IActionResult> DeletePost(string studentId, string postId)
        {
            await _postServices.DeletePost(new Guid(postId), new Guid(studentId));
            return NoContent();
        }
    }
}
