using AutoBogus;
using FluentAssertions;
using Moq;
using Tryitter.Application.Services;
using Tryitter.Domain.Entity;
using Tryitter.Domain.Repository;

namespace Unit.Test;

public class PostServiceTest
{
    [Fact(DisplayName = "É possível criar um novo post com sucesso")]
    public Task CreatePostSuccess()
    {
        var post = AutoFaker.Generate<Post>();
        
        var repositoryMock = new Mock<IPostRepository>();
        
        repositoryMock.Setup(x => x.CreatePost(post, It.IsAny<Guid>())).ReturnsAsync(post);
        
        var postService = new PostServices(repositoryMock.Object);
        
        Func<Task> act = async () => await postService.CreatePost(post, It.IsAny<Guid>());

        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }

    [Fact(DisplayName = "É possível buscar todos os post de um estudante com sucesso")]
    public Task GetAllPostsSuccess()
    {
        var post = AutoFaker.Generate<Post>();
        
        var repositoryMock = new Mock<IPostRepository>();

        var postList = new List<Post> { post };
        
        repositoryMock.Setup(x => x.GetAllPost(It.IsAny<Guid>())).ReturnsAsync(postList);
        
        var postService = new PostServices(repositoryMock.Object);
        
        Func<Task> act = async () => await postService.GetAllPost(It.IsAny<Guid>());

        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É possível buscar um post por meio do id com sucesso")]
    public Task GetPostByIdSuccess()
    {
        var post = AutoFaker.Generate<Post>();
        
        var repositoryMock = new Mock<IPostRepository>();

        repositoryMock.Setup(x => x.GetPostById(post.Id, It.IsAny<Guid>())).ReturnsAsync(post);
        
        var postService = new PostServices(repositoryMock.Object);
        
        Func<Task> act = async () => await postService.GetPostById(post.Id, It.IsAny<Guid>());

        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É possível atualizar um post com sucesso")]
    public Task UpdatePostSuccess()
    {
        var post = AutoFaker.Generate<Post>();
        
        var repositoryMock = new Mock<IPostRepository>();

        repositoryMock.Setup(x => x.UpdatePost(post, It.IsAny<Guid>())).ReturnsAsync(post);
        
        var postService = new PostServices(repositoryMock.Object);
        
        Func<Task> act = async () => await postService.UpdatePost(post, It.IsAny<Guid>());

        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É possível deletar um post com sucesso")]
    public Task DeletePostSuccess()
    {
        var post = AutoFaker.Generate<Post>();
        
        var repositoryMock = new Mock<IPostRepository>();

        repositoryMock.Setup(x => x.DeletePost(post.Id, It.IsAny<Guid>())).ReturnsAsync(true);
        
        var postService = new PostServices(repositoryMock.Object);
        
        Func<Task> act = async () => await postService.DeletePost(post.Id, It.IsAny<Guid>());

        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }
}