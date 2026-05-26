namespace MiniSocial.Application.DTOs;

public class PostResponse
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required string Content { get; set; }
}