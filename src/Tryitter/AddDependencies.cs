using Tryitter.Application.Interfaces;
using Tryitter.Application.Services;
using Tryitter.Application.Services.Auth;
using Tryitter.Domain.Repository;
using Tryitter.Infra.Repository;
using User.Infra.Context;

namespace Tryitter.WebApi
{
    public static class AddDependencies
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPostServices, PostServices>();
            services.AddSingleton<DapperContext>();
            services.AddSingleton<IPostRepository, PostRepository>();
            services.AddSingleton<ITokenGenerator, TokenGenerate>();
            services.AddSingleton<IStudentServices, StudentServices>();
            services.AddSingleton<IStudentRepository, StudentRepository>();
        }
    }
}