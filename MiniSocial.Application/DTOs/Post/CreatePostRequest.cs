namespace MiniSocial.Application.DTOs;

public class CreatePostRequest
{
    public required int UserId { get; set; }
    public required string Content { get; set; }
}