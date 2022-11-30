using Tryitter.Domain.Entity;

namespace Tryitter.Application.Interfaces
{
    public interface ITokenGenerator
    {
        public string GetToken(Student Student);
    }
}
