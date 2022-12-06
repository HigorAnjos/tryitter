using Dapper;
using System.Data;
using Tryitter.Domain.Entity;
using Tryitter.Domain.Repository;
using User.Infra.Context;

namespace Tryitter.Infra.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DapperContext _context;
        public StudentRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateStudent(Student Student)
        {
            var query = "INSERT INTO Student (Name, Email, Module, Status, Password) VALUES (@Name, @Email, @Module, @Status, @Password)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", Student.Name, DbType.String);
            parameters.Add("Email", Student.Email, DbType.String);
            parameters.Add("Module", Student.Module, DbType.String);
            parameters.Add("Status", Student.Status, DbType.String);
            parameters.Add("Password", Student.Password, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                return true;
            }
        }

        public async Task<bool> DeleteStudent(Guid Id)
        {
            var allPostsQuery = "SELECT * FROM Post WHERE Student_Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var allPosts = await connection.QueryAsync<Post>(allPostsQuery, new { Id });
                foreach (var post in allPosts)
                {
                    var queryDeletePost = "DELETE FROM Post WHERE id = @id;";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", post.Id, DbType.Guid);

                    await connection.ExecuteAsync(queryDeletePost, parameters);
                }

                var queryDelteStudent = "DELETE FROM Student WHERE id = @id;";
                await connection.ExecuteAsync(queryDelteStudent, new { Id });

                return true;
            }
        }

        public async Task<Student> GetStudentByEmail(string Email)
        {
            var query = "SELECT * FROM Student WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                var studentFound = await connection.QuerySingleOrDefaultAsync<Student>(query, new { Email });
                return studentFound;
            }
        }

        public async Task<Student> GetStudentById(Guid Id)
        {
            var query = "SELECT * FROM Student WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var studentFound = await connection.QuerySingleOrDefaultAsync<Student>(query, new { Id });
                return studentFound;
            }
        }

        public async Task<Student> UpdateStudent(Student Student)
        {
            var query = "UPDATE Student SET Name = @Name, Email = @Email, Module = @Module, Status = @Status, Password = @Password WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", Student.Id, DbType.Guid);
            parameters.Add("Name", Student.Name, DbType.String);
            parameters.Add("Email", Student.Email, DbType.String);
            parameters.Add("Module", Student.Module, DbType.String);
            parameters.Add("Status", Student.Status, DbType.String);
            parameters.Add("Password", Student.Password, DbType.String);


            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                return Student;
            }
        }
    }
}