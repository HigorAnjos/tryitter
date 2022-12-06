using AutoBogus;
using FluentAssertions;
using Moq;
using Tryitter.Application.Interfaces;
using Tryitter.Application.Services;
using Tryitter.Domain.Entity;
using Tryitter.Domain.Repository;

namespace Unit.Test;

public class StudentsServiceTest
{
    private readonly Mock<ITokenGenerator> _tokenGenerator;

    public StudentsServiceTest()
    {
        _tokenGenerator = new Mock<ITokenGenerator>();
        _tokenGenerator.Setup(x => x.GetToken(It.IsAny<Student>())).Returns("tokenFake");
    }

    [Fact(DisplayName = "É possível cadastrar um novo estudante com sucesso")]
    public async Task RegisterTest()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();
        
        repositoryMock.Setup(x => x.CreateStudent(It.IsAny<Student>())).ReturnsAsync(true);
        
        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        var result = await studentService.Register(student);
        
        result.Should().Be(true);
    }

    [Fact(DisplayName = "É retornado uma exceção quando o e-mail já está cadastrado")]
    public Task RegisterTestFail()
    {
        var student = AutoFaker.Generate<Student>();

        var repositoryMock = new Mock<IStudentRepository>();
        
        repositoryMock.Setup(x => x.GetStudentByEmail(student.Email)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);

        Func<Task> act = async () => await studentService.Register(student);

        act.Should().ThrowAsync<Exception>().Result.WithMessage("Email ja cadastrado!");
        
        return Task.CompletedTask;
    }

    [Fact(DisplayName = "É realizado login quando informado e-mail e senha correto")]
    public Task LoginSuccess()
    {
        var student = AutoFaker.Generate<Student>();

        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentByEmail(student.Email)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);

        Func<Task> act = async () => await studentService.Login(student.Email, student.Password);

        act.Should().NotThrowAsync<Exception>();

        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É retornado uma exceção ao realizar o login com e-mail e/ou senha inválidos")]
    public Task LoginFail()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentByEmail(student.Email)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);

        Func<Task> act = async () => await studentService.Login(student.Email, "123");

        act.Should().ThrowAsync<Exception>().Result.WithMessage("E-mail e/ou senha inválidos!");

        return Task.CompletedTask;
    }

    [Fact(DisplayName = "É retornado um estudante quando informado um id correto")]
    public Task GetStudentByIdSuccess()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentById(student.Id)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        Func<Task> act = async () => await studentService.GetStudent(student.Id);
        
        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É retornado uma exceção quando informado um id incorreto")]
    public Task GetStudentByIdFail()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentById(student.Id)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        Func<Task> act = async () => await studentService.GetStudent(new Guid());
        
        act.Should().ThrowAsync<Exception>().Result.WithMessage("Usuário não encontrado!");
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É deletado um estudante quando informado um id correto")]
    public Task DeleteStudentSuccess()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentById(student.Id)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        Func<Task> act = async () => await studentService.DeleteStudent(student.Id);
        
        act.Should().NotThrowAsync<Exception>();
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É retornado uma exeção quando informado um id incorreto")]
    public Task DeleteStudentFail()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentById(student.Id)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        Func<Task> act = async () => await studentService.DeleteStudent(new Guid());
        
        act.Should().ThrowAsync<Exception>().Result.WithMessage("Usuário não encontrado!");
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É possível atualizar um estudante")]
    public Task UpdateStudentSuccess()
    {
        var student = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentById(student.Id)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        Func<Task> act = async () => await studentService.UpdateStudent(student);
        
        act.Should().NotThrowAsync();
        
        return Task.CompletedTask;
    }
    
    [Fact(DisplayName = "É retornado uma exceção quando passado um id incorreto para atualização do estudante")]
    public Task UpdateStudentFail()
    {
        var student = AutoFaker.Generate<Student>();
        
        var student2 = AutoFaker.Generate<Student>();
        
        var repositoryMock = new Mock<IStudentRepository>();

        repositoryMock.Setup(x => x.GetStudentById(student.Id)).ReturnsAsync(student);

        var studentService = new StudentServices(_tokenGenerator.Object, repositoryMock.Object);
        
        Func<Task> act = async () => await studentService.UpdateStudent(student2);
        
        act.Should().ThrowAsync<Exception>().Result.WithMessage("Usuário não encontrado!");
        
        return Task.CompletedTask;
    }
}