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
            try
            {
                return _postRepository.CreatePost(post, studentId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<IEnumerable<Post>> GetAllPost(Guid studentId)
        {
            try
            {
                return _postRepository.GetAllPost(studentId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Post> GetPostById(Guid postId, Guid studentId)
        {
            try
            {
                return _postRepository.GetPostById(postId, studentId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Post> UpdatePost(Post post, Guid studentId)
        {
            try
            {
                return _postRepository.UpdatePost(post, studentId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<bool> DeletePost(Guid postId, Guid studentId)
        {
            try
            {
                return _postRepository.DeletePost(postId, studentId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}