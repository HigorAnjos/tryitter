namespace Tryitter.WebApi.Requests;

public record StudentRequest(string Name, string Email, string Module, string Status, string Password);