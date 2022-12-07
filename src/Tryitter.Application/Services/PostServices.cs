using Microsoft.AspNetCore.Http;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;
using Tryitter.Domain.Repository;

namespace Tryitter.Application.Services
{
    public class PostServices : IPostServices
    {
        private readonly IPostRepository _postRepository;

        public PostServices(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Task<Post> CreatePost(Post post, Guid studentId)
        {
            return _postRepository.CreatePost(post, studentId);
        }

        public Task<IEnumerable<Post>> GetAllPost(Guid studentId)
        {
            return _postRepository.GetAllPost(studentId);
        }

        public async Task<Post> GetPostById(Guid postId, Guid studentId)
        {
            var postFound = await _postRepository.GetPostById(postId, studentId);

            if (postFound is null)
            {
                throw new BadHttpRequestException("Post não encontrado!");
            }

            return postFound;
        }

        public async Task<Post> UpdatePost(Post post, Guid studentId)
        {
            var postFound = await _postRepository.GetPostById(post.Id, studentId);

            if (postFound is null)
            {
                throw new BadHttpRequestException("Post não encontrado!");
            }

            return await _postRepository.UpdatePost(post, studentId);
        }

        public async Task<bool> DeletePost(Guid postId, Guid studentId)
        {
            var postFound = await _postRepository.GetPostById(postId, studentId);

            if (postFound is null)
            {
                throw new BadHttpRequestException("Post não encontrado!");
            }

            return await _postRepository.DeletePost(postId, studentId);
        }
    }
}