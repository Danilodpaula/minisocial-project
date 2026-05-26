using MiniSocial.Domain.Enums;

namespace MiniSocial.Domain.Entities;

public class Reaction
{
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public ReactionType Type { get; set; } 
        public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
}