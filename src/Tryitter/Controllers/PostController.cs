using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;

namespace Tryitter.Controllers
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

        [HttpGet("{Id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetOne(Guid Id)
        {
            var StudentId = getGuidToken();

            return Ok(await _postServices.GetPostById(Id, StudentId));
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetAll()
        {
            var StudentId = getGuidToken();

            return Ok(await _postServices.GetAllPost(StudentId));
        }

        [HttpPost]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Create(Post ToCreate)
        {
            var StudentId = getGuidToken();

            return Ok(await _postServices.CreatePost(ToCreate, StudentId));
        }
        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<Post> Update(Post ToUpdate)
        {
            var StudentId = getGuidToken();

            return await _postServices.UpdatePost(ToUpdate, StudentId);
        }

        [HttpDelete("{Id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var StudentId = getGuidToken();

            var isDeleted = await _postServices.DeletePost(Id, StudentId);

            if (isDeleted is false)
                return BadRequest();

            return NoContent();
        }

        public Guid getGuidToken()
        {
            if (User is null || User.Identity is null || User.Identity.Name is null)
            {
                return Guid.Empty; // Ja existe, validacao aqui so para parar os Warnings
            }

            var Id = new Guid(User.Identity.Name);
            return Id;
        }
    }
}