using MiniSocial.Domain.Enums;

namespace MiniSocial.Application.DTOs;

public class CreateReactionRequest
{
    public required int PostId { get; set; }
    public required int UserId { get; set; }
    public ReactionType Type { get; set; }
}