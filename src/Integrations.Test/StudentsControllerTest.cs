using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;
using System.Net;
using FluentAssertions.Common;
using User.Infra.Context;
using Microsoft.Extensions.DependencyInjection;
using Tryitter.Domain.Entity;
using Newtonsoft.Json;
using System.Reflection;
using Moq;
using Tryitter.Application.Interfaces;
using System.Net.Http.Json;
using Tryitter.WebApi.ViewModels;
using AutoBogus;
using System.Net.Http.Headers;
using System.Net.Http;
using Tryitter.Application.Services.Auth;

namespace Integrations.Test
{
    public class StudentsControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IStudentServices> _studentService;
        //private readonly ITokenGenerator _tokenGenerator;

        public StudentsControllerTest(WebApplicationFactory<Program> factory)
        {
            _studentService = new Mock<IStudentServices>();
            //_tokenGenerator = new TokenGenerate();

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IStudentServices>(st => _studentService.Object);
                    //services.AddScoped<ITokenGenerator, TokenGenerate>();
                });
            }).CreateClient();
        }

        [Theory(DisplayName = "E possivel cadastrar alguem com sucesso")]
        [InlineData("/Student")]
        public async Task PostEndpointsReturnSuccess(string url)
        {
            _studentService.Setup(set => set.Register(It.IsAny<Student>())).ReturnsAsync(true);
            
            var student = new Student()
            {
                Name = "Student",
                Email = "student@gmail.com",
                Module = "CS",
                Status = "Testando endpoint",
                Password = "1234"
            };

            var studentJson = JsonConvert.SerializeObject(student);
            var requestContent = new StringContent(studentJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, requestContent);
            //var content = await response.Content.ReadFromJsonAsync<string>();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory(DisplayName = "E possivel fazer login com sucesso")]
        [InlineData("/Student/login")]
        public async Task PostEndpointsLoginSuccess(string url)
        {
            _studentService.Setup(set => set.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("token");

            var request = new StudentLoginRequest()
            {
                Email = "test@gmail.com",
                Password = "1234"
            };

            var jsonRequestLogin = JsonConvert.SerializeObject(request);
            var response = await _client.PostAsync(url, new StringContent(jsonRequestLogin, Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory(DisplayName = "E possivel buscar dados do studante logado")]
        [InlineData("/student")]
        public async Task GetStudentSuccess(string url)
        {
            var student = AutoFaker.Generate<Student>();
            var token = new TokenGenerate().GetToken(student);

            _studentService.Setup(set => set.GetStudent(It.IsAny<Guid>())).ReturnsAsync(student);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Theory(DisplayName = "E possivel atualizar os dados do studante logado")]
        [InlineData("/student")]
        public async Task PutUpdateSuccess(string url)
        {
            var student = AutoFaker.Generate<Student>();
            var token = new TokenGenerate().GetToken(student);

            _studentService.Setup(set => set.UpdateStudent(It.IsAny<Student>())).ReturnsAsync(student);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var studentJson = JsonConvert.SerializeObject(student);
            var requestContent = new StringContent(studentJson, Encoding.UTF8, "application/json");
            
            var response = await _client.PutAsync(url, requestContent);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Theory(DisplayName = "E possivel deletar a conta do estudante passado por parametro")]
        [InlineData("/student")]
        public async Task DeleteSuccess(string url)
        {
            var student = AutoFaker.Generate<Student>();
            
            var token = new TokenGenerate().GetToken(student);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            _studentService.Setup(set => set.DeleteStudent(It.IsAny<Guid>())).ReturnsAsync(true);

            var deleteResponse = await _client.DeleteAsync($"{url}/{student.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}