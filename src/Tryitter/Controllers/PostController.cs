using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;

namespace Tryitter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostServices _postServices;

        public PostController(IPostServices postServices)
        {
            _postServices = postServices;
        }

        [HttpGet("{id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var studentId = GetGuidToken();

            return Ok(await _postServices.GetPostById(id, studentId));
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetAll()
        {
            var studentId = GetGuidToken();

            return Ok(await _postServices.GetAllPost(studentId));
        }

        [HttpPost]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Create(Post toCreate)
        {
            var studentId = GetGuidToken();

            return Ok(await _postServices.CreatePost(toCreate, studentId));
        }
        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<Post> Update(Post toUpdate)
        {
            var studentId = GetGuidToken();

            return await _postServices.UpdatePost(toUpdate, studentId);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var studentId = GetGuidToken();

            var isDeleted = await _postServices.DeletePost(id, studentId);

            if (isDeleted is false)
                return BadRequest();

            return NoContent();
        }

        public Guid GetGuidToken()
        {
            if (User.Identity?.Name is null)
            {
                return Guid.Empty; // Ja existe, validacao aqui so para parar os Warnings
            }

            var id = new Guid(User.Identity.Name);
            return id;
        }
    }
}