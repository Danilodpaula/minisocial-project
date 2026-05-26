namespace MiniSocial.Application.DTOs;

public class CreateCommentRequest
{
    public required int PostId { get; set; }
    public required int UserId { get; set; }
    public required string Content { get; set; }
}