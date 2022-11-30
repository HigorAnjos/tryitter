using Tryitter.Domain.Entity;

namespace Tryitter.Domain.Repository
{
    public interface IStudentRepository
    {
        public Task<Student> GetStudentByEmail(string Email);
        public Task<Student> GetStudentById(Guid Id);
        public Task<Student> UpdateStudent(Student Student);
        public Task<bool> DeleteStudent(Guid StudentId);
        public Task<bool> CreateStudent(Student Student);
    }
}
