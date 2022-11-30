using Tryitter.Domain.Entity;

namespace Tryitter.Application.Interfaces
{
    public interface IPostServices
    {
        public Task<IEnumerable<Post>> GetAllPost(Guid StudentId);
        public Task<Post> GetPostById(Guid PostId, Guid StudentId);
        public Task<Post> CreatePost(Post ToCreate, Guid StudentId);
        public Task<Post> UpdatePost(Post ToUpdate, Guid StudentId);
        public Task<bool> DeletePost(Guid PostId, Guid StudentId);
    }
}
