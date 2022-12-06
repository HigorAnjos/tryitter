using Microsoft.AspNetCore.Http;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;
using Tryitter.Domain.Repository;

namespace Tryitter.Application.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IStudentRepository _studentRepository;

        public StudentServices(ITokenGenerator tokenGenerator, IStudentRepository studentRepository)
        {
            _tokenGenerator = tokenGenerator;
            _studentRepository = studentRepository;
        }

        public async Task<bool> Register(Student student)
        {
            var hasEmail = await _studentRepository.GetStudentByEmail(student.Email);

            if (hasEmail is not null)
            {
                throw new BadHttpRequestException("Email ja cadastrado!");
            }

            return await _studentRepository.CreateStudent(student);
        }

        public async Task<string> Login(string email, string password)
        {
            var studentFound = await _studentRepository.GetStudentByEmail(email);

            if (studentFound == null || studentFound.Password != password)
            {
                throw new BadHttpRequestException("E-mail e/ou senha inválidos!");
            }

            return _tokenGenerator.GetToken(studentFound);
        }

        public async Task<Student> GetStudent(Guid studentId)
        {
            var studentFound = await _studentRepository.GetStudentById(studentId);

            if (studentFound is null)
            {
                throw new BadHttpRequestException("Usuário não encontrado!");
            }

            return studentFound;
        }
        
        public async Task<Student> UpdateStudent(Student student)
        {
            var studentFound = await _studentRepository.GetStudentById(student.Id);

            if (studentFound is null)
            {
                throw new BadHttpRequestException("Usuário não encontrado!");
            }

            return await _studentRepository.UpdateStudent(student);
        }

        public async Task<bool> DeleteStudent(Guid studentId)
        {
            var studentFound = await _studentRepository.GetStudentById(studentId);

            if (studentFound is null)
            {
                throw new BadHttpRequestException("Usuário não encontrado!");
            }

            return await _studentRepository.DeleteStudent(studentId);
        }
    }
}