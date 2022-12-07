using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;
using Tryitter.WebApi.Responses;

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
            
            var postFound = await _postServices.GetPostById(id, studentId);

            var postResponse = new PostResponse(postFound.Id, postFound.Message);

            return Ok(postResponse);
        }

        [HttpGet]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> GetAll()
        {
            var studentId = new Guid(User.Identity!.Name!);
            
            var postFound = await _postServices.GetAllPost(studentId);

            var postResponse = postFound.Select(p => new PostResponse(p.Id, p.Message));

            return Ok(postResponse);
        }

        [HttpPost]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Create(Post postBody)
        {
            var studentId = new Guid(User.Identity!.Name!);

            postBody.EditInfo(postBody.Id, postBody.Message);

            if (!postBody.IsValid)
            {
                var keyErrors = postBody.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Key + ", "));
                var errorsMessage = postBody.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Message + ", "));

                return Problem(
                    title: keyErrors,
                    statusCode: 400,
                    detail: errorsMessage);
            }

            var post = await _postServices.CreatePost(postBody, studentId);

            return Created($"/post/{post.Id}", post.Id);
        }

        [HttpPut]
        [Authorize(policy: "Student")]
        public async Task<IActionResult> Update(Post postBody)
        {
            var studentId = new Guid(User.Identity!.Name!);

            if (!postBody.IsValid)
            {
                var keyErrors = postBody.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Key + ", "));
                var errorsMessage = postBody.Notifications.Aggregate("",
                    (current, studentNotification) => current + (studentNotification.Message + ", "));

                return Problem(
                    title: keyErrors,
                    statusCode: 400,
                    detail: errorsMessage);
            }

            var updatedPost = await _postServices.UpdatePost(postBody, studentId);

            var postResponse = new PostResponse(updatedPost.Id, updatedPost.Message);

            return Ok(postResponse);
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