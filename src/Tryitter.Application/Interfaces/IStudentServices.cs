using Tryitter.Domain.Entity;

namespace Tryitter.Application.Interfaces
{
    public interface IStudentServices
    {
        public Task<string> Login(string Email, string Password);
        public Task<bool> Register(Student ToCreate);
        public Task<Student> GetStudent(Guid Id);
        public Task<Student> UpdateStudent(Student ToUpdate);
        public Task<bool> DeleteStudent(Guid Id);
    }
}
