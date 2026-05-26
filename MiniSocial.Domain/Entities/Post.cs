namespace MiniSocial.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /*
    public required User User { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Reaction> Reactions { get; set; } = new List<Reaction>();
    */
}