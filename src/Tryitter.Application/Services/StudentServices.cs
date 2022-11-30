using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Task<bool> DeleteStudent(Guid StudentId)
        {
            return _studentRepository.DeleteStudent(StudentId);
        }

        public Task<Student> GetStudent(Guid Id)
        {
            return _studentRepository.GetStudentById(Id);
        }

        public async Task<string> Login(string Email, string Password)
        {
            // pegar no banco
            var studentFound = await _studentRepository.GetStudentByEmail(Email);
            // verifica a senha
            if (studentFound.Password != Password)
            {
                return "usuario nao cadastrado";
            }
            // retornar o token
            return _tokenGenerator.GetToken(studentFound);
        }

        public async Task<bool> Register(Student ToCreate)
        {
            var hasEmail = await _studentRepository.GetStudentByEmail(ToCreate.Email);
            
            if (hasEmail is not null)
            {
                return false;
            }
            
            return await _studentRepository.CreateStudent(ToCreate);
        }

        public Task<Student> UpdateStudent(Student ToUpdate)
        {
            return _studentRepository.UpdateStudent(ToUpdate);
        }
    }
}
