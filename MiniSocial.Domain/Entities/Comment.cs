namespace MiniSocial.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Post? Post { get; set; }
    public User? User { get; set; }
}