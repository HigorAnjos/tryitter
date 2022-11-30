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
        public Task<Post> CreatePost(Post ToCreate, Guid StudentId)
        {
            return _postRepository.CreatePost(ToCreate, StudentId);
        }

        public Task<bool> DeletePost(Guid PostId, Guid StudentId)
        {
            return _postRepository.DeletePost(PostId, StudentId);
        }

        public Task<IEnumerable<Post>> GetAllPost(Guid StudentId)
        {
            return _postRepository.GetAllPost(StudentId);
        }

        public Task<Post> GetPostById(Guid PostId, Guid StudentId)
        {
            return _postRepository.GetPostById(PostId, StudentId);
        }

        public Task<Post> UpdatePost(Post ToUpdate, Guid StudentId)
        {
            return _postRepository.UpdatePost(ToUpdate, StudentId);
        }
    }
}
