using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Tryitter.Application.Interfaces;
using Tryitter.Application.Services.Auth;
using Tryitter.Domain.Entity;
using Tryitter.WebApi.Requests;
using Tryitter.WebApi.Responses;

namespace Integrations.Test
{
    public class AdminControllerTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IStudentServices> _studentService;
        private readonly Mock<IPostServices> _postServices;
        public AdminControllerTest(WebApplicationFactory<Program> factory)
        {
            _studentService = new Mock<IStudentServices>();
            _postServices = new Mock<IPostServices>();

            _client = factory.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => {
                    services.AddScoped<IStudentServices>(st => _studentService.Object);
                    services.AddScoped<IPostServices>(st => _postServices.Object);
                });
            }).CreateClient();
        }

        public static TheoryData<Student[], StudentResponse[]> AdminGetAllStudentsSuccessData = new TheoryData<Student[], StudentResponse[]>
        {

            {
                new Student[] {
                    new Student(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin"),
                    new Student(new Guid("46F7593C-3060-4EC6-A19F-0C0119EF02D5"), "Jorge", "jorge@gmail.com", "cs", "concluído", "12345", "Student"),
                    new Student(new Guid("AF7016FA-6B3F-4933-8228-CFF7DF82272B"), "Thales", "thales@gmail.com", "cs", "concluído", "12345", "Student")
                },
                new StudentResponse[] {
                    new StudentResponse(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "Admin"),
                    new StudentResponse(new Guid("46F7593C-3060-4EC6-A19F-0C0119EF02D5"), "Jorge", "jorge@gmail.com", "cs", "concluído", "Student"),
                    new StudentResponse(new Guid("AF7016FA-6B3F-4933-8228-CFF7DF82272B"), "Thales", "thales@gmail.com", "cs", "concluído", "Student")
                }
            }
        };

        [Theory]
        [MemberData(nameof(AdminGetAllStudentsSuccessData))]
        public async Task AdminGetAllStudentsSuccess(Student[] studentServicesResponse, StudentResponse[] expectedResponse)
        {
            //Arrange
            var token = new TokenGenerate().GetToken(new Student(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _studentService.Setup(x => x.GetAllStudents()).ReturnsAsync(studentServicesResponse);

            //Act
            var response = await _client.GetAsync("/Admin/students");
            var content = await response.Content.ReadFromJsonAsync<Student[]>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().BeEquivalentTo(expectedResponse);
        }

        public static TheoryData<Student, StudentResponse> AdminGetStudentByIdData = new TheoryData<Student, StudentResponse>
        {

            {
                new Student(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin"),
                new StudentResponse(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "Admin")
            }
        };

        [Theory]
        [MemberData(nameof(AdminGetStudentByIdData))]
        public async Task AdminGetStudentById(Student studentServicesResponse, StudentResponse expectedResponse)
        {
            //Arrange
            var token = new TokenGenerate().GetToken(new Student(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _studentService.Setup(x => x.GetStudent(It.IsAny<Guid>())).ReturnsAsync(studentServicesResponse);

            //Act
            var response = await _client.GetAsync("/Admin/C39C4B39-F378-4925-A6B9-4AC42DE24415");
            var content = await response.Content.ReadFromJsonAsync<StudentResponse>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Be(expectedResponse);
        }

        public static TheoryData<StudentRequest, StudentResponse> AdminUpdateStudentByIdData = new TheoryData<StudentRequest, StudentResponse>
        {

            {
                new StudentRequest("Higor updated", "higor@gmail.com", "cs", "concluído", "12345", "Admin"),
                new StudentResponse(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor updated", "higor@gmail.com", "cs", "concluído", "Admin")
            }
        };

        [Theory]
        [MemberData(nameof(AdminUpdateStudentByIdData))]
        public async Task AdminUpdateStudentById(StudentRequest studentRequest, StudentResponse expectedResponse)
        {
            //Arrange
            var token = new TokenGenerate().GetToken(new Student(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var serializeStudentRequest = JsonConvert.SerializeObject(studentRequest);
            var serializedRequest = new StringContent(serializeStudentRequest, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PutAsync("/Admin/C39C4B39-F378-4925-A6B9-4AC42DE24415", serializedRequest);
            var content = await response.Content.ReadFromJsonAsync<StudentResponse>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Be(expectedResponse);
        }

        [Theory]
        [InlineData("C39C4B39-F378-4925-A6B9-4AC42DE24415", HttpStatusCode.NoContent)]
        public async Task AdminDeleteStudentById(string id, HttpStatusCode expectedResponseStatus)
        {
            //Arrange
            var student = new Student(new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin");
            _studentService.Setup(x => x.GetStudent(It.IsAny<Guid>())).ReturnsAsync(student);

            var token = new TokenGenerate().GetToken(student);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = await _client.DeleteAsync("/Admin/C39C4B39-F378-4925-A6B9-4AC42DE24415");

            //Assert
            response.StatusCode.Should().Be(expectedResponseStatus);
            _studentService.Verify(x => x.GetStudent(It.IsAny<Guid>()), Times.Once);
        }

        public static TheoryData<Guid, string, IEnumerable<Post>> AdminGetAllPostsByIdData = new TheoryData<Guid, string, IEnumerable<Post>>
        {
            {
                new Guid("C39C4B39-F378-4925-A6B9-4AC42DE24415"),
                "mensagem 1",
                new Post[] {
                    new Post("mensagem 1"),
                    new Post("mensagem 2"),
                    new Post("mensagem 3"),
                }
            }
        };

        [Theory]
        [MemberData(nameof(AdminGetAllPostsByIdData))]
        public async Task AdminGetAllPostsById(Guid studentId, string firstMessage, Post[] expectedResponse)
        {
            //Arrange
            var student = new Student(studentId, "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin");
            var token = new TokenGenerate().GetToken(student);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _postServices.Setup(x => x.GetAllPost(It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var response = await _client.GetAsync("/Admin/C39C4B39-F378-4925-A6B9-4AC42DE24415/posts");
            var content = await response.Content.ReadFromJsonAsync<Post[]>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content[0].Message.Should().Be(firstMessage);
        }


        [Theory]
        [InlineData("C39C4B39-F378-4925-A6B9-4AC42DE24415", HttpStatusCode.NoContent)]
        public async Task AdminDeletePost(string studentId, HttpStatusCode expectedResponseStatus)
        {
            //Arrange
            var student = new Student(new Guid(studentId), "Higor", "higor@gmail.com", "cs", "concluído", "12345", "Admin");
            var token = new TokenGenerate().GetToken(student);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _postServices.Setup(x => x.DeletePost(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var postId = Guid.NewGuid();

            //Act
            var response = await _client.DeleteAsync($"/Admin/{studentId}/posts/{postId}");

            //Assert
            response.StatusCode.Should().Be(expectedResponseStatus);
            _postServices.Verify(x => x.DeletePost(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
