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
        
        public Task<Post> GetPostById(Guid postId, Guid studentId)
        {
            return _postRepository.GetPostById(postId, studentId);
        }
        
        public Task<Post> UpdatePost(Post post, Guid studentId)
        {
            return _postRepository.UpdatePost(post, studentId);
        }

        public Task<bool> DeletePost(Guid postId, Guid studentId)
        {
            return _postRepository.DeletePost(postId, studentId);
        }
    }
}