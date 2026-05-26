namespace MiniSocial.Application.DTOs;

public class ReactionResponse
{
    public required int Id { get; set; }
    public required int PostId { get; set; }
    public required int UserId { get; set; }
    public required string Type { get; set; } = null!;
    public required DateTime CreatedAt { get; set; }
}