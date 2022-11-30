using Tryitter.Domain.Entity;

namespace Tryitter.Domain.Repository
{
    public interface IPostRepository
    {
        public Task<IEnumerable<Post>> GetAllPost(Guid StudentId);
        public Task<Post> GetPostById(Guid PostId, Guid StudentId);
        public Task<Post> CreatePost(Post ToCreate, Guid StudentId);
        public Task<Post> UpdatePost(Post ToUpdate, Guid StudentId);
        public Task<bool> DeletePost(Guid Id, Guid StudentId);
    }
}
