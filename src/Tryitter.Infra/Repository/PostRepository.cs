using Dapper;
using System.Data;
using Tryitter.Domain.Entity;
using Tryitter.Domain.Repository;
using User.Infra.Context;

namespace Tryitter.Infra.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DapperContext _context;
        public PostRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<Post> CreatePost(Post ToCreate, Guid StudentId)
        {
            var query = "INSERT INTO Post (Message, Student_Id) OUTPUT INSERTED.Id VALUES (@Message, @StudentId)";
            
            var parameters = new DynamicParameters();
            parameters.Add("Message", ToCreate.Message, DbType.String);
            parameters.Add("StudentId", StudentId, DbType.Guid);


            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteScalarAsync<Guid>(query, parameters);
                var postCreated = new Post(ToCreate.Message);

                return postCreated;
            }
        }

        public async Task<bool> DeletePost(Guid Id, Guid StudentId)
        {
            var query = "DELETE FROM Post WHERE Id = @Id AND Student_Id = @StudentId";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id, StudentId });
                return true;
            }
        }

        public async Task<IEnumerable<Post>> GetAllPost(Guid StudentId)
        {
            var query = "SELECT * FROM Post Where Student_Id = @Student_Id";

            using (var connection = _context.CreateConnection())
            {
                var posts = await connection.QueryAsync<Post>(query, new { Student_Id = StudentId });
                return posts.ToList();
            }
        }

        public async Task<Post> GetPostById(Guid PostId, Guid StudentId)
        {
            var query = "SELECT * FROM Post Where Student_Id = @StudentId AND Id = @PostId";
            
            var parameters = new DynamicParameters();
            parameters.Add("PostId", PostId, DbType.Guid);
            parameters.Add("StudentId", StudentId, DbType.Guid);

            using (var connection = _context.CreateConnection())
            {
                var post = await connection.QuerySingleOrDefaultAsync<Post>(query, parameters);
                return post;
            }
        }

        public async Task<Post> UpdatePost(Post postToUpdate, Guid studentId)
        {
            const string query = "UPDATE Post SET Message = @Message WHERE Student_Id = @StudentID AND Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", postToUpdate.Id, DbType.Guid);
            parameters.Add("Message", postToUpdate.Message, DbType.String);
            parameters.Add("StudentID", studentId, DbType.Guid);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                return postToUpdate;
            }
        }
    }
}