using System.Net;
using System.Net.Http.Headers;
using System.Text;
using AutoBogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Tryitter.Application.Interfaces;
using Tryitter.Application.Services.Auth;
using Tryitter.Domain.Entity;

namespace Integrations.Test;

public class PostControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IPostServices> _postServices;

    public PostControllerTest(WebApplicationFactory<Program> factory)
    {
        _postServices = new Mock<IPostServices>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IPostServices>(ps => _postServices.Object);
            });
        }).CreateClient();
    }

    [Theory(DisplayName = "É possível cadastrar um post com sucesso")]
    [InlineData("/post")]
    public async Task PostEndpointReturnSuccess(string url)
    {
        // cria o student
        var student = AutoFaker.Generate<Student>();
        // gera o token
        var token = new TokenGenerate().GetToken(student);
        // insere o token no headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var post = new Post("Novo Post 2022");
        _postServices.Setup(service => service.CreatePost(It.IsAny<Post>(), It.IsAny<Guid>())).ReturnsAsync(post);

        var postJson = JsonConvert.SerializeObject(post);
        var postRequest = new StringContent(postJson, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(url, postRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory(DisplayName = "É possível buscar todos os posts de um determinado estudante")]
    [InlineData("/post")]
    public async Task GetAllPost(string url)
    {
        // cria o student
        var student = AutoFaker.Generate<Student>();
        // gera o token
        var token = new TokenGenerate().GetToken(student);
        // insere o token no headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //cria o post
        var post = AutoFaker.Generate<Post>();
        var postList = new List<Post> { post };
        _postServices.Setup(set => set.GetAllPost(It.IsAny<Guid>())).ReturnsAsync(postList);

        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory(DisplayName = "É possível buscar um post por meio do seu id")]
    [InlineData("/post")]
    public async Task GetOnePost(string url)
    {
        // cria o student
        var student = AutoFaker.Generate<Student>();
        // gera o token
        var token = new TokenGenerate().GetToken(student);
        // insere o token no headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //cria o post
        var post = AutoFaker.Generate<Post>();
        _postServices.Setup(set => set.GetPostById(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(post);

        var response = await _client.GetAsync($"{url}/{post.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory(DisplayName = "É possível editar um post por meio do seu id")]
    [InlineData("/post")]
    public async Task PutUpdate(string url)
    {
        // cria o student
        var student = AutoFaker.Generate<Student>();
        // gera o token
        var token = new TokenGenerate().GetToken(student);
        // insere o token no headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //cria o post
        var post = new Post("Novo Post 2022");
        _postServices.Setup(set => set.UpdatePost(It.IsAny<Post>(), It.IsAny<Guid>())).ReturnsAsync(post);

        var postJson = JsonConvert.SerializeObject(post);
        var requestContent = new StringContent(postJson, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(url, requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory(DisplayName = "É possível deletar um post por meio do seu id")]
    [InlineData("/post")]
    public async Task DeletePost(string url)
    {
        // cria o student
        var student = AutoFaker.Generate<Student>();
        // gera o token
        var token = new TokenGenerate().GetToken(student);
        // insere o token no headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //cria o post
        var post = AutoFaker.Generate<Post>();
        _postServices.Setup(set => set.DeletePost(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

        var response = await _client.DeleteAsync($"{url}/{post.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}