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
            var studentId = new Guid(User.Identity!.Name!);

            return Ok(await _postServices.GetPostById(id, studentId));
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetAll()
        {
            var studentId = new Guid(User.Identity!.Name!);

            return Ok(await _postServices.GetAllPost(studentId));
        }

        [HttpPost]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Create(Post postBody)
        {
            var studentId = new Guid(User.Identity!.Name!);

            var postToCreate = new Post(postBody.Message);

            if (!postToCreate.IsValid)
            {
                var keyErrors = postToCreate.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Key + ", "));
                var errorsMessage = postToCreate.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Message + ", "));

                return Problem(
                    title: keyErrors,
                    statusCode: 400,
                    detail: errorsMessage);
            }

            var post = await _postServices.CreatePost(postToCreate, studentId);

            return Created($"/post/{post.Id}", post.Id);
        }

        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Update(Post postBody)
        {
            var studentId = new Guid(User.Identity!.Name!);

            var postToUpdate = new Post(postBody.Message);

            if (!postToUpdate.IsValid)
            {
                var keyErrors = postToUpdate.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Key + ", "));
                var errorsMessage = postToUpdate.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Message + ", "));

                return Problem(
                    title: keyErrors,
                    statusCode: 400,
                    detail: errorsMessage);
            }

            var updatedPost = await _postServices.UpdatePost(postToUpdate, studentId);

            return Ok(updatedPost);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var studentId = new Guid(User.Identity!.Name!);

            await _postServices.DeletePost(id, studentId);

            return NoContent();
        }
    }
}