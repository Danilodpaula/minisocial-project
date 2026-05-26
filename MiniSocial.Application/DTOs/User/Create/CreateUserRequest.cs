namespace MiniSocial.Application.DTOs;

public class CreateUserRequest
{
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required  string Email { get; set; }
}