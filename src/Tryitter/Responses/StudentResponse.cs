namespace Tryitter.WebApi.Responses;

public record StudentResponse(Guid Id, string Name, string Email, string Module, string Status);